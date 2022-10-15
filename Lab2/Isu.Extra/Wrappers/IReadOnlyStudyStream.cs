using Isu.Entities;
using Isu.Extra.Extensions;

namespace Isu.Extra.Wrappers;

public interface IReadOnlyStudyStream
{
    public IReadOnlySchedule Schedule { get; }
    public ExtendedGroup Group { get; }
}