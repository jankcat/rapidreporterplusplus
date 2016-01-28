using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using RapidLib;
using RapidLib.Forms;
using RapidLib.Sessions;
using Application = System.Windows.Forms.Application;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MenuItem = System.Windows.Controls.MenuItem;
using MessageBox = System.Windows.MessageBox;

#pragma warning disable 612,618

namespace Rapid_Reporter.Forms
{
    // Controls the main Widget
    public partial class SmWidget
    { 
        // Session Notes variables
        private int _currentNoteType;		// The actual types are controlled by the Session class.
        private int _prevNoteType; private int _nextNoteType; // Used for the hints about the next note up or down.
        private int _currentScreenshot = 1;		// The number of the screenshot (increases by 1). Helps putting them in order, and finding them between multiple the files.
        private string _screenshotName = "";		// Attached to a Session Note.
        public string PlainTextNoteName = "";			// Attached to a Session Note. Public because it is used *directly* by the RTFNote

        //rtf and pt note things
        public bool IsPlainTextDiagOpen;

        // State Based Behaviors:
        // Session flow:
        //  1) There are two initialization stages in application: tester and charter. Then it moves to the 'testing', or 'notes' stage.

        // Session flow state based behavior
        //enum sessionStartingStage { tester, charter, notes };
        // This is used only in the beginning, in order to receive the tester name and charter text
        Session.SessionStartingStage _currentStage = Session.SessionStartingStage.Tester;

        // Timer to perform recurring actions (timing is set on windows load)
        static Timer _recurrenceTimer = new Timer();
        private static DispatcherTimer _sessionTimer = new DispatcherTimer();
        private static int _sessionTicks;

        private static bool _showScreenshotPreviews;
        private static readonly ScreenShotPreview ScreenShotPreviewForm = new ScreenShotPreview();

        HotKey _hotKey;

        // These two classes are external.
        //  We share classes and data all around. This coupling will be a weak spot if app gets complex.
        Session _currentSession  = new Session();    // The session managing class
        readonly PlainTextNote _ptn = new PlainTextNote();                // The enhanced note window

        /** Starting Process **/
        /**********************/
        /// The application starts by asking for tester and charter information. Only then the session starts

        // Default constructor, everything is empty/default values
        public SmWidget()
        {
            RegUtil.InitReg();
            var trans = RegUtil.Transparency;
            InitializeComponent();
            SetBgColor(RegUtil.BackgroundColor);
            TransparencySlide.Value = trans;
            var autoUpdate = RegUtil.CheckForUpdates;
            AutoUpdate.IsChecked = autoUpdate;
            TimerDisplay.Text = "00:00";
            _showScreenshotPreviews = RegUtil.ScreenShotPreviewEnabled;
            ShowScreenshotPreviews.IsChecked = _showScreenshotPreviews;
            _ptn.InitializeComponent();
            _ptn.Sm = this;
            Task.Factory.StartNew(Updater.CheckVersion);
            NoteContent.Focus();
        }

        // Prepare the session report log (adds the notes types, for example)
        private void SMWidgetForm_Loaded(object sender, RoutedEventArgs e)
        {
            SMWidgetForm.Title = Application.ProductName;
            SetWorkingDir(_currentSession.WorkingDir);
            StateMove(Session.SessionStartingStage.Tester);

            // Some of the actions in the tool are recurrent. We do them every 30 seconds.
            _recurrenceTimer.Tick += TimerEventProcessor; // this is the function called everytime the timer expires
            _recurrenceTimer.Interval = 90 * 1000; // 30 times 1 second (1000 milliseconds)
            _recurrenceTimer.Start();
            StartTimer();
            NoteContent.Focus();
        }
        // When the widget is on focus, the note taking area is always on focus. Tester can keep writing all the time
        private void SMWidgetForm_GotFocus(object sender, RoutedEventArgs e)
        {
            SMWidgetForm.NoteContent.Focus();
        }

        /** Closing Process **/
        /*********************/

        //// Existent notes were automatically saved in a persistent file, so no need to save now.
        //// Mainly, the session should be terminated (timing notes added to file too) and all windows closed.
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        // Closing the form can't just close the window, it has to follow the finalization process
        private void SMWidgetForm_Closed(object sender, EventArgs e)
        {
            ExitApp();
        }
        // Before closing the window, we have to close the session and the RTF note
        private void ExitApp(bool dontFinishSession = false)
        {
            StopTimer();
            if (!dontFinishSession)
            {
                // Session
                _currentSession.CloseSession();
            }
            // PT Note
            _ptn.ForceClose = true; // We keep the RTF open (hidden), so we have to force it out
            _ptn.Close();
            // This form
            Environment.Exit(0); // Fixes mysterious crash on non-session executions (reported by AB, MB, JS)
        }

        /** Window Event Handling **/
        /***************************/

        // Application can be set transparent, to avoid distraction from task at hand
        private void TransparencySlide_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            RegUtil.Transparency = e.NewValue;
            SMWidgetForm.Opacity = e.NewValue;
            _ptn.Opacity = Math.Min(e.NewValue + 0.2, 1);
        }

        // Application can be moved around the screen, to keep it out of the way
        void SMWidget_LeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        /** Note Event Handling **/
        // Very important functions happen during note taking:
        //  Every submittal saves the note to disk (data is always safe)
        //  Type of note can be changed easily at all times, by pressing up/down
        private void NoteContent_KeyUp(object sender, KeyEventArgs e)
        {
            var notesLenght = _currentSession.NoteTypes.Length - 1;
            
            // Up and Down cycles through the note types
            if ((e.Key == Key.Down || e.Key == Key.Up) && _currentStage == Session.SessionStartingStage.Notes)
            {
                // Algorithm is spelled out: If at end, forward goes to beginning, if at beginning back goes to end, step by 1 otherwise.
                if (e.Key == Key.Up)
                {
                    _prevNoteType = _currentNoteType;
                    if (_currentNoteType > 0)
                    { _currentNoteType--; }
                    else
                    { _currentNoteType = notesLenght; }
                    if (_nextNoteType > 0)
                    { _nextNoteType--; }
                    else
                    { _nextNoteType = notesLenght; }
                }
                else if (e.Key == Key.Down)
                {
                    _nextNoteType = _currentNoteType;
                    if (_currentNoteType < notesLenght)
                    { _currentNoteType++; }
                    else
                    { _currentNoteType = 0; }
                    if (_prevNoteType < notesLenght)
                    { _prevNoteType++; }
                    else
                    { _prevNoteType = 0; }
                }
                NoteType.Text = _currentSession.NoteTypes[_currentNoteType] + ":";
                prevType.Text = "? " + _currentSession.NoteTypes[_prevNoteType] + ":";
                nextType.Text = "? " + _currentSession.NoteTypes[_nextNoteType] + ":";
            }
            else switch (e.Key)
            {
                // Enter keys accept the note into the report
                case Key.Enter:
                    if (e.Key == Key.Enter && NoteContent.Text.Trim().Length != 0)
                    {
                        switch (_currentStage)
                        {
                            case Session.SessionStartingStage.Tester:
                                _currentSession.Tester = NoteContent.Text.Replace("\"", "''").Replace(",", "").Replace(";", "").Trim();
                                StateMove(Session.SessionStartingStage.ScenarioId);
                                break;
                            case Session.SessionStartingStage.ScenarioId:
                                _currentSession.ScenarioId = NoteContent.Text.Replace("\"", "''").Replace(",", "").Replace(";", "").Trim();
                                StateMove(Session.SessionStartingStage.Charter);
                                break;
                            case Session.SessionStartingStage.Charter:
                                _currentSession.Charter = NoteContent.Text.Replace("\"", "''").Replace(",", "").Replace(";", "").Trim();
                                StateMove(Session.SessionStartingStage.Environment);
                                break;
                            case Session.SessionStartingStage.Environment:
                                _currentSession.Environment = NoteContent.Text.Replace("\"", "''").Replace(",", "").Replace(";", "").Trim();
                                StateMove(Session.SessionStartingStage.Versions);
                                break;
                            case Session.SessionStartingStage.Versions:
                                _currentSession.Versions = NoteContent.Text.Replace("\"", "''").Replace(",", "").Replace(";", "").Trim();
                                StateMove(Session.SessionStartingStage.Notes);
                                break;
                            default:
                                {
                                    // What we do when adding a note:
                                    //  - 1) We add to the session notes
                                    //  - 2) We add to the history context menu
                                    //  - 3) We clear notes and attachments to make place for new ones
                                    /*1*/
                                    _currentSession.UpdateNotes(_currentNoteType, NoteContent.Text.Replace("\"", "''").Replace(",", ";").Trim());
                                    /*2*/   var item = new MenuItem {Header = NoteContent.Text};
                                    item.Click += delegate { GetHistory(item.Header.ToString()); };
                                    NoteHistory.Items.Add(item);
                                    NoteHistory.Visibility = Visibility.Visible;
                                }
                                break;
                        }
                        /*3*/ ClearNote();
                    }
                    break;
                // Esc key clears the note field
                case Key.Escape:
                    ClearNote();
                    break;
            }
        }
        
        // The function below will change the visuals of the application at the different stages (tester/charter/notes state based behavior)
        private void StateMove(Session.SessionStartingStage newStage, bool skipStartSession = false)
        {
            _currentStage = newStage;
            switch (_currentStage)
            {
                case Session.SessionStartingStage.Tester:
                    NoteType.Text = "Reporter:";
                    prevType.Text = ""; 
                    nextType.Text = "";
                    _prevNoteType = 1;
                    _nextNoteType = _currentSession.NoteTypes.Length - 1;
                    NoteType.FontSize = 23;
                    break;
                case Session.SessionStartingStage.Charter:
                    NoteType.Text = "Charter:";
                    break;
                case Session.SessionStartingStage.ScenarioId:
                    NoteType.Text = "Scenario ID:";
                    break;
                case Session.SessionStartingStage.Environment:
                    NoteType.Text = "Environment:";
                    break;
                case Session.SessionStartingStage.Versions:
                    NoteType.Text = "Versions:";
                    break;
                case Session.SessionStartingStage.Notes:
                    NoteContent.ToolTip = (100 < _currentSession.Charter.Length) ? _currentSession.Charter.Remove(100)+"..." : _currentSession.Charter;
                    NoteType.Text = _currentSession.NoteTypes[_currentNoteType] + ":";
                    prevType.Text = "? " + _currentSession.NoteTypes[_prevNoteType] + ":";
                    nextType.Text = "? " + _currentSession.NoteTypes[_nextNoteType] + ":";
                    NoteType.FontSize = 21;
                    if (!skipStartSession) _currentSession.StartSession(); 
                    ProgressGo(90); 
                    t90.IsChecked = true;
                    ScreenShot.IsEnabled = true; 
                    RTFNoteBtn.IsEnabled = true;
                    ResumeSession.IsEnabled = false;
                    PauseSession.IsEnabled = true;
                    // Change the icon of the image of the buttons, to NOT appear disabled.
                    CloseButton.ToolTip = "Save and Quit";
                    SaveAndQuitOption.Header = "Save and Quit";
                    SaveAndNewOption.IsEnabled = true;
                    ScreenShotIcon.Source = new BitmapImage(new Uri("iconshot.png", UriKind.Relative));
                    RTFNoteBtnIcon.Source = new BitmapImage(new Uri("iconnotes.png", UriKind.Relative));
                    TimerMenu.IsEnabled = true;
                    break;
            }
        }

        // AboutBox
        // Shows About dialog box with software info, contacts and credits
        private void AboutBox_Click(object sender, RoutedEventArgs e)
        {
            var about = new AboutForm();
            about.ShowDialog();
        }

        // GetHistory:
        // Note reuse (the historyNote comes from pressing the history context menu)
        void GetHistory(string historyNote)
        {
            NoteContent.Text = historyNote;
        }

        // ProgressTimer
        // Makes the progress bar progress, according to the time chosen
        private void ProgressTimer_Click(object sender, RoutedEventArgs e)
        {
            if (sender.ToString().Contains("1 min")) ProgressGo(1);
            if (sender.ToString().Contains("3 min")) ProgressGo(3);
            if (sender.ToString().Contains("120 min")) ProgressGo(120);
            if (sender.ToString().Contains("90 min")) { ProgressGo(90); t90.IsChecked=true; }
            if (sender.ToString().Contains("60 min")) ProgressGo(60);
            if (sender.ToString().Contains("30 min")) ProgressGo(30);
            if (sender.ToString().Contains("Stop")) ProgressGo(0);
        }
        private void ProgressGo(int time) // time is received in minutes
        {
            ProgressBackground.Value = 0;

            timeralarm.Visibility = Visibility.Hidden;

            StopTimers();
            if (time > 0)
            {
                _currentSession.Duration = time * 60;
                var duration = new Duration(TimeSpan.FromSeconds(_currentSession.Duration));
                var timedAnimation = new DoubleAnimation(100, duration);

                // Progress Bar Repositioning
                ////
                // In order to reposition the timer in a proportional place, we do the following calculation:
                //  The position of the progress bar should be put in the percentage elapsed time from the grand total time,
                //  where the grand total time is the elapsed time until now plus the time that was chosen as the new session end.
                //              Elapsed Time
                //      ------------------------------ == Percentage of time elapsed
                //      Elapsed Time + Additional Time
                ProgressBackground.Value = 100*(
                    ((DateTime.Now - _currentSession.StartingTime).TotalSeconds) /
                    (((DateTime.Now - _currentSession.StartingTime).TotalSeconds) + _currentSession.Duration)
                    );
                
                // In order to reposition the timer at the beginning of the progress bar at every change, one should stop it before restarting;
                //  StopTimers();
                //
                // In order to keep the timer in its place and just speed up or slow down to meet the end at the current time, no
                //  other operation needs to be done.
                //ProgressBackground.Value = ProgressBackground.Value; // WTF? True = True? That's the best 'no other operation' possible?

                ProgressBackground.BeginAnimation(RangeBase.ValueProperty, timedAnimation);
            }
        }
        // Sets the progress bar to null (stopped, not seen), and hides clock icon
        private void StopTimers()
        {
            ProgressBackground.BeginAnimation(RangeBase.ValueProperty, null);
            timeralarm.Visibility = Visibility.Hidden;
        }
        // TimerEventProcessor
        // The actions in this fuction will happen every time the timer expires.
        private void TimerEventProcessor(Object myObject, EventArgs myEventArgs)
        {
            if (100 <= ProgressBackground.Value)
            {
                timeralarm.Visibility = Visibility.Visible;
            }
        }
        // timeralarm_MouseLeftButtonDown:
        // We'll check when the progress bar reaches the end, to trigger the time's up notification
        private void timeralarm_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            t0.IsChecked = true;
            t120.IsChecked = false; t90.IsChecked = false; t60.IsChecked = false; t30.IsChecked = false;

            ProgressGo(0);
        }
        // timer_Checked, gets timing commands from the context menu
        //  This is used both for the checked and unchecked events. On checked, to make sure the event sets the right timer, on unchecked to make sure the timer isn't unchecked.
        private void timer_Checked(object sender, RoutedEventArgs e)
        {
            t0.IsChecked = (t0.Equals(sender));//   || t0.IsChecked);
            t120.IsChecked = (t120.Equals(sender));// || t120.IsChecked);
            t90.IsChecked = (t90.Equals(sender));//  || t90.IsChecked);
            t60.IsChecked = (t60.Equals(sender));//  || t60.IsChecked);
            t30.IsChecked = (t30.Equals(sender));//  || t30.IsChecked);
        }

        /** Note Handling  **/
        /********************/

        // Saves all screenshots in files
        private void ScreenShot_Click(object sender, RoutedEventArgs e)
        {
            var edit = Control.ModifierKeys == Keys.Shift;
            var direct = Control.ModifierKeys == Keys.Control;
            if (edit || !direct)
            {
                ScreenShotPreviewForm.Hide();
                WindowState = WindowState.Minimized;
            }
            Bitmap bmpOut;
            var ss = new ScreenShot();
            if (!direct && !edit)
            {
                bmpOut = ss.CaptureSnippet();
            }
            else
            {
                bmpOut = ss.CaptureScreenShot();
            }
            if (ss.Canceled)
            {
                if (edit || !direct)
                {
                    WindowState = WindowState.Normal;
                }
                return;
            }
            AddScreenshot2Note(bmpOut);                                 
            if (edit)                                                                   
            {
                var paint = new Process
                    {
                        StartInfo =
                            {
                                FileName = "mspaint.exe",
                                Arguments = "\"" + _currentSession.WorkingDir + _screenshotName + "\""
                            }
                    };                                          
                paint.Start();                                                                          
            }
            else if (_showScreenshotPreviews)
            {
                ScreenShotPreviewForm.Show();
                ScreenShotPreviewForm.UpdateScreenshot(bmpOut);
            }

            if (edit || !direct)
            {
                WindowState = WindowState.Normal;
            } 
        }

        // Adding attached screenshot have dedicated functions that deal with the visual
        //  clues as well
        private void AddScreenshot2Note(Image bitmap)
        {
            // Put a visual effect to remember the tester there's an image on the attachment barrel
            var effect = new BevelBitmapEffect {BevelWidth = 2, EdgeProfile = EdgeProfile.BulgedUp};
            ScreenShot.BitmapEffect = effect;
        }

        // The functions below set/unset the hotkey for screenshot
        private void SetShotHotKey_Checked(object sender, RoutedEventArgs e)
        {
            _hotKey = new HotKey(Key.F9, KeyModifier.Ctrl | KeyModifier.Alt, OnHotKeyHandler);
        }
        private void SetShotHotKey_Unchecked(object sender, RoutedEventArgs e)
        {
            _hotKey.Unregister();
        }
        private void OnHotKeyHandler(HotKey hotKey)
        {
            
            // If the user keeps the key pressed, HotKey requests will queue up and lag
            //  With this condition we break the chain, as the requests that come after stopping
            //  to press are ifnored.
            if (Control.ModifierKeys != (Keys.Control | Keys.Alt))
            {
                return;
            }
            if (_currentStage == Session.SessionStartingStage.Notes)
            {
                ScreenShot_Click(null, null);
            }
        }

        private void AutoUpdate_Checked(object sender, RoutedEventArgs e)
        {
            RegUtil.CheckForUpdates = true;
        }
        private void AutoUpdate_Unchecked(object sender, RoutedEventArgs e)
        {
            RegUtil.CheckForUpdates = false;
        }

        // Show or hide the enhanced notes window.
        private void RTFNote_Click(object sender, RoutedEventArgs e)
        {

            IsPlainTextDiagOpen = !IsPlainTextDiagOpen;
            if (IsPlainTextDiagOpen) // Show the note are
            {
                _ptn.Left = Left;
                _ptn.Width = Width;
                if (Top > _ptn.Height + 10)
                {
                    _ptn.Top = Top - (_ptn.Height + 10);
                }
                else
                {
                    _ptn.Top = (Top + Height) + 10;
                }
                _ptn.Show();
                _ptn.Focus();
            }
            else // Hide the note area
            {
                _ptn.Hide();
            }
        }

        /// Autosave Attachments
        internal void SavePlainTextNote(string filename)
        {
            _currentSession.UpdateNotes("PlainText Note", filename);
            var item = new MenuItem
            {
                Header = string.Format("Plaintext Note Saved! Filename: {0}", filename),
                IsEnabled = false
            };
            NoteHistory.Items.Add(item);
            NoteHistory.Visibility = Visibility.Visible;
        }

        // Makes space for a new note
        private void ClearNote()
        {
            NoteContent.Text = "";  // New note
            _screenshotName = "";    // New pic attachment
            PlainTextNoteName = "";       // New note attachment (RTF note area content is left intact!)
            // Clear visual effects (screenshots are all always saved anyway)
            var effect = new BevelBitmapEffect {BevelWidth = 0, EdgeProfile = EdgeProfile.BulgedUp};
            ScreenShot.BitmapEffect = effect;
            RTFNoteBtn.BitmapEffect = effect;
        }

        // Changes the working Directory for the session
        private void SetWorkingDir(string newPath)
        {
            if (!newPath.EndsWith(@"\")) newPath += @"\"; // Add the trailing 'slash' to the directory
            _ptn.WorkingDir = _currentSession.WorkingDir = newPath; // the workingDir needs to be the same for all files!
            //FolderName.Header = (50 < _currentSession.WorkingDir.Length) ? "..." + _currentSession.WorkingDir.Substring(_currentSession.WorkingDir.Length - 47) : _currentSession.WorkingDir;
        }

        private void SaveAndQuitOption_Click(object sender, RoutedEventArgs e)
        {
            CloseButton_Click(sender, e);
        }

        private void ResetSession()
        {
            //reset
            _currentNoteType = 0;		// The actual types are controlled by the Session class.
            _prevNoteType = 0;
            _nextNoteType = 0; // Used for the hints about the next note up or down.
            _currentScreenshot = 1;		// The number of the screenshot (increases by 1). Helps putting them in order, and finding them between multiple the files.
            _screenshotName = "";		// Attached to a Session Note.
            PlainTextNoteName = "";			// Attached to a Session Note. Public because it is used *directly* by the RTFNote
            IsPlainTextDiagOpen = false;
            ResumeSession.IsEnabled = true;
            PauseSession.IsEnabled = false;
            _currentStage = Session.SessionStartingStage.Tester;
            _recurrenceTimer = new Timer();
            _currentSession = new Session();    // The session managing class
            SMWidgetForm.Title = Application.ProductName;
            SetWorkingDir(_currentSession.WorkingDir);
            _recurrenceTimer.Tick += TimerEventProcessor; // this is the function called everytime the timer expires
            _recurrenceTimer.Interval = 90 * 1000; // 30 times 1 second (1000 milliseconds)
            _recurrenceTimer.Start();
            StartTimer();
        }

        private void SaveAndNewOption_Click(object sender, RoutedEventArgs e)
        {
            StopTimer();
            // Session
            _currentSession.CloseSession();
            // PT Note
            _ptn.ForceClose = true; // We keep the RTF open (hidden), so we have to force it out
            _ptn.Close();

            ResetSession();
            StateMove(Session.SessionStartingStage.Tester);
            NoteContent.Focus();
        }

        private void ColorPicker_OnClick(object sender, RoutedEventArgs e)
        {
            var colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            var r = colorDialog.Color.R;
            var g = colorDialog.Color.G;
            var b = colorDialog.Color.B;
            SetBgColor(System.Drawing.Color.FromArgb(colorDialog.Color.A, r, g, b));
        }

        private void SetBgColor(System.Drawing.Color color)
        {
            RegUtil.BackgroundColor = color;
            MainGrid.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
        }

        private void ResumeSession_Click(object sender, RoutedEventArgs e)
        {
            ResetSession();
            if (!_currentSession.ResumeSession()) return;
            StateMove(Session.SessionStartingStage.Notes, true);
            _currentSession.UpdateNotes("Note", "Resuming Session...");
            NoteContent.Focus();
        }

        private void PauseSession_Click(object sender, RoutedEventArgs e)
        {
            _currentSession.UpdateNotes("Note", "Pausing Session...");
            ExitApp(true);
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            Updater.ManualCheckVersion();
        }

        internal void StartTimer()
        {
            _sessionTicks = 0;
            _sessionTimer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 1, 0)
            };
            _sessionTimer.Tick += SessionTimerUpdate;
            _sessionTimer.Start();
        }

        internal void StopTimer()
        {
            _sessionTimer.Stop();
        }

        internal void SessionTimerUpdate(object o, EventArgs sender)
        {
            _sessionTicks++;
            var time = new TimeSpan(0, 0, _sessionTicks);
            TimerDisplay.Text = (time.Hours > 0)
                ? string.Format("{0}:{1:00}:{2:00}", time.Hours, time.Minutes, time.Seconds)
                : string.Format("{0:00}:{1:00}", time.Minutes, time.Seconds);
        }


        private void ShowScreenshotPreviews_Checked(object sender, RoutedEventArgs e)
        {
            _showScreenshotPreviews = true;
            RegUtil.ScreenShotPreviewEnabled = _showScreenshotPreviews;
        }

        private void ShowScreenshotPreviews_Unchecked(object sender, RoutedEventArgs e)
        {
            _showScreenshotPreviews = false;
            RegUtil.ScreenShotPreviewEnabled = _showScreenshotPreviews;
        }
    }
}
