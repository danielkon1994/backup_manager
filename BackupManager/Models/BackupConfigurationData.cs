using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BackupManager.Models
{
    [XmlType("Configuration")]
    public class BackupConfigurationData
    {
        [XmlAttribute("Id")]
        public string Id { get; set; }

        public string FileName { get; set; }

        public string DatabaseName { get; set; }

        public string LocalDirectory { get; set; }

        public string FtpDirectory { get; set; }

        public bool SendToFtp { get; set; }

        public int BackupDays { get; set; }

        public DateTime? LastBackupDay { get; set; }

        public override string ToString()
        {
            return String.Format(FileName + " " + DatabaseName + " " + LocalDirectory + " " + FtpDirectory + " " + SendToFtp + " " + BackupDays + " " + LastBackupDay);
        }
    }
}
