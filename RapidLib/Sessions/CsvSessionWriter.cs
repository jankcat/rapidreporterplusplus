using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace RapidLib.Sessions
{
    public class CsvSessionWriter : ISessionWriter
    {
        private const string ColumnHeaders = "Time,Type,Content";
        private string _fileName = "";
        private readonly List<string> _sessionFiles = new List<string>();

        public bool StartSession()
        {
            _fileName = Path.Combine(Application.StartupPath, string.Format("{0}.csv", DateTime.Now.ToString("yyyyMMdd_HHmmss")));
            return OutputNoteLine(string.Format("{0}{1}", ColumnHeaders, Environment.NewLine));
        }

        public bool AddNote(Note note)
        {
            if (string.IsNullOrWhiteSpace(_fileName)) return false;
            var path = Path.GetDirectoryName(_fileName);
            if (string.IsNullOrWhiteSpace(path)) path = Application.StartupPath;
            switch (note.Type)
            {
                case NoteTypes.ScreenShot:
                    var imgFile = Path.Combine(path, string.Format("cap_{0}.png", DateTime.Now.ToString("yyyyMMdd_HHmmssfff")));
                    var img = ImageUtil.BuildImageFromString(note.Contents);
                    if (!OutputScreenShotFile(img, imgFile)) return false;
                    note.Contents = imgFile;
                    _sessionFiles.Add(imgFile);
                    break;
                case NoteTypes.PlainTextNote:
                    var noteFile = Path.Combine(path, string.Format("note_{0}.txt", DateTime.Now.ToString("yyyyMMdd_HHmmssfff")));
                    if (!OutputPlainTextNoteFile(note.Contents, noteFile)) return false;
                    note.Contents = noteFile;
                    _sessionFiles.Add(noteFile);
                    break;
            }
            var noteText = string.Format("{0},{1},\"{2}\"{3}", note.Time.ToString("yyyyMMdd_HHmmss"), note.Type, note.Contents, Environment.NewLine);
            return OutputNoteLine(noteText);
        }

        public List<Note> GetAllNotes()
        {
            if (string.IsNullOrWhiteSpace(_fileName)) return new List<Note>();
            try
            {
                var notes = new List<Note>();
                foreach (var line in File.ReadAllLines(_fileName, Encoding.UTF8))
                {
                    if ("" == line) continue;
                    if (line.StartsWith(ColumnHeaders)) continue;
                    var thisLine = line.Split(',');
                    if (thisLine.Length <= 2) continue;
                    var note = new Note {Contents = thisLine[2].Replace("\"", "")};
                    try
                    {
                        note.Time = DateTime.ParseExact(thisLine[0], "yyyyMMdd_HHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    }
                    catch
                    {
                        note.Time = DateTime.Now;
                    }
                    NoteTypes type;
                    var success = Enum.TryParse(thisLine[1], true, out type);
                    if (!success) type = NoteTypes.Unknown;
                    note.Type = type;

                    switch (note.Type)
                    {
                        case NoteTypes.ScreenShot:
                            var img = InputScreenShotFile(note.Contents);
                            if (img == null) continue;
                            note.Contents = ImageUtil.BuildStringFromImage(img);
                            break;
                        case NoteTypes.PlainTextNote:
                            var ptn = InputPlainTextNoteFile(note.Contents);
                            if (ptn == null) continue;
                            note.Contents = ptn;
                            break;
                    }
                    notes.Add(note);
                }
                return notes;
            }
            catch
            {
                return new List<Note>();
            }
        }

        public bool DeleteSessionData()
        {
            try
            {
                foreach (var file in _sessionFiles) File.Delete(file);
                File.Delete(_fileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CanIWrite()
        {
            var path = Path.Combine(Application.StartupPath, "testwrite_deleteme.txt");
            try
            {
                File.AppendAllText(path, "test text. please delete this file if you found it.", Encoding.UTF8);
                File.Delete(path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public SessionDetails ResumePausedSession()
        {
            //var csvFile = SelectSessionCsvForOpen();
            //if (string.IsNullOrWhiteSpace(csvFile)) return false;
            //LoadCsvIntoSession(csvFile);
            //set filename var from selected file
            throw new NotImplementedException();
        }

        public bool PauseSession()
        {
            if (string.IsNullOrWhiteSpace(_fileName)) return false;
            throw new NotImplementedException();
        }

        private bool OutputNoteLine(string note)
        {
            try
            {
                File.AppendAllText(_fileName, note, Encoding.UTF8);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool OutputScreenShotFile(Image img, string file)
        {
            try
            {
                img.Save(file, ImageFormat.Png);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool OutputPlainTextNoteFile(string note, string file)
        {
            try
            {
                File.WriteAllText(file, note, Encoding.UTF8);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static Image InputScreenShotFile(string file)
        {
            try
            {
                return Image.FromFile(file);
            }
            catch
            {
                return null;
            }
        }

        private static string InputPlainTextNoteFile(string file)
        {
            try
            {
                return File.ReadAllText(file, Encoding.UTF8);
            }
            catch
            {
                return null;
            }
        }

        //private string SelectSessionCsvForOpen()
        //{
        //    var openFileDialog = new OpenFileDialog()
        //    {
        //        DefaultExt = "csv",
        //        //InitialDirectory = WorkingDir
        //    };
        //    return openFileDialog.ShowDialog() == DialogResult.OK ? openFileDialog.FileName : "";
        //}

        //private void LoadCsvIntoSession(string csvFile)
        //{
        //    try
        //    {
        //        foreach (var line in File.ReadAllLines(csvFile, Encoding.UTF8))
        //        {
        //            if ("" == line) continue;
        //            var thisLine = line.Split(',');
        //            if (thisLine.Length <= 2) continue;
        //            var note = thisLine[2].Replace("\"", "");
        //            switch (thisLine[1])
        //            {
        //                case @"Session Reporter":
        //                    Tester = note;
        //                    StartingTime = DateTime.Parse(thisLine[0]);
        //                    break;
        //                case @"Scenario ID":
        //                    ScenarioId = note;
        //                    break;
        //                case @"Session Charter":
        //                    Charter = note;
        //                    break;
        //                case @"Environment":
        //                    Environment = note;
        //                    break;
        //                case @"Versions":
        //                    Versions = note;
        //                    break;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(string.Format(
        //            "Ouch! An error occured when trying to write the note into a file.\n" +
        //            "The file name is: {0}\n\n" + "Possible causes:\n" +
        //            " -- You don't have write permissions to the folder or file;\n" +
        //            " -- The file is locked by another program (Excel? Explorer preview?);\n" +
        //            " -- Windows preview pane is holding the file blocked for editing;\n" +
        //            " -- (there may be other reasons).\n\n" + "Possible solutions:\n" +
        //            " -- Set write permissions to the folder or file;\n" +
        //            " -- Close another application that may be using the file;\n" +
        //            " -- Select another file in explorer.\n\n" + "Exception details for investigation:\n{1}",
        //            csvFile, ex.Message));
        //    }
        //}
    }
}