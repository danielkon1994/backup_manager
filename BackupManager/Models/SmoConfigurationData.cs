using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupManager.Models
{
    public class SmoConfigurationData
    {
        public SmoConfigurationData()
        {
            LocalFiles = new string[] { };
            FtpFiles = new string[] { };
        }

        public BackupActionType ActionType { get; set; } = BackupActionType.Database;

        public string DatabaseName { get; set; }

        public bool Incremental { get; set; } = false;

        public string DeviceName { get; set; }
        public DeviceType DeviceType { get; set; }

        public bool Initialize { get; set; } = false;

        public string[] LocalFiles { get; set; }

        public string FtpDirectory { get; set; }
        public string[] FtpFiles { get; set; }
    }
}
