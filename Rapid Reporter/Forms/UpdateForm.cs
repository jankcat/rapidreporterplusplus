using System;
using System.Windows.Forms;

namespace Rapid_Reporter.Forms
{
    public partial class UpdateForm : Form
    {
        private Version _serverVersion;
        internal UpdateChosen Choice = UpdateChosen.Later;

        public UpdateForm()
        {
            InitializeComponent();
        }

        private void UpdateForm_Load(object sender, EventArgs e)
        {
            serverver.Text = (_serverVersion == null) ? "Cannot retrieve version" : _serverVersion.ToString();
            myver.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        internal void UpdateServerVersion(Version version)
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
