using Microsoft.EntityFrameworkCore;

namespace EFCore.Compiled.Repositories;

internal class ShopRepository : BaseRepository
{
    #region Compiled Queries

    private static Func<AppDbContext, List<Shop>> _getCollectionCompiled =
            EF.CompileQuery((AppDbContext context)
                => context.Shops.AsNoTracking().ToList());

    private static Func<AppDbContext, IAsyncEnumerable<Shop>> _getCollectionCompiledAsync = 
        EF.CompileAsyncQuery((AppDbContext context) 
            => context.Shops.AsNoTracking());

    #endregion

    #region Ctor

    public ShopRepository(AppDbContext context) : base(context)
    {
    }

    #endregion

    public async Task<List<Shop>> GetCollection()
        => await _context.Shops.ToListAsync();

    public List<Shop> GetCollection_Compiled()
        => _getCollectionCompiled(_context);

    public async Task<List<Shop>> GetCollection_CompiledAsync()
    {
        List<Shop> results = new();
        IAsyncEnumerable<Shop> items = _getCollectionCompiledAsync(_context);
        await foreach (Shop item in items.ConfigureAwait(true))
            results.Add(item);

        return results;
    }
}
