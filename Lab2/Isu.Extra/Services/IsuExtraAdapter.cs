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

    public IReadOnlyCollection<OgnpCourse> Courses => _coreService.Courses;
    public IReadOnlyCollection<Teacher> Teachers => _coreService.Teachers;
    public IReadOnlyCollection<ExtendedGroup> Groups => _coreService.Groups;
    public IReadOnlyCollection<Audience> Audiences => _coreService.Audiences;

    public Group AddGroup(GroupName name)
    {
        return _oldService.AddGroup(name);
    }

    public ExtendedGroupDto AddGroup(GroupName name, int capacity)
    {
        _oldService.AddGroup(name);
        return _coreService.AddGroup(name, capacity);
    }

    public Student AddStudent(Group group, string name)
    {
        return _oldService.AddStudent(group, name);
    }

    public ExtendedStudentDto AddStudent(ExtendedGroupDto groupDto, string name)
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

    public StudyStreamDto AddStream(GroupName name, int capacity)
    {
        _oldService.AddGroup(name);
        return _coreService.AddStream(name, capacity);
    }

    public AudienceDto AddAudience()
    {
        return _coreService.AddAudience();
    }

    public TeacherDto AddTeacher(string name, FacultyId facultyId)
    {
        return _coreService.AddTeacher(name, facultyId);
    }

    public OgnpCourseDto AddCourse(
        FacultyId facultyId,
        TeacherDto teacherDto,
        StudyStreamDto streamDto)
    {
        _coreService.ConstructCourse(facultyId, teacherDto, streamDto);
        return _coreService.AppendConstructedCourse();
    }

    public LessonDto AddLesson(
        DayOfWeek dayOfWeek,
        TimeSpan startTime,
        TimeSpan endTime,
        TeacherDto teacherDto,
        AudienceDto audienceDto,
        StudyStreamDto? stream = null,
        ExtendedGroupDto? group = null)
    {
        _coreService.ConstructLesson(teacherDto, audienceDto, stream, group);
        return new LessonDto(_coreService.ScheduleLesson(dayOfWeek, startTime, endTime));
    }

    public void ScheduleCourse(
        OgnpCourseDto courseDto,
        AudienceDto audienceDto,
        LessonDto lessonDto)
    {
        _coreService.ScheduleCourse(courseDto, audienceDto, lessonDto);
    }

    public void ScheduleGroup(Group group, AudienceDto audienceDto, LessonDto lessonDto)
    {
        _coreService.ScheduleGroup(group, audienceDto, lessonDto);
    }

    public void ScheduleStudent(ExtendedStudentDto studentDto, GroupName streamName, OgnpCourseDto courseDto)
    {
        _coreService.ScheduleStudent(studentDto, streamName, courseDto);
    }

    public void UnscheduleStudent(ExtendedStudentDto studentDto, OgnpCourseDto? oldCourse)
    {
        _coreService.UnscheduleStudent(studentDto, oldCourse);
    }

    public List<StudyStreamDto> FindStreamsByCourse(CourseNumber courseNumber)
    {
        return _coreService.FindStreamsByCourse(courseNumber);
    }

    public List<ExtendedStudentDto> FindStudentsAtCourse(OgnpCourseDto courseDto)
    {
        return _coreService.FindStudentsAtCourse(courseDto);
    }

    public List<ExtendedStudentDto> FindStudentsAtStream(GroupName streamName)
    {
        return _coreService.FindStudentsAtStream(streamName);
    }
}