using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace Rapid_Reporter.Forms
{
    public partial class SnippetForm : Form
    {
        public bool IsAlive = true;
        public Bitmap Snippet;
        public Bitmap ScreenStartImage;

        int _startX;
        int _startY;
        int _endX;
        int _endY;

        public Pen SelectPen;
        bool _start;

        public bool Cancelled;

        public SnippetForm()
        {
            InitializeComponent();
        }

        private void CloseForm()
        {
            Close();
            IsAlive = false; 
        }

        private void SnippetForm_Load(object sender, EventArgs e)
        {
            if (ScreenStartImage == null) { 
                CloseForm();
                return;
            }
            using (var s = new MemoryStream())
            {
                ScreenStartImage.Save(s, ImageFormat.Bmp);
                pictureBox1.Size = new Size(Width, Height);
                pictureBox1.Image = Image.FromStream(s);
            }
            Cursor = Cursors.Cross;
            BringToFront();
            Focus();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (_start) return;
            _startX = e.X;
            _startY = e.Y;
            SelectPen = new Pen(Color.Red, 1) {DashStyle = DashStyle.DashDotDot};
            pictureBox1.Refresh();
            _start = true;
            WindowState = FormWindowState.Normal;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (pictureBox1.Image == null) return;
            if (!_start) return;
            pictureBox1.Refresh();
            pictureBox1.CreateGraphics().DrawRectangle(SelectPen, Math.Min(e.X, _startX), Math.Min(e.Y, _startY), Math.Abs(e.X - _startX), Math.Abs(e.Y - _startY));
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_start) return;
            if (pictureBox1.Image == null) return;
            pictureBox1.Refresh();
            pictureBox1.CreateGraphics()
                       .DrawRectangle(SelectPen, Math.Min(e.X, _startX), Math.Min(e.Y, _startY), Math.Abs(e.X - _startX),
                                      Math.Abs(e.Y - _startY));
            _start = false;
            _endX = e.X;
            _endY = e.Y;
            SaveSnippet();
            CloseForm();
        }

        private void SaveSnippet()
        {
            var top = Math.Min(_endY, _startY);
            var left = Math.Min(_endX, _startX);
            var width = Math.Abs(_endX - _startX);
            var height = Math.Abs(_endY - _startY);
            if (width <= 0) return;
            var rect = new Rectangle(left, top, width, height);
            var originalImage = new Bitmap(pictureBox1.Image, pictureBox1.Width, pictureBox1.Height);
            var img = new Bitmap(width, height);
            var g = Graphics.FromImage(img);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.DrawImage(originalImage, 0, 0, rect, GraphicsUnit.Pixel);
            Snippet = img;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData != Keys.Escape)
                return base.ProcessCmdKey(ref msg, keyData);
            Cancel();
            return true;
        }

        internal void Cancel()
        {
            Cancelled = true;
            CloseForm();
        }

        private void SnippetForm_Shown(object sender, EventArgs e)
        {
            Activate();
        }
    }
}
