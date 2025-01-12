using System.Collections.Immutable;

using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions.Days;

public class Day21 : BaseDay<IEnumerable<string>>
{
    protected override int DayNumber => 21;

    protected override IEnumerable<string> Parse(ImmutableArray<string> input) => input;

    // This table encodes the following rules:
    // 1. If a sequence contains a <, then do it first because this will lead to a double << at the next level instead of a separated <. While a < is
    // expensive because it is far from A, a double << can be performed with additional cost of 1 (AA vs A). While a split < will require the expensive cost to 
    // be costed twice.
    // E.g. see the dictionary entry {">^", "<^"},
    // Demonstration:
    // Cheap:     <^A (3) -> v<<A>^A>A (9) -> <vA<AA>>^A vA<^A>A vA^A (21)
    // Expensive: ^<A (3) -> <Av<A>>^A (9) -> v<<A>>^A <vA<A>>^A vAA<^A>A (25)
    // 2. If a sequence contains a v , then do it first, because again it will lead to a double << at the next level instead of a separated <. 
    // E.g. see the dictionary entry {"^<", "v<"}
    // Demonstration:
    // Cheap: v>A (3) -> <vA>A^A (7) -> v<<A>A^>AvA^A<A>A (17)
    // Expensive: >vA (3) -> vA<A>^A (7) -> <vA^> Av<<A>>^AvA<^A> A (21)
    // 3. Rules 1 and 2 have exceptions where the sequence would run over an empty space.
    // E.g. see the dictionary entry {"A<", "v<<"} (cannot following rule 1)
    // 
    // Because of all these bespoke cases, the rules were hard coded in a dictionary rather than trying to find a general solution.
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

    Dictionary<(string, int), long> _memo = new();

    protected override object Solve1(IEnumerable<string> input) => Solve(input, 3);

    protected override object Solve2(IEnumerable<string> input) => Solve(input, 26);

    long Solve(IEnumerable<string> input, int numberRobots)
    {
        long result = 0;
        foreach (var line in input)
        {
            var numericValue = long.Parse(line[..^1]);
            long len = NestRobot(line, numberRobots);
            result += len * numericValue;
        }

        return result;
    }

    private long NestRobot(string input, int depth)
    {
        if (_memo.TryGetValue((input,depth), out long length)) // Short circuit calc using memoization
            return length;

        if (depth == 0) // Terminate recursion when depth is 0
            return (long)input.Length;

        string input2 = "A" + input; // Each input has implied start position of "A"

        long len = 0;
        for (var i = 0; i < input2.Length - 1; i++)
        {
            var key = input2[i..(i + 2)];
            if (moves.ContainsKey(key))
            {
                len += NestRobot(moves[key] + "A", depth - 1); // Recurse with new input and depth. Each command is activated by "A"
            }
        }
        _memo[(input, depth)] = len; // Record calculated length in memoization
        return len;
    }
}