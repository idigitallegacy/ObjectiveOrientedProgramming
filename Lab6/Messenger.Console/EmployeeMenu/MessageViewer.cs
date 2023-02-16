using Messenger.DataAccess.Employees;
using Messenger.DataAccess.Messages.Objects;

namespace Messenger.Console.EmployeeMenu;

public class MessageViewer
{
    public static void Execute(Application.System.Application application, Employee employee)
    {
        List<IMessage> messages = application.ReadNewMessages(employee);
        if (messages.Count == 0)
        {
            System.Console.WriteLine("No new messages");
            return;
        }
        
        messages.ForEach(message =>
        {
            System.Console.WriteLine("/--------------------------/");
            System.Console.WriteLine($"Send time: {message.Options.SendTime}");
            System.Console.WriteLine($"Subject: {message.Options.Subject}");
            System.Console.WriteLine($"Contents: {message.Read()}");
            System.Console.WriteLine($"Id: {message.Id}");
        });
    }
}