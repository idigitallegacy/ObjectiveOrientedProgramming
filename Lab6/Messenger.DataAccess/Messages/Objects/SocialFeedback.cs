using Messenger.Domains.Exceptions;
using Messenger.Domains.Messages;

namespace Messenger.DataAccess.Messages.Objects;

public class SocialFeedback : Message
{
    public SocialFeedback(MessageOptions options, string contents) : base(options, contents) { }
    public override void Reply(IMessage replyMessage)
    {
        if (replyMessage is not SocialFeedback)
            throw MessageException.SocialFeedbackException($"Unable to create social feedback with type of replyMessage {replyMessage.GetType()}");
        base.Reply(replyMessage);
    }

    public override MessageSerializable AsSerializable()
    {       
        SocialFeedbackSerializable message = new SocialFeedbackSerializable(Options.AsSerializable(), Contents);
        message.State = State;
        message.ReplyMessage = ReplyMessage?.AsSerializable();
        return message;
    }
}