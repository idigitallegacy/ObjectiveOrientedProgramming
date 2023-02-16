using Messenger.Config;
using Messenger.DataAccess.Resources.Objects;
using Messenger.Domains.Messages;
using Messenger.Domains.Times;

namespace Messenger.DataAccess.Messages.Objects;

public class MessageOptions
{
    public MessageOptions()
    {
        Subject = DefaultValues.Message.DefaultSubject;
        SendTime = TimeFlow.Instance.Now;
        MessageId = Guid.NewGuid();
    }
    public MessageOptions(IResource? resource, string subject)
    {
        Subject = subject;
        Resource = resource;
        SendTime = TimeFlow.Instance.Now;
        MessageId = Guid.NewGuid();
    }
    public IResource? Resource { get; internal set; }
    public string Subject { get; internal set; }
    public DateTime SendTime { get; internal set; }
    public Guid MessageId { get; internal set; }

    public MessageOptionsSerializable AsSerializable()
    {
        MessageOptionsSerializable options = new MessageOptionsSerializable(Resource?.AsSerializable(), Subject);
        options.MessageId = MessageId;
        options.SendTime = SendTime;
        return options;
    }
}