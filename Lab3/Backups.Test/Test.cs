using Backups.Algorithms;
using Backups.Exceptions;
using Backups.Models;
using ConsoleApp1.Algorithms.Compression;
using Xunit;

namespace Backups.Test;

public class Test
{
    [Fact]
    public void Test1()
    {
        const string path1 = @"D:\Help\Me.txt";
        const string path2 = @"D:\I\Dont\Understand\This";
        const string path3 = @"D:\I\Remade\It\Four\Times.exe";

        InMemoryRepository repository = new ();
        SingleStorage algorithm = new ();
        InMemoryCompressor compressor = new ();
        BackupTask backupTask = new BackupTaskBuilder()
            .SetName("1")
            .SetRepository(repository)
            .SetStorageAlgorithm(algorithm)
            .SetCompressor(compressor)
            .Build();

        byte[] contents1 = new[] { (byte)0xAA, (byte)0xAA, (byte)0xAA };
        byte[] contents2 = new[] { (byte)0xBB, (byte)0xBB, (byte)0xBB };
        byte[] contents3 = new[] { (byte)0xCC, (byte)0xCC, (byte)0xCC };

        var object1 = new BackupObject(path1, true, new MemoryStream(contents1));
        var object2 = new BackupObject(path2, true, new MemoryStream(contents2));
        var object3 = new BackupObject(path3, true, new MemoryStream(contents3));
        var object4 = new BackupObject(path1, false);

        backupTask.AddObject(object1);
        backupTask.AddObject(object2);
        backupTask.AddObject(object3);

        backupTask.CreateRestorePoint();

        Assert.Equal(1, backupTask.RestorePoints.Count);
        Assert.Equal(3, backupTask.RestorePoints.First().Storages.First().BackupObjects.Count);

        backupTask.CreateRestorePoint();

        Assert.Equal(2, backupTask.RestorePoints.Count);
        Assert.Equal(3, backupTask.RestorePoints.ElementAt(1).Storages.First().BackupObjects.Count);

        backupTask.DeleteBackupObject(object3);
        backupTask.CreateRestorePoint();

        Assert.Equal(3, backupTask.RestorePoints.Count);
        Assert.Equal(2, backupTask.RestorePoints.ElementAt(2).Storages.First().BackupObjects.Count);

        backupTask.AddObject(object4);
        Assert.Throws<BackupException>(() => backupTask.CreateRestorePoint());

        Assert.Equal(contents1, backupTask.RestorePoints.ElementAt(2).Storages.First().BackupObjects.First().Contents.ToArray());
    }
}