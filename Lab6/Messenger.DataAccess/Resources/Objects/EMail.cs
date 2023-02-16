using Messenger.DataAccess.Messages.Objects;
using Messenger.Domains.Exceptions;
using Messenger.Domains.Resources;

namespace Messenger.DataAccess.Resources.Objects;

public class EMail : Resource
{
    public EMail(string name) : base(name) { } 
    public override void AddMessage(IMessage message)
    {
        if (message is not EMailMessage)
            ResourceException.EMailException($"Unable to add message typed {message.GetType()}");
        InternalMessages.Add(message);
    }

    public override ResourceSerializable AsSerializable()
    {
        return new EMailSerializable(Name, Messages.Select(message => message.AsSerializable()).ToList());
    }
}