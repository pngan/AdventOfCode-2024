namespace AdventureOfCode.Utilities.Image;

public readonly record struct Step2(int dr, int dc)
{
    public static readonly Step2 Zero = new(dr: 0, dc: 0);

    public static readonly Step2 N = new(-1, 0);
    public static readonly Step2 S = new(1, 0);
    public static readonly Step2 W = new(0, -1);
    public static readonly Step2 E = new(0, 1);

    public static readonly Step2 NW = new(-1, -1);
    public static readonly Step2 NE = new(-1, 1);
    public static readonly Step2 SE = new(1, 1);
    public static readonly Step2 SW = new(1, -1);

    public static IEnumerable<Step2> Neighbours8() => [N, NE, E, SE, S, SW, W];
    public static IEnumerable<Step2> Neighbours4() => [N, E, S, W];

    public static Step2 operator +(Step2 lhs, Step2 rhs) => new Step2(dr: lhs.dr + rhs.dr, dc: lhs.dc + rhs.dc);
    public static Step2 operator -(Step2 lhs, Step2 rhs) => new Step2(dr: lhs.dr - rhs.dr, dc: lhs.dc - rhs.dc);
    public static Step2 operator -(Step2 step) => new Step2(dr: -step.dr, dc: -step.dc);
    public bool Equals(Step2 step)
    {
        if ((object)step == null) return false;
        return (dr == step.dr) && (dc == step.dc);
    }

    public override int GetHashCode() => dr ^ dc;
}







