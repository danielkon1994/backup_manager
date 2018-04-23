using BackupManager.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using static BackupManager.Form2;

namespace BackupManager
{
    public class XmlSerializeManager
    {        
        static string xmlFilePath = ConfigurationManager.AppSettings["ConfigFile"];
        XmlSerializer xs;

        public XmlSerializeManager()
        {
            xs = new XmlSerializer(typeof(BackupSettings), new XmlRootAttribute("BackupConfigurations"));
        }

        // zapisanie do pliku przesłanej konfiguracji
        public void SaveConfiguration(BackupConfigurationData backupConf, CallBackDel callBack)
        {
            BackupSettings settings;

            // jeżeli plik nie istnieje to jest tworzony i zapisywane są do niego dane
            if (!fileHasAnyData())
            {
                try
                {
                    using (var fs = new FileStream(xmlFilePath, FileMode.Open))
                    {
                        settings = new BackupSettings();
                        backupConf.Id = randomIntGuid();
                        settings.ConfigurationList.Add(backupConf);
                        xs.Serialize(fs, settings);
                    }

                    callBack(new CallBackModel { Success = true, Message = "Konfiguracja została zapisana !" });
                }
                catch (Exception ex)
                {
                    LogInfo.LogErrorWrite("Nie udało się zapisać konfiguracji backup'u do pustego pliku", ex);
                    callBack(new CallBackModel { Success = false });
                }                
            }
            else // jeżeli plik istnieje to dane są 'dokładane' na koniec
            {
                try
                {
                    using (var fs = new FileStream(xmlFilePath, FileMode.Open))
                    {
                        settings = (BackupSettings)xs.Deserialize(fs);
                        fs.SetLength(0);
                        backupConf.Id = randomIntGuid();
                        settings.ConfigurationList.Add(backupConf);
                        xs.Serialize(fs, settings);
                    }

                    callBack(new CallBackModel { Success = true, Message = "Konfiguracja została zapisana !" });
                }
                catch(Exception ex)
                {
                    LogInfo.LogErrorWrite("Nie udało się zapisać konfiguracji backup'u do pliku", ex);
                    callBack(new CallBackModel { Success = false });
                }
            }
        }

        // zaaktualizowanie pliku konfiguracyjnego
        public void UpdateConfiguration(string id, BackupConfigurationData backupConf, CallBackDel callBack)
        {
            // pobranie danych z pliku .xml
            var backupSettings = this.ReadFromXmlFile();
            if (backupSettings == null)
                callBack(new CallBackModel { Success = false, Message = "Nie można odczytać pliku zawierającego konfigurację." });
            
            try
            {
                // znalezienie 'odpowiedniej' konfiguracji
                var dataConf = backupSettings.ConfigurationList.Where(d => d.Id == id).FirstOrDefault();

                // zamiana danych na te przesłane z formularza i zapisanie pliku
                // z zaaktualizowaną konfiguracją
                using (var fs = new FileStream(xmlFilePath, FileMode.Open))
                {
                    dataConf.DatabaseName = backupConf.DatabaseName;
                    dataConf.FileName = backupConf.FileName;
                    dataConf.LocalDirectory = backupConf.LocalDirectory;
                    dataConf.FtpDirectory = backupConf.FtpDirectory;
                    dataConf.SendToFtp = backupConf.SendToFtp;
                    dataConf.BackupDays = backupConf.BackupDays;
                    if (backupConf.LastBackupDay != null)
                    {
                        dataConf.LastBackupDay = backupConf.LastBackupDay;
                    }
                    fs.SetLength(0);
                    xs.Serialize(fs, backupSettings);
                }

                callBack(new CallBackModel { Success = true, Message = "Konfiguracja została zapisana." });
            }
            catch(Exception ex)
            {
                LogInfo.LogErrorWrite($"Nie udało się zaaktualizować konfiguracji backup'u w pliku {xmlFilePath}", ex);
                callBack(new CallBackModel { Success = false});
            }
        }

        // usuwanie konfiguracji z pliku
        public void DeleteConfiguration(string id, Form1.CallBackDel callBack)
        {
            // pobranie danych z pliku .xml
            var backupSettings = this.ReadFromXmlFile();
            if (backupSettings == null)
                callBack(new CallBackModel { Success = false, Message = "Nie można odczytać pliku zawierającego konfigurację." });

            try
            {
                // znalezienie 'odpowiedniej' konfiguracji
                var dataConf = backupSettings.ConfigurationList.Where(d => d.Id == id).FirstOrDefault();

                // zapisanie pliku już bez usuniętej konfiguracji
                using (var fs = new FileStream(xmlFilePath, FileMode.Open))
                {
                    backupSettings.ConfigurationList.Remove(dataConf);
                    fs.SetLength(0);
                    xs.Serialize(fs, backupSettings);
                }

                callBack(new CallBackModel { Success = true, Message = "Konfiguracja została usunięta." });
            }
            catch(Exception ex)
            {
                LogInfo.LogErrorWrite($"Nie udało się usunąć konfiguracji backup'u z pliku {xmlFilePath}", ex);
                callBack(new CallBackModel { Success = false });
            }
        }

        // odczytywanie listy ustawień z pliku
        public List<BackupConfigurationData> GetListFromXmlFile()
        {
            List<BackupConfigurationData> listBackupConf = new List<BackupConfigurationData>();

            // pobranie danych z pliku .xml
            var backupSettings = this.ReadFromXmlFile();
            if (backupSettings == null)
                return listBackupConf;

            listBackupConf = backupSettings.ConfigurationList;

            return listBackupConf;
        }

        // odczytywanie ustawień z pliku po id
        public BackupConfigurationData GetConfigurationById(string id)
        {
            // pobranie danych z pliku .xml
            var backupSettings = this.ReadFromXmlFile();
            if (backupSettings == null)
                return null;

            return backupSettings.ConfigurationList.Where(i => i.Id == id).FirstOrDefault();
        }

        // aktualizacja czasu ostatniego backupu
        public void LastBackupDateUpdate(string id, DateTime dateTime)
        {
            // pobranie danych z pliku .xml
            var backupSettings = this.ReadFromXmlFile();
            if (backupSettings == null)
                return;

            try
            {
                // znalezienie 'odpowiedniej' konfiguracji
                var dataConf = backupSettings.ConfigurationList.Where(d => d.Id == id).FirstOrDefault();

                // zamiana danych na te przesłane z formularza i zapisanie pliku
                // z zaaktualizowaną konfiguracją
                using (var fs = new FileStream(xmlFilePath, FileMode.Open))
                {
                    dataConf.LastBackupDay = dateTime;
                    fs.SetLength(0);
                    xs.Serialize(fs, backupSettings);
                }

            }
            catch (Exception ex)
            {
                LogInfo.LogErrorWrite($"Nie udało się zaaktualizować daty ostatniego backup'u w pliku {xmlFilePath}", ex);
            }
        }

        // odczytywanie z pliku wszystkich konfiguracji
        public BackupSettings ReadFromXmlFile()
        {
            BackupSettings backupSettings = null;

            try
            {
                using (var fs = new FileStream(xmlFilePath, FileMode.Open))
                {
                    backupSettings = (BackupSettings)xs.Deserialize(fs);
                }
            }
            catch (Exception ex)
            {
                LogInfo.LogErrorWrite($"Nie udało się odczytać z pliku {xmlFilePath}", ex);
            }

            return backupSettings;
        }

        // sprawdzenie czy plik istnieje i czy nie jest pusty
        private bool fileHasAnyData()
        {
            if (File.Exists(xmlFilePath) && new FileInfo(xmlFilePath).Length > 0)
            {
                return true;
            }

            try
            {
                BackupSettings settings = new BackupSettings();

                using (var fs = new FileStream(xmlFilePath, FileMode.OpenOrCreate))
                {
                    xs.Serialize(fs, settings);
                }
            }
            catch (Exception ex)
            {
                LogInfo.LogErrorWrite($"Nie udało się utworzyć nowego, pustego pliku {xmlFilePath}", ex);
            }

            return false;
        }

        // tworzenie randomowego identyfikatora
        private string randomIntGuid()
        {
            Guid guid = Guid.NewGuid();
            string guidString = guid.ToString();
            return guidString.Replace("-", "").Substring(0, 6);
        }
    }
}
