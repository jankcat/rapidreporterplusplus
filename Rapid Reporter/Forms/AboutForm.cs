using System;
using System.Windows.Forms;

namespace Rapid_Reporter.Forms
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            appName.Text = Application.ProductName;
            appVer.Text = Application.ProductVersion;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
