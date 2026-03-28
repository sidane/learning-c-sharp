BankAccount joeBankAccount = new BankAccount("Joe Bloggs", "joe@example.com", "555-123", 100m, "email", new EmailNotificationService());
BankAccount peterBankAccount = new BankAccount("Peter Parker", "peter@example.com", "555-456", 400m, "phone", new SmsNotificationService());
BankAccount janeBankAccount = new BankAccount("Jane Doe", "jane@example.com", "555-789", 0m, "email", new EmailNotificationService());
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

Console.WriteLine($"Depositing £50 to {peterBankAccount.Name}");
peterBankAccount.Deposit(50m);
Console.WriteLine($"{peterBankAccount.Name} account balance is now £{peterBankAccount.Balance}");

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
  public string Email { get; private set; }
  public string PhoneNumber { get; private set; }
  public decimal Balance { get; private set; }
  public string PreferredContactMethod { get; private set; }

  private readonly INotificationService _notificationService;

  public BankAccount(
      string accountHolderName,
      string email,
      string phoneNumber,
      decimal initialBalance,
      string preferredContactMethod,
      INotificationService notificationService
  )
  {
     Name = accountHolderName;
     Email = email;
     PhoneNumber = phoneNumber;
     Balance = initialBalance;
     PreferredContactMethod = preferredContactMethod;
     _notificationService = notificationService;
  }

  public void Deposit(decimal amount)
  {
    Balance += amount;
    _notificationService.Send(
      ContactDetails(),
      $"Your deposit of £{amount} has been received"
    );
  }

  public void Withdraw(decimal amount)
  {
    if (Balance < amount) {
      throw new InsufficientFundsException(amount);
    }

    Balance -= amount;
    _notificationService.Send(
      ContactDetails(),
      $"Your withdrawal of £{amount} has been received"
    );
  }

  private string ContactDetails()
  {
    switch(PreferredContactMethod)
    {
      case "email":
        return Email;
      case "phone":
        return PhoneNumber;
      default:
        throw new Exception("Unrecognised contact method");
    }
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

public interface INotificationService {
  void Send(string recipient, string message);
}

public class EmailNotificationService : INotificationService
{
  public void Send(string recipient, string message)
  {
    Console.WriteLine($"[EMAIL to {recipient}]: {message}");
  }
}

public class SmsNotificationService : INotificationService
{
  public void Send(string recipient, string message)
  {
    Console.WriteLine($"[SMS to {recipient}]: {message}");
  }
}
