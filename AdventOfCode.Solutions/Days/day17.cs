using System.Collections.Immutable;

using AdventOfCode.Solutions.Common;
using AdventOfCode.Solutions.Extensions;

using MoreLinq;
namespace AdventOfCode.Solutions.Days;

public class Day17 : BaseDay<(ulong a, int[] program)>
{
    protected override int DayNumber => 17;
    protected override (ulong a, int[] program) Parse(ImmutableArray<string> input)
    {
        var part = input.Split("");
        var (a, b, c) = part.First().Fold((aa, bb, cc) =>
        (
            ulong.Parse(aa[12..]),
            ulong.Parse(bb[12..]),
            ulong.Parse(cc[12..])
        ));
        var prog = part.Last().First().Substring(9).Split(',').Select(x => (int.Parse(x)));
        return (a, prog.ToArray());
    }

    protected override object Solve1((ulong a, int[] program) input)
    {
        return Machine(input.a, input.program);
    }

    protected override object Solve2((ulong a, int[] program) input)
    {
        var queue = new Queue<(ulong a, int index)>();
        queue.Enqueue((0UL, 0));
        while (queue.Any())
        {
            var (A, index) = queue.Dequeue();
            index++;
            if (index > input.program.Length)
                return A;
            A <<= 3;
            for (long i = 7; i >= 0 ; i--)
            {
                var result = Machine(A+(ulong) i, input.program).ToString();
                if (Enumerable.SequenceEqual(input.program[^index..], result.Split(',').TakeLast(index).Select(x => int.Parse(x))))
                    queue.Enqueue((A + (ulong)i, index));
            }
        }

        throw new Exception("Did not reproduce the input");
    }

    object Machine(ulong a, int[] program)
    {
        int instructionPointer = 0;
        ulong A = a;
        ulong B = 0UL;
        ulong C =0UL;
        string outString = "";
        while (instructionPointer < program.Length)
        {
            int code = program[instructionPointer];
            int op = program[instructionPointer + 1];
            switch (code)
            {
                case 0: // adv
                    A >>= (int)ComboOperand(op);
                    break;
                case 1: // bxl
                    B ^= (ulong)op;
                    break;
                case 2: // bst
                    B = ComboOperand(op) & 0b0111;
                    break;
                case 3: // jnz
                    if (A != 0)
                    {
                        instructionPointer = op;
                        instructionPointer-=2;
                    }
                    break;
                case 4: // bxc
                    B ^= C;
                    break;
                case 5: // OUT
                    outString += $"{ComboOperand(op) & 0b0111},";
                    //return outString[..^1];
                    break;
                case 6: // bdv
                    B = A >>> (int)ComboOperand(op);
                    break;
                case 7: // cdv
                    C = A >>> (int)ComboOperand(op);
                    break;
            }
            //Console.WriteLine($"{A},{B},{C}");
            instructionPointer+=2;
        }

        ulong ComboOperand(int op) => op switch
        {
            0 => 0UL,
            1 => 1UL,
            2 => 2UL,
            3 => 3UL,
            4 => A,
            5 => B,
            6 => C,
            _ => throw new InvalidOperationException()
        };
        return outString[..^1]; // ^1 - remove trailing comma
    }
}
    