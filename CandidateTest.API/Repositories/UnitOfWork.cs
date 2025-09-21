using CandidateTest.Models;

namespace CandidateTest.API.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _ctx;
        public IProductRepository Products { get; }

        public UnitOfWork(AppDbContext ctx, IProductRepository products)
        {
            _ctx = ctx;
            Products = products;
        }

        public Task<int> CommitAsync(CancellationToken ct = default) => _ctx.SaveChangesAsync(ct);

        public void Dispose() => _ctx.Dispose();
    }
}
