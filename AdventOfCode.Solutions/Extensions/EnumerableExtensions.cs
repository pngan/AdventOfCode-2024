using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdventOfCode.Solutions.Extensions;
public static class EnumerableExtensions
{
    public static List<List<T>> Split<T>(this IEnumerable<T> input, Func<T, bool> splitOn)
    {
        var result = new List<List<T>> { new() };

        input.ToList().ForEach(x =>
        {
            if (splitOn(x))
            {
                result.Add(new List<T>());
            }
            else
            {
                result.Last().Add(x);
            }
        });

        return result;
    }

    public static IEnumerable<T> Dump<T>(this IEnumerable<T> input)
    {
        foreach (var element in input)
        {
            Console.WriteLine(element);
            yield return element;
        }
    }

    public static IEnumerable<string> SkipEmptyLines(this IEnumerable<string> input)
    {
        foreach (var element in input)
        {
            if (!string.IsNullOrWhiteSpace(element))
                yield return element;
        }
    }
    public static List<List<long>> ConvertToLong(this IEnumerable<string> input)
    {
        return input.Select(x => x.Split(' ').Select(v => Convert.ToInt64(v)).ToList()).ToList();
    }


    // Operate on every distinct pairing of two enumerables, which must be of same length
    public static IEnumerable<TResult> DistinctPairs<T1, T2, TResult>(
        this IEnumerable<T1> first,
        IEnumerable<T2> second,
        Func<T1, T2, TResult> resultSelector)
    {
        var firstArray = first.ToArray();
        var secondArray = second.ToArray();
        if (firstArray.Length != secondArray.Length)
            throw new ArgumentException("Input arrays are of different lengths.");

        for (int x = 0; x < firstArray.Length; x++)
        {
            for (int y = x+1; y < secondArray.Length; y++)
            {
                yield return resultSelector(firstArray[x], secondArray[y]);
            }
        }
    }
}