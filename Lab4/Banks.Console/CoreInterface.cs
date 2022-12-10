using Banks.Entities.BankConcept;
using Banks.Models.BankClientConcept;

namespace Banks.Console;

public class CoreInterface
{
    private CoreInterface() { }

    public static void Execute(ICentralBank centralBank)
    {
        List<IBank> banks = new List<IBank>();
        System.Console.WriteLine("Would you line to register your first bank?");
        switch (System.Console.ReadLine()?.ToLower())
        {
            case "y":
            {
                banks.Add(RegisterInterface.SetupBank(centralBank, "\t"));
                System.Console.WriteLine($"\t Bank with id {banks[0].BankId} created.");
                break;
            }

            case "n": break;
            default:
                System.Console.WriteLine("Unhandled answer. Bank registration skipped.");
                break;
        }

        CoreExecutor(centralBank, banks);
    }

    private static void CoreExecutor(ICentralBank centralBank, List<IBank> banks)
    {
        bool loopFlag = true;
        while (loopFlag)
        {
            System.Console.WriteLine("Choose role:");
            System.Console.WriteLine("\t 1) Central bank");
            System.Console.WriteLine("\t 2) Bank");
            System.Console.WriteLine("\t 3) Client (at some bank)");
            System.Console.WriteLine("\t 4) Timeflow");
            System.Console.WriteLine("\t 0) <<<< Exit");

            switch (Convert.ToInt32(System.Console.ReadLine()))
            {
                case 1: CentralBankInterface.Execute(centralBank, banks); break;
                case 2:
                {
                    System.Console.WriteLine("\t Enter bank id:");
                    string? guid = System.Console.ReadLine();
                    IBank bank = banks.First(bank => bank.BankId.ToString().Equals(guid));
                    BankInterface.Execute(centralBank, bank);
                    break;
                }

                case 3:
                {
                    System.Console.WriteLine("\t Enter bank id:");
                    string? guid = System.Console.ReadLine();
                    IBank bank = banks.First(bank => bank.BankId.ToString().Equals(guid));
                    System.Console.WriteLine("\t Enter client id:");
                    int clid = Convert.ToInt32(System.Console.ReadLine());
                    BankClient? client = bank.FindClientById(clid);
                    if (client is null)
                    {
                        System.Console.WriteLine("\t Client not found");
                        break;
                    }

                    ClientInterface.Execute(bank, client);
                    break;
                }

                case 4:
                {
                    TimeFlowInterface.Execute();
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