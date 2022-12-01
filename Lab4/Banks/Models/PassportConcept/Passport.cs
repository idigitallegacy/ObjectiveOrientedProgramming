using Banks.Exceptions;

namespace Banks.Models.PassportConcept;

public class Passport
{
    public Passport(int series, int number, string givenBy, string divisionCode)
    {
        if (string.IsNullOrWhiteSpace(givenBy))
            throw PassportException.WrongGivenBy();
        if (string.IsNullOrWhiteSpace(divisionCode))
            throw PassportException.WrongDivisionCode();

        Series = series;
        Number = number;
        GivenBy = givenBy;
        DivisionCode = divisionCode;
    }

    public int Series { get; }
    public int Number { get; }
    public string GivenBy { get; }
    public string DivisionCode { get; }
}