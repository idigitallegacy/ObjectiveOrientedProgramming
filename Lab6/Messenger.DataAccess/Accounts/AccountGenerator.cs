using Messenger.Domains.Accounts;

namespace Messenger.DataAccess.Accounts;

public static class AccountGenerator
{
    public static Account Generate(string login, string password)
    {
        AccountValidator.ValidateLogin(login);
        AccountValidator.ValidatePassword(password);
        return new Account(new AccountDetails(login, password));
    }

    public static Account GenerateFromSerialized(AccountSerializable serialized)
    {
        return new Account(new AccountDetails(serialized.AccountDetails.Login, serialized.AccountDetails.Password));
    }
}