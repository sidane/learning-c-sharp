BankAccount joeBankAccount = new BankAccount("Joe Bloggs", 100m);
Bank bank = new Bank();
bank.AddAccount(joeBankAccount);
BankAccount foundAccount = bank.FindAccount("Joe Bloggs2");
Console.WriteLine($"Found accounts for {foundAccount?.Name}");

public class Bank
{
  public List<BankAccount> Accounts {
    get;
    private set;
  } = new List<BankAccount>();

  public void AddAccount(BankAccount account)
  {
    Accounts.Add(account);
  }

  public BankAccount FindAccount(string accountName)
  {
    return Accounts.FirstOrDefault(
      a => a.Name == accountName,
      new NullBankAccount("No account", 0m)
    );
  }

  public decimal TotalDeposits()
  {
    return Accounts.Sum(a => a.Balance);
  }

  public BankAccount? GetRichestAccount()
  {
    return Accounts.MaxBy(a => a.Balance);
  }
}

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

public class NullBankAccount : BankAccount
{
  public NullBankAccount(string accountHolderName, decimal initialBalance)
    :base("No account", 0m) {}
}

public class InsufficientFundsException : Exception
{
  public InsufficientFundsException(decimal amount)
    : base($"Insufficient funds: attempted to withdraw {amount}") {}
}
