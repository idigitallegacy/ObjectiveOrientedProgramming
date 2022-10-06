using Shops.Exceptions;
using Shops.Models;

namespace Shops.Entities;

public class Person
{
    public Person(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new PersonException("Invalid person name");
        Name = name;
    }

    public string Name { get; }

    public int Balance { get; private set; }

    public void GiveMoney(int amount) { Balance += amount; }

    public void TakeMoney(int amount)
    {
        if (Balance < amount)
            throw new BalanceException("Not enough money to give");
        Balance -= amount;
    }
}