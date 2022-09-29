using Shops.Exceptions;
using Shops.Models;

namespace Shops.Entities;

public class Person
{
    public Person(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public float Balance { get; private set; }

    public void TakeMoney(float amount) { Balance += amount; }

    public void GiveMoney(float amount)
    {
        if (Balance < amount)
            throw new BalanceException("Not enough money to give");
        Balance -= amount;
    }
}