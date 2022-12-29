namespace Messenger.Domains.Exceptions;

public class MessageException : Exception
{
    private MessageException(string message = "") : base(message) { }

    public static MessageException SocialFeedbackException(string message = "") => new MessageException(message);
    public static MessageException SmsException(string message = "") => new MessageException(message);
    public static MessageException EMailException(string message = "") => new MessageException(message);
    public static MessageException DeserializationNotSupported(string message = "") => new MessageException(message);
    public static MessageException RegexMissmatch(string message = "") => new MessageException(message);
    public static MessageException InputEmpty(string message = "") => new MessageException(message);
    public static MessageException ContentOverflow(string message = "") => new MessageException(message);
}