using Banks.Models.BankInterestPolicyConcept;

namespace Banks.Models.BankConstructorOptionsConcept;

public class BankConstructorOptionsBuilder
{
    private double _debitInterestRate;
    private double _creditInterestRate;
    private decimal _defaultWithdrawLimit;
    private double _defaultCreditCoefficient;
    private List<DepositInterestRange> _depositInterestRates = new List<DepositInterestRange>();

    public BankConstructorOptionsBuilder SetDebitInterestRate(double debitInterestRate)
    {
        _debitInterestRate = debitInterestRate;
        return this;
    }

    public BankConstructorOptionsBuilder SetCreditInterestRate(double creditInterestRate)
    {
        _creditInterestRate = creditInterestRate;
        return this;
    }

    public BankConstructorOptionsBuilder SetDefaultWithdrawLimit(decimal defaultWithdrawLimit)
    {
        _defaultWithdrawLimit = defaultWithdrawLimit;
        return this;
    }

    public BankConstructorOptionsBuilder SetDefaultCreditCoefficient(double defaultCreditCoefficient)
    {
        _defaultCreditCoefficient = defaultCreditCoefficient;
        return this;
    }

    public BankConstructorOptionsBuilder SetDepositInterestRates(List<DepositInterestRange> rates)
    {
        _depositInterestRates = rates;
        return this;
    }

    public BankConstructorOptions Build()
    {
        return new BankConstructorOptions(
            _debitInterestRate,
            _creditInterestRate,
            _defaultWithdrawLimit,
            _defaultCreditCoefficient,
            _depositInterestRates);
    }
}