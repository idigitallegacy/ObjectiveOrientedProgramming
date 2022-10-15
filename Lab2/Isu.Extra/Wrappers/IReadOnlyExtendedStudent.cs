using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Models;

namespace Isu.Extra.Wrappers;

public interface IReadOnlyExtendedStudent
{
    public IReadOnlyCollection<IReadOnlyOgnpCourse?> OgnpCourses { get; }
    public FacultyId FacultyId { get; }
    public IReadOnlyExtendedGroup ExtendedGroup { get; }
    public string Name { get; }
    public Group Group { get; }
    public int Id { get; }
}