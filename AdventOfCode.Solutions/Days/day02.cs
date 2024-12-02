using AdventOfCode.Solutions.Common;

using Microsoft.CodeAnalysis;

namespace AdventOfCode.Solutions.Days;

public class Day02 : BaseDay<IEnumerable<List<int>>>
{
    protected override int DayNumber => 2;

    protected override IEnumerable<List<int>> Parse(string[] input)
    {
        List<List<int>> result = new();
        foreach (var item in input)
        {
            List<int> row = new();
            foreach (var v in item.Split(' '))
            {
                row.Add(int.Parse(v));
            }
            result.Add(row);
        }
        return result;
    }

    bool isAscendingValid(int i, int j)
    {
        return (j - i) <= 3 && (j - i) > 0;

    }
    bool isDecendingValid(int i, int j)
    {
        return (i - j) <= 3 && (i - j) > 0;

    }

    protected override object Solve1(IEnumerable<List<int>> input)
    {
        int count = 0;
        foreach (var item in input)
        {
            if (isValid(item))
                count++;
        }

        return count;
    }

    bool isValid(List<int> row)
    {
        var isAscending = false;
        if (row.Count() < 2)
            return false;

        isAscending = (row[0] < row[1]);

        for (int i = 0, j = 1; i < row.Count() - 1; i++, j++)
        {
            if (isAscending)
            {
                if (!isAscendingValid(row[i], row[j]))
                {
                    return false;
                }
            }
            else
            {
                if (!isDecendingValid(row[i], row[j]))
                {
                    return false;
                }
            }
        }
        return true;
    }



    protected override object Solve2(IEnumerable<List<int>> input)
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
                    var row = new List<int>(item);
                    row.RemoveAt(i);
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
