using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;
using Isu.Models;

namespace Isu.Extra.Builders;

public class OgnpCourseBuilder
{
    private List<StudyStream> _streams = new ();
    private List<Teacher> _teachers = new ();
    private FacultyId? _facultyId;

    public OgnpCourseBuilder SetFacultyId(FacultyId facultyId)
    {
        if (_facultyId is not null && !_facultyId.Equals(facultyId))
            throw OgnpCourseException.WrongFacultyId();
        _facultyId = facultyId;
        return this;
    }

    public OgnpCourseBuilder AddStream(StudyStream stream)
    {
        if (_facultyId is null)
            throw OgnpCourseException.WrongFacultyId();
        _streams.Add(stream);
        return this;
    }

    public OgnpCourseBuilder AddTeacher(Teacher teacher)
    {
        if (_facultyId is null || !this._facultyId.Equals(teacher.FacultyId))
            throw OgnpCourseException.WrongFacultyId();
        _teachers.Add(teacher);
        return this;
    }

    public OgnpCourse Build()
    {
        if (_facultyId is null)
            throw OgnpCourseException.WrongFacultyId();
        OgnpCourse result = new OgnpCourse(_facultyId, _teachers, _streams);
        Reset();
        return result;
    }

    public void Reset()
    {
        _facultyId = null;
        _streams = new ();
        _teachers = new ();
    }
}