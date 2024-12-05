using System.Collections.Immutable;

using AdventOfCode.Solutions.Common;
using AdventOfCode.Solutions.Extensions;

namespace AdventOfCode.Solutions.Days;
using Map = HashSet<(long r, long c)>;
using MSPair = ((long r, long c) mdir, (long r, long c) sdir);

public class Day04 : BaseDay<(Map X, Map M, Map A, Map S)>
{
    protected override int DayNumber => 04;

    protected override (Map X, Map M, Map A, Map S) Parse(ImmutableArray<string> input)
    {
        Map X = new();
        Map M = new();
        Map A = new();
        Map S = new();
        for (long r = 0; r < input.Length; r++)
            for (long c = 0; c < input[(int)r].Length; c++)
            {
                char ch = input[(int)r][(int)c];
                switch (ch)
                {
                    case 'X': X.Add((r, c)); break;
                    case 'M': M.Add((r, c)); break;
                    case 'A': A.Add((r, c)); break;
                    case 'S': S.Add((r, c)); break;
                }
            }
        return (X, M, A, S);
    }

    (long dr, long dc)[] Dirs() => [(-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1)];


    protected override object Solve1((Map X, Map M, Map A, Map S)  input)
    {
        long count = 0;
        foreach (var x in input.X) // Foreach X
        {
            // Get Dirs of adjacent M's
            List<(long r, long c)> dirs = new();
            foreach (var d in Dirs())
            {
                if (input.M.Contains(x.Add(d)))
                {
                    dirs.Add(d);
                }
            }

            // Search along dirs for A's and S's
            foreach (var d in dirs)
            {
                var offset = x.Add(d).Add(d);
                if (input.A.Contains(offset))
                    if (input.S.Contains(offset.Add(d)))
                        count++;
            }
        }

        return count;
    }

    protected override object Solve2((Map X, Map M, Map A, Map S) input)
    {
        // Considered using rotation matrices to do clockwise and counter-clockwise variations of M-S locations
        // but decided to keep it simple and precomputed the deltas.

        var search = new Dictionary<MSPair, MSPair[]> ();
        search[((-1,-1), ( 1, 1))] = [((-1, 1), ( 1,-1)), (( 1,-1), (-1, 1))]; // M at top-left, S at bottom-right
        search[((-1, 1), ( 1,-1))] = [(( 1, 1), (-1,-1)), ((-1,-1), ( 1, 1))]; // M at top-right, S at bottom-left
        search[(( 1,-1), (-1, 1))] = [((-1,-1), ( 1, 1)), (( 1, 1), (-1,-1))]; // M at bottom-left, S at top-right
        search[(( 1, 1), (-1,-1))] = [(( 1,-1), (-1, 1)), ((-1, 1), ( 1,-1))]; // M at bottom-right, S at top-left
        long count = 0;
        foreach (var a in input.A) // Visit each A
        {
            foreach (var s in search) // Search through each possible valid configuration for M-S pair adjacent to A
            {
                var delta = s.Key;
                if (input.M.Contains(a.Add(delta.mdir)) && input.S.Contains(a.Add(delta.sdir)))
                {
                    var cwDelta = s.Value[0];  // Clockwise rotation
                    var ccwDelta = s.Value[1]; // Counter clockwise rotation
                    if ((input.M.Contains(a.Add(cwDelta.mdir)) && input.S.Contains(a.Add(cwDelta.sdir)))
                        || (input.M.Contains(a.Add(ccwDelta.mdir)) && input.S.Contains(a.Add(ccwDelta.sdir))))
                    {
                        count++;
                        break;  // We are done with this 'A'. Move to next 'A'.
                    }
                }
            }
        }
        return count;
    }
}