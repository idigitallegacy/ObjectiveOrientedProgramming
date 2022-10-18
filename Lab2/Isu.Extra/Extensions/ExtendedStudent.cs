using System.Collections.ObjectModel;
using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;
using Isu.Extra.Wrappers;
using Isu.Services;

namespace Isu.Extra.Extensions;

public class ExtendedStudent : Student, IReadOnlyExtendedStudent
{
    private List<IReadOnlyOgnpCourse?> _ognpCourses = new ();
    private FacultyId _facultyId;
    private IReadOnlyExtendedGroup _group;

    public ExtendedStudent(string name, ExtendedGroup group, int id)
        : base(name, group, id)
    {
        _facultyId = group.GroupName.GetFacultyId();
        _group = group;
    }

    public FacultyId FacultyId => _facultyId;
    public IReadOnlyExtendedGroup ExtendedGroup => _group;

    public IReadOnlyCollection<IReadOnlyOgnpCourse?> OgnpCourses => _ognpCourses;

    public void AddOgnpCourse(IReadOnlyOgnpCourse course)
    {
        if (_ognpCourses.Count == 2)
            throw ExtendedStudentException.StudentHasAllOgnp();
        else
            _ognpCourses.Add(course);
    }

    public void ChangeOgnpCourse(IReadOnlyOgnpCourse? oldOgnpCourse, IReadOnlyOgnpCourse? newOgnpCourse)
    {
        int needleCourseIndex = _ognpCourses.IndexOf(oldOgnpCourse);
        if (needleCourseIndex == -1)
            throw ExtendedStudentException.UnableToFindOgnp();
        _ognpCourses[needleCourseIndex] = newOgnpCourse;
    }
}