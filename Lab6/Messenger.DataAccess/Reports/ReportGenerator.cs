using Messenger.DataAccess.Employees;

namespace Messenger.DataAccess.Reports;

public static class ReportGenerator
{
    public static Report Generate(Employee employee)
    {
        return new Report(employee);
    }
}