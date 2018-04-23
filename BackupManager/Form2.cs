using BackupManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BackupManager
{
    public partial class Form2 : Form
    {
        SmoManager smoManager;
        XmlSerializeManager xmlSerialize;

        public delegate void CallBackDel(CallBackModel callBackModel);
        private CallBackDel callBackDel;

        public delegate void CompletedTaskEventHandler(object o, EventArgs e);
        public event CompletedTaskEventHandler CompletedTask;

        public Form2()
        {
            initForm();
        }

        public Form2(BackupConfigurationData backupConfigurationData) : this()
        {
            initFields(backupConfigurationData);
        }

        protected virtual void OnCompletedTask()
        {
            if (CompletedTask != null)
                CompletedTask(this, EventArgs.Empty);
        }

        private void initForm()
        {
            InitializeComponent();
            textBoxLocalDirectory.Enabled = false;

            smoManager = new SmoManager();
            xmlSerialize = new XmlSerializeManager();

            callBackDel = callBackResultMethod;
        }

        private void initFields(BackupConfigurationData backupConfigurationData)
        {
            loadComboboxList();

            textBoxFileName.Text = backupConfigurationData.FileName;
            textBoxId.Text = backupConfigurationData.Id;
            comboBoxDatabaseName.SelectedIndex = comboBoxDatabaseName.FindStringExact(backupConfigurationData.DatabaseName);
            textBoxLocalDirectory.Text = backupConfigurationData.LocalDirectory;            
            numericBoxBackupDays.Value = backupConfigurationData.BackupDays;
            checkBoxSendToFtp.Checked = backupConfigurationData.SendToFtp;
            textBoxFtpDirectory.Text = backupConfigurationData.SendToFtp ? backupConfigurationData.FtpDirectory : string.Empty;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            loadComboboxList();
        }

        private void buttonSaveConfiguration_Click(object sender, EventArgs e)
        {
            if (errorFields().Any())
            {
                string errorsToDisplay = string.Join(Environment.NewLine, errorFields());
                MessageBox.Show(errorsToDisplay);
                return;
            }

            BackupConfigurationData data = getFormData();

            if(string.IsNullOrEmpty(textBoxId.Text))
                xmlSerialize.SaveConfiguration(data, callBackDel);
            else
                xmlSerialize.UpdateConfiguration(textBoxId.Text, data, callBackDel);
        }

        // sprawdzenie czy button jest zaznaczony
        // jezeli jest zaznaczony to textbox z ilością dni jest widoczny
        // jeżeli nie jest zaznaczony to textbox z ilością dni nie jest widoczny
        private void checkBoxSendToFtp_Check(object sender, EventArgs e)
        {
            CheckBox chbx = (CheckBox)sender;
            if (chbx.Checked)
            {
                textBoxFtpDirectory.Enabled = true;
            }
            else
            {
                textBoxFtpDirectory.Enabled = false;
            }
        }

        // przeglądanie plików w celu wybrania ścieżki
        private void buttonBrowseLocalDirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog showDialog = new FolderBrowserDialog();
            if (showDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxLocalDirectory.Text = showDialog.SelectedPath;
            }
        }

        // metoda wywołująca zamkniecie okna
        private void buttonCloseWindow_Click(object sender, EventArgs e)
        {
            zamknijOkno();
        }

        // zamkniecie okna
        private void zamknijOkno()
        {
            czyscPola();
            this.Close();
        }

        // wyczyszczenie pól
        private void czyscPola()
        {
            textBoxFileName.Text = String.Empty;
            comboBoxDatabaseName.SelectedValue = String.Empty;
            textBoxLocalDirectory.Text = String.Empty;
            textBoxFtpDirectory.Text = String.Empty;
            checkBoxSendToFtp.Checked = false;
        }

        private void loadComboboxList()
        {
            if (comboBoxDatabaseName.Items.Count > 0)
                return;

            var databaseList = smoManager.GetListDatabases();

            databaseList.ForEach(i =>
            {
                i = i.Replace("[", "").Replace("]", "");
                comboBoxDatabaseName.Items.Add(i);
            });
        }

        private List<string> errorFields()
        {
            List<string> errors = new List<string>();

            if (textBoxFileName.Text == string.Empty)
            {
                errors.Add("Pole 'Nazwa kopii' nie może być puste");
            }

            if (comboBoxDatabaseName.SelectedItem == null)
            {
                errors.Add("Pole 'Nazwa bazy' nie może być puste");
            }

            if (textBoxLocalDirectory.Text == string.Empty)
            {
                errors.Add("Pole 'Folder docelowy na dysku lokalnym' nie może być puste");
            }

            if(checkBoxSendToFtp.Checked)
            {
                if (textBoxFtpDirectory.Text == string.Empty)
                {
                    errors.Add("Pole 'Nazwa folderu na serwerze FTP' nie może być puste");
                }
            }

            return errors;
        }

        private BackupConfigurationData getFormData()
        {
            BackupConfigurationData model = new BackupConfigurationData() {
                FileName = textBoxFileName.Text,
                DatabaseName = (string)comboBoxDatabaseName.SelectedItem,
                LocalDirectory = textBoxLocalDirectory.Text,
                BackupDays = (int)numericBoxBackupDays.Value,
                SendToFtp = checkBoxSendToFtp.Checked,
                FtpDirectory = checkBoxSendToFtp.Checked ? textBoxFtpDirectory.Text : string.Empty
            };

            return model;
        }

        private void callBackResultMethod(CallBackModel callBackModel)
        {
            if (callBackModel.Success)
            { 
                zamknijOkno();

                // wywołanie zdarzenia po zamknięciu okna
                OnCompletedTask();

                if (!string.IsNullOrEmpty(callBackModel.Message))
                    MessageBox.Show(callBackModel.Message);
            }
            else
            {
                if (!string.IsNullOrEmpty(callBackModel.Message))
                    MessageBox.Show(callBackModel.Message);
                else
                    MessageBox.Show("Coś poszło nie tak");
            }
        }
    }
}
