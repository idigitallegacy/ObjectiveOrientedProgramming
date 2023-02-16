using Messenger.DataAccess.Messages.Generators;
using Messenger.DataAccess.Messages.Objects;
using Messenger.DataAccess.Resources.Objects;

namespace Messenger.Console;

public class ResourceMessageImitator
{
    public static void Execute(Application.System.Application application, List<IResource> resources)
    {
        System.Console.WriteLine("Enter resource name:");
        IResource resource = resources.First(needleResource => needleResource.Name.Equals(System.Console.ReadLine()));
        System.Console.WriteLine("Enter subject:");
        string? subject = System.Console.ReadLine();

        MessageOptions options = MessageOptionsGenerator.Generate(resource, subject);
        IMessage? message = null;
        System.Console.WriteLine("Enter message contents:");
        
        if (resource is EMail)
            message = MessageGenerator.GenerateEMailMessage(options, System.Console.ReadLine());
        if (resource is Phone)
            message = MessageGenerator.GenerateSms(options, System.Console.ReadLine());
        if (resource is Social)
            message = MessageGenerator.GenerateSocialFeedback(options, System.Console.ReadLine());
        
        resource.AddMessage(message);
    }
}