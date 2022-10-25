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
    private List<OgnpCourse> _courses = new ();
    private List<Teacher> _teachers = new ();
    private List<ExtendedGroup> _groups = new ();
    private List<Audience> _audiences = new ();
    private List<ExtendedStudent> _students = new ();

    public IReadOnlyCollection<OgnpCourse> Courses => _courses.AsReadOnly();
    public IReadOnlyCollection<Teacher> Teachers => _teachers.AsReadOnly();
    public IReadOnlyCollection<ExtendedGroup> Groups => _groups.AsReadOnly();
    public IReadOnlyCollection<Audience> Audiences => _audiences.AsReadOnly();

    public ExtendedGroupDto AddGroup(GroupName groupName, int capacity)
    {
        ExtendedGroup group = new ExtendedGroup(groupName, capacity);
        _groups.Add(group);
        return group.AsDto();
    }

    public ExtendedStudentDto AddStudent(ExtendedGroupDto groupDto, string name)
    {
        ExtendedGroup extendedGroup = GetGroup(groupDto.ToExtendedGroup());
        ExtendedStudent student = new ExtendedStudent(name, extendedGroup, _idGenerator.Id);
        extendedGroup.AddStudent(student);
        _students.Add(student);
        _idGenerator.Update();
        return student.AsDto();
    }

    public TeacherDto AddTeacher(string name, FacultyId facultyId)
    {
        Teacher teacher = new Teacher(name, facultyId);
        _teachers.Add(teacher);
        return teacher.AsDto();
    }

    public AudienceDto AddAudience()
    {
        Audience audience = new Audience(_audienceNumberGenerator.Number);
        _audienceNumberGenerator.Update();
        _audiences.Add(audience);
        return audience.AsDto();
    }

    public StudyStreamDto AddStream(GroupName streamName, int streamCapacity)
    {
        StudyStream stream = new StudyStream(streamName, streamCapacity);
        return stream.AsDto();
    }

    public void ConstructCourse(
        FacultyId facultyId,
        TeacherDto? teacher = null,
        StudyStreamDto? stream = null)
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

    public OgnpCourseDto AppendConstructedCourse()
    {
        OgnpCourse course = _ognpCourseBuilder.Build();
        _courses.Add(course);
        return course.AsDto();
    }

    public void ConstructLesson(
        TeacherDto? teacher = null,
        AudienceDto? audience = null,
        StudyStreamDto? associatedStream = null,
        ExtendedGroupDto? associatedGroup = null)
    {
        Teacher? rwTeacher = FindTeacher(teacher) ?? null;
        Audience? rwAudience = FindAudience(audience);
        StudyStream? rwStream = associatedStream?.ToStream();
        ExtendedGroup? rwGroup = FindGroup(associatedGroup);

        if (rwTeacher is not null)
            _lessonBuilder.Teacher(rwTeacher);
        if (rwAudience is not null)
            _lessonBuilder.Audience(rwAudience);
        if (rwStream is not null)
            _lessonBuilder.AssociatedStream(rwStream);
        if (rwGroup is not null)
            _lessonBuilder.AssociatedGroup(rwGroup);
    }

    public Lesson ScheduleLesson(DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime)
    {
        return _lessonBuilder
            .WeekDay(dayOfWeek)
            .StartTime(startTime)
            .EndTime(endTime)
            .Build();
    }

    public void ScheduleGroup(Group group, AudienceDto audienceDto, LessonDto lessonDto)
    {
        ValidateLesson(audienceDto, lessonDto);

        ExtendedGroup rwGroup = GetGroup(group);
        Lesson rwLesson = lessonDto.ToLesson();
        Audience rwAudience = GetAudience(audienceDto);

        rwGroup.AddLesson(lessonDto);
        GetTeacher(lessonDto.Teacher.AsDto()).AddLesson(rwLesson);
        rwAudience.AddLesson(rwLesson);
    }

    public void ScheduleCourse(
        OgnpCourseDto courseDto,
        AudienceDto audienceDto,
        LessonDto lessonDto)
    {
        ValidateLesson(audienceDto, lessonDto);

        OgnpCourse? rwCourse = _courses.Find(needleCourse => needleCourse.Equals(courseDto.ToOgnpCourse()));
        Lesson rwLesson = lessonDto.ToLesson();

        if (lessonDto.AssociatedStream is null)
            throw IsuExtraCoreException.StreamIsNotSet();
        if (rwCourse is null)
            throw IsuExtraCoreException.CourseNotFound();
        rwCourse.AddLesson(lessonDto.AssociatedStream, rwLesson);
        lessonDto.Teacher.AddLesson(rwLesson);
    }

    public void ScheduleStudent(ExtendedStudentDto studentDto, GroupName streamName, OgnpCourseDto courseDto)
    {
        ValidateStudent(studentDto);
        ExtendedStudent rwStudent = GetStudent(studentDto);
        OgnpCourse? rwCourse = _courses.Find(needleCourse => needleCourse.Equals(courseDto.ToOgnpCourse()));
        if (rwCourse is null)
            throw IsuExtraCoreException.CourseNotFound();
        rwCourse.AddStudent(streamName, studentDto);
        rwStudent.AddOgnpCourse(courseDto);
    }

    public void UnscheduleStudent(ExtendedStudentDto studentDto, OgnpCourseDto? oldCourse)
    {
        ExtendedStudent rwStudent = GetStudent(studentDto);
        OgnpCourse? rwCourse = _courses.Find(needleCourse => needleCourse.Equals(oldCourse?.ToOgnpCourse()));
        if (rwCourse is null)
            throw IsuExtraCoreException.CourseNotFound();
        rwCourse.RemoveStudent(rwStudent);
        rwStudent.ChangeOgnpCourse(oldCourse, null);
    }

    public List<StudyStreamDto> FindStreamsByCourse(CourseNumber courseNumber)
    {
        return _courses
            .SelectMany(course => course.Streams)
            .Where(stream => stream.Group.GroupName.Course.Value == courseNumber.Value)
            .ToList();
    }

    public List<ExtendedStudentDto> FindStudentsAtCourse(OgnpCourseDto courseDto)
    {
        return _courses
            .First(needleCourse => needleCourse.Equals(courseDto.ToOgnpCourse())).Streams
            .SelectMany(stream => stream.Group.Students)
            .Select(student => student.AsDto())
            .ToList();
    }

    public List<ExtendedStudentDto> FindStudentsAtStream(GroupName streamName)
    {
        return _courses
            .SelectMany(course => course.Streams)
            .Where(stream => stream.Group.GroupName.Name == streamName.Name)
            .SelectMany(stream => stream.Group.Students)
            .Select(student => student.AsDto())
            .ToList();
    }

    private void ValidateLesson(AudienceDto audienceDto, LessonDto lessonDto)
    {
        if (audienceDto.Schedule.ToSchedule().TimeIsScheduled(lessonDto.DayOfWeek, lessonDto.StartTime, lessonDto.EndTime) ||
            lessonDto.Teacher.Schedule.ToSchedule().TimeIsScheduled(lessonDto.DayOfWeek, lessonDto.StartTime, lessonDto.EndTime) ||

            (lessonDto.AssociatedGroup is not null &&
             lessonDto.AssociatedGroup.ScheduleDto.ToSchedule().TimeIsScheduled(lessonDto.DayOfWeek, lessonDto.StartTime, lessonDto.EndTime)) ||

            (lessonDto.AssociatedStream is not null &&
             lessonDto.AssociatedStream.Schedule.ToSchedule().TimeIsScheduled(lessonDto.DayOfWeek, lessonDto.StartTime, lessonDto.EndTime)))
            throw SchedulerException.TimeIsAlreadyScheduled(lessonDto.DayOfWeek, lessonDto.StartTime, lessonDto.EndTime);
    }

    private void ValidateStudent(ExtendedStudentDto studentDto)
    {
        if (studentDto.OgnpCourses.All(course => course is not null))
            throw IsuExtraCoreException.StudentHasAllOgnp();
    }

    private Teacher? FindTeacher(TeacherDto? teacher)
    {
        return _teachers.Find(needleTeacher => needleTeacher.Equals(teacher?.ToTeacher()));
    }

    private ExtendedGroup? FindGroup(object? group)
    {
        return _groups.Find(needleGroup => needleGroup.Equals(group));
    }

    private Audience? FindAudience(AudienceDto? audience)
    {
        return _audiences.Find(needleAudience => needleAudience.Equals(audience?.ToAudience()));
    }

    private Audience GetAudience(AudienceDto? audience)
    {
        return FindAudience(audience) ?? throw IsuExtraCoreException.AudienceNotFound();
    }

    private ExtendedStudent? FindStudent(ExtendedStudentDto? studentDto)
    {
        return _students.Find(needleStudent => needleStudent.Equals(studentDto?.ToExtendedStudent()));
    }

    private ExtendedStudent GetStudent(ExtendedStudentDto? studentDto)
    {
        return FindStudent(studentDto) ?? throw IsuExtraCoreException.AudienceNotFound();
    }

    private ExtendedGroup GetGroup(object group)
    {
        return FindGroup(group) ?? throw IsuExtraCoreException.GroupNotFound(group);
    }

    private Teacher GetTeacher(TeacherDto teacherDto)
    {
        return FindTeacher(teacherDto) ?? throw IsuExtraCoreException.TeacherNotFound(teacherDto);
    }
}