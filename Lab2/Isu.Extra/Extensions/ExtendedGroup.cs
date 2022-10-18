using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Models;
using Isu.Extra.Wrappers;
using Isu.Models;
using Isu.Services;

namespace Isu.Extra.Extensions;

public class ExtendedGroup : Group, IReadOnlyExtendedGroup, IEquatable<Group>
{
    private Schedule _schedule = new ();
    private List<IReadOnlyExtendedStudent> _students = new ();
    private FacultyId _facultyId;

    public ExtendedGroup(GroupName groupName, int capacity)
        : base(groupName, capacity)
    {
        _facultyId = groupName.GetFacultyId();
        Capacity = capacity;
    }

    public ExtendedGroup(IReadOnlyExtendedGroup copiedGroup)
        : base(copiedGroup.GroupName, copiedGroup.Capacity)
    {
        _schedule = new Schedule(copiedGroup.Schedule);
        _students = new List<IReadOnlyExtendedStudent>(copiedGroup.Students);
        _facultyId = new FacultyId(copiedGroup.FacultyId);
        Capacity = copiedGroup.Capacity;
    }

    public IReadOnlySchedule Schedule => _schedule;
    public new IReadOnlyCollection<IReadOnlyExtendedStudent> Students => _students;
    public FacultyId FacultyId => _facultyId;

    public int Capacity { get; }

    public void AddStudent(IReadOnlyExtendedStudent student)
    {
        AddStudent(new Student(student.Name, student.Group, student.Id));
        _students.Add(student);
    }

    public void RemoveStudent(IReadOnlyExtendedStudent student)
    {
        RemoveStudent(new Student(student.Name, student.Group, student.Id));
        _students.Remove(student);
    }

    public void AddLesson(IReadOnlyLesson lesson)
    {
        Lesson rwLesson = (Lesson)lesson;
        _schedule.AddLesson(rwLesson);
    }

    public bool Equals(Group? other)
    {
        return base.Equals(other);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ExtendedGroup)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), _schedule, _students, _facultyId, Capacity);
    }

    protected bool Equals(ExtendedGroup other)
    {
        return base.Equals(other) && _schedule.Equals(other._schedule) && _students.Equals(other._students) && _facultyId.Equals(other._facultyId) && Capacity == other.Capacity;
    }
}