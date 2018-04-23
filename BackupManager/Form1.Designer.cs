using System.Threading.Tasks;

namespace BackupManager
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.buttonUtworzKopie = new System.Windows.Forms.Button();
            this.buttonDeleteConf = new System.Windows.Forms.Button();
            this.buttonEditConf = new System.Windows.Forms.Button();
            this.labelProgressBarBackup = new System.Windows.Forms.Label();
            this.buttonAddConf = new System.Windows.Forms.Button();
            this.listViewConf = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonRefreshListView = new System.Windows.Forms.Button();
            this.progressBarBackup = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(19, 228);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(100, 23);
            this.progressBar1.TabIndex = 9;
            // 
            // buttonUtworzKopie
            // 
            this.buttonUtworzKopie.Enabled = false;
            this.buttonUtworzKopie.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonUtworzKopie.Location = new System.Drawing.Point(418, 224);
            this.buttonUtworzKopie.Name = "buttonUtworzKopie";
            this.buttonUtworzKopie.Size = new System.Drawing.Size(248, 23);
            this.buttonUtworzKopie.TabIndex = 8;
            this.buttonUtworzKopie.Text = "Utwórz kopię";
            this.buttonUtworzKopie.UseVisualStyleBackColor = true;
            this.buttonUtworzKopie.Click += new System.EventHandler(this.buttonUtworzKopie_Click);
            // 
            // buttonDeleteConf
            // 
            this.buttonDeleteConf.Enabled = false;
            this.buttonDeleteConf.Location = new System.Drawing.Point(183, 224);
            this.buttonDeleteConf.Name = "buttonDeleteConf";
            this.buttonDeleteConf.Size = new System.Drawing.Size(75, 23);
            this.buttonDeleteConf.TabIndex = 13;
            this.buttonDeleteConf.Text = "Usuń";
            this.buttonDeleteConf.UseVisualStyleBackColor = true;
            this.buttonDeleteConf.Click += new System.EventHandler(this.buttonDeleteConf_DeleteRow);
            // 
            // buttonEditConf
            // 
            this.buttonEditConf.Enabled = false;
            this.buttonEditConf.Location = new System.Drawing.Point(102, 224);
            this.buttonEditConf.Name = "buttonEditConf";
            this.buttonEditConf.Size = new System.Drawing.Size(75, 23);
            this.buttonEditConf.TabIndex = 13;
            this.buttonEditConf.Text = "Edytuj";
            this.buttonEditConf.UseVisualStyleBackColor = true;
            this.buttonEditConf.Click += new System.EventHandler(this.buttonEditConf_EditRow);
            // 
            // labelProgressBarBackup
            // 
            this.labelProgressBarBackup.AutoSize = true;
            this.labelProgressBarBackup.Location = new System.Drawing.Point(418, 268);
            this.labelProgressBarBackup.Name = "labelProgressBarBackup";
            this.labelProgressBarBackup.Size = new System.Drawing.Size(40, 13);
            this.labelProgressBarBackup.TabIndex = 14;
            this.labelProgressBarBackup.Text = "Postęp";
            // 
            // buttonAddConf
            // 
            this.buttonAddConf.Location = new System.Drawing.Point(21, 224);
            this.buttonAddConf.Name = "buttonAddConf";
            this.buttonAddConf.Size = new System.Drawing.Size(75, 23);
            this.buttonAddConf.TabIndex = 13;
            this.buttonAddConf.Text = "Dodaj";
            this.buttonAddConf.UseVisualStyleBackColor = true;
            this.buttonAddConf.Click += new System.EventHandler(this.buttonAddConf_AddConf);
            // 
            // listViewConf
            // 
            this.listViewConf.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.listViewConf.FullRowSelect = true;
            this.listViewConf.GridLines = true;
            this.listViewConf.Location = new System.Drawing.Point(21, 12);
            this.listViewConf.Name = "listViewConf";
            this.listViewConf.Size = new System.Drawing.Size(645, 193);
            this.listViewConf.TabIndex = 15;
            this.listViewConf.UseCompatibleStateImageBehavior = false;
            this.listViewConf.View = System.Windows.Forms.View.Details;
            this.listViewConf.SelectedIndexChanged += new System.EventHandler(this.listViewConf_SelectedRow);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ID";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Nazwa kopii";
            this.columnHeader2.Width = 150;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Nazwa bazy";
            this.columnHeader3.Width = 150;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Wysłać backup na serwer FTP?";
            this.columnHeader4.Width = 180;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Co ile dni backup?";
            this.columnHeader5.Width = 100;
            // 
            // buttonRefreshListView
            // 
            this.buttonRefreshListView.Location = new System.Drawing.Point(264, 224);
            this.buttonRefreshListView.Name = "buttonRefreshListView";
            this.buttonRefreshListView.Size = new System.Drawing.Size(75, 23);
            this.buttonRefreshListView.TabIndex = 16;
            this.buttonRefreshListView.Text = "Odśwież";
            this.buttonRefreshListView.UseVisualStyleBackColor = true;
            this.buttonRefreshListView.Click += new System.EventHandler(this.buttonRefreshListView_RefreshListView);
            // 
            // progressBarBackup
            // 
            this.progressBarBackup.Enabled = false;
            this.progressBarBackup.Location = new System.Drawing.Point(418, 287);
            this.progressBarBackup.Name = "progressBarBackup";
            this.progressBarBackup.Size = new System.Drawing.Size(255, 23);
            this.progressBarBackup.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 425);
            this.Controls.Add(this.buttonRefreshListView);
            this.Controls.Add(this.listViewConf);
            this.Controls.Add(this.labelProgressBarBackup);
            this.Controls.Add(this.buttonEditConf);
            this.Controls.Add(this.buttonAddConf);
            this.Controls.Add(this.buttonDeleteConf);
            this.Controls.Add(this.progressBarBackup);
            this.Controls.Add(this.buttonUtworzKopie);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Form1";
            this.Text = "Menadżer kopii zapasowych baz danych";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button BtnDisconnect;
        private System.Windows.Forms.Button BtnConnect;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button buttonUtworzKopie;
        private System.Windows.Forms.Button buttonDeleteConf;
        private System.Windows.Forms.Button buttonEditConf;
        private System.Windows.Forms.Label labelProgressBarBackup;
        private System.Windows.Forms.Button buttonAddConf;
        private System.Windows.Forms.ListView listViewConf;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Button buttonRefreshListView;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ProgressBar progressBarBackup;
    }
}