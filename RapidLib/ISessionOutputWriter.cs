﻿using System.Collections.Generic;

namespace RapidLib
{
    public interface ISessionOutputWriter
    {
        bool OutputSession(List<Note> notes, SessionDetails details);
        List<Note> InputSessionFromOutput(out SessionDetails details);
    }
}