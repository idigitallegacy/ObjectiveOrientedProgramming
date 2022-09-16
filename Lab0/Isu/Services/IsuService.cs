using Isu.Entities;
using Isu.Exceptions;
using Isu.Models;

namespace Isu.Services;

public class IsuService : IIsuService
{
    private readonly List<Group> _groups = new ();
    private int _lastStudentId = 0;

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
        var student = new Student(name, group, _lastStudentId);

        Group? needleGroup = _groups.Find(currentGroup => currentGroup == group);
        if (needleGroup is null)
            throw new IsuException("Unable to add student: group not found.");

        needleGroup.AddStudent(student);
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
        return _groups.SelectMany(group => group.Students).FirstOrDefault(student => student.Id == id);
    }

    public List<Student> FindStudents(GroupName groupName)
    {
        return FindGroup(groupName)?.Students.ToList() ??
               throw new IsuException("Group not found");
    }

    public List<Student> FindStudents(CourseNumber courseNumber)
    {
        return FindGroups(courseNumber).SelectMany(group => group.Students).ToList();
    }

    public Group? FindGroup(GroupName groupName)
    {
        return _groups.Find(group => group.GroupName.Name == groupName.Name);
    }

    public List<Group> FindGroups(CourseNumber courseNumber)
    {
        return _groups.Where(group => group.GroupName.Course == courseNumber).ToList();
    }

    public void ChangeStudentGroup(Student student, Group newGroup)
    {
        ValidateGroupChange(student, newGroup);
        student.ChangeGroup(newGroup);
    }

    private void ValidateGroupChange(Student student, Group newGroup)
    {
        if (FindGroup(newGroup.GroupName) is null)
            throw new IsuException("Unable to find new group.");

        Student? serviceStoredStudent = FindStudent(student.Id);

        if (serviceStoredStudent is null)
            throw new IsuException("Unable to change student's group: Student not found at IsuService.");

        if (serviceStoredStudent.Group != student.Group)
            throw new IsuException("Unable to change student's group: Already removed from old group.");

        if (serviceStoredStudent.Group == newGroup)
            throw new IsuException("Unable to change student's group: Already exists at the new group.");
    }
}