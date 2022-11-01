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

    #region Get By Id

    [Benchmark]
    public async Task<Shop> GetbyId_GenericAsync()
        => await _genericRepository.GetById(shopId);

    [Benchmark]
    public async Task<Shop> GetbyId_DedicatedAsync()
        => await _shopRepository.GetById(shopId);

    [Benchmark]
    public Shop GetbyId_Dedicated_Compiled()
        => _shopRepository.GetByIdCompiled(shopId);

    [Benchmark]
    public async Task<Shop> GetbyId_Dedicated_CompiledAsync()
        => await _shopRepository.GetByIdCompiledAsync(shopId);

    #endregion

    #region Get Collection

    [Benchmark]
    public async Task<HashSet<Shop>> GetbyCollection_GenericAsync()
        => await _genericRepository.GetCollection();

    [Benchmark]
    public async Task<HashSet<Shop>> GetbyCollection_DedicatedAsync()
        => await _shopRepository.GetCollection();

    [Benchmark]
    public HashSet<Shop> GetbyCollection_Dedicated_Compiled()
        => _shopRepository.GetCollectionCompiled();

    [Benchmark]
    public async Task<HashSet<Shop>> GetbyCollection_Dedicated_CompiledAsync()
        => await _shopRepository.GetCollectionCompiledAsync();

    #endregion
}
