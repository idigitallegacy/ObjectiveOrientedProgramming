using Backups.Models;

namespace Backups.Services;

public interface IRepository
{
    public string? FullPathRepository { get; }
    public DirectoryInfo? DirectoryInfo { get; }
    public void Save(IBackupTask backupTask);
    public void Accept(IVisitor visitor);
}