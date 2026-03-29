namespace task5.Models;
    
public interface IPaymentProcessor
{
    Task<bool> AuthoriseAsync(string cardNumber, decimal amount);
    Task ProcessPaymentAsync(string cardNumber, decimal amount);
}