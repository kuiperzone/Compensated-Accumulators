//---------------------------------------------------------------------------
// PROJECT      : Compensated Accumulators
// COPYRIGHT    : Andy Thomas (C) 2019
// WEB URL      : https://kuiper.zone
// LICENSE      : GPLv3
//---------------------------------------------------------------------------

namespace CompensatedAccumulators
{
    /// <summary>
    /// An interface for a compensated sum algorithm. A "compensated sum" offers reduced numerical
    /// error when summing finite precision values in comparison to "naive" summation.
    /// </summary>
    public interface ICompensatedSum
    {
        /// <summary>
        /// Get or sets the current summation value.
        /// </summary>
        double Value { get; set; }

        /// <summary>
        /// Adds 'x' to the <see cref="Value"/> sum, using a compensation technique designed to reduce numerical
        /// error.
        void Add(double x);

        /// <summary>
        /// Sets the sum value to zero and resets initial state.
        /// </summary>
        void Clear();

        /// <summary>
        /// Creates a "deep" copy of the instance with the same value and internal state. Returns the new instance.
        /// </summary>
        ICompensatedSum Clone();
    }
}
