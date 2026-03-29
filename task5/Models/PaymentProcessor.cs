namespace task5.Models;

public class PaymentProcessor : IPaymentProcessor
{
    public async Task<bool> AuthoriseAsync(string cardNumber, decimal amount)
    {
        await Task.Delay(1000);
        return amount <= 1000m;
    }

    public async Task ProcessPaymentAsync(string cardNumber, decimal amount)
    {
        await Task.Delay(500);
        Console.WriteLine($"Payment for £{amount} processed successfully");
    }
}