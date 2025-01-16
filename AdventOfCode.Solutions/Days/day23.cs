using System.Collections.Immutable;

using AdventOfCode.Solutions.Common;
using AdventOfCode.Solutions.Extensions;
using MoreLinq;
using MoreLinq.Extensions;


namespace AdventOfCode.Solutions.Days;

public class Day23 : BaseDay<IEnumerable<IEnumerable<string>>>
{
    protected override int DayNumber => 23;

    protected override IEnumerable<IEnumerable<string>> Parse(ImmutableArray<string> input)
    {
        foreach (var item in input)
            yield return item.Split("-").OrderBy(x=>x);
    }

    protected override object Solve1(IEnumerable<IEnumerable<string>> input)
    {
        Dictionary<string, HashSet<string>> dict = new();
        foreach (var item in input)
        {
            if (dict.TryGetValue(item.First(), out var valueR))
                valueR.Add(item.Last());
            else
                dict[item.First()] = [item.Last()];

            if (dict.TryGetValue(item.Last(), out var valueL))
                valueL.Add(item.First());
            else
                dict[item.Last()] = [item.First()];
        }

        HashSet<(string,string,string)> triples = new();

        foreach (var v1 in dict.Keys)
        {
            foreach (var v2 in dict[v1])
            {
                if (v1 == v2)
                    continue;
                foreach (var v3 in dict[v2])
                {
                    if ((v2 == v3) || (v1==v3))
                        continue;

                    if (dict[v3].Contains(v1))
                    {
                        string[] triple = [v1, v2, v3];
                        var sorted = triple.OrderBy(x => x).ToArray();
                        triples.Add((sorted[0], sorted[1], sorted[2]));
                    }
                }
            }
        }

        //foreach (var triple in triples)
        //{
        //    Console.WriteLine(triple);
        //}

        return triples.Where(x => x.Item1.StartsWith("t") || x.Item2.StartsWith("t") || x.Item3.StartsWith("t")).Count();

    }

    protected override object Solve2(IEnumerable<IEnumerable<string>> input)
    {
        HashSet<string> visited = new();
        foreach (var item in input)
        {
            visited.Add(item.First());
            visited.Add(item.Last());
        }

        return visited.Count();
    }
}