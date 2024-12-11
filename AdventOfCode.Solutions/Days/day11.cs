using System.Collections.Immutable;
using System.Linq;

using AdventOfCode.Solutions.Common;

using MoreLinq;

namespace AdventOfCode.Solutions.Days;

using Stone = (string value, int depth);

public class Day11 : BaseDay<IEnumerable<string>>
{
    protected override int DayNumber => 11;

    protected override IEnumerable<string> Parse(ImmutableArray<string> input)
    {
        return input.First().Split(' ');
    }

    protected override object Solve1(IEnumerable<string> input)
    {
        List<string> stones = new(input);
        for (int blink = 0; blink < 25; blink++)
        {
            for (int i = 0; i < stones.Count; i++)
            {
                if (stones[i] == "0")
                    stones[i] = "1";
                else if (stones[i].Length % 2 == 0)
                {
                    var evenLength = stones[i];
                    stones[i] = ulong.Parse(evenLength[..(evenLength.Length / 2)]).ToString(); ;
                    stones.Insert(++i, ulong.Parse(evenLength[(evenLength.Length / 2)..]).ToString());
                }
                else
                {
                    ulong value = ulong.Parse(stones[i]);
                    checked
                    {
                        value = value * 2024UL;
                        stones[i] = value.ToString();
                    }
                }
            }
        }
        return stones.Count;
    }
    const int maxDepth = 75;

    protected override object Solve2(IEnumerable<string> input)
    {
        ulong count = 0;
        foreach (var item in input)
        {
            Console.Write($" {item}");
            count += Process((item, 0));
        }

        Console.WriteLine("");
        return count;
    }

    Dictionary<Stone, ulong> _memo = new();

    ulong Process(Stone s)
    {
        ulong count = 0;
        if (s.depth == maxDepth)
        {
            return 1;
        }

        if (s.value == "0")
        {
            Stone nextStone = ("1", s.depth + 1);
            if (_memo.TryGetValue(nextStone, out ulong c))
                count = c;
            else
            {
                count = Process(nextStone);
                _memo[nextStone] = count;
            }
        }

        else if (s.value.Length % 2 == 0)
        {
            var evenLength = s.value;
            Stone nextStone = (evenLength, s.depth + 1);
            if (_memo.TryGetValue(nextStone, out ulong cc))
                count = cc;
            else
            {
                Stone nextStoneL = (ulong.Parse(evenLength[..(evenLength.Length / 2)]).ToString(), s.depth + 1);
                ulong countl = Process(nextStoneL);


                Stone nextStoneR = (ulong.Parse(evenLength[(evenLength.Length / 2)..]).ToString(), s.depth + 1);
                ulong countr = Process(nextStoneR);

                count = countl + countr;
                _memo[nextStone] = count;
            }
        }
        else
        {
            checked // Detect multiplication overflow
            {
                ulong v = ulong.Parse(s.value) * 2024UL;
                Stone nextStone = (v.ToString(), s.depth + 1);
                count = Process(nextStone);
                //_memo[nextStone] = count;
            }
        }

        return count;
    }
}