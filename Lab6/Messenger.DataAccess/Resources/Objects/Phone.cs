using Messenger.DataAccess.Messages.Objects;
using Messenger.Domains.Resources;

namespace Messenger.DataAccess.Resources.Objects;

public class Phone : Resource
{
    public Phone(string name) : base(name) { }
    public override void AddMessage(IMessage message)
    {
        if (message is not Sms)
            throw new NotImplementedException(); // todo
        InternalMessages.Add(message);
    }
    
    
    public override ResourceSerializable AsSerializable()
    {
        return new PhoneSerializable(Name, Messages.Select(message => message.AsSerializable()).ToList());
    }
}