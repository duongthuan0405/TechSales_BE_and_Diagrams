using System.Collections.Generic;
using System.Threading.Tasks;
using TechSalesManagement.Application.Exceptions;
using TechSalesManagement.Application.Repositories;
using TechSalesManagement.Application.Services.Interfaces;
using TechSalesManagement.Application.Services.Params;
using TechSalesManagement.Common;
using TechSalesManagement.Domain.Entities;

namespace TechSalesManagement.Application.Services.Implementations;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<Product>> SearchProductsAsync(SearchProductParams parameters)
    {
        var keyword = parameters.Keyword?.Trim();
        
        return await _productRepository.GetProductsAsync(keyword, parameters.CategoryIds, parameters.SortOrder);
    }

    public async Task<Product> GetProductDetailsAsync(GetProductDetailsParams parameters)
    {
        var product = await _productRepository.GetByIdAsync(parameters.ProductId);
        if (product == null)
        {
            throw new NotFoundException(MessageConstants.MSG25);
        }
        return product;
    }
}
