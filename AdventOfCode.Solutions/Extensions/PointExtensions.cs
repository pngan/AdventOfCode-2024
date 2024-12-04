namespace AdventOfCode.Solutions.Extensions;

public static class PointExtensions
{
    public static (long r, long c) Add(this (long r, long c) orig, (long r, long c) addend)
    {
        return (orig.r+addend.r, orig.c+addend.c);
    }
}
