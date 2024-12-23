using System.Collections.Immutable;
using System.Text.RegularExpressions;

using AdventOfCode.Solutions.Common;

using MoreLinq;

namespace AdventOfCode.Solutions.Days;

public class Day13 : BaseDay<IEnumerable<(int ax, int ay, int bx, int by, int x, int y)>>
{
    protected override int DayNumber => 13;

    protected override IEnumerable<(int ax, int ay, int bx, int by, int x, int y)> Parse(ImmutableArray<string> input)
    {
        var result = new List<(int ax, int ay, int bx, int by, int x, int y)>();
        foreach (var group in input.Split(""))
        {
            result.Add( group.Fold((a, b, c) =>
            {
                Regex regex = new Regex(@"X\+(\d+), Y\+(\d+)");
                Match match = regex.Match(a);
                int ax = int.Parse(match.Groups[1].Value);
                int ay = int.Parse(match.Groups[2].Value);

                match = regex.Match(b);
                int bx = int.Parse(match.Groups[1].Value);
                int by = int.Parse(match.Groups[2].Value);

                Regex regexPrize = new Regex(@"X\=(\d+), Y\=(\d+)");
                match = regexPrize.Match(c);
                int px = int.Parse(match.Groups[1].Value);
                int py = int.Parse(match.Groups[2].Value);

                return (ax, ay, bx, by, px, py);
            }));
        }

        return result;
    }

    protected override object Solve1(IEnumerable<(int ax, int ay, int bx, int by, int x, int y)> input)
    {
        int  cost = 0;
        foreach ((int ax, int ay, int bx, int by, int x, int y) machine in input)
        {
            int machineCost = int.MaxValue;
            const int maxPresses = 100;
            for (int a = 0; a < maxPresses; a++)
            {
                for (int b = 0; b < maxPresses; b++)
                {
                    if (((machine.ax * a + machine.bx * b) == machine.x) && ((machine.ay * a + machine.by * b) == machine.y))
                    {
                        int c = 3 * a + b;
                        if (c < machineCost)
                            machineCost = c;
                    }
                }
            }

            if (machineCost != int.MaxValue)
            {
                cost += machineCost;
            }
        }
        return cost;
    }

    protected override object Solve2(IEnumerable<(int ax, int ay, int bx, int by, int x, int y)> input)
    {
        decimal cost = 0m;
        decimal DecimalEpsilon = (decimal)(1 / Math.Pow(10, 28));
        foreach (var machine in input)
        {
            decimal a = machine.ax;//a1
            decimal b = machine.ay;//b1
            decimal c = machine.bx;//a2
            decimal d = machine.by;//b2
            decimal e = machine.x + 10000000000000m;//a3
            decimal f = machine.y + 10000000000000m;//b3

            decimal y = (b * e - a * f) / (b * c - a * d);
            decimal yi = Math.Round(y);
            if ( Math.Abs(y-yi) < DecimalEpsilon)
            {
                decimal x = (d * e - c* f) / (d*a-c*b);
                decimal xi = Math.Round(x);
                if (Math.Abs(x - xi) < DecimalEpsilon)
                {
                    cost += 3 * x + y;
                }

            }

        }

        return cost;

    }
}