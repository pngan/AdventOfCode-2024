using System.Collections.Immutable;
using System.Text;

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


    // This function does not compute the result! It outputs a dot file which can be rendered using Graphviz.

    // The return statement in this function was hand crafted after visually inspecting the 
    // circuit which was rendered using https://dreampuf.github.io/GraphvizOnline/?engine=dot
    // This function created a dot file located in "My Documents\Aoc-2024Day24.dot"
    // The puzzle input implements a full adder for two binary numbers as described in this wiki 
    // article https://en.wikipedia.org/wiki/Adder_(electronics)
    // 
    // The wiring in the circuit has 4 faults, which can be remedied by swapping four pairs of 
    // wiring connections. A visual inspection will readily reveal a break in the pattern.
    //
    // Click this image to see the rendered result: "AOC2024-day24-part2.png"
    protected override object Solve2((Dictionary<string, bool>, Gate[]) input)
    {
        StringBuilder sb = new();
        // Output dot file
        sb.AppendLine("digraph G {");
        OutputInputsX(input.Item1.Keys, sb);
        OutputInputsY(input.Item1.Keys, sb);
        OutputOutputs(input.Item2, sb);
        OutputAndGates(input.Item2, sb);
        OutputOrGates(input.Item2, sb);
        OutputXorGates(input.Item2, sb);
        OutputCircuit(input.Item2, sb);
        sb.AppendLine("}");

        string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        File.WriteAllText(Path.Combine(docPath, "Aoc-2024Day24.dot"), sb.ToString());

        return string.Join(",", "vwr,z06,z16,kfs,hcm,gfv,tqm,z11".Split(',').Order());
    }

    void OutputCircuit(Gate[] gates, StringBuilder sb)
    {
        foreach (var gate in gates)
            sb.AppendLine($"{gate.In1} -> {gate.Output}; {gate.In2} -> {gate.Output}");
    }
    void OutputAndGates(Gate[] gates, StringBuilder sb)
    {
        sb.AppendLine("  subgraph gates_and {\r\n    node [style=filled,color=lightgreen];");
        foreach (var gate in gates.Where(g => g.Op == "AND"))
            sb.Append($"{gate.Output}; ");
        sb.AppendLine("\r\n  }");
    }
    void OutputOrGates(Gate[] gates, StringBuilder sb)
    {
        sb.AppendLine("  subgraph gates_or {\r\n    node[style = filled, color = yellow];");
        foreach (var gate in gates.Where(g => g.Op == "OR"))
            sb.Append($"{gate.Output}; ");
        sb.AppendLine("\r\n  }");
    }
    void OutputXorGates(Gate[] gates, StringBuilder sb)
    {
        sb.AppendLine("  subgraph gates_xor {\r\n    node[style = filled, color = lightskyblue];");
        foreach (var gate in gates.Where(g => g.Op == "XOR"))
            sb.Append($"{gate.Output}; ");
        sb.AppendLine("\r\n  }");
    }

    void OutputInputsX(IEnumerable<string> wires, StringBuilder sb)
    {
        sb.AppendLine("  subgraph input_x {\r\n    node [style=filled,color=white];");
        bool isFirst = true;
        foreach (var input in wires.Where(x => x.StartsWith("x")))
        {
            if (!isFirst)
                sb.Append($" ->");
            isFirst = false;
            sb.Append($"{input}");
        }
        sb.AppendLine(";\r\n  }");
    }

    void OutputInputsY(IEnumerable<string> wires, StringBuilder sb)
    {
        sb.AppendLine("  subgraph input_y {\r\n    node [style=filled,color=white];");
        bool isFirst = true;
        foreach (var input in wires.Where(x => x.StartsWith("y")))
        {
            if (!isFirst)
                sb.Append($" ->");
            isFirst = false;
            sb.Append($"{input}");
        }
        sb.AppendLine(";\r\n  }");
    }
    void OutputOutputs(Gate[] gates, StringBuilder sb)
    {
        sb.AppendLine("  subgraph output_z {");
        bool isFirst = true;
        foreach (var z in gates.Select(x => x.Output).Where(g => g.StartsWith("z")).Order())
        {
            if (!isFirst)
                sb.Append($" ->");
            isFirst = false;
            sb.Append($"{z}");
        }

        sb.AppendLine(";\r\n  }");
    }
}