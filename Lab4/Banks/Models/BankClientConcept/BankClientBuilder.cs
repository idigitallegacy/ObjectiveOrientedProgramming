using Banks.Exceptions;
using Banks.Models.AddressConcept;
using Banks.Models.PassportConcept;

namespace Banks.Models.BankClientConcept;

public class BankClientBuilder
{
    private int? _id;
    private string? _name;
    private string? _surname;
    private PassportBuilder _passportBuilder = new PassportBuilder();
    private AddressBuilder _addressBuilder = new AddressBuilder();

    public BankClientBuilder SetId(int id)
    {
        _id = id;
        return this;
    }

    public BankClientBuilder SetName(string name)
    {
        _name = name;
        return this;
    }

    public BankClientBuilder SetSurname(string surname)
    {
        _surname = surname;
        return this;
    }

    public BankClientBuilder SetPassportSeries(int series)
    {
        _passportBuilder.SetSeries(series);
        return this;
    }

    public BankClientBuilder SetPassportNumber(int number)
    {
        _passportBuilder.SetNumber(number);
        return this;
    }

    public BankClientBuilder SetPassportGivenBy(string givenBy)
    {
        _passportBuilder.SetGivenBy(givenBy);
        return this;
    }

    public BankClientBuilder SetPassportDivisionCode(string divisionCode)
    {
        _passportBuilder.SetDivisionCode(divisionCode);
        return this;
    }

    public BankClientBuilder SetAddressCountry(string country)
    {
        _addressBuilder.SetCountry(country);
        return this;
    }

    public BankClientBuilder SetAddressCity(string city)
    {
        _addressBuilder.SetCity(city);
        return this;
    }

    public BankClientBuilder SetAddressStreet(string street)
    {
        _addressBuilder.SetStreet(street);
        return this;
    }

    public BankClientBuilder SetAddressBuildingNumber(string? buildingNumber)
    {
        _addressBuilder.SetBuildingNumber(buildingNumber);
        return this;
    }

    public BankClientBuilder SetAddressFlat(int? flat)
    {
        _addressBuilder.SetFlat(flat);
        return this;
    }

    public BankClient Build()
    {
        return new BankClient(
            _name ?? throw BankClientBuilderException.WrongName(),
            _surname ?? throw BankClientBuilderException.WrongSurname(),
            _id ?? throw BankClientBuilderException.WrongId(),
            BuildPassport(),
            BuildAddress());
    }

    private Passport? BuildPassport()
    {
        try
        {
            return _passportBuilder.Build();
        }
        catch (PassportBuilderException)
        {
            return null;
        }
    }

    private Address? BuildAddress()
    {
        try
        {
            return _addressBuilder.Build();
        }
        catch (AddressBuilderException)
        {
            return null;
        }
    }
}