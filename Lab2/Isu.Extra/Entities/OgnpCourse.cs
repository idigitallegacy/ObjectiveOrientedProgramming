using Isu.Entities;
using Isu.Extra.Builders;
using Isu.Extra.Composites;
using Isu.Extra.Models;
using Isu.Extra.Wrappers;
using Isu.Models;

namespace Isu.Extra.Entities;

public class OgnpCourse : IReadOnlyOgnpCourse
{
    private List<StudyStream> _streams;
    private List<Teacher> _teachers;
    private FacultyId _facultyId;

    public OgnpCourse(FacultyId facultyId, List<Teacher> teachers, List<StudyStream> streams)
    {
        if (teachers.Count == 0)
            throw new Exception(); // TODO
        if (streams.Count == 0)
            throw new Exception(); // TODO
        _streams = new List<StudyStream>(streams);
        _teachers = new List<Teacher>(teachers);
        _facultyId = facultyId;
    }

    public IReadOnlyCollection<IReadOnlyStudyStream> Streams => _streams;
    public IReadOnlyCollection<IReadOnlyTeacher> Teachers => _teachers;
    public FacultyId FacultyId => _facultyId;

    public void AddStudent(GroupName streamName, IReadOnlyExtendedStudent student)
    {
        GetStream(streamName).AddStudent(student);
    }

    public void RemoveStudent(IReadOnlyExtendedStudent student)
    {
        _streams
            .First(stream => stream.Group.Students.Contains(student))
            .RemoveStudent(student);
    }

    public IReadOnlyTeacher? FindTeacher(string name)
    {
        return _teachers.FirstOrDefault(teacher => teacher.Name == name);
    }

    public void AddLesson(StudyStream stream, Lesson lesson)
    {
        ValidateStreamLesson(stream, lesson);
        stream.AddLesson(lesson);
    }

    public void RemoveLesson(StudyStream stream, Lesson lesson)
    {
        stream.RemoveLesson(lesson);
    }

    private StudyStream GetStream(GroupName streamName)
    {
        return _streams.FirstOrDefault(stream => stream.Group.GroupName.Name == streamName.Name) ??
               throw new Exception(); // TODO
    }

    private void ValidateStreamLesson(StudyStream stream, Lesson lesson)
    {
        if (!_teachers.Contains(lesson.Teacher))
            throw new Exception(); // TODO
        if (stream.Schedule.TimeIsScheduled(lesson.DayOfWeek, lesson.StartTime, lesson.EndTime))
            throw new Exception(); // TODO
    }
}