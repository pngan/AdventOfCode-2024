using System.Collections.Immutable;

using AdventOfCode.Solutions.Common;
using AdventOfCode.Solutions.Extensions;

using System.Numerics;

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

public static class I64Extensions
{
    public static ulong Mix(this ulong input, ulong value) => input ^ value;
    public static ulong Prune(this ulong input) =>  input & 0xffffff;  // fast modulo 16777216
    public static ulong Mult64(this ulong input) =>  input << 6;
    public static ulong Div32(this ulong input) => input >> 5; 
    public static ulong Mult2048(this ulong input) => input << 11;


}


