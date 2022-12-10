using Banks.Models.BankClientConcept;

namespace Banks.Entities.BankConcept;

public class BankMessageBroker
{
    private List<BankClient> _debitSubscribers = new ();
    private List<BankClient> _creditSubscribers = new ();
    private List<BankClient> _depositSubscribers = new ();

    public void AddDebitSuscriber(BankClient client)
    {
        _debitSubscribers.Add(client);
    }

    public void AddCreditSuscriber(BankClient client)
    {
        _creditSubscribers.Add(client);
    }

    public void AddDepositSuscriber(BankClient client)
    {
        _depositSubscribers.Add(client);
    }

    public void RemoveDebitSuscriber(BankClient client)
    {
        _debitSubscribers.Remove(client);
    }

    public void RemoveCreditSuscriber(BankClient client)
    {
        _creditSubscribers.Remove(client);
    }

    public void RemoveDepositSuscriber(BankClient client)
    {
        _depositSubscribers.Remove(client);
    }

    public void NotifyDebitSuscribers(string message)
    {
        _debitSubscribers.ForEach(subscriber => subscriber.AcceptNotification(message));
    }

    public void NotifyCreditSuscribers(string message)
    {
        _creditSubscribers.ForEach(subscriber => subscriber.AcceptNotification(message));
    }

    public void NotifyDepositSuscribers(string message)
    {
        _depositSubscribers.ForEach(subscriber => subscriber.AcceptNotification(message));
    }

    public void NotifyAllSuscribers(string message)
    {
        _debitSubscribers.ForEach(subscriber => subscriber.AcceptNotification(message));
        _creditSubscribers.ForEach(subscriber => subscriber.AcceptNotification(message));
        _depositSubscribers.ForEach(subscriber => subscriber.AcceptNotification(message));
    }
}