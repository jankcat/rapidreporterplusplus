using System;
using System.Windows.Forms;

namespace RapidLib.Forms
{
    public partial class PlainTextNoteForm : Form
    {
        private readonly Session _session;
        public PlainTextNoteForm(Session session)
        {
            InitializeComponent();
            _session = session;
        }

        private void clearNoteButton_Click(object sender, EventArgs e)
        {
            noteTextBox.Clear();
            noteTextBox.Focus();
        }

        private void PlainTextNoteForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void saveAndCloseButton_Click(object sender, EventArgs e)
        {
            var noteText = noteTextBox.Text;
            noteTextBox.Clear();
            if (string.IsNullOrWhiteSpace(noteText))
            {
                Close();
                return;
            }
            var safeNote = System.Net.WebUtility.HtmlEncode(noteText);

            var note = new Note
            {
                Type = NoteTypes.PlainTextNote,
                Time = DateTime.Now,
                Contents = safeNote
            };

            _session.AddNote(note);
            Close();
        }

        private void PlainTextNoteForm_Load(object sender, EventArgs e)
        {
            noteTextBox.Focus();
        }
    }
}
