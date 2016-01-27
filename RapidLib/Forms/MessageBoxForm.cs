using System;
using System.Windows.Forms;

namespace RapidLib.Forms
{
    public partial class MessageBoxForm : Form
    {
        private readonly string _message;
        private readonly string _title;
        public MessageBoxForm(string message, string title = "")
        {
            InitializeComponent();
            _message = message ?? "";
            _title = title ?? "";
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MessageBoxForm_Load(object sender, EventArgs e)
        {
            messageLabel.Text = _message;
            Text = _title;
        }

        public static void Alert(string message, string title = "")
        {
            var msg = new MessageBoxForm(message, title);
            msg.Show();
        }
    }
}
