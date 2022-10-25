namespace Isu.Extra.Models;

public class AudienceNumberGenerator
{
    public int Number { get; private set; } = 100;

    public void Update() { Number++; }
}