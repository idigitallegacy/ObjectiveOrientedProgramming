using Messenger.DataAccess.Messages.Objects;
using Messenger.Domains.Resources;

namespace Messenger.DataAccess.Resources.Objects;

public class EMail : Resource
{
    public EMail(string name) : base(name) { } 
    public override void AddMessage(IMessage message)
    {
        if (message is not EMailMessage)
            throw new NotImplementedException(); // todo
        InternalMessages.Add(message);
    }

    public override ResourceSerializable AsSerializable()
    {
        return new EMailSerializable(Name, Messages.Select(message => message.AsSerializable()).ToList());
    }
}