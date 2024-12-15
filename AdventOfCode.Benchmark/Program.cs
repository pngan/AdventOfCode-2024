// See https://aka.ms/new-console-template for more information
using AdventOfCode.Solutions.Days;

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;


BenchmarkRunner.Run<Day12>(
        ManualConfig
            .Create(DefaultConfig.Instance)
            .AddDiagnoser(MemoryDiagnoser.Default));