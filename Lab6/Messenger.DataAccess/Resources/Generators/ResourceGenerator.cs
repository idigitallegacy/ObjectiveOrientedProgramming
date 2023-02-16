using Messenger.DataAccess.Resources.Objects;
using Messenger.DataAccess.Resources.Validators;

namespace Messenger.DataAccess.Resources.Generators;

public static class ResourceGenerator
{
    public static EMail GenerateEmail(string name)
    {
        ResourceValidator.ValidateEmailName(name);
        return new EMail(name);
    }

    public static Phone GeneratePhone(string name)
    {
        ResourceValidator.ValidatePhoneName(name);
        return new Phone(name);
    }

    public static Social GenerateSocial(string name)
    {
        ResourceValidator.ValidateSocialName(name);
        return new Social(name);
    }
}