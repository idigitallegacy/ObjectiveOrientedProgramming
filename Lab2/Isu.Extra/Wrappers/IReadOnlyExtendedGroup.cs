using Isu.Entities;
using Isu.Models;
using Isu.Services;

namespace Isu.Extra.Wrappers;

public interface IReadOnlyExtendedGroup
{
    public IReadOnlySchedule Schedule { get; }
    public GroupName GroupName { get; }
    public IReadOnlyCollection<IReadOnlyExtendedStudent> Students { get; }
}