using Messenger.Config;
using Messenger.Domains.Resources;
using Messenger.Domains.Times;

namespace Messenger.Domains.Messages;

[Serializable]
public class MessageOptionsSerializable
{
    public MessageOptionsSerializable()
    {
        Subject = DefaultValues.Message.DefaultSubject;
        SendTime = TimeFlow.Instance.Now;
        MessageId = Guid.NewGuid();
    }
    public MessageOptionsSerializable(ResourceSerializable? resource, string subject)
    {
        Subject = subject;
        Resource = resource;
        SendTime = TimeFlow.Instance.Now;
        MessageId = Guid.NewGuid();
    }
    public ResourceSerializable? Resource { get; set; }
    public string Subject { get; set; }
    public DateTime SendTime { get; set; }
    public Guid MessageId { get; set; }
}