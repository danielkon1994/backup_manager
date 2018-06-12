using BackupManager.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupManager
{
    public class FilesManager
    {
        public void OnDeleteFiles(object o, CustomEventArgs a)
        {
            if (a.DeleteFiles.Any())
                DeleteOldFilesFromLocalDirectory(a.DeleteFiles);
        }

        public bool CheckIfHeadCopyExist(string folderPath)
        {
            try
            {
                createLocalDirectory(folderPath);

                if (Directory.GetFiles(folderPath, "*_head.bak", SearchOption.AllDirectories).Any())
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                LogInfo.LogErrorWrite(ex);
                return false;
            }
        }

        public List<string> GetOldFilesFromLocalDirectory(string folderPath)
        {
            List<string> filesList = new List<string>();
            try
            {
                //return Directory.GetFiles(folderPath).ToList();
                string[] files = Directory.GetFiles(folderPath);
                foreach (string file in files)
                    filesList.Add(Path.GetFileName(file));
            }
            catch (Exception ex)
            {
                LogInfo.LogErrorWrite(ex);
            }

            return filesList;
        }

        public bool BackupIncremental(int backupDays, DateTime? lastBackupDay, string backupLocalDirectory)
        {
            if (DateTime.Now.Day == 1)
                return false;

            if (this.DaysLeft(backupDays, lastBackupDay) <= 0)
                return false;

            if (!this.CheckIfHeadCopyExist(backupLocalDirectory))
                return false;

            return true;
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

        public void DeleteOldFilesFromLocalDirectory(List<string> oldFiles)
        {
            foreach(string filePath in oldFiles)
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }

        public string GetBackupFileFullPath(bool incrementalCopy, string fileName, string directoryPath)
        {
            string fullPath = string.Empty;
            if (incrementalCopy)
            {
                fullPath = $@"{directoryPath}\{fileName}_{DateTime.Now.Month}_{DateTime.Now.Day}_incr.bak";
            }
            else
            {
                fullPath = $@"{directoryPath}\{fileName}_{DateTime.Now.Month}_{DateTime.Now.Day}_head.bak";
            }

            return fullPath;
        }

        private void createLocalDirectory(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }
    }
}
