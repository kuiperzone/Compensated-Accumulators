//---------------------------------------------------------------------------
// PROJECT      : Compensated Accumulators
// COPYRIGHT    : Andy Thomas (C) 2019
// WEB URL      : https://kuiper.zone
// LICENSE      : GPLv3
//---------------------------------------------------------------------------

using System.Collections.Generic;

namespace CompensatedAccumulators
{
    /// <summary>
    /// A "pairwise" implementation of <see cref="ICompensatedSum"/>. In this, values
    /// are buffered. Computation of the result is performed only periodically or when
    /// the <see cref="Value"/> is requred.
    /// </summary>
    public class PairwiseSum : ICompensatedSum
    {
        private const int BufferSize =  8192;
        private const int BaseN = 2;
        private readonly List<double> _buffer = new List<double>(BufferSize);

        // Cached result
        private double _value;
        private bool _hasValue = true;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PairwiseSum()
        {
        }

        /// <summary>
        /// Constructor with initial value.
        /// </summary>
        public PairwiseSum(double x)
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
                return ActualizeCache(false);
            }

            set
            {
                _value = value;
                _hasValue = true;
                _buffer.Clear();
            }
        }

        /// <summary>
        /// Implements <see cref="ICompensatedSum.Add(double)"/>.
        /// </summary>
        public void Add(double x)
        {
            if (_buffer.Count == BufferSize)
            {
                // Process buff
                ActualizeCache(true);
            }

            _buffer.Add(x);
            _hasValue = false;
        }

        /// <summary>
        /// Implements <see cref="ICompensatedSum.Clear()"/>.
        /// </summary>
        public void Clear()
        {
            _value = 0;
            _hasValue = true;
            _buffer.Clear();
        }

        /// <summary>
        /// Implements <see cref="ICompensatedSum.Clone()"/>.
        /// </summary>
        public ICompensatedSum Clone()
        {
            var clone = new PairwiseSum();
            clone._value = _value;
            clone._hasValue = _hasValue;
            clone._buffer.AddRange(_buffer);

            return clone;
        }

        private static double Pairwise(IList<double> values, int start, int end)
        {
            // https://en.wikipedia.org/wiki/Pairwise_summation
            int sz = end - start;

            if (sz <= BaseN)
            {
                double sum = 0;

                while(start < end)
                {
                    sum += values[start++];
                }

                return sum;
            }

            sz = start + sz / 2 + 1;
            return Pairwise(values, start, sz) + Pairwise(values, sz, end);
        }

        private double ActualizeCache(bool clear)
        {
            if (!_hasValue)
            {
                if (_buffer.Count > 0)
                {
                    _value = Pairwise(_buffer, 0, _buffer.Count);
                }
                else
                {
                    _value = 0;
                }

                _hasValue = true;
            }

            if (clear || _buffer.Count >= BufferSize)
            {
                // Already cached here
                _buffer.Clear();
                _buffer.Add(_value);
            }

            return _value;
        }

    }
}
