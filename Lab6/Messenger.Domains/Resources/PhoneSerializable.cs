using Messenger.Domains.Messages;

namespace Messenger.Domains.Resources;

[Serializable]
public class PhoneSerializable : ResourceSerializable
{
    public PhoneSerializable() { }
    public PhoneSerializable(string name, List<MessageSerializable> messages) : base(name, messages) { }
}