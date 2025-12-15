using System;
using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using NatLib.Core;

namespace NatLib.Benchmark
{
    //[SimpleJob(launchCount: 1, 5, 5, 1000)]
    [MemoryDiagnoser]
    public class Unit2WithComposed
    {
        private Unit2<double> u1 = new(1.0, 2.0);
        private Unit2<double> u2 = new(3.0, 4.0);
        private Point2<double> p1 = new(1.0, 2.0);
        private Point2<double> p2 = new(3.0, 4.0);
        private Vector2<double> v1 = new(1.0, 2.0);
        private Vector2<double> v2 = new(3.0, 4.0);

        private double db1 = 2.0;
        private double db2 = 2.0;

        private const int Iterations = 1_000_000;

        private readonly Consumer _consumer = new();

        #region Properties read-write

        [Benchmark]
        public void Unit2_ReadProperty()
        {
            var val = u1;

            for (int i = 0; i < Iterations; i++)
            {
                _consumer.Consume(val.X);
            }
        }

        [Benchmark]
        public void Unit2_WriteProperty()
        {
            var val = u1;
            var v = db1;

            for (int i = 0; i < Iterations; i++)
            {
                val.X = v;
            }

            _consumer.Consume(val);
        }

        [Benchmark]
        public void ComposedUnit2_ReadProperty()
        {
            var val = p1;

            for (int i = 0; i < Iterations; i++)
            {
                _consumer.Consume(val.X);
            }
        }

        [Benchmark]
        public void ComposedUnit2_WriteProperty()
        {
            var val = p1;
            var v = db1;

            for (int i = 0; i < Iterations; i++)
            {
                val.X = v;
            }

            _consumer.Consume(val);
        }

        #endregion

        #region Creating structs

        [Benchmark]
        public void Unit2_Create()
        {
            var d1 = db1;
            var d2 = db2;
            for (int i = 0; i < Iterations; i++)
            {
                _consumer.Consume(new Unit2<double>(d1, d2));
            }
        }

        [Benchmark]
        public void Composed_Create()
        {
            var d1 = db1;
            var d2 = db2;
            for (int i = 0; i < Iterations; i++)
            {
                _consumer.Consume(new Point2<double>(d1, d2));
            }
        }

        #endregion

        #region Implicit Conversions

        [Benchmark]
        public void Unit2ToComposed()
        {
            var loc = u1;
            for (var i = 0; i < Iterations; i++)
            {
                _consumer.Consume<Point2<double>>(loc);
            }
        }

        [Benchmark]
        public void ComposedToUnit2()
        {
            var loc = p1;
            for (var i = 0; i < Iterations; i++)
            {
                _consumer.Consume<Unit2<double>>(loc);
            }
        }

        #endregion

        #region Arithmetic operations with object

        [Benchmark]
        public void Unit2_ObjectArithmetic()
        {
            var loc1  = u1;
            var loc2 = u2;

            for (int i = 0; i < Iterations; i++)
            {
                _consumer.Consume(loc1 + loc2);
            }
        }

        [Benchmark]
        public void Composed_ObjectArithmetic()
        {
            var loc1  = p1;
            var loc2 = p2;

            for (int i = 0; i < Iterations; i++)
            {
                _consumer.Consume(loc1 + loc2);
            }
        }

        #endregion

        #region Arithmetic operations with T (operation + consumption)
        
        [Benchmark]
        public void Unit2_TArithmetic()
        {
            var loc1  = u1;
            var loc2 = db1;

            for (int i = 0; i < Iterations; i++)
            {
                _consumer.Consume(loc1 + loc2);
            }
        }

        [Benchmark]
        public void Composed_TArithmetic()
        {
            var loc1  = p1;
            var loc2 = db1;

            for (int i = 0; i < Iterations; i++)
            {
                _consumer.Consume(loc1 + loc2);
            }
        }

        #endregion

        #region Equality comparers IEquatable<T> and override Equals(object? obj)

        [Benchmark]
        public void Unit2_IEquatable()
        {
            var loc1  = u1;
            var loc2 = u2;

            for (int i = 0; i < Iterations; i++)
            {
                _consumer.Consume(loc1.Equals(loc2));
            }
        }

        [Benchmark]
        public void Composed_IEquatable()
        {
            var loc1  = p1;
            var loc2 = p2;

            for (int i = 0; i < Iterations; i++)
            {
                _consumer.Consume(loc1.Equals(loc2));
            }
        }

        [Benchmark]
        public void Unit2_Equals()
        {
            var loc1 = u1;
            object loc2 = u2;
            
            for (int i = 0; i < Iterations; i++)
            {
                _consumer.Consume(loc1.Equals(loc2));
            }
        }
        
        [Benchmark]
        public void Composed_Equals()
        {
            var loc1 = p1;
            object loc2 = p2;
            
            for (int i = 0; i < Iterations; i++)
            {
                _consumer.Consume(loc1.Equals(loc2));
            }
        }

        #endregion
        
        #region Existing structs operations
        
        [Benchmark]
        public void Unit2_Existing()
        {
            var loc1  = u1;
            var loc2 = db1;

            for (var i = 0; i < Iterations; i++)
            {
                loc1.Add(loc2);
            }
            
            _consumer.Consume(loc1);
        }

        [Benchmark]
        public void Composed_Existing()
        {
            var loc1  = p1;
            var loc2 = db1;

            for (var i = 0; i < Iterations; i++)
            {
                loc1.Add(loc2);
            }
            
            _consumer.Consume(loc1);
        }
        
        #endregion
    }
}