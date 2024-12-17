using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;

using AdventOfCode.Solutions.Common;

using AdventureOfCode.Utilities.Image;
namespace AdventOfCode.Solutions.Days;

using Point = (int r, int c);
using State = ((int r, int c) p, (int dr, int dc) s);

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
        Dictionary<(State, State), ulong> costs = new();
        var pq = new PriorityQueue<(State curr, State prev), ulong>();
        pq.Enqueue(((input.start, Step.E), (input.start, Step.E)), 0);
        Dictionary<State, State> visited = new(); // <Point, Previous Point>

        while (true)
        {
            // Stop if priority queue is empty
            if (!pq.TryDequeue(out (State curr, State prev) best, out ulong cost) )
                break;

            // Ignore if the inspected node has already been visited
            if (visited.ContainsKey((best.curr)))
                continue;

            // Handle minimum item from priority queue
            costs[best] = cost;
            visited[best.curr] = best.prev;

            HashSet<Point> uniqueLocations = new();
            if (best.curr.p == input.end)
            {
                var prev = visited[best.curr];
                uniqueLocations.Add(best.prev.p);
                while (prev != (input.start, Step.E))
                {
                    uniqueLocations.Add((prev.p));
                    prev = visited[(prev)];
                }
                Console.WriteLine(uniqueLocations.Count);
                return cost;
            }
            // Next possible actions according to puzzle:

            // Walk one step in current direction
            var step0 = best.curr.s.Rot0();
            var p0 = best.curr.p.Add(step0);
            if (input.map[p0] != '#' && InBounds(p0))
                pq.Enqueue(((p0, step0), best.curr), cost + 1);

            // Turn 90 deg
            var step90 = best.curr.s.Rot90();
            pq.Enqueue(((best.curr.p, step90), best.curr), cost + 1000);

            // Turn 270 deg
            var step270 = best.curr.s.Rot270();
            pq.Enqueue(((best.curr.p, step270), best.curr), cost + 1000);
        }

        bool InBounds(Point p) => p.r >= 0 && p.c >= 0 && p.r < input.map.ROWS && p.c < input.map.COLS;

        throw new UnreachableException("Did not find end location.");
    }

    protected override object Solve2((CharImage2 map, Point start, Point end) input)
    {
        throw new NotImplementedException();
    }
}