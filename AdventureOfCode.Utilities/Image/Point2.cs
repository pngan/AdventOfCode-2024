using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventureOfCode.Utilities.Image;
public readonly record struct Point2(int r, int c)
{
    public static readonly Point2 Zero = new(r: 0, c: 0);

    public static Point2 operator +(Point2 p, Step2 s) => new Point2(r: p.r + s.dr, c: p.c + s.dc);
    public static Point2 operator -(Point2 p, Step2 s) => new Point2(r: p.r - s.dr, c: p.c - s.dc);
    public static implicit operator Point2((int r, int c) p) => new Point2(p.r, p.c);

    public bool Equals(Point2 p)
    {
        if ((object)p == null) return false;
        return (r == p.r) && (c == p.c);
    }
    public override int GetHashCode() => r ^ c;

    public IEnumerable<Point2> Neighbours8()
    {
        foreach (var s in Step2.Neighbours8())
        {
            yield return this + s;
        }
    }
    public IEnumerable<Point2> Neighbours4()
    {
        foreach (var s in Step2.Neighbours4())
        {
            yield return this + s;
        }
    }
}
