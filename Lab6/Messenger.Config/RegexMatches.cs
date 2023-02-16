namespace Messenger.Domains;

[Serializable]
public static class RegexMatches
{
    public static readonly string LoginMatch = @"[\w\-_]+";
    public static readonly string TextMatch = @"[\w\s\?\.\,\<\>\!\$\%\*\;\'\\\/]+";
    public static readonly string PhoneMatch = @"[0-9]+";
    public static readonly string EmailMatch = @"[\w\-_]+@[\w]{2,}\.[\w]{2,}";
}