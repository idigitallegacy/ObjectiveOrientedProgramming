using Isu.Exceptions;
using Isu.Models;
using Isu.Services;

namespace Isu.Test;
using Xunit;

public class IsuServiceTest
{
    [Fact]
    public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
    {
        // Allocate
        var service = new IsuService();
        var groupName = new GroupName("M3206"); // M3206 ~ M32061

        // Act
        var group = service.AddGroup(groupName);
        var student = service.AddStudent(group, "Michael Makarov");
        var foundGroup = service.FindGroup(groupName);

        // Assert
        Assert.True(
            student.Group == group,
            "student.Group should be equal to the service stored one");
        Assert.True(
            service.FindGroup(groupName)?.Students.Contains(student),
            "Group should contain student.");
        Assert.True(
            service.FindGroup(groupName)?.Students.Contains(student),
            "Service should contain student.");
        Assert.True(
            foundGroup?.Students.Contains(student),
            "Group should contain student.");
    }

    [Fact]
    public void ReachMaxStudentPerGroup_ThrowException()
    {
        // Allocate
        var service = new IsuService();
        var groupName = new GroupName("M3206");

        // Act
        var group = service.AddGroup(groupName);
        for (var i = 0; i < 30; i++) // 30 is a MaxStudentAmount private constant at Isu.Entities.Group
            service.AddStudent(group, Convert.ToString(i));

        // Assert
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
    }

    [Fact]
    public void DuplicateGroup_ThrowException()
    {
        // Allocate
        var service = new IsuService();
        var groupName1 = new GroupName("M3206");
        var groupName2 = new GroupName("M3206");

        // Act
        service.AddGroup(groupName1);

        // Assert
        var ex = Assert.Throws<IsuException>(() => service.AddGroup(groupName2));
        Assert.True(ex.Message == "Unable to add group: it's already exists.");
    }

    [Fact]
    public void TransferStudentToAnotherGroup_GroupChanged()
    {
        // Allocate
        var service = new IsuService();
        var groupName1 = new GroupName("M3206");
        var groupName2 = new GroupName("M3306");

        // Act
        var group1 = service.AddGroup(groupName1);
        var group2 = service.AddGroup(groupName2);
        var student = service.AddStudent(group1, "Michael Makarov");
        service.ChangeStudentGroup(student, group2);
        student = service.GetStudent(student.Id);

        // Assert
        Assert.True(
            service.FindGroup(groupName2)?.Students.Contains(student),
            "New group should contain student");
        Assert.False(
            service.FindGroup(groupName1)?.Students.Contains(student),
            "Old group shouldn't contain student");
        Assert.True(
            service.FindGroup(groupName2) ! == service.FindStudent(student.Id) !.Group,
            "Student should contain new group");
    }
}