namespace Banks.Models.BankInterestPolicyConcept;

public class DepositInterestRange
{
    public decimal StartValue { get; set; }
    public decimal? EndValue { get; set; }
    public double InterestRate { get; set; }
}