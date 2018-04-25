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
            if (a.Files.Any())
                DeleteOldFilesFromLocalDirectory(a.Files);
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

        public string[] GetOldFilesFromLocalDirectory(string folderPath)
        {
            try
            {
                return Directory.GetFiles(folderPath).ToArray();
            }
            catch (Exception ex)
            {
                LogInfo.LogErrorWrite(ex);
            }

            return new string[] { };
        }

        public void DeleteOldFilesFromLocalDirectory(string[] oldFiles)
        {
            foreach(string fileName in oldFiles)
            {
                if (File.Exists(fileName))
                    File.Delete(fileName);
            }
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
