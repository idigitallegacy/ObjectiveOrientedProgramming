using Messenger.Domains.Messages;

namespace Messenger.DataAccess.Messages.Objects;

public abstract class Message : IMessage
{
    public Message(MessageOptions options, string contents)
    {
        State = MessageState.New;
        Options = options;
        Contents = contents;
        Id = Guid.NewGuid();
    }
    
    public MessageOptions Options { get; internal set; }
    public MessageState State { get; internal set; }
    public IMessage? ReplyMessage { get; internal set; }
    public Guid Id { get; }
    protected string Contents { get; }
    
    public virtual string Read()
    {
        if (State is MessageState.New)
            State = MessageState.Viewed;
        return Contents;
    }

    public virtual void Reply(IMessage replyMessage)
    {
        State = MessageState.Replied;
        ReplyMessage = replyMessage;
    }

    public virtual MessageSerializable AsSerializable()
    {
        MessageSerializable message = new MessageSerializable(Options.AsSerializable(), Contents);
        message.State = State;
        message.ReplyMessage = ReplyMessage?.AsSerializable();
        return message;
    }

    protected bool Equals(Message other)
    {
        return Options.Equals(other.Options);
    }

    public bool Equals(IMessage? other)
    {
        if (other is not Message)
            return false;
        return Equals((Message)other);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((MessageSerializable)obj);
    }

    public override int GetHashCode()
    {
        return Options.GetHashCode();
    }
}