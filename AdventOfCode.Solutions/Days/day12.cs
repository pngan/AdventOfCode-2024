using System.Collections.Immutable;
using System.Numerics;

using AdventOfCode.Solutions.Common;
using AdventOfCode.Solutions.Extensions;

namespace AdventOfCode.Solutions.Days;

public class Day12 : BaseDay<ImmutableArray<string>>
{
    protected override int DayNumber => 12;
    int ROWS;
    int COLS;

    protected override ImmutableArray<string> Parse(ImmutableArray<string> input)
    {
        ROWS = input.Length;
        COLS = input.First().Length;
        return input;
    }

    protected override object Solve1(ImmutableArray<string> input)
    {
        List<List<bool>> visited = new();
        for (int r = 0; r < ROWS; r++)
        {
            visited.Add(Enumerable.Repeat(false, COLS).ToList());
        }

        List<List<uint>> edgeCount = new();
        for (int r = 0; r < ROWS; r++)
        {
            edgeCount.Add(Enumerable.Repeat(0u, COLS).ToList());
        }

        for (int r = 0; r < ROWS; r++)
        {
            for (int c = 0; c < COLS; c++)
            {
                uint edges = 0;
                var value = input[r][c];

                var p = N(r, c);
                if (p.r < 0 || input[p.r][p.c] != value) edges++;
                p = S(r, c);
                if (p.r >= ROWS) edges++;
                if (input[p.r][p.c] != value) edges++;
                p = W(r, c);
                if (p.c < 0 || input[p.r][p.c] != value) edges++;
                p = E(r, c);
                if (p.c >= COLS || input[p.r][p.c] != value) edges++;

                edgeCount[r][c] = edges;
            }
        }

        (int r, int c) N(int r, int c) => (r - 1, c);
        (int r, int c) S(int r, int c) => (r + 1, c);
        (int r, int c) W(int r, int c) => (r, c-1);
        (int r, int c) E(int r, int c) => (r, c+1);

        return 0;
    }

    protected override object Solve2(ImmutableArray<string> input)
    {
        throw new NotImplementedException();
    }
}