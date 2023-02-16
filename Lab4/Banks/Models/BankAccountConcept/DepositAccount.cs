using Banks.Exceptions;
using Banks.Models.TimeFlowConcept;

namespace Banks.Models.BankAccountConcept;

public class DepositAccount : BankAccount, IEquatable<DepositAccount>
{
    public DepositAccount(int accountId, decimal creditLimit, double creditRate, double interestRate, DateTime validThru, decimal? withdrawLimit = null)
        : base(accountId, creditLimit,  creditRate, interestRate, validThru, withdrawLimit)
    { }

    public override void RemoveMoney(decimal amount)
    {
        if (TimeFlow.Now < ValidThru)
            throw BankAccountException.OperationNotPermitted();

        base.RemoveMoney(amount);
    }

    public override void TransferMoney(IBankAccount account, decimal amount)
    {
        if (TimeFlow.Now < ValidThru)
            throw BankAccountException.OperationNotPermitted();

        base.TransferMoney(account, amount);
    }

    public bool Equals(DepositAccount? other)
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
        return Equals((DepositAccount)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), AccountId);
    }
}