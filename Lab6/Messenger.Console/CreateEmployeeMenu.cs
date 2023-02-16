using Messenger.Application.System.Commands;

namespace Messenger.Console;

public class CreateEmployeeMenu
{
    public static void Execute(Application.System.Application application)
    {
        CreateEmployeeCommand command = new CreateEmployeeCommand();
        System.Console.WriteLine("Enter Login:");
        command.Login = System.Console.ReadLine();
        System.Console.WriteLine("Enter password:");
        command.Password = System.Console.ReadLine();
        application.RegisterEmployee(command);
    }
}