using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupManager.Events
{
    public class CustomEventArgs : EventArgs
    {
        public CustomEventArgs()
        {
            Files = new string[] { };
        }

        public string[] Files { get; set; }
    }
}
