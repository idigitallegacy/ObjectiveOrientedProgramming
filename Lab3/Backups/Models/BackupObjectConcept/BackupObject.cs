using Backups.Exceptions;

namespace Backups.Models;

public class BackupObject : IBackupObject
{
    public BackupObject(string path, bool inMemory = true, Stream? contents = null)
    {
        if (string.IsNullOrEmpty(path))
            throw BackupException.BackupObjectException("Path to file is empty");

        FullPath = path;
        FileName = Path.GetFileNameWithoutExtension(path);
        Virtual = inMemory;
        if (inMemory && contents is null)
            throw BackupException.BackupObjectException("Virtual backup objects must have contents");
        Contents = new MemoryStream();
        if (inMemory)
            contents?.CopyTo(Contents);
    }

    public string FullPath { get; }
    public string FileName { get; }
    public bool Virtual { get; }
    public MemoryStream Contents { get; set; }

    public bool Equals(IBackupObject? other)
    {
        if (ReferenceEquals(null, other)) return false;
        return Equals((object)other);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((BackupObject)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(FullPath, Virtual, Contents);
    }

    protected bool Equals(BackupObject other)
    {
        return FullPath == other.FullPath && Virtual == other.Virtual && Contents.Equals(other.Contents);
    }
}