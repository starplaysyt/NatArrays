using System;
using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;
using NatLib.Core;

namespace NatLib.Benchmark
{
    [SimpleJob(launchCount: 1, 5, 5, 1000)]
    [MemoryDiagnoser]
    public class VectorBenchmarks
    {
        private Unit2<double> u1 = new(1.0, 2.0);
        private Unit2<double> u2 = new(3.0, 4.0);
        private Point2<double> p1 = new(1.0, 2.0);
        private Point2<double> p2 = new(3.0, 4.0);

        private const int Iterations = 1_000_000;

        [Benchmark]
        public Unit2<double> Unit2_Add_Loop()
        {
            var sum = u1;
            for (int i = 0; i < Iterations; i++)
            {
                sum += u2;
            }
            return sum;
        }

        [Benchmark]
        public Point2<double> Point2_Add_Loop()
        {
            var sum = p1;
            for (int i = 0; i < Iterations; i++)
            {
                sum += p2;
            }
            return sum;
        }

        [Benchmark]
        public Unit2<double> Unit2_Add_ScalarLoop()
        {
            var sum = u1;
            for (int i = 0; i < Iterations; i++)
            {
                sum += 1.0;
            }
            return sum;
        }

        [Benchmark]
        public Point2<double> Point2_Add_ScalarLoop()
        {
            var sum = p1;
            for (int i = 0; i < Iterations; i++)
            {
                sum += 1.0;
            }
            return sum;
        }
    }
}