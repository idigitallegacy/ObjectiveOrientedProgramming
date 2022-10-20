using Isu.Entities;
using Isu.Extra.Composites;
using Isu.Extra.Exceptions;
using Isu.Extra.Extensions;
using Isu.Extra.Models;
using Isu.Extra.Wrappers;
using Isu.Models;

namespace Isu.Extra.Entities;

public class StudyStreamDto : IScheduler, IStudyStreamDto, IEquatable<StudyStreamDto>
{
    private ScheduleDto _scheduleDto;
    private ExtendedGroupDto _groupDto;

    public StudyStreamDto(GroupName streamName, int streamCapacity)
    {
        _scheduleDto = new ScheduleDto();
        _groupDto = new ExtendedGroupDto(streamName, streamCapacity);
    }

    public StudyStreamDto(IStudyStreamDto copiedStreamDto)
    {
        _scheduleDto = new ScheduleDto(copiedStreamDto.ScheduleDto);
        _groupDto = new ExtendedGroupDto(copiedStreamDto.GroupDto);
    }

    public IScheduleDto ScheduleDto => _scheduleDto;
    public ExtendedGroupDto GroupDto => _groupDto;

    public void AddStudent(IExtendedStudentDto studentDto)
    {
        ValidateStudent(studentDto);
        _groupDto.AddStudent(studentDto);
    }

    public void RemoveStudent(IExtendedStudentDto studentDto)
    {
        _groupDto.RemoveStudent(studentDto.ToExtendedStudent());
    }

    public void AddLesson(LessonDto lessonDto)
    {
        _groupDto.AddLesson(lessonDto);
        _scheduleDto.AddLesson(lessonDto);
    }

    public void RemoveLesson(LessonDto lessonDto)
    {
        _scheduleDto.RemoveLesson(lessonDto);
    }

    public LessonDto? FindLesson(DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime)
    {
        return _scheduleDto.FindLesson(dayOfWeek, startTime, endTime);
    }

    public bool Equals(StudyStreamDto? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _groupDto.GroupName == other._groupDto.GroupName;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((StudyStreamDto)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_scheduleDto, _groupDto);
    }

    private void ValidateStudent(IExtendedStudentDto studentDto)
    {
        bool studentGroupTimeIsScheduled =
            _scheduleDto.Lessons.Any(lesson =>
            {
                return studentDto.ExtendedGroupDto.ScheduleDto
                    .TimeIsScheduled(lesson.DayOfWeek, lesson.StartTime, lesson.EndTime);
            });
        bool studentIsScheduledByOgnp =
            studentDto.OgnpCourses.Any(course =>
            {
                bool? returnValue = course?.Streams
                    .Where(stream => stream.GroupDto.Students.Contains(studentDto))
                    .Any(stream =>
                    {
                        return _scheduleDto.Lessons.Any(lesson =>
                            stream.ScheduleDto.TimeIsScheduled(lesson.DayOfWeek, lesson.StartTime, lesson.EndTime));
                    });
                return returnValue is not null && returnValue == true;
            });
        if (studentGroupTimeIsScheduled || studentIsScheduledByOgnp)
            throw SchedulerException.TimeIsAlreadyScheduled();
    }
}