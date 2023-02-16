using Messenger.DataAccess.Messages.Objects;
using Messenger.Domains.Resources;

namespace Messenger.DataAccess.Resources.Objects;

public interface IResource : IEquatable<IResource>
{
    string Name { get; }
    IReadOnlyCollection<IMessage> Messages { get; }
    void AddMessage(IMessage message);
    bool RemoveMessage(IMessage message);
    ResourceSerializable AsSerializable();
}