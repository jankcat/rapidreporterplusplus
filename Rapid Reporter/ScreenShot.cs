using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Rapid_Reporter.Forms;

// Code adapted from:
// http://kseesharp.blogspot.com/2008/11/c-capture-screenshot.html
// Sent email requesting license to adapt the code and permission to refer to his link.
//  There were many changes to the code, but still good to keep credit for original.
//  Althought similar code appears in so many places thet it is hard to point who is the original. Probably Microsoft :).

namespace Rapid_Reporter
{
    class ScreenShot
    {
        [DllImport("gdi32.dll")]
        static extern bool BitBlt(IntPtr hdcDest, int xDest, int yDest, int wDest, int hDest, IntPtr hdcSource, int xSrc, int ySrc, CopyPixelOperation rop);
        [DllImport("user32.dll")]
        static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDc);
        [DllImport("gdi32.dll")]
        static extern IntPtr DeleteDC(IntPtr hDc);
        [DllImport("gdi32.dll")]
        static extern IntPtr DeleteObject(IntPtr hDc);
        [DllImport("gdi32.dll")]
        static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);
        [DllImport("gdi32.dll")]
        static extern IntPtr CreateCompatibleDC(IntPtr hdc);
        [DllImport("gdi32.dll")]
        static extern IntPtr SelectObject(IntPtr hdc, IntPtr bmp);
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr ptr);

        public bool Canceled;

        public Bitmap CaptureSnippet()
        {
            int minX = 0, minY = 0, maxX = 0, maxY = 0;
            foreach (var oneScreen in Screen.AllScreens)
            {
                Logger.Record("\t[CaptureSnippet]: AllScreens[]: SNIPPET" + oneScreen.Bounds.ToString(), "ScreenShot", "info");
                minX = Math.Min(minX, oneScreen.Bounds.Left);
                minY = Math.Min(minY, oneScreen.Bounds.Top);
                maxX = Math.Max(maxX, oneScreen.Bounds.Width + oneScreen.Bounds.X);
                maxY = Math.Max(maxY, oneScreen.Bounds.Height + oneScreen.Bounds.Y);
            }
            maxY = Math.Abs(maxY) + Math.Abs(minY);
            maxX = Math.Abs(maxX) + Math.Abs(minX);

            var snipForm = new SnippetForm
                {
                    Top = minY,
                    Left = minX,
                    Width = maxX,
                    Height = maxY,
                    ScreenStartImage = CaptureScreenShot()
                };
            snipForm.ShowDialog();
            if (snipForm.Cancelled || snipForm.Snippet == null) Canceled = true;
            return snipForm.Snippet ?? new Bitmap(1, 1);
        }

        public Bitmap CaptureScreenShot()
        {
            Logger.Record("[CaptureScreenshot]: Will take a screenshot of the monitor.", "ScreenShot", "info");
            int minX = 0, minY = 0, maxX = 0, maxY = 0;

            /*

             * Composite desktop calculations for multiple monitors

                A, B            E, B                     ||                  E, B
                  +--------------+                       ||   *              +---------------+ D, B
                  |              |E, G                   ||                  |               |
                  |              +---------------+ D, G  ||   +--------------+ E, C          |
                  |              |               |       ||   |A, C          |               |
                  |              |               |       ||   |              |               |
                  |              |               |       ||   |              |               |
                  +--------------+ E, C          |       ||   |              +---------------+ D, G
                A, C             |               |       ||   |              |E, G
                                 +---------------+ D, F  ||   +--------------+               *
                                E, F                     ||   A, F           E, F

             * We capture from "A, B" to "D, F".
             *   That is, we look for the minimum X,Y coordinate first.
             *   Then we look for the largest width,height.
 
            */

            foreach (var oneScreen in Screen.AllScreens)
            {
                Logger.Record("\t[CaptureScreenshot]: AllScreens[]: " + oneScreen.Bounds.ToString(), "ScreenShot", "info");
                minX = Math.Min(minX, oneScreen.Bounds.Left);
                minY = Math.Min(minY, oneScreen.Bounds.Top);
                maxX = Math.Max(maxX, oneScreen.Bounds.Width + oneScreen.Bounds.X);
                maxY = Math.Max(maxY, oneScreen.Bounds.Height + oneScreen.Bounds.Y);
            }
            var fullBounds = new Rectangle(minX, minY, maxX - minX, maxY - minY);
            Logger.Record("[CaptureScreenshot]: fullScreen[]: " + fullBounds, "ScreenShot", "info");

            var hDesk = GetDesktopWindow();
            var hSrce = GetWindowDC(hDesk);
            var hDest = CreateCompatibleDC(hSrce);
            // Our bitmap will have the size of the composite screenshot
            var hBmp = CreateCompatibleBitmap(hSrce, fullBounds.Width, fullBounds.Height);
            var hOldBmp = SelectObject(hDest, hBmp);
            // We write on coordinate 0,0 of the bitmap buffer, of course. But we write the the fullBoundsX,Y pixels.
            BitBlt(hDest, 0, 0, fullBounds.Width, fullBounds.Height, hSrce, fullBounds.X, fullBounds.Y, CopyPixelOperation.SourceCopy | CopyPixelOperation.CaptureBlt);
            var bmp = Image.FromHbitmap(hBmp);
            SelectObject(hDest, hOldBmp);
            DeleteObject(hBmp);
            DeleteDC(hDest);
            ReleaseDC(hDesk, hSrce);
            Logger.Record("[CaptureScreenshot]: BMP object ready, returning it to calling function", "ScreenShot", "info");
            return (bmp);
        }

        
    }
}