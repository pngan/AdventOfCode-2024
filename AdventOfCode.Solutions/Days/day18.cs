using System.Collections.Immutable;

using AdventOfCode.Solutions.Common;

using AdventureOfCode.Utilities.Image;

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

    protected override object Solve1(IEnumerable<(int r, int c)> input) => ShortestPath(input, takeN);

    protected override object Solve2(IEnumerable<(int r, int c)> raw)
    {
        var input = raw.ToArray();
        for (var i = takeN; i < input.Count(); i++)
            if (ShortestPath(input, i) is -1)
                return input[i-1];
        return -123;
    }

    private int ShortestPath(IEnumerable<(int r, int c)> input, int take)
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
            foreach (var np in p.Neighbours4())
            {
                if (np.r < 0 || np.r > end || np.c < 0 || np.c > end || visited.Contains(np))
                    continue;
                q.Enqueue((np, cost + 1));
                visited.Add(np);
            }
        }

        return -1;
    }
}