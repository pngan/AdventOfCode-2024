using System.Collections.Immutable;

using MoreLinq;
using AdventOfCode.Solutions.Common;
using AdventOfCode.Solutions.Extensions;

namespace AdventOfCode.Solutions.Days;

public class Day19 : BaseDay<(IEnumerable<string> towels, IEnumerable<string> patterns)>
{
    protected override int DayNumber => 19;

    protected override (IEnumerable<string>, IEnumerable<string>) Parse(ImmutableArray<string> input) 
        => input.Split("").Fold((towels, patterns) => (towels.First().Split(",").Select(x=>x.Trim()), patterns));

    protected override object Solve1((IEnumerable<string> towels, IEnumerable<string> patterns) input)
    {
        int count = 0;
        Dictionary<string, ulong> memo = new(); // One memo for all patterns to avoid recalculating
        foreach (var pattern in input.patterns)
            count += Parse(pattern, input.towels.ToArray(), memo) > 0 ? 1 : 0;
        return count;
    }

    protected override object Solve2((IEnumerable<string> towels, IEnumerable<string> patterns) input)
    {
        ulong count = 0;
        Dictionary<string, ulong> memo = new(); // One memo for all patterns to avoid recalculating
        foreach (var pattern in input.patterns)
            count += Parse(pattern, input.towels.ToArray(), memo);
        return count;
    }

    private ulong Parse(string pattern, string[] towels, Dictionary<string, ulong> count)
    {
        if (count.ContainsKey(pattern))
            return count[pattern];
        if (pattern.Length == 0)
            return 1;
        ulong total = 0;
        foreach (var towel in towels)
        {
            if (pattern.StartsWith(towel))
            {
                var residual = pattern[towel.Length..];
                total += Parse(residual, towels, count);
            }
        }
        count[pattern] = total;
        return total;
    }
}