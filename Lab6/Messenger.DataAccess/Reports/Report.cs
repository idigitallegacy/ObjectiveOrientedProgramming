using Messenger.DataAccess.Employees;

namespace Messenger.DataAccess.Reports;

public class Report
{
    public Report(Employee employee)
    {
        Employee = employee;
        ReportItems = new List<ReportItem>();
    }
    
    public Employee? Employee { get; }
    public List<ReportItem> ReportItems { get; }

    public void AddReportItem(ReportItem reportItem)
    {
        ReportItems.Add(reportItem);
    }
}