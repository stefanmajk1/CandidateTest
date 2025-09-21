using CandidateTest.Models.Entities;
using CandidateTest.Models;
using Microsoft.EntityFrameworkCore;

namespace CandidateTest.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _ctx;
        public ProductRepository(AppDbContext ctx) => _ctx = ctx;

        public async Task<IEnumerable<ProductEntity>> GetAllAsync() =>
            await _ctx.Products.Include(p => p.ProductCategories).ThenInclude(pc => pc.Category)
                .AsNoTracking().ToListAsync();

        public async Task<IEnumerable<ProductEntity>> GetAllFilteredAsync(string? name, decimal? minPrice, decimal? maxPrice, int? categoryId)
        {
            IQueryable<ProductEntity> q = _ctx.Products
                .Include(p => p.ProductCategories).ThenInclude(pc => pc.Category)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(name)) q = q.Where(p => p.ProductName.Contains(name));
            if (minPrice.HasValue) q = q.Where(p => p.Price >= minPrice.Value);
            if (maxPrice.HasValue) q = q.Where(p => p.Price <= maxPrice.Value);
            if (categoryId.HasValue) q = q.Where(p => p.ProductCategories.Any(pc => pc.CategoryId == categoryId));

            return await q.ToListAsync();
        }

        public async Task<(IReadOnlyList<ProductEntity> Items, int TotalCount)> GetAllFilteredPagedAsync(
            string? name, decimal? minPrice, decimal? maxPrice, IEnumerable<int>? categoryIds,
            int page, int pageSize)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0 || pageSize > 200) pageSize = 20;

            IQueryable<ProductEntity> q = _ctx.Products
                .Include(p => p.ProductCategories).ThenInclude(pc => pc.Category)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(name))
                q = q.Where(p => p.ProductName.Contains(name));

            if (minPrice.HasValue)
                q = q.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                q = q.Where(p => p.Price <= maxPrice.Value);

            if (categoryIds is { } ids && ids.Any())
                q = q.Where(p => p.ProductCategories.Any(pc => ids.Contains(pc.CategoryId)));

            var total = await q.CountAsync();

            var items = await q
                .OrderBy(p => p.ProductId)       // stabilno sortiranje da paginacija bude deterministička
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public Task<ProductEntity?> GetByIdAsync(int id) =>
            _ctx.Products.Include(p => p.ProductCategories).ThenInclude(pc => pc.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id);

        public Task CreateAsync(ProductEntity product) { _ctx.Products.Add(product); return Task.CompletedTask; }
        public Task UpdateAsync(ProductEntity product) { _ctx.Products.Update(product); return Task.CompletedTask; }
        public async Task DeleteAsync(int id)
        {
            var p = await _ctx.Products.FindAsync(id);
            if (p != null) _ctx.Products.Remove(p);
        }
    }
}
