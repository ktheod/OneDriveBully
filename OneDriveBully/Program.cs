using System;
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
