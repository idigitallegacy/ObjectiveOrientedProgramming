using Banks.Exceptions;

namespace Banks.Models.PassportConcept;

public class PassportBuilder
{
    private int? _series;
    private int? _number;
    private string? _givenBy;
    private string? _divisionCode;

    public PassportBuilder SetSeries(int series)
    {
        _series = series;
        return this;
    }

    public PassportBuilder SetNumber(int number)
    {
        _number = number;
        return this;
    }

    public PassportBuilder SetGivenBy(string givenBy)
    {
        _givenBy = givenBy;
        return this;
    }

    public PassportBuilder SetDivisionCode(string divisionCode)
    {
        _divisionCode = divisionCode;
        return this;
    }

    public Passport Build()
    {
        return new Passport(
            _series ?? throw PassportBuilderException.WrongSeries(),
            _number ?? throw PassportBuilderException.WrongNumber(),
            _givenBy ?? throw PassportBuilderException.WrongGivenBy(),
            _divisionCode ?? throw PassportBuilderException.WrongDivisionCode());
    }
}