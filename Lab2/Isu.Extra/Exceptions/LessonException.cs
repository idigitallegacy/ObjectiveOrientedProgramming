namespace Isu.Extra.Exceptions;

public class LessonException : Exception
{
    private LessonException(string message = "")
        : base(message) { }

    public static LessonException NoAssignee()
    {
        return new LessonException("No assignee: assign group or stream with lesson first.");
    }

    public static LessonException NoTeacherOrAudience()
    {
        return new LessonException("Unable to build lesson: assign teacher and audience first.");
    }
}