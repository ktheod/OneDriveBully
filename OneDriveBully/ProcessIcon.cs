using System;
using System.Windows.Forms;
using OneDriveBully.Properties;

namespace OneDriveBully
{
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
            ni.MouseDoubleClick += new MouseEventHandler(ni_MouseDoubleClick);
			ni.Icon = Resources.StandardIcon;
			ni.Text = "OneDrive Bully";

			// Attach a context menu.
			ni.ContextMenuStrip = new ContextMenus().Create();

            // Initialize Application
            fn.initApp();
            ni.Visible = true;
        }
         
        void ni_MouseDoubleClick(object sender, MouseEventArgs e)
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
            // When the application closes, this will remove the icon from the system tray immediately.
            ni.Dispose();
        }
    }
}