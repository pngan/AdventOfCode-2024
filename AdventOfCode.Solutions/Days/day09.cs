using System.Collections.Immutable;

using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions.Days;
using Span = (int value, int length);

public class Day09 : BaseDay<ImmutableArray<string>>
{
    protected override int DayNumber => 9;
    const int FREE = -1;

    protected override ImmutableArray<string> Parse(ImmutableArray<string> input) => input;

    protected override object Solve1(ImmutableArray<string> input)
    {
        string compressed = input.First();
        List<int> uncompressed = new();
        int id = 0;
        for (int ii = 0; ii < compressed.Length;)
        {
            int nId = compressed[ii++] - '0';
            uncompressed.AddRange(Enumerable.Repeat(id++, nId));
            if ((ii) == compressed.Length) break;
            int nFree = compressed[ii++] - '0';
            uncompressed.AddRange(Enumerable.Repeat(FREE, nFree));
        }

        int i = 0;
        int j = uncompressed.Count-1;

        do
        {
            while (uncompressed[i] != FREE)
            {
                if (i == j) 
                    break;
                i++;
            }

            while (uncompressed[j] == FREE)
            {
                if (i == j) 
                    break;
                j--;
            }

            int tmp = uncompressed[j];
            uncompressed[j] = uncompressed[i];
            uncompressed[i] = tmp;

        } while (i != j);

        long result = 0;
        for(int k = 0; k < uncompressed.Count && uncompressed[k] != -1; k++)
        {
            result += k * uncompressed[k];
        }
        return result;
    }

    // Define the sequence in terms of Spans, which comprise value and run lengths
    protected override object Solve2(ImmutableArray<string> input)
    {
        string compressed = input.First();
        List<Span> uncompressed = new();
        int id = 0;
        for (int ii = 0; ii < compressed.Length;)
        {
            int nId = compressed[ii++] - '0';
            uncompressed.Add((id++, nId));
            if ((ii) == compressed.Length) break;
            int nFree = compressed[ii++] - '0';
            if (nFree > 0)
                uncompressed.Add((FREE, nFree));
        }
        id--; // Adjust to give highest id value


        int i = 0;

        for (;  id >= 0; id--)
        {
            // Find span with id
            var spanIdx = uncompressed.FindLastIndex(span => span.value == id);
            int spanLength = uncompressed[spanIdx].length;

            // Find first gap big enough to accomodate span
            var freeIdx = uncompressed.FindIndex(free => free.value == FREE && free.length >= spanLength);

            if (freeIdx < 0 || freeIdx > spanIdx)
                continue;   // Skip when no free space found or space is to right of span

            // Reduce free space length to accomodate span
            var freeSpan = uncompressed[freeIdx];
            uncompressed[freeIdx] = (-1, freeSpan.length - uncompressed[spanIdx].length);

            // Insert span into free space
            uncompressed.Insert(freeIdx, uncompressed[spanIdx]);

            // Replace moved span with free space
            var replacementFreeSpan = uncompressed[spanIdx + 1];// +1 because an extra entry was added
            uncompressed[spanIdx + 1] = (-1, replacementFreeSpan.length);
        }

        long result = 0;
        int runningIdx = 0;
        for (int k = 0; k < uncompressed.Count; k++)
        {
            var segment = uncompressed[k];
            for(int z = 0; z < segment.length ; z++, runningIdx++)
            {
                if (segment.value != -1)
                    result += runningIdx * segment.value;
            }
        }
        return result;
    }
}