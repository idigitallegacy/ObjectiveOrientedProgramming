using Messenger.Config;
using Messenger.DataAccess.Messages.Objects;
using Messenger.DataAccess.Messages.Validators;
using Messenger.DataAccess.Resources.Objects;
using Messenger.Domains.Exceptions;
using Messenger.Domains.Messages;

namespace Messenger.DataAccess.Messages.Generators;

public static class MessageOptionsGenerator
{
    public static MessageOptions Generate(IResource? resource, string subject)
    {
        if (string.IsNullOrWhiteSpace(subject))
            subject = DefaultValues.Message.DefaultSubject;
        MessageOptionsValidator.ValidateSubject(subject);
        return new MessageOptions(resource, subject);
    }
    
    public static MessageOptions GenerateFromSerialized(MessageOptionsSerializable serialized)
    {
        throw MessageException.DeserializationNotSupported();
    }
}