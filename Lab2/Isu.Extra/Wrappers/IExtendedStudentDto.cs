using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Models;

namespace Isu.Extra.Wrappers;

public interface IExtendedStudentDto
{
    public IReadOnlyCollection<IOgnpCourseDto?> OgnpCourses { get; }
    public FacultyId FacultyId { get; }
    public IExtendedGroupDto ExtendedGroupDto { get; }
    public string Name { get; }
    public Group Group { get; }
    public int Id { get; }
}