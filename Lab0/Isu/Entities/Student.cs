namespace Isu.Entities;

public class Student
{
    public Student(string name, Group group, int id)
    {
        Name = name;
        GroupId = group;
        Id = id;
    }

    public Student(Student rhs)
    {
        Name = rhs.Name;
        GroupId = rhs.GroupId;
        Id = rhs.Id;
    }

    public string Name { get; }
    public Group GroupId { get; }
    public int Id { get; }

    public static bool operator !=(Student lhs, Student rhs)
    {
        return lhs.Name != rhs.Name & lhs.GroupId != rhs.GroupId & lhs.Id != rhs.Id;
    }

    public static bool operator ==(Student lhs, Student rhs)
    {
        return lhs.Name == rhs.Name & lhs.GroupId == rhs.GroupId & lhs.Id == rhs.Id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Student) return (Student)obj == this;
        return false;
    }

    public override int GetHashCode()
    {
        return (Name.GetHashCode() + GroupId.GetHashCode() + Id.GetHashCode()).GetHashCode();
    }
}