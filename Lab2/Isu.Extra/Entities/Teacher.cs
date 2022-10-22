using Isu.Extra.Composites;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;
using Isu.Extra.Wrappers;

namespace Isu.Extra.Entities;

public class Teacher : IScheduler, IEquatable<Teacher>
{
    private Schedule _schedule = new ();

    public Teacher(string name, FacultyId facultyId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new TeacherException("Wrong teacher name");
        Name = name;
        FacultyId = facultyId;
    }

    public Teacher(TeacherDto copiedTeacherDto)
    {
        Name = copiedTeacherDto.Name;
        FacultyId = new FacultyId(copiedTeacherDto.FacultyId);
    }

    public string Name { get; }
    public FacultyId FacultyId { get; }
    public ScheduleDto Schedule => new ScheduleDto(_schedule);

    public void AddLesson(Lesson lesson)
    {
        _schedule.AddLesson(lesson);
    }

    public void RemoveLesson(Lesson lesson)
    {
        _schedule.RemoveLesson(lesson);
    }

    public Lesson? FindLesson(DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime)
    {
        return _schedule.FindLesson(dayOfWeek, startTime, endTime);
    }

    public bool Equals(Teacher? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name && FacultyId.Equals(other.FacultyId);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Teacher)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_schedule, Name, FacultyId);
    }
}