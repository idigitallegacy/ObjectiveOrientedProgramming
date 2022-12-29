using System.Text.RegularExpressions;
using Messenger.Config;
using Messenger.Domains;

namespace Messenger.DataAccess.Messages.Validators;

public static class MessageValidator
{
    public static void ValidateSocialFeedbackContents(string contents)
    {
        if (string.IsNullOrWhiteSpace(contents))
            throw new Exception(); // todo
        if (!Regex.IsMatch(contents, RegexMatches.TextMatch))
            throw new Exception(); // todo
    }

    public static void ValidateSmsContents(string contents)
    {
        
        if (string.IsNullOrWhiteSpace(contents))
            throw new Exception(); // todo
        if (contents.Length > DefaultValues.SMS.MaxDigitsAmount)
            throw new Exception(); // todo
        if (!Regex.IsMatch(contents, RegexMatches.TextMatch))
            throw new Exception(); // todo
    }

    public static void ValidateEMailContents(string contents)
    {
        if (string.IsNullOrWhiteSpace(contents))
            throw new Exception(); // todo
        if (!Regex.IsMatch(contents, RegexMatches.TextMatch))
            throw new Exception(); // todo
    }
}