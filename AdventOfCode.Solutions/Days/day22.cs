using System.Collections.Immutable;

using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions.Days;

public class Day22 : BaseDay<IEnumerable<ulong>>
{
    protected override int DayNumber => 22;

    const int Iterations = 2000;

    protected override IEnumerable<ulong> Parse(ImmutableArray<string> input) => input.Select(Convert.ToUInt64);

    protected override object Solve1(IEnumerable<ulong> input)
    {
        ulong result = 0;

        foreach (var i in input)
        {
            var secret = i;
            for (int j = 0; j < Iterations; j++)
            {
                secret = Iterate(secret);
            }
            result += secret;
        }
        return result;
    }

    protected override object Solve2(IEnumerable<ulong> input)
    {
        throw new NotImplementedException();
    }

    private static ulong Iterate(ulong i)
    {
        i ^= i << 6;
        i &= 0xffffff;
        i ^= i >> 5;
        i &= 0xffffff;
        i ^= i << 11;
        i &= 0xffffff;

        return i;
    }
}
public class CircularQuadBuffer
{
    private const int SIZE = 5;
    private readonly int[] _buffer = new int[SIZE];
    private int _p = -1;
    private int _pushes = 0;

    public void Push(ulong item)
    {
        _pushes++;
        _p++;
        if (_p == SIZE) _p = 0;
        _buffer[_p] = (int)item % 10;
    }
    public int Current()
    {
        if (_pushes < SIZE) throw new IndexOutOfRangeException();
        return _buffer[_p];
    }
    private int[] Values()
    {
        if (_pushes < SIZE) throw new IndexOutOfRangeException();
        int[] result = new int[SIZE];
        result[4] = _buffer[_p];
        result[3] = _buffer[(_p + SIZE - 1) % SIZE];
        result[2] = _buffer[(_p + SIZE - 2) % SIZE];
        result[1] = _buffer[(_p + SIZE - 3) % SIZE];
        result[0] = _buffer[(_p + SIZE - 4) % SIZE];

        return result;
    }
    public (int, int, int, int) Changes()
    {
        var v = Values();
        return (v[1] - v[0], v[2] - v[1], v[3] - v[2], v[4] - v[3]);
    }
}