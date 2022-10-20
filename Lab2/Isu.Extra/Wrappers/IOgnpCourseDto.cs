using Isu.Extra.Models;

namespace Isu.Extra.Wrappers;

public interface IOgnpCourseDto
{
    public IReadOnlyCollection<IStudyStreamDto> Streams { get; }
    public IReadOnlyCollection<ITeacherDto> Teachers { get; }
    public FacultyId FacultyId { get; }

    public ITeacherDto? FindTeacher(string name);
}