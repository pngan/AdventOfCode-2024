using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions.Days;

public class Day01 : BaseDay<(List<int> l, List<int> r)>
{
    protected override int DayNumber => 1;

    protected override (List<int>, List<int>) Parse(string[] input)
    {
        List<int> left = new();
        List<int> right = new();

        foreach (var item in input)
        {
            if (item.Split("   ") is [string l, string r])
            {
                left.Add(int.Parse(l));
                right.Add(int.Parse(r));
            }
        }

        return (left.Order().ToList(), right.Order().ToList());
    }

    protected override object Solve1((List<int> l, List<int> r ) input)
    {
        int diff = 0;
        for (int i = 0; i < input.l.Count(); i++)
        {
            diff += (int)(Math.Abs(input.l[i] - input.r[i]));
        }
        return diff;
    }

    protected override object Solve2((List<int> l, List<int> r ) input)
    {
        // Histogram
        Dictionary<int, int> histr = input.r
            .GroupBy(i => i)
            .Select(i => new { i.Key, Count = i.Count() })
            .ToDictionary(i => i.Key, i => i.Count);

        long similarity = 0;
        foreach (int key in input.l)
        {
            if (histr.TryGetValue(key, out int tally))
                similarity += key * tally;
        }

        return similarity;
    }
}