// PaymentProcessor payment = new PaymentProcessor();
// var auth = payment.AuthoriseAsync("1111-2222-3333-4444", 100m);

OrderService orderService = new OrderService(new PaymentProcessor());
try
{
  var order1 = await orderService.PlaceOrderAsync(
    "Joe Bloggs",
    "1111-2222-3333-4444",
    10m
  );
}
catch (PaymentDeclinedException ex)
{
  Console.WriteLine(ex.Message);
}
try {
  var order2 = await orderService.PlaceOrderAsync(
    "Peter Parker",
    "1111-2222-3333-4444",
    1001m
  );
}
catch (PaymentDeclinedException ex)
{
  Console.WriteLine(ex.Message);
}

public interface IPaymentProcessor
{
  Task<bool> AuthoriseAsync(string cardNumber, decimal amount);
  Task ProcessPaymentAsync(string cardNumber, decimal amount);
}

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

public record Order(int Id, string CustomerName, decimal Total, bool IsPaid);

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

public class PaymentDeclinedException : Exception
{
  public PaymentDeclinedException(Order order)
    : base($"Payment declined for order {order.Id} for amount £{order.Total}") {}
}
