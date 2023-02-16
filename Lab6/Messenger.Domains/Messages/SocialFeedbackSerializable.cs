namespace Messenger.Domains.Messages;

[Serializable]
public class SocialFeedbackSerializable : MessageSerializable
{
    public SocialFeedbackSerializable() : base() { }
    public SocialFeedbackSerializable(MessageOptionsSerializable options, string contents) : base(options, contents) { }
}