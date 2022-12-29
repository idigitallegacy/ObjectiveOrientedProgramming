using System.Text.RegularExpressions;
using Messenger.Domains;

namespace Messenger.DataAccess.Resources.Validators;

public static class ResourceValidator
{
    public static void ValidateEmailName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception(); // todo
        if (!Regex.IsMatch(name, RegexMatches.EmailMatch))
            throw new Exception(); // todo
    }
    
    public static void ValidatePhoneName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception(); // todo
        if (!Regex.IsMatch(name, RegexMatches.PhoneMatch))
            throw new Exception(); // todo
    }
    
    public static void ValidateSocialName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception(); // todo
        if (!Regex.IsMatch(name, RegexMatches.TextMatch))
            throw new Exception(); // todo
    }
}