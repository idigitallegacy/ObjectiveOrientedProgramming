namespace Isu.Extra.Exceptions;

public class SchedulerException : Exception
{
    public SchedulerException(string message = "")
        : base(message)
    {
    }

    public static SchedulerException TimeIsAlreayScheduled(DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime)
    {
        return new SchedulerException(
            $"Unable to schedule day {dayOfWeek} from {startTime} to {endTime}: it's already scheduled.");
    }
}