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
            UploadFiles = new List<string>();
            DeleteFiles = new List<string>();
        }

        public List<string> UploadFiles { get; set; }

        public List<string> DeleteFiles { get; set; }

        public string Path { get; set; }

        public string Message { get; set; }
    }
}
