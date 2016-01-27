using System;
using System.Reflection;
using System.Windows.Forms;

namespace RapidLib.Forms
{
    public partial class UpdateForm : Form
    {
        private Version _serverVersion;
        public UpdateChosen Choice = UpdateChosen.Later;

        public UpdateForm()
        {
            InitializeComponent();
        }

        private void UpdateForm_Load(object sender, EventArgs e)
        {
            serverver.Text = (_serverVersion == null) ? "Cannot retrieve version" : _serverVersion.ToString();
            myver.Text = Assembly.GetEntryAssembly().GetName().Version.ToString();
        }

        public void UpdateServerVersion(Version version)
        {
            _serverVersion = version;
            serverver.Text = (_serverVersion == null) ? "Cannot retrieve version" : _serverVersion.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Choice = UpdateChosen.Update;
            Close();
        }

        private void btnIgnore_Click(object sender, EventArgs e)
        {
            Choice = UpdateChosen.Skip;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Choice = UpdateChosen.Later;
            Close();
        }
    }
}
