using Backups.Algorithms;
using Backups.Exceptions;

namespace Backups.Models;

public class Storage : IStorage
{
    private List<IBackupObject> _backupObjects = new ();

    public Storage(string path, ICompressor compressor)
    {
        if (string.IsNullOrEmpty(path))
            throw BackupException.StorageException("Path is null");

        Path = path;
        Compressor = compressor;
    }

    public string Path { get; }
    public ICompressor Compressor { get; }
    public IReadOnlyCollection<IBackupObject> BackupObjects => _backupObjects.AsReadOnly();

    public void AddBackupObject(IBackupObject backupObject) => _backupObjects.Add(backupObject);
    public void RemoveBackupObject(IBackupObject backupObject) => _backupObjects.Remove(backupObject);
}