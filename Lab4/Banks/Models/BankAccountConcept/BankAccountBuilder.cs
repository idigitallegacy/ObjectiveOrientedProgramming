using Banks.Exceptions;
using Banks.Models.TimeFlowConcept;

namespace Banks.Models.BankAccountConcept;

public class BankAccountBuilder
{
    private const decimal DefaultCreditLimit = 0;
    private const double DefaultCreditRate = 0.0;
    private const double DefaultInterestRate = 0.0;

    private int? _accountId;
    private decimal? _creditLimit;
    private double? _creditRate;
    private double? _interestRate;
    private DateTime? _validThru;
    private TimeFlow _timeFlow = TimeFlow.Instance;
    private decimal? _withdrawLimit;

    public BankAccountBuilder SetId(int id)
    {
        _accountId = id;
        return this;
    }

    public BankAccountBuilder SetCreditLimit(decimal amount)
    {
        _creditLimit = amount;
        return this;
    }

    public BankAccountBuilder SetCreditRate(double rate)
    {
        _creditRate = rate;
        return this;
    }

    public BankAccountBuilder SetInterestRate(double rate)
    {
        _interestRate = rate;
        return this;
    }

    public BankAccountBuilder SetExpirationDate(DateTime expirationDate)
    {
        if (expirationDate <= _timeFlow.Now)
            throw BankAccountBuilderException.WrondExpirationDate();
        _validThru = expirationDate;
        return this;
    }

    public BankAccountBuilder SetWithdrawLimit(decimal limit)
    {
        _withdrawLimit = limit;
        return this;
    }

    public DebitAccount BuildDebitAccount()
    {
        return new DebitAccount(
            _accountId ?? throw BankAccountBuilderException.WrondAccountId(),
            DefaultCreditLimit,
            DefaultCreditRate,
            _interestRate ?? DefaultInterestRate,
            _validThru ?? throw BankAccountBuilderException.WrondExpirationDate(),
            _withdrawLimit);
    }

    public DepositAccount BuildDepositAccount()
    {
        return new DepositAccount(
            _accountId ?? throw BankAccountBuilderException.WrondAccountId(),
            DefaultCreditLimit,
            DefaultCreditRate,
            _interestRate ?? DefaultInterestRate,
            _validThru ?? throw BankAccountBuilderException.WrondExpirationDate(),
            _withdrawLimit);
    }

    public CreditAccount BuildCreditAccount()
    {
        return new CreditAccount(
            _accountId ?? throw BankAccountBuilderException.WrondAccountId(),
            _creditLimit ?? DefaultCreditLimit,
            _creditRate ?? throw BankAccountBuilderException.WrondCreditRate(),
            _interestRate ?? DefaultInterestRate,
            _validThru ?? throw BankAccountBuilderException.WrondExpirationDate(),
            _withdrawLimit);
    }
}