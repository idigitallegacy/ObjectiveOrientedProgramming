using Banks.Exceptions;

namespace Banks.Models.AddressConcept;

public class Address
{
    public Address(string country, string city, string street, string? buildingNumber = null, int? flat = null)
    {
        if (string.IsNullOrWhiteSpace(country))
            throw AddressException.WrongCountry();
        if (string.IsNullOrWhiteSpace(city))
            throw AddressException.WrongCity();
        if (string.IsNullOrWhiteSpace(street))
            throw AddressException.WrongStreet();
        Country = country;
        City = city;
        Street = street;
        BuildingNumber = buildingNumber;
        Flat = flat;
    }

    public string Country { get; }
    public string City { get; }
    public string Street { get; }
    public string? BuildingNumber { get; }
    public int? Flat { get; }
}