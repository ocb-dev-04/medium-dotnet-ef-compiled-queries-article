using System.Linq.Expressions;

using BenchmarkDotNet.Running;
using BenchmarkDotNet.Attributes;

BenchmarkRunner.Run<MainBenchmark>();

[MemoryDiagnoser]
public class MainBenchmark
{
    #region Props

    private IQueryable<Person> _persons;

    private static readonly Expression<Func<Person, bool>> s_ageExpression = e => e.Age == 23;
    private static readonly Func<Person, bool> s_ageExpressionCompiled = s_ageExpression.Compile();

    #endregion

    #region Init

    [GlobalSetup]
    public void Init()
    {
        _persons = new List<Person> { new("Batman", 81), new("Superman", 9998), new("Catwoman", 23) }
                    .AsQueryable();
    }

    #endregion

    [Benchmark]
    public void Ex() => _persons.Where(s_ageExpression).SingleOrDefault();

    [Benchmark]
    public void Ex_Compiled() => _persons.Where(s_ageExpressionCompiled).SingleOrDefault();
}

public record class Person(string Name, int Age);