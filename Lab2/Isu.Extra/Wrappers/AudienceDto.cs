using Isu.Extra.Models;

namespace Isu.Extra.Wrappers;

public class AudienceDto
{
    public AudienceDto(Audience audience)
    {
        ScheduleDto = audience.Schedule;
        Number = audience.Number;
    }

    public ScheduleDto ScheduleDto { get; }
    public int Number { get; }
}