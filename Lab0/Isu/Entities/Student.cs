namespace Isu.Entities;

public class Student
{
    public Student(string name, Group group, int id)
    {
        Name = name;
        GroupId = group;
        Id = id;
    }

    public Student(Student student)
    {
        Name = new string(student.Name);
        GroupId = new Group(student.GroupId);
        Id = student.Id;
    }

    public string Name { get; }
    public Group GroupId { get; }
    public int Id { get; }

    public static bool operator !=(Student student1, Student student2)
    {
        return student1.Name != student2.Name &
               student1.GroupId != student2.GroupId &
               student1.Id != student2.Id;
    }

    public static bool operator ==(Student student1, Student student2)
    {
        return !(student1 == student2);
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