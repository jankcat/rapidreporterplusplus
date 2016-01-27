using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace RapidLib.Forms
{
    public partial class ScreenShotPreview : Form
    {
        public ScreenShotPreview()
        {
            InitializeComponent();
            var formRectangle = new Rectangle(RegUtil.ScreenShotPreviewX, RegUtil.ScreenShotPreviewY, Width, Height);
            var onScreen = Screen.AllScreens.Any(s => s.WorkingArea.IntersectsWith(formRectangle));
            if (onScreen)
            {
                StartPosition = FormStartPosition.Manual;
                Location = new Point(RegUtil.ScreenShotPreviewX, RegUtil.ScreenShotPreviewY);
                return;
            }
            StartPosition = FormStartPosition.WindowsDefaultLocation;
            RegUtil.ScreenShotPreviewX = Left;
            RegUtil.ScreenShotPreviewY = Top;
        }

        public void UpdateScreenshot(Bitmap image)
        {
            image = (Bitmap)ScaleImage(image, 350, 250);
            using (var s = new MemoryStream())
            {
                image.Save(s, ImageFormat.Bmp);
                screenshotbox.Size = new Size(Width, Height);
                screenshotbox.Image = Image.FromStream(s);
            }
            BringToFront();
            Focus();
        }

        private void ScreenShotPreview_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            if (image.Width < maxWidth && image.Height < maxHeight) return image;
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }

        private void ScreenShotPreview_Move(object sender, EventArgs e)
        {
            RegUtil.ScreenShotPreviewX = Left;
            RegUtil.ScreenShotPreviewY = Top;
        }
    }
}
