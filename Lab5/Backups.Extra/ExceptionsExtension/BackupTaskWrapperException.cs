namespace Backups.Extra.ExceptionsExtension;

public class BackupTaskWrapperException : Exception
{
    private BackupTaskWrapperException(string message = "") { }

    public static BackupTaskWrapperException UnableToLoadState_InvalidName(string message = "") => new BackupTaskWrapperException(message);
    public static BackupTaskWrapperException UnableToLoadState_InvalidStorageAlgorithm(string message = "") => new BackupTaskWrapperException(message);
    public static BackupTaskWrapperException UnableToLoadState_InvalidRepository(string message = "") => new BackupTaskWrapperException(message);
    public static BackupTaskWrapperException UnableToLoadState_InvalidCompressor(string message = "") => new BackupTaskWrapperException(message);
}