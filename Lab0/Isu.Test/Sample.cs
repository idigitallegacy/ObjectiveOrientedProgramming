using Isu.Entities;
using Isu.Models;
using Xunit;

namespace Isu.Test;

// A bit modified IsuService class
public class SampleIsu
{
    private List<Group> _groups = new ();

    public Group AddGroup(GroupName name)
    {
        var group = new Group(name);
        _groups.Add(group);
        return group;
    }
}

public class Sample
{
    [Fact]
    public void GroupOuterScopeChanging_WithoutNewConstruction_Test()
    {
        var service = new SampleIsu();
        var group = service.AddGroup(new GroupName("M3206"));
        group.AddStudent(new Student("Michael", group, 1));
        /*
         * Place a breakpoint here and look at service._groups._students. It contains 1 student, but
         * I didn't add it via service (encapsulation failed: I succeed to change a private field element via it's method)
         */
        return;
    }
}