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
        IAudienceDto audienceDto = service.AddAudience();
        IStudyStreamDto streamDto = service.AddStream(groupName, groupCapacity);
        ITeacherDto teacherDto = service.AddTeacher(teacherNameSting, facultyId);
        IOgnpCourseDto courseDto = service.AddCourse(facultyId, teacherDto, streamDto);
        ILessonDto lessonDto =
            service.AddLesson(DayOfWeek.Monday, lessonStartTime, lessonEndTime, teacherDto, audienceDto, streamDto);
        service.ScheduleCourse(courseDto, audienceDto, lessonDto);

        Assert.Equal(streamDto, courseDto.Streams.First(needleStream => needleStream.Equals(streamDto)));
        Assert.Equal(courseDto, service.Courses.First());
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
        IAudienceDto audienceDto = service.AddAudience();
        IStudyStreamDto streamDto = service.AddStream(streamName, groupCapacity);
        ITeacherDto teacherDto = service.AddTeacher(teacherNameSting, facultyId);
        IOgnpCourseDto courseDto = service.AddCourse(facultyId, teacherDto, streamDto);
        IExtendedGroupDto groupDto = service.AddGroup(groupName, groupCapacity);

        IExtendedStudentDto studentDto = service.AddStudent(groupDto, "Michael");
        service.ScheduleStudent(studentDto, streamName, courseDto);

        Assert.Equal(courseDto, studentDto.OgnpCourses.First());
        Assert.Equal(studentDto, service.Courses.First().Streams.First().GroupDto.Students.First());
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
        IAudienceDto audienceDto = service.AddAudience();
        IStudyStreamDto streamDto = service.AddStream(streamName, groupCapacity);
        ITeacherDto teacherDto = service.AddTeacher(teacherNameSting, facultyId);
        IOgnpCourseDto courseDto = service.AddCourse(facultyId, teacherDto, streamDto);
        IExtendedGroupDto groupDto = service.AddGroup(groupName, groupCapacity);

        IExtendedStudentDto studentDto = service.AddStudent(groupDto, "Michael");
        service.ScheduleStudent(studentDto, streamName, courseDto);
        service.UnscheduleStudent(studentDto, courseDto);

        Assert.True(studentDto.OgnpCourses.First() is null);
        Assert.Equal(0, service.Courses.First().Streams.First().GroupDto.Students.Count);
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
        IAudienceDto audienceDto = service.AddAudience();
        IStudyStreamDto streamDto = service.AddStream(streamName, groupCapacity);
        ITeacherDto teacherDto = service.AddTeacher(teacherNameSting, facultyId);
        IOgnpCourseDto courseDto = service.AddCourse(facultyId, teacherDto, streamDto);
        IExtendedGroupDto groupDto = service.AddGroup(groupName, groupCapacity);

        IExtendedStudentDto studentDto = service.AddStudent(groupDto, "Michael");
        List<IExtendedStudentDto> students = new List<IExtendedStudentDto> { studentDto };
        service.ScheduleStudent(studentDto, streamName, courseDto);

        Assert.Equal(students, service.FindStudentsAtStream(streamName));
    }
}