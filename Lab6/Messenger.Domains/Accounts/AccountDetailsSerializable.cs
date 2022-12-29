using Messenger.Config;

namespace Messenger.Domains.Accounts;

[Serializable]
public class AccountDetailsSerializable
{
    public AccountDetailsSerializable()
    {
        Login = DefaultValues.Account.DefaultLogin;
        Password = DefaultValues.Account.DefaultPassword;
    }

    public AccountDetailsSerializable(string login, string password)
    {
        Login = login;
        Password = password;
    }
    
    public string Login { get; set; }
    public string Password { get; set; }
}