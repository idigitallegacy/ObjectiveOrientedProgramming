using Isu.Exceptions;
using Isu.Models;
using Isu.Services;

namespace Isu.Test;
using Xunit;

public class IsuServicetest
{
    [Fact]
    public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
    {
        var service = new IsuService();
        var groupName = new GroupName("M3206"); // M3206 ~ M32061
        var group = service.AddGroup(groupName);
        var student = service.AddStudent(group, "Michael Makarov");

        Assert.True(student.Group == group.GroupName, "student.Group should be equal to the service stored one");
        Assert.True(service.FindGroup(groupName)?.Students.Contains(student), "Group should contain student.");
        Assert.True(service.FindGroup(groupName)?.Students.Contains(student), "Service should contain student.");

        group = service.FindGroup(groupName);
        Assert.True(group?.Students.Contains(student), "Group should contain student.");
    }

    [Fact]
    public void ReachMaxStudentPerGroup_ThrowException()
    {
        var service = new IsuService();
        var groupName = new GroupName("M3206");
        var group = service.AddGroup(groupName);
        for (var i = 0; i < 30; i++) // 30 is a MaxStudentAmount private constant at Isu.Entities.Group
            service.AddStudent(group, Convert.ToString(i));
        var ex = Assert.Throws<IsuException>(() => service.AddStudent(group, "Michael Makarov"));

        Assert.True(ex.Message == "Unable to add student: group is full.");
    }

    [Fact]
    public void CreateGroupWithInvalidName_ThrowException()
    {
        var ex = Assert.Throws<IsuException>(() => new GroupName("M32061"));
        Assert.True(ex.Message == "Invalid group name: it must contain 5 characters.");
        ex = Assert.Throws<IsuException>(() => new GroupName("N3206"));
        Assert.True(ex.Message == "Invalid group name: must start with M letter");
        ex = Assert.Throws<IsuException>(() => new GroupName("М3206")); // russian M
        Assert.True(ex.Message == "Invalid group name: must start with M letter");
        ex = Assert.Throws<IsuException>(() => new GroupName("M3006"));
        Assert.True(ex.Message == "Invalid course number 0, it must be between 1 and 4.");

        // Duplicate group
        var service = new IsuService();
        service.AddGroup(new GroupName("M3206"));
        ex = Assert.Throws<IsuException>(() => service.AddGroup(new GroupName("M3206")));
        Assert.True(ex.Message == "Unable to add group: it's already exists.");
    }

    [Fact]
    public void TransferStudentToAnotherGroup_GroupChanged()
    {
        var service = new IsuService();
        var groupName1 = new GroupName("M3206");
        var groupName2 = new GroupName("M3306");
        var group1 = service.AddGroup(groupName1);
        var group2 = service.AddGroup(groupName2);
        var student = service.AddStudent(group1, "Michael Makarov");
        service.ChangeStudentGroup(student, group2);
        student = service.GetStudent(student.Id);

        Assert.True(service.FindGroup(groupName2)?.Students.Contains(student), "New group should contain student");
        Assert.False(service.FindGroup(groupName1)?.Students.Contains(student), "Old group shouldn't contain student");
        Assert.True(service.FindGroup(groupName2) !.GroupName.Name == service.FindStudent(student.Id) !.Group.Name, "Student should contain new group");
    }
}