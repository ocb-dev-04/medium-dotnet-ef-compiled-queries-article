using Microsoft.EntityFrameworkCore;

namespace EFCore.Compiled.Repositories;

internal class ShopRepository : BaseRepository
{
    #region Compiled Queries

    private static Func<AppDbContext, Guid, Shop> _getByIdCompiled =
            EF.CompileQuery((AppDbContext context, Guid id)
                => context.Shops.Find(id));

    private static Func<AppDbContext, Guid, Task<Shop>> _getByIdAsyncCompiled =
            EF.CompileAsyncQuery((AppDbContext context, Guid id)
                => context.Shops.Find(id));

    private static Func<AppDbContext, List<Shop>> _getCollectionCompiled =
            EF.CompileQuery((AppDbContext context)
                => context.Shops.ToList());

    private static Func<AppDbContext, Task<List<Shop>>> _getCollectionAsyncCompiled =
            EF.CompileAsyncQuery((AppDbContext context)
                => context.Shops.ToList());

    #endregion
    
    #region Ctor

    public ShopRepository(AppDbContext context) : base(context)
    {

    }

    #endregion

    #region General

    public async Task<Shop> GetById(Guid id)
        => await _context.Shops.FindAsync(id);

    public async Task<HashSet<Shop>> GetCollection()
        => (await _context.Set<Shop>().ToListAsync()).ToHashSet();

    #endregion
    
    #region Compiled

    public Shop GetByIdCompiled(Guid id)
    => _getByIdCompiled(_context, id);

    public async Task<Shop> GetByIdCompiledAsync(Guid id)
    => await _getByIdAsyncCompiled(_context, id);

    public HashSet<Shop> GetCollectionCompiled()
        => _getCollectionCompiled(_context).ToHashSet();
    
    public async Task<HashSet<Shop>> GetCollectionCompiledAsync()
        => (await _getCollectionAsyncCompiled(_context)).ToHashSet();

    #endregion
}
