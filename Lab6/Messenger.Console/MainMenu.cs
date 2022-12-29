using Messenger.Console.EmployeeMenu;
using Messenger.DataAccess.Resources.Objects;

namespace Messenger.Console;

public class MainMenu
{
    public static void Execute(Application.System.Application application)
    {
        bool loopFlag = true;
        List<IResource> resources = new List<IResource>();

        while (loopFlag)
        {
            System.Console.WriteLine("1 - Create Employee");
            System.Console.WriteLine("2 - Create Resource");
            System.Console.WriteLine("3 - Log in");
            System.Console.WriteLine("4 - Imitate resource messages update");
            System.Console.WriteLine("0 - Exit");

            string? userAnswer = System.Console.ReadLine();
            switch (userAnswer)
            {
                default: System.Console.WriteLine("Unknown command"); break;
                case "0": loopFlag = false; break;
                case "1": CreateEmployeeMenu.Execute(application); break;
                case "2": resources.Add(CreateResourceMenu.Execute(application)); break;
                case "3": EmployeeLogin.Execute(application); break;
                case "4": ResourceMessageImitator.Execute(application, resources); break;
            }
        }
    }
}