using System.Collections.Immutable;

using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions.Days;

using Point = (int r, int c);
public class Day12 : BaseDay<(Dictionary<Point, char> image, int rows, int cols)>
{
    protected override int DayNumber => 12;
    const int Unvisited = -1;

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

    protected override object Solve1((Dictionary<Point, char> image, int rows, int cols) input)
    {
        Dictionary<Point, int> labels = new();
        List<int> areas = new(); // idx=label, area count
        List<ulong> sides = new(); // idx=label, sides count
        foreach (Point pt in input.image.Keys)
        {
            labels[pt] = -1;
        }

        int label = -1;
        Queue<Point> queue = new();
        for (int r = 0; r < input.rows; r++)
        {
            for (int c = 0; c < input.cols; c++)
            {
                Point p = (r, c);
                if (labels[p]== Unvisited)
                {
                    queue.Enqueue(p);
                    labels[p] = ++label;
                    areas.Add(0);
                    sides.Add(0UL);

                    while (queue.Any())
                    {
                        var pp = queue.Dequeue();
                        var neighbours = MatchingNonVisitedNeighbours(input.image, labels, pp).ToList();
                        foreach (var neighbour in neighbours)
                        {
                            queue.Enqueue(neighbour);
                            labels[neighbour] = label;
                        }

                        areas[label]++;
                        sides[label] += NonMatchingNeighbours(input.image, pp);
                    }
                }
            }
        }

        // Calculate Cost
        ulong cost = 0;
        for(int l  = 0; l <= label; l++)
        {
            cost += (ulong) areas[l] * sides[l];
        }
        return cost;

        IEnumerable<Point> MatchingNonVisitedNeighbours(Dictionary<Point, char> image, Dictionary<Point, int> labels,
            Point p)
        {
            var pixel = image[p];

            // Up
            var neighbour = (p.r - 1, p.c);
            if (image.ContainsKey(neighbour) && image[neighbour] == pixel && labels[neighbour] == Unvisited) yield return neighbour;

            // Down
            neighbour = (p.r + 1, p.c);
            if (image.ContainsKey(neighbour) && image[neighbour] == pixel && labels[neighbour] == Unvisited) yield return neighbour;

            // Left
            neighbour = (p.r, p.c - 1);
            if (image.ContainsKey(neighbour) && image[neighbour] == pixel && labels[neighbour] == Unvisited) yield return neighbour;

            //Right
            neighbour = (p.r, p.c + 1);
            if (image.ContainsKey(neighbour) && image[neighbour] == pixel && labels[neighbour] == Unvisited) yield return neighbour;
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
    }

    protected override object Solve2((Dictionary<Point, char> image, int rows, int cols) input)
    {
        throw new NotImplementedException();
    }
}