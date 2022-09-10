using Isu.Exceptions;

namespace Isu.Models;

public class GroupName
{
    private char[] _possibleFacultyLettersArray = new char[] { 'M' };

    public GroupName(string name)
    {
        if (name.Length != 5)
            throw new IsuException("Invalid group name: it must contain 5 characters.");

        if (!_possibleFacultyLettersArray.Contains(name[0]))
            throw new IsuException("Invalid group name: must start with M letter");

        NameAsString = name;
        FacultyId = Convert.ToString(name[0]) + name[1];
        Course = new CourseNumber(Convert.ToInt32(name[2] - '0'));
        GroupId = Convert.ToInt32(name[3].ToString() + name[4]);
    }

    public GroupName(GroupName groupName)
    {
        NameAsString = new string(groupName.NameAsString);
        FacultyId = new string(groupName.FacultyId);
        Course = new CourseNumber(groupName.Course);
        GroupId = groupName.GroupId;
    }

    public string NameAsString { get; }

    public string FacultyId { get; }

    public CourseNumber Course { get; }

    public int GroupId { get; }
}