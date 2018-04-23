using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Data.SqlClient;
using System.IO;

namespace Tukano.BackupManager
{
    public partial class Form1 : Form
    {
        private Server server;
        private ServerConnection serverConn;               
        private String connectionString;
        private String databaseName;
        private String backupFolder;
        private String backupFileName;
        private Backup backupDb;

        private BackupSettings settings;
        static String xmlFilePath = Environment.CurrentDirectory + "\\daneConf.xml";
        private ListViewItem listViewItem;

        public Form1()
        {                         
            InitializeComponent();
            //initializeDatabaseData(300);
            loadDataConf();
        }

        public void loadDataConf()
        {
            if (XmlSerializeClass.FileHasAnyData())
            {
                settings = new BackupSettings();
                settings = XmlSerializeClass.ReadFromXmlFile();
                foreach (DaneKonfiguracyjne item in settings.Lista)
                {
                    listViewItem = new ListViewItem(item.Id);
                    listViewItem.SubItems.Add(item.NazwaKopii);
                    listViewItem.SubItems.Add(item.NazwaBazy);
                    if(item.AutoBackup)
                    { 
                        listViewItem.SubItems.Add("Tak");
                    }
                    else
                    {
                        listViewItem.SubItems.Add("Nie");
                    }
                    listViewKonf.Items.Add(listViewItem);                    
                }
            }
        }

        // tworzenie backupu
        private void BtnBackup_Click(object sender, EventArgs e)
        {
            if (listViewKonf.SelectedItems.Count > 0)
            {
                string id = listViewKonf.SelectedItems[0].SubItems[0].Text;
                DaneKonfiguracyjne daneKonf = settings.Lista.Where(s => s.Id == id).FirstOrDefault();
                if (daneKonf != null)
                {
                    connectionString = @"DESKTOP-NPHGI8D\SQLEXPRESS";
                    serverConn = new ServerConnection(connectionString);
                    server = new Server(serverConn);
                    backupDb = new Backup();
                    backupDb.Action = BackupActionType.Database;
                    databaseName = "TestDb";
                    backupFolder = @"C:\Users\developer2\Downloads\Kopie bazy";
                    backupFileName = "";
                    backupDb.Database = databaseName;
                    folderExists(backupFolder + "\\" + DateTime.Now.ToString("MMMM"));

                    // sprawdzenie czy w katalogu istnieje już głowna kopia bazy
                    var file = Directory.GetFiles(backupFolder + "\\" + DateTime.Now.ToString("MMMM"), databaseName + "-" + DateTime.Now.Month + "_head.bak", SearchOption.AllDirectories).FirstOrDefault();
                    if (file == null)
                    {
                        // jeżeli nie istnieje to tworzony jest taki plik
                        backupDb.Incremental = false;
                        backupFileName = databaseName + "-" + DateTime.Now.Month + "_head.bak";
                        backupDb.Devices.AddDevice(backupFolder + "\\" + DateTime.Now.ToString("MMMM") + "\\" + backupFileName, DeviceType.File);
                    }
                    else
                    {
                        // jeżeli istnieje to dodawane są kolejne kopie przyrostowe bazy
                        backupDb.Incremental = true;
                        backupFileName = databaseName + "-" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_incr.bak";
                        backupDb.Devices.AddDevice(backupFolder + "\\" + DateTime.Now.ToString("MMMM") + "\\" + backupFileName, DeviceType.File);
                    }
                    backupDb.Initialize = false;
                    // zapis procentów tworzenia kopii
                    backupDb.PercentComplete += ProgressEventHandler;
                    backupDb.Complete += CompleteEventHandler;
                    backupDb.SqlBackup(server);
                    // zamknięcie połączenia
                    server.ConnectionContext.Disconnect();
                }
                else
                {
                    MessageBox.Show("Musisz zaznaczyć którąś z konfiguracji", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Musisz zaznaczyć którąś z konfiguracji", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // informacja o utworzeniu (lub nie) kopii zapasowej
        private void CompleteEventHandler(object sender, Microsoft.SqlServer.Management.Common.ServerMessageEventArgs e)
        {
            if(e.ToString().Contains("successfully"))
            {
                labelBackupStatus.ForeColor = Color.Green;
                labelBackupStatus.Text = "Kopia została poprawnie wykonana";
            }
            else
            {
                labelBackupStatus.ForeColor = Color.Red;
                labelBackupStatus.Text = "Coś poszło nie tak !";
            }

        }

        // nadanie progress bar wartosci
        private void ProgressEventHandler(object sender, PercentCompleteEventArgs e)
        {            
            progressBarBackup.Value = e.Percent;
        }

        private void folderExists(String urlFolder)
        {
            if (!Directory.Exists(urlFolder))
            {
                Directory.CreateDirectory(urlFolder);                
            }            
        }

        // inicjalizacja tabeli w bazie i uzupełnienie jej o odpowiednią liczbę rekordów
        // podaną w parametrach
        private void initializeDatabaseData(int loop)
        {
            SqlConnection sqlConn = new SqlConnection();
            SqlCommand sql1;
            sqlConn.ConnectionString = @"Data Source=DESKTOP-NPHGI8D\SQLEXPRESS;Database=TestDb;Integrated Security=True";
            sqlConn.Open();

            try
            {
                sql1 = new SqlCommand("CREATE TABLE Person (Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY, Name VARCHAR(50), Surname VARCHAR(50), Address VARCHAR(50), City VARCHAR(50), ZipCode VARCHAR(50), Country VARCHAR(50), Time INT )", sqlConn);
                sql1.ExecuteNonQuery();
            }
            catch
            {
                for(int i=0;i<loop;i++)
                { 
                    sql1 = new SqlCommand("INSERT INTO Person (Name, Surname, Address, City, ZipCode, Country, Time) VALUES ('Daniel', 'Konstanty', 'Bohat orl bial', 'Nowy Sacz', '33-300', 'Poland', 1)", sqlConn);
                    sql1.ExecuteNonQuery();
                }
            }

            sqlConn.Close();
        }

        private void buttonDodajKonf_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.FormClosed += new FormClosedEventHandler(Form2_PoZamknieciu);
            form2.Show();            
        }

        void Form2_PoZamknieciu(object sender, FormClosedEventArgs e)
        {
            odswiezListe();
            przyciskiFunkcyjneWidocznosc(false);
        }

        private void buttonWyslijNaFtp_Click(object sender, EventArgs e)
        {

        }

        private void listViewKonf_ZaznaczonyWiersz(object sender, EventArgs e)
        {
            if (listViewKonf.SelectedItems.Count > 0)
            {
                przyciskiFunkcyjneWidocznosc(true);
            }
            else
            {
                przyciskiFunkcyjneWidocznosc(false);
            }
        }

        private void przyciskiFunkcyjneWidocznosc(bool widocznosc)
        {
            buttonEdytujKonf.Enabled = widocznosc;
            buttonUsunKonf.Enabled = widocznosc;
            buttonUtworzKopie.Enabled = widocznosc;
        }

        private void buttonOdswiez_OdswiezListe(object sender, EventArgs e)
        {
            odswiezListe();
        }

        private void odswiezListe()
        {
            listViewKonf.Items.Clear();
            loadDataConf();
        }

        private void buttonEdytujKonf_EdytujWiersz(object sender, EventArgs e)
        {
            if (listViewKonf.SelectedItems.Count > 0)
            {
                string id = listViewKonf.SelectedItems[0].SubItems[0].Text;
                DaneKonfiguracyjne daneKonf = settings.Lista.Where(s => s.Id == id).FirstOrDefault();
                if (daneKonf != null)
                {
                    Form2 form2 = new Form2();
                    form2.FormClosed += new FormClosedEventHandler(Form2_PoZamknieciu);
                    form2.Show();
                    form2.textBoxId.Text = daneKonf.Id;
                    form2.textBoxNazwaKopii.Text = daneKonf.NazwaKopii;
                    form2.textBoxNazwaBazy.Text = daneKonf.NazwaBazy;
                    form2.textBoxNazwaFolderuLokalnego.Text = daneKonf.NazwaFolderuLokalnego;
                    form2.textBoxNazwaFolderuFtp.Text = daneKonf.NazwaFolderuFtp;
                    form2.checkBoxAutoBackup.Checked = daneKonf.AutoBackup;
                    form2.textBoxCoIleDni.Text = daneKonf.CoIleDni.ToString();
                }
                else
                {
                    //MessageBox.Show("Nie ma takiej konfiguracji");
                    MessageBox.Show("Nie ma takiej konfiguracji", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonUsunKonf_UsunWiersz(object sender, EventArgs e)
        {
            if (listViewKonf.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("Jesteś pewien że chcesz usunąć tą konfiguracje?", "Usuń", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                { 
                    string id = listViewKonf.SelectedItems[0].SubItems[0].Text;
                    int idListView = listViewKonf.SelectedIndices[0];
                    bool wynik = XmlSerializeClass.DeleteConfiguration(id);
                    if(wynik)
                    {
                        listViewItem.SubItems.RemoveAt(idListView);
                        odswiezListe();
                        przyciskiFunkcyjneWidocznosc(false);
                    }
                }
            }
        }
    }
}
