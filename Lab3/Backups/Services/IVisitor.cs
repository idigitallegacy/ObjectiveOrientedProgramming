using Backups.Algorithms;
using Backups.Models;

namespace Backups.Services;

public interface IVisitor
{
    void Visit(ICompressor compressor);
    void Visit(IBackupTask backupTask);
    void Visit(IRepository repository);
}