using System.Xml;
using Backups.Algorithms;
using Backups.Models;
using Backups.Services;

namespace Backups.Extra.NewBackupTask;

public class BackupTaskWrapperBuilder
{
    private DateTime? _dateLimit;
    private uint? _amountLimit;
    private BackupTaskWrapper.LimitCombinationType _limitCombination = BackupTaskWrapper.LimitCombinationType.None;
    public BackupTaskBuilder BaseBuilder { get; } = new BackupTaskBuilder();
    public string? Name { get; set; }

    public BackupTaskWrapperBuilder SetDateLimit(DateTime dateLimit)
    {
        _dateLimit = dateLimit;
        return this;
    }

    public BackupTaskWrapperBuilder SetAmountLimit(uint amountLimit)
    {
        _amountLimit = amountLimit;
        return this;
    }

    public BackupTaskWrapperBuilder SetLimitCombination(BackupTaskWrapper.LimitCombinationType combinationType)
    {
        _limitCombination = combinationType;
        return this;
    }

    public BackupTaskWrapper Build()
    {
        BackupTaskWrapper returnable = new BackupTaskWrapper(BaseBuilder.Build());
        returnable.LimitCombination = _limitCombination;
        returnable.RestorePointAmountLimit = _amountLimit;
        returnable.RestorePointDateLimit = _dateLimit;
        return returnable;
    }
}