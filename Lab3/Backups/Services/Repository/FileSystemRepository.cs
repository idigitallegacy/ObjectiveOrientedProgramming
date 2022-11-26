using Backups.Exceptions;
using Backups.Services;

namespace Backups.Models;

public class FileSystemRepository : IRepository
{
    public FileSystemRepository(string path)
    {
        if (string.IsNullOrEmpty(path))
            throw BackupException.RepositoryException("Path is null");

        FullPathRepository = path;
        DirectoryInfo = Directory.CreateDirectory(FullPathRepository);
    }

    public string? FullPathRepository { get; }
    public DirectoryInfo? DirectoryInfo { get; }

    public void Save(IBackupTask backupTask)
    {
        if (DirectoryInfo is null)
            throw BackupException.RepositoryException("DirectoryInfo at FileSystemRepository must be not null");

        string rpPath = Path.Combine(DirectoryInfo.FullName, backupTask.Name, backupTask.RestorePoints.Last().Name);
        var rpDirectory = new DirectoryInfo(rpPath);

        if (!rpDirectory.Exists)
            rpDirectory.Create();

        backupTask.RestorePoints.Last().Storages.ToList()
            .ForEach(storage => storage.Compressor.Compress(Path.Combine(rpPath, storage.Path)));
    }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}