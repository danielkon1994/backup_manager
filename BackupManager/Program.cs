using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackupManager
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        { 
            if (args.Length > 0)
            {
                LogInfo.LogInfoWrite("Aplikacja BackupManager została uruchomiona");



                LogInfo.LogInfoWrite("Aplikacja BackupManager została zamknięta");
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
        }
    }
}
