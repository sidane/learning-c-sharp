BankAccount bankAccount1 = new BankAccount("Joe Bloggs", 100.00m);
bankAccount1.Deposit(50.00m);
try
{
  bankAccount1.Withdraw(10.00m);
}
catch (InsufficientFundsException ex)
{
  Console.WriteLine(ex.Message);
}
Console.WriteLine($"{bankAccount1.Name} your current balance is {bankAccount1.Balance}");

BankAccount bankAccount2 = new BankAccount("Peter Parker", 10.00m);
bankAccount2.Deposit(5.00m);
try
{
  bankAccount2.Withdraw(20.00m);
}
catch (InsufficientFundsException ex)
{
  Console.WriteLine(ex.Message);
}
Console.WriteLine($"{bankAccount2.Name} your current balance is {bankAccount2.Balance}");

public class BankAccount
{
  public string Name { get; private set; }
  public decimal Balance { get; private set; }

  public BankAccount(string accountHolderName, decimal initialBalance)
  {
     Name = accountHolderName;
     Balance = initialBalance;
  }

  public void Deposit(decimal amount)
  {
    Balance += amount;
  }

  public void Withdraw(decimal amount)
  {
    if (Balance < amount) {
      throw new InsufficientFundsException(amount);
    }

    Balance -= amount;
  }
}

public class InsufficientFundsException : Exception
{
  public InsufficientFundsException(decimal amount)
    : base($"Insufficient funds: attempted to withdraw {amount}") {}
}
