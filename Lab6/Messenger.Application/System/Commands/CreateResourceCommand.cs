using Messenger.Config;

namespace Messenger.Application.System.Commands;

public class CreateResourceCommand
{
    public string Name { get; set; } = DefaultValues.Resource.DefaultName;
}