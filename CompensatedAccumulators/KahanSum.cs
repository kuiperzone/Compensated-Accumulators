//---------------------------------------------------------------------------
// PROJECT      : Compensated Accumulators
// COPYRIGHT    : Andy Thomas (C) 2019
// WEB URL      : https://kuiper.zone
// LICENSE      : GPLv3
//---------------------------------------------------------------------------

namespace CompensatedAccumulators
{
    /// <summary>
    /// A classic "Kahan" implementation of <see cref="ICompensatedSum"/>.
    /// </summary>
    public class KahanSum : ICompensatedSum
    {
        private double _kc;
        private double _sum;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public KahanSum()
        {
        }

        /// <summary>
        /// Constructor with initial value.
        /// </summary>
        public KahanSum(double x)
        {
            Value = x;
        }


        /// <summary>
        /// Implements <see cref="ICompensatedSum.Value"/>.
        /// </summary>
        public double Value
        {
            get { return _sum; }

            set
            {
                _kc = 0;
                _sum = value;
            }
        }

        /// <summary>
        /// Implements <see cref="ICompensatedSum.Add(double)"/>.
        /// </summary>
        public void Add(double x)
        {
            double y = x - _kc;
            double t = _sum + y;

            // Update state
            _kc = t - _sum - y;
            _sum = t;
        }

        /// <summary>
        /// Implements <see cref="ICompensatedSum.Clear()"/>.
        /// </summary>
        public void Clear()
        {
            _kc = 0;
            _sum = 0;
        }

        /// <summary>
        /// Implements <see cref="ICompensatedSum.Clone()"/>.
        /// </summary>
        public ICompensatedSum Clone()
        {
            var clone = new KahanSum();
            clone._sum = _sum;
            clone._kc = _kc;
            return clone;
        }

    }
}
