namespace AdventOfCode.Solutions.Extensions;

using System.Numerics;

public static class PointExtensions
{
    public static (T r, T c) Add<T>(this (T r, T c) orig, (T r, T c) addend) where T : INumber<T>
        => (orig.r + addend.r, orig.c + addend.c);

    public static IEnumerable<(T r, T c)> Neighbours8<T>((T r, T c) p) where T : INumber<T>
        => 
        [
            (p.r-T.One, p.c-T.One), // nw
            (p.r-T.One, p.c), // n
            (p.r-T.One, p.c+T.One), // ne
            (p.r, p.c+T.One), // e
            (p.r+T.One, p.c+T.One), // se
            (p.r+T.One, p.c), // s
            (p.r+T.One, p.c-T.One), // sw
            (p.r, p.c-T.One), // w
        ];
    public static IEnumerable<(T r, T c)> Neighbours4<T>((T r, T c) p) where T : INumber<T> 
        => 
        [
            (p.r-T.One, p.c), // n
            (p.r, p.c+T.One), // e
            (p.r+T.One, p.c), // s
            (p.r, p.c-T.One), // w
        ];
}
