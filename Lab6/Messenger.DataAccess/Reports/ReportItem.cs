namespace Messenger.DataAccess.Reports;

public class ReportItem
{
    public ReportItem(string name, long value)
    {
        Name = name;
        Value = value;
    }
    
    public string Name { get; }
    public long Value { get; }
}