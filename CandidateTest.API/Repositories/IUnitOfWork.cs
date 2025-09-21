namespace CandidateTest.API.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        Task<int> CommitAsync(CancellationToken ct = default);
    }
}
