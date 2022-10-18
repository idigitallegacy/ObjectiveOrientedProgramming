using Isu.Extra.Composites;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;
using Isu.Extra.Wrappers;

namespace Isu.Extra.Entities;

public class Teacher : IScheduler, IReadOnlyTeacher
{
    private Schedule _schedule = new ();

    public Teacher(string name, FacultyId facultyId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new TeacherException("Wrong teacher name");
        Name = name;
        FacultyId = facultyId;
    }

    public string Name { get; }
    public FacultyId FacultyId { get; }
    public IReadOnlySchedule Schedule => _schedule;

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
}