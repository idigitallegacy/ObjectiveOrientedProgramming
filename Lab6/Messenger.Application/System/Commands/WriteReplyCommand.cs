using Messenger.Config;
using Messenger.DataAccess.Employees;
using Messenger.DataAccess.Resources.Objects;

namespace Messenger.Application.System.Commands;

public class WriteReplyCommand
{
    public Employee? Employee { get; set; }
    public string MessageId { get; set; } = DefaultValues.Message.DefaultId;
    public IResource? Resource { get; set; }
    public string Subject = DefaultValues.Message.DefaultSubject;
    public string Contents = DefaultValues.Message.DefaultContents;
}