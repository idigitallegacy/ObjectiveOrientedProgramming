using Isu.Extra.Models;

namespace Isu.Extra.Wrappers;

public interface IReadOnlyTeacher
{
    public string Name { get; }

    public FacultyId FacultyId { get; }

    public IReadOnlySchedule Schedule { get; }
}