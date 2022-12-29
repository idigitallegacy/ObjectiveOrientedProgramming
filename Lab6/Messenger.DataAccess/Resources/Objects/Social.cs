using Messenger.DataAccess.Messages.Objects;
using Messenger.Domains.Resources;

namespace Messenger.DataAccess.Resources.Objects;

public class Social : Resource
{
    public Social(string name) : base(name) { }
    
    public override void AddMessage(IMessage message)
    {
        if (message is not SocialFeedback)
            throw new NotImplementedException(); // todo
        InternalMessages.Add(message);
    }
    
    
    public override ResourceSerializable AsSerializable()
    {
        return new SocialSerializable(Name, Messages.Select(message => message.AsSerializable()).ToList());
    }
}