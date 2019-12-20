//---------------------------------------------------------------------------
// PROJECT      : Compensated Accumulators
// COPYRIGHT    : Andy Thomas (C) 2019
// WEB URL      : https://kuiper.zone
// LICENSE      : GPLv3
//---------------------------------------------------------------------------

namespace CompensatedAccumulators.Test
{
    /// <summary>
    /// Naive sum implementation for test comparison only.
    /// </summary>
    public class NaiveSum : ICompensatedSum
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public NaiveSum()
        {
        }

        /// <summary>
        /// Constructor with initial value.
        /// </summary>
        public NaiveSum(double x)
        {
            Value = x;
        }

        /// <summary>
        /// Implements <see cref="ICompensatedSum.Value"/>.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Implements <see cref="ICompensatedSum.Add(double)"/>.
        /// </summary>
        public void Add(double x)
        {
            Value += x;
        }

        /// <summary>
        /// Implements <see cref="ICompensatedSum.Clear()"/>.
        /// </summary>
        public void Clear()
        {
            Value = 0;
        }

        /// <summary>
        /// Implements <see cref="ICompensatedSum.Clone()"/>.
        /// </summary>
        public ICompensatedSum Clone()
        {
            return new NaiveSum(Value);
        }

    }

}
