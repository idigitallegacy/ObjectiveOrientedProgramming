using Messenger.Domains.Messages;

namespace Messenger.Domains.Resources;

[Serializable]
public class EMailSerializable : ResourceSerializable
{
    public EMailSerializable() { }
    public EMailSerializable(string name, List<MessageSerializable> messages) : base(name, messages) { }
}