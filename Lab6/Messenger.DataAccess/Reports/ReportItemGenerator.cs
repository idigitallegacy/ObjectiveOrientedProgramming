namespace Messenger.DataAccess.Reports;

public static class ReportItemGenerator
{
    public static ReportItem Generate(string name, long value)
    {
        return new ReportItem(name, value);
    }
}