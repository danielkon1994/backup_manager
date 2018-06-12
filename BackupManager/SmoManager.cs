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
        public event EventHandler<CustomEventArgs> CreatedBackupFtpManager;

        Server server;
        ServerConnection serverConnection;

        public SmoManager()
        {
            connectWithServer();
        }

        protected virtual void OnCreatedBackupFileManager(List<string> oldFiles)
        {
            if (CreatedBackupFileManager != null)
                CreatedBackupFileManager(this, new CustomEventArgs() { DeleteFiles = oldFiles });
        }
        
        protected virtual void OnCreatedBackupFtpManager(string path, List<string> filesToUpload, List<string> filesToDelete)
        {
            if (CreatedBackupFtpManager != null)
                CreatedBackupFtpManager(this, new CustomEventArgs() { Path = path, UploadFiles = filesToUpload, DeleteFiles = filesToDelete });
        }

        public void CreateBackup(SmoConfigurationData smoConfiguration,
            Form1.BackupStatusDel backupStatusDel = null,
            PercentCompleteEventHandler backup_PercentComplete = null)
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

                LogInfo.LogInfoWrite($"Rozpoczęcie tworzenia backup'u bazy danych {smoConfiguration.DatabaseName}");
                backup.SqlBackup(server);
                LogInfo.LogInfoWrite($"Utworzono backup bazy danych {smoConfiguration.DatabaseName}");

                List<string> filesToDelete = smoConfiguration.LocalFiles.Where(name => !name.Contains(Path.GetFileName(smoConfiguration.DeviceName))).ToList();
                filesToDelete = getLocalFullPathFileList(smoConfiguration.LocalDirectory, filesToDelete);
                OnCreatedBackupFileManager(filesToDelete);
                LogInfo.LogInfoWrite($"Operacje na lokalnych plikach zostały zakończone");

                List<string> filesToFtpUpload = smoConfiguration.LocalFiles;
                filesToFtpUpload.Add(Path.GetFileName(smoConfiguration.DeviceName));
                filesToFtpUpload = filesToFtpUpload.Except(smoConfiguration.FtpFiles).ToList();
                filesToFtpUpload = getLocalFullPathFileList(smoConfiguration.LocalDirectory, filesToFtpUpload);
                
                List<string> filesToFtpDelete = new List<string>();
                if(!smoConfiguration.Incremental)
                {
                    filesToFtpDelete = getFtpFullPathFileList(smoConfiguration.FtpDirectory, smoConfiguration.FtpFiles);
                }

                OnCreatedBackupFtpManager(smoConfiguration.FtpDirectory, filesToFtpUpload, filesToFtpDelete);
                LogInfo.LogInfoWrite($"Operacje na serwerze FTP zostały zakończone");

                backupStatusDel?.Invoke($"Utworzono backup bazy danych {smoConfiguration.DatabaseName}");
                LogInfo.LogInfoWrite($"Zakończono proces tworzenia backup'u bazy danych {smoConfiguration.DatabaseName}");
            }
            catch (Exception ex)
            {
                LogInfo.LogErrorWrite($"Nie można utworzyć backupu bazy danych {smoConfiguration.DatabaseName}", ex);
                backupStatusDel?.Invoke($"Nie można utworzyć backupu bazy danych {smoConfiguration.DatabaseName}");
                return;
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

        private List<string> getLocalFullPathFileList(string localDirectory, List<string> listFiles)
        {
            return listFiles.Select(item => $@"{localDirectory}\{item}").ToList();
        }

        private List<string> getFtpFullPathFileList(string ftpDirectory, List<string> listFiles)
        {
            return listFiles.Select(item => $@"{ftpDirectory}\{item}").ToList();
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
