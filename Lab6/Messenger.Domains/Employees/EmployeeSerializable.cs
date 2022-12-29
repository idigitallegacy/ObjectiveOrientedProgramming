using Messenger.Domains.Accounts;

namespace Messenger.Domains.Employees;

[Serializable]
public class EmployeeSerializable
{
    public EmployeeSerializable(AccountSerializable? account, EmployeeSerializable? director = null)
    {
        Director = director;
        Ordinates = new List<EmployeeSerializable>();
        AccountDetails = account?.AccountDetails;
    }

    public EmployeeSerializable? Director { get; set; }
    public List<EmployeeSerializable> Ordinates { get; set; }
    public AccountDetailsSerializable? AccountDetails { get; set; }
}