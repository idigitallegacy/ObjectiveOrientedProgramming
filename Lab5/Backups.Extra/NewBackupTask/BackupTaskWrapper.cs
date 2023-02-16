using System.Text.Json;
using System.Xml.Serialization;
using Backups.Algorithms;
using Backups.Extra.ExceptionsExtension;
using Backups.Extra.LoggerConcept;
using Backups.Extra.UpgradedBackupConcept;
using Backups.Models;
using Backups.Services;
using ConsoleApp1.Algorithms.Compression;

namespace Backups.Extra.NewBackupTask;

public class BackupTaskWrapper : IBackupTask
{
    private BackupTask _backupTask;

    public BackupTaskWrapper()
    {
        _backupTask = new BackupTaskBuilder()
            .SetName("1")
            .SetRepository(new InMemoryRepository())
            .SetStorageAlgorithm(new SingleStorage())
            .SetCompressor(new InMemoryCompressor())
            .Build();
        Name = _backupTask.Name;
        StorageAlgorithm = _backupTask.StorageAlgorithm;
        Compressor = _backupTask.Compressor;
        Repository = _backupTask.Repository;
    }

    public BackupTaskWrapper(BackupTask backupTask)
    {
        _backupTask = backupTask;
        Name = backupTask.Name;
        StorageAlgorithm = backupTask.StorageAlgorithm;
        Compressor = backupTask.Compressor;
        Repository = backupTask.Repository;
    }

    public enum LimitCombinationType
    {
        None = 0,
        And,
        Or,
        OnlyAmount,
        OnlyDate,
    }

    public string Name { get; private set; }
    public IStorageAlgorithm StorageAlgorithm { get; private set; }
    public ICompressor Compressor { get; private set; }
    public IRepository Repository { get; private set; }
    public IReadOnlyCollection<IBackupObject> BackupObjects { get => _backupTask.BackupObjects; }
    public IReadOnlyCollection<IRestorePoint> RestorePoints { get => _backupTask.RestorePoints; }

    public uint? RestorePointAmountLimit { get; set; } = null;
    public DateTime? RestorePointDateLimit { get; set; } = null;
    public LimitCombinationType LimitCombination { get; set; } = LimitCombinationType.None;
    public UpgradedBackupTask? Proxy { get; set; }

    public void AddObject(IBackupObject backupObject)
    {
        _backupTask.AddObject(backupObject);
        ValidateLimits();
    }

    public void DeleteBackupObject(IBackupObject backupObject)
    {
        _backupTask.DeleteBackupObject(backupObject);
        ValidateLimits();
    }

    public void CreateRestorePoint()
    {
        _backupTask.CreateRestorePoint();
        ValidateLimits();
    }

    public void DeleteRestorePoint(IRestorePoint restorePoint)
    {
        _backupTask.DeleteRestorePoint(restorePoint);
        ValidateLimits();
    }

    public void AddRestorePoint(IRestorePoint restorePoint)
    {
        _backupTask.AddRestorePoint(restorePoint);
        ValidateLimits();
    }

    public void Accept(IVisitor visitor)
    {
        _backupTask.Accept(visitor);
        visitor.Visit(this);
        ValidateLimits();
    }

    public string SaveState()
    {
        ValidateLimits();
        string json = string.Empty;
        json = JsonSerializer.Serialize(new BackupStateSerializable(this));
        return json;
    }

    public void RestoreState(string json)
    {
        BackupStateSerializable? deserializedState = JsonSerializer.Deserialize<BackupStateSerializable>(json);

        _backupTask = new BackupTaskBuilder()
            .SetName(deserializedState?.Name ?? throw BackupTaskWrapperException.UnableToLoadState_InvalidName())
            .SetStorageAlgorithm(deserializedState?.StorageAlgorithm ?? throw BackupTaskWrapperException.UnableToLoadState_InvalidStorageAlgorithm())
            .SetRepository(deserializedState?.Repository ?? throw BackupTaskWrapperException.UnableToLoadState_InvalidRepository())
            .SetCompressor(deserializedState?.Compressor ?? throw BackupTaskWrapperException.UnableToLoadState_InvalidCompressor())
            .Build();

        Name = _backupTask.Name;
        StorageAlgorithm = _backupTask.StorageAlgorithm;
        Compressor = _backupTask.Compressor;
        Repository = _backupTask.Repository;
        deserializedState?.BackupObjects?.ToList().ForEach(obj => _backupTask.AddObject(obj));
        deserializedState?.RestorePoints?.ToList().ForEach(point => _backupTask.AddRestorePoint(point));
        ValidateLimits();
    }

    private IRestorePoint Merge(IRestorePoint oldRestorePoint1, IRestorePoint oldRestorePoint2)
    {
        BackupTask backupTask = new BackupTaskBuilder()
            .SetName(_backupTask.Name)
            .SetCompressor(_backupTask.Compressor)
            .SetRepository(_backupTask.Repository)
            .SetStorageAlgorithm(_backupTask.StorageAlgorithm)
            .Build();

        List<IBackupObject> rp2BackupObjects = oldRestorePoint2.Storages
            .SelectMany(storage2 => storage2.BackupObjects)
            .ToList();

        oldRestorePoint1.Storages
            .SelectMany(storage => storage.BackupObjects)
            .Where(backupObject => !rp2BackupObjects.Contains(backupObject))
            .ToList()
            .ForEach(oldObject => backupTask.AddObject(oldObject));

        rp2BackupObjects.ForEach(backupObject => backupTask.AddObject(backupObject));
        backupTask.CreateRestorePoint();

        IRestorePoint mergedRestorePoint = backupTask.RestorePoints.First();
        Proxy?.NotifyLogger($"Restore points {oldRestorePoint1.Name} and {oldRestorePoint2.Name} merged into {mergedRestorePoint.Name}");
        return mergedRestorePoint;
    }

    private void RemoveRestorePointsByDate()
    {
        List<IRestorePoint> restorePoints = _backupTask.RestorePoints.ToList();
        restorePoints = restorePoints.OrderBy(point => point.CreationDate).ToList();
        restorePoints.ForEach(point =>
        {
            if (restorePoints.Last().Equals(point) && point.CreationDate.CompareTo(RestorePointDateLimit) < 0)
            {
                _backupTask.DeleteRestorePoint(point);
            }
            else
            {
                if (point.CreationDate.CompareTo(RestorePointDateLimit) < 0)
                {
                    IRestorePoint nextRestorePoint = restorePoints[restorePoints.IndexOf(point) + 1];
                    IRestorePoint newRestorePoint = Merge(point, nextRestorePoint);
                    _backupTask.DeleteRestorePoint(point);
                    _backupTask.DeleteRestorePoint(nextRestorePoint);
                    _backupTask.AddRestorePoint(newRestorePoint);
                }
            }
        });
    }

    private void RemoveRestorePointsByAmount()
    {
        List<IRestorePoint> restorePoints = _backupTask.RestorePoints.ToList();
        restorePoints = restorePoints.OrderBy(point => point.CreationDate).ToList();
        while (_backupTask.RestorePoints.Count > RestorePointAmountLimit)
        {
            IRestorePoint oldRestorePoint = restorePoints.First();
            if (restorePoints.Last().Equals(oldRestorePoint))
            {
                _backupTask.DeleteRestorePoint(oldRestorePoint);
            }
            else
            {
                IRestorePoint nextRestorePoint = restorePoints[restorePoints.IndexOf(oldRestorePoint) + 1];
                IRestorePoint newRestorePoint = Merge(oldRestorePoint, nextRestorePoint);

                _backupTask.DeleteRestorePoint(oldRestorePoint);
                _backupTask.DeleteRestorePoint(nextRestorePoint);
                _backupTask.AddRestorePoint(newRestorePoint);
            }
        }
    }

    private void ValidateLimits()
    {
        switch (LimitCombination)
        {
            case LimitCombinationType.None: break;

            case LimitCombinationType.And:
            {
                _backupTask.RestorePoints.ToList().ForEach(point =>
                {
                    if (point.CreationDate.CompareTo(RestorePointDateLimit) < 0 && _backupTask.RestorePoints.Count > RestorePointAmountLimit)
                        _backupTask.DeleteRestorePoint(point);
                });
                Proxy?.NotifyLogger($"Cleaned up backups using rule 'Date and Amount'");
                break;
            }

            case LimitCombinationType.Or:
            {
                RemoveRestorePointsByDate();
                if (_backupTask.RestorePoints.Count > RestorePointAmountLimit)
                    RemoveRestorePointsByAmount();
                Proxy?.NotifyLogger($"Cleaned up backups using rule 'Date or Amount'");
                break;
            }

            case LimitCombinationType.OnlyAmount:
            {
                RemoveRestorePointsByAmount();
                Proxy?.NotifyLogger($"Cleaned up backups using rule 'only Amount'");
                break;
            }

            case LimitCombinationType.OnlyDate:
            {
                RemoveRestorePointsByDate();
                Proxy?.NotifyLogger($"Cleaned up backups using rule 'only Date'");
                break;
            }
        }
    }
}