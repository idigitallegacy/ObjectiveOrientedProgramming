using Banks.Models.BankAccountConcept;
using Banks.Models.BankClientConcept;

namespace Banks.Models.ClientAccountConcept;

public class ClientAccount : IClientAccount
{
    public ClientAccount(BankClient personInfo)
    {
        PersonInfo = personInfo;
    }

    public BankClient PersonInfo { get; }
    public List<IBankAccount> Accounts { get; } = new ();
    public bool IsTrusted
    {
        get
        {
            return PersonInfo.Passport is not null | PersonInfo.Address is not null;
        }
    }

    public bool Equals(IClientAccount? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Equals((ClientAccount)other);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ClientAccount)obj);
    }

    public override int GetHashCode()
    {
        return PersonInfo.GetHashCode();
    }

    protected bool Equals(ClientAccount other)
    {
        return PersonInfo.Equals(other.PersonInfo);
    }
}