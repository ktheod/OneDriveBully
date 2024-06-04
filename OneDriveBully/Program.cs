using System;
using System.Linq;
using System.Windows.Forms;

namespace OneDriveBully
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        // A lot of help came from here: https://www.codeproject.com/articles/290013/formless-system-tray-application

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Version 1.4 - Check if the application is already running -
            if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1)
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            //Version 1.4 - Check if the application is already running +


            // Show the system tray icon.					
            using (ProcessIcon pi = new ProcessIcon())
            {
                pi.Display();

                // Make sure the application runs!
                Application.Run();               
            }
        }
    }
}
