using Banks.Models.BankInterestPolicyConcept;

namespace Banks.Models.BankConstructorOptionsConcept;

public class BankConstructorOptions
{
    public BankConstructorOptions(
        double debitInterestRate,
        double creditInterestRate,
        decimal defaultWithdrawLimit,
        double defaultCreditCoefficient,
        List<DepositInterestRange> depositInterestRates)
    {
        DebitInterestRate = debitInterestRate;
        CreditInterestRate = creditInterestRate;
        DefaultWithdrawLimit = defaultWithdrawLimit;
        DefaultCreditCoefficient = defaultCreditCoefficient;
        DepositInterestRates = depositInterestRates;
    }

    public double DebitInterestRate { get; }
    public double CreditInterestRate { get; }
    public decimal DefaultWithdrawLimit { get; }
    public double DefaultCreditCoefficient { get; }
    public List<DepositInterestRange> DepositInterestRates { get; }
}