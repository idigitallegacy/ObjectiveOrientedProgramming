using System.Text.RegularExpressions;
using Messenger.Config;
using Messenger.Domains;
using Messenger.Domains.Exceptions;

namespace Messenger.DataAccess.Messages.Validators;

public static class MessageValidator
{
    public static void ValidateSocialFeedbackContents(string contents)
    {
        if (string.IsNullOrWhiteSpace(contents))
            throw MessageException.InputEmpty();
        if (!Regex.IsMatch(contents, RegexMatches.TextMatch))
            throw MessageException.RegexMissmatch("Specified content doesn't match text pattern.");
    }

    public static void ValidateSmsContents(string contents)
    {
        
        if (string.IsNullOrWhiteSpace(contents))
            throw MessageException.InputEmpty();
        if (contents.Length > DefaultValues.SMS.MaxDigitsAmount)
            throw MessageException.ContentOverflow("Content exceeds digit limit.");
        if (!Regex.IsMatch(contents, RegexMatches.TextMatch))
            throw MessageException.RegexMissmatch("Specified string doesn't match text pattern.");
    }

    public static void ValidateEMailContents(string contents)
    {
        if (string.IsNullOrWhiteSpace(contents))
            throw MessageException.InputEmpty();
        if (!Regex.IsMatch(contents, RegexMatches.TextMatch))
            throw MessageException.RegexMissmatch("Specified string doesn't match text pattern.");
    }
}