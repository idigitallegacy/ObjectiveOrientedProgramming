namespace Messenger.Domains.Times;

public class TimeFlow
{
    private static readonly TimeFlow InstanceHolder = new TimeFlow();
    private TimeFlow() {}
    
    public static TimeFlow Instance => InstanceHolder;
    public DateTime Now { get; private set; }
}