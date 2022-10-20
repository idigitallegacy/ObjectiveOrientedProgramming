using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Models;
using Isu.Extra.Wrappers;
using Isu.Models;
using Isu.Services;

namespace Isu.Extra.Extensions;

public class ExtendedGroupDto : Group, IExtendedGroupDto, IEquatable<Group>
{
    private ScheduleDto _scheduleDto = new ();
    private List<IExtendedStudentDto> _students = new ();

    public ExtendedGroupDto(GroupName groupName, int capacity)
        : base(groupName, capacity)
    {
        FacultyId = groupName.GetFacultyId();
        Capacity = capacity;
    }

    public ExtendedGroupDto(IExtendedGroupDto copiedGroupDto)
        : base(copiedGroupDto.GroupName, copiedGroupDto.Capacity)
    {
        _scheduleDto = new ScheduleDto(copiedGroupDto.ScheduleDto);
        _students = new List<IExtendedStudentDto>(copiedGroupDto.Students);
        FacultyId = new FacultyId(copiedGroupDto.FacultyId);
        Capacity = copiedGroupDto.Capacity;
    }

    public IScheduleDto ScheduleDto => _scheduleDto;
    public new IReadOnlyCollection<IExtendedStudentDto> Students => _students.AsReadOnly();
    public FacultyId FacultyId { get; }

    public int Capacity { get; }

    public void AddStudent(IExtendedStudentDto studentDto)
    {
        AddStudent(new Student(studentDto.Name, studentDto.Group, studentDto.Id));
        _students.Add(studentDto);
    }

    public void RemoveStudent(IExtendedStudentDto studentDto)
    {
        RemoveStudent(new Student(studentDto.Name, studentDto.Group, studentDto.Id));
        _students.Remove(studentDto);
    }

    public void AddLesson(ILessonDto lessonDto)
    {
        _scheduleDto.AddLesson(lessonDto.ToLesson());
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
        return Equals((ExtendedGroupDto)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), _scheduleDto, _students, FacultyId, Capacity);
    }

    protected bool Equals(ExtendedGroupDto other)
    {
        return base.Equals(other);
    }
}