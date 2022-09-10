using Isu.Exceptions;

namespace Isu.Models;

public class CourseNumber
{
    private const int MinValue = 1;
    private const int MaxValue = 4;
    private int _value = 1;

    public CourseNumber(int number)
    {
        Value = number;
    }

    public int Value
    {
        get
        {
            return _value;
        }
        private set
        {
            if (value < MinValue | value > MaxValue)
                throw new IsuException($"Invalid course number {value}, it must be between {MinValue} and {MaxValue}.");
            _value = value;
        }
    }

    public static bool operator !=(CourseNumber courseNumber1, CourseNumber courseNumber2)
    {
        return courseNumber1.Value != courseNumber2.Value;
    }

    public static bool operator ==(CourseNumber courseNumber1, CourseNumber courseNumber2)
    {
        return courseNumber1.Value == courseNumber2.Value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is CourseNumber) return (CourseNumber)obj == this;
        return false;
    }

    public override int GetHashCode()
    {
        return Value;
    }
}