using Isu.Exceptions;

namespace Isu.Models;

public class GroupName
{
    private static readonly char[] PossibleFacultyLettersArray = { 'M' };

    public GroupName(string name)
    {
        if (name.Length != 5)
            throw new IsuException("Invalid group name: it must contain 5 characters.");

        if (!PossibleFacultyLettersArray.Contains(name[0]))
            throw new IsuException("Invalid group name: must start with M letter");

        NameAsString = name;
        FacultyId = Convert.ToString(name[0]) + name[1];
        Course = new CourseNumber(Convert.ToInt32(name[2] - '0'));
        GroupId = Convert.ToInt32(name[3].ToString() + name[4]);
    }

    public string NameAsString { get; }

    public string FacultyId { get; }

    public CourseNumber Course { get; }

    public int GroupId { get; }
}