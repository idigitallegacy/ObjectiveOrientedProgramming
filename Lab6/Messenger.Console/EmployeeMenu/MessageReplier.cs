using Messenger.Application.System.Commands;
using Messenger.DataAccess.Employees;
using Messenger.DataAccess.Messages.Objects;
using Messenger.DataAccess.Resources.Objects;

namespace Messenger.Console.EmployeeMenu;

public class MessageReplier
{
    public static void Execute(Application.System.Application application, Employee employee)
    {
        WriteReplyCommand command = new WriteReplyCommand();
        
        command.Employee = employee;
        System.Console.WriteLine("Enter message id:");
        command.MessageId = System.Console.ReadLine();
        System.Console.WriteLine("Enter message subject:");
        command.Subject = System.Console.ReadLine();
        System.Console.WriteLine("Enter message contents:");
        command.Contents = System.Console.ReadLine();

        IMessage originalMessage = application.GetMessageById(command.MessageId);
        if (originalMessage is EMailMessage)
        {
            application.WriteEmailReply(command);
            return;
        }

        if (originalMessage is Sms)
        {
            application.WritePhoneReply(command);
            return;
        }

        if (originalMessage is SocialFeedback)
            application.WriteSocialReply(command);
    }
}