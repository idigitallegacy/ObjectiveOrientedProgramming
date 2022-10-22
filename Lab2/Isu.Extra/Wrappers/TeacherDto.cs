using Isu.Extra.Entities;
using Isu.Extra.Models;

namespace Isu.Extra.Wrappers;

public class TeacherDto
{
    public TeacherDto(Teacher teacher)
    {
        Name = teacher.Name;
        FacultyId = teacher.FacultyId;
        Schedule = teacher.Schedule;
    }

    public string Name { get; }

    public FacultyId FacultyId { get; }

    public ScheduleDto Schedule { get; }

    public Teacher ToTeacher()
    {
        return new Teacher(this);
    }
}