namespace Backups.Exceptions;

public class BackupException : Exception
{
    private BackupException(string message = "")
        : base(message) { }

    public static BackupException CompressionException(string message = "") => new BackupException(message);

    public static BackupException StorageAlgorithmException(string message = "") => new BackupException(message);

    public static BackupException BackupConceptException(string message = "") => new BackupException(message);

    public static BackupException BackupTaskException(string message = "") => new BackupException(message);

    public static BackupException BackupObjectException(string message = "") => new BackupException(message);

    public static BackupException RestorePointException(string message = "") => new BackupException(message);

    public static BackupException StorageException(string message = "") => new BackupException(message);

    public static BackupException RepositoryException(string message = "") => new BackupException(message);
}