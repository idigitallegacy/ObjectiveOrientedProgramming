using Backups.Models;
using Backups.Services;

namespace Backups.Algorithms;

public class SingleStorage : IStorageAlgorithm
{
    public List<IStorage> WriteData(IBackupTask backupTask, Guid id, ICompressor compressor)
    {
        compressor.Clear();
        backupTask.BackupObjects.ToList().ForEach(jobObject => compressor.AddFile(jobObject));

        var storages = new List<IStorage>();
        var storage = new Storage($"Archive_{id}.zip", compressor);
        backupTask.BackupObjects.ToList().ForEach(jobObject => storage.AddBackupObject(jobObject));
        storages.Add(storage);

        return storages;
    }
}