using Messenger.Config;

namespace Messenger.Domains.Messages;

[Serializable]
public class MessageSerializable
{
    public MessageSerializable()
    {
        Options = new MessageOptionsSerializable();
        State = DefaultValues.Message.DefaultState;
        Contents = DefaultValues.Message.DefaultContents;
    }
    public MessageSerializable(MessageOptionsSerializable options, string contents)
    {
        State = MessageState.New;
        Options = options;
        Contents = contents;
    }
    
    public MessageOptionsSerializable Options { get; set; }
    public MessageState State { get; set; }
    public MessageSerializable? ReplyMessage { get; set; }
    public string Contents { get; set; }
}