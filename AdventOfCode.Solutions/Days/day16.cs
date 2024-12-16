using System.Collections.Immutable;
using System.Diagnostics;

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
        Dictionary<(State, State), ulong> costs = new();
        var pq = new PriorityQueue<(State curr, State prev), ulong>();
        pq.Enqueue(((input.start, Step.E), (input.start, Step.E)), 0);
        Dictionary<State, (HashSet<State>, ulong)> visited = new(); // <Point, Previous Point>
        visited[(input.start, Step.E)] = (new HashSet<State>(), 0);
        ulong costOfShortestPath = ulong.MaxValue;

        // Stop if priority queue is empty
        while (pq.Count>0)
        {
            if (!pq.TryDequeue(out (State curr, State prev) best, out ulong cost))
                break;

            // Optimization to deplete the priority queue: stop searching along a path
            // when cost exceeds the cost of the shortest path because this cannot 
            // be the shortest path. Benchmarking reveals that this optimization yields a 
            // 7% speedup.
            if (cost > costOfShortestPath)
                continue;

            // Handle minimum item from priority queue
            if (best.curr != best.prev)
            {
                if (!visited.ContainsKey(best.curr))
                {
                    costs[best] = cost;
                    visited[best.curr] = ([best.prev], cost);
                }
                else if (cost == visited[best.curr].Item2) // !! Found another shortest path: so record it
                {
                    visited[best.curr].Item1.Add(best.prev);
                    continue;
                }
            }

            // Even though end point has been reached, keep going to find all
            // other shortest paths
            if (best.curr.p == input.end)
            {
                costOfShortestPath = cost; // Optimization
                continue;
            }

            // Next possible actions according to puzzle:

            // Walk one step in current direction
            var step0 = best.curr.s.Rot0();
            var p0 = best.curr.p.Add(step0);
            if (input.map[p0] != '#' && InBounds(p0))
            {
                // Optimization: 
                if (costs.TryGetValue(((p0, step0), best.curr), out var currBest) && cost >= currBest)
                    continue;
                pq.Enqueue(((p0, step0), best.curr), cost + 1);
            }

            // Turn 90 deg
            var step90 = best.curr.s.Rot90();
            var p90 = best.curr.p.Add(step90);
            if (input.map[p90] != '#' && InBounds(p90))
            {
                if (costs.TryGetValue(((p90, step90), best.curr), out var currBest) && cost >= currBest)
                    continue;
                pq.Enqueue(((best.curr.p, step90), best.curr), cost + 1000);
            }

            // Turn 270 deg
            var step270 = best.curr.s.Rot270();
            var p270 = best.curr.p.Add(step270);
            if (input.map[p270] != '#' && InBounds(p270))
            {
                if (costs.TryGetValue(((p270, step270), best.curr), out var currBest) && cost >= currBest)
                    continue;
                pq.Enqueue(((best.curr.p, step270), best.curr), cost + 1000);
            }
        }

        bool InBounds(Point p) => p.r >= 0 && p.c >= 0 && p.r < input.map.ROWS && p.c < input.map.COLS;

        // Back track from End, along all paths to start, and count the distinct locations
        // over all paths
        var dfsStack = new Stack<State>();
        visited.Where(kvp => kvp.Key.p == input.end).Select(kvp => kvp.Key).ToList().ForEach(s => dfsStack.Push(s));
        HashSet<State> uniqueVisited = new(); 
        while (dfsStack.Count > 0)
        {
            var current = dfsStack.Pop();
            if (uniqueVisited.Add(current))
                visited[current].Item1.ToList().ForEach(prev => dfsStack.Push(prev));
        }

        var distinctLocations =  uniqueVisited.Select(state => state.p).Distinct();
        return distinctLocations.Count();
    }
}