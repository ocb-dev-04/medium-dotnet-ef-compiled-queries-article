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
    public async Task Init()
    {
        DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseInMemoryDatabase(nameof(AppDbContext));
        optionsBuilder.UseLazyLoadingProxies();

        _context = new AppDbContext(optionsBuilder.Options);
        var user = await _context.Users.ToListAsync();
        var shops = await _context.Shops.ToListAsync();
        Console.WriteLine("All init");
    }

    #endregion

    [Benchmark]
    public void Ex() { }

    [Benchmark]
    public void Ex_Compiled() { }
}
