using Banks.Models.BankConstructorOptionsConcept;
using Banks.Models.BankInterestPolicyConcept;

namespace Banks.Entities.BankConcept;

public interface ICentralBank
{
    double BaseRate { get; }
    IBank CreateBank(BankConstructorOptions options);
    void UpdateBaseInterestRate(double newRate);
    void UpdateDebitInterestRate(IBank bank, double newRate);
    void UpdateCreditInterestRate(IBank bank, double newRate);
    void UpdateDepositInterestRate(IBank bank, List<DepositInterestRange> interestRanges);
}