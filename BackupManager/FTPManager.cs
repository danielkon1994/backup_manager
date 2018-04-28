using BackupManager.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BackupManager
{
    public class FTPManager
    {
        private string serwerAddress { get; set; }
        private int serwerAddressPort { get; set; } = 21;
        private string serwerLogin { get; set; }
        private string serwerPassword { get; set; }

        public FTPManager()
        {
            serwerAddress = ConfigurationManager.AppSettings["FtpAddress"];
            int serverPort;
            if(Int32.TryParse(ConfigurationManager.AppSettings["FtpPort"], out serverPort))
            {
                serwerAddressPort = serverPort;
            }
            serwerLogin = ConfigurationManager.AppSettings["FtpLogin"];
            serwerPassword = ConfigurationManager.AppSettings["FtpPass"];
        }

        public FTPManager(string ftpAddress, string ftpLogin, string ftpPassword, int ftpPort = 21)
        {
            serwerAddress = ftpAddress;
            serwerAddressPort = ftpPort;
            serwerLogin = ftpLogin;
            serwerPassword = ftpPassword;
        }

        public void OnDeleteFiles(object o, CustomEventArgs a)
        {
            if (a.Files.Any())
                DeleteOldFiles(a.Files);
        }

        public void OnUploadFile(object o, CustomEventArgs a)
        {
            if (a.Files.Any())
                UploadFile(a.Path, a.Files.FirstOrDefault());
        }

        public string[] GetOldFiles(string folderPath)
        {
            List<string> listFiles = new List<string>();

            try
            {
                FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create($@"ftp://{serwerAddress}:{serwerAddressPort}/{folderPath}");
                ftpWebRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                ftpWebRequest.Credentials = new NetworkCredential(serwerLogin, serwerPassword);

                using (FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(ftpWebResponse.GetResponseStream()))
                    {
                        string responseString = sr.ReadToEnd();
                        var results = responseString.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        if(results.Any())
                        {
                            results.ForEach(i =>
                            {
                                i = $@"{folderPath}/{i}";
                            });
                        }
                        listFiles.AddRange(results);
                    }
                }
            }
            catch (Exception ex)
            {
                LogInfo.LogErrorWrite(ex);
            }

            return listFiles.ToArray();
        }

        public void UploadFile(string ftpFolderPath, string localFilePath)
        {
            try
            {
                string fileName = Path.GetFileName(localFilePath);
                if (!fileName.Contains(".bak"))
                    throw new Exception($@"Nazwa pliku {fileName} jest nieprawidłowa dla ścieżki {localFilePath}");

                FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create($@"ftp://{serwerAddress}:{serwerAddressPort}/{ftpFolderPath}/{fileName}");
                ftpWebRequest.Method = WebRequestMethods.Ftp.UploadFile;
                ftpWebRequest.Credentials = new NetworkCredential(serwerLogin, serwerPassword);
                ftpWebRequest.KeepAlive = true;
                ftpWebRequest.UseBinary = true;

                byte[] fileContents = new byte[] { };
                using (StreamReader sourceStream = new StreamReader(localFilePath))
                { 
                    fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                }
                ftpWebRequest.ContentLength = fileContents.Length;

                using (Stream requestStream = ftpWebRequest.GetRequestStream())
                {
                    requestStream.Write(fileContents, 0, fileContents.Length);
                }

                FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
                ftpWebResponse.Close();
            }
            catch (Exception ex)
            {
                LogInfo.LogErrorWrite(ex);
            }
        }

        public void DeleteOldFiles(string[] oldFiles)
        {
            try
            {
                foreach(string file in oldFiles)
                {
                    DeleteFile(file);
                }
            }
            catch (Exception ex)
            {
                LogInfo.LogErrorWrite(ex);
            }
        }

        public void DeleteFile(string filePath)
        {
            try
            {
                FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create($@"ftp://{serwerAddress}:{serwerAddressPort}/{filePath}");
                ftpWebRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                ftpWebRequest.Credentials = new NetworkCredential(serwerLogin, serwerPassword);

                using (FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(ftpWebResponse.GetResponseStream()))
                    {
                        sr.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                LogInfo.LogErrorWrite(ex);
            }
        }
    }
}
