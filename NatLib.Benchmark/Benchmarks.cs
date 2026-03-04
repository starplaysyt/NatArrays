using System;
using System.Numerics;
using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using CommandLine;
using NatLib.Core;

namespace NatLib.Benchmark
{
    //[SimpleJob(launchCount: 1, 5, 5, 1000)]
    [MemoryDiagnoser]
    public class Unit2WithComposed
    {
        private Vector2 v1 = new(1.0f, 2.0f);
        private Vector2 v2 = new(3.0f, 4.0f);

        private float db1 = 2.0f;
        private float db2 = 2.0f;

        private const int Iterations = 1_000_000;

        private readonly Consumer _consumer = new();
    }
}