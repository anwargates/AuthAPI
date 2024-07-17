using AuthAPI.Controllers;
using AuthAPI.models;
using AuthAPI.models.request;
using AuthAPI.models.response;
using System.Collections.Generic;
using System.Threading.Tasks;

// namespace AuthAPI.services;
public interface IProductService
{
    Task<GenericResponse> AddProduct(ProductReqDto dto);
    Task<GenericResponse> GetAllProducts();
    Task<GenericResponse> CountAllProducts();
    Task<GenericResponse> GetProductById(int id);
    Task<GenericResponse> UpdateProductAsync(ProductReqDto dto);
    Task<GenericResponse> DeleteProductAsync(int id);
    Task<GenericResponse> AddMultiProduct(List<ProductReqDto> dto);
    Task<GenericResponse> BrowseProduct(string productName, string units, decimal? price);
}
