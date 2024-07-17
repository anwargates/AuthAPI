using Microsoft.EntityFrameworkCore;
using AuthAPI.Data;
using AuthAPI.models;
using AuthAPI.models.request;
using AuthAPI.models.response;
using AuthAPI.Controllers;

// namespace AuthAPI.services;
public class ProductService : IProductService
{
    private readonly MyDbContext _context;
    private readonly ILogger<ProductService> _logger;

    public ProductService(MyDbContext context, ILogger<ProductService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<GenericResponse> AddProduct(ProductReqDto dto)
    {
        try
        {
            _logger.LogInformation("Adding Products...");
            var product = new Product
            {
                ProductName = dto.ProductName,
                Units = dto.Units,
                Price = dto.Price,
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Products added successfully");
            return new GenericResponse(201, "Products added successfully", product);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error When Registering User");
            return new GenericResponse(500, "Error When Registering User");
        }
    }

    public async Task<GenericResponse> AddMultiProduct(List<ProductReqDto> dtos)
    {
        try
        {
            _logger.LogInformation("Adding Products...");

            var products = new List<Product>();
            foreach (var dto in dtos)
            {
                var product = new Product
                {
                    ProductName = dto.ProductName,
                    Units = dto.Units,
                    Price = dto.Price,
                };

                products.Add(product);
            }

            _context.Products.AddRange(products);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Products added successfully");
            return new GenericResponse(201, "Products added successfully", products);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error When Adding Products");
            return new GenericResponse(500, "Error When Adding Products");
        }
    }

    public async Task<GenericResponse> DeleteProductAsync(int id)
    {
        try
        {
            var productResponse = await GetProductById(id);

            if (productResponse.StatusCode == 404)
            {
                return new GenericResponse(404, "Product not found");
            }

            var product = productResponse.Data as Product;
            if (product == null)
            {
                return new GenericResponse(500, "Error while retrieving product data");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return new GenericResponse(200, "Product deleted successfully", product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting product");
            return new GenericResponse(500, "Error while deleting product");
        }
    }

    public async Task<GenericResponse> GetAllProducts()
    {
        try
        {
            _logger.LogInformation("Fetching all Products...");

            var products = await _context.Products.AsQueryable().OrderByDescending(p => p.CreatedOn).ToListAsync();

            _logger.LogInformation("All Products fetched successfully");
            return new GenericResponse(200, "All Products fetched successfully", products);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error When fetching all products");
            return new GenericResponse(500, "Error When fetching all products");
        }
    }

    public async Task<GenericResponse> CountAllProducts()
    {
        try
        {
            _logger.LogInformation("Fetching all Products Count...");

            var totalData = await _context.Products.CountAsync();

            _logger.LogInformation("All Products Count fetched successfully");
            return new GenericResponse(200, "All Products Count fetched successfully", new { totalData });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error When fetching all products count");
            return new GenericResponse(500, "Error When fetching all products count");
        }
    }

    public async Task<GenericResponse> GetProductById(int id)
    {
        try
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return new GenericResponse(404, "Product not found");
            }

            return new GenericResponse(200, "Product retrieved successfully", product);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while retrieving product with ID: {id}");
            return new GenericResponse(500, "Error while retrieving product");
        }
    }

    public async Task<GenericResponse> UpdateProductAsync(ProductReqDto dto)
    {
        try
        {
            var productResponse = await GetProductById(dto.Id);

            if (productResponse.StatusCode == 404)
            {
                return new GenericResponse(404, "Product not found");
            }

            var product = productResponse.Data as Product;
            if (product == null)
            {
                return new GenericResponse(500, "Error while retrieving product data");
            }

            product.ProductName = dto.ProductName;
            product.Units = dto.Units;
            product.Price = dto.Price;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return new GenericResponse(200, "Product updated successfully", product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating product");
            return new GenericResponse(500, "Error while updating product");
        }
    }

    public async Task<GenericResponse> BrowseProduct(string productName, string units, decimal? price)
    {
        try
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(productName))
            {
                query = query.Where(p => EF.Functions.ILike(p.ProductName, $"%{productName}%"));
            }

            if (!string.IsNullOrEmpty(units))
            {
                query = query.Where(p => EF.Functions.ILike(p.Units, $"%{units}%"));
            }

            if (price.HasValue)
            {
                query = query.Where(p => EF.Functions.ILike(p.Price.ToString(), $"%{price}%"));
            }

            var result = await query.ToListAsync();

            return new GenericResponse(200, "Product Browsed", result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while browsing product");
            return new GenericResponse(500, "Error while browsing product");
        }
    }
}