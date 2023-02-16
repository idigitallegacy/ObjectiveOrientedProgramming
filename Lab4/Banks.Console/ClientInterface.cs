using System.Globalization;
using Banks.Entities.BankConcept;
using Banks.Models.BankAccountConcept;
using Banks.Models.BankClientConcept;
using Banks.Models.TransactionConcept;

namespace Banks.Console;

public class ClientInterface
{
    private ClientInterface() { }

    public static void Execute(IBank bank, BankClient client)
    {
        bool loopFlag = true;
        while (loopFlag)
        {
            System.Console.WriteLine("Choose action:");
            System.Console.WriteLine("\t 1) Register debit account");
            System.Console.WriteLine("\t 2) Register credit account");
            System.Console.WriteLine("\t 3) Register deposit account");
            System.Console.WriteLine("\t 4) Add money to account");
            System.Console.WriteLine("\t 5) Remove money from account");
            System.Console.WriteLine("\t 6) Transfer money to other account");
            System.Console.WriteLine("\t 7) Check total balance (related to all of the accounts)");
            System.Console.WriteLine("\t 8) Subscribe for debit rate changes");
            System.Console.WriteLine("\t 9) Subscribe for credit rate changes");
            System.Console.WriteLine("\t 10) Subscribe for deposit rate changes");
            System.Console.WriteLine("\t 11) Unsubscribe from debit rate changes");
            System.Console.WriteLine("\t 12) Unsubscribe from credit rate changes");
            System.Console.WriteLine("\t 13) Unsubscribe from deposit rate changes");
            System.Console.WriteLine("\t 14) Undo some transaction");
            System.Console.WriteLine("\t 0) << Back");

            switch (Convert.ToInt32(System.Console.ReadLine()))
            {
                case 1:
                {
                    System.Console.WriteLine("\t Enter preferred expiration length (years):");
                    IBankAccount bankAccount =
                        bank.RegisterDebitAccount(client, Convert.ToInt32(System.Console.ReadLine()));
                    System.Console.WriteLine($"\t Account with id {bankAccount.AccountId} created.");
                    break;
                }

                case 2:
                {
                    System.Console.WriteLine("\t Enter preferred expiration length (years):");
                    int expirationLength = Convert.ToInt32(System.Console.ReadLine());
                    System.Console.WriteLine("\t Enter your annual month income:");
                    decimal clientIncome = Convert.ToInt32(System.Console.ReadLine());
                    IBankAccount bankAccount = bank.RegisterCreditAccount(client, expirationLength, clientIncome);
                    System.Console.WriteLine($"\t Account with id {bankAccount.AccountId} created.");
                    break;
                }

                case 3:
                {
                    System.Console.WriteLine("\t Enter preferred expiration length (years):");
                    int expirationLength = Convert.ToInt32(System.Console.ReadLine());
                    System.Console.WriteLine("\t Enter start value:");
                    decimal startValue = Convert.ToInt32(System.Console.ReadLine());
                    IBankAccount bankAccount = bank.RegisterCreditAccount(client, expirationLength, startValue);
                    System.Console.WriteLine($"\t Account with id {bankAccount.AccountId} created.");
                    break;
                }

                case 4:
                {
                    System.Console.WriteLine("\t Enter account id:");
                    int accountId = Convert.ToInt32(System.Console.ReadLine());
                    IBankAccount? bankAccount = bank.FindAccountById(client, accountId);
                    if (bankAccount is null)
                    {
                        System.Console.WriteLine("\t Account not found");
                        break;
                    }

                    System.Console.WriteLine("\t Enter money amount:");
                    decimal amount = decimal.Parse(System.Console.ReadLine() ?? string.Empty, NumberStyles.Any, CultureInfo.InvariantCulture);
                    Transaction transaction = bank.AddMoney(client, bankAccount, amount);
                    System.Console.WriteLine($"\t Transation with {transaction.TransactionId} succeed.");
                    break;
                }

                case 5:
                {
                    System.Console.WriteLine("\t Enter account id:");
                    int accountId = Convert.ToInt32(System.Console.ReadLine());
                    IBankAccount? bankAccount = bank.FindAccountById(client, accountId);
                    if (bankAccount is null)
                    {
                        System.Console.WriteLine("\t Account not found");
                        break;
                    }

                    System.Console.WriteLine("\t Enter money amount:");
                    decimal amount = decimal.Parse(System.Console.ReadLine() ?? string.Empty, NumberStyles.Any, CultureInfo.InvariantCulture);
                    Transaction transaction = bank.RemoveMoney(client, bankAccount, amount);
                    System.Console.WriteLine($"\t Transaction with {transaction.TransactionId} succeed.");
                    break;
                }

                case 6:
                {
                    System.Console.WriteLine("\t Enter account id:");
                    int accountId = Convert.ToInt32(System.Console.ReadLine());
                    IBankAccount? bankAccount = bank.FindAccountById(client, accountId);
                    if (bankAccount is null)
                    {
                        System.Console.WriteLine("\t Account not found");
                        break;
                    }

                    System.Console.WriteLine("\t Enter destination client id:");
                    BankClient? destClient = bank.FindClientById(Convert.ToInt32(System.Console.ReadLine()));
                    if (destClient is null)
                    {
                        System.Console.WriteLine("\t Destination client not found");
                        break;
                    }

                    System.Console.WriteLine("\t Enter destination client account id:");
                    IBankAccount? destAccount =
                        bank.FindAccountById(destClient, Convert.ToInt32(System.Console.ReadLine()));
                    if (destAccount is null)
                    {
                        System.Console.WriteLine("\t Destination account not found");
                        break;
                    }

                    System.Console.WriteLine("\t Enter money amount:");
                    decimal amount = decimal.Parse(System.Console.ReadLine() ?? string.Empty, NumberStyles.Any, CultureInfo.InvariantCulture);

                    Transaction transaction = bank.TransferMoney(client, bankAccount, destClient, destAccount, amount);
                    System.Console.WriteLine($"\t Transaction with {transaction.TransactionId} succeed.");
                    break;
                }

                case 7:
                {
                    System.Console.WriteLine("\t" + bank.CheckTotalBalance(client));
                    break;
                }

                case 8:
                {
                    bank.SubscribeForDebitRateChanges(client);
                    break;
                }

                case 9:
                {
                    bank.SubscribeForCreditRateChanges(client);
                    break;
                }

                case 10:
                {
                    bank.SubscribeForDepositRateChanges(client);
                    break;
                }

                case 11:
                {
                    bank.UnsubscribeFromDebitRateChanges(client);
                    break;
                }

                case 12:
                {
                    bank.UnsubscribeFromCreditRateChanges(client);
                    break;
                }

                case 13:
                {
                    bank.UnsubscribeFromDepositRateChanges(client);
                    break;
                }

                case 14:
                {
                    System.Console.WriteLine("\t Enter transaction id");
                    Transaction? transaction = bank.FindTransactionById(Convert.ToInt32(System.Console.ReadLine()));
                    if (transaction is null)
                    {
                        System.Console.WriteLine("\t Transaction not found");
                        break;
                    }

                    try
                    {
                        bank.UndoTransaction(client, transaction);
                        System.Console.WriteLine("Transaction cancelled successfully");
                    }
                    catch (Exception e)
                    {
                        System.Console.WriteLine("Unable to undo transaction:");
                        System.Console.WriteLine(e);
                    }

                    break;
                }

                case 0:
                {
                    loopFlag = false;
                    break;
                }
            }
        }
    }
}