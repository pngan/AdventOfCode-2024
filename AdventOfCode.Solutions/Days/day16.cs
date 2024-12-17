using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;

using AdventOfCode.Solutions.Common;

using AdventureOfCode.Utilities.Image;
namespace AdventOfCode.Solutions.Days;

using Point = (int r, int c);

public class Day16 : BaseDay<(CharImage2 map, Point start, Point end)>
{
    protected override int DayNumber => 16;

    protected override (CharImage2 map, Point start, Point end) Parse(ImmutableArray<string> input)
    {
        var map = CharImage2.Parse(input);
        var start = map.Find('S').Single();
        var end = map.Find('E').Single();
        return (map, start, end);
    }

    protected override object Solve1((CharImage2 map, Point start, Point end) input)
    {
        Dictionary<(Point, (int dr, int dc), Point, (int dr, int dc)), ulong> costs = new();
        var pq = new PriorityQueue<(Point, (int,int), Point, (int,int)), ulong>();
        pq.Enqueue((input.start, Step.E, input.start, Step.E), 0);
        Dictionary<(Point, (int dr, int dc)), ((int dr, int dc), Point)> path = new(); // <Point, Previous Point>

        while (true)
        {
            // Stop if priority queue is empty
            if (!pq.TryDequeue(out (Point p, (int dr, int dc) s, Point prevPoint, (int dr, int dc) prevStep) curr, out ulong cost) )
                break;

            // Ignore if the inspected node has its cost already determined
            if (costs.ContainsKey(curr))
                continue;

            // Handle minimum item from priority queue
            costs[curr] = cost;
            path[(curr.prevPoint, curr.prevStep)] = (curr.p, curr.s);

            if (curr.p == input.end)
            {

                var next = path[(input.start, Step.E)];
                while(next.Item1 != input.end)
                {
                    next = path[(next)];
                }


                return cost;
            }
            // Next possible actions according to puzzle:

            // Walk one step in current direction
            var step0 = curr.s.Rot0();
            var p0 = curr.p.Add(step0);
            if (input.map[p0] != '#' && InBounds(p0))
                pq.Enqueue((p0, step0, curr.p, curr.s), cost + 1);

            // Turn 90 deg
            var step90 = curr.s.Rot90();
            pq.Enqueue((curr.p, step90, curr.p, curr.s), cost + 1000);

            // Turn 270 deg
            var step270 = curr.s.Rot270();
            pq.Enqueue((curr.p, step270, curr.p, curr.s), cost + 1000);
        }

        bool InBounds(Point p) => p.r >= 0 && p.c >= 0 && p.r < input.map.ROWS && p.c < input.map.COLS;

        throw new UnreachableException("Did not find end location.");
    }

    protected override object Solve2((CharImage2 map, Point start, Point end) input)
    {
        throw new NotImplementedException();
    }
}