using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Text;

using AdventOfCode.Solutions.Common;

using Microsoft.Diagnostics.Tracing.Parsers.MicrosoftWindowsTCPIP;

using MoreLinq;

namespace AdventOfCode.Solutions.Days;

public class Day21 : BaseDay<IEnumerable<string>>
{
    protected override int DayNumber => 21;

    protected override IEnumerable<string> Parse(ImmutableArray<string> input) => input;

    Dictionary<string, string> moves = new() {
        {"AA", ""},
        {"<<", ""},
        {"^^", ""},
        {"vv", ""},
        {">>", ""},
        {"A^", "<"},
        {"A<", "v<<"},
        {"Av", "<v"},
        {"A>", "v"},
        {"^A", ">"},
        {"^<", "v<"},
        {"^v", "v"},
        {"^>", "v>"},
        {"<A", ">>^"},
        {"<^", ">^"},
        {"<v", ">"},
        {"<>", ">>"},
        {"vA", "^>"},
        {"v^", "^"},
        {"v<", "<"},
        {"v>", ">"},
        {">A", "^"},
        {">^", "<^"},
        {"><", "<<"},
        {">v", "<"},
        {"A0", "<"},
        {"A1", "^<<"},
        {"A2", "<^"},
        {"A3", "^"},
        {"A4", "^^<<"},
        {"A5", "<^^"},
        {"A6", "^^"},
        {"A7", "^^^<<"},
        {"A8", "<^^^"},
        {"A9", "^^^"},
        {"0A", ">"},
        {"01", "^<"},
        {"02", "^"},
        {"03", "^>"},
        {"04", "^^<"},
        {"05", "^^"},
        {"06", "^^>"},
        {"07", "^^^<"},
        {"08", "^^^"},
        {"09", "^^^>"},
        {"1A", ">>v"},
        {"10", ">v"},
        {"12", ">"},
        {"13", ">>"},
        {"14", "^"},
        {"15", "^>"},
        {"16", "^>>"},
        {"17", "^^"},
        {"18", "^^>"},
        {"19", "^^>>"},
        {"2A", "v>"},
        {"20", "v"},
        {"21", "<"},
        {"23", ">"},
        {"24", "<^"},
        {"25", "^"},
        {"26", "^>"},
        {"27", "<^^"},
        {"28", "^^"},
        {"29", "^^>"},
        {"3A", "v"},
        {"30", "<v"},
        {"31", "<<"},
        {"32", "<"},
        {"34", "<<^"},
        {"35", "<^"},
        {"36", "^"},
        {"37", "<<^^"},
        {"38", "<^^"},
        {"39", "^^"},
        {"4A", ">>vv"},
        {"40", ">vv"},
        {"41", "v"},
        {"42", "v>"},
        {"43", "v>>"},
        {"45", ">"},
        {"46", ">>"},
        {"47", "^"},
        {"48", "^>"},
        {"49", "^>>"},
        {"5A", "vv>"},
        {"50", "vv"},
        {"51", "<v"},
        {"52", "v"},
        {"53", "v>"},
        {"54", "<"},
        {"56", ">"},
        {"57", "<^"},
        {"58", "^"},
        {"59", "^>"},
        {"6A", "vv"},
        {"60", "<vv"},
        {"61", "<<v"},
        {"62", "<v"},
        {"63", "v"},
        {"64", "<<"},
        {"65", "<"},
        {"67", "<<^"},
        {"68", "<^"},
        {"69", "^"},
        {"7A", ">>vvv"},
        {"70", ">vvv"},
        {"71", "vv"},
        {"72", "vv>"},
        {"73", "vv>>"},
        {"74", "v"},
        {"75", "v>"},
        {"76", "v>>"},
        {"78", ">"},
        {"79", ">>"},
        {"8A", "vvv>"},
        {"80", "vvv"},
        {"81", "<vv"},
        {"82", "vv"},
        {"83", "vv>"},
        {"84", "<v"},
        {"85", "v"},
        {"86", "v>"},
        {"87", "<"},
        {"89", ">"},
        {"9A", "vvv"},
        {"90", "<vvv"},
        {"91", "<<vv"},
        {"92", "<vv"},
        {"93", "vv"},
        {"94", "<<v"},
        {"95", "<v"},
        {"96", "v"},
        {"97", "<<"},
        {"98", "<"}
    };

    Dictionary<string, List<string>> memo = new();



    // 94284 correct part 1
    protected override object Solve1(IEnumerable<string> input)
    {
        long result = 0;
        foreach (var line in input)
        {
            var numericValue = long.Parse(line[..^1]);
            long len = NestRobot2(line, 3);
            result += len * numericValue;

            Console.Write($"{line},{numericValue}");
            Console.Write(" ");
            Console.WriteLine(len);
        }

        return result;
    }

    protected override object Solve2(IEnumerable<string> input)
    {
        long result = 0;
        foreach (var line in input)
        {
            var numericValue = long.Parse(line[..^1]);
            long len = NestRobot2(line, 3);
            result += len * numericValue;

            Console.Write($"{line},{numericValue}");
            Console.Write(" ");
            Console.WriteLine(len);
        }

        return result;
    }

    string NestRobot(string input, int depth)
    {
        Console.WriteLine($"Depth={depth}");
        StringBuilder result = new();
        for (var i = 0; i < input.Length - 1; i++)
        {
            var key = input[i..(i + 2)];
            if (moves.ContainsKey(key))
            {
                result.Append(moves[key]);
            }

            result.Append("A");
        }
        return (depth == 1) ? result.ToString() : NestRobot("A"+result.ToString(), depth-1);
    }

    //void NestRobot3(string input, int depth)
    //{
    //    if (depth == 0)
    //    {
    //        return;// (long)input.Length;
    //    }

    //    Console.WriteLine($"Depth={depth}, {input}");
    //    List<string> parts = new();

    //    string input2 = "A" + input;
        
    //    for (var i = 0; i < input2.Length - 1; i++)
    //    {
    //        var key = input2[i..(i + 2)];
    //        if (moves.ContainsKey(key))
    //        {
    //            parts.Add(StringCache.Intern( moves[key]+"A"));// Intern string to save memory on duplicated strings
    //        }

    //    }

    //    memo[input] = parts;

    //    return memo[input].Sum(x => NestRobot2(x, depth - 1));
    //}


    long NestRobot2(string input, int depth)
    {
        if (depth == 0)
        {
            Console.Write(input);
            return (long)input.Length;
        }

        string input2 = "A" + input;

        long len = 0;
        for (var i = 0; i < input2.Length - 1; i++)
        {
            var key = input2[i..(i + 2)];
            if (moves.ContainsKey(key))
            {
                len += NestRobot2(moves[key] + "A", depth - 1);
            }
        }
        return len;
    }
}   

public static class StringCache
{
    private static ConcurrentDictionary<string, string> cache = new(StringComparer.Ordinal);

    public static IEnumerable<string> Intern(IEnumerable<string> strs) => strs.Select(x => cache.GetOrAdd(x, x));
    public static string Intern(string str) => cache.GetOrAdd(str, str);

    public static void Clear() => cache.Clear();
}
