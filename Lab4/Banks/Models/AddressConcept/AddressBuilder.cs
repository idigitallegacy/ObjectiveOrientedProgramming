using Banks.Exceptions;

namespace Banks.Models.AddressConcept;

public class AddressBuilder
{
    private string? _country;
    private string? _city;
    private string? _street;
    private string? _buildingNumber;
    private int? _flat;

    public AddressBuilder SetCountry(string country)
    {
        _country = country;
        return this;
    }

    public AddressBuilder SetCity(string city)
    {
        _city = city;
        return this;
    }

    public AddressBuilder SetStreet(string street)
    {
        _street = street;
        return this;
    }

    public AddressBuilder SetBuildingNumber(string? buildingNumber)
    {
        _buildingNumber = buildingNumber;
        return this;
    }

    public AddressBuilder SetFlat(int? flat)
    {
        _flat = flat;
        return this;
    }

    public Address Build()
    {
        return new Address(
            _country ?? throw AddressBuilderException.WrongCountry(),
            _city ?? throw AddressBuilderException.WrongCity(),
            _street ?? throw AddressBuilderException.WrongStreet(),
            _buildingNumber,
            _flat);
    }
}