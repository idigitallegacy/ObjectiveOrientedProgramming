using Isu.Extra.Composites;
using Isu.Extra.Models;
using Isu.Extra.Wrappers;

namespace Isu.Extra.Entities;

public class Teacher : Scheduler, IReadOnlyTeacher
{
    private Schedule _schedule = new ();

    public Teacher(string name, FacultyId facultyId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception(); // TODO
        Name = name;
        FacultyId = facultyId;
    }

    public string Name { get; }
    public FacultyId FacultyId { get; }
    public IReadOnlySchedule Schedule => _schedule;

    public override void AddLesson(Lesson lesson)
    {
        _schedule.AddLesson(lesson);
    }

    public override void RemoveLesson(Lesson lesson)
    {
        _schedule.RemoveLesson(lesson);
    }

    public override Lesson? FindLesson(DayOfWeek dayOfWeek, Time startTime, Time endTime)
    {
        return _schedule.FindLesson(dayOfWeek, startTime, endTime);
    }
}