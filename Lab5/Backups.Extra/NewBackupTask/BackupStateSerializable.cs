using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using Backups.Algorithms;
using Backups.Extra.NewBackupObject;
using Backups.Models;
using Backups.Services;
using ConsoleApp1.Algorithms.Compression;

namespace Backups.Extra.NewBackupTask;

[Serializable]
public class BackupStateSerializable
{
    public BackupStateSerializable() { Name = string.Empty; }

    public BackupStateSerializable(
        string name,
        SingleStorage? backend_SingleStorage,
        SplitStorage? backend_SplitStorage,
        InMemoryCompressor? backend_IMC,
        FileSystemCompressor? backend_FSC,
        InMemoryRepository? backend_IMR,
        List<BackupObjectSerializable>? backend_BO,
        List<RestorePoint>? backend_RP)
    {
        Name = name;
        Backend_SingleStorage = backend_SingleStorage;
        Backend_SplitStorage = backend_SplitStorage;
        Backend_IMC = backend_IMC;
        Backend_FSC = backend_FSC;
        Backend_IMR = backend_IMR;
        Backend_BO = backend_BO;
        Backend_RP = backend_RP;
    }

    public BackupStateSerializable(BackupTaskWrapper backupTask)
    {
        Name = backupTask.Name;
        StorageAlgorithm = backupTask.StorageAlgorithm;
        Compressor = backupTask.Compressor;
        Repository = backupTask.Repository;
        BackupObjects = backupTask.BackupObjects;
        RestorePoints = backupTask.RestorePoints;
    }

    [JsonInclude]
    public string Name { get; set; }

    [JsonIgnore]
    public IStorageAlgorithm? StorageAlgorithm { get; private set; }

    [JsonInclude]
    public SingleStorage? Backend_SingleStorage
    {
        get => StorageAlgorithm is SingleStorage ? (SingleStorage)StorageAlgorithm : null;
        set => StorageAlgorithm = value is null ? StorageAlgorithm : value;
    }

    [JsonInclude]
    public SplitStorage? Backend_SplitStorage
    {
        get => StorageAlgorithm is SplitStorage ? (SplitStorage)StorageAlgorithm : null;
        set => StorageAlgorithm = value is null ? StorageAlgorithm : value;
    }

    [JsonIgnore]
    public ICompressor? Compressor { get; private set; }

    [JsonInclude]
    public InMemoryCompressor? Backend_IMC
    {
        get => Compressor is InMemoryCompressor ? (InMemoryCompressor)Compressor : null;
        set => Compressor = value is null ? Compressor : value;
    }

    [JsonInclude]
    public FileSystemCompressor? Backend_FSC
    {
        get => Compressor is FileSystemCompressor ? (FileSystemCompressor)Compressor : null;
        set => Compressor = value is null ? Compressor : value;
    }

    [JsonIgnore]
    public IRepository? Repository { get; private set; }

    [JsonInclude]
    public InMemoryRepository? Backend_IMR
    {
        get => Repository is InMemoryRepository ? (InMemoryRepository)Repository : null;
        set => Repository = value is null ? Repository : value;
    }

    [JsonInclude]
    public FileSystemRepository? Backend_FSR
    {
        get => Repository is FileSystemRepository ? (FileSystemRepository)Repository : null;
        set => Repository = value is null ? Repository : value;
    }

    [JsonIgnore]
    public IReadOnlyCollection<IBackupObject>? BackupObjects { get; private set; }

    [JsonInclude]
    public List<BackupObjectSerializable>? Backend_BO
    {
        get
        {
            List<BackupObjectSerializable> objects = new List<BackupObjectSerializable>();
            BackupObjects?.ToList().Select(obj => new BackupObjectSerializable(obj)).ToList().ForEach(obj => objects.Add(obj));
            return objects.Count > 0 ? objects : null;
        }
        set => BackupObjects = value is null ? BackupObjects : value.AsReadOnly();
    }

    [JsonIgnore]
    public IReadOnlyCollection<IRestorePoint>? RestorePoints { get; private set; }

    [JsonInclude]
    public List<RestorePoint>? Backend_RP
    {
        get
        {
            List<RestorePoint> objects = new List<RestorePoint>();
            RestorePoints?.ToList().ForEach(obj => objects.Add((RestorePoint)obj));
            return objects.Count > 0 ? objects : null;
        }
        set => RestorePoints = value is null ? RestorePoints : value.AsReadOnly();
    }
}