using Microsoft.EntityFrameworkCore;

namespace EFCore.Compiled.Repositories;

internal class GenericRepository<T> : BaseRepository where T : Base
{
	public GenericRepository(AppDbContext context):base(context)
	{
		
	}

    public async Task<T> GetById(Guid id)
		=> await _context.Set<T>().FindAsync(id);

    public async Task<HashSet<T>> GetCollection()
		=> (await _context.Set<T>().ToListAsync()).ToHashSet();

}
