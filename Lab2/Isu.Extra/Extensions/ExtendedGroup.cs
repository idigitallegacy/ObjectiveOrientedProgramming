using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Models;
using Isu.Extra.Wrappers;
using Isu.Models;
using Isu.Services;

namespace Isu.Extra.Extensions;

public class ExtendedGroup : Group, IReadOnlyExtendedGroup
{
    private Schedule _schedule = new ();
    private List<IReadOnlyExtendedStudent> _students = new ();
    private FacultyId _facultyId;

    public ExtendedGroup(GroupName groupName, int capacity)
        : base(groupName, capacity)
    {
        _facultyId = groupName.GetFacultyId();
        Capacity = capacity;
    }

    public ExtendedGroup(IReadOnlyExtendedGroup copiedGroup)
        : base(copiedGroup.GroupName, copiedGroup.Capacity)
    {
        _schedule = new Schedule(copiedGroup.Schedule);
        _students = new List<IReadOnlyExtendedStudent>(copiedGroup.Students);
        _facultyId = new FacultyId(copiedGroup.FacultyId);
        Capacity = copiedGroup.Capacity;
    }

    public IReadOnlySchedule Schedule => _schedule;
    public new IReadOnlyCollection<IReadOnlyExtendedStudent> Students => _students;
    public FacultyId FacultyId => _facultyId;

    public int Capacity { get; }

    public void AddStudent(IReadOnlyExtendedStudent student)
    {
        AddStudent(new Student(student.Name, student.Group, student.Id));
        _students.Add(student);
    }

    public void RemoveStudent(IReadOnlyExtendedStudent student)
    {
        RemoveStudent(new Student(student.Name, student.Group, student.Id));
        _students.Remove(student);
    }

    public void AddLesson(IReadOnlyLesson lesson)
    {
        Lesson rwLesson = (Lesson)lesson;
        _schedule.AddLesson(rwLesson);
    }
}