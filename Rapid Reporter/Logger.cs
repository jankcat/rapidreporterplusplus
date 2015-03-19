// This application lets the tester be the master of the session.
//  Logger - logs details in the 'log.log' file.

// References and Dependencies
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Rapid_Reporter
{
    public static class Logger
    {
        // Notice there's only one operation: record a new message.
        //  Three overloaded options to use at convenience, all end up in the same function.

        // Note: Log is active only if there's a _rrlog_.log log file available in the directory

        public static void Record(string message)   // Default origin: generic
        {
            Record(message, "generic");
        }
        public static void Record(string message, string origin)    // Default type: general
        {
            Record(message, origin, "general");
        }
        public static void Record(string message, string origin, string type)
        {
            // How it works: log is logged only if the log file exists.
            //  In normal operations the user sees no log.
            //  The upside is that the log can be started even in the middle of the application operation!
            //  The downside is that there are many calls to disk even when no log is present.
            //      One option could be to set a flag and re-check status once every few minutes until the file exists.
            //          Or every 15 log writes...

            // This part will keep the Directory.GetCurrentDirectory. The rest will work with session files.
            string targetFile = Directory.GetCurrentDirectory() + @"\_rrlog_.log";
            if (File.Exists(targetFile))
            {
                try
                {
                    File.AppendAllText(targetFile, string.Format("{0}, {1}, {2}, {3}, {4}\n", Process.GetCurrentProcess().Id, DateTime.Now, origin, type, message));
                }
                catch (Exception ex)
                {
                    // We ignore silently errors of logging
                    // Reason: On other exceptions we put out a message box. But the log is a secondary feature, we don't want it to annoy.
                    //  Also, we would require a logger for the log, to keep tab on log errors :)
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        // FileErrorMessage
        //  This is called from 'catch' operations throghout the code. When a file exception is found, we come here and show a message box.
        //  Note: there's an exception in RTFNote.xaml.cs that still does not use this function.
        public static bool FileErrorMessage(Exception ex, string title, string fileName)
        {
            Record("\t[FileErrorMessage]: EXCEPTION reached - (Log)", "Logger", "error");
            Record("\t[FileErrorMessage]: EXCEPTION: " + ex.Message, "Logger", "error");
            var retryMsg = string.Format(
                "Ouch! An error occured when trying to write the note into a file.\n" +
                "The file name is: {0}\n\n" + "Possible causes:\n" +
                " -- You don't have write permissions to the folder or file;\n" +
                " -- The file is locked by another program (Excel? Explorer preview?);\n" +
                " -- Windows preview pane is holding the file blocked for editing;\n" +
                " -- (there may be other reasons).\n\n" + "Possible solutions:\n" +
                " -- Set write permissions to the folder or file;\n" +
                " -- Close another application that may be using the file;\n" +
                " -- Select another file in explorer.\n\n" + "Exception details for investigation:\n{1}",
                fileName, ex.Message);
            var retryTitle = string.Format(@"Rapid Reporter file error: {0}", title);
            return (DialogResult.Retry == MessageBox.Show(retryMsg, retryTitle, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error));
        }
    }
}
