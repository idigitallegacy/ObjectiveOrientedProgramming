namespace Banks.Models.BankInterestPolicyConcept;

public class BankInterestPolicy
{
    public double DebitInterest { get; set; }
    public double CreditInterest { get; set; }
    public List<DepositInterestRange> DepositInterest { get; } = new ();
}