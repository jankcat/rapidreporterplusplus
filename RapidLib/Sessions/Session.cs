using System;
using System.Drawing;
using System.Globalization;

namespace RapidLib.Sessions
{
    public class Session
    {
        private SessionDetails _details;

        public SessionStartingStages CurrentStage = SessionStartingStages.Tester;
        private readonly ISessionWriter _sessionWriter; 
        private readonly ISessionOutputWriter _sessionOutputWriter;

        public Session(ISessionWriter sessionWriter, ISessionOutputWriter sessionOutputWriter, SessionDetails details)
        {
            _sessionWriter = sessionWriter;
            _sessionOutputWriter = sessionOutputWriter;
            _details = details;
        }

        public bool Addnote(Image image)
        {
            return AddNote(NoteTypes.ScreenShot, ImageUtil.BuildStringFromImage(image), DateTime.Now);
        }
        
        public bool AddNote(NoteTypes type, string note)
        {
            return AddNote(type, note, DateTime.Now);
        }

        public bool AddNote(NoteTypes type, string note, DateTime time)
        {
            var myNote = new Note
            {
                Type = type,
                Contents = note,
                Time = time
            };
            return AddNote(myNote);
        }

        public bool AddNote(Note note)
        {
            return _sessionWriter.AddNote(note);
        }

        public bool StartSession()
        {
            _details.StartingTime = DateTime.Now;
            if (!DoesSessionContainStartingNotes()) return false;
            
            var n = _sessionWriter.AddNote(new Note
            {
                Type = NoteTypes.Reporter,
                Contents = _details.Tester,
                Time = _details.StartingTime
            });
            if (!n) return false;

            n = _sessionWriter.AddNote(new Note
            {
                Type = NoteTypes.ScenarioId,
                Contents = _details.ScenarioId,
                Time = _details.StartingTime
            });
            if (!n) return false;

            n = _sessionWriter.AddNote(new Note
            {
                Type = NoteTypes.Charter,
                Contents = _details.Charter,
                Time = _details.StartingTime
            });
            if (!n) return false;

            n = _sessionWriter.AddNote(new Note
            {
                Type = NoteTypes.Environment,
                Contents = _details.Environment,
                Time = _details.StartingTime
            });
            if (!n) return false;

            n = _sessionWriter.AddNote(new Note
            {
                Type = NoteTypes.Versions,
                Contents = _details.Versions,
                Time = _details.StartingTime
            });

            return n;
        }

        public bool ResumeSession()
        {
            _details = _sessionWriter.ResumePausedSession();
            _details.StartingTime = DateTime.Now;
            var n =_sessionWriter.AddNote(new Note
            {
                Type = NoteTypes.Automatic,
                Contents = "Resuming paused session.",
                Time = DateTime.Now
            });
            return n && DoesSessionContainStartingNotes();
        }

        public bool PauseSession()
        {
            var duration = DateTime.Now - _details.StartingTime;
            var n = _sessionWriter.AddNote(new Note
            {
                Type = NoteTypes.Automatic,
                Contents = string.Format("Pausing session. Duration: {0}:{1}:{2}", duration.Hours.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'), duration.Minutes.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'), duration.Seconds.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0')),
                Time = DateTime.Now
            });
            return n && PauseSession();
        }

        public bool CloseSession()
        {
            if (string.Equals(_details.Versions, "")) return true; // incase not started session, dont save
            var duration = DateTime.Now - _details.StartingTime;
            var n = _sessionWriter.AddNote(new Note
            {
                Type = NoteTypes.Automatic,
                Contents = string.Format("Session Ending. Duration: {0}:{1}:{2}", duration.Hours.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'), duration.Minutes.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'), duration.Seconds.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0')),
                Time = DateTime.Now
            });
            return n && _sessionOutputWriter.OutputSession(_sessionWriter.GetAllNotes(), _details) && _sessionWriter.DeleteSessionData();
        }

        public bool DoesSessionContainStartingNotes()
        {
            return !string.IsNullOrWhiteSpace(_details.Tester) && !string.IsNullOrWhiteSpace(_details.Charter) &&
                   !string.IsNullOrWhiteSpace(_details.Versions) && !string.IsNullOrWhiteSpace(_details.Environment) &&
                   !string.IsNullOrWhiteSpace(_details.ScenarioId) && _details.StartingTime != DateTime.MinValue; 
        }

        public bool CanWriterWrite()
        {
            return _sessionWriter.CanIWrite();
        }
    }
}
