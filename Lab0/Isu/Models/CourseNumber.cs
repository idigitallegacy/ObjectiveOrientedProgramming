using Isu.Exceptions;

namespace Isu.Models;

public class CourseNumber
{
    private const byte MinValue = 1;
    private const byte MaxValue = 4;
    private byte _value = 1;

    public CourseNumber(byte number)
    {
        Value = number;
    }

    public byte Value
    {
        get
        {
            return _value;
        }
        set
        {
            if (value < MinValue | value > MaxValue)
                throw new IsuException($"Invalid course number {value}, it must be between {MinValue} and {MaxValue}.");
            _value = value;
        }
    }

    public static CourseNumber operator ++(CourseNumber courseNumber)
    {
        if (courseNumber._value < MaxValue)
            ++courseNumber._value;
        else
            throw new IsuException($"Unable to increase course value: it's already {MaxValue}");
        return courseNumber;
    }

    public static bool operator !=(CourseNumber lhs, CourseNumber rhs)
    {
        return lhs.Value != rhs.Value;
    }

    public static bool operator ==(CourseNumber lhs, CourseNumber rhs)
    {
        return lhs.Value == rhs.Value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is CourseNumber) return (CourseNumber)obj == this;
        return false;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}