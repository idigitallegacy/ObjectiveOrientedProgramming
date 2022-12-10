using Backups.Exceptions;

namespace Backups.Extra.ExceptionsExtension;

public class BackupTaskException : Exception
{
    private BackupTaskException(string message = "") { }

    public static BackupTaskException UnableToSaveState_InvalidType(string message = "") => new BackupTaskException(message);
    public static BackupTaskException UnableToLoadState_InvalidType(string message = "") => new BackupTaskException(message);
}