using System.Collections.ObjectModel;
using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;
using Isu.Extra.Wrappers;
using Isu.Services;

namespace Isu.Extra.Extensions;

public class ExtendedStudentDto : Student, IExtendedStudentDto, IEquatable<Student>
{
    private List<IOgnpCourseDto?> _ognpCourses = new List<IOgnpCourseDto?> { null, null };
    private IExtendedGroupDto _groupDto;

    public ExtendedStudentDto(string name, ExtendedGroupDto groupDto, int id)
        : base(name, groupDto, id)
    {
        FacultyId = groupDto.GroupName.GetFacultyId();
        _groupDto = groupDto;
    }

    public ExtendedStudentDto(IExtendedStudentDto copiedStudentDto)
        : base(copiedStudentDto.Name, copiedStudentDto.Group, copiedStudentDto.Id)
    {
        FacultyId = new FacultyId(copiedStudentDto.FacultyId);
        _groupDto = new ExtendedGroupDto(copiedStudentDto.ExtendedGroupDto);
    }

    public FacultyId FacultyId { get; }

    public IExtendedGroupDto ExtendedGroupDto => _groupDto;
    public new Group Group => _groupDto.ToExtendedGroup();

    public IReadOnlyCollection<IOgnpCourseDto?> OgnpCourses => _ognpCourses.AsReadOnly();

    public void AddOgnpCourse(IOgnpCourseDto courseDto)
    {
        if (_ognpCourses.All(needleCourse => needleCourse is not null))
            throw ExtendedStudentException.StudentHasAllOgnp();

        if (_ognpCourses[0] is null)
            _ognpCourses[0] = courseDto;
        else
            _ognpCourses[1] = courseDto;
    }

    public void ChangeOgnpCourse(IOgnpCourseDto? oldOgnpCourse, IOgnpCourseDto? newOgnpCourse)
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
        return Equals((ExtendedStudentDto)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), _ognpCourses, FacultyId, _groupDto);
    }

    protected bool Equals(ExtendedStudentDto other)
    {
        return base.Equals(other);
    }
}