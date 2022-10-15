using Isu.Entities;
using Isu.Models;
using Isu.Services;

namespace Isu.Extra.Services;

public class IsuExtraAdapter : IIsuService
{
    public IsuExtraAdapter(IIsuService service)
    {
        IsuService = service;
    }

    protected IsuExtraAdapter()
    {
        IsuService = new IsuService();
    }

    protected IIsuService IsuService { get; }

    public Group AddGroup(GroupName name)
    {
        return IsuService.AddGroup(name);
    }

    public Student AddStudent(Group group, string name)
    {
        return IsuService.AddStudent(group, name);
    }

    public Student GetStudent(int id)
    {
        return IsuService.GetStudent(id);
    }

    public Student? FindStudent(int id)
    {
        return IsuService.FindStudent(id);
    }

    public List<Student> FindStudents(GroupName groupName)
    {
        return IsuService.FindStudents(groupName);
    }

    public List<Student> FindStudents(CourseNumber courseNumber)
    {
        return IsuService.FindStudents(courseNumber);
    }

    public Group? FindGroup(GroupName groupName)
    {
        return IsuService.FindGroup(groupName);
    }

    public List<Group> FindGroups(CourseNumber courseNumber)
    {
        return IsuService.FindGroups(courseNumber);
    }

    public void ChangeStudentGroup(Student student, Group newGroup)
    {
        IsuService.ChangeStudentGroup(student, newGroup);
    }
}