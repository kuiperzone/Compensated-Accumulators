# C# COMPENSATED ACCUMULATORS #

## Abstract ##
Numerical errors are incurred when summing sequences of finite precision floating
point numbers. However, there are a number of techniques which significantly
reduce, or compensate for, the accumulation of errors.

This is a lightweight C# library comprising implementations of common "compensated sum"
accumulators. Additionally, an Xunit test project not only provides unit testing,
but allows for performance and accuracy comparisions between the generators.

## Compensated Accumulators ##
Implmentations inherits a common interface *ICompensatedSum*, as follows:

* KahanSum - Classic "Kahan" implementation.
* NeumaierSum - Variation of the Kahan algorithm, also referred to as "Kahan–Babuška".
* KleinSum - Klein variation of the Kahan algorithm, also referred to as "Kahan-Babuška-Neumaier" compensated sum. It offers improved accuracy over both KahanSum and NeumaierSum.
* PairwiseSum - A "pairwise" implementation of ICompensatedSum. In this, values are buffered. Computation of the result is performed only periodically or when the Value is requred.

The KahanSum, NeumaierSum and KleinSum algorithms are variations of the same Kahan algorithm. The
PairwiseSum uses a recursive approach, however, with input values buffered and computed periodically.

## Accumulator Performance ##
Performance and error results are presented below. An implementation of a "naive summation"
is also provided for comparison.

`
ALGORITHM         Add()         Error
KahanSum          6.24 ns       1.86E-09
NeumaierSum       4.22 ns       0
KleinSum          5.59 ns       0
PairwiseSum       9.71 ns       0.015
NaiveSum          3.67 ns       0.111
`
Smaller values are better. The test hardware was as follows:

Windows 10
.NET Core 3.0
i7-6700 3.40GHz

In generating the error result, 1000,000 pseudo random values were generated in the range 0
to 1E7 and added to the accumulator. The same psuedo random sequence was then re-generated, but
this time values were subracted. The final summation, without error, must therefore equal 0.
The errors shown are the absolute deviation from zero.

Performance is presented as the average time taken to call the accumulator's ICompensatedSum.Add()
method over many iterations.

## License ##
This is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by the
Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

It is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this software. If not, see <http://www.gnu.org/licenses/>.

