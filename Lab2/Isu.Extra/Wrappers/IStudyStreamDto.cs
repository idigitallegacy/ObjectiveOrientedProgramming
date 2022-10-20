using Isu.Entities;
using Isu.Extra.Extensions;

namespace Isu.Extra.Wrappers;

public interface IStudyStreamDto
{
    public IScheduleDto ScheduleDto { get; }
    public ExtendedGroupDto GroupDto { get; }
}