using System.Text.RegularExpressions;
using Messenger.Config;
using Messenger.Domains;
using Messenger.Domains.Exceptions;

namespace Messenger.DataAccess.Accounts;

public static class AccountValidator
{
    public static void ValidateLogin(string login)
    {
        if (string.IsNullOrWhiteSpace(login))
            throw AccountException.InputEmpty();
        if (login.Length < DefaultValues.Account.MinimalLoginLength)
            throw AccountException.LoginTooShort();
        if (login.Length > DefaultValues.Account.MaximalLoginLength)
            throw AccountException.LoginTooLong();
        if (!Regex.IsMatch(login, RegexMatches.LoginMatch))
            throw AccountException.RegexMissmatch();
    }
    
    public static void ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw AccountException.InputEmpty();
        if (password.Length < DefaultValues.Account.MinimalPasswordLength)
            throw AccountException.RegexMissmatch();
    }
}