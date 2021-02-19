using System;
using System.Data;
using System.Windows.Forms;
using System.IO;

namespace OneDriveBully
{
    //Version 1.3 - Added Show Instructions
    //            - Bug Fix on Deleting Symbolic Links when the Grid is empty
    public partial class SettingsForm : Form
    {
        bool startWithWindowsChanged = false;
        bool isDirty = false;
        DataTable SymLinksTable = new DataTable();

        public SettingsForm()
        {
            InitializeComponent();

            // Load user settings           
            if (ProcessIcon.fn.UserDefinedSettingsExist)
            {
                LoadSettings();
            }
            //Version 1.3 -
            else
            {
                cb_ShowInstructions.Checked = true;
            }
            //Version 1.3 +
        }

        #region Settings Handling

        private void LoadSettings()
        {
            // Load user settings and update form controls
            Properties.Settings.Default.Reload();
            txt_OneDriveFolder.Text = Properties.Settings.Default.OneDriveRootFolder;
            txt_Interval.Text = Properties.Settings.Default.TimerInterval.ToString();
            cb_LoadOnWindowsStartup.Checked = Properties.Settings.Default.LoadOnWindowsStartup;
            //Version 1.3 -
            cb_ShowInstructions.Checked = Properties.Settings.Default.ShowInstructions;

            //Version 1.3 +
            isDirty = false;

            if (Properties.Settings.Default.UserDefinedSettings)
            {
                Refresh_dgv();
            }
        }

        private bool ValidateSettings()
        {
            // Check OneDrive Root Folder exists
            if (txt_OneDriveFolder.Text == "")
            {
                MessageBox.Show("OneDrive root folder must have a value", "Value Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!Directory.Exists(@txt_OneDriveFolder.Text + @"\"))
            {
                MessageBox.Show("OneDrive root folder not found.", "Value Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Check Interval
            if (txt_Interval.Text == "")
            {
                MessageBox.Show("Minutes must be more than 0.", "Value Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (int.TryParse(txt_Interval.Text, out int intervalInt))
            {
                if (intervalInt <= 0)
                {
                    MessageBox.Show("Minutes must be more than 0.", "Value Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Value : " + txt_Interval.Text + " is not a number.", "Value Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void SaveSettings()
        {           
            // Save user settings
            Properties.Settings.Default.OneDriveRootFolder = txt_OneDriveFolder.Text;
            Properties.Settings.Default.TimerInterval = Convert.ToInt32(txt_Interval.Text);
            Properties.Settings.Default.LoadOnWindowsStartup = cb_LoadOnWindowsStartup.Checked;
            //Version 1.3 -
            Properties.Settings.Default.ShowInstructions = cb_ShowInstructions.Checked;
            //Version 1.3 +
            Properties.Settings.Default.UserDefinedSettings = true;
            Properties.Settings.Default.Save();
            isDirty = false;

            // Start Timer
            ProcessIcon.fn.setTimerInterval(Properties.Settings.Default.TimerInterval);

            if (startWithWindowsChanged)
            {
                //Update Windows Registry Key (Add/Remove)
                ProcessIcon.fn.startOnWindowsStartup(Properties.Settings.Default.LoadOnWindowsStartup);
                startWithWindowsChanged = false;
            }

            MessageBox.Show("Settings saved successfully.","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);            
        }

        #endregion Settings Handling

        #region Form Controls

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isDirty)
            {
                if (MessageBox.Show("You have not saved your changes. Do you want to close the form?", "Unsaved settings",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void B_SaveSettings_Click(object sender, EventArgs e)
        {
            if (ValidateSettings())
            {
                SaveSettings();
                LoadSettings();
            }
        }

        private void B_browser_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd_OneDrivePath = new FolderBrowserDialog();
            if (fbd_OneDrivePath.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                if (fbd_OneDrivePath.SelectedPath != null)
                {
                    txt_OneDriveFolder.Text = fbd_OneDrivePath.SelectedPath;
                }                
            }
            isDirty = true;
        }

        private void Txt_Interval_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txt_Interval.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter numbers only.");
                txt_Interval.Text = "";
            }
            isDirty = true;
        }

        private void Cb_LoadOnWindowsStartup_CheckedChanged(object sender, EventArgs e)
        {
            startWithWindowsChanged = true;
            isDirty = true;
        }

        //Version 1.3 -
        private void Cb_ShowInstructions_CheckedChanged(object sender, EventArgs e)
        {  
            isDirty = true;
        }
        //Version 1.3 +

        #endregion Form Controls

        #region Symbolic Link Form Controls & Functions

        private void B_addSymLink_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd_SymLinks = new FolderBrowserDialog();
            if (fbd_SymLinks.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                if (fbd_SymLinks.SelectedPath != null || fbd_SymLinks.SelectedPath != "")
                {
                    //Version 1.3 -
                    /*
                    string fAdd = fbd_SymLinks.SelectedPath + @"\";
                    string fAddName = System.IO.Path.GetDirectoryName(fAdd);
                    System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(fAddName);
                    fAddName = info.Name;

                    if (fAdd != "" && fAddName != "")
                    {
                        //if (ProcessIcon.fn.createSymbolicLink(@Properties.Settings.Default.OneDriveRootFolder + @"\" + @fAddName, @fAdd))
                        //{
                        //    Refresh_dgv();
                        //}
                    }
                    */

                    string fAdd = fbd_SymLinks.SelectedPath + @"\";
                    string fAddName;
                    System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(fAdd);
                    fAddName = info.Name;
                    //Avoid accidently linking C:\, D:\ etc.
                    if (info.Parent == null)
                    {
                        MessageBox.Show("You cannot select root folders like C:\\ drive etc. for this operation", "Wrong selection", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        if (fAdd != "" && info.Parent != null) //avoid accidently linking C:\, D:\ etc.
                        {
                            DialogResult dialogResult = MessageBox.Show(
                            "Do you want to copy the folder structure?" + Environment.NewLine + Environment.NewLine +
                            "- If you select Yes, the folder structure will be copied to OneDrive." + Environment.NewLine + "(Select this if you have the same folder structure in all your PCs!)" + Environment.NewLine + Environment.NewLine +
                            "- If you select No, the folder will be created in OneDrive root folder." + Environment.NewLine + "(Select this if you have different folder structure per PC!)" + Environment.NewLine + Environment.NewLine +
                            "- Click Cancel if you are not sure and want to rethink it"
                            , "Select type of link to create", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                            switch (dialogResult)
                            {
                                case DialogResult.Yes:
                                    info = new System.IO.DirectoryInfo(fAdd);
                                    string OneDriveFolder =  fAdd.Replace(@info.Root.ToString(), @Properties.Settings.Default.OneDriveRootFolder + @"\");
                                    string OneDriveFolderStructure = OneDriveFolder.Remove(OneDriveFolder.LastIndexOf(info.Name));
                                    if (!System.IO.Directory.Exists(OneDriveFolderStructure))
                                    {
                                        System.IO.Directory.CreateDirectory(OneDriveFolderStructure);
                                    }
                                    ProcessIcon.fn.createSymbolicLink(@OneDriveFolder, @fAdd);
                                    break;
                                case DialogResult.No:
                                    ProcessIcon.fn.createSymbolicLink(@Properties.Settings.Default.OneDriveRootFolder + @"\" + @fAddName, @fAdd);
                                    break;
                                case DialogResult.Cancel:
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    //Version 1.3 +
                }
            }
            //Version 1.3 -
            Refresh_dgv();
            //Version 1.3 +
        }

        private void B_DeleteSymLink_Click(object sender, EventArgs e)
        {
            //Version 1.3 -
            //if (dgv_SymLinks.SelectedRows != null)
            if ((dgv_SymLinks.SelectedRows != null) && (dgv_SymLinks.RowCount > 0))
            //Version 1.3 + 
            {
                string fDel = SymLinksTable.Rows[dgv_SymLinks.SelectedRows[0].Index].ItemArray[1].ToString();
                if (MessageBox.Show(
                    "Do you want to delete this Symbolic Link?" + Environment.NewLine + Environment.NewLine + 
                    "Warning: This will remove all files in the folder from OneDrive!" + Environment.NewLine + Environment.NewLine +
                    "To avoid file deletion, please stop syncing this folder first by changing OneDrive settings."
                    , "Delete confirmation",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    //Version 1.3 -
                    //if (ProcessIcon.fn.deleteSymbolicLink(@fDel))
                    //{
                    //    Refresh_dgv();
                    //}
                    ProcessIcon.fn.deleteSymbolicLink(@fDel);
                    //Version 1.3 +
                }
            }
            //Version 1.3 -
            Refresh_dgv();
            //Version 1.3 +
        }

        private void B_refreshSymLinks_Click(object sender, EventArgs e)
        {
            Refresh_dgv();
        }

        private void Refresh_dgv()
        {
            SymLinksTable = new DataTable();
            SymLinksTable = ProcessIcon.fn.getOneDriveForSymLinks();
            //Version 1.3 -
            //if (SymLinksTable.Rows.Count > 0)
            //{
            //Version 1.3 +
            dgv_SymLinks.DataSource = SymLinksTable;
            dgv_SymLinks.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv_SymLinks.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv_SymLinks.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv_SymLinks.Refresh();
            //} //Version 1.3 -+
        }

        #endregion Symbolic Link Form Controls & Functions

    }
}
