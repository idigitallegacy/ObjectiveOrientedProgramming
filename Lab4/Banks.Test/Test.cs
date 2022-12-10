using Banks.Entities.BankConcept;
using Banks.Models.BankAccountConcept;
using Banks.Models.BankClientConcept;
using Banks.Models.BankConstructorOptionsConcept;
using Banks.Models.BankInterestPolicyConcept;
using Banks.Models.TimeFlowConcept;
using Xunit;

namespace Banks.Test;

public class Test
{
    [Fact]
    public void SimpleTest_ItsBetterToTestWithConsoleProject()
    {
        double baseRate = 3.0;
        CentralBank.GetInstance().UpdateBaseInterestRate(baseRate);
        ICentralBank centralBank = CentralBank.GetInstance();
        Assert.Equal(baseRate, centralBank.BaseRate);
    }

    [Fact]
    public void SimpleTest()
    {
        string clientName = "Michael";
        string address = "abcabc";
        double baseRate = 3.0;
        double debitInterestRate = 1.0;
        double creditInterestRate = 2.0;
        decimal defaultWithdrawLimit = 500;
        double defaultCreditCoefficient = 1;
        int expiryLength = 5;
        int moneyAmount = 50000;
        int yearsPassed = 1;
        DepositInterestRange range1 = new DepositInterestRange();
        range1.StartValue = 0;
        range1.EndValue = 50000;
        range1.InterestRate = 1;
        DepositInterestRange range2 = new DepositInterestRange();
        range2.StartValue = 50000;
        range2.EndValue = 100000;
        range2.InterestRate = 2;
        DepositInterestRange range3 = new DepositInterestRange();
        range2.StartValue = 100000;
        range2.InterestRate = 3;
        List<DepositInterestRange> depositInterestRates = new List<DepositInterestRange>
        {
            range1, range2, range3,
        };
        BankConstructorOptions options = new BankConstructorOptionsBuilder()
            .SetDebitInterestRate(debitInterestRate)
            .SetCreditInterestRate(creditInterestRate)
            .SetDefaultCreditCoefficient(defaultCreditCoefficient)
            .SetDefaultWithdrawLimit(defaultWithdrawLimit)
            .SetDepositInterestRates(depositInterestRates)
            .Build();

        CentralBank.GetInstance().UpdateBaseInterestRate(baseRate);
        ICentralBank centralBank = CentralBank.GetInstance();
        IBank bank = centralBank.CreateBank(options);
        BankClient bankClient = bank.RegisterClient(clientName, address);
        IBankAccount bankAccount = bank.RegisterDebitAccount(bankClient, expiryLength);
        bank.AddMoney(bankClient, bankAccount, moneyAmount);
        TimeFlow.Instance.SetTime(new DateTime(TimeFlow.Instance.Now.Year + yearsPassed, TimeFlow.Instance.Now.Month, TimeFlow.Instance.Now.Day));
        Assert.Equal(new DateTime(DateTime.Now.Year + yearsPassed, DateTime.Now.Month, DateTime.Now.Day), TimeFlow.Instance.Now);

        decimal newMoney = moneyAmount;
        for (int day = 0; day < 364; day++)
            newMoney += Convert.ToDecimal(newMoney * Convert.ToDecimal(bank.InterestPolicy.DebitInterest)) / Convert.ToDecimal(365 * 100);
        Assert.Equal(Math.Round(newMoney, 4), Math.Round(bank.CheckTotalBalance(bankClient), 4));
    }
}