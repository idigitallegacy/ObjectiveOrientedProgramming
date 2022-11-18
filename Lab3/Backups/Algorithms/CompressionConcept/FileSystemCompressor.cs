using System.Xml.Serialization;
using Backups.Algorithms;
using Backups.Exceptions;
using Backups.Models;
using Backups.Services;

namespace ConsoleApp1.Algorithms.Compression;

public class FileSystemCompressor : ICompressor, IDisposable
{
    private List<MemoryStream> _data = new ();
    private string _fileHeader = "<FILE>";
    private string _fileEnding = "</FILE>";

    public void AddFile(IBackupObject backupObject) => _data.Add(backupObject.Contents);

    public void Compress(string? outputPath)
    {
        if (string.IsNullOrEmpty(outputPath))
            throw BackupException.CompressionException("Output path is null");

        byte[] header = ConvertStringToByteArray(_fileHeader);
        byte[] footer = ConvertStringToByteArray(_fileEnding);

        using (FileStream fileStream = new FileStream(outputPath, FileMode.OpenOrCreate))
        {
            _data.ForEach(data =>
            {
                fileStream.Write(header);
                data.WriteTo(fileStream);
                fileStream.Write(footer);
            });
        }
    }

    public void Clear() => _data.Clear();

    public void Accept(IVisitor visitor) => visitor.Visit(this);

    public void Dispose() => _data.ForEach(data => data.Dispose());

    private static byte[] ConvertStringToByteArray(string obj)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(string));
        MemoryStream ms = new MemoryStream();
        serializer.Serialize(ms, obj);
        return ms.ToArray();
    }

    private static string ConvertByteArrayToString(byte[] byteArray)
    {
        Stream stream = new MemoryStream(byteArray);
        return (string?)new XmlSerializer(byteArray.GetType()).Deserialize(stream) ??
               throw BackupException.CompressionException("Unable to deserialize byte array (FileSystemCompressor.ConvertByteArrayToString)");
    }
}