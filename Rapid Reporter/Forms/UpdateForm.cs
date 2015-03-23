using System;
using System.Windows.Forms;

namespace Rapid_Reporter.Forms
{
    public partial class UpdateForm : Form
    {
        internal Version ServerVersion;
        internal UpdateChosen Choice = UpdateChosen.Later;

        public UpdateForm()
        {
            InitializeComponent();
        }

        private void UpdateForm_Load(object sender, EventArgs e)
        {
            serverver.Text = (ServerVersion == null) ? "Cannot retrieve version" : ServerVersion.ToString();
            myver.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
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
