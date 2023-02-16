using Backups.Algorithms;
using Backups.Extra.ExceptionsExtension;
using Backups.Extra.LoggerConcept;
using Backups.Extra.NewBackupTask;
using Backups.Models;
using Backups.Services;

namespace Backups.Extra.UpgradedBackupConcept;

public class UpgradedBackupTask : IUpgradedBackupTask
{
    private ILogger? _logger;
    private IBackupTask _backupTask;

    public UpgradedBackupTask(IBackupTask backupTask, ILogger? logger = null)
    {
        _backupTask = backupTask;
        _logger = logger;
    }

    public string Name { get => _backupTask.Name; }
    public IStorageAlgorithm StorageAlgorithm { get => _backupTask.StorageAlgorithm; }
    public ICompressor Compressor { get => _backupTask.Compressor; }
    public IRepository Repository { get => _backupTask.Repository; }
    public IReadOnlyCollection<IBackupObject> BackupObjects { get => _backupTask.BackupObjects; }
    public IReadOnlyCollection<IRestorePoint> RestorePoints { get => _backupTask.RestorePoints; }

    public void UpdateLogger(ILogger? logger)
    {
        Stream? oldStream = _logger?.Log($"Logger has changed.");
        if (oldStream is not null)
            logger?.CopyStreamBytes(oldStream);
        _logger = logger;
    }

    public void AddObject(IBackupObject backupObject)
    {
        _backupTask.AddObject(backupObject);
        _logger?.Log($"Added backup object: {backupObject.FullPath}");
    }

    public void DeleteBackupObject(IBackupObject backupObject)
    {
        _backupTask.DeleteBackupObject(backupObject);
        _logger?.Log($"Deleted backup object: {backupObject.FullPath}");
    }

    public void CreateRestorePoint()
    {
        _backupTask.CreateRestorePoint();
        _logger?.Log($"Created restore point: {_backupTask.RestorePoints.Last().Name}");
    }

    public void DeleteRestorePoint(IRestorePoint restorePoint)
    {
        _backupTask.DeleteRestorePoint(restorePoint);
        _logger?.Log($"Deleted restore point: {restorePoint.Name}");
    }

    public void AddRestorePoint(IRestorePoint restorePoint)
    {
        _backupTask.AddRestorePoint(restorePoint);
        _logger?.Log($"Added restore point: {restorePoint.Name}");
    }

    public void Accept(IVisitor visitor)
    {
        _logger?.Log($"Accepted visitor");
        _backupTask.Accept(visitor);
    }

    public string SaveState()
    {
        if (_backupTask is BackupTaskWrapper)
        {
            try
            {
                string json = ((BackupTaskWrapper)_backupTask).SaveState();
                _logger?.Log("Serialized successfully.");
                return json;
            }
            catch (Exception e)
            {
                _logger?.Log($"Error serializing BackupTask state. Exception message: {e.Message}");
                throw;
            }
        }

        throw BackupTaskException.UnableToSaveState_InvalidType();
    }

    public void RestoreState(string json)
    {
        if (_backupTask is BackupTaskWrapper)
        {
            try
            {
                ((BackupTaskWrapper)_backupTask).RestoreState(json);
                _logger?.Log($"Backup task {Name} restored successfully");
                return;
            }
            catch (Exception e)
            {
                _logger?.Log($"Error serializing BackupTask state. Exception message: {e.Message}");
                throw;
            }
        }

        throw BackupTaskException.UnableToLoadState_InvalidType();
    }

    public void NotifyLogger(string message)
    {
        _logger?.Log(message);
    }
}