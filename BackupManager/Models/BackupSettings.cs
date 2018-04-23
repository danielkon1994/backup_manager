using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BackupManager.Models
{
    public class BackupSettings
    {
        public BackupSettings()
        {
            ConfigurationList = new List<BackupConfigurationData>();
        }

        [XmlElement("Configuration")]
        public List<BackupConfigurationData> ConfigurationList { get; set; }
    }
}
