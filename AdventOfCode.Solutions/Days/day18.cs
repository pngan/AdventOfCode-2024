using System.Collections.Immutable;

using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions.Days;

public class Day18 : BaseDay<IEnumerable<(int r, int c)>>
{
    protected override int DayNumber => 18;
    const int takeN = 1024;
    const int end = 70;

    protected override IEnumerable<(int r, int c)> Parse(ImmutableArray<string> input)
    {
        foreach(var line in input)
        {
            var vals = line.Split(",").Select(int.Parse).ToArray();
            yield return (vals[0], vals[1]);
        }
    }

    protected override object Solve1(IEnumerable<(int r, int c)> input)
    {
        return ShortestPath(input, takeN, end);
    }

    protected override object Solve2(IEnumerable<(int r, int c)> raw)
    {
        var input = raw.ToArray();
        for (var i = takeN; i < input.Count(); i++)
        {
            if (ShortestPath(input, i, end) is -1)
            {
                return input[i-1];
            }
        }

        return -123;
    }

    private int ShortestPath(IEnumerable<(int r, int c)> input, int take, int end)
    {
        var visited = input.Take(take).ToHashSet(); // Treat bad bytes as visited
        var pend = (r: end, c: end);

        // Dikjstra's algorithm with constant weights is equivalent to BFS
        var q = new Queue<((int r, int c) p, int cost)>();
        q.Enqueue(((0, 0), 0));

        while (q.Any())
        {
            var (p, cost) = q.Dequeue();
            if (p == pend)
                return cost;
            foreach (var d in new[] { (0, 1), (1, 0), (0, -1), (-1, 0) })
            {
                var np = (r: p.r + d.Item1, c: p.c + d.Item2);
                if (np.r < 0 || np.r > end || np.c < 0 || np.c > end || visited.Contains(np))
                    continue;
                q.Enqueue((np, cost + 1));
                visited.Add(np);
            }
        }

        return -1;
    }
}