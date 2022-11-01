using Microsoft.EntityFrameworkCore;

namespace EFCore.Compiled.Repositories;

internal class BaseRepository
{
    protected readonly AppDbContext _context;

    public BaseRepository(AppDbContext context)
	{
		ArgumentNullException.ThrowIfNull(context, nameof(context));

		_context = context;
	}
}
