namespace RapidLib.HTML
{
    public static class HtmlEmbedder
    {
        public static string BuildSessionRow_Img(int imgCount, string image)
        {
            return
                string.Format(
                    "<a href=\"#\" onclick=\"ShowImgEle('imgdiv{0}', 'imgbig{0}', 'imgsmall{0}');\"><img id='imgsmall{0}' src=\"\"></a><script>var imgSrcData{0} = \"data:image/png;base64,{1}\"; document.getElementById(\"imgsmall{0}\").src = imgSrcData{0};</script>{2}",
                    imgCount, image, "&nbsp;");
        }

        public static string BuildPopUp_Img(int imgCount)
        {
            return
                string.Format(
                    "<div id='imgdiv{0}' style=\"{1}\" onclick=\"HideImgEle('imgdiv{0}')\">Click here to hide...<br><br><img id='imgbig{0}'></div>",
                    imgCount, "position: absolute; top: 40px; left: 5px; right: 0; bottom: 0; display:none;");
        }

        public static string BuildSessionRow_PTNote(int noteCount)
        {
            return
                string.Format("<a href=\"#\" onclick=\"ShowPlaintextNote('ptndiv{0}');\">Click to show note...</a>{1}",
                    noteCount, "&nbsp;");
        }

        public static string BuildPopUp_PTNote(int noteCount, string plainTextNote)
        {
            return
                string.Format(
                    "<div id='ptndiv{0}' style=\"{1}\"><div><a href=\"#\" onclick=\"HidePlaintextNote('ptndiv{0}')\">Click here to hide...</a></div><pre>{2}</pre></div>",
                    noteCount, "position: absolute; top: 40px; left: 5px; right: 0; bottom: 0; display:none;",
                    plainTextNote);
        }
    }
}
