using Messenger.Domains.Exceptions;
using Messenger.Domains.Messages;

namespace Messenger.DataAccess.Messages.Objects;

public class EMailMessage : Message
{
    public EMailMessage(MessageOptions options, string contents) : base(options, contents) { }
    
    public override void Reply(IMessage replyMessage)
    {
        if (replyMessage is not EMailMessage)
            throw MessageException.EMailException($"Unable to create E-Mail reply with type of replyMessage {replyMessage.GetType()}");
        base.Reply(replyMessage);
    }

    public override MessageSerializable AsSerializable()
    {
        EMailMessageSerializable message = new EMailMessageSerializable(Options.AsSerializable(), Contents);
        message.State = State;
        message.ReplyMessage = ReplyMessage?.AsSerializable();
        return message;
    }
}