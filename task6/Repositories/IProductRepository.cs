using task6.DTOs;
using task6.Models;

namespace task6.Repositories;

public interface IProductRepository {
  IEnumerable<Product> GetAll();
  Product? GetById(int id);
  Product Add(CreateProductRequest productData);
  bool Delete(int id);
}
