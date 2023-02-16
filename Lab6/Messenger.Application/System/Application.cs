using Messenger.Application.Common.Reports;
using Messenger.Application.System.Commands;
using Messenger.DataAccess.Accounts;
using Messenger.DataAccess.Employees;
using Messenger.DataAccess.Messages.Generators;
using Messenger.DataAccess.Messages.Objects;
using Messenger.DataAccess.Reports;
using Messenger.DataAccess.Resources.Generators;
using Messenger.DataAccess.Resources.Objects;
using Messenger.Domains.Messages;
using Messenger.Domains.Times;

namespace Messenger.Application.System;

public class Application
{
    private List<Employee> _employees = new List<Employee>();
    private List<IResource> _resources = new List<IResource>();
    private ReportAgregator _reportAgregator = ReportAgregator.Instance;
    private TimeFlow _time = TimeFlow.Instance;

    public Employee RegisterEmployee(CreateEmployeeCommand command)
    {
        Account account = AccountGenerator.Generate(command.Login, command.Password);
        Employee employee = EmployeeGenerator.Generate(account);
        _employees.Add(employee);
        _reportAgregator.AddObservable(employee);
        return employee;
    }

    public Employee? AuthorizeEmployee(AuthorizeEmployeeCommand command)
    {
        return _employees.Find(employee =>
            employee.AccountDetails.Login.Equals(command.Login) &&
            employee.AccountDetails.Password.Equals(command.Password));
    }

    public EMail RegisterEmailResource(CreateResourceCommand command)
    {
        EMail resource = ResourceGenerator.GenerateEmail(command.Name);
        _resources.Add(resource);
        return resource;
    }
    
    public Phone RegisterPhoneResource(CreateResourceCommand command)
    {
        Phone resource = ResourceGenerator.GeneratePhone(command.Name);
        _resources.Add(resource);
        return resource;
    }
    
    public Social RegisterSocialResource(CreateResourceCommand command)
    {
        Social resource = ResourceGenerator.GenerateSocial(command.Name);
        _resources.Add(resource);
        return resource;
    }

    public void AddOrdinate(Employee mainEmployee, Employee ordinate)
    {
        if (ordinate.Director is not null)
            throw Domains.Exceptions.ApplicationException.OrdinateHasDirector();
        mainEmployee.AddOrdinate(ordinate);
        ordinate.UpdateDirector(mainEmployee);
    }

    public void AddControlledResource(Employee employee, IResource resource)
    {
        employee.AddResource(resource);
    }

    public List<IMessage> ReadNewMessages(Employee employee)
    {
        List<IMessage> messages = employee.AvailableResources.SelectMany(resource => resource.Messages)
            .Where(message => message.State == MessageState.New).ToList();
        for (int i = 0; i < messages.Count; i++)
            _reportAgregator.IncreaseViewed(employee);
        return messages;
    }

    public IMessage GetMessageById(string id)
    {
        return _resources.SelectMany(resource => resource.Messages).First(message => message.Id.Equals(Guid.Parse(id)));
    }

    public EMailMessage? WriteEmailReply(WriteReplyCommand command)
    {
        if (command.Employee is null)
            return null;
        MessageOptions options = MessageOptionsGenerator.Generate(command.Resource, command.Subject);
        EMailMessage message = MessageGenerator.GenerateEMailMessage(options, command.Contents);
        GetMessageById(command.MessageId).Reply(message);
        _reportAgregator.IncreaseReplied(command.Employee);
        return message;
    }
    
    public Sms? WritePhoneReply(WriteReplyCommand command)
    {
        if (command.Employee is null)
            return null;
        MessageOptions options = MessageOptionsGenerator.Generate(command.Resource, command.Subject);
        Sms message = MessageGenerator.GenerateSms(options, command.Contents);
        GetMessageById(command.MessageId).Reply(message);
        _reportAgregator.IncreaseReplied(command.Employee);
        return message;
    }
    
    public SocialFeedback? WriteSocialReply(WriteReplyCommand command)
    {
        if (command.Employee is null)
            return null;
        MessageOptions options = MessageOptionsGenerator.Generate(command.Resource, command.Subject);
        SocialFeedback message = MessageGenerator.GenerateSocialFeedback(options, command.Contents);
        GetMessageById(command.MessageId).Reply(message);
        _reportAgregator.IncreaseReplied(command.Employee);
        return message;
    }

    public List<Report> GetReports(Employee employee)
    {
        if (employee.Ordinates.Count == 0)
            throw Domains.Exceptions.ApplicationException.OrdinateIsNotDirector();
        return _reportAgregator.GetReports(employee);
    }
}