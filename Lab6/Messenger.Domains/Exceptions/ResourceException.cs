namespace Messenger.Domains.Exceptions;

public class ResourceException : Exception
{
    private ResourceException(string message = "") : base(message) { }

    public static ResourceException SocialException(string message = "") => new ResourceException(message);
    public static ResourceException PhoneException(string message = "") => new ResourceException(message);
    public static ResourceException EMailException(string message = "") => new ResourceException(message);
    public static ResourceException DeserializationNotSupported(string message = "") => new ResourceException(message);
    public static ResourceException RegexMissmatch(string message = "") => new ResourceException(message);
    public static ResourceException InputEmpty(string message = "") => new ResourceException(message);
    public static ResourceException ContentOverflow(string message = "") => new ResourceException(message);
}