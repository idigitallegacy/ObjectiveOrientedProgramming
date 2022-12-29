namespace Messenger.DataAccess.Employees;

public static class EmployeeValidator
{
    public static void ValidateChain(Employee sourceEmployee, Employee? targetEmployee)
    {
        if (targetEmployee is null)
            return;
        if (sourceEmployee.Ordinates.Contains(targetEmployee) | targetEmployee.Ordinates.Contains(sourceEmployee))
            throw new Exception(); // todo
        if (RecursiveOrdinatory(sourceEmployee, targetEmployee))
            throw new Exception(); // todo
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