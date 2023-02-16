using Backups.Algorithms;
using Backups.Exceptions;
using Backups.Models;
using Backups.Services;

namespace ConsoleApp1.Algorithms.Compression;

public class InMemoryCompressor : ICompressor, IDisposable
{
    private List<MemoryStream> _data = new ();

    public void AddFile(IBackupObject backupObject)
    {
        if (!backupObject.Virtual)
            throw BackupException.CompressionException("InMemoryCompressor could only store virtual backup objects.");
        _data.Add(backupObject.Contents);
    }

    public void Compress(string? outputPath) { }

    public void Clear() => _data.Clear();

    public void Dispose() => _data.ForEach(data => data.Dispose());

    public void Accept(IVisitor visitor) => visitor.Visit(this);
    }