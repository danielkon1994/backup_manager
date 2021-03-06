﻿using System;
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
using System.IO;
using FluentFTP;
using System.Net;
using System.Diagnostics;
using System.Configuration;
using BackupManager.Models;
//using System.Data.SqlClient;

namespace BackupManager
{
    public partial class Form1 : Form
    {
        public delegate void CallBackDel(CallBackModel model);
        public delegate void ShowBackupPercentCompleteDel(int percent);
        public delegate void ShowBackupMessageDel(string message);
        public delegate void ToggleProgressBar(bool show);

        private CallBackDel callBackDel;
        private PercentCompleteEventHandler backupPercentComplete;
        private ServerMessageEventHandler backupComplete;
        private ServerMessageEventHandler backupInformation;

        XmlSerializeManager xmlSerializeManager;
        SmoManager smoManager;

        public Form1()
        {
            InitializeComponent();

            xmlSerializeManager = new XmlSerializeManager();
            smoManager = new SmoManager();

            callBackDel = callBackResultMethod;
            backupPercentComplete = backup_PercentComplete;
            backupComplete = backup_Complete;
            backupInformation = backup_Information;
            // usupełnianie bazy danych testowymi rekordami
            //initializeDatabaseData(300); 
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadListView();
        }

        // Subskrybent używany dla wydarzenia oryginalnego, opisanego poniżej
        //private void Form2_AfterClosing(object sender, FormClosedEventArgs e)
        //{
        //    refreshListView();
        //    changeFunctionButtonsVisible(false);
        //}

        // Subskrybent używany dla wydarzenia customowego, opisanego poniżej
        private void Form2_AfterClosing(object sender, EventArgs e)
        {
            refreshListView();
            changeFunctionButtonsVisible(false);
        }

        private void listViewConf_SelectedRow(object sender, EventArgs e)
        {
            changeFunctionButtonsVisible(true);
        }

        private void buttonAddConf_AddConf(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            // Można zamknąć okno na dwa sposoby:
            // a) wydarzenie oryginalne
            //form2.FormClosed += new FormClosedEventHandler(Form2_AfterClosing);
            // b) wydarzenie customowe
            form2.CompletedTask += Form2_AfterClosing;
            form2.Show();
        }

        private void buttonEditConf_EditRow(object sender, EventArgs e)
        {
            checkSelectedListViewItems();

            string id = listViewConf.SelectedItems[0].SubItems[0].Text;
            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Show("Konfiguracja nie posiada identyfikatora");
                return;
            }

            BackupConfigurationData backupConfigurationData = xmlSerializeManager.GetConfigurationById(id);
            if (backupConfigurationData != null)
            {
                Form2 form2 = new Form2(backupConfigurationData);
                form2.CompletedTask += Form2_AfterClosing;
                form2.Show();
            }
            else
            {
                MessageBox.Show("Nie ma takiej konfiguracji");
                return;
            }
        }

        private void buttonDeleteConf_DeleteRow(object sender, EventArgs e)
        {
            checkSelectedListViewItems();

            string id = listViewConf.SelectedItems[0].SubItems[0].Text;
            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Show("Konfiguracja nie posiada identyfikatora");
                return;
            }

            xmlSerializeManager.DeleteConfiguration(id, callBackDel);
        }

        private void buttonRefreshListView_RefreshListView(object sender, EventArgs e)
        {
            refreshListView();
        }

        private void buttonUtworzKopie_Click(object sender, EventArgs e)
        {
            checkSelectedListViewItems();

            string id = listViewConf.SelectedItems[0].SubItems[0].Text;
            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Show("Konfiguracja nie posiada identyfikatora");
                return;
            }
            BackupConfigurationData backupConfiguration = xmlSerializeManager.GetConfigurationById(id);

            togglePercentBar(true);
            smoManager.CreateBackup(backupConfiguration, backupPercentComplete, backupComplete);
        }

        private void refreshListView()
        {
            listViewConf.Items.Clear();
            loadListView();
        }

        private void changeFunctionButtonsVisible(bool widocznosc)
        {
            buttonEditConf.Enabled = widocznosc;
            buttonDeleteConf.Enabled = widocznosc;
            buttonUtworzKopie.Enabled = widocznosc;
        }

        private void loadListView()
        {
            var backupConfList = xmlSerializeManager.GetListFromXmlFile();
            foreach(var conf in backupConfList)
            {
                var listViewItem = new ListViewItem(conf.Id);
                listViewItem.SubItems.Add(conf.FileName);
                listViewItem.SubItems.Add(conf.DatabaseName);
                listViewItem.SubItems.Add(conf.SendToFtp ? "Tak" : "Nie");
                listViewItem.SubItems.Add(conf.BackupDays.ToString());
                listViewConf.Items.Add(listViewItem);
            }
        }

        private void checkSelectedListViewItems()
        {
            if (listViewConf.SelectedItems.Count < 1 || listViewConf.SelectedItems.Count > 1)
            {
                string message = listViewConf.SelectedItems.Count < 1 ? "Musi być zaznaczona jakakolwiek konfiguracja" : "Musi być zaznaczona tylko jedna konfiguracja";
                MessageBox.Show(message);
                return;
            }
        }

        private void backup_PercentComplete(object sender, PercentCompleteEventArgs e)
        {
            object[] percent = new object[1];
            percent[0] = e.Percent;

            this.BeginInvoke(new ShowBackupPercentCompleteDel(showPercentComplete), percent);
        }

        private void backup_Information(object sender, ServerMessageEventArgs e)
        {
            object[] message = new object[1];
            message[0] = e.Error.Message;

            this.BeginInvoke(new ShowBackupMessageDel(showMessage), message);
        }

        private void backup_Complete(object sender, ServerMessageEventArgs e)
        {
            object[] message = new object[1];
            message[0] = "Backup został zakończony";

            this.BeginInvoke(new ToggleProgressBar(togglePercentBar), false);
            this.BeginInvoke(new ShowBackupMessageDel(showMessage), message);
        }

        private void callBackResultMethod(CallBackModel callBackModel)
        {
            if (callBackModel.Success)
            {
                if (!string.IsNullOrEmpty(callBackModel.Message))
                    MessageBox.Show(callBackModel.Message);

                refreshListView();
            }
            else
            {
                if (!string.IsNullOrEmpty(callBackModel.Message))
                    MessageBox.Show(callBackModel.Message);
                else
                    MessageBox.Show("Coś poszło nie tak");
            }
        }

        private void togglePercentBar(bool show)
        {
            labelProgressBarBackup.Visible = show;
            progressBarBackup.Visible = show;
        }

        private void showPercentComplete(int percent)
        {
            progressBarBackup.Value = percent;
        }

        private void showMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}