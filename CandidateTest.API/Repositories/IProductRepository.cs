using CandidateTest.Models.Entities;

namespace CandidateTest.API.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductEntity>> GetAllAsync();
        Task<IEnumerable<ProductEntity>> GetAllFilteredAsync(string? name, decimal? minPrice, decimal? maxPrice, int? categoryId);
        Task<(IReadOnlyList<ProductEntity> Items, int TotalCount)> GetAllFilteredPagedAsync(
        string? name, decimal? minPrice, decimal? maxPrice, IEnumerable<int>? categoryIds,
        int page, int pageSize);
        Task<ProductEntity?> GetByIdAsync(int id);
        Task CreateAsync(ProductEntity product);
        Task UpdateAsync(ProductEntity product);
        Task DeleteAsync(int id);
    }
}
