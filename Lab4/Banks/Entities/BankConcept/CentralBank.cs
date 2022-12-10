using Banks.Models.BankInterestPolicyConcept;
using Banks.Models.TimeFlowConcept;

namespace Banks.Entities.BankConcept;

public class CentralBank : ICentralBank
{
    private List<IBank> _banks = new ();

    public CentralBank(double baseRate)
    {
        BaseRate = baseRate;
    }

    public double BaseRate { get; private set; }

    public IBank CreateBank(
        double debitInterestRate,
        double creditInterestRate,
        decimal defaultWithdrawLimit,
        double defaultCreditCoefficient,
        List<DepositInterestRange> depositInterestRates)
    {
        IBank bank = new Bank(
            BaseRate,
            debitInterestRate,
            creditInterestRate,
            defaultWithdrawLimit,
            defaultCreditCoefficient,
            depositInterestRates);
        _banks.Add(bank);
        TimeFlow.Instance.MessageBroker.AddSubscriber(bank);
        return bank;
    }

    public void UpdateBaseInterestRate(double newRate)
    {
        BaseRate = newRate;
        _banks.ForEach(bank => bank.AcceptNewBaseRate(newRate));
    }

    public void UpdateDebitInterestRate(IBank bank, double newRate)
    {
        _banks.First(needleBank => needleBank.Equals(bank)).AcceptNewDebitInterestRate(newRate);
    }

    public void UpdateCreditInterestRate(IBank bank, double newRate)
    {
        _banks.First(needleBank => needleBank.Equals(bank)).AcceptNewDebitInterestRate(newRate);
    }

    public void UpdateDepositInterestRate(IBank bank, List<DepositInterestRange> interestRanges)
    {
        _banks.First(needleBank => needleBank.Equals(bank)).AcceptNewDepositInterestRates(interestRanges);
    }
}