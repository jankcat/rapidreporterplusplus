namespace Rapid_Reporter.HTML
{
	public static class Htmlstrings
	{
		// Dynamic
		public static string HtmlTitle = ": Session Report";

		// Static values
		//  the letter at the beginning of the var name is to hint about their order
		public static string AHtmlHeader = "";
		public static string CJavascript = "";
		public static string DStyle = "";
		public static string GHtmlBody1 = "";
        public static string ToggleAuto = "";
		public static string JHtmlBodytable1 = "";
		public static string MHtmlBodytable2 = "";
		public static string PHtmlFooter = "";
		static Htmlstrings()
		{

			//****************************************/
			AHtmlHeader = @"<html>
    <head>
        <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"">
        <title>";
            //****************************************/
            CJavascript = HtmlTitle + @"        </title>";

			//****************************************/
			DStyle = @"
        <style>
            html *
            {
                font-family: Verdana !important;
                font-size: 11px;
            }
            .aroundtable {font-family: Verdana; font-size: 11px; }
            H1 {text-align: center; font-family: Verdana;}
            H5 {text-align: center; font-family: Verdana; font-weight: normal;} 
            table {margin-left: auto; margin-right: auto; min-width:700px; width:80%; border-collapse: collapse; border: 1px solid black;}
            table tr img {max-width: 350px; max-height: 250px; resize-scale: showall;}
            table tr td {padding: 2px;}

            table tr.Session {font-weight: bold; background: #FAFAFA;}
            table tr.Scenario {font-weight: bold; background: #FAFAFA;}
            table tr.Environment {font-weight: bold; background: #FAFAFA;}
            table tr.Versions {font-weight: bold; background: #FAFAFA;}

            table tr.Bug\/Issue {background: #FF4D4D;}
            table tr.Follow {background: #5CADFF;}
            table tr.Note {background: #FAFAFA;}
            table tr.Test {background: #FAFAFA;}
            table tr.Success {background: #80FF80;}
            table tr.Prerequisite {background: #FAFAFA;}
            table tr.Summary {background: #FAFAFA;}

            table tr.Screenshot {background: #FAFAFA;}
            table tr.PlainText {background: #FAFAFA;}

            table td.notetype {font-weight: bold; width:190px;}
            table td.timestamp {font-weight: bold; width:175px;}
        </style>";

			//****************************************/
			GHtmlBody1 = @"
        <script>
            function ShowImgEle(eleId, img64)
            {
	            var ele = document.getElementById(eleId);
                var eletable = document.getElementById('aroundtable');
	            eletable.style.display = ""none"";
	            ele.style.display = ""inline"";
	            ele.style.background = ""url('""+img64+""') no-repeat"";
	            ele.style.backgroundPosition = ""0px 15px"";
	            ele.style.textDecoration = ""underline"";
            }
            function HideImgEle(eleId)
            {
	            var ele = document.getElementById(eleId);
                var eletable = document.getElementById('aroundtable');
	            eletable.style.display = ""inline"";
	            ele.style.display = ""none"";
            }
            function ShowPlaintextNote(eleId)
            {
	            var ele = document.getElementById(eleId);
                var eletable = document.getElementById('aroundtable');
	            eletable.style.display = ""none"";
	            ele.style.display = ""inline"";
            }
            function HidePlaintextNote(eleId)
            {
	            var ele = document.getElementById(eleId);
                var eletable = document.getElementById('aroundtable');
	            eletable.style.display = ""inline"";
	            ele.style.display = ""none"";
            }
        </script>
    </head>
    <body>
        <div id=""allbody"">";

            //****************************************/
            ToggleAuto = @"";

			//****************************************/
			JHtmlBodytable1 = @"
            <div id=""aroundtable"">
                <table border=""1"">
";

			//****************************************/
			MHtmlBodytable2 = @"
                </table>
            </div>
";

			//****************************************/
			PHtmlFooter = @"
        </div>
    </body>
</html>
";
		}
	}
}