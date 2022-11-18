namespace Backups.Models;

public interface IRestorePoint : IEquatable<IRestorePoint>, IComparable<IRestorePoint>
{
    string Name { get; }
    DateTime CreationDate { get; }
    IReadOnlyCollection<IStorage> Storages { get; }

    public void AddStorage(IStorage storage);

    public void DeleteStorages(IStorage storage);
}