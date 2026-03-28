BankAccount joeBankAccount = new BankAccount("Joe Bloggs", 100m);
BankAccount peterBankAccount = new BankAccount("Peter Parker", 400m);
BankAccount janeBankAccount = new BankAccount("Jane Doe", 0m);
Bank bank = new Bank();

BankAccount? richestBankAccount;

try {
  richestBankAccount = bank.GetRichestAccount();
  Console.WriteLine($"The richest bank account is {richestBankAccount?.Name} with £{richestBankAccount?.Balance}");
}
catch (NoBankAccountsException ex)
{
  Console.WriteLine(ex.Message);
}

try {
  bank.AddAccount(joeBankAccount);
  bank.AddAccount(peterBankAccount);
  bank.AddAccount(peterBankAccount);
}
catch (DuplicateAccountException ex)
{
  Console.WriteLine(ex.Message);
}
Console.WriteLine($"Bank total deposits: £{bank.TotalDeposits()}");

BankAccount? foundAccount = bank.FindAccount("Joe Bloggs");

if (foundAccount is not null)
{
  Console.WriteLine($"Found account for {foundAccount.Name} with balance £{foundAccount.Balance}");
  Console.WriteLine($"Withdrawing £10 from {foundAccount.Name}");
  foundAccount.Withdraw(10m);
  Console.WriteLine($"{foundAccount.Name} account balance is now £{foundAccount.Balance}");
}

richestBankAccount = bank.GetRichestAccount();

Console.WriteLine($"The richest bank account is {richestBankAccount?.Name} with £{richestBankAccount?.Balance}");

public class Bank
{
  public List<BankAccount> Accounts {
    get;
    private set;
  } = new List<BankAccount>();

  public void AddAccount(BankAccount account)
  {
    if (FindAccount(account.Name) is not null) {
      throw new DuplicateAccountException(account);
    }

    Accounts.Add(account);
  }

  public BankAccount? FindAccount(string accountName)
  {
    return Accounts.FirstOrDefault(a => a.Name == accountName);
  }

  public decimal TotalDeposits()
  {
    return Accounts.Sum(a => a.Balance);
  }

  public BankAccount GetRichestAccount()
  {
    var account = Accounts.MaxBy(a => a.Balance);

    if (account is null)
    {
      throw new NoBankAccountsException();
    }

    return account;
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

public class DuplicateAccountException : Exception
{
  public DuplicateAccountException(BankAccount account)
    : base($"Failed to add account, one already exists for {account.Name}") {}
}

public class NoBankAccountsException : Exception
{
  public NoBankAccountsException()
    :base($"There are currently no bank accounts") {}
}

public class InsufficientFundsException : Exception
{
  public InsufficientFundsException(decimal amount)
    : base($"Insufficient funds: attempted to withdraw {amount}") {}
}
