using System.Collections.Generic;

namespace RapidLib
{
    public interface ISessionWriter
    {
        bool AddNote(Note note);
        SessionDetails ResumePausedSession();
        bool StartSession();
        bool PauseSession();
        List<Note> GetAllNotes();
        bool DeleteSessionData();
        bool CanIWrite();
    }
}