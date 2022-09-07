using Isu.Entities;
using Isu.Exceptions;
using Isu.Models;

namespace Isu.Services;

public class IsuService : IIsuService
{
    private readonly List<Group> _groups = new ();
    private int _lastStudentId;

    public Group AddGroup(GroupName name)
    {
        if (FindGroup(name) is not null)
            throw new IsuException("Unable to add group: it's already exists.");

        _groups.Add(new Group(name));
        return new Group(name);
    }

    public Student AddStudent(Group group, string name)
    {
        try
        {
            _groups[_groups.FindIndex(it => it == group)]
                .AddStudent(new Student(name, group, _lastStudentId));
            ++_lastStudentId;
            return new Student(name, group, _lastStudentId - 1);
        }
        catch (IsuException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Student GetStudent(int id)
    {
        Student? student = FindStudent(id);
        if (student is null)
            throw new IsuException($"Student with id {id} not found.");
        return new Student(student);
    }

    public Student? FindStudent(int id)
    {
        Student? needle = _groups
            .Find(group => group.FindStudent(id) is not null)?
                .GetStudent(id);
        return (needle is null)
            ? null
            : new Student(needle.Name, needle.GroupId, needle.Id);
    }

    public List<Student> FindStudents(GroupName groupName)
    {
        Group? group = FindGroup(groupName);
        if (group is null)
            throw new IsuException("Group not found");
        return group.Students.ToList();
    }

    public List<Student> FindStudents(CourseNumber courseNumber)
    {
        var students = new List<Student>();
        _groups.ForEach(group =>
        {
            if (group.Course == courseNumber)
                group.Students.ToList().ForEach(student => { students.Add(new Student(student)); });
        });
        return students;
    }

    public Group? FindGroup(GroupName groupName)
    {
        Group? needle = _groups.Find(group => group.Name.Name == groupName.Name);
        return (needle is null)
            ? null
            : new Group(needle);
    }

    public List<Group> FindGroups(CourseNumber courseNumber)
    {
        var groups = new List<Group>();
        _groups.ForEach(group =>
        {
            if (group.Course == courseNumber)
                groups.Add(new Group(group));
        });
        return groups;
    }

    public void ChangeStudentGroup(Student student, Group newGroup)
    {
        if (FindGroup(newGroup.Name) is null)
            throw new IsuException("Unable to find new group.");

        try
        {
            _groups[_groups.FindIndex(group => group == newGroup)]
                .AddStudent(new Student(student.Name, newGroup, student.Id));

            _groups[_groups.FindIndex(group => group == student.GroupId)]
                .RemoveStudent(student);
        }
        catch (IsuException e)
        {
            // Restoring changes if some made
            if (_groups.Find(group => group == newGroup)?
                    .FindStudent(student.Id) is not null)
            {
                _groups.Find(group => group == newGroup)?
                    .RemoveStudent(new Student(student.Name, newGroup, student.Id));
            }

            if (_groups.Find(group => group == student.GroupId)?
                    .FindStudent(student.Id) is null)
            {
                _groups.Find(group => group == student.GroupId)?
                    .AddStudent(student);
            }

            // Rethrowing
            Console.WriteLine(e);
            throw;
        }
    }
}