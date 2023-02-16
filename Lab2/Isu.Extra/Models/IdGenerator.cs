using System.Data;

namespace Isu.Extra.Models;

public class IdGenerator
{
    public int Id { get; private set; } = 0;

    public void Update() { Id++; }
}