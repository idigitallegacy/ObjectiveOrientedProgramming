using System.Text.Json.Serialization;
using Backups.Exceptions;
using Backups.Models;

namespace Backups.Extra.NewBackupObject;

[Serializable]
public class BackupObjectSerializable : IBackupObject
{
    public BackupObjectSerializable()
    {
        FullPath = string.Empty;
        FileName = string.Empty;
        Virtual = true;
        Backend_ByteContents = new[] { (byte)0x0 };
    }

    public BackupObjectSerializable(IBackupObject backupObject)
    {
        FullPath = backupObject.FullPath;
        FileName = backupObject.FileName;
        Virtual = backupObject.Virtual;
        Backend_ByteContents = backupObject.Contents.ToArray();
    }

    [JsonInclude]
    public string FullPath { get; set; }
    [JsonInclude]
    public string FileName { get; set; }
    [JsonInclude]
    public bool Virtual { get; set; }
    [JsonIgnore]
    public MemoryStream Contents
    {
        get
        {
            MemoryStream memoryStream = new MemoryStream();
            memoryStream.Write(Backend_ByteContents);
            return memoryStream;
        }
        set => Backend_ByteContents = value.ToArray();
    }

    [JsonInclude]
    public byte[] Backend_ByteContents { get; set; }

    public bool Equals(IBackupObject? other)
    {
        if (ReferenceEquals(null, other)) return false;
        return Equals((object)other);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return Equals((BackupObject)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(FullPath, Virtual, Contents);
    }

    protected bool Equals(BackupObject other)
    {
        if (Contents.ToArray().Length != other.Contents.ToArray().Length) return false;
        List<byte> array = Contents.ToArray().ToList();
        List<byte> otherArray = other.Contents.ToArray().ToList();

        for (int i = 0; i < array.Count; i++)
        {
            if (array[i] != otherArray[i])
                return false;
        }

        return FullPath == other.FullPath && Virtual == other.Virtual;
    }
}