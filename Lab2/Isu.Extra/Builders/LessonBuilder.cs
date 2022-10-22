using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Extensions;
using Isu.Extra.Models;
using Isu.Models;

namespace Isu.Extra.Builders;

public class LessonBuilder
{
    private DayOfWeek _weekDay;
    private TimeSpan _startTime;
    private TimeSpan _endTime;
    private TeacherDto? _teacher;
    private Audience? _audience;
    private StudyStreamDto? _associatedStream;
    private ExtendedGroupDto? _associatedGroup;

    public LessonBuilder WeekDay(DayOfWeek weekDay)
    {
        _weekDay = weekDay;
        return this;
    }

    public LessonBuilder StartTime(TimeSpan time)
    {
        _startTime = time;
        return this;
    }

    public LessonBuilder EndTime(TimeSpan time)
    {
        _endTime = time;
        return this;
    }

    public LessonBuilder Teacher(TeacherDto teacherDto)
    {
        _teacher = teacherDto;
        return this;
    }

    public LessonBuilder Audience(Audience audience)
    {
        _audience = audience;
        return this;
    }

    public LessonBuilder AssociatedGroup(ExtendedGroupDto groupDtoName)
    {
        _associatedGroup = groupDtoName;
        return this;
    }

    public LessonBuilder AssociatedStream(StudyStreamDto streamDto)
    {
        _associatedStream = streamDto;
        return this;
    }

    public LessonDto Build()
    {
        if (_teacher is null || _audience is null)
            throw LessonException.NoTeacherOrAudience();
        LessonDto lessonDto = new LessonDto(_weekDay, _startTime, _endTime, _teacher, _audience, _associatedStream, _associatedGroup);
        Reset();
        return lessonDto;
    }

    private void Reset()
    {
        _teacher = null;
        _associatedGroup = null;
    }
}