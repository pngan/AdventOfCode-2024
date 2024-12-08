using System.Collections.Immutable;

using AdventOfCode.Solutions.Common;
using AdventOfCode.Solutions.Extensions;

namespace AdventOfCode.Solutions.Days;

using Point = (int r, int c);
using Delta = (int dr, int dc);

public class Day08 : BaseDay<(Dictionary<char, HashSet<Point>> antenna, int rows, int cols) >
{
    protected override int DayNumber => 8;

    protected override (Dictionary<char, HashSet<(int r, int c)>> antenna, int rows, int cols) Parse(ImmutableArray<string> input)
    {
        Dictionary<char, HashSet<(int r, int c)>> antennna = new();
        int columns = 0;
        for (int r = 0; r < input.Length; r++)
        {
            for (int c = 0; c < input[r].Length; c++)
            {
                columns = input[r].Length;
                char v = input[r][c];
                if (v == '.') continue; // Ignore empty locations
                if (!antennna.ContainsKey(v))
                    antennna[v] = new HashSet<Point>();
                antennna[v].Add((r, c));
            }
        }
        return (antennna, input.Length, columns);
    }

    protected override object Solve1((Dictionary<char, HashSet<(int r, int c)>> antenna, int rows, int cols) input)
    {
        bool OutOfBounds(Point p) => p.r < 0 || p.c < 0 || p.r >= input.rows || p.c >= input.cols;

        HashSet<Point> antinodes = new();

        foreach(var antenna in input.antenna)
        {
            var pairsAndDiff = antenna.Value.DistinctPairs<Point, Point, ((Point a, Point b) points, Delta delta)>(antenna.Value, (a, b) => ((a, b), Diff(a, b)));
            foreach (var pair in pairsAndDiff)
            {
                var node = Add(pair.points.a, pair.delta);
                if (!OutOfBounds(node))
                    antinodes.Add(node);
                node = Subtract(pair.points.b, pair.delta);
                if (!OutOfBounds(node))
                    antinodes.Add(node);
            }
        }
        return antinodes.Count;
    }

    protected override object Solve2((Dictionary<char, HashSet<(int r, int c)>> antenna, int rows, int cols) input)
    {
        bool OutOfBounds(Point p) => p.r < 0 || p.c < 0 || p.r >= input.rows || p.c >= input.cols;

        HashSet<Point> antinodes = new();

        foreach (var antenna in input.antenna)
        {
            var pairsAndDiff = antenna.Value.DistinctPairs<Point, Point, ((Point a, Point b) points, Delta delta)>(antenna.Value, (a, b) => ((a, b), Diff(a, b)));
            foreach (var pair in pairsAndDiff)
            {
                // Add harmonics in postive direction
                var node = pair.points.a;
                while (!OutOfBounds(node))
                {
                    antinodes.Add(node);
                    node = Add(node, pair.delta);
                }

                // Add harmonics in negative direction
                node = pair.points.b;
                while (!OutOfBounds(node))
                {
                    antinodes.Add(node);
                    node = Subtract(node, pair.delta);
                }
            }
        }
        return antinodes.Count;
    }

    private static Point Subtract(Point p, Delta d) => (p.r - d.dr, p.c - d.dc);
    private static Point Add(Point p, Delta d) => (p.r + d.dr, p.c + d.dc);
    private static Delta Diff(Point a, Point b) => (a.r-b.r,a.c-b.c);
    private static Delta Negate(Delta d) => (-d.dr, -d.dc);

}