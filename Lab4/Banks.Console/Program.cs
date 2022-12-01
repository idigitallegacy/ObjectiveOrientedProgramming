using Banks.Console;
using Banks.Entities.BankConcept;
using Banks.Models.BankInterestPolicyConcept;

ICentralBank centralBank = RegisterInterface.SetupCentralBank();
CoreInterface.Execute(centralBank);