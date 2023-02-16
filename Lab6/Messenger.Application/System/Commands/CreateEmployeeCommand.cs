using Messenger.Config;

namespace Messenger.Application.System.Commands;

public class CreateEmployeeCommand
{
    public string Login { get; set; } = DefaultValues.Account.DefaultLogin;
    public string Password { get; set; } = DefaultValues.Account.DefaultPassword;
}