using Messenger.Domains.Accounts;

namespace Messenger.DataAccess.Accounts;

public class Account
{
    public Account(AccountDetails details)
    {
        AccountDetails = details;
    }
    
    public AccountDetails AccountDetails { get; }

    public AccountSerializable AsSerializable() => new AccountSerializable(AccountDetails.AsSerializable());
}