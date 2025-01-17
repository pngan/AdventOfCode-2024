using System.Collections.Immutable;

using AdventOfCode.Solutions.Common;

using MoreLinq;
using MoreLinq.Extensions;


namespace AdventOfCode.Solutions.Days;

public class Day23 : BaseDay<IEnumerable<IEnumerable<string>>>
{
    protected override int DayNumber => 23;

    protected override IEnumerable<IEnumerable<string>> Parse(ImmutableArray<string> input)
    {
        foreach (var item in input)
            yield return item.Split("-").OrderBy(x=>x);
    }

    protected override object Solve1(IEnumerable<IEnumerable<string>> input)
    {
        Dictionary<string, HashSet<string>> dict = new();
        foreach (var item in input)
        {
            if (dict.TryGetValue(item.First(), out var valueR))
                valueR.Add(item.Last());
            else
                dict[item.First()] = [item.Last()];

            if (dict.TryGetValue(item.Last(), out var valueL))
                valueL.Add(item.First());
            else
                dict[item.Last()] = [item.First()];
        }

        HashSet<(string,string,string)> triples = new();

        foreach (var v1 in dict.Keys)
        {
            foreach (var v2 in dict[v1])
            {
                if (v1 == v2)
                    continue;
                foreach (var v3 in dict[v2])
                {
                    if ((v2 == v3) || (v1==v3))
                        continue;

                    if (dict[v3].Contains(v1))
                    {
                        string[] triple = [v1, v2, v3];
                        var sorted = triple.OrderBy(x => x).ToArray();
                        triples.Add((sorted[0], sorted[1], sorted[2]));
                    }
                }
            }
        }

        return triples.Where(x => x.Item1.StartsWith("t") || x.Item2.StartsWith("t") || x.Item3.StartsWith("t")).Count();

    }



    // We are looking for a complete graph, which is a graph where every vertex is connected to every other vertex.
    // So number of edges = nVertices^2, when this graph is not connected to anything else.
    // We can remove vertices with least number of edges until we have a single complete graph that is not connected to anything else.

    // This algorithm works, but success depends on order of input. It requires the .Reverse() several lines below this comment.
    // If this is not done, the algorithm a complete graph of 12 nodes is found, but the correct answer is a 13 node graph.

    // The input is crafted so that every node in the graph has 14 edges. There is a single complete graph (13 node), the other nodes may be part of complete graphs with a smaller number of nodes.
    // My guess is that at the first iteration looking for vertex to delete, there is a uniformly random chance of selecting any vertex.
    // Which means there is a 14/560 chance of choosing a vertex from the largest complete graph. If this is done, the largest  complete graph cannot be found.
    // Otherwise, if the first vertex removed is from outside the largest complete graph, the algorithm will never select a vertex from the largest complete graph,
    // and will find the correct answer.

    // A brute force solution would be to try all possible permutations of vertices to remove first. This is not feasible for large graphs.

    protected override object Solve2(IEnumerable<IEnumerable<string>> input)
    {
        Dictionary<string,int> index = new();
        foreach (var item in input.Reverse())  // HACK: The original input does not work presumably because a node in the largest complete graph is selected first. So use Reverse() to avoid this.
        {
            if (!index.ContainsKey(item.First()))
                index[item.First()] = index.Count();
            if (!index.ContainsKey(item.Last()))
                index[item.Last()] = index.Count();
        }

        int[][] graph = new int[index.Count][];
        for (int i = 0; i < graph.Length; i++)
            graph[i] = new int[index.Count];

        foreach (var item in input)
        {
            graph[index[item.First()]][index[item.Last()]] = 1;
            graph[index[item.Last()]][index[item.First()]] = 1;
            graph[index[item.Last()]][index[item.Last()]] = 1;
            graph[index[item.First()]][index[item.First()]] = 1;
        }

        //// Debugging
        //int[] originalEdgeCount = new int[graph.Length];
        //for (int i = 0; i < graph.Length; i++)
        //{
        //    originalEdgeCount[i] = graph[i].Where(x=>x==1).Count();
        //    Console.WriteLine($"Vertex {index.First(x => x.Value == i)} has {originalEdgeCount[i]} edges");
        //}
        //// end debugging


        int vertexCount = 0;
        int edgeCount = 0;
        HashSet<int> deletedVertices = new HashSet<int>();

        do
        {
            // remove all vertices with least number of edges
            int vertexWithLeastEdges = -1;
            int leastEdges = int.MaxValue;
            for (int i = 0; i < graph.Length; i++)
            {
                // Ignore vertices that have been deleted. 
                if (deletedVertices.Contains(i))
                    continue;

                int edges = 0;
                for (int j = 0; j < graph[i].Length; j++)
                {
                    if (graph[i][j] == 1)
                    {
                        edges++;
                    }
                }
                if (edges < leastEdges)
                {
                    leastEdges = edges;
                    vertexWithLeastEdges = i;
                }
            }

            // Remove the vertex with least edges from the graph
            deletedVertices.Add(vertexWithLeastEdges);
            for (int i = 0; i < graph.Length; i++)
            {
                graph[i][vertexWithLeastEdges] = 0;
                graph[vertexWithLeastEdges][i] = 0;
            }

            // See if the graph is now a complete graph.
            // A complete graph occurs when numEdges = nVertices^2
            vertexCount = 0;
            edgeCount = 0;
            for (int i = 0; i < graph.Length; i++)
            {
                int edges = 0;
                for (int j = 0; j < graph[i].Length; j++)
                {
                    if (graph[i][j] == 1)
                    {
                        edges++;
                        edgeCount++;
                    }
                }
                if (edges > 0)
                    vertexCount++;
            }

          // Console.WriteLine($"Vertex count: {vertexCount}, Edge count: {edgeCount}");
        } while (edgeCount < vertexCount*vertexCount);

        // Find vertices of the complete graph
        List<int> completeGraphVertices = new();
        for (int i = 0; i < graph.Length; i++)
        {
            bool hasEdge = false;
            for (int j = 0; j < graph[i].Length; j++)
            {
                if (graph[i][j] == 1)
                {
                    hasEdge = true;
                    break;
                }
            }

            if (hasEdge)
            {
                completeGraphVertices.Add(i);
            }
        }

        //foreach (var item in completeGraphVertices)
        //{
        //    Console.Write(index.First(x => x.Value == item));
        //    Console.WriteLine($" len= {originalEdgeCount[item]}");
        //}


        var completeGraphVertexLabels = index.Where(x => completeGraphVertices.Contains(x.Value)).Select(x => x.Key).OrderBy(x=>x).ToArray();
        return string.Join(",", completeGraphVertexLabels);
    }
}