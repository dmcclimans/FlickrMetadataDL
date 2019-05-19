using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

namespace FlickrMetadataDL
{
    public partial class FormAbout : Form
    {
        private Settings Settings { get; set; }
        public FormAbout(Settings settings)
        {
            Settings = settings;
            InitializeComponent();
        }

        private void FormAbout_Load(object sender, EventArgs e)
        {
            lblVersion.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            if (Settings.FormAboutLocation.X != 0 ||
                 Settings.FormAboutLocation.Y != 0)
            {
                this.Location = Settings.FormAboutLocation;
            }
        }

        private void FormAbout_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.FormAboutLocation = this.Location;
        }
    }
}
