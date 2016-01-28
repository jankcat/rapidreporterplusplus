using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RapidLib.Sessions;

namespace RapidLib.HTML
{
    public class HtmlOutputWriter : ISessionOutputWriter
    {
        public bool OutputSession(List<Note> notes, SessionDetails details)
        {
            var title = string.Format("{0}{1}", details.ScenarioId, HtmlStrings.HtmlTitle);
            var htmlTitle = string.Format("{0}{1}{2}{3}", HtmlStrings.Head, HtmlStrings.TitleStart, title, HtmlStrings.TitleEnd);
            var htmlHead = string.Format("{0}{1}{2}{3}", HtmlStrings.StyleSheet, HtmlStrings.Javascript, HtmlStrings.SharePointMeta, HtmlStrings.Body);
            var htmlTop = string.Format("{0}{1}{2}{3}", htmlTitle, htmlHead, title, HtmlStrings.Table);

            var imgCount = 0;
            var ptnCount = 0;
            var topNotes = "";
            var bottomNotes = "";
            var bottomPopups = "";

            foreach (var note in notes.Where(note => !string.IsNullOrWhiteSpace(note.Contents)))
            {
                switch (note.Type)
                {
                    case NoteTypes.ScreenShot:
                        note.Contents = HtmlEmbedder.BuildSessionRow_Img(imgCount, note.Contents);
                        bottomPopups += HtmlEmbedder.BuildPopUp_Img(imgCount);
                        bottomNotes += BuildTableRow(note);
                        imgCount++;
                        break;
                    case NoteTypes.PlainTextNote:
                        bottomPopups += HtmlEmbedder.BuildPopUp_PTNote(ptnCount, System.Net.WebUtility.HtmlEncode(note.Contents));
                        note.Contents = HtmlEmbedder.BuildSessionRow_PTNote(ptnCount);
                        bottomNotes += BuildTableRow(note);
                        ptnCount++;
                        break;
                    case NoteTypes.Reporter:
                    case NoteTypes.Charter:
                    case NoteTypes.ScenarioId:
                    case NoteTypes.Environment:
                    case NoteTypes.Versions:
                    case NoteTypes.Summary:
                        topNotes += BuildTableRow(note);
                        break;
                    default:
                        bottomNotes += BuildTableRow(note);
                        break;
                }
            }

            topNotes = topNotes + BuildTableRow(new Note(), true);
            var output = string.Format("{0}{1}{2}{3}{4}{5}", htmlTop, topNotes, bottomNotes, HtmlStrings.TableEnd,
                bottomPopups, HtmlStrings.Foot);

            var badChars = (new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars()));
            var fileName = string.Format("{0} - {1}.htm", details.StartingTime, details.ScenarioId);
            var safeName = badChars.Aggregate(fileName, (current, c) => current.Replace(c.ToString(CultureInfo.InvariantCulture), ""));
            var path = Path.Combine(Application.StartupPath, safeName);
            var saveFileDialog = new SaveFileDialog
            {
                DefaultExt = "htm",
                FileName = safeName,
                InitialDirectory = Application.StartupPath
            };
            if (saveFileDialog.ShowDialog() == DialogResult.OK) path = saveFileDialog.FileName;

            return SaveHtmlFile(path, output);
        }

        private static string BuildTableRow(Note note, bool blank = false)
        {
            return blank
                ? "<tr><td class=\"timestamp\"></td><td class=\"notetype\"></td><td></td></tr>\n"
                : string.Format(
                    "<tr class=\"{0}\"> <td class=\"timestamp\">{1}</td><td class=\"notetype\">{0}</td><td>{2}</td></tr>\n",
                    Note.GetTypeName(note.Type), note.Time, note.Contents);
        }

        private static bool SaveHtmlFile(string path, string contents)
        {
            try
            {
                File.Delete(path);
                File.WriteAllText(path, contents, Encoding.UTF8);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Note> InputSessionFromOutput(out SessionDetails details)
        {
            // TODO this to resume sessions that have been HTMLd. LATER
            throw new System.NotImplementedException();
        }

    }
}