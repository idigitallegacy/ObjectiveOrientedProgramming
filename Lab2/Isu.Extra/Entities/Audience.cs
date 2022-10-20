using Isu.Extra.Composites;
using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Wrappers;

namespace Isu.Extra.Models;

public class AudienceDto : IScheduler, IAudienceDto, IEquatable<AudienceDto>
{
    private ScheduleDto _scheduleDto;
    private int _number;

    public AudienceDto(int number)
    {
        if (number < 0)
            throw SchedulerException.WrongAudienceNumber(number);
        _number = number;
        _scheduleDto = new ScheduleDto();
    }

    public AudienceDto(IAudienceDto copiedAudienceDto)
    {
        _scheduleDto = new ScheduleDto(copiedAudienceDto.ScheduleDto);
        _number = copiedAudienceDto.Number;
    }

    public IScheduleDto ScheduleDto => _scheduleDto;
    public int Number => _number;

    public void AddLesson(LessonDto lessonDto)
    {
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

    public bool Equals(AudienceDto? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _scheduleDto.Equals(other._scheduleDto) && _number == other._number;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((AudienceDto)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_scheduleDto, _number);
    }
}