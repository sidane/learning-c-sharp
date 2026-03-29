using task5.Models;

namespace task5.Services;

public class OrderService
{
    private int _id;
    private IPaymentProcessor _paymentProcessor;

    public OrderService(IPaymentProcessor paymentProcessor) {
        _paymentProcessor = paymentProcessor;
    }

    public async Task<Order> PlaceOrderAsync(string customerName, string cardNumber, decimal total)
    {
        Order order = new Order(NextId(), customerName, total, false);

        var authPayment = await _paymentProcessor.AuthoriseAsync(cardNumber, total);

        if (authPayment)
        {
            await _paymentProcessor.ProcessPaymentAsync(cardNumber, total);
            order = order with { IsPaid = true };
            return order;
        } else {
            throw new PaymentDeclinedException(order);
        }
    }  
    
    private int NextId()
    {
        return _id += 1;
    }
}