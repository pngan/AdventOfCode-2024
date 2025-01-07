using AdventOfCode.Solutions.Common;

using AdventureOfCode.Utilities.Image;

using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace AdventOfCode.Solutions.Days;

public class Day20 : BaseDay<(Collection<(int r, int c)> path, Dictionary<(int r, int c), int> pivotPath, (int r, int c) start, (int r, int c) end)> // pos, index
{
    protected override int DayNumber => 20;

    protected override (Collection<(int r, int c)> path, Dictionary<(int r, int c), int> pivotPath, (int r, int c) start, (int r, int c) end) Parse(ImmutableArray<string> input)
    {
        var start = (r: 0, c: 0);
        var end = (r: 0, c: 0);
        for (int r = 0; r < input.Length; r++)
            for (int c = 0; c < input[r].Length; c++)
                if (input[r][c] == 'S')
                    start = (r, c);
                else if (input[r][c] == 'E')
                    end = (r, c);

        var p = start;
        var index = 0;
        Dictionary<(int r, int c), int> pivotPath = new() { [p] = index++ };
        Collection<(int r, int c)> path = new() { p };
        while (p != end)
            foreach (var np in p.Neighbours4())
                if ((input[np.r][np.c] == '.' || input[np.r][np.c] == 'E') && !pivotPath.ContainsKey(np))
                {
                    p = np;
                    pivotPath[p] = index++;
                    path.Add(p);
                    break;
                }
        return (path, pivotPath, start, end);
    }

    protected override object Solve1((Collection<(int r, int c)> path, Dictionary<(int r, int c), int> pivotPath, (int r, int c) start, (int r, int c) end) input)
    {
        Dictionary<int, int> cheats = new(); // picosecond, count
        foreach (var p in input.path)
        {
            int idx = input.pivotPath[p];
            foreach (var cheat in new ((int r, int c) trial, (int r, int c) intermediate)[]
            {   ((p.r - 2, p.c),(p.r - 1, p.c)),
                ((p.r + 2, p.c),(p.r + 1, p.c)),
                ((p.r, p.c - 2),(p.r, p.c - 1)),
                ((p.r, p.c + 2),(p.r, p.c + 1))
            })
            {
                if (input.pivotPath.TryGetValue(cheat.trial, out int cheatIdx))
                {
                    // Cheats must have an intermediate wall. If not, it's not a cheat
                    if (input.pivotPath.ContainsKey(cheat.intermediate))
                        continue;

                    int gain = cheatIdx - idx - 2;
                    if (gain <= 0) continue;

                    if (cheats.ContainsKey(gain))
                        cheats[gain]++;
                    else
                        cheats[gain] = 1;
                }
            }
        }

        int numberOfCheats = 0;
        foreach (var picoseconds in cheats.Keys.OrderByDescending(x => x))
        {
            if (picoseconds >= 100)
                numberOfCheats += cheats[picoseconds];
        }

        return numberOfCheats;
    }

    protected override object Solve2((Collection<(int r, int c)> path, Dictionary<(int r, int c), int> pivotPath, (int r, int c) start, (int r, int c) end) input)
    {
        return EvaluateCheats(input.path, 20);
    }

    private int EvaluateCheats(Collection<(int r, int c)> path, int maxCheatLength)
    {
        // for every point b on path
        //   for every other point e on path
        //     if dist(b,e) <= 20
        //       index(e) - index(b) - dist(b,e)   where dist = Manhattan distance
        int numCheats = 0;
        for (int b = 0; b < path.Count; b++)
            for (int e = b + 1; e < path.Count; e++)
            {
                var dist = PointExtensions.ManhattanDistance(path[b], path[e]);
                if (dist > 20) continue;
                int gain = e - b - dist;
                if (gain >= 100) numCheats++;
            }
        return numCheats;
    }
}