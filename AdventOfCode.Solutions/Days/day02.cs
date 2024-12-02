using AdventOfCode.Solutions.Common;
using AdventOfCode.Solutions.Extensions;

namespace AdventOfCode.Solutions.Days;

public class Day02 : BaseDay<IEnumerable<List<long>>>
{
    protected override int DayNumber => 2;

    protected override IEnumerable<List<long>> Parse(string[] input) => input.ConvertToLong();

    bool IsAscendingValid(long i, long j) => (j - i) <= 3 && (j - i) > 0;
    bool IsDecendingValid(long i, long j) => (i - j) <= 3 && (i - j) > 0;

    protected override object Solve1(IEnumerable<List<long>> input)
    {
        int count = 0;
        foreach (var item in input)
        {
            if (isValid(item))
                count++;
        }

        return count;
    }

    bool isValid(List<long> row)
    {
        if (row.Count() < 2)
            return false;

        bool isAscending = (row[0] < row[1]);

        for (int i = 0, j = 1; i < row.Count() - 1; i++, j++)
        {
            if (isAscending && !IsAscendingValid(row[i], row[j]))
            {
                return false;
            }

            if (!isAscending && !IsDecendingValid(row[i], row[j]))
            {
                return false;
            }
        }
        return true;
    }

    protected override object Solve2(IEnumerable<List<long>> input)
    {
        int count = 0;
        foreach (var item in input)
        {
            if (isValid(item))
                count++;
            else // Brute force remove one item at a time
            {
                for(int i = 0;  i < item.Count(); i++)
                {
                    var row = item.Where((x,idx) => idx != i).ToList();
                    if (isValid(row))
                    {
                        count++;
                        break;
                    }
                }
            }
        }

        return count;
    }
}
