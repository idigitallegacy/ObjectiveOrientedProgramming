namespace Messenger.Domains.Accounts;

[Serializable]
public class AccountSerializable
{
    public AccountSerializable()
    {
        AccountDetails = new AccountDetailsSerializable();
    }
    
    public AccountSerializable(AccountDetailsSerializable accountDetails)
    {
        AccountDetails = accountDetails;
    }

    public AccountDetailsSerializable AccountDetails { get; set; }
}