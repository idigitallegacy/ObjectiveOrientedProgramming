using Shops.Exceptions;
using Shops.Models;

namespace Shops.Entities;

public class Person
{
    public Person(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new PersonException("Invalid person name");
        Name = name;
    }

    public string Name { get; }

    public decimal Balance { get; private set; }

    public void GiveMoney(decimal amount) { Balance += amount; }

    public void TakeMoney(decimal amount)
    {
        if (Balance < amount)
            throw new BalanceException("Not enough money to give");
        Balance -= amount;
    }
}