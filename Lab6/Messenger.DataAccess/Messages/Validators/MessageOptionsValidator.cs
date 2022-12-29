using System.Text.RegularExpressions;
using Messenger.Domains;
using Messenger.Domains.Exceptions;

namespace Messenger.DataAccess.Messages.Validators;

public static class MessageOptionsValidator
{
    public static void ValidateSubject(string subject)
    {
        if (!Regex.IsMatch(subject, RegexMatches.TextMatch))
            throw MessageException.RegexMissmatch("Specified string doesn't match text pattern.");
    }
}