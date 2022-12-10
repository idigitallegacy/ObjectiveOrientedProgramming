using Banks.Models.TimeFlowConcept;

namespace Banks.Models.BankAccountConcept;

public class CreditAccount : BankAccount, IEquatable<CreditAccount>
{
    public CreditAccount(int accountId, decimal creditLimit, double creditRate, double interestRate, DateTime validThru, decimal? withdrawLimit = null)
        : base(accountId, creditLimit,  creditRate, interestRate, validThru, withdrawLimit)
    { }

    public bool Equals(CreditAccount? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return AccountId == other.AccountId;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((CreditAccount)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), AccountId);
    }
}