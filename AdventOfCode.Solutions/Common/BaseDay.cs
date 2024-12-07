using System.Collections.Immutable;

using BenchmarkDotNet.Attributes;

namespace AdventOfCode.Solutions.Common;

public abstract class BaseDay
{
    public abstract string? Solve1();
    public abstract string? Solve2();
}

public abstract class BaseDay<TInput> : BaseDay
{
    const int Year = 2024;

    protected abstract int DayNumber { get; }

    protected abstract TInput Parse(ImmutableArray<string> input);

    private ImmutableArray<string> _input;

    protected BaseDay()
    {
        List<string> lines = [.. File.ReadAllLines($@"Inputs\{Year}_{DayNumber:00}_input.txt")];
        while (true)
        {
            if (string.IsNullOrWhiteSpace(lines.Last()))
                lines.RemoveAt(lines.Count() - 1);
            else
                break;
        }
        _input = lines.ToImmutableArray();
    }

    [Benchmark]
    public override string? Solve1() => Solve1(Parse(_input)).ToString();

    protected abstract object Solve1(TInput input);

    [Benchmark]
    public override string? Solve2() => Solve2(Parse(_input)).ToString();

    protected abstract object Solve2(TInput input);
}