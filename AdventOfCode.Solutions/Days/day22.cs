using System.Collections.Immutable;

using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions.Days;

public class Day22 : BaseDay<IEnumerable<int>>
{
    protected override int DayNumber => 22;

    const int Iterations = 2000;

    protected override IEnumerable<int> Parse(ImmutableArray<string> input) => input.Select(Convert.ToInt32);

    protected override object Solve1(IEnumerable<int> input)
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


    protected override object Solve2(IEnumerable<int> input)
    {
        Dictionary<(int,int,int,int), int> seqHistogram = new();

        foreach (var i in input)
        {
            HashSet<(int,int,int,int)> visitedSeq = new();
            CircularBuffer4 buffer4 = new();

            var secret = i;
            for (int j = 0; j <= Iterations; j++) // <= because 2000 iterations after initial secret
            {
                var price = (byte)(secret % 10);
                buffer4.Push(price);
                secret = Iterate(secret);
                if (j < 4) continue; // Need to have at least 4 iterations to have a valid sequence

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

    private static int Iterate(int i)
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
    private readonly byte[] _buffer = new byte[SIZE];
    private int _p = -1;

    public void Push(byte item)
    {
        _p++;
        if (_p == SIZE) _p = 0;
        _buffer[_p] = item;
    }

    public int Current() => _buffer[_p];

    public (int,int,int,int) DiffLast4()
    {
        var v = Values();
        return (v[1] - v[0],v[2] - v[1],v[3] - v[2],v[4] - v[3]);
    }

    private byte[] Values() 
        => [_buffer[(_p + SIZE - 4) % SIZE], _buffer[(_p + SIZE - 3) % SIZE], _buffer[(_p + SIZE - 2) % SIZE], _buffer[(_p + SIZE - 1) % SIZE], _buffer[_p]];
}