using System.Collections.Immutable;

using AdventOfCode.Solutions.Common;

using MoreLinq;

namespace AdventOfCode.Solutions.Days;

using Data07 = (ulong test, List<ulong> value);
public class Day07 : BaseDay<IEnumerable<Data07>>
{
    protected override int DayNumber => 7;

    protected override IEnumerable<(ulong test, List<ulong> value)> Parse(ImmutableArray<string> input) =>
        input.Select(line => line.Split(':').Fold((test, values) => (
                    ulong.Parse(test.Trim()),
                        values.Trim().Split(" ").Select(v => ulong.Parse(v)).ToList()
                    )));

    protected override object Solve1(IEnumerable<(ulong test, List<ulong> value)> input) => Solve(input, "+*", ApplyOperationPart1);
    protected override object Solve2(IEnumerable<(ulong test, List<ulong> value)> input) => Solve(input, "+*|", ApplyOperationPart2);

    private ulong Solve(IEnumerable<(ulong test, List<ulong> value)> input, string operations, Func<char, ulong, ulong, ulong> ApplyOperationFunc)
    {
        ulong result = 0;
        foreach (var line in input)
        {
            int numOperators = line.value.Count() - 1;
            foreach (var operators in GetOperators(operations, numOperators))
            {
                ulong calc = line.value[0];
                int i = 1;
                foreach (var op in operators)
                {
                    calc = ApplyOperationFunc(op, calc, line.value[i]);
                    i++;
                }

                if (line.test == calc)
                {
                    result += line.test;
                    break;
                }
            }
        }
        return result;
    }


    ulong ApplyOperationPart1(char operation, ulong currentValue, ulong operand)
    {
        return operation switch
        {
            '+' => currentValue + operand,
            '*' => currentValue * operand,
            _ => throw new InvalidOperationException($"Unknown operation '{operand}'"),
        };
    }

    ulong ApplyOperationPart2(char operation, ulong currentValue, ulong operand)
    {
        return operation switch
        {
            '+' => currentValue + operand,
            '*' => currentValue * operand,
            '|' => currentValue * (ulong)Math.Pow(10, NumDigits(operand)) + operand,
            _ => throw new InvalidOperationException($"Unknown operation '{operand}'"),
        };
    }

    int NumDigits(ulong v)
    {
        int i = 1;
        while ((v /= 10) > 0) { i++; }
        return i;
    }

    public static IEnumerable<IEnumerable<char>> GetOperators(string alphabet, Int32 length)
    {
        if (length <= 0)
            yield break;

        foreach (char c in alphabet)
        {
            if (length > 1)
            {
                foreach (String restWord in GetOperators(alphabet, length - 1))
                    yield return c + restWord;
            }
            else
                yield return "" + c;
        }
    }
}