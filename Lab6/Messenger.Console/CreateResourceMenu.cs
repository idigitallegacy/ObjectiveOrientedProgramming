using Messenger.Application.System.Commands;
using Messenger.DataAccess.Resources.Objects;

namespace Messenger.Console;

public class CreateResourceMenu
{
    private static EMail RegisterEmail(Application.System.Application application)
    {
        CreateResourceCommand command = new CreateResourceCommand();
        System.Console.WriteLine("Enter resource name:");
        command.Name = System.Console.ReadLine();
        return application.RegisterEmailResource(command);
    }
    
    private static Phone RegisterPhone(Application.System.Application application)
    {
        CreateResourceCommand command = new CreateResourceCommand();
        System.Console.WriteLine("Enter resource name:");
        command.Name = System.Console.ReadLine();
        return application.RegisterPhoneResource(command);
    }
    
    private static Social RegisterSocial(Application.System.Application application)
    {
        CreateResourceCommand command = new CreateResourceCommand();
        System.Console.WriteLine("Enter resource name:");
        command.Name = System.Console.ReadLine();
        return application.RegisterSocialResource(command);
    }
    
    public static IResource Execute(Application.System.Application application)
    {
        System.Console.WriteLine("1 - E-Mail resource");
        System.Console.WriteLine("2 - Phone resource");
        System.Console.WriteLine("3 - Social resource");
        string? userAnswer = System.Console.ReadLine();

        switch (userAnswer)
        {
            default: System.Console.WriteLine("Unknown command."); break;
            case "1": return RegisterEmail(application);
            case "2": return RegisterPhone(application);
            case "3": return RegisterSocial(application);
        }

        throw new Exception(); // todo
    }
}