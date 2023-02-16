using Isu.Extra.Exceptions;

namespace Isu.Extra.Models;

public class FacultyId : IEquatable<FacultyId>
{
    private List<char> _possibleFacultyIds = new List<char> { 'M', 'F', 'C', 'T' };
    public FacultyId(char facultyId)
    {
        if (!_possibleFacultyIds.Contains(facultyId))
            throw new FacultyIdException("Unable to construct faculty.");
        Value = _possibleFacultyIds.IndexOf(facultyId);
    }

    public FacultyId(FacultyId copiedFacultyId)
    {
        Value = copiedFacultyId.Value;
    }

    public int Value { get; }

    public void AddFacultyId(char facultyId)
    {
        _possibleFacultyIds.Add(facultyId);
    }

    public bool Equals(FacultyId? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((FacultyId)obj);
    }

    public override int GetHashCode()
    {
        return Value;
    }
}