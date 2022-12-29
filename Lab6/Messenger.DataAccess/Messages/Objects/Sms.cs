using Messenger.Domains.Exceptions;
using Messenger.Domains.Messages;

namespace Messenger.DataAccess.Messages.Objects;

public class Sms : Message
{
    public Sms(MessageOptions options, string contents) : base(options, contents) { }
    public override void Reply(IMessage replyMessage)
    {
        if (replyMessage is not Sms)
            throw MessageException.SmsException($"Unable to create SMS with type of replyMessage {replyMessage.GetType()}");
        base.Reply(replyMessage);
    }

    public override MessageSerializable AsSerializable()
    {
        SmsSerializable message = new SmsSerializable(Options.AsSerializable(), Contents);
        message.State = State;
        message.ReplyMessage = ReplyMessage?.AsSerializable();
        return message;
    }
}