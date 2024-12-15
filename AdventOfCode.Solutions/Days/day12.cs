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

    protected override object Solve1((Dictionary<Point, char> image, int rows, int cols) input) => ProcessImage(input, CountPixelEdges);

    protected override object Solve2((Dictionary<Point, char> image, int rows, int cols) input) => ProcessImage(input, CountPixelSides);

    private object ProcessImage((Dictionary<Point, char> image, int rows, int cols) input, Func<Dictionary<Point, int>, Point, ulong> CalcEdgesOrSides)
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
                if (labels[p] == Unvisited)
                {
                    queue.Enqueue(p);
                    labels[p] = ++label;
                    areas.Add(0);
                    sides.Add(0UL);

                    // Flood Fill
                    while (queue.Any())
                    {
                        p = queue.Dequeue();
                        var neighbours = MatchingNonVisitedNeighbours(input.image, labels, p).ToList();
                        foreach (var neighbour in neighbours)
                        {
                            queue.Enqueue(neighbour);
                            labels[neighbour] = label;
                        }

                        areas[label]++;

                    }
                }
            }
        }

        for (int r = 0; r < input.rows; r++)
        {
            for (int c = 0; c < input.cols; c++)
            {
                ulong n = CalcEdgesOrSides(labels, (r, c));
                sides[labels[(r, c)]] += n;
            }
        }


        // Calculate Cost
        ulong cost = 0;
        for (int l = 0; l <= label; l++)
        {
            cost += (ulong)areas[l] * sides[l];
        }
        return cost;

        static IEnumerable<Point> MatchingNonVisitedNeighbours(Dictionary<Point, char> image, Dictionary<Point, int> labels, Point p)
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
    }

    static ulong CountPixelEdges(Dictionary<Point, int> labels, Point currPoint)
    {
        ulong edgeCount = 0;
        int label = labels[currPoint];
        // Up
        var neighbour = (currPoint.r - 1, currPoint.c);
        if (!labels.ContainsKey(neighbour) || labels[neighbour] != label) edgeCount++;

        // Down
        neighbour = (currPoint.r + 1, currPoint.c);
        if (!labels.ContainsKey(neighbour) || labels[neighbour] != label) edgeCount++;

        // Left
        neighbour = (currPoint.r, currPoint.c - 1);
        if (!labels.ContainsKey(neighbour) || labels[neighbour] != label) edgeCount++;

        //Right
        neighbour = (currPoint.r, currPoint.c + 1);
        if (!labels.ContainsKey(neighbour) || labels[neighbour] != label) edgeCount++;

        return edgeCount;
    }

    ulong CountPixelSides(Dictionary<Point, int> labels, Point p)
    {
        ulong sideCount = 0;

        if (MatchesE() && NotMatchesSE() && MatchesS()) sideCount++;// Inner corner se
        if (MatchesW() && MatchesS() && NotMatchesSW()) sideCount++;// Inner corner sw
        if (MatchesN() && NotMatchesNE() && MatchesE()) sideCount++;// Inner corner ne
        if (NotMatchesNW() && MatchesN() && MatchesW()) sideCount++;// Inner corner nw

        if (NotMatchesS() && NotMatchesE()) sideCount++; // Outer corner se
        if (NotMatchesW() && NotMatchesS()) sideCount++; // Outer corner sw
        if (NotMatchesN() && NotMatchesE()) sideCount++; // Outer corner ne
        if (NotMatchesN() && NotMatchesW()) sideCount++; // Outer corner nw

        return sideCount;

        bool MatchesN() => Matches((-1, 0));
        bool MatchesS() => Matches((1, 0));
        bool MatchesE() => Matches((0, 1));
        bool MatchesW() => Matches((0, -1));

        bool NotMatchesW() => NotMatches((0, -1));
        bool NotMatchesE() => NotMatches((0, 1));
        bool NotMatchesNW() => NotMatches((-1, -1));
        bool NotMatchesNE() => NotMatches((-1, 1));
        bool NotMatchesN() => NotMatches((-1, 0));
        bool NotMatchesSW() => NotMatches((1, -1));
        bool NotMatchesS() => NotMatches((1, 0));
        bool NotMatchesSE() => NotMatches((1, 1));

        bool Matches((int dr, int dc) step)
        {
            int l = labels[p];
            Point pp = (p.r + step.dr, p.c + step.dc);
            if (!labels.ContainsKey(pp)) return false;
            return labels[pp] == l;
        }

        bool NotMatches((int dr, int dc) step)
        {
            int l = labels[p];
            Point pp = (p.r + step.dr, p.c + step.dc);
            if (!labels.ContainsKey(pp)) return true;
            return labels[pp] != l;
        }
    }
}