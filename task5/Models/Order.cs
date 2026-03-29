namespace task5.Models;

public record Order(int Id, string CustomerName, decimal Total, bool IsPaid);