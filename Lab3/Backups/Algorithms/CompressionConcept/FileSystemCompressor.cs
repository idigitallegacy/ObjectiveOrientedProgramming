using System.Xml.Serialization;
using Backups.Algorithms;
using Backups.Exceptions;
using Backups.Models;
using Backups.Services;

namespace ConsoleApp1.Algorithms.Compression;

public class FileSystemCompressor : ICompressor, IDisposable
{
    private List<MemoryStream> _data = new ();

    public void AddFile(IBackupObject backupObject) => _data.Add(backupObject.Contents);

    public void Compress(string? outputPath)
    {
        if (string.IsNullOrEmpty(outputPath))
            throw BackupException.CompressionException("Output path is null");

        using (FileStream fileStream = new FileStream(outputPath, FileMode.OpenOrCreate))
        {
            _data.ForEach(data =>
            {
                data.WriteTo(fileStream);
            });
        }
    }

    public void Clear() => _data.Clear();

    public void Accept(IVisitor visitor) => visitor.Visit(this);

    public void Dispose() => _data.ForEach(data => data.Dispose());
}