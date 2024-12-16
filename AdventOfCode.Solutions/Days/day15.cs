using System.Collections.Immutable;
using AdventOfCode.Solutions.Common;
using AdventOfCode.Solutions.Extensions;

using AdventureOfCode.Utilities.Image;

using MoreLinq;
namespace AdventOfCode.Solutions.Days;
public class Day15 : BaseDay<(CharImage2 map, string code, (int r, int c) start)>
{
    protected override int DayNumber => 15;

    protected override (CharImage2 map, string code, (int r, int c) start) Parse(ImmutableArray<string> input) =>
        input.Split("").Fold((map, codes) =>
        {
            var im = CharImage2.Parse(map);
            var start = im.Find('@').Single();
            return (im, string.Join(null, codes), start);
        });

    protected override object Solve1((CharImage2 map, string code, (int r, int c) start) input)
    {
        var stack = new Stack<((int r, int c) curr, (int r, int c) next)>();
        var robot = input.start;
        // For each code
        foreach (var code in input.code)
        {
            var step = code switch
            {
                '^' => (-1, 0),
                'v' => (1, 0),
                '<' => (0, -1),
                '>' => (0, 1),
                _ => throw new NotImplementedException()
            };
            var pos = robot;
            var next = robot.Add(step);
            bool canStep = true;
            // Push everything in direction of code onto stack until space or wall
            while(true)
            {
                var nextValue = input.map[next];
                stack.Push((pos, next));
                if (nextValue == '#')
                {
                    canStep = false;
                    break;
                }
                if (nextValue == '.') break;


                pos = next;
                next=next.Add(step);
            }

            // If space, then move all the items
            if (canStep)
            {
                while (stack.Any())
                {
                    var move = stack.Pop();
                    var tmp = input.map[move.next];
                    input.map[move.next] = input.map[move.curr];
                    input.map[move.curr] = tmp;

                    robot = move.next;
                }
            }
            else
            {   // Else hit a wall and nothing moves. Do nothing and go to next movement code
                stack.Clear();
            }
        }

        // Calc score
        long score = input.map.Find('O').Sum(v => v.r*100+v.c);
        return score;
    }

    protected override object Solve2((CharImage2 map, string code, (int r, int c) start) input)
    {
        // Create double wide map
        CharImage2 map = new CharImage2(input.map.ROWS, input.map.COLS*2);
        for(int r = 0; r < input.map.ROWS; r++)
            for(int c = 0; c < input.map.COLS; c++)
            {
                var wide1 = (r, c * 2);
                var wide2 = wide1.StepE();
                switch(input.map[(r,c)])
                {
                    case '#':
                        map[wide1] = '#';
                        map[wide2] = '#';
                        break;
                    case 'O':
                        map[wide1] = '[';
                        map[wide2] = ']';
                        break;
                    case '.':
                        map[wide1] = '.';
                        map[wide2] = '.';
                        break;
                    case '@':
                        map[wide1] = '@';
                        map[wide2] = '.';
                        break;
                    default: throw new InvalidDataException();
                }
            }


        var bfs = new Queue<(int r, int c)>();
        var visited = new Stack<(int r, int c)>();
        var robot = (input.start.r, input.start.c*2);
        // For each code
        foreach (var code in input.code)
        {
            bool skipped = false;
            var step = code switch
            {
                '^' => (-1, 0),
                'v' => (1, 0),
                '<' => (0, -1),
                '>' => (0, 1),
                _ => throw new NotImplementedException()
            };

            bool canMove = true;
            bfs.Clear();
            bfs.Enqueue(robot);
            visited.Push(robot);
            // BFS through tree of obstacle. Used BFS rather than DFS because this gives the correct 
            // ordering of obstacles in the visited collection. If they are not ordered correctly, 
            // the obstacles pairs are broken apart.
            while(bfs.Any())
            {
                var p = bfs.Dequeue();

                var next = p.Add(step);
                var nextValue = map[next];
                if (visited.Contains(next))
                    continue;
                if (nextValue == '#') // Hit wall, abandon search, goto next code
                {
                    canMove = false;
                    break;
                }
                if (nextValue == '[')
                {
                    bfs.Enqueue(next);
                    visited.Push(next);

                    if (code == '^' || code == 'v')
                    {
                        var right = next.StepE();
                        var rightValue = map[right];
                        if (rightValue != ']')
                        {
                            throw new InvalidDataException($"Expected ], got {rightValue}");
                        }

                        if (!visited.Contains(right))
                        {
                            bfs.Enqueue(right);
                            visited.Push(right);
                        }
                    }
                }

                if (nextValue == ']')
                {
                    bfs.Enqueue(next);
                    visited.Push(next);

                    if (code == '^' || code == 'v')
                    {
                        var left = next.StepW();
                        var leftValue = map[left];
                        if (leftValue != '[')
                        {
                            throw new InvalidDataException($"Expected [, got {leftValue}");
                        }
                        if (!visited.Contains(left))
                        {
                            bfs.Enqueue(left);
                            visited.Push(left);
                        }
                    }
                }
            }

            // Move all items in step, if allowed
            if (canMove)
            {
                while (visited.Any())
                {
                    var from = visited.Pop();
                    var to = from.Add(step);
                    var tmp = map[to];
                    map[to] = map[from];
                    map[from] = tmp;

                    robot = to;
                }
            }
            else
            {   // Else hit a wall and nothing moves. Do nothing and go to next movement code
                bfs.Clear();
                visited.Clear();
            }

            // Uncomment the following to help debugging
            // Search map for broken pattern
            //var pos = map.Find('[');
            //foreach (var x in pos)
            //{
            //    if (map[(x.r,x.c+1)] != ']')
            //    {
            //        Console.WriteLine($"******** Broken Pattern iter={iter}, l={map[x]} r={map[(x.r, x.c + 1)]}, pos={x}, code={code}");
            //       prevMap.DrawMap();
            //        map.DrawMap();
            //        Console.ReadLine();
            //    }
            //}
        }
        // Calc score
        long score = map.Find('[').Sum(v => v.r * 100 + v.c);
        return score;
    }
}