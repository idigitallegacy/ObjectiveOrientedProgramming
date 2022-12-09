using Banks.Entities.BankConcept;
using Banks.Exceptions;

namespace Banks.Models.TimeFlowConcept;

public class TimeFlowMessageBroker
{
    private List<IBank> _banks = new List<IBank>();
    public TimeFlowMessageBroker() { }

    public void AddSubscriber(IBank bank)
    {
        _banks.Add(bank);
    }

    public void RemoveSubscriber(IBank bank)
    {
        if (!_banks.Remove(bank))
            throw TimeFlowException.InvalidSubscriber();
    }

    public void Notify(TimeSpan difference)
    {
        _banks.ForEach(bank => bank.AcceptTimeNotification(difference));
    }
}