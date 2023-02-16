using Isu.Entities;
using Isu.Extra.Extensions;
using Isu.Extra.Models;
using Isu.Models;
using Isu.Services;

namespace Isu.Extra.Wrappers;

public class ExtendedGroupDto
{
    public ExtendedGroupDto(ExtendedGroup group)
    {
        ScheduleDto = group.ScheduleDto;
        GroupName = group.GroupName;
        Students = group.Students.Select(student => new ExtendedStudentDto(student)).ToList();
        Capacity = group.Capacity;
        FacultyId = group.FacultyId;
    }

    public ScheduleDto ScheduleDto { get; }
    public GroupName GroupName { get; }
    public IReadOnlyCollection<ExtendedStudentDto> Students { get; }
    public int Capacity { get; }
    public FacultyId FacultyId { get; }

    public ExtendedGroup ToExtendedGroup()
    {
        return new ExtendedGroup(this);
    }
}