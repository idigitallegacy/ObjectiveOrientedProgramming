using Messenger.Application.System.Commands;
using Messenger.DataAccess.Employees;

namespace Messenger.Console.EmployeeMenu;

public class EmployeeLogin
{
    private static Employee? _employee;
    public static void Execute(Application.System.Application application)
    {
        AuthorizeEmployeeCommand command = new AuthorizeEmployeeCommand();
        System.Console.WriteLine("Enter login:");
        command.Login = System.Console.ReadLine();
        System.Console.WriteLine("Enter password:");
        command.Password = System.Console.ReadLine();
        
        _employee = application.AuthorizeEmployee(command);
        if (_employee is null)
        {
            System.Console.WriteLine("Login Incorrect");
            return;
        }

        bool loopFlag = true;
        while (loopFlag)
        {
            System.Console.WriteLine("1 - View messages");
            System.Console.WriteLine("2 - Reply for a message");
            System.Console.WriteLine("3 - Make a report");
            System.Console.WriteLine("0 - Exit");

            string? userAnswer = System.Console.ReadLine();
            switch (userAnswer)
            {
                default: System.Console.WriteLine("Unknown command"); break;
                case "0": loopFlag = false; break;
                case "1": MessageViewer.Execute(application, _employee); break;
                case "2": MessageReplier.Execute(application, _employee); break;
                case "3": ReportMaker.Execute(application, _employee); break;
            }
        }
    }
}