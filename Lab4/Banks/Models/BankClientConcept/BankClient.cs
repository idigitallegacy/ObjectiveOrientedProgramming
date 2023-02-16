using Banks.Exceptions;
using Banks.Models.AddressConcept;
using Banks.Models.PassportConcept;

namespace Banks.Models.BankClientConcept;

public class BankClient : IEquatable<BankClient>
{
    public BankClient(string name, string surname, int id, Passport? passport = null, Address? address = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw BankClientException.WrongName();
        if (string.IsNullOrWhiteSpace(surname))
            throw BankClientException.WrongSurname();

        Name = name;
        Surname = surname;
        Passport = passport;
        Address = address;
        ClientId = id;
    }

    public string Name { get; }
    public string Surname { get; }
    public Passport? Passport { get; internal set; }
    public Address? Address { get; internal set; }
    public int ClientId { get; }

    public void AcceptNotification(string message) { Console.WriteLine($"Client with ID: {ClientId} notified: {message}"); }

    public bool Equals(BankClient? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return ClientId == other.ClientId;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((BankClient)obj);
    }

    public override int GetHashCode()
    {
        return ClientId;
    }
}