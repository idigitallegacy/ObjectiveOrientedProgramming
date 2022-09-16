using Isu.Models;

namespace Isu.Entities;

public class Student
{
    public Student(string name, Group group, int id)
    {
        Name = name;
        Group = group;
        Id = id;
    }

    public string Name { get; }
    public Group Group { get; private set; }
    public int Id { get; }

    public static bool operator !=(Student student1, Student student2)
    {
        return student1.Id != student2.Id;
    }

    public static bool operator ==(Student student1, Student student2)
    {
        return student1.Id == student2.Id;
    }

    public void ChangeGroup(Group newGroup)
    {
        Group.RemoveStudent(this);
        newGroup.AddStudent(this);
        Group = newGroup;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Student) return (Student)obj == this;
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Group, Id);
    }
}