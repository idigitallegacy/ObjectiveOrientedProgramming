using Backups.Models;
using Backups.Services;

namespace Backups.Algorithms;

public class SplitStorage : IStorageAlgorithm
{
    public List<IStorage> WriteData(IBackupTask backupTask, Guid id, ICompressor compressor)
    {
        var storages = new List<IStorage>();

        compressor.Clear();
        backupTask.BackupObjects.ToList().ForEach(jobObject =>
        {
            compressor.AddFile(jobObject);
            var storage = new Storage(jobObject.FileName + $"{id}.storage", compressor);
            storage.AddBackupObject(jobObject);
            storages.Add(storage);
        });

        return storages;
    }
}