using Banks.Exceptions;
using Banks.Models.TimeFlowConcept;

namespace Banks.Models.BankAccountConcept;

public abstract class BankAccount : IBankAccount, IEquatable<BankAccount>
{
    protected BankAccount(int accountId, decimal creditLimit, double creditRate, double interestRate, DateTime validThru, decimal? withdrawLimit = null)
    {
        TimeFlow = TimeFlow.Instance;
        AccountId = accountId;
        AccountValue = 0;
        CreditLimit = creditLimit;
        CreditRate = creditRate;
        InterestRate = interestRate;
        RegistrationDate = TimeFlow.Now;
        ValidThru = validThru;
        IsActive = validThru > TimeFlow.Now;
        WithdrawLimit = withdrawLimit;
        FrozenMoney = 0;
        LastPayoff = TimeFlow.Now;
    }

    public decimal AccountValue { get; private set; }
    public int AccountId { get; }
    public decimal CreditLimit { get; set; }
    public double CreditRate { get; set; }
    public double InterestRate { get; set; }
    public DateTime RegistrationDate { get; }
    public DateTime ValidThru { get; set; }
    public bool IsActive { get; set; }
    public decimal? WithdrawLimit { get; set; }
    public decimal FrozenMoney { get; set; }
    public DateTime LastPayoff { get; set; }
    protected TimeFlow TimeFlow { get; }

    public virtual void AddMoney(decimal amount)
    {
        if (amount < 0)
        {
            RemoveMoney(Math.Abs(amount));
            return;
        }

        AccountValue += amount;
    }

    public virtual void RemoveMoney(decimal amount)
    {
        if (amount < 0)
        {
            AddMoney(Math.Abs(amount));
            return;
        }

        if (amount > AccountValue + CreditLimit)
            throw BankAccountException.WrongMoneyAmount();
        if (WithdrawLimit is not null && amount > WithdrawLimit)
            throw BankAccountException.WithdrawLimitExceed();
        AccountValue -= amount;
    }

    public virtual void TransferMoney(IBankAccount account, decimal amount)
    {
        if (amount < 0)
            throw BankAccountException.WrongMoneyAmount();
        if (amount > AccountValue + CreditLimit)
            throw BankAccountException.NotEnoughMoney();
        if (WithdrawLimit is not null && amount > WithdrawLimit)
            throw BankAccountException.WithdrawLimitExceed();

        RemoveMoney(amount);
        account.AddMoney(amount);
    }

    public bool Equals(BankAccount? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return AccountId == other.AccountId;
    }

    public bool Equals(IBankAccount? other)
    {
        if (ReferenceEquals(other, null)) return false;
        if (ReferenceEquals(other, this)) return true;
        return other.AccountId.Equals(this.AccountId);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((BankAccount)obj);
    }

    public override int GetHashCode()
    {
        return AccountId;
    }
}