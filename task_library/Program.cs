Library library = new Library();
Book book1 = new Book("The Blade Itself", "Joe Abercrombie", 2006, false);
Book book2 = new Book("Before They Are Hanged", "Joe Abercrombie", 2007, false);
Book book3 = new Book("Last Argument of Kings", "Joe Abercrombie", 2008, false);

try {
  library.AddBook(book1);
  library.AddBook(book2);
  library.AddBook(book3);
  library.AddBook(book3);
}
catch (DuplicateBookException ex)
{
  Console.WriteLine(ex.Message);
}

try
{
  Console.WriteLine($"Borrowing '{book1.Title}'");
  library.Borrow(book1.Title);
  Console.WriteLine($"{book1.Title} borrowed");
}
catch (BookNotFoundException ex)
{
  Console.WriteLine(ex.Message);
}

var books = library.GetAvailableBooks();
Console.WriteLine("Available Books:");
foreach (Book book in books)
{
  Console.WriteLine($"- {book.Title}");
}

try
{
  Console.WriteLine($"Borrowing '{book1.Title}'");
  library.Borrow(book1.Title);
  Console.WriteLine($"{book1.Title} borrowed");
}
catch (Exception ex) when (ex is BookNotFoundException || ex is BookAlreadyBorrowedException)
{
  Console.WriteLine(ex.Message);
}

try
{
  Console.WriteLine($"Returning '{book1.Title}'");
  library.Return(book1.Title);
  Console.WriteLine($"{book1.Title} returned");
}
catch (Exception ex) when (ex is BookAlreadyBorrowedException || ex is BookNotFoundException)
{
  Console.WriteLine(ex.Message);
}

try
{
  Console.WriteLine($"Borrowing '{book1.Title}'");
  library.Borrow(book1.Title);
  Console.WriteLine($"{book1.Title} borrowed");
}
catch (Exception ex) when (ex is BookAlreadyBorrowedException || ex is BookNotFoundException)
{
  Console.WriteLine(ex.Message);
}

Book? oldestBook = library.GetOldestBook();
if (oldestBook is not null)
{
  Console.WriteLine($"Oldest Book: {oldestBook.Title} ({oldestBook.Year})");
}

public record Book(string Title, string Author, int Year, bool IsBorrowed);

public class Library {
  private readonly List<Book> _books = new List<Book>();

  public void AddBook(Book book)
  {
    if (FindByTitle(book.Title) is not null) {
      throw new DuplicateBookException(book);
    }
    _books.Add(book);
  }

  public void Borrow(string title)
  {
    Book? book = FindByTitle(title);
    if (book is null)
    {
      throw new BookNotFoundException(title);
    }

    if (book.IsBorrowed)
    {
      throw new BookAlreadyBorrowedException(book);
    }

    int bookIndex = FindBookIndex(book);
    if (bookIndex != -1)
    {
      _books[bookIndex] = book with { IsBorrowed = true };
    }
  }

  public void Return(string title)
  {
    Book? book = FindByTitle(title);
    if (book is null)
    {
      throw new BookNotFoundException(title);
    }

    if (!book.IsBorrowed)
    {
      throw new BookNotBorrowedException(book);
    }

    int bookIndex = FindBookIndex(book);
    if (bookIndex != -1)
    {
      _books[bookIndex] = book with { IsBorrowed = false };
    }
  }

  public IEnumerable<Book> GetAvailableBooks()
  {
    return _books.Where(b => b.IsBorrowed == false);
  }

  public Book? GetOldestBook()
  {
    return _books.MinBy(b => b.Year);
  }

  private Book? FindByTitle(string title)
  {
    return _books.FirstOrDefault(b => b.Title == title);
  }

  private int FindBookIndex(Book book)
  {
    return _books.FindIndex(b => b.Title == book.Title);
  }
}

public class DuplicateBookException : Exception
{
  public DuplicateBookException(Book book) : base(
    $"Book '{book.Title}' already exists in Library"
  ) {}
}

public class BookNotFoundException : Exception
{
  public BookNotFoundException(string title) : base(
    $"Book '{title}' not found"
  ) {}
}

public class BookAlreadyBorrowedException : Exception
{
  public BookAlreadyBorrowedException(Book book) : base(
    $"Book '{book.Title}' has already been borrowed"
  ) {}
}

public class BookNotBorrowedException : Exception
{
  public BookNotBorrowedException(Book book) : base(
    $"Book '{book.Title}' not borrowed so cannot be returned"
  ) {}
}
