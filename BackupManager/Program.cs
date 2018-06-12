using BackupManager.Models;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackupManager
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        { 
            if (args.Length > 0)
            {
                LogInfo.LogInfoWrite("Aplikacja BackupManager została uruchomiona");

                SmoManager smoManager = new SmoManager();
                FilesManager filesManager = new FilesManager();

                XmlSerializeManager xmlSerializeManager = new XmlSerializeManager();
                var listSettings = xmlSerializeManager.GetListFromXmlFile();
                foreach (var backupConfiguration in listSettings)
                {
                    bool backupIsIncremental = filesManager.BackupIncremental(backupConfiguration.BackupDays, backupConfiguration.LastBackupDay, backupConfiguration.LocalDirectory);
                    List<string> localFiles = filesManager.GetOldFilesFromLocalDirectory(backupConfiguration.LocalDirectory);

                    FTPManager ftpManager = new FTPManager();
                    List<string> ftpFiles = new List<string>();
                    if (backupConfiguration.SendToFtp && !string.IsNullOrEmpty(backupConfiguration.FtpDirectory))
                    {
                        ftpFiles = ftpManager.GetOldFiles(backupConfiguration.FtpDirectory);
                    }

                    string backupFileFullPath = filesManager.GetBackupFileFullPath(backupIsIncremental, backupConfiguration.FileName, backupConfiguration.LocalDirectory);

                    SmoConfigurationData smoConfiguration = new SmoConfigurationData()
                    {
                        DatabaseName = backupConfiguration.DatabaseName,
                        Incremental = backupIsIncremental,
                        DeviceName = backupFileFullPath,
                        DeviceType = DeviceType.File,
                        LocalFiles = localFiles,
                        FtpFiles = ftpFiles,
                        LocalDirectory = backupConfiguration.LocalDirectory
                    };

                    if (!backupIsIncremental)
                    {
                        xmlSerializeManager.LastBackupDateUpdate(backupConfiguration.Id, DateTime.Now);
                        smoManager.CreatedBackupFileManager += filesManager.OnDeleteFiles;
                    }

                    if (backupConfiguration.SendToFtp && !string.IsNullOrEmpty(backupConfiguration.FtpDirectory))
                    {
                        smoConfiguration.FtpDirectory = backupConfiguration.FtpDirectory;
                        smoManager.CreatedBackupFtpManager += ftpManager.OnUploadFile;
                    }

                    Task backup = new Task(() => { smoManager.CreateBackup(smoConfiguration); });
                    backup.Start();
                    backup.ContinueWith((t) => {
                        smoManager.CreatedBackupFileManager -= filesManager.OnDeleteFiles;
                        smoManager.CreatedBackupFtpManager -= ftpManager.OnUploadFile;
                    });
                    backup.Wait();
                }

                LogInfo.LogInfoWrite("Aplikacja BackupManager została zamknięta");
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
        }
    }
}
