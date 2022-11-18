using Backups.Algorithms;
using Backups.Services;

namespace Backups.Models;

public interface IBackupTask
{
    public string Name { get; }
    public IStorageAlgorithm StorageAlgorithm { get; }
    public ICompressor Compressor { get; }
    public IRepository Repository { get; }
    public IReadOnlyCollection<IBackupObject> BackupObjects { get; }
    public IReadOnlyCollection<IRestorePoint> RestorePoints { get; }

    public void AddObject(IBackupObject backupObject);
    public void DeleteBackupObject(IBackupObject backupObject);
    public void CreateRestorePoint();
    public void DeleteRestorePoint(IRestorePoint restorePoint);
    public void Accept(IVisitor visitor);
}