using System.Collections.Immutable;
using AdventOfCode.Solutions.Common;
using MoreLinq;
using AdventOfCode.Solutions.Extensions;

namespace AdventOfCode.Solutions.Days;

using PagePair = (int a, int b);
public class Day05 : BaseDay<(HashSet<PagePair> pagePairs, List<List<int>> pages)>
{
    protected override int DayNumber => 5;

    private readonly List<List<int>> _invalidPageSequences = []; // Calculated in Part 1, used in part 2

    protected override (HashSet<PagePair> pagePairs, List<List<int>> pages) Parse(ImmutableArray<string> input) =>
        input.Split("").Fold((pagepairs, pages) => (
            pagepairs.Select(pair => pair.Split('|').Fold((before, after) => (int.Parse(before), int.Parse(after)))).ToHashSet(),
            pages.Select(pages => pages.Split(',').Select(v => Convert.ToInt32(v)).ToList()).ToList()));

    protected override object Solve1((HashSet<PagePair> pagePairs, List<List<int>> pages) input)
    {
        List<List<int>> valid = new();
        foreach(var p in input.pages)
        {
            List<PagePair> pagePairs = new();
            for (int i = 0; i < p.Count() - 1; i++)
            {
                for (int j = i + 1; j < p.Count(); j++)
                {
                    int a = p[i];
                    int b = p[j];
                    pagePairs.Add((a, b));
                }
            }

            // Inspect every page-pair looking for valid page sequence
            bool isValid = true;
            foreach (var pagePair in pagePairs)
            {
                var contra = (pagePair.b, pagePair.a);
                if (!( input.pagePairs.Contains(pagePair) || !input.pagePairs.Contains(contra)))
                {
                    isValid = false;
                    continue;
                }
            }

            if (isValid)
            {
                valid.Add(p);
            }
            else
            {
                _invalidPageSequences.Add(p);
            }
        }

        int sum = 0;
        foreach (var pages in valid)
        {
            sum += pages[pages.Count/2];
        }
        return sum;
    }

    // Use a form of Quicksort
    protected override object Solve2((HashSet<PagePair> pagePairs, List<List<int>> pages) input)
    {
        List<List<int>> sortedPages = new();

        foreach(var pages in _invalidPageSequences)
        {
            bool didSort = true;
            while (didSort)
            {
                didSort = false;
                for (int i = 0, j = 1; i < pages.Count - 1; i++, j++)
                {
                    int a = pages[i];
                    int b = pages[j];
                    if (input.pagePairs.Contains((a,b)))
                        { continue; }
                    if (input.pagePairs.Contains((b, a)))
                    {
                        pages[i] = b;
                        pages[j] = a;
                        didSort = true;
                        continue;
                    }
                    // Sanity check: there is a problem is this is printed because it means that we are missing a mapping.
                    // Since we are not walking the graph, we have to assume that every required pairing is present in the page pairs
                    Console.WriteLine($"Did not find ({a},{b})"); 
                }
            }
            sortedPages.Add(pages);
        }

        int sum = 0;
        foreach (var pages in sortedPages)
        {
            sum += pages[pages.Count / 2];
        }
        return sum;
    }
}