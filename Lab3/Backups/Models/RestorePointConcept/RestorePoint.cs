using Backups.Exceptions;

namespace Backups.Models;

public class RestorePoint : IRestorePoint, IComparable<RestorePoint>
{
    private List<IStorage> _storages = new ();

    public RestorePoint(List<IStorage> storages, string name, Guid id)
    {
        if (string.IsNullOrEmpty(name))
            throw BackupException.RestorePointException("Name is null");
        if (storages.Count == 0)
            throw BackupException.RestorePointException("Too few files to save");

        _storages = storages;
        CreationDate = DateTime.Now;
        Name = name + $"_{id}";
    }

    public string Name { get; }
    public DateTime CreationDate { get; }
    public IReadOnlyCollection<IStorage> Storages => _storages.AsReadOnly();

    public void AddStorage(IStorage storage) => _storages.Add(storage);

    public void DeleteStorages(IStorage storage) => _storages.Remove(storage);

    public bool Equals(IRestorePoint? other)
    {
        if (ReferenceEquals(null, other)) return false;
        return Equals((object)other);
    }

    public int CompareTo(IRestorePoint? other)
    {
        if (ReferenceEquals(null, other)) return 1;
        return other.CreationDate.CompareTo(CreationDate);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((RestorePoint)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, CreationDate);
    }

    public int CompareTo(RestorePoint? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return CreationDate.CompareTo(other.CreationDate);
    }

    protected bool Equals(RestorePoint? other)
    {
        return Name == other?.Name && CreationDate.Equals(other.CreationDate);
    }
}