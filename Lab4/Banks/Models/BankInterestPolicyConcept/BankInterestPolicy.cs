namespace Banks.Models.BankInterestPolicyConcept;

public class BankInterestPolicy
{
    public double DebitInterest { get; internal set; }
    public double CreditInterest { get; internal set; }
    public List<DepositInterestRange> DepositInterest { get; } = new ();
}