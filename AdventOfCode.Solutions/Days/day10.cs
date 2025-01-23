using System.Collections.Immutable;

using AdventOfCode.Solutions.Common;

using AdventureOfCode.Utilities.Image;

namespace AdventOfCode.Solutions.Days;
using Point = (int r, int c);
public class Day10 : BaseDay<Dictionary<Point, int>>
{
    protected override int DayNumber => 10;

    protected override Dictionary<(int r, int c), int> Parse(ImmutableArray<string> input)
    {
        int R = input.Length;
        int C = input.First().Length;
        Dictionary<Point, int> result = new();

        for (int r = 0; r < R; r++)
            for (int c = 0; c < C; c++)
            {
                var v = input[r][c];
                result[(r, c)] = v == '.' ? -1 : v- '0';
            }
        return result;
    }

    protected override object Solve1(Dictionary<(int r, int c), int> input) => Solve(input, 1);


    protected override object Solve2(Dictionary<(int r, int c), int> input) => Solve(input, 2);

    private
    int Solve(Dictionary<(int r, int c), int> input, int part)
    {
        var trailheads = input.Where(kvp => kvp.Value == 0).Select(kvp => kvp.Key).ToList();
        Dictionary<Point, List<Point>> paths = new();
        Queue<(Point, int)> q = new();
        foreach (var head in trailheads)
        {
            q.Enqueue((head, 0));
            paths[head] = new();

            while (q.Any())
            {
                var (cur, val) = q.Dequeue();
                int nextval = val + 1;

                foreach (var next in cur.Neighbours4())
                {
                    if (input.ContainsKey(next) && input[next] == nextval)
                    {
                        if (nextval == 9)
                        {
                            paths[head].Add(next);
                            continue;
                        }
                        q.Enqueue((next, nextval));
                    }
                }
            }
            q.Clear();
        }

        var result = paths.Sum(p => part == 1 ? p.Value.Distinct().Count() : p.Value.Count());
        return result;
    }
}