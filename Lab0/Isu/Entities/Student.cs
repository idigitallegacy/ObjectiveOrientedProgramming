using Isu.Exceptions;
using Isu.Models;

namespace Isu.Entities;

public class Student
{
    public Student(string name, Group groupDto, int id)
    {
        Name = name;
        GroupDto = groupDto;
        Id = id;
    }

    public string Name { get; }
    public Group GroupDto { get; private set; }
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
        newGroup.AddStudent(this);
        GroupDto.RemoveStudent(this);
        GroupDto = newGroup;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Student) return (Student)obj == this;
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, GroupDto, Id);
    }
}