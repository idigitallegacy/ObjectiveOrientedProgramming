using Isu.Exceptions;

namespace Isu.Models;

public class GroupName
{
    private const string PossibleFacultyLettersSet = "M";

    private string _nameAsString;
    private string _facultyId;
    private CourseNumber _course;
    private int _groupId;

    public GroupName(string groupNameAsString)
    {
        _nameAsString = groupNameAsString;
        FacultyId = groupNameAsString[0].ToString() + groupNameAsString[1];
        _facultyId = FacultyId;
        try
        {
            Course = new CourseNumber(Convert.ToInt32(groupNameAsString[2] - '0'));
            _course = Course;
            GroupId = Convert.ToByte(groupNameAsString[3].ToString() + groupNameAsString[4]);
        }
        catch (IsuException ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public GroupName(GroupName groupName)
    {
        _nameAsString = new string(groupName.NameAsString);
        _facultyId = new string(groupName.FacultyId);
        _course = new CourseNumber(groupName.Course);
        _groupId = groupName.GroupId;
    }

    public string NameAsString
    {
        get => _nameAsString;
        private set
        {
            if (value.Length != 5)
                throw new IsuException("Invalid group name: it must contain 5 characters.");
            _nameAsString = value;
        }
    }

    public string FacultyId
    {
        get => _facultyId;
        private set
        {
            if (!PossibleFacultyLettersSet.Contains(value[0]))
                throw new IsuException("Invalid group name: must start with M letter");
        }
    }

    public CourseNumber Course
    {
        get => _course;
        private set => _course = value;
    }

    public int GroupId
    {
        get => _groupId;
        private set => _groupId = value;
    }
}