using Microsoft.Diagnostics.Runtime.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace EFCore.Compiled.Repositories;

internal class GenericRepository<T> : BaseRepository where T : Base
{
    #region Compiled Queries

    private static Func<AppDbContext, List<T>> _getCollectionCompiled =
            EF.CompileQuery((AppDbContext context)
                => context.Set<T>().AsNoTracking().ToList());
    
    private static Func<AppDbContext, IAsyncEnumerable<T>> _getCollectionCompiledAsync =
        EF.CompileAsyncQuery((AppDbContext context)
            => context.Set<T>().AsNoTracking());

    #endregion

    #region Ctor

    public GenericRepository(AppDbContext context) : base(context)
    {

    }

    #endregion

    public async Task<List<T>> GetCollection()
		=> await _context.Set<T>().ToListAsync();

    public List<T> GetCollection_Compiled()
        => _getCollectionCompiled(_context);

    public async Task<List<T>> GetCollection_CompiledAsync()
    {
        List<T> results = new();
        IAsyncEnumerable<T> items = _getCollectionCompiledAsync(_context);
        await foreach (T item in items.ConfigureAwait(true))
            results.Add(item);

        return results;
    }
}
