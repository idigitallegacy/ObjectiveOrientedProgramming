using Isu.Extra.Extensions;
using Isu.Extra.Models;
using Isu.Extra.Services;
using Isu.Extra.Wrappers;
using Isu.Models;
using Isu.Services;
using Xunit;

namespace Isu.Extra.Test;

public class IsuExtraTest
{
    [Fact]
    public void AddNewCourse_CourseAdded()
    {
        IsuService oldService = new IsuService();
        IsuExtraCore coreService = new IsuExtraCore();
        IsuExtraAdapter service = new IsuExtraAdapter(oldService, coreService);

        int groupCapacity = 60;
        string groupNameString = "M3206";
        FacultyId facultyId = new FacultyId('M');
        string teacherNameSting = "Michael";
        TimeSpan lessonStartTime = new TimeSpan(10, 0, 0);
        TimeSpan lessonEndTime = new TimeSpan(11, 30, 0);

        GroupName groupName = new GroupName(groupNameString);
        AudienceDto audienceDto = service.AddAudience();
        StudyStreamDto streamDto = service.AddStream(groupName, groupCapacity);
        TeacherDto teacherDto = service.AddTeacher(teacherNameSting, facultyId);
        OgnpCourseDto courseDto = service.AddCourse(facultyId, teacherDto, streamDto);
        LessonDto lessonDto =
            service.AddLesson(DayOfWeek.Monday, lessonStartTime, lessonEndTime, teacherDto, audienceDto, streamDto);
        service.ScheduleCourse(courseDto, audienceDto, lessonDto);

        Assert.Equal(streamDto.ToStream(), courseDto.Streams.First().ToStream());
        Assert.Equal(courseDto.ToOgnpCourse(), new OgnpCourseDto(service.Courses.First()).ToOgnpCourse());
    }

    [Fact]
    public void AddStudentToCourse_StudentHasCourse_CourseContainsStudent()
    {
        IsuService oldService = new IsuService();
        IsuExtraCore coreService = new IsuExtraCore();
        IsuExtraAdapter service = new IsuExtraAdapter(oldService, coreService);

        int groupCapacity = 60;
        string groupNameString = "M3206";
        string streamNameString = "F3201";
        FacultyId facultyId = new FacultyId('F');
        string teacherNameSting = "Michael";
        TimeSpan lessonStartTime = new TimeSpan(10, 0, 0);
        TimeSpan lessonEndTime = new TimeSpan(11, 30, 0);

        GroupName groupName = new GroupName(groupNameString);
        GroupName streamName = new GroupName(streamNameString);
        AudienceDto audienceDto = service.AddAudience();
        StudyStreamDto streamDto = service.AddStream(streamName, groupCapacity);
        TeacherDto teacherDto = service.AddTeacher(teacherNameSting, facultyId);
        OgnpCourseDto courseDto = service.AddCourse(facultyId, teacherDto, streamDto);
        ExtendedGroupDto groupDto = service.AddGroup(groupName, groupCapacity);

        ExtendedStudentDto studentDto = service.AddStudent(groupDto, "Michael");
        service.ScheduleStudent(studentDto, streamName, courseDto);

        Assert.Equal(courseDto.ToOgnpCourse(), studentDto.OgnpCourses.First()?.ToOgnpCourse());
        Assert.Equal(studentDto.ToExtendedStudent(), service.Courses.First().Streams.First().Group.Students.First());
    }

    [Fact]
    public void RemoveStudentFromCourse_StudentHasNoCourse_CourseNotContainStudent()
    {
        IsuService oldService = new IsuService();
        IsuExtraCore coreService = new IsuExtraCore();
        IsuExtraAdapter service = new IsuExtraAdapter(oldService, coreService);

        int groupCapacity = 60;
        string groupNameString = "M3206";
        string streamNameString = "F3201";
        FacultyId facultyId = new FacultyId('F');
        string teacherNameSting = "Michael";
        TimeSpan lessonStartTime = new TimeSpan(10, 0, 0);
        TimeSpan lessonEndTime = new TimeSpan(11, 30, 0);

        GroupName groupName = new GroupName(groupNameString);
        GroupName streamName = new GroupName(streamNameString);
        AudienceDto audienceDto = service.AddAudience();
        StudyStreamDto streamDto = service.AddStream(streamName, groupCapacity);
        TeacherDto teacherDto = service.AddTeacher(teacherNameSting, facultyId);
        OgnpCourseDto courseDto = service.AddCourse(facultyId, teacherDto, streamDto);
        ExtendedGroupDto groupDto = service.AddGroup(groupName, groupCapacity);

        ExtendedStudentDto studentDto = service.AddStudent(groupDto, "Michael");
        service.ScheduleStudent(studentDto, streamName, courseDto);
        service.UnscheduleStudent(studentDto, courseDto);

        Assert.True(studentDto.OgnpCourses.First() is null);
        Assert.Equal(0, service.Courses.First().Streams.First().Group.Students.Count);
    }

    [Fact]
    public void GetStudentsFromOgnp()
    {
        IsuService oldService = new IsuService();
        IsuExtraCore coreService = new IsuExtraCore();
        IsuExtraAdapter service = new IsuExtraAdapter(oldService, coreService);

        int groupCapacity = 60;
        string groupNameString = "M3206";
        string streamNameString = "F3201";
        FacultyId facultyId = new FacultyId('F');
        string teacherNameSting = "Michael";
        TimeSpan lessonStartTime = new TimeSpan(10, 0, 0);
        TimeSpan lessonEndTime = new TimeSpan(11, 30, 0);

        GroupName groupName = new GroupName(groupNameString);
        GroupName streamName = new GroupName(streamNameString);
        AudienceDto audienceDto = service.AddAudience();
        StudyStreamDto streamDto = service.AddStream(streamName, groupCapacity);
        TeacherDto teacherDto = service.AddTeacher(teacherNameSting, facultyId);
        OgnpCourseDto courseDto = service.AddCourse(facultyId, teacherDto, streamDto);
        ExtendedGroupDto groupDto = service.AddGroup(groupName, groupCapacity);

        ExtendedStudentDto studentDto = service.AddStudent(groupDto, "Michael");
        List<ExtendedStudentDto> students = new List<ExtendedStudentDto> { studentDto };
        service.ScheduleStudent(studentDto, streamName, courseDto);

        Assert.Equal(students.Select(student => student.ToExtendedStudent()), service.FindStudentsAtStream(streamName).Select(student => student.ToExtendedStudent()));
    }
}