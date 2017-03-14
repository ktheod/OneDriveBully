using System;
using System.Diagnostics;
using System.Windows.Forms;
using OneDriveBully.Properties;
using System.Drawing;

namespace OneDriveBully
{
	class ContextMenus
	{
		bool isAboutLoaded = false;
        bool isSettingsFormLoaded = false;

		public ContextMenuStrip Create()
		{
			// Add the default menu options.
			ContextMenuStrip menu = new ContextMenuStrip();
			ToolStripMenuItem item;
			ToolStripSeparator sep;

			// Bully Now.
			item = new ToolStripMenuItem();
			item.Text = "Bully Now";
			item.Click += new EventHandler(Bully_Click);
			menu.Items.Add(item);

			// Settings.
			item = new ToolStripMenuItem();
			item.Text = "Settings";
			item.Click += new EventHandler(Settings_Click);
			menu.Items.Add(item);

            // About.
            item = new ToolStripMenuItem();
            item.Text = "About";
            item.Click += new EventHandler(About_Click);
            menu.Items.Add(item);

            // Separator.
            sep = new ToolStripSeparator();
			menu.Items.Add(sep);

			// Exit.
			item = new ToolStripMenuItem();
			item.Text = "Exit";
			item.Click += new System.EventHandler(Exit_Click);
			menu.Items.Add(item);

			return menu;
		}

		void Bully_Click(object sender, EventArgs e)
		{
            ProcessIcon.fn.bullyNow();
		}
        void Settings_Click(object sender, EventArgs e)
        {
            if (!isSettingsFormLoaded)
            {
                isSettingsFormLoaded = true;
                SettingsForm _SettingsForm = new SettingsForm();
                _SettingsForm.ShowDialog();
                isSettingsFormLoaded = false;
            }
        }
        void About_Click(object sender, EventArgs e)
		{
			if (!isAboutLoaded)
			{
				isAboutLoaded = true;
				new AboutBox().ShowDialog();
				isAboutLoaded = false;
			}
        }
		void Exit_Click(object sender, EventArgs e)
		{
            Application.Exit();
		}
	}
}