namespace Banks.Models.BankAccountConcept;

public interface IBankAccount : IEquatable<IBankAccount>
{
    decimal AccountValue { get; }
    int AccountId { get; }
    decimal CreditLimit { get; }
    double CreditRate { get; }
    double InterestRate { get; set; }
    DateTime RegistrationDate { get; }
    DateTime ValidThru { get; }
    bool IsActive { get; set; }
    decimal? WithdrawLimit { get; set; }
    decimal FrozenMoney { get; set; }
    DateTime LastPayoff { get; set; }
    void AddMoney(decimal amount);
    void RemoveMoney(decimal amount);
    void TransferMoney(IBankAccount account, decimal amount);
}