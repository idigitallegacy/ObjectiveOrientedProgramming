using Isu.Extra.Models;
using Isu.Models;

namespace Isu.Extra.Extensions;

public static class GroupNameExtension
{
    private static int _facultyIdPosition = 0;
    public static FacultyId GetFacultyId(this GroupName groupName)
    {
        char facultyLetter = groupName.FacultyId[_facultyIdPosition];
        return new FacultyId(facultyLetter);
    }
}