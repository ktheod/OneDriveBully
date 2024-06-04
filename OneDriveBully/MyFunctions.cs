using System;
using System.Windows.Forms;
using System.IO;
using System.Timers;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Data;
using OneDriveBully.Properties;
using System.Threading;
using System.Management;
using System.Linq;
using System.Collections.Generic;

namespace OneDriveBully
{
    //Version 1.3 - Changed temp file handling from create/delete to create/rename to keep clean OneDrive log and Recycle Bin
    //            - Changed  [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)] to [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    //

    public class MyFunctions
    {
        //User Settings Variables
        public bool UserDefinedSettingsExist;
        private string rootPath;
        //Version 1.3 -
        //private string fileName = @"\OneDriveBully_SyncTempFile.txt";
        private string fileName = "OneDriveBully_SyncTempFile";
        //Version 1.3 +

        //Timer Variables
        private System.Timers.Timer MyTimer;
        private Int32 timeRemaining;

        public void initApp()
        {
            initTimer();
            checkUserSettings();
            if (UserDefinedSettingsExist)
            {
                setTimerInterval(Properties.Settings.Default.TimerInterval);
            }
        }

        #region User Settings

        public bool checkUserSettings() //Version 1.1 - Bug Fix -+
        {
            //Check if user has updated the User Settings
            if (!Properties.Settings.Default.UserDefinedSettings)
            {
                // Show Settings Form
                SettingsForm _SettingsForm = new SettingsForm();
                _SettingsForm.ShowDialog();
            }

            // Check again
            Properties.Settings.Default.Reload();
            UserDefinedSettingsExist = Properties.Settings.Default.UserDefinedSettings;
            if (!UserDefinedSettingsExist)
            {
                WrongSettings();
                return false; //Version 1.1 - Bug Fix -+
            }
            else
            {
                //Check OneDrive root Path
                rootPath = @Properties.Settings.Default.OneDriveRootFolder + @"\";
                if (rootPath != null)
                {
                    if (!Directory.Exists(rootPath))
                    {
                        WrongSettings();
                        return false; //Version 1.1 - Bug Fix -+
                    }
                }

                //Check Timer
                if(Properties.Settings.Default.TimerInterval <1)
                {
                    WrongSettings();
                    return false; //Version 1.1 - Bug Fix -+
                }
            }
            return true; //Version 1.1 - Bug Fix -+
        }

        public void WrongSettings()
        {
            MessageBox.Show("Please update your settings.", "Settings Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            UpdateIconText(2);
            stopTimer();
        }

        #endregion User Settings

        #region Timer Related Functions

        public void initTimer()
        {
            MyTimer = new System.Timers.Timer();
            MyTimer.AutoReset = true;
        }

        public void setTimerInterval(Int32 newInterval)
        {
            if (newInterval <= 0)
            {
                WrongSettings();
            }
            else
            {
                timeRemaining = newInterval * 60 * 1000;
                MyTimer.Interval = 1 * 60 * 1000; //Check timer every 1 minute

                stopTimer();
                startTimer();
            }
        }

        public void startTimer()
        {
            MyTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            MyTimer.Enabled = true;

            UpdateIconText(0);
        }

        public void stopTimer()
        {
            MyTimer.Enabled = false;
            MyTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent);
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            timeRemaining -= 1 * 60 * 1000;
            if (timeRemaining <= 0)
            {
                bullyNow();
            }
            UpdateIconText(0);
        }

        private void UpdateIconText(int ProgressStatus)
        {
            switch (ProgressStatus)
            {
                case 0:
                    ProcessIcon.ni.Icon = Resources.StandardIcon;
                    ProcessIcon.ni.Text = "OneDrive Bully" + " - Next Sync in: "
                    + (timeRemaining / 60 / 1000).ToString() + " minutes";
                    break;
                case 1:
                    ProcessIcon.ni.Icon = Resources.IconProgress;
                    ProcessIcon.ni.Text = "OneDrive Bullying in progress.";
                    break;
                case 2:
                    ProcessIcon.ni.Icon = Resources.IconError;
                    ProcessIcon.ni.Text = "Timer Stopped. Check Settings.";
                    break;
                default:
                    ProcessIcon.ni.Icon = Resources.StandardIcon;
                    ProcessIcon.ni.Text = "OneDrive Bully" + " - Next Sync in: "
                    + (timeRemaining / 60 / 1000).ToString() + " minutes";
                    break;
            }

        }

        #endregion Timer Related Functions

        #region Bully Function

        public void bullyNow()
        {
            UpdateIconText(1);
            //Version 1.1 - Bug Fix -
            //checkUserSettings();
            if (checkUserSettings())
            {
                //Version 1.1 - Bug Fix +

                //Version 1.3 -
                //if (File.Exists(rootPath + fileName))
                //{
                //    File.Delete(rootPath + fileName);
                //}

                //File.Create(rootPath + fileName).Close();
                //Thread.Sleep(5000);
                //if (File.Exists(rootPath + fileName))
                //{
                //    File.Delete(rootPath + fileName);
                //}

                //Get all files that start with 'OneDriveBully_SyncTempFile'
                string[] oldFiles = Directory.GetFiles(@rootPath, fileName + "*.txt");
                if ((oldFiles.Length > 1) || (oldFiles.Length == 0))
                {
                    //Remove old files - in case there are leftovers from previous syncs
                    foreach (string oldFile in oldFiles)
                    {
                        File.Delete(oldFile);
                    }

                    //Create new file
                    File.Create(rootPath + fileName + DateTime.Now.ToString("-yyyyMMdd-hhmmss") + ".txt").Close();
                }
                else
                {
                    //Only 1 file exists (normal case), rename it with latest datetime
                    File.Move(oldFiles[0], rootPath + fileName + DateTime.Now.ToString("-yyyyMMdd-hhmmss") + ".txt");
                }
                //Version 1.3 +

                Thread.Sleep(5000);
                setTimerInterval(Properties.Settings.Default.TimerInterval);
            } //Version 1.1 - Bug Fix -+
        }

        #endregion Bully Function

        #region Windows Registry Related Functions 

        public void startOnWindowsStartup(bool register)
        {
            if (register)
            {
                RegistryKey add = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                add.SetValue(@"OneDriveBully", "\"" + Application.ExecutablePath.ToString() + "\"");
            }
            else
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                {
                    if (key != null)
                    {
                        key.DeleteValue(@"OneDriveBully");
                    }
                }
            }
        }

        #endregion Windows Registry Related Functions 

        #region Symbolic Links Related Functions
        //Version 1.4 - Replaced CreateSymbolicLink and deleteSymbolicLink - 
        public bool CreateSymbolicLink(string linkPath, string targetPath)
        {
            if (!IsElevated)
            {
                MessageBox.Show("Please run the application as Administrator to create symbolic links.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try
            {
                SymbolicLinks.CreateSymlink(@linkPath, @targetPath, true);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error creating symbolic link: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        public bool deleteSymbolicLink(string linkPath, bool showErrors = true)
        {
            if(!IsElevated)
            {
                MessageBox.Show("Please run the application as Administrator to delete symbolic links.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try
            {
                SymbolicLinks.DeleteSymlink(@linkPath,true);
            }
            catch (Exception ex)
            {
                if (showErrors)
                {
                    MessageBox.Show("Error deleting symbolic link: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
            return true;
        }
        static bool IsElevated
        {
            get
            {
                return WindowsIdentity.GetCurrent().Owner
                  .IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid);
            }
        }

        //Version 1.4 - Replaced CreateSymbolicLink and deleteSymbolicLink  +

        public DataTable getOneDriveSymLinks()
        {
            DataTable SymLinksTable = new DataTable();
            SymLinksTable.Columns.Add("Folder Path", typeof(string));
            SymLinksTable.Columns.Add("OneDrive Path", typeof(string));
            SymLinksTable.Columns.Add("Folder Name", typeof(string));

            Properties.Settings.Default.Reload();

            List<string> dirs = new List<string>();
            try
            {
                dirs = GetDirectories(Properties.Settings.Default.OneDriveRootFolder);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error getting OneDrive folders: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return SymLinksTable;
            }

            foreach (string subDir in dirs)
            {
                if (subDir.Length < 260)
                {
                    if (SymbolicLinks.IsDirectorySymbolicLink(subDir))
                    {
                        //string targetF = sl.GetSymLinkTarget(@subDir);
                        try
                        {
                            string targetF = SymbolicLinks.GetSymbolicLinkTarget(@subDir);
                            Console.WriteLine(subDir.ToString() + " - " + true.ToString()
                                              + " ==> " + @targetF);
                            SymLinksTable.Rows.Add(@targetF, @subDir, getFolderName(subDir));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(subDir.ToString() + " - " + false.ToString()
                                                                             + " ==> " + ex.Message);
                        }
                    }
                }
            }
            return SymLinksTable;
        }
        private string getFolderName(string path)
        {
            string OneDrivePath = Properties.Settings.Default.OneDriveRootFolder;    
            return path.Remove(0, OneDrivePath.Length + 1);
        }

        public static List<string> GetDirectories(string path, string searchPattern = "*",
        SearchOption searchOption = SearchOption.AllDirectories)
        {
            if (searchOption == SearchOption.TopDirectoryOnly)
                return Directory.GetDirectories(path, searchPattern).ToList();

            var directories = new List<string>(GetDirectories(path, searchPattern));

            for (var i = 0; i < directories.Count; i++)
                directories.AddRange(GetDirectories(directories[i], searchPattern));

            return directories;
        }

        private static List<string> GetDirectories(string path, string searchPattern)
        {
            try
            {
                return Directory.GetDirectories(path, searchPattern).ToList();
            }
            catch (UnauthorizedAccessException)
            {
                return new List<string>();
            }
            catch(Exception ex)
            {
                return new List<string>();
            }
        }

        #endregion Symbolic Links Related Functions
    }
}
