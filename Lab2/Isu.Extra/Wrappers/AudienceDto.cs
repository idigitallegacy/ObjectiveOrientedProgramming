using Isu.Extra.Models;

namespace Isu.Extra.Wrappers;

public class AudienceDto
{
    public AudienceDto(Audience audience)
    {
        ScheduleDto = audience.ScheduleDto;
        Number = audience.Number;
    }

    public IScheduleDto ScheduleDto { get; }
    public int Number { get; }
}