using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Extensions;
using Isu.Extra.Models;
using Isu.Extra.Wrappers;
using Isu.Models;
using Isu.Services;

namespace Isu.Extra.Services;

public class IsuExtraAdapter : IIsuService
{
    private IIsuService _oldService;
    private IsuExtraCore _coreService;
    public IsuExtraAdapter(IIsuService oldService, IsuExtraCore coreService)
    {
        _oldService = oldService;
        _coreService = coreService;
    }

    public IReadOnlyCollection<OgnpCourseDto> Courses => _coreService.Courses;
    public IReadOnlyCollection<TeacherDto> Teachers => _coreService.Teachers;
    public IReadOnlyCollection<ExtendedGroupDto> Groups => _coreService.Groups;
    public IReadOnlyCollection<AudienceDto> Audiences => _coreService.Audiences;

    public Group AddGroup(GroupName name)
    {
        return _oldService.AddGroup(name);
    }

    public IExtendedGroupDto AddGroup(GroupName name, int capacity)
    {
        _oldService.AddGroup(name);
        return _coreService.AddGroup(name, capacity);
    }

    public Student AddStudent(Group group, string name)
    {
        return _oldService.AddStudent(group, name);
    }

    public IExtendedStudentDto AddStudent(IExtendedGroupDto groupDto, string name)
    {
        _oldService.AddStudent(groupDto.ToExtendedGroup(), name);
        return _coreService.AddStudent(groupDto, name);
    }

    public Student GetStudent(int id)
    {
        return _oldService.GetStudent(id);
    }

    public Student? FindStudent(int id)
    {
        return _oldService.FindStudent(id);
    }

    public List<Student> FindStudents(GroupName groupName)
    {
        return _oldService.FindStudents(groupName);
    }

    public List<Student> FindStudents(CourseNumber courseNumber)
    {
        return _oldService.FindStudents(courseNumber);
    }

    public Group? FindGroup(GroupName groupName)
    {
        return _oldService.FindGroup(groupName);
    }

    public List<Group> FindGroups(CourseNumber courseNumber)
    {
        return _oldService.FindGroups(courseNumber);
    }

    public void ChangeStudentGroup(Student student, Group newGroup)
    {
        _oldService.ChangeStudentGroup(student, newGroup);
    }

    public IStudyStreamDto AddStream(GroupName name, int capacity)
    {
        _oldService.AddGroup(name);
        return _coreService.AddStream(name, capacity);
    }

    public IAudienceDto AddAudience()
    {
        return _coreService.AddAudience();
    }

    public ITeacherDto AddTeacher(string name, FacultyId facultyId)
    {
        return _coreService.AddTeacher(name, facultyId);
    }

    public IOgnpCourseDto AddCourse(
        FacultyId facultyId,
        ITeacherDto teacherDto,
        IStudyStreamDto streamDto)
    {
        _coreService.ConstructCourse(facultyId, teacherDto, streamDto);
        return _coreService.AppendConstructedCourse();
    }

    public ILessonDto AddLesson(
        DayOfWeek dayOfWeek,
        TimeSpan startTime,
        TimeSpan endTime,
        ITeacherDto teacherDto,
        IAudienceDto audienceDto,
        IStudyStreamDto? stream = null,
        IExtendedGroupDto? group = null)
    {
        _coreService.ConstructLesson(teacherDto, audienceDto, stream, group);
        return _coreService.ScheduleLesson(dayOfWeek, startTime, endTime);
    }

    public void ScheduleCourse(
        IOgnpCourseDto courseDto,
        IAudienceDto audienceDto,
        ILessonDto lessonDto)
    {
        _coreService.ScheduleCourse(courseDto, audienceDto, lessonDto);
    }

    public void ScheduleGroup(Group group, IAudienceDto audienceDto, ILessonDto lessonDto)
    {
        _coreService.ScheduleGroup(group, audienceDto, lessonDto);
    }

    public void ScheduleStudent(IExtendedStudentDto studentDto, GroupName streamName, IOgnpCourseDto courseDto)
    {
        _coreService.ScheduleStudent(studentDto, streamName, courseDto);
    }

    public void UnscheduleStudent(IExtendedStudentDto studentDto, IOgnpCourseDto? oldCourse)
    {
        _coreService.UnscheduleStudent(studentDto, oldCourse);
    }

    public List<IStudyStreamDto> FindStreamsByCourse(CourseNumber courseNumber)
    {
        return _coreService.FindStreamsByCourse(courseNumber);
    }

    public List<IExtendedStudentDto> FindStudentsAtCourse(IOgnpCourseDto courseDto)
    {
        return _coreService.FindStudentsAtCourse(courseDto);
    }

    public List<IExtendedStudentDto> FindStudentsAtStream(GroupName streamName)
    {
        return _coreService.FindStudentsAtStream(streamName);
    }
}