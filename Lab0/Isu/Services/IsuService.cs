using Isu.Entities;
using Isu.Exceptions;
using Isu.Models;

namespace Isu.Services;

/*
 * All the sample code that proves comments is shown at <Isu.Test>/Sample.cs
 */

/*
 * I use return new Group(group), return new Student(student), etc. constructions to prevent IsuService._groups
 * editing from outer scope. E.g.:
 * var service = new IsuService();
 * var group = service.AddGroup(new GroupName("M3206"));
 * group.AddStudent(new Student("Michael", group, 1)); <- leads to appear this student at IsuService._groups[0],
 *                                                        if I remove return new Group(group) construction.
 */

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
        catch (IsuException)
        {
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
        return needle;
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
            if (group.Name.Course == courseNumber)
            {
                group.Students.ToList().ForEach(student => { students.Add(student); });
            }
        });
        return students;
    }

    public Group? FindGroup(GroupName groupName)
    {
        Group? needle = _groups.Find(group => group.Name.NameAsString == groupName.NameAsString);
        return needle;
    }

    public List<Group> FindGroups(CourseNumber courseNumber)
    {
        var groups = new List<Group>();
        _groups.ForEach(group =>
        {
            if (group.Name.Course == courseNumber)
                groups.Add(group);
        });
        return groups;
    }

    public void ChangeStudentGroup(Student student, Group newGroup)
    {
        if (FindGroup(newGroup.Name) is null)
            throw new IsuException("Unable to find new group.");

        if (!ValidateGroupChange(student, newGroup))
            throw new IsuException("Unable to change student's group");

        try
        {
            _groups[_groups.FindIndex(group => group == newGroup)]
                .AddStudent(new Student(student.Name, newGroup, student.Id));

            _groups[_groups.FindIndex(group => group == student.GroupId)]
                .RemoveStudent(student);
        }
        catch (IsuException)
        {
            throw;
        }
    }

    private bool ValidateGroupChange(Student student, Group newGroup)
    {
        if (_groups[_groups.FindIndex(group => group == student.GroupId)]
                .FindStudent(student.Id) is null)
            return false;
        if (_groups[_groups.FindIndex(group => group == newGroup)]
                .FindStudent(student.Id) is not null)
            return false;
        return true;
    }
}