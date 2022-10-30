using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using EF.Compiled;

using BenchmarkDotNet.Running;
using BenchmarkDotNet.Attributes;

BenchmarkRunner.Run<MainBenchmark>();

[MemoryDiagnoser]
public class MainBenchmark
{
    #region Props

    private AppDbContext _context;

    private static readonly Expression<Func<Shop, bool>> s_NameExpression = e => e.Name.ToLower().Contains("a");
    private static readonly Func<Shop, bool> s_NameExpressionCompiled = s_NameExpression.Compile();

    #endregion

    #region Init

    [GlobalSetup]
    public async Task Init()
    {
        _context = await Initializer.Run();
    }

    #endregion

    [Benchmark]
    public async Task Ex()
    {
        await _context.Shops.Where(s_NameExpression).ToListAsync();
    }

    [Benchmark]
    public async Task Ex_Compiled()
    {
        _context.Shops.Where(s_NameExpressionCompiled).ToList();
    }

    [Benchmark]
    public async Task Ex_SelectFirst()
    {
        await _context.Shops.Select(s => new Shop { Name = s.Name }).Where(s_NameExpression).ToListAsync();
    }

    [Benchmark]
    public async Task Ex_Compiled_SelectFirst()
    {
        _context.Shops.Select(s => new Shop { Name = s.Name }).Where(s_NameExpressionCompiled).ToList();
    }
    
    [Benchmark]
    public async Task Ex_SelectLast()
    {
        await _context.Shops.Where(s_NameExpression).Select(s => new Shop { Name = s.Name }).ToListAsync();
    }

    [Benchmark]
    public async Task Ex_Compiled_SelectLast()
    {
        _context.Shops.Where(s_NameExpressionCompiled).Select(s => new { Name = s.Name }).ToList();
    }
}
