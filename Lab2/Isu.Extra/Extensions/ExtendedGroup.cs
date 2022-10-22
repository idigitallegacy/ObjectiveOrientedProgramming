using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Models;
using Isu.Extra.Wrappers;
using Isu.Models;
using Isu.Services;

namespace Isu.Extra.Extensions;

public class ExtendedGroup : Group, IEquatable<Group>
{
    private Schedule _schedule = new ();
    private List<ExtendedStudent> _students = new ();

    public ExtendedGroup(GroupName groupName, int capacity)
        : base(groupName, capacity)
    {
        FacultyId = groupName.GetFacultyId();
        Capacity = capacity;
    }

    public ExtendedGroup(ExtendedGroupDto copiedGroupDto)
        : base(copiedGroupDto.GroupName, copiedGroupDto.Capacity)
    {
        _schedule = new Schedule(copiedGroupDto.ScheduleDto);
        _students = new List<ExtendedStudent>(copiedGroupDto.Students.Select(student => student.ToExtendedStudent()));
        FacultyId = new FacultyId(copiedGroupDto.FacultyId);
        Capacity = copiedGroupDto.Capacity;
    }

    public ScheduleDto ScheduleDto => new ScheduleDto(_schedule);
    public new IReadOnlyCollection<ExtendedStudent> Students => _students.AsReadOnly();
    public FacultyId FacultyId { get; }

    public int Capacity { get; }

    public void AddStudent(ExtendedStudentDto studentDto)
    {
        AddStudent(new Student(studentDto.Name, studentDto.Group, studentDto.Id));
        _students.Add(studentDto.ToExtendedStudent());
    }

    public void RemoveStudent(ExtendedStudent student)
    {
        RemoveStudent(new Student(student.Name, student.Group, student.Id));
        _students.Remove(student);
    }

    public void AddLesson(LessonDto lessonDto)
    {
        _schedule.AddLesson(lessonDto.ToLesson());
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
        return HashCode.Combine(base.GetHashCode(), _schedule, _students, FacultyId, Capacity);
    }

    protected bool Equals(ExtendedGroup other)
    {
        return base.Equals(other);
    }
}