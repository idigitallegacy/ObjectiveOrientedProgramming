using System.Text;
using Backups.Extra.NewBackupTask;

namespace Backups.Extra.LoggerConcept;

public class Logger : ILogger
{
    private Stream _stream;

    public Logger(Stream stream)
    {
        _stream = stream;
    }

    public Stream? Log(string message)
    {
        if (!_stream.CanWrite)
            return null;
        byte[] bytes = Encoding.ASCII.GetBytes(message + '\n');
        _stream.Write(bytes);
        return _stream;
    }

    public void CopyStreamBytes(Stream sourceStream)
    {
        sourceStream.CopyTo(_stream);
    }
}