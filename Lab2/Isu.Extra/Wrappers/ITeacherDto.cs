using Isu.Extra.Models;

namespace Isu.Extra.Wrappers;

public interface ITeacherDto
{
    public string Name { get; }

    public FacultyId FacultyId { get; }

    public IScheduleDto ScheduleDto { get; }
}