// int int_Choose = (int) Ingredients.TheFullEnglish;
// Console.WriteLine(int_Choose);
//
//
// public enum Ingredients
// {
//     Eggs           =        0b1, // 1
//     Bacon          =       0b10, // 2
//     Sausages       =      0b100, // 4
//     Mushrooms      =     0b1000, // 8
//     Tomato         =   0b1_0000, // 16
//     BlackPudding   =  0b10_0000, // 32
//     BakedBeans     = 0b100_0000, // 64
//     TheFullEnglish = 0b111_1111, // 127
// }
//
//

var orderService = new OrderService();
Order order1 = orderService.PlaceOrder(
  "Jane Bloggs",
  new Address("22 Main Street", "Big City", "190-ABC", "Fun Land"),
  100.00m
);
Order order2 = orderService.PlaceOrder(
  "Peter Parker",
  new Address("11 Short Street", "Small City", "200-DEF", "Sad Land"),
  20.00m
);
Order order3 = orderService.PlaceOrder(
  "Joe Bloggs",
  new Address("11 Medium Street", "Mid City", "300-GHI", "Bland Land"),
  4903.00m
);


orderService.UpdateStatus(order1.Id, OrderStatus.Shipped);
orderService.UpdateStatus(order3.Id, OrderStatus.Shipped);

// Console.WriteLine($"Order {order1.Id} status: {order1.Status}");
// Console.WriteLine($"Order {order2.Id} status: {order2.Status}");
// Console.WriteLine($"Order {order3.Id} status: {order3.Status}");
Console.WriteLine(order1.DeliveryAddress);

var pendingOrders = orderService.GetOrdersByStatus(OrderStatus.Pending);

foreach (var order in pendingOrders)
{
  Console.WriteLine($"Order {order.Id}: {order.CustomerName} - {order.Status}");
}

var shippedOrders = orderService.GetOrdersByStatus(OrderStatus.Shipped);

foreach (var order in shippedOrders)
{
  Console.WriteLine($"Order {order.Id}: {order.CustomerName} - {order.Status}");
}

public enum OrderStatus
{
  Pending,
  Processing,
  Shipped,
  Delivered,
  Cancelled
}

public record Address(string Street, string City, string PostCode, string Country);

public record Order(
    int Id,
    string CustomerName,
    Address DeliveryAddress,
    OrderStatus Status,
    decimal Total
);

public class OrderService
{
  private List<Order> _orders = new List<Order>();
  private int _id;

  public Order PlaceOrder(string customerName, Address address, decimal total)
  {
    var order = new Order(
      NextId(),
      customerName,
      address,
      OrderStatus.Pending,
      total
    );
    _orders.Add(order);
    return order;
  }

  public void UpdateStatus(int orderId, OrderStatus newStatus)
  {
    int index = _orders.FindIndex(o => o.Id == orderId);
    if (index != -1)
        _orders[index] = _orders[index] with { Status = newStatus };
  }

  public List<Order> GetOrdersByStatus(OrderStatus status)
  {
    return _orders.FindAll(o => o.Status == status);
  }

  private int NextId()
  {
    return _id += 1;
  }
}
