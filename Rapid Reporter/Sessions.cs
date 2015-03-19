using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Rapid_Reporter.HTML;
using MessageBox = System.Windows.MessageBox;
// ReSharper disable EmptyGeneralCatchClause

namespace Rapid_Reporter
{
    class Session
    {
        /** Variables **/
        /***************/
        
        // This is configurable from inside the application:
        // Session characteristics:
        public DateTime StartingTime;   // Time started, starts when moving from 'charter' to 'notes'.
        public int Duration = 90 * 60;  // Duration, in seconds (default is 90 min, can be changed in runtime).
        private const string ColumnHeaders = "Time,Type,Content"; // Consider adding sequencial number?

        // Session data:
        public string ScenarioId = "";     // Session objective. Configured in runtime.
        public string Tester = "";      // Tester's name. Configured in runtime.
        public string Charter = "";      // Configured in runtime.
        public string Environment = "";      // Configured in runtime.
        public string Versions = "";      // Configured in runtime.
        // The types of comments. This can be overriden from command line, so every person can use his own terminology or language
        public string[] NoteTypes = new[] { "Prerequisite", "Test", "Success", "Bug/Issue", "Note", "Follow Up", "Summary" };

        // Session files:
        public string WorkingDir = Directory.GetCurrentDirectory() + @"\";  // File to write the session to
        private string _sessionFile;      // File to write the session to
        private string _sessionFileFull;  // workingDir + sessionFile
        public string SessionNote = "";         // Latest note only

        // Session State Based Behavior:
        //  The application iterates: tester, charter, notes.
        //  This is done in this way in case we have to add more stages... But the stages are not moved by  number or placement, they're chosen directly.
        public enum SessionStartingStage { Tester, ScenarioId, Charter, Environment, Versions, Notes }; // Tester == tester's name. Charter == session charter. Notes == all the notes of different note types.
        public SessionStartingStage CurrentStage = SessionStartingStage.Tester; // This is used only in the beginning, in order to receive the tester name and charter text

        /** Sessions **/
        /**************/

        // Start Session and Close Session prepare/finalize the log file
        public void StartSession()
        {
            Logger.Record("[StartSession]: Session configuration starting", "Session", "info");

            StartingTime = DateTime.Now; // The time the session started is used for many things, like knowing the session file name
            _sessionFile = StartingTime.ToString("yyyyMMdd_HHmmss") + ".csv";
            _sessionFileFull = WorkingDir + _sessionFile; // All files should be written to a working directory -- be it current or not.
            SaveToSessionNotes(ColumnHeaders + "\n"); // Headers of the notes table
            //UpdateNotes("Reporter Tool Version", System.Windows.Forms.Application.ProductVersion);
            UpdateNotes("Session Reporter", Tester);
            UpdateNotes("Scenario ID", ScenarioId);
            UpdateNotes("Session Charter", Charter);
            UpdateNotes("Environment", Environment);
            UpdateNotes("Versions", Versions);
        }
        public void CloseSession() // Not closing directly, we first finalize the session
        {
            Logger.Record("[CloseSession]: Session closing...", "Session", "info");

            // Why this if? We will only add the 'end session' note if we were past the charter step.
            if (!String.Equals(Versions, ""))
            {
                TimeSpan duration = DateTime.Now - StartingTime;
                UpdateNotes("Session End. Duration",
                            duration.Hours.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0') + ":" +
                            duration.Minutes.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0') + ":" +
                            duration.Seconds.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'));
                MessageBox.Show(
                "RapidReporter++ will now convert your test session into a lovely HTML file. You will be alerted in a few seconds when this process is complete.",
                "Converting to HTML File");
                Logger.Record("[CloseSession]: Starting csv to html method...", "Session", "info");
                Csv2Html(_sessionFileFull, false);
            }

            Logger.Record("[CloseSession]: ...Session closed", "Session", "info");
        }

        /** Notes **/
        /***********/
        // Notes are always saved on file, not only when program exists (so no data loss in case of crash)

        // UpdateNotes: There are two overloads: One receives all strings (custom messages), the other an int (typed messages)
        public void UpdateNotes(int type, string note, string screenshot, string rtfNote)
        {
            UpdateNotes(NoteTypes[type], note, screenshot, rtfNote);
            Logger.Record("[UpdateNotes isss]: Note added to session log. Attachments: (" + (screenshot.Length > 0).ToString() + " | " + (rtfNote.Length > 0).ToString() + ")", "Session", "info");
        }
        public void UpdateNotes(string type, string note, string screenshot = "", string rtfNote = "")
        {
            SessionNote = DateTime.Now + "," + type + ",\"" + note + "\"," + rtfNote + "\n";
            SaveToSessionNotes(SessionNote);
            Logger.Record("[UpdateNotes ss]: Note added to session log (" + screenshot + ", " + rtfNote + ")", "Session", "info");
        }
        // Save all notes on file, after every single note
        private void SaveToSessionNotes(string note)
        {
            Logger.Record("[SaveToSessionNotes]: File will be updated and saved to " + _sessionFile, "Session", "info");
            bool exDrRetry;

            do
            { exDrRetry = false;
                try
                {
                    File.AppendAllText(_sessionFileFull, note, System.Text.Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    Logger.Record("\t[SaveToSessionNotes]: EXCEPTION reached - Session Note file could not be saved (" + _sessionFile + ")", "Session", "error");
                    exDrRetry = Logger.FileErrorMessage(ex, "SaveToSessionNotes", _sessionFile);
                }
            } while (exDrRetry);
        }

        //TODO: Make this better...
        public void Csv2Html(string csvFile, bool relativePath)
        {
            Logger.Record("[CSV2HTML]: HTML Report building", "Session", "info");
            bool exDrRetry;
            var htmlFileBufferPopups = "";

            // Find out if we are relative or not. Make us not relative
            var csvFileFull = relativePath ? WorkingDir + csvFile : csvFile;

            // format the file name for the save box and show it
            var htmlFileShort = Path.GetFileNameWithoutExtension(csvFile);
            htmlFileShort = string.Format("{0} - {1}.htm", htmlFileShort, ScenarioId);

            //Remove any invalid characters
            var invalidChars = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            htmlFileShort = invalidChars.Aggregate(htmlFileShort, (current, c) => current.Replace(c.ToString(CultureInfo.InvariantCulture), ""));

            // if they canceled, we go with the default
            var htmlFileFull = WorkingDir + htmlFileShort;
            var saveDlg = new SaveFileDialog { DefaultExt = "htm", FileName = htmlFileShort, InitialDirectory = WorkingDir };
            if (saveDlg.ShowDialog() == DialogResult.OK)
                htmlFileFull = saveDlg.FileName;

            do
            {
                exDrRetry = false;
                
                try
                {
                    var imgCount = 0;
                    var ptnCount = 0;
                    var t = "th";
                    Htmlstrings.HtmlTitle = _sessionFile;
                    File.Delete(htmlFileFull);
                    var htmlFileBuffer = string.Format("{0}{1}{2}{3}{4}<h1>{1}: Session Report</h1><!--[if IE]><h5>For best results, use Chrome or Firefox.</h5><![endif]-->{5}{6}",
                                                       Htmlstrings.AHtmlHeader, ScenarioId, Htmlstrings.CJavascript,
                                                       Htmlstrings.DStyle, Htmlstrings.GHtmlBody1,
                                                       Htmlstrings.ToggleAuto, Htmlstrings.JHtmlBodytable1);

                    foreach (var line in File.ReadAllLines(csvFileFull, System.Text.Encoding.UTF8))
					{
                        if ("" == line) continue;
                        var note = ""; 
                        var thisLine = line.Split(',');
                        if (thisLine.Length > 2)
                        {
                            note = thisLine[2].Replace("\"", "");
                            switch (thisLine[1])
                            {
                                case @"Screenshot":
                                    if (!File.Exists(WorkingDir + note)) break;
                                    note = HtmlEmbedder.BuildSessionRow_Img(imgCount, WorkingDir + note);
                                    htmlFileBufferPopups += HtmlEmbedder.BuildPopUp_Img(imgCount);
                                    imgCount++;
                                    break;
                                case @"PlainText Note":
                                    if (!File.Exists(WorkingDir + note)) break;
                                    htmlFileBufferPopups += HtmlEmbedder.BuildPopUp_PTNote(ptnCount, WorkingDir + note);
                                    note = HtmlEmbedder.BuildSessionRow_PTNote(ptnCount);
                                    ptnCount++;
                                    break;
                            }
                        }

					    htmlFileBuffer += string.Format(
					            "<tr class=\"{0}\"> <{1} class=\"timestamp\">{2}</{1}><{1} class=\"notetype\">{0}</{1}><{1}>{3}</{1}></tr>\n",
					            thisLine[1], t, thisLine[0], note);
                        t = "td";
                    }
                    htmlFileBuffer += Htmlstrings.MHtmlBodytable2;
                    htmlFileBuffer += htmlFileBufferPopups;
                    htmlFileBuffer += Htmlstrings.PHtmlFooter;
                    File.WriteAllText(htmlFileFull, htmlFileBuffer, System.Text.Encoding.UTF8);
                    // Thread.Sleep(150); // Old sleep when file was being written line by line and issues were showing up

                    // remove old CSV file if everything worked well.
                    try { 
                        File.Delete(csvFileFull);
                    } catch { } // supress delete errors like access denied etc 
                    MessageBox.Show("The HTML was created successfully!\nFile created: " + htmlFileFull, "HTML Conversion Successful!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    Logger.Record("[CSV2HTML]: EXCEPTION reached - Session Report file could not be saved (" + htmlFileFull + ")", "Session", "error");
                    exDrRetry = Logger.FileErrorMessage(ex, "CSV to HTML", htmlFileFull);
                }
            } while (exDrRetry);
            Logger.Record("[CSV2HTML]: HTML Report built, done.", "Session", "info");
        }
    }
}
