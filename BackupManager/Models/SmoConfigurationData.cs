﻿using Microsoft.SqlServer.Management.Smo;
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
            Files = new string[] { };
        }

        public BackupActionType ActionType { get; set; } = BackupActionType.Database;

        public string DatabaseName { get; set; }

        public bool Incremental { get; set; } = false;

        public string DeviceName { get; set; }
        public DeviceType DeviceType { get; set; }

        public bool Initialize { get; set; } = false;

        public string[] Files { get; set; }
    }
}