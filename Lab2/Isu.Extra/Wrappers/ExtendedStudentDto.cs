using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Extensions;
using Isu.Extra.Models;

namespace Isu.Extra.Wrappers;

public class ExtendedStudentDto
{
    public ExtendedStudentDto(ExtendedStudent student)
    {
        OgnpCourses = student.OgnpCourses;
        FacultyId = student.FacultyId;
        ExtendedGroup = student.ExtendedGroup;
        Name = student.Name;
        Group = student.Group;
        Id = student.Id;
    }

    public IReadOnlyCollection<OgnpCourseDto?> OgnpCourses { get; }
    public FacultyId FacultyId { get; }
    public ExtendedGroupDto ExtendedGroup { get; }
    public string Name { get; }
    public Group Group { get; }
    public int Id { get; }
}