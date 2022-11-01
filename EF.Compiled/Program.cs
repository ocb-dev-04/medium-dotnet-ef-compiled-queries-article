using EFCore.Compiled;

using BenchmarkDotNet.Running;
using BenchmarkDotNet.Attributes;

using EFCore.Compiled.Repositories;

BenchmarkRunner.Run<MainBenchmark>();

[MemoryDiagnoser]
public class MainBenchmark
{
    #region Props

    private GenericRepository<Shop> _genericRepository;
    private ShopRepository _shopRepository;

    private Guid shopId;

    #endregion

    #region Init

    [GlobalSetup]
    public async Task Init()
    {
        AppDbContext _context = await Initializer.Run();
        _genericRepository = new GenericRepository<Shop>(_context);
        _shopRepository = new ShopRepository(_context);

        shopId = (await _genericRepository.GetCollection()).First().Id;
    }

    #endregion

    #region Generic Repository

    [Benchmark]
    public async Task<List<Shop>> GetCollection_GenericAsync()
        => await _genericRepository.GetCollection();

    [Benchmark]
    public List<Shop> GetCollection_Generic_CompiledSync()
        => _genericRepository.GetCollection_Compiled();

    [Benchmark]
    public async Task<List<Shop>> GetCollection_Generic_CompiledAsync()
        => await _genericRepository.GetCollection_CompiledAsync();

    #endregion

    #region Dedicated Repository

    [Benchmark]
    public async Task<List<Shop>> GetCollection_DedicatedAsync()
        => await _shopRepository.GetCollection();

    [Benchmark]
    public List<Shop> GetCollection_Dedicated_CompiledSync()
        => _shopRepository.GetCollection_Compiled();

    [Benchmark]
    public async Task<List<Shop>> GetCollection_Dedicated_CompiledAsync()
        => await _shopRepository.GetCollection_CompiledAsync();

    #endregion
}
