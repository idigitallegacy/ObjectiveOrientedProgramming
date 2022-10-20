using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;
using Isu.Models;

namespace Isu.Extra.Builders;

public class OgnpCourseBuilder
{
    private List<StudyStreamDto> _streams = new ();
    private List<TeacherDto> _teachers = new ();
    private FacultyId? _facultyId;

    public OgnpCourseBuilder SetFacultyId(FacultyId facultyId)
    {
        if (_facultyId is not null && !_facultyId.Equals(facultyId))
            throw OgnpCourseException.WrongFacultyId();
        _facultyId = facultyId;
        return this;
    }

    public OgnpCourseBuilder AddStream(StudyStreamDto streamDto)
    {
        if (_facultyId is null)
            throw OgnpCourseException.WrongFacultyId();
        _streams.Add(streamDto);
        return this;
    }

    public OgnpCourseBuilder AddTeacher(TeacherDto teacherDto)
    {
        if (_facultyId is null || !this._facultyId.Equals(teacherDto.FacultyId))
            throw OgnpCourseException.WrongFacultyId();
        _teachers.Add(teacherDto);
        return this;
    }

    public OgnpCourseDto Build()
    {
        if (_facultyId is null)
            throw OgnpCourseException.WrongFacultyId();
        OgnpCourseDto result = new OgnpCourseDto(_facultyId, _teachers, _streams);
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