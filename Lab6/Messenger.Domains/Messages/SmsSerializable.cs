namespace Messenger.Domains.Messages;

[Serializable]
public class SmsSerializable : MessageSerializable
{
    public SmsSerializable(MessageOptionsSerializable options, string contents) : base(options, contents) { }
}