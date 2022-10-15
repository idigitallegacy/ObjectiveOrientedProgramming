using Isu.Extra.Models;

namespace Isu.Extra.Wrappers;

public interface IReadOnlyOgnpCourse
{
    public IReadOnlyCollection<IReadOnlyStudyStream> Streams { get; }
    public IReadOnlyCollection<IReadOnlyTeacher> Teachers { get; }
    public FacultyId FacultyId { get; }

    public IReadOnlyTeacher? FindTeacher(string name);
}