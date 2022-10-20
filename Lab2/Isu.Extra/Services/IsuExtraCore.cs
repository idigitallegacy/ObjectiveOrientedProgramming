using Isu.Entities;
using Isu.Extra.Builders;
using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Extensions;
using Isu.Extra.Models;
using Isu.Extra.Wrappers;
using Isu.Models;
using Isu.Services;

namespace Isu.Extra.Services;

public class IsuExtraCore
{
    private OgnpCourseBuilder _ognpCourseBuilder = new ();
    private LessonBuilder _lessonBuilder = new ();

    private IdGenerator _idGenerator = new ();
    private AudienceNumberGenerator _audienceNumberGenerator = new ();
    private List<OgnpCourseDto> _courses = new ();
    private List<TeacherDto> _teachers = new ();
    private List<ExtendedGroupDto> _groups = new ();
    private List<AudienceDto> _audiences = new ();

    public IReadOnlyCollection<OgnpCourseDto> Courses => _courses.AsReadOnly();
    public IReadOnlyCollection<TeacherDto> Teachers => _teachers.AsReadOnly();
    public IReadOnlyCollection<ExtendedGroupDto> Groups => _groups.AsReadOnly();
    public IReadOnlyCollection<AudienceDto> Audiences => _audiences.AsReadOnly();

    public IExtendedGroupDto AddGroup(GroupName groupName, int capacity)
    {
        ExtendedGroupDto groupDto = new ExtendedGroupDto(groupName, capacity);
        _groups.Add(groupDto);
        return groupDto;
    }

    public IExtendedStudentDto AddStudent(IExtendedGroupDto groupDto, string name)
    {
        ExtendedGroupDto extendedGroupDto = GetGroup(groupDto);
        ExtendedStudentDto studentDto = new ExtendedStudentDto(name, extendedGroupDto, _idGenerator.Id);
        extendedGroupDto.AddStudent(studentDto);
        _idGenerator.Update();
        return studentDto;
    }

    public ITeacherDto AddTeacher(string name, FacultyId facultyId)
    {
        TeacherDto teacherDto = new TeacherDto(name, facultyId);
        _teachers.Add(teacherDto);
        return teacherDto;
    }

    public IAudienceDto AddAudience()
    {
        AudienceDto audienceDto = new AudienceDto(_audienceNumberGenerator.Number);
        _audienceNumberGenerator.Update();
        _audiences.Add(audienceDto);
        return audienceDto;
    }

    public IStudyStreamDto AddStream(GroupName streamName, int streamCapacity)
    {
        StudyStreamDto streamDto = new StudyStreamDto(streamName, streamCapacity);
        return streamDto;
    }

    public void ConstructCourse(
        FacultyId facultyId,
        ITeacherDto? teacher = null,
        IStudyStreamDto? stream = null)
    {
        if (teacher is not null && !teacher.FacultyId.Equals(facultyId))
            throw IsuExtraCoreException.WrongFacultyId();

        _ognpCourseBuilder.SetFacultyId(facultyId);
        if (teacher is not null)
        {
            _ognpCourseBuilder.AddTeacher(GetTeacher(teacher));
        }

        if (stream is not null)
            _ognpCourseBuilder.AddStream(stream.ToStream());
    }

    public IOgnpCourseDto AppendConstructedCourse()
    {
        OgnpCourseDto courseDto = _ognpCourseBuilder.Build();
        _courses.Add(courseDto);
        return courseDto;
    }

    public void ConstructLesson(
        ITeacherDto? teacher = null,
        IAudienceDto? audience = null,
        IStudyStreamDto? associatedStream = null,
        IExtendedGroupDto? associatedGroup = null)
    {
        TeacherDto? rwTeacher = FindTeacher(teacher) ?? null;
        AudienceDto? rwAudience = FindAudience(audience);
        StudyStreamDto? rwStream = associatedStream?.ToStream();
        ExtendedGroupDto? rwGroup = FindGroup(associatedGroup);

        if (rwTeacher is not null)
            _lessonBuilder.Teacher(rwTeacher);
        if (rwAudience is not null)
            _lessonBuilder.Audience(rwAudience);
        if (rwStream is not null)
            _lessonBuilder.AssociatedStream(rwStream);
        if (rwGroup is not null)
            _lessonBuilder.AssociatedGroup(rwGroup);
    }

    public LessonDto ScheduleLesson(DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime)
    {
        return _lessonBuilder
            .WeekDay(dayOfWeek)
            .StartTime(startTime)
            .EndTime(endTime)
            .Build();
    }

    public void ScheduleGroup(Group group, IAudienceDto audienceDto, ILessonDto lessonDto)
    {
        ValidateLesson(audienceDto, lessonDto);

        ExtendedGroupDto rwGroupDto = GetGroup(group);
        LessonDto rwLessonDto = lessonDto.ToLesson();
        AudienceDto rwAudienceDto = GetAudience(audienceDto);

        rwGroupDto.AddLesson(lessonDto);
        GetTeacher(rwLessonDto.TeacherDto).AddLesson(rwLessonDto);
        rwAudienceDto.AddLesson(rwLessonDto);
    }

    public void ScheduleCourse(
        IOgnpCourseDto courseDto,
        IAudienceDto audienceDto,
        ILessonDto lessonDto)
    {
        ValidateLesson(audienceDto, lessonDto);

        OgnpCourseDto? rwCourse = _courses.Find(needleCourse => needleCourse.Equals(courseDto));
        LessonDto rwLessonDto = lessonDto.ToLesson();

        if (lessonDto.AssociatedStream is null)
            throw IsuExtraCoreException.StreamIsNotSet();
        if (rwCourse is null)
            throw IsuExtraCoreException.CourseNotFound();
        rwCourse.AddLesson(lessonDto.AssociatedStream, rwLessonDto);
        lessonDto.TeacherDto.AddLesson(rwLessonDto);
    }

    public void ScheduleStudent(IExtendedStudentDto studentDto, GroupName streamName, IOgnpCourseDto courseDto)
    {
        ValidateStudent(studentDto);
        ExtendedStudentDto rwStudentDto = (ExtendedStudentDto)studentDto;
        OgnpCourseDto? rwCourse = _courses.Find(needleCourse => needleCourse.Equals(courseDto));
        if (rwCourse is null)
            throw IsuExtraCoreException.CourseNotFound();
        rwCourse.AddStudent(streamName, studentDto);
        rwStudentDto.AddOgnpCourse(courseDto);
    }

    public void UnscheduleStudent(IExtendedStudentDto studentDto, IOgnpCourseDto? oldCourse)
    {
        ExtendedStudentDto rwStudentDto = (ExtendedStudentDto)studentDto;
        OgnpCourseDto? rwCourse = _courses.Find(needleCourse => needleCourse.Equals(oldCourse));
        if (rwCourse is null)
            throw IsuExtraCoreException.CourseNotFound();
        rwCourse?.RemoveStudent(studentDto);
        rwStudentDto.ChangeOgnpCourse(oldCourse, null);
    }

    public List<IStudyStreamDto> FindStreamsByCourse(CourseNumber courseNumber)
    {
        return _courses
            .SelectMany(course => course.Streams)
            .Where(stream => stream.GroupDto.GroupName.Course.Value == courseNumber.Value)
            .ToList();
    }

    public List<IExtendedStudentDto> FindStudentsAtCourse(IOgnpCourseDto courseDto)
    {
        return _courses
            .First(needleCourse => needleCourse.Equals(courseDto)).Streams
            .SelectMany(stream => stream.GroupDto.Students)
            .ToList();
    }

    public List<IExtendedStudentDto> FindStudentsAtStream(GroupName streamName)
    {
        return _courses
            .SelectMany(course => course.Streams)
            .Where(stream => stream.GroupDto.GroupName.Name == streamName.Name)
            .SelectMany(stream => stream.GroupDto.Students).ToList();
    }

    private void ValidateLesson(IAudienceDto audienceDto, ILessonDto lessonDto)
    {
        if (audienceDto.ScheduleDto.TimeIsScheduled(lessonDto.DayOfWeek, lessonDto.StartTime, lessonDto.EndTime) ||
            lessonDto.TeacherDto.ScheduleDto.TimeIsScheduled(lessonDto.DayOfWeek, lessonDto.StartTime, lessonDto.EndTime) ||

            (lessonDto.AssociatedGroup is not null &&
             lessonDto.AssociatedGroup.ScheduleDto.TimeIsScheduled(lessonDto.DayOfWeek, lessonDto.StartTime, lessonDto.EndTime)) ||

            (lessonDto.AssociatedStream is not null &&
             lessonDto.AssociatedStream.ScheduleDto.TimeIsScheduled(lessonDto.DayOfWeek, lessonDto.StartTime, lessonDto.EndTime)))
            throw SchedulerException.TimeIsAlreadyScheduled(lessonDto.DayOfWeek, lessonDto.StartTime, lessonDto.EndTime);
    }

    private void ValidateStudent(IExtendedStudentDto studentDto)
    {
        if (studentDto.OgnpCourses.All(course => course is not null))
            throw IsuExtraCoreException.StudentHasAllOgnp();
    }

    private TeacherDto? FindTeacher(ITeacherDto? teacher)
    {
        return _teachers.Find(needleTeacher => needleTeacher.Equals(teacher));
    }

    private ExtendedGroupDto? FindGroup(object? group)
    {
        return _groups.Find(needleGroup => needleGroup.Equals(group));
    }

    private AudienceDto? FindAudience(IAudienceDto? audience)
    {
        return _audiences.Find(needleAudience => needleAudience.Equals(audience));
    }

    private AudienceDto GetAudience(IAudienceDto? audience)
    {
        return FindAudience(audience) ?? throw IsuExtraCoreException.AudienceNotFound();
    }

    private ExtendedGroupDto GetGroup(object group)
    {
        return FindGroup(group) ?? throw IsuExtraCoreException.GroupNotFound(group);
    }

    private TeacherDto GetTeacher(ITeacherDto teacherDto)
    {
        return FindTeacher(teacherDto) ?? throw IsuExtraCoreException.TeacherNotFound(teacherDto);
    }
}