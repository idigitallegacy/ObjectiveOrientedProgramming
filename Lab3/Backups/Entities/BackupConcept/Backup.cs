using Backups.Exceptions;

namespace Backups.Models;

public class Backup : IBackup
{
    private List<IRestorePoint> _restorePoints = new ();
    private int _iteratorStage = 0;

    public bool HasNext { get; private set; } = false;

    public IReadOnlyCollection<IRestorePoint> GetAll() => _restorePoints.AsReadOnly();

    public IRestorePoint GetNext()
    {
        if (!HasNext)
            throw BackupException.BackupConceptException("Backup history has no next restore points");
        _iteratorStage++;
        if (_iteratorStage == _restorePoints.Count)
            HasNext = false;
        return _restorePoints[_iteratorStage - 1];
    }

    public void Remove(IRestorePoint restorePoint)
    {
        _restorePoints.Remove(restorePoint);
        if (!HasNext)
            _iteratorStage--;
        if (_iteratorStage == 0 && _restorePoints.Count == 0)
            HasNext = false;
    }

    public void Add(IRestorePoint restorePoint)
    {
        HasNext = true;
        _restorePoints.Add(restorePoint);
    }
}