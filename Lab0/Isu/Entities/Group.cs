using Isu.Exceptions;
using Isu.Models;

namespace Isu.Entities;

public class Group
{
    private const byte MaxStudentsAmount = 30;
    private List<Student> _students = new List<Student>();
    private byte _studentsAmount;
    public Group(GroupName groupName)
    {
        Name = groupName;
        Course = groupName.Course;
    }

    public Group(Group rhs)
    {
        Name = rhs.Name;
        Course = rhs.Course;
        _students = rhs.Students.ToList();
        _studentsAmount = rhs._studentsAmount;
    }

    public GroupName Name { get; }
    public CourseNumber Course { get; }
    public IEnumerable<Student> Students
    {
        get
        {
            foreach (Student student in _students)
                yield return student;
        }
    }

    public static bool operator !=(Group lhs, Group rhs)
    {
        return lhs.Name != rhs.Name;
    }

    public static bool operator ==(Group lhs, Group rhs)
    {
        return lhs.Name == rhs.Name;
    }

    /*
     public List<Student> Students
     {
         get => _students;
         private set => _students = value;
    }
    */

    public void AddStudent(Student student)
    {
        if (_students.Count == MaxStudentsAmount)
            throw new IsuException("Unable to add student: group is full.");
        if (_students.Contains(student))
            throw new IsuException("Unable to add student: group already contains student.");
        _students.Add(student);
        ++_studentsAmount;
    }

    public void RemoveStudent(Student student)
    {
        if (!_students.Remove(student))
            throw new IsuException("Student not found.");
    }

    public Student GetStudent(int id)
    {
        Student? student = _students.Find(it => it.Id == id);
        if (student is null)
            throw new IsuException($"Unable to get student {id} at group {Name}.");
        return student;
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
        return (Name.GetHashCode() + Students.GetHashCode()).GetHashCode();
    }
}