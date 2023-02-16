using Messenger.Domains.Messages;

namespace Messenger.Domains.Resources;

[Serializable]
public class SocialSerializable : ResourceSerializable
{
    public SocialSerializable() { }
    public SocialSerializable(string name, List<MessageSerializable> messages) : base(name, messages) { }
}