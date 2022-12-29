using Messenger.DataAccess.Messages.Objects;
using Messenger.DataAccess.Messages.Validators;
using Messenger.Domains.Exceptions;
using Messenger.Domains.Messages;

namespace Messenger.DataAccess.Messages.Generators;

public class MessageGenerator
{
    public static SocialFeedback GenerateSocialFeedback(MessageOptions options, string contents)
    {
        MessageValidator.ValidateSocialFeedbackContents(contents);
        return new SocialFeedback(options, contents);
    }
    
    public static Sms GenerateSms(MessageOptions options, string contents)
    {
        MessageValidator.ValidateSmsContents(contents);
        return new Sms(options, contents);
    }
    
    public static EMailMessage GenerateEMailMessage(MessageOptions options, string contents)
    {
        MessageValidator.ValidateEMailContents(contents);
        return new EMailMessage(options, contents);
    }
    
    public static SocialFeedback GenerateSocialFeedbackFromSerialized(MessageSerializable serialized)
    {
        throw MessageException.DeserializationNotSupported();
    }
    
    public static Sms GenerateSmsFromSerialized(MessageSerializable serialized)
    {
        throw MessageException.DeserializationNotSupported();
    }

    public static EMailMessage GenerateEMailMessageFromSerialized(MessageSerializable serialized)
    {
        throw MessageException.DeserializationNotSupported();
    }
}