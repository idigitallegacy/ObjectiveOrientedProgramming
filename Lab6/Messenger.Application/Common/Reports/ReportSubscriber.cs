using Messenger.DataAccess.Employees;

namespace Messenger.Application.Common.Reports;

public class ReportSubscriber
{
    public Employee? Employee { get; set; }
    public long StartTime { get; set; }
    public long EndTime { get; set; }
    public int ViewedMessages { get; set; }
    public int RepliedMessages { get; set; }
}