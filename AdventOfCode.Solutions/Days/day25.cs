using System.Collections.Immutable;

using AdventOfCode.Solutions.Common;
using AdventOfCode.Solutions.Extensions;

using MoreLinq;
namespace AdventOfCode.Solutions.Days;


public class Day25 : BaseDay<(IEnumerable<int[]>, IEnumerable<int[]>)>
{
    protected override int DayNumber => 25;

    protected override (IEnumerable<int[]>, IEnumerable<int[]>) Parse(ImmutableArray<string> input)
    {
        List<int[]> locks = new();
        List<int[]> keys = new();

        IEnumerable<IEnumerable<string>> blocks = input.Split("");
        foreach (var block in blocks)
        {
            int[] values = [-1,-1,-1,-1,-1];
            foreach (var line in block)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] == '#')
                    {
                        values[i]++;
                    }
                }
            }
            if (block.First().First() == '#')
            {
                locks.Add(values);
            }
            else
            {
                keys.Add(values);
            }

        }
        return (locks, keys);
    }

    protected override object Solve1((IEnumerable<int[]>, IEnumerable<int[]>) input)
    {
        int result = 0;
        foreach (var l in input.Item1)
        {
            foreach (var k in input.Item2)
            {
                bool fit = true;
                for (int i = 0; i < k.Length;i++)
                {
                    if (l[i] + k[i] > 5)
                    {
                        fit = false; break;
                    }
                }
                if (fit)
                    result++;
            }
        }

        return result;
    }

    protected override object Solve2((IEnumerable<int[]>, IEnumerable<int[]>) input) => 0;
}