namespace task6.DTOs;

public record CreateProductRequest(
  string Name,
  decimal Price,
  string Category
);
