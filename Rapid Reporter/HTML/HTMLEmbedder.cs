using System;
using System.IO;

// ReSharper disable EmptyGeneralCatchClause

namespace Rapid_Reporter.HTML
{
    internal static class HtmlEmbedder
    {
        const string Nbsp = "&nbsp;";
        const string ImgPopUpStyle = "position: absolute; top: 40px; left: 5px; right: 0; bottom: 0; display:none;";
        const string NotePopUpStyle = "position: absolute; top: 40px; left: 5px; right: 0; bottom: 0; display:none;";

        internal static string BuildSessionRow_Img(int imgCount, string imgFile)
        {
            var imgSrc = MakeImageSrcData(imgFile);
            return string.Format("<a href=\"#\" onclick=\"ShowImgEle('imgdiv{0}', '{1}');\"><img src=\"{1}\"></a>{2}", imgCount, imgSrc, Nbsp);
        }

        internal static string BuildPopUp_Img(int imgCount)
        {
            return string.Format("<div id='imgdiv{0}' style=\"{1}\" onclick=\"HideImgEle('imgdiv{0}')\">Click here to hide...</div>", imgCount, ImgPopUpStyle);
        }

        internal static string BuildSessionRow_PTNote(int noteCount)
        {
            return string.Format("<a href=\"#\" onclick=\"ShowPlaintextNote('ptndiv{0}');\">Click to show note...</a>{1}", noteCount, Nbsp);
        }

        internal static string BuildPopUp_PTNote(int noteCount, string noteFile)
        {
            var noteSrc = GetPlainTextNote(noteFile);
            return
                string.Format(
                    "<div id='ptndiv{0}' style=\"{1}\"><div><a href=\"#\" onclick=\"HidePlaintextNote('ptndiv{0}')\">Click here to hide...</a></div><pre>{2}</pre></div>",
                    noteCount, NotePopUpStyle, noteSrc);
        }

        private static string MakeImageSrcData(string filename)
        {
            try
            {
                var fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                var filebytes = new byte[fs.Length];
                fs.Read(filebytes, 0, Convert.ToInt32(fs.Length));
                var returnData = "data:image/png;base64," +
                       Convert.ToBase64String(filebytes, Base64FormattingOptions.None);
                fs.Close();
                try
                {
                    File.Delete(filename);
                }
                catch
                {
                    // supress delete errors like access denied etc
                }
                return returnData;
            }
            catch
            {
                // cannot read file or something i guess... screenshot must be dead.
                return "";
            }
        }
        private static string GetPlainTextNote(string filename)
        {
            try
            {
                var fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                var filebytes = new byte[fs.Length];
                fs.Read(filebytes, 0, Convert.ToInt32(fs.Length));
                var returnData = System.Text.Encoding.UTF8.GetString(filebytes);
                fs.Close();
                try
                {
                    File.Delete(filename);
                }
                catch
                {
                    // supress delete errors like access denied etc
                }
                return returnData;
            }
            catch
            {
                // cannot read file or something i guess... screenshot must be dead.
                return "";
            }
        }
    }
}
