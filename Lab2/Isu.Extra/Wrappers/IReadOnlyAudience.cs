namespace Isu.Extra.Wrappers;

public interface IReadOnlyAudience
{
    public IReadOnlySchedule Schedule { get; }
    public int Number { get; }
}