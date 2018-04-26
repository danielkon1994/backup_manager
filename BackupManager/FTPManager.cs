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

        public string[] GetOldFilesFromFtp(string folderPath)
        {
            List<string> listFiles = new List<string>();

            try
            {
                FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create($@"ftp://{serwerAddress}:{serwerAddressPort}/");
                ftpWebRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                ftpWebRequest.Credentials = new NetworkCredential(serwerLogin, serwerPassword);

                using (FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(ftpWebResponse.GetResponseStream()))
                    {
                        string responseString = sr.ReadToEnd();
                        string[] results = responseString.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
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
    }
}
