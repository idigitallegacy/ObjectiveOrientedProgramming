using Isu.Entities;
using Isu.Extra.Builders;
using Isu.Extra.Composites;
using Isu.Extra.Exceptions;
using Isu.Extra.Extensions;
using Isu.Extra.Models;
using Isu.Extra.Wrappers;
using Isu.Models;

namespace Isu.Extra.Entities;

public class OgnpCourse : IEquatable<OgnpCourse>
{
    private List<StudyStream> _streams;
    private List<Teacher> _teachers;
    private FacultyId _facultyId;

    public OgnpCourse(FacultyId facultyId, List<Teacher> teachers, List<StudyStream> streams)
    {
        if (teachers.Count == 0)
            throw OgnpCourseException.TooFewTeachers();
        if (streams.Count == 0)
            throw OgnpCourseException.TooFewStreams();
        _streams = new List<StudyStream>(streams);
        _teachers = new List<Teacher>(teachers);
        _facultyId = facultyId;
    }

    public OgnpCourse(OgnpCourseDto copiedCourseDto)
    {
        _streams = new List<StudyStream>(copiedCourseDto.Streams.Select(roStream => roStream.ToStream()));
        _teachers = new List<Teacher>(copiedCourseDto.Teachers.Select(roTeacher => roTeacher.ToTeacher()));
        _facultyId = new FacultyId(copiedCourseDto.FacultyId);
    }

    public IEnumerable<StudyStreamDto> Streams => _streams.Select(stream => stream.AsDto());
    public IEnumerable<TeacherDto> Teachers => _teachers.Select(teacher => teacher.AsDto());
    public FacultyId FacultyId => _facultyId;

    public void AddStudent(GroupName streamName, ExtendedStudentDto studentDto)
    {
        GetStream(streamName).AddStudent(studentDto);
    }

    public void RemoveStudent(ExtendedStudent student)
    {
        var st = _streams
            .First(stream => stream.Group.Students.Contains(student));
        _streams
            .First(stream => stream.Group.Students.Contains(student))
            .RemoveStudent(student);
    }

    public TeacherDto? FindTeacher(string name)
    {
        Teacher? needle = _teachers.FirstOrDefault(teacher => teacher.Name == name);
        if (needle is null)
            return null;
        return needle.AsDto();
    }

    public void AddLesson(StudyStream stream, Lesson lesson)
    {
        ValidateStreamLesson(stream, lesson);
        StudyStream? rwStream = _streams.Find(needleStream => needleStream.Equals(stream));
        if (rwStream is null)
            throw OgnpCourseException.StreamNotFound(stream.Group.GroupName);
        rwStream.AddLesson(lesson);
    }

    public void RemoveLesson(StudyStream stream, Lesson lesson)
    {
        stream.RemoveLesson(lesson);
    }

    public OgnpCourseDto AsDto()
    {
        return new OgnpCourseDto(this);
    }

    public bool Equals(OgnpCourse? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _streams.All(stream => other._streams.All(otherStream => otherStream.Equals(stream)));
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((OgnpCourse)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_streams);
    }

    private StudyStream GetStream(GroupName streamName)
    {
        return _streams.FirstOrDefault(stream => stream.Group.GroupName.Name == streamName.Name) ??
               throw OgnpCourseException.StreamNotFound(streamName);
    }

    private void ValidateStreamLesson(StudyStream stream, Lesson lesson)
    {
        if (stream.Schedule.ToSchedule().TimeIsScheduled(lesson.DayOfWeek, lesson.StartTime, lesson.EndTime))
            throw SchedulerException.TimeIsAlreadyScheduled(lesson.DayOfWeek, lesson.StartTime, lesson.EndTime);
    }
}