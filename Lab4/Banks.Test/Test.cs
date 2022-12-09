using Banks.Entities.BankConcept;
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
}