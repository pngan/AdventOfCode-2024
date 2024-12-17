namespace AdventureOfCode.Utilities.Image;

using System.Net.NetworkInformation;

using Point2 = (int r, int c);

public static class PointExtensions
{
    public static (long r, long c) Add(this (long r, long c) orig, (long r, long c) addend)
    {
        return (orig.r+addend.r, orig.c+addend.c);
    }

    public static IEnumerable<(int r, int c)> Neighbours8( this (int r, int c) p)
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
    public static IEnumerable<Point2> Neighbours4(this (int r, int c) p)
    {
        return [
            (p.r-1, p.c), // n
            (p.r, p.c+1), // e
            (p.r+1, p.c), // s
            (p.r, p.c-1), // w
            ];
    }

    public static (int r, int c) Add(this (int r, int c) p, (int dr, int dc) s) => (r: p.r + s.dr, c: p.c + s.dc);
    public static (int r, int c) Subtract(this (int r, int c) p, (int dr, int dc) s) => (r: p.r - s.dr, c: p.c - s.dc);
    public static (int r, int c) Negate(this (int r, int c) p) => (r: -p.r, c: -p.c);
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