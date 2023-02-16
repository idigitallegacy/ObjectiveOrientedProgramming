using Backups.Algorithms;
using Backups.Services;
using ConsoleApp1.Algorithms.Compression;

namespace Backups.Models;

public class BackupTaskBuilder
{
    private string _name = string.Empty;
    private IRepository _repository = new InMemoryRepository();
    private IStorageAlgorithm _storageAlgorithm = new SingleStorage();
    private ICompressor _compressor = new InMemoryCompressor();

    public BackupTaskBuilder SetName(string name)
    {
        _name = name;
        return this;
    }

    public BackupTaskBuilder SetRepository(IRepository repository)
    {
        _repository = repository;
        return this;
    }

    public BackupTaskBuilder SetStorageAlgorithm(IStorageAlgorithm algorithm)
    {
        _storageAlgorithm = algorithm;
        return this;
    }

    public BackupTaskBuilder SetCompressor(ICompressor compressor)
    {
        _compressor = compressor;
        return this;
    }

    public BackupTask Build()
    {
        BackupTask backupTask = new BackupTask(_name, _repository, _storageAlgorithm, _compressor);
        return backupTask;
    }

    public void Reset()
    {
        _name = string.Empty;
        _repository = new InMemoryRepository();
        _storageAlgorithm = new SingleStorage();
        _compressor = new InMemoryCompressor();
    }
}