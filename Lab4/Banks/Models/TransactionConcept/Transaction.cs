namespace Banks.Models.TransactionConcept;

public class Transaction : IEquatable<Transaction>
{
    public Transaction(
        int sourceClientId,
        int? destinationClientId,
        decimal transactionValue,
        int sourceAccountId,
        int? destinationAccountId,
        int transactionId)
    {
        SourceClientId = sourceClientId;
        DestinationClientId = destinationClientId;
        TransactionValue = transactionValue;
        SourceAccountId = sourceAccountId;
        DestinationAccountId = destinationAccountId;
        TransactionId = TransactionId;
    }

    public int SourceClientId { get; }
    public int? DestinationClientId { get; }
    public decimal TransactionValue { get; }
    public int SourceAccountId { get; }
    public int? DestinationAccountId { get; }
    public int TransactionId { get; }

    public bool Equals(Transaction? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return TransactionValue.Equals(other.TransactionValue) && TransactionId == other.TransactionId && SourceClientId == other.SourceClientId;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Transaction)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(TransactionValue, TransactionId);
    }
}