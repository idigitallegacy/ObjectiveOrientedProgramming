using Isu.Exceptions;
using Isu.Models;

namespace Isu.Entities;

public class Group
{
    private const int MaxStudentsAmount = 30;
    private List<Student> _students = new ();
    private int _studentsAmount;

    public Group(GroupName groupName)
    {
        Name = groupName;
    }

    public Group(Group group)
    {
        Name = new GroupName(group.Name);
        _students = new List<Student>(group.Students.ToList());
        _studentsAmount = group._studentsAmount;
    }

    public GroupName Name { get; }
    public IReadOnlyCollection<Student> Students => _students;

    public static bool operator !=(Group group1, Group group2)
    {
        return group1.Name != group2.Name;
    }

    public static bool operator ==(Group group1, Group group2)
    {
        return group1.Name.NameAsString == group2.Name.NameAsString;
    }

    public void AddStudent(Student student)
    {
        if (_studentsAmount == MaxStudentsAmount)
            throw new IsuException("Unable to add student: group is full.");

        if (_students.Contains(student))
            throw new IsuException("Unable to add student: group already contains student.");

        _students.Add(student);
        _studentsAmount++;
    }

    public void RemoveStudent(Student student)
    {
        if (!_students.Remove(student))
            throw new IsuException("Student not found.");
        _studentsAmount++;
    }

    public Student GetStudent(int id)
    {
        return FindStudent(id) ??
               throw new IsuException($"Unable to get student {id} at group {Name}.");
    }

    public Student? FindStudent(int id)
    {
        return _students.Find(student => student.Id == id);
    }

    public override bool Equals(object? obj)
    {
        if (obj is Group) return (Group)obj == this;
        return false;
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode() + Students.GetHashCode();
    }
}