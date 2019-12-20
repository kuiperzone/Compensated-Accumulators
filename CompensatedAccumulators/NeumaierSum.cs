//---------------------------------------------------------------------------
// PROJECT      : Compensated Accumulators
// COPYRIGHT    : Andy Thomas (C) 2019
// WEB URL      : https://kuiper.zone
// LICENSE      : GPLv3
//---------------------------------------------------------------------------

using System;

namespace CompensatedAccumulators
{
    /// <summary>
    /// Neumaier variation of the Kahan algorithm, also referred to as "Kahan–Babuška".
    /// </summary>
    public class NeumaierSum : ICompensatedSum
    {
        private double _kc;
        private double _sum;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NeumaierSum()
        {
        }

        /// <summary>
        /// Constructor with initial value.
        /// </summary>
        public NeumaierSum(double x)
        {
            Value = x;
        }

        /// <summary>
        /// Implements <see cref="ICompensatedSum.Value"/>.
        /// </summary>
        public double Value
        {
            get { return _sum + _kc; }

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
            double t = _sum + x;

            if (Math.Abs(_sum) >= Math.Abs(x))
            {
                _kc += _sum - t + x;
            }
            else
            {
                _kc += x - t + _sum;
            }

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
            var clone = new NeumaierSum();
            clone._sum = _sum;
            clone._kc = _kc;
            return clone;
        }
    }
}
