using Banks.Models.BankInterestPolicyConcept;

namespace Banks.Entities.BankConcept;

public interface ICentralBank
{
    public double BaseRate { get; }
    IBank CreateBank(
        double debitInterestRate,
        double creditInterestRate,
        decimal defaultWithdrawLimit,
        double defaultCreditCoefficient,
        List<DepositInterestRange> depositInterestRates);
    void UpdateBaseInterestRate(double newRate);
    void UpdateDebitInterestRate(IBank bank, double newRate);
    void UpdateCreditInterestRate(IBank bank, double newRate);
    void UpdateDepositInterestRate(IBank bank, List<DepositInterestRange> interestRanges);
}