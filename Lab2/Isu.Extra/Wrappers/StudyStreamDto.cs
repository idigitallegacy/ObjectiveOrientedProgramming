using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Extensions;

namespace Isu.Extra.Wrappers;

public class StudyStreamDto
{
    public StudyStreamDto(StudyStream stream)
    {
        Schedule = stream.Schedule;
        Group = stream.Group;
    }

    public ScheduleDto Schedule { get; }
    public ExtendedGroup Group { get; }

    public StudyStream ToStream()
    {
        return new StudyStream(this);
    }
}