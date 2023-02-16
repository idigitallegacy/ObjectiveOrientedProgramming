using Messenger.DataAccess.Accounts;

namespace Messenger.DataAccess.Employees;

public static class EmployeeGenerator
{
    public static Employee Generate(Account account)
    {
        return new Employee(account);
    }
}