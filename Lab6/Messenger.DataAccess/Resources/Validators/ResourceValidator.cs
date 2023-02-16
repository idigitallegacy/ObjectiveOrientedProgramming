using System.Text.RegularExpressions;
using Messenger.Domains;
using Messenger.Domains.Exceptions;

namespace Messenger.DataAccess.Resources.Validators;

public static class ResourceValidator
{
    public static void ValidateEmailName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            ResourceException.InputEmpty();
        if (!Regex.IsMatch(name, RegexMatches.EmailMatch))
            ResourceException.RegexMissmatch();
    }
    
    public static void ValidatePhoneName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            ResourceException.InputEmpty();
        if (!Regex.IsMatch(name, RegexMatches.PhoneMatch))
            ResourceException.RegexMissmatch();
    }
    
    public static void ValidateSocialName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            ResourceException.InputEmpty();
        if (!Regex.IsMatch(name, RegexMatches.TextMatch))
            ResourceException.RegexMissmatch();
    }
}