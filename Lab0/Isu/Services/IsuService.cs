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

        var group = new Group(name);
        _groups.Add(group);
        return group;
    }

    public Student AddStudent(Group group, string name)
    {
        var student = new Student(name, group.Name, _lastStudentId);
        _groups[_groups.FindIndex(it => it == group)].AddStudent(student);
        _lastStudentId++;
        return student;
    }

    public Student GetStudent(int id)
    {
        return FindStudent(id) ??
               throw new IsuException($"Student with id {id} not found.");
    }

    public Student? FindStudent(int id)
    {
        return _groups.Find(group => group.FindStudent(id) is not null)?.GetStudent(id);
    }

    public List<Student> FindStudents(GroupName groupName)
    {
        return FindGroup(groupName)?.Students.ToList() ?? throw new IsuException("Group not found");
    }

    public List<Student> FindStudents(CourseNumber courseNumber)
    {
        var students = new List<Student>();
        _groups.ForEach(group =>
        {
            if (group.Name.Course == courseNumber)
                group.Students.ToList().ForEach(student => students.Add(student));
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
        ValidateGroupChange(student, newGroup);

        _groups[_groups.FindIndex(group => group == newGroup)]
            .AddStudent(new Student(student.Name, newGroup.Name, student.Id));

        _groups[_groups.FindIndex(group => group.Name.NameAsString == student.GroupId.NameAsString)]
            .RemoveStudent(student);
    }

    private void ValidateGroupChange(Student student, Group newGroup)
    {
        if (FindGroup(newGroup.Name) is null)
            throw new IsuException("Unable to find new group.");

        if (_groups[_groups.FindIndex(group => group.Name.NameAsString == student.GroupId.NameAsString)]
                .FindStudent(student.Id) is null)
            throw new IsuException("Unable to change student's group: Already removed from old group.");

        if (_groups[_groups.FindIndex(group => group == newGroup)]
                .FindStudent(student.Id) is not null)
            throw new IsuException("Unable to change student's group: Already exists at the new group.");
    }
}