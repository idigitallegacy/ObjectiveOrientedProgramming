using Isu.Exceptions;
using Isu.Models;

namespace Isu.Entities;

public class Group
{
    private const int MaxStudentsAmount = 30;
    private List<Student> _students = new ();

    public Group(GroupName groupName)
    {
        GroupName = groupName;
    }

    public GroupName GroupName { get; }
    public IReadOnlyCollection<Student> Students => _students;

    public static bool operator !=(Group group1, Group group2)
    {
        return group1.GroupName.Name != group2.GroupName.Name;
    }

    public static bool operator ==(Group group1, Group group2)
    {
        return group1.GroupName.Name == group2.GroupName.Name;
    }

    public void AddStudent(Student student)
    {
        if (_students.Count == MaxStudentsAmount)
            throw new IsuException("Unable to add student: group is full.");

        if (_students.Contains(student))
            throw new IsuException("Unable to add student: group already contains student.");

        _students.Add(student);
    }

    public void RemoveStudent(Student student)
    {
        if (!_students.Remove(student))
            throw new IsuException("Student not found.");
    }

    public override bool Equals(object? obj)
    {
        if (obj is Group) return (Group)obj == this;
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(GroupName, Students);
    }
}