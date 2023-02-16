using Messenger.DataAccess.Accounts;
using Messenger.DataAccess.Resources.Objects;

namespace Messenger.DataAccess.Employees;

public class Employee
{
    private List<Employee> _ordinates = new List<Employee>();
    private Account _accountData;
    private List<IResource> _controlledResources = new List<IResource>();

    public Employee(Account accountData)
    {
        _accountData = accountData;
    }
    
    public Employee? Director { get; private set; }
    public IReadOnlyCollection<Employee> Ordinates => _ordinates.AsReadOnly();
    public AccountDetails AccountDetails => _accountData.AccountDetails;
    public IReadOnlyCollection<IResource> AvailableResources => _controlledResources.AsReadOnly();

    public void UpdateDirector(Employee? newDirector)
    {
        EmployeeValidator.ValidateChain(this, newDirector);
        Director = newDirector;
    }

    public void AddOrdinate(Employee ordinate)
    {
        if (_ordinates.Contains(ordinate))
            return;
        EmployeeValidator.ValidateChain(this, ordinate);
        _ordinates.Add(ordinate);
    }

    public bool RemoveOrdinate(Employee ordinate)
    {
        return _ordinates.Remove(ordinate);
    }

    public void AddResource(IResource resource)
    {
        _controlledResources.Add(resource);
    }
}