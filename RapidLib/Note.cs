using System;

namespace RapidLib
{
    public class Note
    {
        public NoteTypes Type { get; set; }
        public string Contents { get; set; }
        public DateTime Time { get; set; }

        public static string GetTypeName(NoteTypes type)
        {
            switch (type)
            {
                case NoteTypes.Automatic:
                    return "Automatic";
                case NoteTypes.Reporter:
                    return "Reporter";
                case NoteTypes.ScenarioId:
                    return "Scenario ID";
                case NoteTypes.Environment:
                    return "Environment";
                case NoteTypes.Charter:
                    return "Charter";
                case NoteTypes.Versions:
                    return "Versions";
                case NoteTypes.ScreenShot:
                    return "ScreenShot";
                case NoteTypes.PlainTextNote:
                    return "Plain Text Note";
                case NoteTypes.Prerequisite:
                    return "Prerequisite";
                case NoteTypes.Test:
                    return "Test";
                case NoteTypes.Success:
                    return "Success";
                case NoteTypes.Bug:
                    return "Bug/Issue";
                case NoteTypes.Note:
                    return "Note";
                case NoteTypes.FollowUp:
                    return "Follow Up";
                case NoteTypes.Summary:
                    return "Summary";
                default:
                    return "Unknown";
            }
        }
    }
}