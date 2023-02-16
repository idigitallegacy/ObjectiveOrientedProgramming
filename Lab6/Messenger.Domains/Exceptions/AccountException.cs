namespace Messenger.Domains.Exceptions;

public class AccountException : Exception
{
    private AccountException(string message = "") : base(message) { }

    public static AccountException SocialFeedbackException(string message = "") => new AccountException(message);
    public static AccountException SmsException(string message = "") => new AccountException(message);
    public static AccountException EMailException(string message = "") => new AccountException(message);
    public static AccountException LoginTooLong(string message = "") => new AccountException(message);
    public static AccountException RegexMissmatch(string message = "") => new AccountException(message);
    public static AccountException InputEmpty(string message = "") => new AccountException(message);
    public static AccountException LoginTooShort(string message = "") => new AccountException(message);
}