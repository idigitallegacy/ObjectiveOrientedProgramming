using Messenger.Domains.Messages;

namespace Messenger.Config;

[Serializable]
public static class DefaultValues
{
    public struct Account
    {
        public static readonly string DefaultLogin = "undefined";
        public static readonly string DefaultPassword = "undefined";
        public static readonly int MinimalLoginLength = 6;
        public static readonly int MaximalLoginLength = 20;
        public static readonly int MinimalPasswordLength = 6;
        public static readonly string RootAccountLogin = "root";
        public static readonly string RootAccountPassword = "root";
    }

    public struct Message
    {
        public static readonly string DefaultSubject = "No subject";
        public static readonly MessageState DefaultState = MessageState.New;
        public static readonly string DefaultContents = "Undefined";
        public static readonly string DefaultId = "Undefined";
    }
    
    public struct SMS
    {
        public static readonly int MaxDigitsAmount = 140;
    }
    
    public struct Resource
    {
        public static readonly string DefaultName = "Undefined";
    }

    public struct Report
    {
        public static readonly string DefaultItemName = "Undefined property";
        public static readonly int DefaultItemValue = 0;
    }
    
    public struct ReportAgregator
    {
        public static readonly string StartTimePropertyName = "StartTime";
        public static readonly string EndTimePropertyName = "EndTime";
        public static readonly string ViewedPropertyName = "Viewed";
        public static readonly string RepliedPropertyName = "Replied";
    }
}