using System.Collections.ObjectModel;
using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;
using Isu.Extra.Wrappers;
using Isu.Services;

namespace Isu.Extra.Extensions;

public class ExtendedStudent : Student, IEquatable<Student>
{
    private List<OgnpCourseDto?> _ognpCourses = new List<OgnpCourseDto?> { null, null };
    private ExtendedGroupDto _group;

    public ExtendedStudent(string name, ExtendedGroup group, int id)
        : base(name, group, id)
    {
        FacultyId = group.GroupName.GetFacultyId();
        _group = new ExtendedGroupDto(group);
    }

    public ExtendedStudent(ExtendedStudentDto copiedStudentDto)
        : base(copiedStudentDto.Name, copiedStudentDto.Group, copiedStudentDto.Id)
    {
        FacultyId = new FacultyId(copiedStudentDto.FacultyId);
        _group = new ExtendedGroupDto(new ExtendedGroup(copiedStudentDto.ExtendedGroup));
    }

    public FacultyId FacultyId { get; }

    public ExtendedGroupDto ExtendedGroup => _group;
    public new Group Group => _group.ToExtendedGroup();

    public IReadOnlyCollection<OgnpCourseDto?> OgnpCourses => _ognpCourses.AsReadOnly();

    public void AddOgnpCourse(OgnpCourseDto courseDto)
    {
        if (_ognpCourses.All(needleCourse => needleCourse is not null))
            throw ExtendedStudentException.StudentHasAllOgnp();

        if (_ognpCourses[0] is null)
            _ognpCourses[0] = courseDto;
        else
            _ognpCourses[1] = courseDto;
    }

    public void ChangeOgnpCourse(OgnpCourseDto? oldOgnpCourse, OgnpCourseDto? newOgnpCourse)
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
        return HashCode.Combine(base.GetHashCode(), _ognpCourses, FacultyId, _group);
    }

    protected bool Equals(ExtendedStudent other)
    {
        return base.Equals(other);
    }
}