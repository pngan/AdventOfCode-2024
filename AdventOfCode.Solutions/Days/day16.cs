using System.Collections.Immutable;
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
        // Eight possible actions
        // Four rotations, four steps

        Dictionary<Point, ulong> costs = new();
        var pq = new PriorityQueue<(Point, (int,int)), ulong>();
        pq.Enqueue((input.start, Step.E), 0);

        while (true)
        {
            // Stop if priority queue is empty
            if (!pq.TryDequeue(out (Point p, (int dr, int dc) s) curr , out ulong cost) || curr.p == input.end)
                break;

            // Ignore if the inspected node has its cost already determined
            if (costs.ContainsKey(curr.p))
                continue;

            // Handle minimum item from priority queue
            costs[(curr.p)] = cost;

            // Enqueue all next possible actions
            foreach (var neighbour in curr.p.Neighbours4().Where(x => input.map[x] == '.' && InBounds(x)))
            {
                ulong stepCost = 0UL;
                var diff = neighbour.Subtract(curr.p);
                if (diff == curr.s)
                    stepCost = 1; // Same direction
                else if (diff == curr.s.Negate())
                    stepCost = 2000; // 2 x 90 deg turns
                else
                    stepCost = 1000; // 90 deg turn

                pq.Enqueue((neighbour, diff), cost + stepCost);
            }
        }

        bool InBounds(Point p) => p.r >= 0 && p.c >= 0 && p.r < input.map.ROWS && p.c < input.map.COLS;

        return costs[input.end];
    }

    protected override object Solve2((CharImage2 map, Point start, Point end) input)
    {
        throw new NotImplementedException();
    }
}