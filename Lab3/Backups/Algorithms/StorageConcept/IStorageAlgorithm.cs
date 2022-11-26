using Backups.Algorithms;
using Backups.Models;

namespace Backups.Services;

public interface IStorageAlgorithm
{
    List<IStorage> WriteData(IBackupTask backupTask, Guid id, ICompressor compressor);
}