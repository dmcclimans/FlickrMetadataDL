using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FlickrMetadataDL
{
    public partial class FormProgress : Form
    {
        public FormProgress(string whyWeAreWaiting, DoWorkEventHandler work)
        {
            InitializeComponent();
            if (!String.IsNullOrWhiteSpace(whyWeAreWaiting))
            {
                this.Text = whyWeAreWaiting; // Show in title bar
            }
            backgroundWorker1.DoWork += work; // Event handler to be called in context of new thread.
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label1.Text = e.UserState as string;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void FormProgress_Load(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync(); // Tell worker to abort.
            btnCancel.Enabled = false;
        }
    }
}
