using Isu.Models;

namespace Isu.Entities;

public class Student
{
    public Student(string name, GroupName groupId, int id)
    {
        Name = name;
        GroupId = groupId;
        Id = id;
    }

    public string Name { get; }
    public GroupName GroupId { get; }
    public int Id { get; }

    public static bool operator !=(Student student1, Student student2)
    {
        return student1.Id != student2.Id;
    }

    public static bool operator ==(Student student1, Student student2)
    {
        return !(student1 != student2);
    }

    public override bool Equals(object? obj)
    {
        if (obj is Student) return (Student)obj == this;
        return false;
    }

    public override int GetHashCode()
    {
        return (Name.GetHashCode() + GroupId.GetHashCode() + Id).GetHashCode();
    }
}