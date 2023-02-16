using Messenger.DataAccess.Employees;
using Messenger.DataAccess.Reports;

namespace Messenger.Console.EmployeeMenu;

public class ReportMaker
{
    public static void Execute(Application.System.Application application, Employee employee)
    {
        try
        {
            List<Report> reports = application.GetReports(employee);
            reports.ForEach(report =>
            {
                System.Console.WriteLine("/---------------------------/");
                System.Console.WriteLine($"UserId: {report.Employee?.AccountDetails.Login}");
                report.ReportItems.ForEach(item =>
                {
                    System.Console.WriteLine($"{item.Name}: {item.Value}");
                });
            });
        }
        catch (Exception)
        {
            System.Console.WriteLine("Something went wrong.");
        }
    }
}