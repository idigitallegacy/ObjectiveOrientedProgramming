using Messenger.Config;
using Messenger.Domains.Messages;

namespace Messenger.Domains.Resources;

[Serializable]
public class ResourceSerializable
{
    public ResourceSerializable()
    {
        Name = DefaultValues.Resource.DefaultName;
        Messages = new List<MessageSerializable>();
    }
    public ResourceSerializable(string name, List<MessageSerializable> messages)
    {
        Name = name;
        Messages =  messages;
    }
    public string Name { get; set; }
    public List<MessageSerializable> Messages { get; set; }
}