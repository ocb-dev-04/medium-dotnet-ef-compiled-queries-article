using System.Linq.Expressions;

using BenchmarkDotNet.Running;
using BenchmarkDotNet.Attributes;
using EF.Compiled;
using Microsoft.EntityFrameworkCore;

BenchmarkRunner.Run<MainBenchmark>();

[MemoryDiagnoser]
public class MainBenchmark
{
    #region Props

    private AppDbContext _context;

    private static readonly Expression<Func<User, bool>> s_ageExpression = e => e.FullName.Equals("Fulgencio");
    private static readonly Func<User, bool> s_ageExpressionCompiled = s_ageExpression.Compile();

    #endregion

    #region Init

    [GlobalSetup]
    public void Init()
    {
        DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseInMemoryDatabase(nameof(AppDbContext));
        optionsBuilder.UseLazyLoadingProxies();

        _context = new AppDbContext(optionsBuilder.Options);
        
    }

    #endregion

    [Benchmark]
    public async Task Ex() {
        await _context.Shops.Where(w => w.Name.Contains("A")).ToListAsync();
    }

    [Benchmark]
    public async Task Ex_Compiled() {
        await _context.Users.Where(w => w.Email.Contains("@")).ToListAsync();
    }
}
