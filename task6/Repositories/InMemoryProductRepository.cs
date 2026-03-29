using task6.DTOs;
using task6.Models;

namespace task6.Repositories;

public class InMemoryProductRepository : IProductRepository {
  private readonly List<Product> _products = new List<Product>();
  private int _nextId;

  public InMemoryProductRepository()
  {
    Add(new CreateProductRequest("Product 1", 10.99m, "Stuff"));
    Add(new CreateProductRequest("Product 2", 3.99m, "Misc"));
    Add(new CreateProductRequest("Product 3", 20.99m, "Stuff"));
  }

  public IEnumerable<Product> GetAll()
  {
    return _products;
  }

  public Product? GetById(int id)
  {
    return _products.FirstOrDefault(p => p.Id == id);
  }

  public Product Add(CreateProductRequest productData)
  {
    var product = new Product(
      NextId(),
      productData.Name,
      productData.Price,
      productData.Category
    );
    _products.Add(product);
    return product;
  }

  public bool Delete(int id)
  {
    Product? product = GetById(id);
    if (product is not null) {
      return _products.Remove(product);
    } else {
      return false;
    }
  }

  private int NextId()
  {
    return ++_nextId;
  }
}
