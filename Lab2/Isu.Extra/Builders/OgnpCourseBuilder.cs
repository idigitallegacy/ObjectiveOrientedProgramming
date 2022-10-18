using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;
using Isu.Models;

namespace Isu.Extra.Builders;

public class OgnpCourseBuilder
{
    private bool _facultyIdIsSet = false;
    private List<StudyStream> _streams = new ();
    private List<Teacher> _teachers = new ();
    private FacultyId _facultyId;

    public OgnpCourseBuilder SetFacultyId(FacultyId facultyId)
    {
        if (_facultyIdIsSet && (_facultyId != facultyId))
            throw OgnpCourseException.WrongFacultyId();
        _facultyId = facultyId;
        _facultyIdIsSet = true;
        return this;
    }

    public OgnpCourseBuilder AddStream(StudyStream stream)
    {
        ValidateFacultyIdIsSet();
        _streams.Add(stream);
        return this;
    }

    public OgnpCourseBuilder AddTeacher(Teacher teacher)
    {
        ValidateFacultyIdIsSet();
        if (teacher.FacultyId != _facultyId)
            throw OgnpCourseException.WrongFacultyId();
        _teachers.Add(teacher);
        return this;
    }

    public OgnpCourse Build()
    {
        ValidateFacultyIdIsSet();
        OgnpCourse result = new OgnpCourse(_facultyId, _teachers, _streams);
        Reset();
        return result;
    }

    public void Reset()
    {
        _facultyIdIsSet = false;
        _streams = new ();
        _teachers = new ();
    }

    private void ValidateFacultyIdIsSet()
    {
        if (!_facultyIdIsSet)
            throw OgnpCourseException.WrongFacultyId();
    }
}