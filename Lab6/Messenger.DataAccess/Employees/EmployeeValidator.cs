using Messenger.Domains.Exceptions;

namespace Messenger.DataAccess.Employees;

public static class EmployeeValidator
{
    public static void ValidateChain(Employee sourceEmployee, Employee? targetEmployee)
    {
        if (targetEmployee is null)
            return;
        if (sourceEmployee.Ordinates.Contains(targetEmployee) |
            targetEmployee.Ordinates.Contains(sourceEmployee) |
            RecursiveOrdinatory(sourceEmployee, targetEmployee))
            throw EmployeeException.RecursiveOrdinatory(); 
    } 
    
    private static bool RecursiveOrdinatory(Employee sourceEmployee, Employee targetEmployee)
    {
        if (sourceEmployee.Equals(targetEmployee))
            return true;
        if (sourceEmployee.Ordinates.Count == 0)
            return false;

        return sourceEmployee.Ordinates.Any(employee => RecursiveOrdinatory(employee, targetEmployee));
    }
}