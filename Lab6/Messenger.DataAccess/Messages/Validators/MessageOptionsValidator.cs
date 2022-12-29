using System.Text.RegularExpressions;
using Messenger.Domains;

namespace Messenger.DataAccess.Messages.Validators;

public static class MessageOptionsValidator
{
    public static void ValidateSubject(string subject)
    {
        if (!Regex.IsMatch(subject, RegexMatches.TextMatch))
            throw new Exception(); // todo
    }
}