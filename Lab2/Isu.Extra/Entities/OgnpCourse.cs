using Isu.Entities;
using Isu.Extra.Builders;
using Isu.Extra.Composites;
using Isu.Extra.Exceptions;
using Isu.Extra.Extensions;
using Isu.Extra.Models;
using Isu.Extra.Wrappers;
using Isu.Models;

namespace Isu.Extra.Entities;

public class OgnpCourseDto : IOgnpCourseDto, IEquatable<OgnpCourseDto>
{
    private List<StudyStreamDto> _streams;
    private List<TeacherDto> _teachers;
    private FacultyId _facultyId;

    public OgnpCourseDto(FacultyId facultyId, List<TeacherDto> teachers, List<StudyStreamDto> streams)
    {
        if (teachers.Count == 0)
            throw OgnpCourseException.TooFewTeachers();
        if (streams.Count == 0)
            throw OgnpCourseException.TooFewStreams();
        _streams = new List<StudyStreamDto>(streams);
        _teachers = new List<TeacherDto>(teachers);
        _facultyId = facultyId;
    }

    public OgnpCourseDto(IOgnpCourseDto copiedCourseDto)
    {
        _streams = new List<StudyStreamDto>(copiedCourseDto.Streams.Select(roStream => roStream.ToStream()));
        _teachers = new List<TeacherDto>(copiedCourseDto.Teachers.Select(roTeacher => roTeacher.ToTeacher()));
        _facultyId = new FacultyId(copiedCourseDto.FacultyId);
    }

    public IReadOnlyCollection<IStudyStreamDto> Streams => _streams.AsReadOnly();
    public IReadOnlyCollection<ITeacherDto> Teachers => _teachers.AsReadOnly();
    public FacultyId FacultyId => _facultyId;

    public void AddStudent(GroupName streamName, IExtendedStudentDto studentDto)
    {
        GetStream(streamName).AddStudent(studentDto);
    }

    public void RemoveStudent(IExtendedStudentDto studentDto)
    {
        _streams
            .First(stream => stream.GroupDto.Students.Contains(studentDto))
            .RemoveStudent(studentDto);
    }

    public ITeacherDto? FindTeacher(string name)
    {
        return _teachers.FirstOrDefault(teacher => teacher.Name == name);
    }

    public void AddLesson(StudyStreamDto streamDto, LessonDto lessonDto)
    {
        ValidateStreamLesson(streamDto, lessonDto);
        StudyStreamDto? rwStream = _streams.Find(needleStream => needleStream.Equals(streamDto));
        if (rwStream is null)
            throw OgnpCourseException.StreamNotFound(streamDto.GroupDto.GroupName);
        rwStream.AddLesson(lessonDto);
    }

    public void RemoveLesson(StudyStreamDto streamDto, LessonDto lessonDto)
    {
        streamDto.RemoveLesson(lessonDto);
    }

    public bool Equals(OgnpCourseDto? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _streams.Equals(other._streams) && _teachers.Equals(other._teachers) && _facultyId.Equals(other._facultyId);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((OgnpCourseDto)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_streams, _teachers, _facultyId);
    }

    private StudyStreamDto GetStream(GroupName streamName)
    {
        return _streams.FirstOrDefault(stream => stream.GroupDto.GroupName.Name == streamName.Name) ??
               throw OgnpCourseException.StreamNotFound(streamName);
    }

    private void ValidateStreamLesson(StudyStreamDto streamDto, LessonDto lessonDto)
    {
        if (streamDto.ScheduleDto.TimeIsScheduled(lessonDto.DayOfWeek, lessonDto.StartTime, lessonDto.EndTime))
            throw SchedulerException.TimeIsAlreadyScheduled(lessonDto.DayOfWeek, lessonDto.StartTime, lessonDto.EndTime);
    }
}