using Banks.Models.BankAccountConcept;
using Banks.Models.BankClientConcept;

namespace Banks.Models.ClientAccountConcept;

public interface IClientAccount : IEquatable<IClientAccount>
{
    BankClient PersonInfo { get; }
    List<IBankAccount> Accounts { get; }
    bool IsTrusted { get; }
}