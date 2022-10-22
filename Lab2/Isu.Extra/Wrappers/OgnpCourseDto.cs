using Isu.Extra.Entities;
using Isu.Extra.Models;

namespace Isu.Extra.Wrappers;

public class OgnpCourseDto
{
    public OgnpCourseDto(OgnpCourse course)
    {
        Streams = course.Streams;
        Teachers = course.Teachers;
        FacultyId = course.FacultyId;
    }

    public IEnumerable<StudyStreamDto> Streams { get; }
    public IEnumerable<TeacherDto> Teachers { get; }
    public FacultyId FacultyId { get; }

    public OgnpCourse ToOgnpCourse()
    {
        return new OgnpCourse(this);
    }
}