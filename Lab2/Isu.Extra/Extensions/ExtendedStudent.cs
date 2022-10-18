using System.Collections.ObjectModel;
using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;
using Isu.Extra.Wrappers;
using Isu.Services;

namespace Isu.Extra.Extensions;

public class ExtendedStudent : Student, IReadOnlyExtendedStudent, IEquatable<Student>
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

    public ExtendedStudent(IReadOnlyExtendedStudent copiedStudent)
        : base(copiedStudent.Name, copiedStudent.Group, copiedStudent.Id)
    {
        _facultyId = new FacultyId(copiedStudent.FacultyId);
        _group = new ExtendedGroup(copiedStudent.ExtendedGroup);
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

    public bool Equals(Student? other)
    {
        return base.Equals(other);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ExtendedStudent)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), _ognpCourses, _facultyId, _group);
    }

    protected bool Equals(ExtendedStudent other)
    {
        return base.Equals(other) && _ognpCourses.Equals(other._ognpCourses) && _facultyId.Equals(other._facultyId) && _group.Equals(other._group);
    }
}