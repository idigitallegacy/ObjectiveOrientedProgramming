using Backups.Algorithms;
using Backups.Extra.NewBackupTask;
using Backups.Extra.UpgradedBackupConcept;
using Backups.Models;
using ConsoleApp1.Algorithms.Compression;
using Xunit;

namespace Backups.Extra.Test;

public class Test
{
    [Fact]
    public void TestSerializationDeserialization()
    {
        InMemoryRepository repository = new ();
        SingleStorage algorithm = new ();
        InMemoryCompressor compressor = new ();
        BackupTaskWrapperBuilder backupTaskBuilder = new BackupTaskWrapperBuilder();
        backupTaskBuilder.BaseBuilder
            .SetName("1")
            .SetRepository(repository)
            .SetStorageAlgorithm(algorithm)
            .SetCompressor(compressor);
        UpgradedBackupTask backupTask = new UpgradedBackupTask(backupTaskBuilder.Build());

        byte[] contents1 = new[] { (byte)0xAA, (byte)0xAA, (byte)0xAA };
        byte[] contents2 = new[] { (byte)0xBB, (byte)0xBB, (byte)0xBB };
        byte[] contents3 = new[] { (byte)0xCC, (byte)0xCC, (byte)0xCC };

        MemoryStream memoryStream1 = new MemoryStream(contents1);
        MemoryStream memoryStream2 = new MemoryStream(contents2);
        MemoryStream memoryStream3 = new MemoryStream(contents3);

        var object1 = new BackupObject("abc", true, memoryStream1);
        var object2 = new BackupObject("abc", true, memoryStream2);
        var object3 = new BackupObject("abc", true, memoryStream3);

        backupTask.AddObject(object1);
        backupTask.AddObject(object2);
        backupTask.AddObject(object3);

        string json = backupTask.SaveState();

        UpgradedBackupTask newBackupTask = new UpgradedBackupTask(new BackupTaskWrapper());
        newBackupTask.RestoreState(json);

        newBackupTask.CreateRestorePoint();

        Assert.Equal(1, newBackupTask.RestorePoints.Count);
        Assert.Equal(3, newBackupTask.RestorePoints.First().Storages.First().BackupObjects.Count);

        newBackupTask.CreateRestorePoint();

        Assert.Equal(2, newBackupTask.RestorePoints.Count);
        Assert.Equal(3, newBackupTask.RestorePoints.ElementAt(1).Storages.First().BackupObjects.Count);

        newBackupTask.DeleteBackupObject(object3);
        newBackupTask.CreateRestorePoint();

        Assert.Equal(3, newBackupTask.RestorePoints.Count);
        Assert.Equal(2, newBackupTask.RestorePoints.ElementAt(2).Storages.First().BackupObjects.Count);

        Assert.Equal(contents1, newBackupTask.RestorePoints.ElementAt(2).Storages.First().BackupObjects.First().Contents.ToArray());
        Assert.Equal(contents2, newBackupTask.RestorePoints.ElementAt(2).Storages.First().BackupObjects.ElementAt(1).Contents.ToArray());
    }
}