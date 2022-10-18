namespace Isu.Extra.Exceptions;

public class SchedulerException : Exception
{
    public SchedulerException(string message = "")
        : base(message)
    {
    }

    public static SchedulerException TimeIsAlreadyScheduled(
        DayOfWeek? dayOfWeek = null,
        TimeSpan? startTime = null,
        TimeSpan? endTime = null)
    {
        return new SchedulerException(
            $"Unable to schedule day {dayOfWeek} from {startTime} to {endTime}: it's already scheduled.");
    }

    public static SchedulerException TimeIsNotScheduled(
        DayOfWeek? dayOfWeek = null,
        TimeSpan? startTime = null,
        TimeSpan? endTime = null)
    {
        return new SchedulerException(
            $"Unable to unschedule day {dayOfWeek} from {startTime} to {endTime}: it's not scheduled.");
    }

    public static SchedulerException WrongAudienceNumber(int number)
    {
        return new SchedulerException($"Unable to place lesson at audience #{number}.");
    }
}