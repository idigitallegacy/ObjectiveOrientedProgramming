using Banks.Models.PassportConcept;

namespace Banks.Exceptions;

public class PassportBuilderException : Exception
{
    private PassportBuilderException(string message = "")
        : base(message) { }

    public static PassportBuilderException WrongSeries(string message = "") => new PassportBuilderException(message);
    public static PassportBuilderException WrongNumber(string message = "") => new PassportBuilderException(message);
    public static PassportBuilderException WrongGivenBy(string message = "") => new PassportBuilderException(message);
    public static PassportBuilderException WrongDivisionCode(string message = "") => new PassportBuilderException(message);
}