BankAccount bankAccount = new BankAccount("Joe Bloggs", 100.00m);
bankAccount.Deposit(50.00m);
bankAccount.Withdraw(10.00m);
Console.WriteLine($"Your current balance is {bankAccount.CurrentBalance()}");

public class BankAccount
{
  public string name;
  public decimal balance;

  public BankAccount(string name, decimal initialBalance)
  {
     this.name = name;
     balance = initialBalance;
  }

  public void Deposit(decimal amount)
  {
    balance += amount;
  }

  public void Withdraw(decimal amount)
  {
    balance -= amount;
  }

  public decimal CurrentBalance()
  {
    return balance;
  }
}


