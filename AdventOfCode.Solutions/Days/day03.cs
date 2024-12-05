using System.Collections.Immutable;
using System.Text.RegularExpressions;
using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions.Days;

public class Day03 : BaseDay<string>
{
    protected override int DayNumber => 3;

    protected override string Parse(ImmutableArray<string> input) => string.Join("", input);

    protected override object Solve1(string input)
    {
        Regex regex = new Regex(@"^\((\d{1,3}),(\d{1,3})\)");
        long result = 0;
        foreach (var item in input.Split("mul"))
        {
            Match match = regex.Match(item);
            if (match.Success)
            {
                result += (long.Parse(match.Groups[1].Value) * long.Parse(match.Groups[2].Value));
            }
        }
        return result;
    }

    protected override object Solve2(string input)
    {
        long result = 0;
        foreach (var item in input.Split("do()"))    // Get all segments following a do()
        {
            var i = item.IndexOf("don't()");        // Ignore parts of segments beyond a don't()
            var s = (i == -1) ? item : item[..i];
            result += (long) Solve1(s);
        }
        return result;
    }
}