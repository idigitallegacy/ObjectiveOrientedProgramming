using System.Globalization;
using Banks.Entities.BankConcept;

namespace Banks.Console;

public class CentralBankInterface
{
    private CentralBankInterface() { }

    public static void Execute(ICentralBank centralBank, List<IBank> banks)
    {
        bool loopFlag = true;
        while (loopFlag)
        {
            System.Console.WriteLine("Choose action:");
            System.Console.WriteLine("\t 1) Update base rate");
            System.Console.WriteLine("\t 2) Register bank");
            System.Console.WriteLine("\t 3) Update debit interest rate (at some bank)");
            System.Console.WriteLine("\t 4) Update credit interest rate (at some bank)");
            System.Console.WriteLine("\t 5) Update deposit interest rates (at some bank)");
            System.Console.WriteLine("\t 0) << Back");
            switch (Convert.ToInt32(System.Console.ReadLine()))
            {
                case 1:
                {
                    System.Console.WriteLine("\t Enter new base rate:");
                    centralBank.UpdateBaseInterestRate(double.Parse(System.Console.ReadLine() ?? string.Empty, NumberStyles.Any, CultureInfo.InvariantCulture));
                    break;
                }

                case 2:
                {
                    banks.Add(RegisterInterface.SetupBank(centralBank, "\t"));
                    System.Console.WriteLine($"\t Bank with id {banks.Last().BankId} created.");
                    break;
                }

                case 3:
                {
                    System.Console.WriteLine("\t Enter bank id:");
                    string? guid = System.Console.ReadLine();
                    IBank bank = banks.First(bank => bank.BankId.ToString().Equals(guid));
                    System.Console.WriteLine("\t Enter new debit interest rate (NOT related to base rate):");
                    double newRate = double.Parse(System.Console.ReadLine() ?? string.Empty, NumberStyles.Any, CultureInfo.InvariantCulture);
                    centralBank.UpdateDebitInterestRate(bank, newRate);
                    break;
                }

                case 4:
                {
                    System.Console.WriteLine("\t Enter bank id:");
                    string? guid = System.Console.ReadLine();
                    IBank bank = banks.First(bank => bank.BankId.ToString().Equals(guid));
                    System.Console.WriteLine("\t Enter new credit interest rate (NOT related to base rate):");
                    double newRate = double.Parse(System.Console.ReadLine() ?? string.Empty, NumberStyles.Any, CultureInfo.InvariantCulture);
                    centralBank.UpdateCreditInterestRate(bank, newRate);
                    break;
                }

                case 5:
                {
                    System.Console.WriteLine("\t Enter bank id:");
                    string? guid = System.Console.ReadLine();
                    IBank bank = banks.First(bank => bank.BankId.ToString().Equals(guid));
                    centralBank.UpdateDepositInterestRate(bank, RegisterInterface.BuildInterestRanges(centralBank,  false, "\t"));
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