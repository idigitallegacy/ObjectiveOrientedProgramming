using Banks.Models.BankConstructorOptionsConcept;
using Banks.Models.BankInterestPolicyConcept;
using Banks.Models.TimeFlowConcept;

namespace Banks.Entities.BankConcept;

public class CentralBank : ICentralBank
{
    private static readonly ICentralBank _instanceHolder = new CentralBank();
    private List<IBank> _banks = new ();

    private CentralBank()
    {
        BaseRate = 1.0;
    }

    public double BaseRate { get; private set; }

    public static ICentralBank GetInstance() => _instanceHolder;

    public IBank CreateBank(BankConstructorOptions options)
    {
        IBank bank = new Bank(BaseRate, options);
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