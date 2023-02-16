using Messenger.Config;
using Messenger.DataAccess.Employees;
using Messenger.DataAccess.Reports;
using Messenger.Domains.Times;

namespace Messenger.Application.Common.Reports;

public class ReportAgregator
{
    private static List<ReportSubscriber> _observable = new List<ReportSubscriber>();

    private static readonly ReportAgregator InstanceHolder = new ReportAgregator();
    private ReportAgregator() { }

    public void AddObservable(Employee employee)
    {
        ReportSubscriber subscriber = new ReportSubscriber();
        subscriber.Employee = employee;
        subscriber.StartTime = TimeFlow.Instance.Now.Ticks;
        subscriber.ViewedMessages = 0;
        subscriber.RepliedMessages = 0;
        _observable.Add(subscriber);
    }

    public List<Report> GetReports(Employee requestor)
    {
        if (requestor.Ordinates.Count == 0)
            throw new Exception();
        List<Report> reports = new List<Report>();
        
        requestor.Ordinates.ToList().ForEach(ordinate =>
        {
            Report report = ReportGenerator.Generate(ordinate);
            ReportSubscriber? subscriber = _observable.Find(subscriber => subscriber.Employee?.Equals(ordinate) ?? false);
            if (subscriber is null)
                return;
            subscriber.EndTime = TimeFlow.Instance.Now.Ticks;
            List<ReportItem> reportItems = BuildReportItems(subscriber);
            reportItems.ForEach(item => report.AddReportItem(item));
            reports.Add(report);
        });
        return reports;
    }

    public static ReportAgregator Instance => InstanceHolder;

    public void IncreaseViewed(Employee employee)
    {
        _observable.First(needleEmployee => needleEmployee.Equals(employee)).ViewedMessages += 1;
    }
    
    public void IncreaseReplied(Employee employee)
    {
        _observable.First(needleEmployee => needleEmployee.Equals(employee)).RepliedMessages += 1;
    }

    private List<ReportItem> BuildReportItems(ReportSubscriber subscriber)
    {
        ReportItem startTime = ReportItemGenerator.Generate(DefaultValues.ReportAgregator.StartTimePropertyName, subscriber.StartTime);
        ReportItem endTime =
            ReportItemGenerator.Generate(DefaultValues.ReportAgregator.EndTimePropertyName, subscriber.EndTime);
        ReportItem viewed = ReportItemGenerator.Generate(DefaultValues.ReportAgregator.ViewedPropertyName,
            subscriber.ViewedMessages);
        ReportItem replied = ReportItemGenerator.Generate(DefaultValues.ReportAgregator.RepliedPropertyName,
            subscriber.RepliedMessages);
        return new List<ReportItem> { startTime, endTime, viewed, replied };
    }
}