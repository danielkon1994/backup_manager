namespace BackupManager
{
    partial class Form2
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
            this.label1 = new System.Windows.Forms.Label();
            this.labelFileName = new System.Windows.Forms.Label();
            this.textBoxFileName = new System.Windows.Forms.TextBox();
            this.labelDatabaseName = new System.Windows.Forms.Label();
            this.labelLocalDirectory = new System.Windows.Forms.Label();
            this.textBoxLocalDirectory = new System.Windows.Forms.TextBox();
            this.buttonBrowseLocalDirectory = new System.Windows.Forms.Button();
            this.labelFtpDirectory = new System.Windows.Forms.Label();
            this.textBoxFtpDirectory = new System.Windows.Forms.TextBox();
            this.labelSendToFtp = new System.Windows.Forms.Label();
            this.checkBoxSendToFtp = new System.Windows.Forms.CheckBox();
            this.buttonSaveConfiguration = new System.Windows.Forms.Button();
            this.labelBackupDays = new System.Windows.Forms.Label();
            this.buttonCloseWindow = new System.Windows.Forms.Button();
            this.textBoxId = new System.Windows.Forms.TextBox();
            this.comboBoxDatabaseName = new System.Windows.Forms.ComboBox();
            this.numericBoxBackupDays = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericBoxBackupDays)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(240, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Zarządzanie kopiami baz danych";
            // 
            // labelFileName
            // 
            this.labelFileName.AutoSize = true;
            this.labelFileName.Location = new System.Drawing.Point(17, 52);
            this.labelFileName.Name = "labelFileName";
            this.labelFileName.Size = new System.Drawing.Size(65, 13);
            this.labelFileName.TabIndex = 1;
            this.labelFileName.Text = "Nazwa kopii";
            // 
            // textBoxFileName
            // 
            this.textBoxFileName.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxFileName.Location = new System.Drawing.Point(20, 69);
            this.textBoxFileName.Name = "textBoxFileName";
            this.textBoxFileName.Size = new System.Drawing.Size(270, 20);
            this.textBoxFileName.TabIndex = 1;
            // 
            // labelDatabaseName
            // 
            this.labelDatabaseName.AutoSize = true;
            this.labelDatabaseName.Location = new System.Drawing.Point(17, 104);
            this.labelDatabaseName.Name = "labelDatabaseName";
            this.labelDatabaseName.Size = new System.Drawing.Size(65, 13);
            this.labelDatabaseName.TabIndex = 1;
            this.labelDatabaseName.Text = "Nazwa bazy";
            // 
            // labelLocalDirectory
            // 
            this.labelLocalDirectory.AutoSize = true;
            this.labelLocalDirectory.Location = new System.Drawing.Point(17, 156);
            this.labelLocalDirectory.Name = "labelLocalDirectory";
            this.labelLocalDirectory.Size = new System.Drawing.Size(174, 13);
            this.labelLocalDirectory.TabIndex = 1;
            this.labelLocalDirectory.Text = "Folder docelowy na dysku lokalnym";
            // 
            // textBoxLocalDirectory
            // 
            this.textBoxLocalDirectory.Location = new System.Drawing.Point(20, 173);
            this.textBoxLocalDirectory.Name = "textBoxLocalDirectory";
            this.textBoxLocalDirectory.Size = new System.Drawing.Size(270, 20);
            this.textBoxLocalDirectory.TabIndex = 2;
            // 
            // buttonBrowseLocalDirectory
            // 
            this.buttonBrowseLocalDirectory.Location = new System.Drawing.Point(297, 170);
            this.buttonBrowseLocalDirectory.Name = "buttonBrowseLocalDirectory";
            this.buttonBrowseLocalDirectory.Size = new System.Drawing.Size(75, 26);
            this.buttonBrowseLocalDirectory.TabIndex = 3;
            this.buttonBrowseLocalDirectory.Text = "Przeglądaj";
            this.buttonBrowseLocalDirectory.UseVisualStyleBackColor = true;
            this.buttonBrowseLocalDirectory.Click += new System.EventHandler(this.buttonBrowseLocalDirectory_Click);
            // 
            // labelFtpDirectory
            // 
            this.labelFtpDirectory.AutoSize = true;
            this.labelFtpDirectory.Location = new System.Drawing.Point(20, 306);
            this.labelFtpDirectory.Name = "labelFtpDirectory";
            this.labelFtpDirectory.Size = new System.Drawing.Size(158, 13);
            this.labelFtpDirectory.TabIndex = 1;
            this.labelFtpDirectory.Text = "Nazwa folderu na serwerze FTP";
            // 
            // textBoxFtpDirectory
            // 
            this.textBoxFtpDirectory.Enabled = false;
            this.textBoxFtpDirectory.Location = new System.Drawing.Point(23, 323);
            this.textBoxFtpDirectory.Name = "textBoxFtpDirectory";
            this.textBoxFtpDirectory.Size = new System.Drawing.Size(270, 20);
            this.textBoxFtpDirectory.TabIndex = 6;
            // 
            // labelSendToFtp
            // 
            this.labelSendToFtp.AutoSize = true;
            this.labelSendToFtp.Location = new System.Drawing.Point(20, 260);
            this.labelSendToFtp.Name = "labelSendToFtp";
            this.labelSendToFtp.Size = new System.Drawing.Size(161, 13);
            this.labelSendToFtp.TabIndex = 1;
            this.labelSendToFtp.Text = "Wysłać backup na serwer FTP?";
            // 
            // checkBoxSendToFtp
            // 
            this.checkBoxSendToFtp.AutoSize = true;
            this.checkBoxSendToFtp.Location = new System.Drawing.Point(23, 277);
            this.checkBoxSendToFtp.Name = "checkBoxSendToFtp";
            this.checkBoxSendToFtp.Size = new System.Drawing.Size(94, 17);
            this.checkBoxSendToFtp.TabIndex = 5;
            this.checkBoxSendToFtp.Text = "Tak, poproszę";
            this.checkBoxSendToFtp.UseVisualStyleBackColor = true;
            this.checkBoxSendToFtp.CheckedChanged += new System.EventHandler(this.checkBoxSendToFtp_Check);
            // 
            // buttonSaveConfiguration
            // 
            this.buttonSaveConfiguration.Location = new System.Drawing.Point(20, 375);
            this.buttonSaveConfiguration.Name = "buttonSaveConfiguration";
            this.buttonSaveConfiguration.Size = new System.Drawing.Size(270, 23);
            this.buttonSaveConfiguration.TabIndex = 7;
            this.buttonSaveConfiguration.Text = "Zapisz";
            this.buttonSaveConfiguration.UseVisualStyleBackColor = true;
            this.buttonSaveConfiguration.Click += new System.EventHandler(this.buttonSaveConfiguration_Click);
            // 
            // labelBackupDays
            // 
            this.labelBackupDays.AutoSize = true;
            this.labelBackupDays.Location = new System.Drawing.Point(20, 208);
            this.labelBackupDays.Name = "labelBackupDays";
            this.labelBackupDays.Size = new System.Drawing.Size(184, 13);
            this.labelBackupDays.TabIndex = 6;
            this.labelBackupDays.Text = "Co ile dni wykonywać pełny backup?";
            // 
            // buttonCloseWindow
            // 
            this.buttonCloseWindow.Location = new System.Drawing.Point(20, 426);
            this.buttonCloseWindow.Name = "buttonCloseWindow";
            this.buttonCloseWindow.Size = new System.Drawing.Size(62, 23);
            this.buttonCloseWindow.TabIndex = 8;
            this.buttonCloseWindow.Text = "Zamknij";
            this.buttonCloseWindow.UseVisualStyleBackColor = true;
            this.buttonCloseWindow.Click += new System.EventHandler(this.buttonCloseWindow_Click);
            // 
            // textBoxId
            // 
            this.textBoxId.Enabled = false;
            this.textBoxId.Location = new System.Drawing.Point(311, 68);
            this.textBoxId.Name = "textBoxId";
            this.textBoxId.Size = new System.Drawing.Size(100, 20);
            this.textBoxId.TabIndex = 8;
            this.textBoxId.Visible = false;
            // 
            // comboBoxDatabaseName
            // 
            this.comboBoxDatabaseName.FormattingEnabled = true;
            this.comboBoxDatabaseName.Location = new System.Drawing.Point(20, 121);
            this.comboBoxDatabaseName.Name = "comboBoxDatabaseName";
            this.comboBoxDatabaseName.Size = new System.Drawing.Size(270, 21);
            this.comboBoxDatabaseName.TabIndex = 2;
            // 
            // numericBoxBackupDays
            // 
            this.numericBoxBackupDays.Location = new System.Drawing.Point(23, 224);
            this.numericBoxBackupDays.Name = "numericBoxBackupDays";
            this.numericBoxBackupDays.Size = new System.Drawing.Size(267, 20);
            this.numericBoxBackupDays.TabIndex = 10;
            this.numericBoxBackupDays.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 461);
            this.Controls.Add(this.numericBoxBackupDays);
            this.Controls.Add(this.comboBoxDatabaseName);
            this.Controls.Add(this.textBoxId);
            this.Controls.Add(this.labelBackupDays);
            this.Controls.Add(this.buttonCloseWindow);
            this.Controls.Add(this.buttonSaveConfiguration);
            this.Controls.Add(this.checkBoxSendToFtp);
            this.Controls.Add(this.buttonBrowseLocalDirectory);
            this.Controls.Add(this.textBoxFtpDirectory);
            this.Controls.Add(this.textBoxLocalDirectory);
            this.Controls.Add(this.labelSendToFtp);
            this.Controls.Add(this.labelFtpDirectory);
            this.Controls.Add(this.labelLocalDirectory);
            this.Controls.Add(this.labelDatabaseName);
            this.Controls.Add(this.textBoxFileName);
            this.Controls.Add(this.labelFileName);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Form2";
            this.Text = "Dodaj konfiguracje";
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericBoxBackupDays)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelFileName;
        public System.Windows.Forms.TextBox textBoxFileName;
        private System.Windows.Forms.Label labelDatabaseName;
        private System.Windows.Forms.Label labelLocalDirectory;
        public System.Windows.Forms.TextBox textBoxLocalDirectory;
        public System.Windows.Forms.Button buttonBrowseLocalDirectory;
        private System.Windows.Forms.Label labelFtpDirectory;
        public System.Windows.Forms.TextBox textBoxFtpDirectory;
        private System.Windows.Forms.Label labelSendToFtp;
        public System.Windows.Forms.CheckBox checkBoxSendToFtp;
        public System.Windows.Forms.Button buttonSaveConfiguration;
        private System.Windows.Forms.Label labelBackupDays;
        public System.Windows.Forms.Button buttonCloseWindow;
        public System.Windows.Forms.TextBox textBoxId;
        private System.Windows.Forms.ComboBox comboBoxDatabaseName;
        private System.Windows.Forms.NumericUpDown numericBoxBackupDays;
    }
}