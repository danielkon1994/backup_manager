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
            if (a.DeleteFiles.Any())
                DeleteOldFiles(a.DeleteFiles);
        }

        public void OnUploadFile(object o, CustomEventArgs a)
        {
            if (a.DeleteFiles.Any())
                DeleteOldFiles(a.DeleteFiles);

            if (a.UploadFiles.Any())
            {
                if(a.UploadFiles.Count() == 1)
                    UploadFile(a.Path, a.UploadFiles.FirstOrDefault()).Wait();
                if(a.UploadFiles.Count() > 1)
                    UploadFiles(a.Path, a.UploadFiles);
            }
        }

        public List<string> GetOldFiles(string folderPath)
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
                        //if(results.Any())
                        //{
                        //    results.ForEach(i => {
                        //        listFiles.Add($@"{folderPath}/{i}");
                        //    });
                        //}
                        listFiles.AddRange(results);
                    }
                }
            }
            catch (WebException ex)
            {
                FtpWebResponse ftpWebResponse = (FtpWebResponse)ex.Response;
                if(ftpWebResponse.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    ftpWebResponse.Close();
                    this.CreateDirectory(folderPath);
                }
                else
                { 
                    LogInfo.LogErrorWrite(ex);
                }
            }

            return listFiles;
        }

        public void UploadFiles(string ftpFolderPath, List<string> filesPathArr)
        {
            foreach(string filePath in filesPathArr)
            {
                this.UploadFile(ftpFolderPath, filePath).Wait();
            }
        }

        public async Task UploadFile(string ftpFolderPath, string localFilePath)
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

                using (FileStream fs = new FileStream(localFilePath, FileMode.Open, FileAccess.Read))
                { 
                    using (Stream requestStream = await ftpWebRequest.GetRequestStreamAsync())
                    {
                        byte[] buffer = new byte[8092];
                        int read = 0;
                        while ((read = await fs.ReadAsync(buffer, 0, buffer.Length)) != 0)
                        {
                            await requestStream.WriteAsync(buffer, 0, read);
                        }

                        await requestStream.FlushAsync();
                    }
                }

                FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
                ftpWebResponse.Close();
            }
            catch (Exception ex)
            {
                LogInfo.LogErrorWrite(ex);
            }
        }

        public void DeleteOldFiles(List<string> oldFiles)
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
                
                FtpWebResponse response = (FtpWebResponse)ftpWebRequest.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                LogInfo.LogErrorWrite(ex);
            }
        }

        public void CreateDirectory(string folderPath)
        {
            try
            {
                FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create($@"ftp://{serwerAddress}:{serwerAddressPort}/{folderPath}");
                ftpWebRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                ftpWebRequest.Credentials = new NetworkCredential(serwerLogin, serwerPassword);

                FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
                ftpWebResponse.Close();
            }
            catch (Exception ex)
            {
                LogInfo.LogErrorWrite(ex);
            }
        }
    }
}
