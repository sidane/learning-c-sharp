namespace task5.Models;

public class PaymentDeclinedException : Exception
{
    public PaymentDeclinedException(Order order)
        : base($"Payment declined for order {order.Id} for amount £{order.Total}") {}
}