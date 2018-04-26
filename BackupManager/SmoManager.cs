using BackupManager.Events;
using BackupManager.Models;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupManager
{
    public class SmoManager
    {
        public delegate void CreatedBackupEventHandler(object o, CustomEventArgs e);
        public event CreatedBackupEventHandler CreatedBackupFileManager;
        public event EventHandler CreatedBackupFtpManager;

        Server server;
        ServerConnection serverConnection;

        public SmoManager()
        {
            connectWithServer();
        }

        protected virtual void OnCreatedBackupFileManager(string[] oldFiles)
        {
            if (CreatedBackupFileManager != null)
                CreatedBackupFileManager(this, new CustomEventArgs() { Files = oldFiles });
        }

        protected virtual void OnCreatedBackupFtpManager()
        {
            if (CreatedBackupFtpManager != null)
                CreatedBackupFtpManager(this, EventArgs.Empty);
        }

        public void CreateBackup(SmoConfigurationData smoConfiguration,
            PercentCompleteEventHandler backup_PercentComplete = null, 
            ServerMessageEventHandler backup_Complete = null,
            ServerMessageEventHandler backup_Information = null)
        {
            Backup backup = new Backup();
            backup.Action = smoConfiguration.ActionType;
            backup.Database = smoConfiguration.DatabaseName;
            backup.Incremental = smoConfiguration.Incremental;
            backup.Devices.AddDevice(smoConfiguration.DeviceName, smoConfiguration.DeviceType);
            backup.Initialize = smoConfiguration.Initialize;
            try { 
                backup.PercentCompleteNotification = 5;
                if (backup_PercentComplete != null)
                    backup.PercentComplete += new PercentCompleteEventHandler(backup_PercentComplete);
                if (backup_Complete != null)
                    backup.Complete += new ServerMessageEventHandler(backup_Complete);
                if (backup_Information != null)
                    backup.Information += new ServerMessageEventHandler(backup_Information);

                backup.SqlBackupAsync(server);

                OnCreatedBackupFileManager(smoConfiguration.Files);

                OnCreatedBackupFtpManager();
            }
            catch (Exception ex)
            {
                LogInfo.LogErrorWrite($"Nie można utworzyć backapu bazy danych {smoConfiguration.DatabaseName}", ex);
            }
        }

        public List<string> GetListDatabases()
        {
            List<string> listDatabases = new List<string>();
            try
            {
                var databaseCollection = server.Databases;
                foreach (var db in databaseCollection)
                {
                    listDatabases.Add(db.ToString());
                }
            }
            catch (Exception ex)
            {
                LogInfo.LogErrorWrite($"Nie można pobrać listy baz danych z serwera", ex);
            }

            return listDatabases;
        }

        // obliczanie ile dni pozostało pełnego backup'u
        public int DaysLeft(int backupDays, DateTime? lastBackupDay)
        {
            double dniZostalo = 0;
            double dni = 0;
            if (lastBackupDay != null)
            {
                TimeSpan roznica = DateTime.Now - (DateTime)lastBackupDay;
                dni = roznica.TotalDays;
                if (dni < 0)
                {
                    dniZostalo = 0;
                }
                else
                {
                    dniZostalo = (double)backupDays - dni;
                }
            }
            return Convert.ToInt32(dniZostalo);
        }

        private void connectWithServer()
        {
            if (server == null)
            {
                try
                {
                    String connectionString = ConfigurationManager.ConnectionStrings["ServerName"].ConnectionString;
                    serverConnection = new ServerConnection(connectionString);
                    if ((ConfigurationManager.AppSettings["SqlWinAuth"] != "" &&
                        ConfigurationManager.AppSettings["SqlWinAuth"] != "false") &&
                        ConfigurationManager.AppSettings["SqlLogin"] != "" &&
                        ConfigurationManager.AppSettings["SqlPass"] != "")
                    {
                        serverConnection.LoginSecure = false;
                        serverConnection.Login = ConfigurationManager.AppSettings["SqlLogin"];
                        serverConnection.Password = ConfigurationManager.AppSettings["SqlPass"];
                    }
                    server = new Server(serverConnection);
                    server.ConnectionContext.StatementTimeout = 18000;
                }
                catch (Exception ex)
                {
                    LogInfo.LogErrorWrite($"Nie udało się połączyć z serwerem", ex);
                }
            }
        }
    }
}
