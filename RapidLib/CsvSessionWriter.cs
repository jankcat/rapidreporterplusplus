using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using RapidLib.Images;

namespace RapidLib
{
    public class CsvSessionWriter : ISessionWriter
    {
        private const string ColumnHeaders = "Time,Type,Content";
        private string _fileName = "";
        private List<string> _sessionFiles = new List<string>();

        public bool StartSession()
        {
            _fileName = Path.Combine(Application.StartupPath, string.Format("{0}.csv", DateTime.Now.ToString("yyyyMMdd_HHmmss")));
            return OutputNoteLine(string.Format("{0}{1}", ColumnHeaders, Environment.NewLine));
        }

        public bool AddNote(Note note)
        {
            if (string.IsNullOrWhiteSpace(_fileName)) return false;
            switch (note.Type)
            {
                case NoteTypes.ScreenShot:
                    var img = ImageUtil.BuildImageFromString(note.Contents);
                    var file = string.Format("cap_{0}.png", DateTime.Now.ToString("yyyyMMdd_HHmmssfff"));
                    note.Contents = file;

                    break;
                case NoteTypes.PlainTextNote:
                    break;
            }


            // if plaintext:
            // File.WriteAllText(string.Format("{0}.txt", DateTime.Now.ToString("yyyyMMdd_HHmmssfff")), safeNote);
            // save contents to txt file, save file name to contents var
            // add filename to _sessionFiles

            // if screenshot/image:
            // save contents to img file
            // add filename to _sessionFiles

            var noteText = string.Format("{0},{1},\"{2}\"{3}", note.Time, note.Type, note.Contents, Environment.NewLine);
            return OutputNoteLine(noteText);
            throw new NotImplementedException();
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

        public List<Note> GetAllNotes()
        {
            if (string.IsNullOrWhiteSpace(_fileName)) return new List<Note>();


            throw new NotImplementedException();
        }

        public bool DeleteSessionData()
        {
            //File.Delete(_fileName); and all _sessionFiles
            throw new NotImplementedException();
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

        private bool OutputNoteLine(string note)
        {
            try
            {
                File.AppendAllText(_fileName, note, Encoding.UTF8);
            }
            catch
            {
                return false;
            }
            return true;
        }

        private bool OutputScreenShotFile(Image img, string file)
        {
            try
            {
                var path = Path.GetDirectoryName(_fileName);
                if (string.IsNullOrWhiteSpace(path)) path = Application.StartupPath;
                file = Path.Combine(path, file);

                img.Save(file, ImageFormat.Png);
                _currentSession.UpdateNotes("Screenshot", _screenshotName);
                var item = new MenuItem
                {
                    Header = string.Format("Screenshot Saved! Filename: {0}", _screenshotName),
                    IsEnabled = false
                };
                NoteHistory.Items.Add(item);
                NoteHistory.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(
                    "Ouch! An error occured when trying to write the note into a file.\n" +
                    "The file name is: {0}\n\n" + "Possible causes:\n" +
                    " -- You don't have write permissions to the folder or file;\n" +
                    " -- The file is locked by another program (Excel? Explorer preview?);\n" +
                    " -- Windows preview pane is holding the file blocked for editing;\n" +
                    " -- (there may be other reasons).\n\n" + "Possible solutions:\n" +
                    " -- Set write permissions to the folder or file;\n" +
                    " -- Close another application that may be using the file;\n" +
                    " -- Select another file in explorer.\n\n" + "Exception details for investigation:\n{1}",
                    _screenshotName, ex.Message));
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