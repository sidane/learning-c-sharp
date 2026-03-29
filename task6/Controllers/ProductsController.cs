using Microsoft.AspNetCore.Mvc;
using task6.DTOs;
using task6.Models;
using task6.Repositories;

namespace task6.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
  private readonly IProductRepository _productRepo;

  public ProductsController(IProductRepository productsRepository)
  {
    _productRepo = productsRepository;
  }

  [HttpGet]
  public ActionResult<IEnumerable<Product>> GetAll()
  {
    var products = _productRepo.GetAll();
    return Ok(products);
  }

  [HttpGet("{id}")]
  public ActionResult<Product> GetProduct(int id)
  {
    Product? product = _productRepo.GetById(id);
    if (product is null)
    {
      return NotFound();
    }
    return Ok(product);
  }

  [HttpPost]
  public ActionResult<Product> CreateProduct(CreateProductRequest request)
  {
    var newProduct = _productRepo.Add(request);
    return CreatedAtAction(nameof(GetProduct), new { id = newProduct.Id }, newProduct);
  }

  [HttpDelete("{id}")]
  public ActionResult<Product> DeleteProduct(int id)
  {
    var product = _productRepo.GetById(id);
    if (product is null)
    {
      return NotFound();
    }
    _productRepo.Delete(id);
    return NoContent();
  }
}
