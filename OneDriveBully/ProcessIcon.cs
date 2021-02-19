using System;
using System.Threading;
using System.Windows.Forms;
using OneDriveBully.Properties;

namespace OneDriveBully
{
    //Version 1.3 - Added MessageBox with instructions when the application is opened/closed for the first time
    public class ProcessIcon : IDisposable
	{
		public static NotifyIcon ni;
        public static MyFunctions fn;

        public ProcessIcon()
		{
			// Instantiate the NotifyIcon object.
			ni = new NotifyIcon();

            // Instantiate the MyFunctions object.
            fn = new MyFunctions();
        }

		// Displays the icon in the system tray.
		public void Display()
		{
			// Put the icon in the system tray and allow it react to mouse clicks.			
            ni.MouseDoubleClick += new MouseEventHandler(Ni_MouseDoubleClick);
			ni.Icon = Resources.StandardIcon;
			ni.Text = "OneDrive Bully";

			// Attach a context menu.
			ni.ContextMenuStrip = new ContextMenus().Create();

            // Initialize Application
            fn.initApp();
            ni.Visible = true;

            //Version 1.3 -
            if (Properties.Settings.Default.ShowInstructions)
            {
                MessageBox.Show("OneDrive Bully is now running on the system tray (bottom right icons on taskbar)." + Environment.NewLine +
                                Environment.NewLine +
                                "You can" + Environment.NewLine +
                                " - Double click to Bully on Demand" + Environment.NewLine +
                                " - Right click to show the Menu", "OneDrive Bully is running",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            //Version 1.3 +
        }

        void Ni_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Handle mouse button clicks.
            if (e.Button == MouseButtons.Left)
            {
                if (fn.UserDefinedSettingsExist)
                {
                    fn.bullyNow();
                }
                else
                {
                    fn.checkUserSettings();
                }
            }
        }

        // Releases unmanaged and - optionally - managed resources
        public void Dispose()
        {
            //On Close
            //Version 1.3 -
            if (Properties.Settings.Default.ShowInstructions)
            {
                Properties.Settings.Default.ShowInstructions = false;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();

                MessageBox.Show("Next time you run OneDrive Bully you can find the icon on the system tray (bottom right icons on taskbar).","OneDrive Bully", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //Version 1.3 +

            // When the application closes, this will remove the icon from the system tray immediately.
            ni.Dispose();
        }
    }
}