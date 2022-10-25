using Isu.Extra.Models;

namespace Isu.Extra.Wrappers;

public class AudienceDto
{
    public AudienceDto(Audience audience)
    {
        Schedule = audience.Schedule;
        Number = audience.Number;
    }

    public ScheduleDto Schedule { get; }
    public int Number { get; }

    public Audience ToAudience()
    {
        return new Audience(this);
    }
}