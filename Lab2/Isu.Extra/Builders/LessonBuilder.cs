using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Extensions;
using Isu.Extra.Models;
using Isu.Models;

namespace Isu.Extra.Builders;

public class LessonBuilder
{
    private DayOfWeek _weekDay;
    private TimeSpan _startTime;
    private TimeSpan _endTime;
    private Teacher? _teacher;
    private Audience? _audience;
    private StudyStream? _associatedStream;
    private ExtendedGroup? _associatedGroup;

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

    public LessonBuilder Teacher(Teacher teacher)
    {
        _teacher = teacher;
        return this;
    }

    public LessonBuilder Audience(Audience audience)
    {
        _audience = audience;
        return this;
    }

    public LessonBuilder AssociatedGroup(ExtendedGroup groupName)
    {
        _associatedGroup = groupName;
        return this;
    }

    public LessonBuilder AssociatedStream(StudyStream stream)
    {
        _associatedStream = stream;
        return this;
    }

    public Lesson Build()
    {
        if (_teacher is null || _audience is null)
            throw new Exception(); // TODO
        Lesson lesson = new Lesson(_weekDay, _startTime, _endTime, _teacher, _audience, _associatedStream, _associatedGroup);
        Reset();
        return lesson;
    }

    private void Reset()
    {
        _teacher = null;
        _associatedGroup = null;
    }
}