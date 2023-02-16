namespace Backups.Models;

public interface IBackupObject : IEquatable<IBackupObject>
{
    string FullPath { get; }
    string FileName { get; }
    bool Virtual { get; }
    MemoryStream Contents { get; set; }
}