using Messenger.Config;

namespace Messenger.Domains.Reports;

[Serializable]
public class ReportItemSerializable
{
    public ReportItemSerializable()
    {
        Name = DefaultValues.Report.DefaultItemName;
        Value = DefaultValues.Report.DefaultItemValue;
    }
    public ReportItemSerializable(string name, long value)
    {
        Name = name;
        Value = value;
    }
    
    public string Name { get; set; }
    public long Value { get; set; }
}