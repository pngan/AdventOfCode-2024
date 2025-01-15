using System.Collections.Immutable;

using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions.Days;

public class Day22 : BaseDay<IEnumerable<long>>
{
    protected override int DayNumber => 22;

    const int Iterations = 2000;

    protected override IEnumerable<long> Parse(ImmutableArray<string> input) => input.Select(Convert.ToInt64);

    protected override object Solve1(IEnumerable<long> input)
    {
        long result = 0;

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


    protected override object Solve2(IEnumerable<long> input)
    {
        Dictionary<string, long> seqHistogram = new();

        foreach (var i in input)
        {
            HashSet<string> visitedSeq = new();
            CircularBuffer4 buffer4 = new();

            var secret = i;
            for (int j = 0; j <= Iterations; j++) // <= because 2000 iterations after initial secret
            {
                var price = secret % 10;
                buffer4.Push(price);
                secret = Iterate(secret);
                if (j < 4) continue; // Meed to have at least 4 iterations to have a valid sequence

                var currentSeq = buffer4.DiffLast4();
                if (visitedSeq.Add(currentSeq))
                {
                    if (seqHistogram.TryGetValue(currentSeq, out var val))
                        seqHistogram[currentSeq] = val + buffer4.Current();
                    else
                        seqHistogram[currentSeq] = buffer4.Current();
                }
            }
        }

        // Find the most common sequence
        long max = 0;
        foreach (var kvp in seqHistogram)
        {
            if (kvp.Value > max)
            {
                max = kvp.Value;
            }
        }
        return max;
    }

    private static long Iterate(long i)
    {
        i ^= i << 6;
        i &= 0xffffff;
        i ^= i >>> 5;
        i &= 0xffffff;
        i ^= i << 11;
        i &= 0xffffff;

        return i;
    }
}

public class CircularBuffer4
{
    private const int SIZE = 5;
    private readonly long[] _buffer = new long[SIZE];
    private int _p = -1;
    private int _pushes = 0;

    public void Push(long item)
    {
        _pushes++;
        _p++;
        if (_p == SIZE) _p = 0;
        _buffer[_p] = item % 10;
    }

    public long Current()
    {
        if (_pushes < SIZE) throw new IndexOutOfRangeException();
        return _buffer[_p];
    }

    public string DiffLast4()
    {
        if (_pushes < SIZE) throw new IndexOutOfRangeException();
        var v = Values();
        return $"{v[1] - v[0]},{v[2] - v[1]},{v[3] - v[2]},{v[4] - v[3]}";
    }

    private long[] Values()
    {
        if (_pushes < SIZE) throw new IndexOutOfRangeException();
        long[] result = new long[SIZE];
        result[4] = _buffer[_p];
        result[3] = _buffer[(_p + SIZE - 1) % SIZE];
        result[2] = _buffer[(_p + SIZE - 2) % SIZE];
        result[1] = _buffer[(_p + SIZE - 3) % SIZE];
        result[0] = _buffer[(_p + SIZE - 4) % SIZE];

        return result;
    }
}