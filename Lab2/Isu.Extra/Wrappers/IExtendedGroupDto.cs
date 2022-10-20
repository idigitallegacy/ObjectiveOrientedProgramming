using Isu.Entities;
using Isu.Extra.Models;
using Isu.Models;
using Isu.Services;

namespace Isu.Extra.Wrappers;

public interface IExtendedGroupDto
{
    public IScheduleDto ScheduleDto { get; }
    public GroupName GroupName { get; }
    public IReadOnlyCollection<IExtendedStudentDto> Students { get; }
    public int Capacity { get; }
    public FacultyId FacultyId { get; }
}