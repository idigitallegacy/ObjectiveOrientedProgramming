using Backups.Algorithms;
using Backups.Exceptions;
using Backups.Services;

namespace Backups.Models;

public class BackupTask : IBackupTask
{
    private Guid _id = Guid.NewGuid();
    private List<IBackupObject> _backupObjects = new ();
    private IBackup _backupHistory = new Backup();

    public BackupTask(string name, IRepository repository, IStorageAlgorithm storageAlgorithm, ICompressor compressor)
    {
        if (string.IsNullOrEmpty(name))
            throw BackupException.BackupTaskException("The name is empty");

        Name = name;
        StorageAlgorithm = storageAlgorithm;
        Repository = repository;
        Compressor = compressor;
    }

    public string Name { get; }
    public IStorageAlgorithm StorageAlgorithm { get; }
    public ICompressor Compressor { get; }
    public IRepository Repository { get; }
    public IReadOnlyCollection<IBackupObject> BackupObjects => _backupObjects.AsReadOnly();
    public IReadOnlyCollection<IRestorePoint> RestorePoints => _backupHistory.GetAll();

    public void AddObject(IBackupObject backupObject) => _backupObjects.Add(backupObject);

    public void DeleteBackupObject(IBackupObject backupObject)
    {
        if (_backupObjects.Find(needleObject => needleObject.Equals(backupObject)) is null)
            throw BackupException.BackupTaskException("Backup object doesn't exists");
        _backupObjects.Remove(backupObject);
    }

    public void CreateRestorePoint()
    {
        List<IStorage> storages = StorageAlgorithm.WriteData(this, _id, Compressor);
        var restorePoint = new RestorePoint(storages, "RestorePoint", _id);

        _id = Guid.NewGuid();
        _backupHistory.Add(restorePoint);
        Repository.Save(this);
    }

    public void AddRestorePoint(IRestorePoint restorePoint) => _backupHistory.Add(restorePoint);

    public void Accept(IVisitor visitor) => visitor.Visit(this);

    public void DeleteRestorePoint(IRestorePoint restorePoint) => _backupHistory.Remove(restorePoint);
}