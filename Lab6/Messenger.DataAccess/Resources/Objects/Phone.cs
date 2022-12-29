using Messenger.DataAccess.Messages.Objects;
using Messenger.Domains.Exceptions;
using Messenger.Domains.Resources;

namespace Messenger.DataAccess.Resources.Objects;

public class Phone : Resource
{
    public Phone(string name) : base(name) { }
    public override void AddMessage(IMessage message)
    {
        if (message is not Sms)
            ResourceException.PhoneException($"Unable to add message typed {message.GetType()}");
        InternalMessages.Add(message);
    }
    
    
    public override ResourceSerializable AsSerializable()
    {
        return new PhoneSerializable(Name, Messages.Select(message => message.AsSerializable()).ToList());
    }
}