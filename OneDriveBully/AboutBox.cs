using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using OneDriveBully.Properties;

namespace OneDriveBully
{
    partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();
            this.Text = $"About {AssemblyTitle}";
            this.labelProductName.Text = AssemblyProduct;
            this.labelVersion.Text = $"Version {AssemblyVersion}";
            this.labelCopyright.Text = AssemblyCopyright;
            this.labelCompanyName.Text = AssemblyCompany;
            this.textBoxDescription.Text = AssemblyDescription;
            this.linkLabelFacebook.Text = "http://www.facebook.com/OneDriveBully";
            this.labelContactEmail.Text = "odbully@outlook.com";
        }

        private void linkLabelFacebook_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo("http://facebook.com/OneDriveBully") { UseShellExecute = true });
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                var attributes =
                    Assembly.GetExecutingAssembly().GetCustomAttributes<AssemblyTitleAttribute>().ToArray();
                return attributes[0].Title;
            }
        }

        public string AssemblyVersion => Assembly.GetExecutingAssembly().GetName().Version?.ToString();

        public string AssemblyDescription
        {
            get
            {
                var attributes =
                    Assembly.GetExecutingAssembly().GetCustomAttributes<AssemblyDescriptionAttribute>().ToArray();
                return attributes[0].Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                var attributes =
                    Assembly.GetExecutingAssembly().GetCustomAttributes<AssemblyProductAttribute>().ToArray();
                return attributes[0].Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                var attributes =
                    Assembly.GetExecutingAssembly().GetCustomAttributes<AssemblyCopyrightAttribute>().ToArray();
                return attributes[0].Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                var attributes =
                    Assembly.GetExecutingAssembly().GetCustomAttributes<AssemblyCompanyAttribute>().ToArray();
                return attributes[0].Company;
            }
        }
        #endregion

    }
}