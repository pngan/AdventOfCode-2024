using System.Collections.Immutable;
using System.Data;
using System.Formats.Asn1;
using System.Numerics;
using System.Runtime.ExceptionServices;

using AdventOfCode.Solutions.Common;
using AdventOfCode.Solutions.Extensions;

namespace AdventOfCode.Solutions.Days;

using static System.Net.Mime.MediaTypeNames;

using Point = (int r, int c);
using Equivalence = (int to, int from);
public class Day12 : BaseDay<(Dictionary<Point, char> image, int rows, int cols)>
{
    protected override int DayNumber => 12;

    protected override (Dictionary<Point, char> image, int rows, int cols) Parse(ImmutableArray<string> input)
    {
        Dictionary<Point, char> result = new();
        int r = 0;
        int c = 0;

        foreach (var row in input)
        {
            c = 0;
            foreach (var ch in row)
            {
                result[(r, c)] = ch;
                c++;
            }
            r++;
        }
        return (result,r,c);
    }



    //protected override object Solve1((Dictionary<Point, char> image, int rows, int cols) input)
    //{
    //    int nextLabel = 0;
    //    int currLabel = 0;

    //    Dictionary<Point, int> label = new();
    //    List<HashSet<int>> equivalence = new();
    //    List<int> area = new();
    //    List<int> side = new();
    //    List<char> value = new();

    //    for (int r = 0; r < input.rows; r++)
    //    {
    //        char currPixel = '\0';
    //        for (int c = 0; c < input.cols; c++)
    //        {
    //            Point p = (r, c);
    //            if (input.image[p] != currPixel)
    //            {
    //                currPixel = input.image[p];
    //                currLabel = nextLabel;
    //                area.Add(0);
    //                side.Add(0);
    //                value.Add('\0');
    //                nextLabel++;
    //            }

    //            // Update Label
    //            label[p] = currLabel;

    //            // Update equivalence
    //            var aboveLocation = (p.r - 1, p.c);
    //            if (input.image.TryGetValue(aboveLocation, out char abovePixel) && currPixel == abovePixel)
    //            {
    //                var hss = equivalence.Where(hs => hs.Contains(label[aboveLocation]));
    //                if (hss == null)
    //                    throw new ApplicationException("Did not find equivalence");
    //                var firstEquivalenceSet = hss.First();
    //                firstEquivalenceSet.Add(currLabel);

    //                var otherEquivalenceSets = hss.Skip(1).ToList();
    //                foreach (var hs in otherEquivalenceSets)
    //                {
    //                    firstEquivalenceSet.UnionWith(hs);
    //                }

    //                var removed = equivalence.Except(otherEquivalenceSets).ToList();
    //                equivalence = removed;
    //            }
    //            else
    //            {
    //                equivalence.Add([currLabel]);
    //            }

    //            // Increment Area
    //            area[currLabel]++;

    //            // Increment Sides
    //            side[currLabel] += NonMatchingNeighbours(input.image, p);
    //        }
    //    }

    //    // Resolve the equivalences
    //    foreach (var hs in equivalence)
    //    {
    //        var equivs = hs.ToList();
    //        var root = equivs[0];
    //        for (int i = 1; i < equivs.Count; i++)
    //        {
    //            area[root] += area[i];
    //            side[root] += side[i];
    //            area[i] = 0;
    //            side[i] = 0;
    //        }
    //    }

    //    // Calculate Cost
    //    int cost = 0;
    //    for (int i = 0; i < area.Count; i++)
    //    {
    //        cost += area[i] * side[i];
    //    }

    //    return cost;
    //}
    protected override object Solve1((Dictionary<Point, char> image, int rows, int cols) input)
    {
        List<(ulong area, ulong sides)> results = [];
        Dictionary<Point, bool> visited = new();
        foreach (Point pt in input.image.Keys)
        {
            visited[pt] = false;
        }

        Queue<Point> queue = new();
        for (int r = 0; r < input.rows; r++)
        {
            for (int c = 0; c < input.cols; c++)
            {
                Point p = (r, c);
                var region = (0UL, 0UL);
                if (!visited[p])
                {
                    queue.Enqueue(p);
                    visited[p] = true;

                    while (queue.Any())
                    {
                        var pp = queue.Dequeue();
                        var neighbours = MatchingNonVisitedNeighbours(input.image, visited, pp).ToList();
                        foreach (var neighbour in neighbours)
                        {
                            queue.Enqueue(neighbour);
                            visited[neighbour] = true;
                        }

                        region = (region.Item1+1, region.Item2 + NonMatchingNeighbours(input.image, pp));
                    }

                }
                results.Add(region);
            }
        }
        // Calculate Cost
        ulong cost = 0;

        foreach (var r in results)
        {
            cost += r.area * r.sides;
        }
        return cost;

        IEnumerable<Point> MatchingNonVisitedNeighbours(Dictionary<Point, char> image, Dictionary<Point, bool> visited,
            Point p)
        {
            var pixel = image[p];

            // Up
            var neighbour = (p.r - 1, p.c);
            if (image.ContainsKey(neighbour) && image[neighbour] == pixel && !visited[neighbour]) yield return neighbour;

            // Down
            neighbour = (p.r + 1, p.c);
            if (image.ContainsKey(neighbour) && image[neighbour] == pixel && !visited[neighbour]) yield return neighbour;

            // Left
            neighbour = (p.r, p.c - 1);
            if (image.ContainsKey(neighbour) && image[neighbour] == pixel && !visited[neighbour]) yield return neighbour;

            //Right
            neighbour = (p.r, p.c + 1);
            if (image.ContainsKey(neighbour) && image[neighbour] == pixel && !visited[neighbour]) yield return neighbour;

        }


        ulong NonMatchingNeighbours(Dictionary<Point, char> image, Point currPoint)
        {
            ulong nonMatchingNeighbours = 0;
            char currPixel = image[currPoint];
            // Up
            var neighbour = (currPoint.r - 1, currPoint.c);
            if (!image.ContainsKey(neighbour) || image[neighbour] != currPixel) nonMatchingNeighbours++;

            // Down
            neighbour = (currPoint.r + 1, currPoint.c);
            if (!image.ContainsKey(neighbour) || image[neighbour] != currPixel) nonMatchingNeighbours++;

            // Left
            neighbour = (currPoint.r, currPoint.c - 1);
            if (!image.ContainsKey(neighbour) || image[neighbour] != currPixel) nonMatchingNeighbours++;

            //Right
            neighbour = (currPoint.r, currPoint.c + 1);
            if (!image.ContainsKey(neighbour) || image[neighbour] != currPixel) nonMatchingNeighbours++;

            return nonMatchingNeighbours;
        }

        return -123;
    }



    protected override object Solve2((Dictionary<Point, char> image, int rows, int cols) input)
    {
        throw new NotImplementedException();
    }
}