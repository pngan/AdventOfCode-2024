using System.Collections.Immutable;

using AdventOfCode.Solutions.Common;
using AdventOfCode.Solutions.Extensions;
using MoreLinq;


namespace AdventOfCode.Solutions.Days;


public record Gate(string In1, string In2, string Op, string Output);

public class Day24 : BaseDay<(Dictionary<string, bool>, Gate[])>
{
    protected override int DayNumber => 24;

    protected override (Dictionary<string, bool>, Gate[]) Parse(ImmutableArray<string> input) => input
        .Split("")
        .Fold((p1, p2) => (
            p1.Select(_ => _.Split(": ")).ToDictionary(_ => _[0], _ => _[1] is "1"),
            p2.Select(_ => _.Split(' ').Fold((a, b, c, _, d) => new Gate(a, c, b, d))).ToArray()
        ));

    // ans 50411513338638

    protected override object Solve1((Dictionary<string, bool>, Gate[]) input)
    {
        Dictionary<string, bool?> wires = new();
        // Populate wires with known inputs
        foreach (var (key, value) in input.Item1)
            wires[key] = value;

        // Populate wires with known gates, initialize them to null
        foreach (var gate in input.Item2)
            wires[gate.Output] = null;

        // Loop until all wires are resolved
        while (wires.Values.Any(_ => _ is null))
        {
            foreach (var gate in input.Item2)
            {
                if (wires[gate.Output] is not null)
                    continue;
                bool? in1 = wires[gate.In1];
                bool? in2 = wires[gate.In2];
                if (in1 is null || in2 is null) // Skip evaluating gate when one or more inputs are not resolved
                    continue;
                wires[gate.Output] = gate.Op switch
                {
                    "AND" => in1.Value & in2.Value,
                    "OR" => in1.Value | in2.Value,
                    "XOR" => in1.Value ^ in2.Value,
                    _ => throw new InvalidOperationException()
                };
            }
        }

        // Get outputs as 0 or 1
        var output = wires.Where(_ => _.Key.StartsWith("z")).OrderBy(_ => _.Key).Select(_ => _.Value.Value ? 1L : 0L);

        // Convert to decimal
        return output.Select((v,i) => v << i).Sum(x => x);
    }

    protected override object Solve2((Dictionary<string, bool>, Gate[]) input)
    {
        throw new NotImplementedException();
    }
}