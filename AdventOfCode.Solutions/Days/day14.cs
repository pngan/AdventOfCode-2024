using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.Metrics;
using System.Text.RegularExpressions;

using AdventOfCode.Solutions.Common;
using AdventOfCode.Solutions.Extensions;

using MoreLinq;

namespace AdventOfCode.Solutions.Days;

using Robot = ((int x, int y) p, (int dx, int dy) v);

public class Day14 : BaseDay<IEnumerable<Robot>>
{
    protected override int DayNumber => 14;
    //const int Width = 11;
    //const int Height = 7;
    const int Midx = Width / 2;
    const int Midy = Height / 2;

    const int Width = 101;
    const int Height = 103;

    protected override IEnumerable<Robot> Parse(ImmutableArray<string> input)
    {
        foreach (var line in input)
        {
            Regex regex = new Regex(@"p=(\d+),(\d+)\ v=(-?\d+),(-?\d+)");
            Match match = regex.Match(line);
            int x = int.Parse(match.Groups[1].Value);
            int y = int.Parse(match.Groups[2].Value);
            int dx = int.Parse(match.Groups[3].Value);
            int dy = int.Parse(match.Groups[4].Value);
            yield return ((x, y), (dx, dy));
        }
    }
    //87536700 too low
    //230900224


    protected override object Solve1(IEnumerable<Robot> input)
    {
        List<(int x, int y)> pos = new();

        foreach (var r in input)
        {
            var val = (r.p.x, r.p.y);
            for (int i = 0; i < 100; i++)
            {
                int valx = (val.x + r.v.dx) % Width;
                int valy = (val.y + r.v.dy) % Height;
                valx = valx < 0 ? valx + Width : valx;
                valy = valy < 0 ? valy + Height : valy;
                val = (valx, valy);
            }

            if (val.x == Midx || val.y == Midy) continue;
            pos.Add(val);
        }

        var tl = pos.Count(p => p.x < Midx && p.y < Midy);
        var tr = pos.Count(p => p.x > Midx && p.y < Midy);
        var bl = pos.Count(p => p.x < Midx && p.y > Midy);
        var br = pos.Count(p => p.x > Midx && p.y > Midy);

        return (ulong)tl * (ulong)tr * (ulong)bl * (ulong)br;
    }

    protected override object Solve2(IEnumerable<Robot> input)
    {
        for (int iters = 6532; iters < 6533; iters++)
        {
            List<int> posx = new();
            List<int> posy = new();
            int[,] image = new int[Width, Height];
            foreach (var r in input)
            {
                var val = (r.p.x, r.p.y);
                for (int i = 0; i < iters; i++)
                {
                    int valx = (val.x + r.v.dx) % Width;
                    int valy = (val.y + r.v.dy) % Height;
                    valx = valx < 0 ? valx + Width : valx;
                    valy = valy < 0 ? valy + Height : valy;
                    val = (valx, valy);
                }

                if (val.x == Midx || val.y == Midy) continue;
                posx.Add(val.x);
                posy.Add(val.y);
                image[val.x, val.y] = 1;
            }

            int block = 0;
            for (int p = 0; p < posx.Count; p++)
            {
                int x = posx[p];
                int y = posy[p];
                if (x == 0 || x == Width - 1 || y == 0 || y == Height - 1) continue;

                if (image[x - 1, y - 1] == 1
                    && image[x - 1, y] == 1
                    && image[x - 1, y + 1] == 1
                    && image[x, y - 1] == 1
                    && image[x, y] == 1
                    && image[x, y + 1] == 1
                    && image[x + 1, y - 1] == 1
                    && image[x + 1, y] == 1
                    && image[x + 1, y + 1] == 1
                    )
                    block++;
            }

            if (block > 10)
            {
               // Console.WriteLine($"{iters},{block}");
                return iters;
            }

            //// Draw image as ascii
            //for (int yy = 0; yy < Height; yy++)
            //{
            //    for (int xx = 0; xx < Width; xx++)
            //    {
            //        char c = image[xx, yy] == 1 ? '*' : ' ';
            //        Console.Write(c);
            //    }
            //    Console.WriteLine();
            //}
        }
        return 0;
    }
}