using AdventOfCode.Solutions.Common;
using System.Collections.Immutable;

namespace AdventOfCode.Solutions.Days;

using Point = (int r, int c);
using Step = (int dr, int dc);

public class Day06 : BaseDay<(HashSet<Point> obstacles, int rows, int cols, Point start)>
{
    protected override int DayNumber => 6;

    private List<Point> _visited = new();
    protected override (HashSet<Point> obstacles, int rows, int cols, (int r, int c) start) Parse(ImmutableArray<string> input)
    {
        int rows = input.Length;
        int cols = input.First().Length;
        (int r, int c) start = new();
        HashSet<Point> obstacles = new();
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
            {
                if (input[r][c] == '^')
                {
                    start.r = r;
                    start.c = c;
                }
                else if (input[r][c] == '#')
                {
                    obstacles.Add((r,c));
                }
            }

        return (obstacles, rows, cols, start);
    }

    protected override object Solve1((HashSet<Point> obstacles, int rows, int cols, Point start) input)
    {
        (Visited v, bool isLoop) result = WalkPath((input.obstacles, input.rows, input.cols, input.start));
        _visited = result.v.AsSequence();
        return _visited.Distinct().Count();
    }

    protected override object Solve2((HashSet<Point> obstacles, int rows, int cols, Point start) input)
    {
        HashSet<Point> loopObstacles = new();
        // Loop for every position that was visited in Part 1
        // Ignore last item because it is about to exit the grid, and so there is no opportunity to put
        // an obstacle in front of it
        for (int i = 0, j = 1; i < _visited.Count()-1; i++, j++)
        {
            // In next position place candidate obstacle - candidate to see if it creates a loop
            Point candidateObstaclePostion = _visited[j];
            HashSet<Point> obstaclesAndCandidate = new(input.obstacles);
            if (!obstaclesAndCandidate.Add(candidateObstaclePostion))
                continue; // Early return because the candidate obstacle has already been examined

            // Walk the path with new obstacle and see if it encounters a loop
            (Visited v, bool isLoop) result = WalkPath((obstaclesAndCandidate, input.rows, input.cols, input.start));

            // If walk results in loop, record in obstacle set
            if (result.isLoop)
                loopObstacles.Add(candidateObstaclePostion);
        }

        // Return number in obstacle set
        // A simple count is inadequate because we might examine a candidate obstacle position more than once
        return loopObstacles.Count();
    }

    private (Visited visited, bool isLoop) WalkPath((HashSet<Point> obstacles, int rows, int cols, Point start) input)
    {
        Point pos = (input.start.r, input.start.c);
        Point step = (-1, 0); // Initial step direction is defined in puzzle description
        Point next = new();

        Visited visited = new();

        bool OutOfBounds(Point p) => p.r < 0 || p.c < 0 || p.r >= input.rows || p.c >= input.cols;
        Step Rotate(Step step) => (step.dc, -step.dr);
        while (true)
        {
            if (OutOfBounds(pos)) break;  // Check for out of bounds
            if (!visited.Add(pos, step)) return (visited, true); // If encounter a loop, exit with loop flag set on

            next = Step(pos, step); // Generate candidate for next position
            if (input.obstacles.Contains(next)) // If next position contains obstacle, then turn right 
            {
                step = Rotate(step);
                continue;
            }
            pos = next; // Everything is fine, commit to candidate position, and move along
        }
        return (visited, false);
    }

    Point Step(Point p, Step s) => (p.r + s.dr, p.c + s.dc);
}

public class Visited
{
    private Dictionary<Step, HashSet<Point>> _visitedByStep = new()
    {
        { (-1, 0), new HashSet<Point>() },
        { (0, 1), new HashSet<Point>() },
        { (1, 0), new HashSet<Point>() },
        { (0, -1), new HashSet<Point>() }
    };
    private List<Point> _visitedSequence = new();

    public bool Add(Point p, Step s)
    {
        _visitedSequence.Add(p);
        return _visitedByStep[s].Add(p);
    }

    // Visited locations in order.
    // Can have repeated values if location is visited more than once.
    public List<Point> AsSequence() => _visitedSequence;
}