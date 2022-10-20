using Isu.Extra.Composites;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;
using Isu.Extra.Wrappers;

namespace Isu.Extra.Entities;

public class TeacherDto : IScheduler, ITeacherDto, IEquatable<TeacherDto>
{
    private ScheduleDto _scheduleDto = new ();

    public TeacherDto(string name, FacultyId facultyId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new TeacherException("Wrong teacher name");
        Name = name;
        FacultyId = facultyId;
    }

    public TeacherDto(ITeacherDto copiedTeacherDto)
    {
        Name = copiedTeacherDto.Name;
        FacultyId = new FacultyId(copiedTeacherDto.FacultyId);
    }

    public string Name { get; }
    public FacultyId FacultyId { get; }
    public IScheduleDto ScheduleDto => _scheduleDto;

    public void AddLesson(LessonDto lessonDto)
    {
        _scheduleDto.AddLesson(lessonDto);
    }

    public void RemoveLesson(LessonDto lessonDto)
    {
        _scheduleDto.RemoveLesson(lessonDto);
    }

    public LessonDto? FindLesson(DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime)
    {
        return _scheduleDto.FindLesson(dayOfWeek, startTime, endTime);
    }

    public bool Equals(TeacherDto? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _scheduleDto.Equals(other._scheduleDto) && Name == other.Name && FacultyId.Equals(other.FacultyId);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((TeacherDto)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_scheduleDto, Name, FacultyId);
    }
}