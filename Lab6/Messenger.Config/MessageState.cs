namespace Messenger.Domains.Messages;


[Serializable]
public enum MessageState
{
    New = 0,
    Viewed,
    Replied
}