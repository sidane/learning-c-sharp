// PaymentProcessor payment = new PaymentProcessor();
// var auth = payment.AuthoriseAsync("1111-2222-3333-4444", 100m);
using task5.Models;
using task5.Services;

OrderService orderService = new OrderService(new PaymentProcessor());
try
{
  await orderService.PlaceOrderAsync(
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
  await orderService.PlaceOrderAsync(
    "Peter Parker",
    "1111-2222-3333-4444",
    1001m
  );
}
catch (PaymentDeclinedException ex)
{
  Console.WriteLine(ex.Message);
}
