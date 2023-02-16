using Messenger.Domains.Employees;

namespace Messenger.Domains.Reports;

[Serializable]
public class ReportSerializable
{
    public ReportSerializable()
    {
        ReportItems = new List<ReportItemSerializable>();
    }

    public ReportSerializable(List<ReportItemSerializable> reportItems)
    {
        ReportItems = reportItems;
    }
    
    public EmployeeSerializable? Employee { get; set; }
    public List<ReportItemSerializable> ReportItems { get; set; }
}