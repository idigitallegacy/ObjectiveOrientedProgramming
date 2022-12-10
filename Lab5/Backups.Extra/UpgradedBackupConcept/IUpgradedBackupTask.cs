using Backups.Models;

namespace Backups.Extra.UpgradedBackupConcept;

public interface IUpgradedBackupTask : IBackupTask
{
    string SaveState();
    void RestoreState(string json);
    void NotifyLogger(string message);
}