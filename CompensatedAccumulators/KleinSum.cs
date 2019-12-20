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
    /// Klein variation of the Kahan algorithm, also referred to as "Kahan-Babuška-Neumaier"
    /// compensated sum. It offers improved accuracy over both <see cref="KahanSum"/> and
    /// <see cref="NeumaierSum"/>.
    /// </summary>
    public class KleinSum : ICompensatedSum
    {
        private double _cs;
        private double _ccs;
        private double _sum;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public KleinSum()
        {
        }

        /// <summary>
        /// Constructor with initial value.
        /// </summary>
        public KleinSum(double x)
        {
            Value = x;
        }

        /// <summary>
        /// Implements <see cref="ICompensatedSum.Value"/>.
        /// </summary>
        public double Value
        {
            get
            {
                return _sum + _cs + _ccs;
            }

            set
            {
                _cs = 0;
                _ccs = 0;
                _sum = value;
            }
        }

        /// <summary>
        /// Implements <see cref="ICompensatedSum.Add(double)"/>.
        /// </summary>
        public void Add(double x)
        {
            double c, cc;
            double t = _sum + x;
            
            if (Math.Abs(_sum) >= Math.Abs(x))
            {
                c = (_sum - t) + x;
            }
            else
            {
                c = (x - t) + _sum;
            }

            _sum = t;
            t = _cs + c;

            if (Math.Abs(_cs) >= Math.Abs(c))
            {
                cc = (_cs - t) + c;
            }
            else
            {
                cc = (c - t) + _cs;
            }

            _cs = t;
            _ccs += cc;
        }

        /// <summary>
        /// Implements <see cref="ICompensatedSum.Clear()"/>.
        /// </summary>
        public void Clear()
        {
            _cs = 0;
            _ccs = 0;
            _sum = 0;
        }

        /// <summary>
        /// Implements <see cref="ICompensatedSum.Clone()"/>.
        /// </summary>
        public ICompensatedSum Clone()
        {
            var clone = new KleinSum();
            clone._cs = _cs;
            clone._ccs = _ccs;
            clone._sum = _sum;
            return clone;
        }

    }
}

