namespace Backups.Models;

public interface IBackup
{
    public bool HasNext { get; }
    public IReadOnlyCollection<IRestorePoint> GetAll();
    public IRestorePoint GetNext();
    public void Remove(IRestorePoint restorePoint);
    public void Add(IRestorePoint restorePoint);
}