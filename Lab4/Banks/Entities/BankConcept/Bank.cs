using Banks.Exceptions;
using Banks.Models.AddressConcept;
using Banks.Models.BankAccountConcept;
using Banks.Models.BankClientConcept;
using Banks.Models.BankInterestPolicyConcept;
using Banks.Models.ClientAccountConcept;
using Banks.Models.PassportConcept;
using Banks.Models.TimeFlowConcept;
using Banks.Models.TransactionConcept;

namespace Banks.Entities.BankConcept;

public class Bank : IBank
{
    private TimeFlow _timeFlow = TimeFlow.Instance;
    private int _lastClientId = 0;
    private int _lastAccountId = 0;
    private int _lastTransactionId = 0;

    private double _baseRate;
    private decimal _defaultWithdrawLimit;
    private double _defaultCreditCoefficient;
    private List<IClientAccount> _clientAccounts = new ();
    private List<Transaction> _transactions = new ();
    private BankMessageBroker _messageBroker = new ();

    public Bank(
        double baseRate,
        double debitInterestRate,
        double creditInterestRate,
        decimal defaultWithdrawLimit,
        double defaultCreditCoefficient,
        List<DepositInterestRange> depositInterestRates)
    {
        _baseRate = baseRate;
        _defaultWithdrawLimit = defaultWithdrawLimit;
        _defaultCreditCoefficient = defaultCreditCoefficient;
        InterestPolicy.DebitInterest = baseRate - debitInterestRate;
        InterestPolicy.CreditInterest = baseRate + creditInterestRate;
        InterestPolicy.DepositInterest.AddRange(depositInterestRates);
        InterestPolicy.DepositInterest.ForEach(rate => rate.InterestRate = baseRate - rate.InterestRate);
    }

    public Guid BankId { get; } = Guid.NewGuid();
    public BankInterestPolicy InterestPolicy { get; } = new BankInterestPolicy();

    public BankClient RegisterClient(string name, string surname, Passport? passport = null, Address? address = null)
    {
        BankClientBuilder bankClientBuilder = new BankClientBuilder()
            .SetId(_lastClientId)
            .SetName(name)
            .SetSurname(surname);

        if (address is not null)
        {
            bankClientBuilder
                .SetAddressCountry(address.Country)
                .SetAddressCity(address.City)
                .SetAddressStreet(address.Street)
                .SetAddressBuildingNumber(address.BuildingNumber)
                .SetAddressFlat(address.Flat);
        }

        if (passport is not null)
        {
            bankClientBuilder
                .SetPassportSeries(passport.Series)
                .SetPassportNumber(passport.Number)
                .SetPassportGivenBy(passport.GivenBy)
                .SetPassportDivisionCode(passport.DivisionCode);
        }

        BankClient bankClient = bankClientBuilder.Build();
        ClientAccount clientAccount = new ClientAccount(bankClient);
        _lastClientId++;
        _clientAccounts.Add(clientAccount);
        return bankClient;
    }

    public BankClient AddClientData(BankClient client, Passport? newPassport = null, Address? newAddress = null)
    {
        IClientAccount bankAccount = _clientAccounts.First(needleAccount => needleAccount.PersonInfo.Equals(client));
        bankAccount.PersonInfo.Passport = newPassport;
        bankAccount.PersonInfo.Address = newAddress;
        if (bankAccount.IsTrusted)
            bankAccount.Accounts.ForEach(account => account.WithdrawLimit = null);
        return bankAccount.PersonInfo;
    }

    public Transaction? FindTransactionById(int transactionId)
    {
        return _transactions.FirstOrDefault(transaction => transaction.TransactionId.Equals(transactionId));
    }

    public BankClient? FindClientById(int clientId)
    {
        return _clientAccounts.FirstOrDefault(account => account.PersonInfo.ClientId.Equals(clientId))?.PersonInfo;
    }

    public IBankAccount? FindAccountById(BankClient client, int accountId)
    {
        return _clientAccounts
            .FirstOrDefault(account => account.PersonInfo.Equals(client))?.Accounts
            .FirstOrDefault(account => account.AccountId.Equals(accountId));
    }

    public IBankAccount RegisterDebitAccount(BankClient client, int expirationLength)
    {
        DateTime expirationDate = new DateTime(_timeFlow.Now.Year + expirationLength, _timeFlow.Now.Month, _timeFlow.Now.Day);
        IClientAccount clientAccount = _clientAccounts.First(needleAccount => needleAccount.PersonInfo.Equals(client));
        double interestRate = InterestPolicy.DebitInterest;

        BankAccountBuilder accountBuilder = new BankAccountBuilder()
            .SetId(_lastAccountId)
            .SetExpirationDate(expirationDate)
            .SetInterestRate(interestRate);

        if (!clientAccount.IsTrusted)
            accountBuilder.SetWithdrawLimit(_defaultWithdrawLimit);

        DebitAccount newAccount = accountBuilder.BuildDebitAccount();
        clientAccount.Accounts.Add(newAccount);
        _lastAccountId++;

        return newAccount;
    }

    public IBankAccount RegisterDepositAccount(BankClient client, int expirationLength, decimal startValue)
    {
        DateTime expirationDate = new DateTime(_timeFlow.Now.Year + expirationLength, _timeFlow.Now.Month, _timeFlow.Now.Day);
        IClientAccount clientAccount = _clientAccounts.First(needleAccount => needleAccount.PersonInfo.Equals(client));
        double interestRate = InterestPolicy.DepositInterest.First(interestRange =>
        {
            if (interestRange.EndValue is not null)
                return startValue >= interestRange.StartValue && startValue <= interestRange.EndValue;
            else
                return startValue >= interestRange.StartValue;
        }).InterestRate;

        BankAccountBuilder accountBuilder = new BankAccountBuilder()
            .SetId(_lastAccountId)
            .SetExpirationDate(expirationDate)
            .SetInterestRate(interestRate);

        if (!clientAccount.IsTrusted)
            accountBuilder.SetWithdrawLimit(_defaultWithdrawLimit);

        DepositAccount newAccount = accountBuilder.BuildDepositAccount();
        clientAccount.Accounts.Add(newAccount);
        _lastAccountId++;

        return newAccount;
    }

    public IBankAccount RegisterCreditAccount(BankClient client, int expirationLength, decimal clientIncome)
    {
        DateTime expirationDate = new DateTime(_timeFlow.Now.Year + expirationLength, _timeFlow.Now.Month, _timeFlow.Now.Day);
        IClientAccount clientAccount = _clientAccounts.First(needleAccount => needleAccount.PersonInfo.Equals(client));
        double interestRate = InterestPolicy.CreditInterest;
        decimal creditLimit = clientIncome * Convert.ToDecimal(_defaultCreditCoefficient);

        BankAccountBuilder accountBuilder = new BankAccountBuilder()
            .SetId(_lastAccountId)
            .SetExpirationDate(expirationDate)
            .SetCreditRate(interestRate)
            .SetCreditLimit(creditLimit);

        if (!clientAccount.IsTrusted)
            accountBuilder.SetWithdrawLimit(_defaultWithdrawLimit);

        DepositAccount newAccount = accountBuilder.BuildDepositAccount();
        clientAccount.Accounts.Add(newAccount);
        _lastAccountId++;

        return newAccount;
    }

    public decimal CheckTotalBalance(BankClient client)
    {
        IClientAccount clientAccount = _clientAccounts.First(needleAccount => needleAccount.PersonInfo.Equals(client));
        return clientAccount.Accounts.Sum(account => account.AccountValue);
    }

    public Transaction AddMoney(BankClient client, IBankAccount account, decimal amount)
    {
        if (amount < 0)
            throw BankException.WrongMoneyAmount();
        IClientAccount clientAccount = _clientAccounts.First(needleAccount => needleAccount.PersonInfo.Equals(client));
        IBankAccount bankAccount = clientAccount.Accounts.First(needleAccount => needleAccount.Equals(account));
        bankAccount.AddMoney(amount);
        Transaction transaction = new Transaction(
            client.ClientId,
            client.ClientId,
            amount,
            bankAccount.AccountId,
            bankAccount.AccountId,
            _lastTransactionId);
        _transactions.Add(transaction);
        _lastTransactionId++;
        return transaction;
    }

    public Transaction RemoveMoney(BankClient client, IBankAccount account, decimal amount)
    {
        if (amount < 0)
            throw BankException.WrongMoneyAmount();
        IClientAccount clientAccount = _clientAccounts.First(needleAccount => needleAccount.PersonInfo.Equals(client));
        IBankAccount bankAccount = clientAccount.Accounts.First(needleAccount => needleAccount.Equals(account));

        if (!clientAccount.IsTrusted && amount > bankAccount.WithdrawLimit)
            throw BankException.WithdrawLimitExceed();

        bankAccount.RemoveMoney(amount);
        Transaction transaction = new Transaction(
            client.ClientId,
            client.ClientId,
            0 - amount,
            bankAccount.AccountId,
            bankAccount.AccountId,
            _lastTransactionId);
        _transactions.Add(transaction);
        _lastTransactionId++;
        return transaction;
    }

    public Transaction TransferMoney(BankClient sourceClient, IBankAccount sourceAccount, BankClient destClient, IBankAccount destAccount, decimal amount)
    {
        if (amount < 0)
            throw BankException.WrongMoneyAmount();
        IClientAccount sourceClientAccount = _clientAccounts.First(needleAccount => needleAccount.PersonInfo.Equals(sourceClient));
        IClientAccount destClientAccount = _clientAccounts.First(needleAccount => needleAccount.PersonInfo.Equals(destClient));
        IBankAccount sourceBankAccount = sourceClientAccount.Accounts.First(needleAccount => needleAccount.Equals(sourceAccount));
        IBankAccount destBankAccount = sourceClientAccount.Accounts.First(needleAccount => needleAccount.Equals(destAccount));

        if (!sourceClientAccount.IsTrusted && amount > sourceBankAccount.WithdrawLimit)
            throw BankException.WithdrawLimitExceed();

        sourceBankAccount.RemoveMoney(amount);
        Transaction transaction;
        try
        {
            destBankAccount.AddMoney(amount);
            transaction = new Transaction(
                sourceClient.ClientId,
                destClient.ClientId,
                amount,
                sourceBankAccount.AccountId,
                destBankAccount.AccountId,
                _lastTransactionId);
        }
        catch (Exception)
        {
            sourceAccount.AddMoney(amount);
            transaction = new Transaction(
                sourceClient.ClientId,
                sourceClient.ClientId,
                0,
                sourceBankAccount.AccountId,
                sourceBankAccount.AccountId,
                _lastTransactionId);
        }

        _transactions.Add(transaction);
        _lastTransactionId++;
        return transaction;
    }

    public void SubscribeForDebitRateChanges(BankClient client)
    {
        if (_clientAccounts.Find(clientAccount => clientAccount.PersonInfo.Equals(client)) is null)
            throw BankException.WrongClient();
        _messageBroker.AddDebitSuscriber(client);
    }

    public void SubscribeForCreditRateChanges(BankClient client)
    {
        if (_clientAccounts.Find(clientAccount => clientAccount.PersonInfo.Equals(client)) is null)
            throw BankException.WrongClient();
        _messageBroker.AddCreditSuscriber(client);
    }

    public void SubscribeForDepositRateChanges(BankClient client)
    {
        if (_clientAccounts.Find(clientAccount => clientAccount.PersonInfo.Equals(client)) is null)
            throw BankException.WrongClient();
        _messageBroker.AddDepositSuscriber(client);
    }

    public void UnsubscribeFromDebitRateChanges(BankClient client)
    {
        if (_clientAccounts.Find(clientAccount => clientAccount.PersonInfo.Equals(client)) is null)
            throw BankException.WrongClient();
        _messageBroker.RemoveDebitSuscriber(client);
    }

    public void UnsubscribeFromCreditRateChanges(BankClient client)
    {
        if (_clientAccounts.Find(clientAccount => clientAccount.PersonInfo.Equals(client)) is null)
            throw BankException.WrongClient();
        _messageBroker.RemoveCreditSuscriber(client);
    }

    public void UnsubscribeFromDepositRateChanges(BankClient client)
    {
        if (_clientAccounts.Find(clientAccount => clientAccount.PersonInfo.Equals(client)) is null)
            throw BankException.WrongClient();
        _messageBroker.RemoveDepositSuscriber(client);
    }

    public void UndoTransaction(BankClient client, Transaction transaction)
    {
        if (!transaction.SourceClientId.Equals(client.ClientId))
            throw BankException.NotPermittedOperation();
        if (transaction.DestinationClientId is null || transaction.DestinationAccountId is null)
            throw BankException.NotPermittedOperation();
        if (_transactions.Find(needleTransaction => needleTransaction.Equals(transaction)) is null)
            throw BankException.BadTransactionId();
        IClientAccount sourceAccount = _clientAccounts.First(needleAccount => needleAccount.PersonInfo.Equals(client));
        IClientAccount destAccount = _clientAccounts.First(needleAccount =>
            needleAccount.PersonInfo.ClientId.Equals(transaction.DestinationClientId));

        sourceAccount.PersonInfo.AcceptNotification($"Transaction {transaction.TransactionId} has been cancelled. {transaction.TransactionValue} returned to FrozenMoney.");
        destAccount.PersonInfo.AcceptNotification($"Transaction {transaction.TransactionId} has been cancelled. {transaction.TransactionValue} have been taken from FrozenMoney.");
        sourceAccount.Accounts.First(bankAccount =>
            bankAccount.AccountId.Equals(transaction.SourceAccountId)).FrozenMoney += transaction.TransactionValue;
        destAccount.Accounts.First(bankAccount =>
            bankAccount.AccountId.Equals(transaction.DestinationAccountId)).FrozenMoney -= transaction.TransactionValue;
        _transactions.Remove(transaction);
    }

    public void AcceptTimeNotification(TimeSpan difference)
    {
        int daysPerYear = 265;
        int percetageMultiplier = 100;
        _clientAccounts.ForEach(clientAccount =>
        {
            clientAccount.Accounts.ForEach(bankAccount =>
            {
                int daysDifference = difference.Days;
                if (bankAccount is DebitAccount | bankAccount is DepositAccount)
                {
                    double dayInterest = bankAccount.InterestRate / (daysPerYear * percetageMultiplier);
                    for (int daysPassed = 0; daysPassed < daysDifference; daysPassed++)
                        bankAccount.FrozenMoney += Convert.ToDecimal(dayInterest * Convert.ToDouble(bankAccount.AccountValue + bankAccount.FrozenMoney));
                }

                if (bankAccount is CreditAccount)
                {
                    double dayInterest = bankAccount.CreditRate / (daysPerYear * percetageMultiplier);
                    for (int daysPassed = 0; daysPassed < daysDifference; daysPassed++)
                        bankAccount.FrozenMoney -= Convert.ToDecimal(dayInterest * Convert.ToDouble(bankAccount.AccountValue + bankAccount.FrozenMoney));
                }

                if (bankAccount.ValidThru < _timeFlow.Now)
                    bankAccount.IsActive = false;

                if ((_timeFlow.Now - bankAccount.LastPayoff).Days >= 30 && bankAccount.IsActive)
                {
                    try
                    {
                        bankAccount.AddMoney(bankAccount.FrozenMoney);
                        bankAccount.LastPayoff = _timeFlow.Now;
                        bankAccount.FrozenMoney = 0;
                    }
                    catch (Exception)
                    {
                        clientAccount.PersonInfo.AcceptNotification("Unhandled exception occured while trying to add frozen money. Check 'FrozenMoney' property. Probably, not enough money to pay the credit interest rate.");
                    }
                }
            });
        });
    }

    public void AcceptNewBaseRate(double newBaseRate)
    {
        InterestPolicy.CreditInterest += newBaseRate - _baseRate;
        InterestPolicy.DebitInterest += newBaseRate - _baseRate;
        InterestPolicy.DepositInterest.ForEach(rateRange => rateRange.InterestRate += newBaseRate - _baseRate);
        _messageBroker.NotifyAllSuscribers($"Interest rate has changed: from {_baseRate} to {newBaseRate}. It" +
                                           $"leads to change:\n" +
                                           $"- Credit interest: {newBaseRate - _baseRate}\n" +
                                           $"- Debit interest: {newBaseRate - _baseRate}\n" +
                                           $"- All of the deposit interests: {newBaseRate - _baseRate}");
        _clientAccounts.ForEach(clientAccount =>
        {
            clientAccount.Accounts.ForEach(bankAccount =>
            {
                bankAccount.InterestRate += newBaseRate - _baseRate;
            });
        });
        _baseRate = newBaseRate;
    }

    public void AcceptNewDebitInterestRate(double newRate)
    {
        _messageBroker.NotifyDebitSuscribers($"Debit interest rate has changed: from {InterestPolicy.DebitInterest}" +
                                           $"to {newRate}.");
        InterestPolicy.DebitInterest += newRate - InterestPolicy.DebitInterest;
        _clientAccounts.ForEach(clientAccount =>
        {
            clientAccount.Accounts.ForEach(bankAccount =>
            {
                if (bankAccount is DebitAccount)
                    bankAccount.InterestRate += bankAccount.InterestRate - newRate;
            });
        });
    }

    public void AcceptNewCreditInterestRate(double newRate)
    {
        _messageBroker.NotifyDebitSuscribers($"Credit interest rate has changed: from {InterestPolicy.CreditInterest}" +
                                             $"to {newRate}.");
        InterestPolicy.CreditInterest += newRate - InterestPolicy.CreditInterest;
        _clientAccounts.ForEach(clientAccount =>
        {
            clientAccount.Accounts.ForEach(bankAccount =>
            {
                if (bankAccount is CreditAccount)
                    bankAccount.InterestRate += bankAccount.InterestRate - newRate;
            });
        });
    }

    public void AcceptNewDepositInterestRates(List<DepositInterestRange> newRates)
    {
        _messageBroker.NotifyDebitSuscribers($"Deposit interest rates has changed. It won't be changed for already" +
                                             $"opened deposit accounts.");
        InterestPolicy.DepositInterest.Clear();
        InterestPolicy.DepositInterest.AddRange(newRates);
    }

    public bool Equals(IBank? other)
    {
        if (ReferenceEquals(other, null)) return false;
        if (ReferenceEquals(other, this)) return true;
        return other.BankId.Equals(BankId);
    }
}