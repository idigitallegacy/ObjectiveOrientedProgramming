using Messenger.DataAccess.Messages.Objects;
using Messenger.Domains.Exceptions;
using Messenger.Domains.Resources;

namespace Messenger.DataAccess.Resources.Objects;

public abstract class Resource : IResource
{
    protected Resource(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            ResourceException.InputEmpty();
        Name = name;
        InternalMessages =  new List<IMessage>();
    }
    public string Name { get; }
    public IReadOnlyCollection<IMessage> Messages => InternalMessages.AsReadOnly();
    protected List<IMessage> InternalMessages { get; set; }

    public abstract void AddMessage(IMessage message);

    public virtual bool RemoveMessage(IMessage message)
    {
        return InternalMessages.Remove(message);
    }
    
    public virtual ResourceSerializable AsSerializable()
    {
        return new ResourceSerializable(Name, Messages.Select(message => message.AsSerializable()).ToList());
    }

    public bool Equals(Resource? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name.Equals(other.Name);
    }

    public bool Equals(IResource? other)
    {
        if (other is not Resource)
            return false;
        return Equals((Resource)other);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Resource)obj);
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}