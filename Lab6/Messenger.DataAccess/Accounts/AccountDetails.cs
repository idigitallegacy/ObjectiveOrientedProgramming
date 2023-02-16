using Messenger.Domains.Accounts;

namespace Messenger.DataAccess.Accounts;

public class AccountDetails
{
    public AccountDetails(string login, string password)
    {
        Login = login;
        Password = password;
    }
    
    public string Login { get; }
    public string Password { get; }
    
    public AccountDetailsSerializable AsSerializable() => new AccountDetailsSerializable(Login, Password);
}