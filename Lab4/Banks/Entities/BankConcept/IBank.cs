using Banks.Models.AddressConcept;
using Banks.Models.BankAccountConcept;
using Banks.Models.BankClientConcept;
using Banks.Models.BankInterestPolicyConcept;
using Banks.Models.ClientAccountConcept;
using Banks.Models.PassportConcept;
using Banks.Models.TransactionConcept;

namespace Banks.Entities.BankConcept;

public interface IBank : IEquatable<IBank>
{
    Guid BankId { get; }
    BankInterestPolicy InterestPolicy { get; }

    BankClient RegisterClient(string name, string surname, Passport? passport = null, Address? address = null);
    BankClient AddClientData(BankClient client, Passport? newPassport = null, Address? newAddress = null);
    BankClient? FindClientById(int clientId);
    Transaction? FindTransactionById(int transactionId);
    IBankAccount RegisterDebitAccount(BankClient client, int expirationLength);
    IBankAccount RegisterDepositAccount(BankClient client, int expirationLength, decimal startValue);
    IBankAccount RegisterCreditAccount(BankClient client, int expirationLength, decimal clientIncome);
    IBankAccount? FindAccountById(BankClient client, int accountId);
    decimal CheckTotalBalance(BankClient client);
    Transaction AddMoney(BankClient client, IBankAccount account, decimal amount);
    Transaction RemoveMoney(BankClient client, IBankAccount account, decimal amount);
    Transaction TransferMoney(BankClient sourceClient, IBankAccount sourceAccount, BankClient destClient, IBankAccount destAccount, decimal amount);
    void SubscribeForDebitRateChanges(BankClient client);
    void SubscribeForCreditRateChanges(BankClient client);
    void SubscribeForDepositRateChanges(BankClient client);
    void UnsubscribeFromDebitRateChanges(BankClient client);
    void UnsubscribeFromCreditRateChanges(BankClient client);
    void UnsubscribeFromDepositRateChanges(BankClient client);
    void UndoTransaction(BankClient client, Transaction transaction);

    void AcceptTimeNotification(TimeSpan difference);
    void AcceptNewBaseRate(double newBaseRate);
    void AcceptNewDebitInterestRate(double newRate);
    void AcceptNewCreditInterestRate(double newRate);
    void AcceptNewDepositInterestRates(List<DepositInterestRange> newRates);
}