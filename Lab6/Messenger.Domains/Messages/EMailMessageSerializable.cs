namespace Messenger.Domains.Messages;

[Serializable]
public class EMailMessageSerializable : MessageSerializable
{
    public EMailMessageSerializable(MessageOptionsSerializable options, string contents) : base(options, contents) { }
}