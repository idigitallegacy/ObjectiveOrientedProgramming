namespace Backups.Extra.LoggerConcept;

public interface ILogger
{
    Stream? Log(string message);
    void CopyStreamBytes(Stream sourceStream);
}