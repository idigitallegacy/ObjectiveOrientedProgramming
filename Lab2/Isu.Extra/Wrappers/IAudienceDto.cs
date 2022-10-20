namespace Isu.Extra.Wrappers;

public interface IAudienceDto
{
    public IScheduleDto ScheduleDto { get; }
    public int Number { get; }
}