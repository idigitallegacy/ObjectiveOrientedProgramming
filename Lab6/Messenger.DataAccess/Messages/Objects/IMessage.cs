using Messenger.Domains.Messages;

namespace Messenger.DataAccess.Messages.Objects;

public interface IMessage : IEquatable<IMessage>
{
    MessageOptions Options { get; }
    MessageState State { get; }
    IMessage? ReplyMessage { get; }
    Guid Id { get; }
    string Read();
    void Reply(IMessage replyMessage);
    MessageSerializable AsSerializable();
}