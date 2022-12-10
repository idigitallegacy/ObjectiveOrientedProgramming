using Banks.Entities.BankConcept;
using Banks.Models.AddressConcept;
using Banks.Models.BankClientConcept;
using Banks.Models.PassportConcept;
using Banks.Models.TransactionConcept;

namespace Banks.Console;

public class BankInterface
{
    private BankInterface() { }

    public static void Execute(ICentralBank centralBank, IBank bank)
    {
        bool loopFlag = true;

        while (loopFlag)
        {
            System.Console.WriteLine("Choose action:");
            System.Console.WriteLine("\t 1) Register client");
            System.Console.WriteLine("\t 2) Add client data");
            System.Console.WriteLine("\t 3) Undo some transaction");
            System.Console.WriteLine("\t 0 << Back");

            switch (Convert.ToInt32(System.Console.ReadLine()))
            {
                case 1:
                {
                    System.Console.WriteLine("\t Enter client's name");
                    string name = System.Console.ReadLine() ?? string.Empty;
                    System.Console.WriteLine("\t Enter client's surname");
                    string surname = System.Console.ReadLine() ?? string.Empty;
                    Passport? passport = RegisterInterface.SetupClientPassport("\t");
                    Address? address = RegisterInterface.SetupClientAddress("\t");
                    try
                    {
                        BankClient bankClient = bank.RegisterClient(name, surname, passport, address);
                        System.Console.WriteLine($"\t Client with id {bankClient.ClientId} registred.");
                    }
                    catch (Exception e)
                    {
                        System.Console.WriteLine("\t Unable to register client:");
                        System.Console.WriteLine(e);
                    }

                    break;
                }

                case 2:
                {
                    System.Console.WriteLine("\t Enter client's id");
                    BankClient? client = bank.FindClientById(Convert.ToInt32(System.Console.ReadLine()));
                    if (client is null)
                    {
                        System.Console.WriteLine("\t Client not found");
                        break;
                    }

                    Passport? passport = RegisterInterface.SetupClientPassport("\t");
                    Address? address = RegisterInterface.SetupClientAddress("\t");
                    bank.AddClientData(client, passport, address);
                    break;
                }

                case 3:
                {
                    System.Console.WriteLine("\t Enter client's id");
                    BankClient? client = bank.FindClientById(Convert.ToInt32(System.Console.ReadLine()));
                    if (client is null)
                    {
                        System.Console.WriteLine("\t Client not found");
                        break;
                    }

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
                        System.Console.WriteLine("\t Transaction cancelled successfully.");
                    }
                    catch (Exception e)
                    {
                        System.Console.WriteLine("\t Unable to undo transaction:");
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