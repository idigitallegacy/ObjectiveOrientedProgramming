using Isu.Entities;
using Isu.Extra.Builders;
using Isu.Extra.Entities;
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

    private int _lastStudentId = 0;
    private int _lastAudienceNumber = 100;
    private List<OgnpCourse> _courses = new ();

    public IReadOnlyExtendedStudent AddStudent(IReadOnlyExtendedGroup group, string name)
    {
        ExtendedGroup extendedGroup = (ExtendedGroup)group;

        ExtendedStudent student = new ExtendedStudent(name, extendedGroup, _lastStudentId);
        extendedGroup.AddStudent(student);
        _lastStudentId++;

        return student;
    }

    public IReadOnlyTeacher AddTeacher(string name, FacultyId facultyId)
    {
        Teacher teacher = new Teacher(name, facultyId);
        return teacher;
    }

    public IReadOnlyAudience AddAudience()
    {
        Audience audience = new Audience(_lastAudienceNumber);
        _lastAudienceNumber++;
        return audience;
    }

    public IReadOnlyStudyStream AddStream(GroupName streamName, int streamCapacity)
    {
        StudyStream stream = new StudyStream(streamName, streamCapacity);
        return stream;
    }

    public void ConstructCourse(FacultyId facultyId, IReadOnlyTeacher? teacher = null, IReadOnlyStudyStream? stream = null)
    {
        if (teacher?.FacultyId != facultyId)
            throw new Exception(); // TODO
        Teacher? rwTeacher = teacher is null ? null : (Teacher)teacher;
        StudyStream? rwStream = stream is null ? null : (StudyStream)stream;

        _ognpCourseBuilder.SetFacultyId(facultyId);
        if (rwTeacher is not null)
            _ognpCourseBuilder.AddTeacher(rwTeacher);
        if (rwStream is not null)
            _ognpCourseBuilder.AddStream(rwStream);
    }

    public IReadOnlyOgnpCourse AppendConstructedCourse()
    {
        OgnpCourse course = _ognpCourseBuilder.Build();
        _courses.Add(course);
        return course;
    }

    public Lesson ConstructLesson(
        IReadOnlyTeacher? teacher = null,
        IReadOnlyAudience? audience = null,
        IReadOnlyStudyStream? associatedStream = null,
        IReadOnlyExtendedGroup? associatedGroup = null)
    {
        Teacher? rwTeacher = teacher is null ? null : (Teacher)teacher;
        Audience? rwAudience = audience is null ? null : (Audience)audience;
        StudyStream? rwStream = associatedStream is null ? null : (StudyStream)associatedStream;
        ExtendedGroup? rwGroup = associatedGroup is null ? null : (ExtendedGroup)associatedGroup;

        if (rwTeacher is not null)
            _lessonBuilder.Teacher(rwTeacher);
        if (rwAudience is not null)
            _lessonBuilder.Audience(rwAudience);
        if (rwStream is not null)
            _lessonBuilder.AssociatedStream(rwStream);
        if (rwGroup is not null)
            _lessonBuilder.AssociatedGroup(rwGroup);

        return _lessonBuilder.Build();
    }

    public void ScheduleGroup(Group group, IReadOnlyAudience audience, IReadOnlyLesson lesson)
    {
        ValidateLesson(audience, lesson);

        ExtendedGroup rwGroup = (ExtendedGroup)group;
        Lesson rwLesson = (Lesson)lesson;
        Audience rwAudience = (Audience)audience;

        rwGroup.AddLesson(lesson);
        rwLesson.Teacher.AddLesson(rwLesson);
        rwAudience.AddLesson(rwLesson);
    }

    public void ScheduleCourse(
        IReadOnlyOgnpCourse course,
        IReadOnlyAudience audience,
        IReadOnlyLesson lesson)
    {
        ValidateLesson(audience, lesson);

        OgnpCourse rwCourse = (OgnpCourse)course;
        Lesson rwLesson = (Lesson)lesson;

        if (lesson.AssociatedStream is null)
            throw new Exception(); // TODO
        rwCourse.AddLesson(lesson.AssociatedStream, rwLesson);
        lesson.Teacher.AddLesson(rwLesson);
    }

    public void ScheduleStudent(IReadOnlyExtendedStudent student, GroupName streamName, IReadOnlyOgnpCourse course)
    {
        ValidateStudent(student);
        ExtendedStudent rwStudent = (ExtendedStudent)student;
        OgnpCourse rwCourse = (OgnpCourse)course;
        rwCourse.AddStudent(streamName, student);
        rwStudent.AddOgnpCourse(course);
    }

    public void UnscheduleStudent(IReadOnlyExtendedStudent student, IReadOnlyOgnpCourse? oldCourse)
    {
        ExtendedStudent rwStudent = (ExtendedStudent)student;
        OgnpCourse? rwCourse = null;
        if (oldCourse is not null)
            rwCourse = (OgnpCourse)oldCourse;
        rwCourse?.RemoveStudent(student);
        rwStudent.ChangeOgnpCourse(oldCourse, null);
    }

    public List<IReadOnlyStudyStream> FindStreamsByCourse(CourseNumber courseNumber)
    {
        return _courses
            .SelectMany(course => course.Streams)
            .Where(stream => stream.Group.GroupName.Course.Value == courseNumber.Value)
            .ToList();
    }

    public List<IReadOnlyExtendedStudent> FindStudentsAtCourse(OgnpCourse course)
    {
        return _courses
            .First(needleCourse => needleCourse == course).Streams
            .SelectMany(stream => stream.Group.Students)
            .ToList();
    }

    public List<IReadOnlyExtendedStudent> FindStudentsAtStream(GroupName streamName)
    {
        return _courses
            .SelectMany(course => course.Streams)
            .Where(stream => stream.Group.GroupName.Name == streamName.Name)
            .SelectMany(stream => stream.Group.Students).ToList();
    }

    private void ValidateLesson(IReadOnlyAudience audience, IReadOnlyLesson lesson)
    {
        if (audience.Schedule.TimeIsScheduled(lesson.DayOfWeek, lesson.StartTime, lesson.EndTime))
            throw new Exception(); // TODO
        if (lesson.Teacher.Schedule.TimeIsScheduled(lesson.DayOfWeek, lesson.StartTime, lesson.EndTime))
            throw new Exception(); // TODO
        if (lesson.AssociatedGroup is not null &&
            lesson.AssociatedGroup.Schedule.TimeIsScheduled(lesson.DayOfWeek, lesson.StartTime, lesson.EndTime))
            throw new Exception(); // TODO
        if (lesson.AssociatedStream is not null &&
            lesson.AssociatedStream.Schedule.TimeIsScheduled(lesson.DayOfWeek, lesson.StartTime, lesson.EndTime))
            throw new Exception(); // TODO
    }

    private void ValidateStudent(IReadOnlyExtendedStudent student)
    {
        if (student.OgnpCourses.All(course => course is not null))
            throw new Exception();
    }
}