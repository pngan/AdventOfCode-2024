namespace AdventureOfCode.Utilities.Image;

using System.Numerics;

public static class PointExtensions
{
    public static IEnumerable<(T r, T c)> Neighbours8<T>(this (T r, T c) p) where T : INumber<T>
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
    public static IEnumerable<(T r, T c)> Neighbours4<T>(this (T r, T c) p) where T : INumber<T>
        =>
        [
            (p.r-T.One, p.c), // n
            (p.r, p.c+T.One), // e
            (p.r+T.One, p.c), // s
            (p.r, p.c-T.One), // w
        ];


    public static (T r, T c) Add<T>(this (T r, T c) x, (T r, T c) y) where T : INumber<T> => (x.r + y.r, x.c + y.c);
    public static (T r, T c) Subtract<T>(this (T r, T c) x, (T r, T c) y) where T : INumber<T> => (x.r - y.r, x.c - y.c);
    public static (T r, T c) Negate<T>(this (T r, T c) x) where T : INumber<T> => (-x.r, -x.c);

    public static (int r, int c) StepNW(this (int r, int c) p) => p.Add(Step.NW);
    public static (int r, int c) StepN(this (int r, int c) p) => p.Add(Step.N);
    public static (int r, int c) StepNE(this (int r, int c) p) => p.Add(Step.NE);
    public static (int r, int c) StepE(this (int r, int c) p) => p.Add(Step.E);
    public static (int r, int c) StepSE(this (int r, int c) p) => p.Add(Step.SE);
    public static (int r, int c) StepS(this (int r, int c) p) => p.Add(Step.S);
    public static (int r, int c) StepSW(this (int r, int c) p) => p.Add(Step.SW);
    public static (int r, int c) StepW(this (int r, int c) p) => p.Add(Step.W);

    public static (int dr, int dc) Rot0(this (int dr, int dc) s) => (dr: s.dr, dc: s.dc);
    public static (int dr, int dc) Rot90(this (int dr, int dc) s) => (dr: -s.dc, dc: s.dr);
    public static (int dr, int dc) Rot180(this (int dr, int dc) s) => (dr: -s.dr, dc: -s.dc);
    public static (int dr, int dc) Rot270(this (int dr, int dc) s) => (dr: s.dc, dc: -s.dr);

    public static T ManhattanDistance<T>((T r, T c) a, (T r, T c) b) where T : INumber<T> => Abs<T>(a.r - b.r) + Abs<T>(a.c - b.c);

    public static T Abs<T>(this T value) where T : INumber<T> => T.Abs(value);
}


public static class Step
{
    public static (int dr, int dc) NW = (-1, -1);
    public static (int dr, int dc) N =  (-1, 0);
    public static (int dr, int dc) NE = (-1, 1);
    public static (int dr, int dc) E =  (0, 1);
    public static (int dr, int dc) SE = (1, 1);
    public static (int dr, int dc) S =  (1, 0);
    public static (int dr, int dc) SW = (1, -1);
    public static (int dr, int dc) W=   (0, -1);
}