using Isu.Exceptions;

namespace Isu.Models;

public class GroupName
{
    private const string PossibleFacultyLettersSet = "M";

    private string _name;
    private string _facultyId;
    private CourseNumber _course;
    private byte _groupId;

    public GroupName(string groupName)
    {
        Name = groupName;
        _name = groupName;
        FacultyId = groupName[0].ToString() + groupName[1];
        _facultyId = FacultyId;
        try
        {
            Course = new CourseNumber(Convert.ToByte(groupName[2] - '0'));
            _course = Course;
            GroupId = Convert.ToByte(groupName[3].ToString() + groupName[4]);
        }
        catch (IsuException ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public string Name
    {
        get => _name;
        private set
        {
            if (value.Length != 5)
                throw new IsuException("Invalid group name: it must contain 5 characters.");
            _name = value;
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

    public byte GroupId
    {
        get => _groupId;
        private set => _groupId = value;
    }
}