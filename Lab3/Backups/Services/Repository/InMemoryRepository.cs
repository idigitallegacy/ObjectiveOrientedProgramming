using Backups.Services;

namespace Backups.Models;

public class InMemoryRepository : IRepository
{
    public string? FullPathRepository { get; } = null;
    public DirectoryInfo? DirectoryInfo { get; } = null;

    public void Save(IBackupTask backupTask) =>
        backupTask.RestorePoints.Last().Storages.ToList()
            .ForEach(storage => storage.Compressor.Compress(null));

    public void Accept(IVisitor visitor) => visitor.Visit(this);
}