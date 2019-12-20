//---------------------------------------------------------------------------
// PROJECT      : Compensated Accumulators
// COPYRIGHT    : Andy Thomas (C) 2019
// WEB URL      : https://kuiper.zone
// LICENSE      : GPLv3
//---------------------------------------------------------------------------

using System;
using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace CompensatedAccumulators.Test
{
    public class CompensatedSumTest
    {
        /// <summary>
        /// Implementation kind used in test theory data.
        /// </summary>
        public enum SumKind {Naive, Pairwise, Kahan, Neumaier, Klein};

        private readonly ITestOutputHelper helper;

        /// <summary>
        /// Constructor with test helper.
        /// </summary>
        public CompensatedSumTest(ITestOutputHelper hlp)
        {
            helper = hlp;
        }

        /// <summary>
        /// Test result of summation is not lost due to presence (and subtraction) of high values.
        /// Not all accumulators will pass this test.
        /// </summary>
        [Theory]
        [InlineData(SumKind.Neumaier)]
        [InlineData(SumKind.Klein)]
        public void Add_PetersTest_ExpectSumOf2(SumKind kind)
        {
            // https://en.wikipedia.org/wiki/Kahan_summation_algorithm
            var sum = CreateSum(kind);
            sum.Add(1.0);
            sum.Add(1E100);
            sum.Add(1.0);
            sum.Add(-1E100);

            // Only Neumaier and Klein both give correct answer 2.0.
            // Others expected to give 0.
            helper.WriteLine(sum.Value.ToString());

            Assert.Equal(2.0, sum.Value, 10);
        }

        /// <summary>
        /// PSRNG zero sum sequence. Expect better result than naive summation.
        /// </summary>
        [Theory]
        [InlineData(SumKind.Pairwise)]
        [InlineData(SumKind.Kahan)]
        [InlineData(SumKind.Neumaier)]
        [InlineData(SumKind.Klein)]
        [InlineData(SumKind.Naive)]
        public void Add_PsrngZeroSum_ExpectBetterAccuracyThanNaive(SumKind kind)
        {
            const double Spread = 1E7;

            var sum = CreateSum(kind);
            var naive = CreateSum(SumKind.Naive);

            var rand = new Random(2002);

            for(int n = 0; n < 1000000; ++n)
            {
                double x = rand.NextDouble() * Spread;
                sum.Add(x);
                naive.Add(x);
            }

            rand = new Random(2002);

            for(int n = 0; n < 1000000; ++n)
            {
                double x = rand.NextDouble() * Spread;
                sum.Add(-x);
                naive.Add(-x);
            }

            helper.WriteLine("RESULT: " + Math.Abs(sum.Value));
            helper.WriteLine("NAIVE: " + Math.Abs(naive.Value));

            // Since we also test Naive as we want output, test
            // LTE instead of LT.
            Assert.True(Math.Abs(sum.Value) <= Math.Abs(naive.Value));
        }

        /// <summary>
        /// Calling resets fully resets the accumulator.
        /// </summary>
        [Theory]
        [InlineData(SumKind.Pairwise)]
        [InlineData(SumKind.Kahan)]
        [InlineData(SumKind.Neumaier)]
        [InlineData(SumKind.Klein)]
        public void Reset_PerformsFullReset(SumKind kind)
        {
            var sum = CreateSum(kind);

            sum.Add(1.5);
            sum.Clear();
            Assert.Equal(0.0, sum.Value, 10);

            // Restarts
            sum.Add(1.5);
            Assert.Equal(1.5, sum.Value, 10);
        }

        /// <summary>
        /// Measure accumulator performance. Result written to helper. NaiveSum also test for comparison.
        /// </summary>
        [Theory]
        [InlineData(SumKind.Pairwise)]
        [InlineData(SumKind.Kahan)]
        [InlineData(SumKind.Neumaier)]
        [InlineData(SumKind.Klein)]
        [InlineData(SumKind.Naive)]
        public void Add_MeasurePerformance(SumKind kind)
        {
            const long TicksPerSecond = 1000 * 1000 * 10;

            var sum = CreateSum(kind);

            // JIT
            sum.Add(1);
            sum.Clear();

            double elap;
            long count = 0;
            var sw = Stopwatch.StartNew();

            do
            {
                for(int n = 0; n < 1000; ++n)
                {
                    sum.Add(++count);
                }

                elap = (double)sw.ElapsedTicks / TicksPerSecond;

            } while (elap < 1.0);

            helper.WriteLine("Count: " + count);
            helper.WriteLine("Elapsed: " + elap + " sec");

            helper.WriteLine("RATE: " + count / elap + " per sec");
            helper.WriteLine("TIME: " + 1E9 * elap / count  + " ns");
        }

        private static ICompensatedSum CreateSum(SumKind kind)
        {
            switch(kind)
            {
            case SumKind.Pairwise: return new PairwiseSum();
            case SumKind.Kahan: return new KahanSum();
            case SumKind.Neumaier: return new NeumaierSum();
            case SumKind.Klein: return new KleinSum();
            default: return new NaiveSum();
            }
        }

    }
}
