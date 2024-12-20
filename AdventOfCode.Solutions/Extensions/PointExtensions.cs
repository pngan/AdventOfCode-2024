﻿namespace AdventOfCode.Solutions.Extensions;

using Point2 = (int r, int c);

public static class PointExtensions
{
    public static (long r, long c) Add(this (long r, long c) orig, (long r, long c) addend)
    {
        return (orig.r+addend.r, orig.c+addend.c);
    }


    public static IEnumerable<(int r, int c)> Neighbours8( (int r, int c) p)
    {
        return [
            (p.r-1, p.c-1), // nw
            (p.r-1, p.c), // n
            (p.r-1, p.c+1), // ne
            (p.r, p.c+1), // e
            (p.r+1, p.c+1), // se
            (p.r+1, p.c), // s
            (p.r+1, p.c-1), // sw
            (p.r, p.c-1), // w
            ];
    }
    public static IEnumerable<Point2> Neighbours4((int r, int c) p)
    {
        return [
            (p.r-1, p.c), // n
            (p.r, p.c+1), // e
            (p.r+1, p.c), // s
            (p.r, p.c-1), // w
            ];
    }

}
