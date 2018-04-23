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
        Server server;
        ServerConnection serverConnection;
        XmlSerializeManager xmlSerializeManager;

        bool deleteLocalFiles;

        public SmoManager()
        {
            connectWithServer();

            xmlSerializeManager = new XmlSerializeManager();
            deleteLocalFiles = false;
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

        private void connectWithServer()
        {
            if(server == null)
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
                catch(Exception ex)
                {
                    LogInfo.LogErrorWrite($"Nie udało się połączyć z serwerem", ex);
                }
            }
        }

        public void CreateBackup(BackupConfigurationData configurationData, 
            PercentCompleteEventHandler backup_PercentComplete = null, 
            ServerMessageEventHandler backup_Complete = null,
            ServerMessageEventHandler backup_Information = null)
        {
            Backup backup = new Backup();
            backup.Action = BackupActionType.Database;
            backup.Database = configurationData.DatabaseName;

            string backupFileFullPath = getBackupFileFullPath(configurationData.FileName, configurationData.LocalDirectory);
            backup.Incremental = backupIncremental(configurationData.Id, backupFileFullPath);
            backup.Devices.AddDevice(backupFileFullPath, DeviceType.File);

            backup.Initialize = false;
            try { 
                backup.PercentCompleteNotification = 5;
                if (backup_PercentComplete != null)
                    backup.PercentComplete += new PercentCompleteEventHandler(backup_PercentComplete);
                if (backup_Complete != null)
                    backup.Complete += new ServerMessageEventHandler(backup_Complete);
                if (backup_Information != null)
                    backup.Information += new ServerMessageEventHandler(backup_Information);

                backup.SqlBackupAsync(server);
            }
            catch (Exception ex)
            {
                LogInfo.LogErrorWrite($"Nie można utworzyć backapu bazy danych {configurationData.DatabaseName}", ex);
            }
        }

        private bool backupIncremental(string id, string backupFileFullPath)
        {
            if (backupFileFullPath.IndexOf("_head.bak") > 0)
            {
                xmlSerializeManager.LastBackupDateUpdate(id, DateTime.Now);
                return false;
            }

            return true;
        }

        private string getBackupFileFullPath(string fileName, string directoryPath)
        {
            string fullPath = string.Empty;
            if(checkIfHeadCopyFileExist(directoryPath)) { 
                fullPath = $@"{directoryPath}\{fileName}_{DateTime.Now.Month}_{DateTime.Now.Day}_incr.bak";
            }
            else { 
                fullPath = $@"{directoryPath}\{fileName}_{DateTime.Now.Month}_{DateTime.Now.Day}_head.bak";
                deleteLocalFiles = true;
            }

            return fullPath;
        }

        private bool checkIfHeadCopyFileExist(string folderPath)
        {
            try
            {
                createLocalDirectory(folderPath);

                if (Directory.GetFiles(folderPath, "*_head.bak", SearchOption.AllDirectories).Any())
                    return true;

                return false;
            }
            catch(Exception)
            {
                return false;
            }
        }

        private void createLocalDirectory(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        // obliczanie ile dni pozostało pełnego backup'u
        private int daysLeft(int backupDays, DateTime? lastBackupDay)
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
    }
}
