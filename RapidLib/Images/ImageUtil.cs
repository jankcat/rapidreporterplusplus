using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace RapidLib.Images
{
    public class ImageUtil
    {
        public static string BuildStringFromImage(Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                var imageBytes = ms.ToArray();
                var base64String = Convert.ToBase64String(imageBytes, Base64FormattingOptions.None);
                return base64String;
            }
        }

        public static Image BuildImageFromString(string base64String)
        {
            var imageBytes = Convert.FromBase64String(base64String);
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                ms.Write(imageBytes, 0, imageBytes.Length);
                var image = Image.FromStream(ms, true);
                return image;
            }
        }
    }
}
