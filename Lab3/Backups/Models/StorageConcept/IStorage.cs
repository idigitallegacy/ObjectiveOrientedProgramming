using Backups.Algorithms;

namespace Backups.Models;

public interface IStorage
{
    string Path { get; }
    ICompressor Compressor { get; }
    IReadOnlyCollection<IBackupObject> BackupObjects { get; }

    void AddBackupObject(IBackupObject backupObject);
}