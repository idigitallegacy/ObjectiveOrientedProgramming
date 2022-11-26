using Backups.Models;
using Backups.Services;

namespace Backups.Algorithms;

public interface ICompressor
{
    void AddFile(IBackupObject backupObject);
    void Compress(string? outputPath);
    void Clear();
    void Accept(IVisitor visitor);
}