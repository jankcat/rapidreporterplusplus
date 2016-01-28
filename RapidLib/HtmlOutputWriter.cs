using System.Collections.Generic;
using System.Linq;
using RapidLib.HTML;

namespace RapidLib
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
                        bottomPopups += HtmlEmbedder.BuildPopUp_PTNote(ptnCount, note.Contents);
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

            //TODO Prompt Save Path, Delete file if existing, check if we can write.

            throw new System.NotImplementedException();
        }

        private static string BuildTableRow(Note note, bool blank = false)
        {
            return blank
                ? "<tr><td class=\"timestamp\"></td><td class=\"notetype\"></td><td></td></tr>\n"
                : string.Format(
                    "<tr class=\"{0}\"> <td class=\"timestamp\">{1}</td><td class=\"notetype\">{0}</td><td>{2}</td></tr>\n",
                    Note.GetTypeName(note.Type), note.Time, note.Contents);
        }

        public List<Note> InputSessionFromOutput(out SessionDetails details)
        {
            // TODO this to resume sessions that have been HTMLd
            throw new System.NotImplementedException();
        }

        //private string DiscoverSavePath(string csvFile)
        //{
        //    var str1 =
        //        (new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars())).Aggregate(
        //            string.Format("{0} - {1}.htm", Path.GetFileNameWithoutExtension(csvFile), ScenarioId),
        //            (current, c) => current.Replace(c.ToString(CultureInfo.InvariantCulture), ""));
        //    var str2 = WorkingDir + str1;
        //    var saveFileDialog1 = new SaveFileDialog
        //    {
        //        DefaultExt = "htm",
        //        FileName = str1,
        //        InitialDirectory = WorkingDir
        //    };
        //    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
        //        str2 = saveFileDialog1.FileName;
        //    return str2;
        //}
    }
}