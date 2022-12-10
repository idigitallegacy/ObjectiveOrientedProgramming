using System.Globalization;
using Banks.Entities.BankConcept;
using Banks.Models.AddressConcept;
using Banks.Models.BankInterestPolicyConcept;
using Banks.Models.PassportConcept;

namespace Banks.Console;

public class RegisterInterface
{
    private RegisterInterface() { }

    public static ICentralBank SetupCentralBank()
    {
        double baseRate = 0;

        System.Console.WriteLine("Enter preferred base rate for central bank (double):");
        baseRate = double.Parse(System.Console.ReadLine() ?? string.Empty, NumberStyles.Any, CultureInfo.InvariantCulture);

        return new CentralBank(baseRate);
    }

    public static IBank SetupBank(ICentralBank centralBank, string prefix)
    {
        double debitInterestRate;
        double creditInterestRate;
        decimal defaultWithdrawLimit;
        double defaultCreditCoefficient;
        List<DepositInterestRange> interestRanges = new List<DepositInterestRange>();

        System.Console.WriteLine($"{prefix} Add bank's debit/credit policy.");
        System.Console.WriteLine($"{prefix} \tEnter debit interest rate (applied to debit accounts, relative to base rate {centralBank.BaseRate}: E.g. Base rate = {centralBank.BaseRate}; debit interest rate = 1.0. Resulting debit interest rate will be {centralBank.BaseRate - 1.0}.");
        debitInterestRate = double.Parse(System.Console.ReadLine() ?? string.Empty, NumberStyles.Any, CultureInfo.InvariantCulture);

        System.Console.WriteLine($"{prefix} \tEnter credit interest rate (applied to credit accounts, relative to base rate {centralBank.BaseRate}: E.g. Base rate = {centralBank.BaseRate}; credit interest rate = 1.0. Resulting debit interest rate will be {centralBank.BaseRate - 1.0}.");
        creditInterestRate = double.Parse(System.Console.ReadLine() ?? string.Empty, NumberStyles.Any, CultureInfo.InvariantCulture);

        System.Console.WriteLine($"{prefix} \tEnter default withdraw limit (applied to untrusted accounts without passport/address data):");
        defaultWithdrawLimit = decimal.Parse(System.Console.ReadLine() ?? string.Empty, NumberStyles.Any, CultureInfo.InvariantCulture);

        System.Console.WriteLine($"{prefix} \tEnter default credit coefficient (applied to credit accounts, at the end of the payoff month; used to calculate bank's interest based on client's annual income; recommended value = 1.0):");
        defaultCreditCoefficient = double.Parse(System.Console.ReadLine() ?? string.Empty, NumberStyles.Any, CultureInfo.InvariantCulture);

        System.Console.WriteLine($"{prefix} \tWould you like to add deposit interest ranges (y/n)?");
        interestRanges = BuildInterestRanges(centralBank, true, prefix);

        return centralBank.CreateBank(debitInterestRate, creditInterestRate, defaultWithdrawLimit, defaultCreditCoefficient, interestRanges);
    }

    public static List<DepositInterestRange> BuildInterestRanges(ICentralBank centralBank, bool awaitUserAnswer, string prefix)
    {
        List<DepositInterestRange> interestRanges = new List<DepositInterestRange>();
        string? userAnswer = "y";
        if (awaitUserAnswer)
            userAnswer = System.Console.ReadLine()?.ToLower();
        switch (userAnswer)
        {
            case "y":
            {
                System.Console.WriteLine($"{prefix} \t \tHow many ranges would you like to add?");
                int userAmount = Convert.ToInt32(System.Console.ReadLine());
                for (int i = 0; i < userAmount - 1; i++)
                {
                    interestRanges.Add(new DepositInterestRange());
                    System.Console.WriteLine($"{prefix} \t \t \tEnter range start:");
                    interestRanges[i].StartValue = decimal.Parse(System.Console.ReadLine() ?? string.Empty, NumberStyles.Any, CultureInfo.InvariantCulture);
                    System.Console.WriteLine($"{prefix} \t \t \tEnter range end:");
                    interestRanges[i].EndValue = decimal.Parse(System.Console.ReadLine() ?? string.Empty, NumberStyles.Any, CultureInfo.InvariantCulture);
                    System.Console.WriteLine($"{prefix} \t \t \tEnter range interest rate (relative to base rate):");
                    interestRanges[i].InterestRate = double.Parse(System.Console.ReadLine() ?? string.Empty, NumberStyles.Any, CultureInfo.InvariantCulture);
                }

                if (userAmount > 0)
                {
                    interestRanges.Add(new DepositInterestRange());
                    System.Console.WriteLine($"{prefix} \t \t \tEnter range start:");
                    interestRanges[userAmount - 1].StartValue = decimal.Parse(System.Console.ReadLine() ?? string.Empty, NumberStyles.Any, CultureInfo.InvariantCulture);
                    System.Console.WriteLine($"{prefix} \t \t \tEnter range interest rate (relative to base rate):");
                    interestRanges[userAmount - 1].InterestRate = double.Parse(System.Console.ReadLine() ?? string.Empty, NumberStyles.Any, CultureInfo.InvariantCulture);
                }

                return interestRanges;
            }

            case "n":
            {
                System.Console.WriteLine($"{prefix} \t \tApplying default interest ranges:");
                System.Console.WriteLine($"{prefix} \t \t \t0 - 50000 = 3%");
                System.Console.WriteLine($"{prefix} \t \t \t50000 - 100000 = 3.5%");
                System.Console.WriteLine($"{prefix} \t \t \t100000 - inf = 4%");

                interestRanges.Add(new DepositInterestRange());
                interestRanges.Add(new DepositInterestRange());
                interestRanges.Add(new DepositInterestRange());
                interestRanges[0].StartValue = 0;
                interestRanges[0].EndValue = 50000;
                interestRanges[0].InterestRate = centralBank.BaseRate - 3.0;
                interestRanges[1].StartValue = 50000;
                interestRanges[1].EndValue = 100000;
                interestRanges[1].InterestRate = centralBank.BaseRate - 3.5;
                interestRanges[2].StartValue = 100000;
                interestRanges[2].InterestRate = centralBank.BaseRate - 4.0;

                return interestRanges;
            }

            default: System.Console.WriteLine("\t \tUnhandled answer. Skipped.");
                goto case "n";
        }
    }

    public static Address? SetupClientAddress(string prefix)
    {
        System.Console.WriteLine($"{prefix} Would you like to add address data (y/n)?");
        switch (System.Console.ReadLine()?.ToLower())
        {
            case "y":
            {
                AddressBuilder addressBuilder = new AddressBuilder();
                System.Console.WriteLine($"{prefix} Enter country:");
                addressBuilder.SetCountry(System.Console.ReadLine() ?? string.Empty);
                System.Console.WriteLine($"{prefix} Enter city:");
                addressBuilder.SetCity(System.Console.ReadLine() ?? string.Empty);
                System.Console.WriteLine($"{prefix} Enter street:");
                addressBuilder.SetStreet(System.Console.ReadLine() ?? string.Empty);
                System.Console.WriteLine($"{prefix} Enter building number:");
                addressBuilder.SetBuildingNumber(System.Console.ReadLine() ?? string.Empty);
                System.Console.WriteLine($"{prefix} Enter flat number:");
                addressBuilder.SetStreet(System.Console.ReadLine() ?? string.Empty);
                try
                {
                    return addressBuilder.Build();
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("Building address failed with the following output:");
                    System.Console.WriteLine(e.Message);
                    return null;
                }
            }

            case "n": return null;

            default: System.Console.WriteLine("Undefined answer. Address building skipped."); return null;
        }
    }

    public static Passport? SetupClientPassport(string prefix)
    {
        System.Console.WriteLine($"{prefix} Would you like to add passport data (y/n)?");
        switch (System.Console.ReadLine()?.ToLower())
        {
            case "y":
            {
                PassportBuilder passportBuilder = new PassportBuilder();
                System.Console.WriteLine($"{prefix} Enter passport series:");
                passportBuilder.SetSeries(Convert.ToInt32(System.Console.ReadLine()));
                System.Console.WriteLine($"{prefix} Enter passport number:");
                passportBuilder.SetNumber(Convert.ToInt32(System.Console.ReadLine()));
                System.Console.WriteLine($"{prefix} Enter passport 'given by' string:");
                passportBuilder.SetGivenBy(System.Console.ReadLine() ?? string.Empty);
                System.Console.WriteLine($"{prefix} Enter passport division code:");
                passportBuilder.SetDivisionCode(System.Console.ReadLine() ?? string.Empty);
                try
                {
                    return passportBuilder.Build();
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("Building address failed with the following output:");
                    System.Console.WriteLine(e.Message);
                    return null;
                }
            }

            case "n": return null;

            default:
                System.Console.WriteLine("Undefined answer. Passport building skipped.");
                return null;
        }
    }

    public static DateTime SetupDate(string prefix)
    {
        System.Console.WriteLine($"{prefix} Enter new year (format: yyyy):");
        int newYear = Convert.ToInt32(System.Console.ReadLine());
        System.Console.WriteLine($"{prefix} Enter new month (format: mm):");
        int newMonth = Convert.ToInt32(System.Console.ReadLine());
        System.Console.WriteLine($"{prefix} Enter new day (format: dd):");
        int newDay = Convert.ToInt32(System.Console.ReadLine());
        return new DateTime(newYear, newMonth, newDay);
    }
}