using System.Text.RegularExpressions;
using Messenger.Config;
using Messenger.Domains;

namespace Messenger.DataAccess.Accounts;

public static class AccountValidator
{
    public static void ValidateLogin(string login)
    {
        if (string.IsNullOrWhiteSpace(login))
            throw new Exception(); // todo
        if (login.Length < DefaultValues.Account.MinimalLoginLength)
            throw new Exception(); // todo
        if (login.Length > DefaultValues.Account.MaximalLoginLength)
            throw new Exception(); // todo
        if (!Regex.IsMatch(login, RegexMatches.LoginMatch))
            throw new Exception(); // todo
    }
    
    public static void ValidatePassword(string login)
    {
        if (string.IsNullOrWhiteSpace(login))
            throw new Exception(); // todo
        if (login.Length < DefaultValues.Account.MinimalPasswordLength)
            throw new Exception(); // todo
    }
}