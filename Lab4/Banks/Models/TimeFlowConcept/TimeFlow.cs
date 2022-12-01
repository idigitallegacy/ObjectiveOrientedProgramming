using Banks.Exceptions;

namespace Banks.Models.TimeFlowConcept;

public class TimeFlow
{
    private static readonly TimeFlow _instanceHolder = new TimeFlow();

    private TimeFlow()
    {
        Now = DateTime.Now;
    }

    public static TimeFlow Instance => _instanceHolder;
    public DateTime Now { get; private set; }
    public TimeFlowMessageBroker MessageBroker { get; } = new TimeFlowMessageBroker();

    public void SetTime(DateTime newTime)
    {
        TimeSpan difference = newTime - Now;
        if (newTime.CompareTo(Now) <= 0)
            throw TimeFlowException.WrongNewDate();
        Now = newTime;
        MessageBroker.Notify(difference);
    }
}