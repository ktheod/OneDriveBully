namespace OneDriveBully
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.txt_OneDriveFolder = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_OneDriveFolder = new System.Windows.Forms.Label();
            this.b_SaveSettings = new System.Windows.Forms.Button();
            this.lbl_Interval = new System.Windows.Forms.Label();
            this.txt_Interval = new System.Windows.Forms.TextBox();
            this.lbl_WindowsStartup = new System.Windows.Forms.Label();
            this.cb_LoadOnWindowsStartup = new System.Windows.Forms.CheckBox();
            this.fbd_OneDrivePath = new System.Windows.Forms.FolderBrowserDialog();
            this.b_browser = new System.Windows.Forms.Button();
            this.b_addSymLink = new System.Windows.Forms.Button();
            this.b_DeleteSymLink = new System.Windows.Forms.Button();
            this.b_refreshSymLinks = new System.Windows.Forms.Button();
            this.dgv_SymLinks = new System.Windows.Forms.DataGridView();
            this.fbd_SymLinks = new System.Windows.Forms.FolderBrowserDialog();
            this.lbl_SymbolicLinks = new System.Windows.Forms.Label();
            this.cb_ShowInstructions = new System.Windows.Forms.CheckBox();
            this.lblShowInstructions = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_SymLinks)).BeginInit();
            this.SuspendLayout();
            // 
            // txt_OneDriveFolder
            // 
            this.txt_OneDriveFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_OneDriveFolder.Location = new System.Drawing.Point(144, 8);
            this.txt_OneDriveFolder.Margin = new System.Windows.Forms.Padding(2);
            this.txt_OneDriveFolder.Name = "txt_OneDriveFolder";
            this.txt_OneDriveFolder.ReadOnly = true;
            this.txt_OneDriveFolder.Size = new System.Drawing.Size(580, 20);
            this.txt_OneDriveFolder.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 1;
            // 
            // lbl_OneDriveFolder
            // 
            this.lbl_OneDriveFolder.AutoSize = true;
            this.lbl_OneDriveFolder.Location = new System.Drawing.Point(11, 11);
            this.lbl_OneDriveFolder.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_OneDriveFolder.Name = "lbl_OneDriveFolder";
            this.lbl_OneDriveFolder.Size = new System.Drawing.Size(113, 13);
            this.lbl_OneDriveFolder.TabIndex = 2;
            this.lbl_OneDriveFolder.Text = "OneDrive Root Folder:";
            // 
            // b_SaveSettings
            // 
            this.b_SaveSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.b_SaveSettings.Location = new System.Drawing.Point(728, 31);
            this.b_SaveSettings.Margin = new System.Windows.Forms.Padding(2);
            this.b_SaveSettings.Name = "b_SaveSettings";
            this.b_SaveSettings.Size = new System.Drawing.Size(56, 20);
            this.b_SaveSettings.TabIndex = 4;
            this.b_SaveSettings.Text = "Save Settings";
            this.b_SaveSettings.UseVisualStyleBackColor = true;
            this.b_SaveSettings.Click += new System.EventHandler(this.B_SaveSettings_Click);
            // 
            // lbl_Interval
            // 
            this.lbl_Interval.AutoSize = true;
            this.lbl_Interval.Location = new System.Drawing.Point(11, 34);
            this.lbl_Interval.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_Interval.Name = "lbl_Interval";
            this.lbl_Interval.Size = new System.Drawing.Size(112, 13);
            this.lbl_Interval.TabIndex = 5;
            this.lbl_Interval.Text = "Bully Every X Minutes:";
            // 
            // txt_Interval
            // 
            this.txt_Interval.Location = new System.Drawing.Point(144, 31);
            this.txt_Interval.Margin = new System.Windows.Forms.Padding(2);
            this.txt_Interval.Name = "txt_Interval";
            this.txt_Interval.Size = new System.Drawing.Size(40, 20);
            this.txt_Interval.TabIndex = 2;
            this.txt_Interval.Text = "0";
            this.txt_Interval.TextChanged += new System.EventHandler(this.Txt_Interval_TextChanged);
            // 
            // lbl_WindowsStartup
            // 
            this.lbl_WindowsStartup.AutoSize = true;
            this.lbl_WindowsStartup.Location = new System.Drawing.Point(11, 58);
            this.lbl_WindowsStartup.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_WindowsStartup.Name = "lbl_WindowsStartup";
            this.lbl_WindowsStartup.Size = new System.Drawing.Size(130, 13);
            this.lbl_WindowsStartup.TabIndex = 7;
            this.lbl_WindowsStartup.Text = "Load on Windows Startup";
            // 
            // cb_LoadOnWindowsStartup
            // 
            this.cb_LoadOnWindowsStartup.AutoSize = true;
            this.cb_LoadOnWindowsStartup.Location = new System.Drawing.Point(169, 57);
            this.cb_LoadOnWindowsStartup.Margin = new System.Windows.Forms.Padding(2);
            this.cb_LoadOnWindowsStartup.Name = "cb_LoadOnWindowsStartup";
            this.cb_LoadOnWindowsStartup.Size = new System.Drawing.Size(15, 14);
            this.cb_LoadOnWindowsStartup.TabIndex = 3;
            this.cb_LoadOnWindowsStartup.UseVisualStyleBackColor = true;
            this.cb_LoadOnWindowsStartup.CheckedChanged += new System.EventHandler(this.Cb_LoadOnWindowsStartup_CheckedChanged);
            // 
            // fbd_OneDrivePath
            // 
            this.fbd_OneDrivePath.RootFolder = System.Environment.SpecialFolder.UserProfile;
            this.fbd_OneDrivePath.ShowNewFolderButton = false;
            // 
            // b_browser
            // 
            this.b_browser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.b_browser.Location = new System.Drawing.Point(728, 7);
            this.b_browser.Margin = new System.Windows.Forms.Padding(2);
            this.b_browser.Name = "b_browser";
            this.b_browser.Size = new System.Drawing.Size(56, 21);
            this.b_browser.TabIndex = 1;
            this.b_browser.Text = "Browse";
            this.b_browser.UseVisualStyleBackColor = true;
            this.b_browser.Click += new System.EventHandler(this.B_browser_Click);
            // 
            // b_addSymLink
            // 
            this.b_addSymLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.b_addSymLink.Location = new System.Drawing.Point(607, 123);
            this.b_addSymLink.Margin = new System.Windows.Forms.Padding(2);
            this.b_addSymLink.Name = "b_addSymLink";
            this.b_addSymLink.Size = new System.Drawing.Size(56, 21);
            this.b_addSymLink.TabIndex = 5;
            this.b_addSymLink.Text = "Add";
            this.b_addSymLink.UseVisualStyleBackColor = true;
            this.b_addSymLink.Click += new System.EventHandler(this.B_addSymLink_Click);
            // 
            // b_DeleteSymLink
            // 
            this.b_DeleteSymLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.b_DeleteSymLink.Location = new System.Drawing.Point(668, 123);
            this.b_DeleteSymLink.Margin = new System.Windows.Forms.Padding(2);
            this.b_DeleteSymLink.Name = "b_DeleteSymLink";
            this.b_DeleteSymLink.Size = new System.Drawing.Size(56, 21);
            this.b_DeleteSymLink.TabIndex = 6;
            this.b_DeleteSymLink.Text = "Delete";
            this.b_DeleteSymLink.UseVisualStyleBackColor = true;
            this.b_DeleteSymLink.Click += new System.EventHandler(this.B_DeleteSymLink_Click);
            // 
            // b_refreshSymLinks
            // 
            this.b_refreshSymLinks.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.b_refreshSymLinks.Location = new System.Drawing.Point(729, 123);
            this.b_refreshSymLinks.Margin = new System.Windows.Forms.Padding(2);
            this.b_refreshSymLinks.Name = "b_refreshSymLinks";
            this.b_refreshSymLinks.Size = new System.Drawing.Size(56, 21);
            this.b_refreshSymLinks.TabIndex = 7;
            this.b_refreshSymLinks.Text = "Refresh";
            this.b_refreshSymLinks.UseVisualStyleBackColor = true;
            this.b_refreshSymLinks.Click += new System.EventHandler(this.B_refreshSymLinks_Click);
            // 
            // dgv_SymLinks
            // 
            this.dgv_SymLinks.AllowUserToAddRows = false;
            this.dgv_SymLinks.AllowUserToDeleteRows = false;
            this.dgv_SymLinks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_SymLinks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_SymLinks.Location = new System.Drawing.Point(13, 148);
            this.dgv_SymLinks.Margin = new System.Windows.Forms.Padding(2);
            this.dgv_SymLinks.MultiSelect = false;
            this.dgv_SymLinks.Name = "dgv_SymLinks";
            this.dgv_SymLinks.ReadOnly = true;
            this.dgv_SymLinks.RowTemplate.Height = 24;
            this.dgv_SymLinks.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgv_SymLinks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_SymLinks.Size = new System.Drawing.Size(772, 305);
            this.dgv_SymLinks.TabIndex = 13;
            // 
            // fbd_SymLinks
            // 
            this.fbd_SymLinks.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // lbl_SymbolicLinks
            // 
            this.lbl_SymbolicLinks.AutoSize = true;
            this.lbl_SymbolicLinks.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_SymbolicLinks.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lbl_SymbolicLinks.Location = new System.Drawing.Point(9, 118);
            this.lbl_SymbolicLinks.Name = "lbl_SymbolicLinks";
            this.lbl_SymbolicLinks.Size = new System.Drawing.Size(127, 20);
            this.lbl_SymbolicLinks.TabIndex = 14;
            this.lbl_SymbolicLinks.Text = "Symbolic Links";
            // 
            // cb_ShowInstructions
            // 
            this.cb_ShowInstructions.AutoSize = true;
            this.cb_ShowInstructions.Location = new System.Drawing.Point(169, 80);
            this.cb_ShowInstructions.Margin = new System.Windows.Forms.Padding(2);
            this.cb_ShowInstructions.Name = "cb_ShowInstructions";
            this.cb_ShowInstructions.Size = new System.Drawing.Size(15, 14);
            this.cb_ShowInstructions.TabIndex = 15;
            this.cb_ShowInstructions.UseVisualStyleBackColor = true;
            this.cb_ShowInstructions.CheckedChanged += new System.EventHandler(this.Cb_ShowInstructions_CheckedChanged);
            // 
            // lblShowInstructions
            // 
            this.lblShowInstructions.AutoSize = true;
            this.lblShowInstructions.Location = new System.Drawing.Point(11, 80);
            this.lblShowInstructions.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblShowInstructions.Name = "lblShowInstructions";
            this.lblShowInstructions.Size = new System.Drawing.Size(149, 13);
            this.lblShowInstructions.TabIndex = 16;
            this.lblShowInstructions.Text = "Show Instructions on next Exit";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(793, 464);
            this.Controls.Add(this.cb_ShowInstructions);
            this.Controls.Add(this.lblShowInstructions);
            this.Controls.Add(this.lbl_SymbolicLinks);
            this.Controls.Add(this.dgv_SymLinks);
            this.Controls.Add(this.b_refreshSymLinks);
            this.Controls.Add(this.b_DeleteSymLink);
            this.Controls.Add(this.b_addSymLink);
            this.Controls.Add(this.b_browser);
            this.Controls.Add(this.cb_LoadOnWindowsStartup);
            this.Controls.Add(this.lbl_WindowsStartup);
            this.Controls.Add(this.lbl_Interval);
            this.Controls.Add(this.txt_Interval);
            this.Controls.Add(this.b_SaveSettings);
            this.Controls.Add(this.lbl_OneDriveFolder);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_OneDriveFolder);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "SettingsForm";
            this.Text = "OneDrive Bully - Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_SymLinks)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_OneDriveFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_OneDriveFolder;
        private System.Windows.Forms.Button b_SaveSettings;
        private System.Windows.Forms.Label lbl_Interval;
        private System.Windows.Forms.TextBox txt_Interval;
        private System.Windows.Forms.Label lbl_WindowsStartup;
        private System.Windows.Forms.CheckBox cb_LoadOnWindowsStartup;
        private System.Windows.Forms.FolderBrowserDialog fbd_OneDrivePath;
        private System.Windows.Forms.Button b_browser;
        private System.Windows.Forms.Button b_addSymLink;
        private System.Windows.Forms.Button b_DeleteSymLink;
        private System.Windows.Forms.Button b_refreshSymLinks;
        private System.Windows.Forms.DataGridView dgv_SymLinks;
        private System.Windows.Forms.FolderBrowserDialog fbd_SymLinks;
        private System.Windows.Forms.Label lbl_SymbolicLinks;
        private System.Windows.Forms.CheckBox cb_ShowInstructions;
        private System.Windows.Forms.Label lblShowInstructions;
    }
}