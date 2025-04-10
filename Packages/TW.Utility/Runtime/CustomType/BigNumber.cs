//************************************************************************************
// BigNumber Class Version 1.03
//
// Copyright (c) 2002 Chew Keong TAN
// All rights reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, provided that the above
// copyright notice(s) and this permission notice appear in all copies of
// the Software and that both the above copyright notice(s) and this
// permission notice appear in supporting documentation.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT
// OF THIRD PARTY RIGHTS. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR
// HOLDERS INCLUDED IN THIS NOTICE BE LIABLE FOR ANY CLAIM, OR ANY SPECIAL
// INDIRECT OR CONSEQUENTIAL DAMAGES, OR ANY DAMAGES WHATSOEVER RESULTING
// FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN ACTION OF CONTRACT,
// NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF OR IN CONNECTION
// WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.
//
//
// Disclaimer
// ----------
// Although reasonable care has been taken to ensure the correctness of this
// implementation, this code should never be used in any application without
// proper verification and testing.  I disclaim all liability and responsibility
// to any person or entity with respect to any loss or damage caused, or alleged
// to be caused, directly or indirectly, by the use of this BigNumber class.
//
// Comments, bugs and suggestions to
// (http://www.codeproject.com/csharp/BigNumber.asp)
//
//
// Overloaded Operators +, -, *, /, %, >>, <<, ==, !=, >, <, >=, <=, &, |, ^, ++, --, ~
//
// Features
// --------
// 1) Arithmetic operations involving large signed integers (2's complement).
// 2) Primality test using Fermat little theorm, Rabin Miller's method,
//    Solovay Strassen's method and Lucas strong pseudoprime.
// 3) Modulo exponential with Barrett's reduction.
// 4) Inverse modulo.
// 5) Pseudo prime generation.
// 6) Co-prime generation.
//
//
// Known Problem
// -------------
// This pseudoprime passes my implementation of
// primality test but failed in JDK's isProbablePrime test.
//
//       byte[] pseudoPrime1 = { (byte)0x00,
//             (byte)0x85, (byte)0x84, (byte)0x64, (byte)0xFD, (byte)0x70, (byte)0x6A,
//             (byte)0x9F, (byte)0xF0, (byte)0x94, (byte)0x0C, (byte)0x3E, (byte)0x2C,
//             (byte)0x74, (byte)0x34, (byte)0x05, (byte)0xC9, (byte)0x55, (byte)0xB3,
//             (byte)0x85, (byte)0x32, (byte)0x98, (byte)0x71, (byte)0xF9, (byte)0x41,
//             (byte)0x21, (byte)0x5F, (byte)0x02, (byte)0x9E, (byte)0xEA, (byte)0x56,
//             (byte)0x8D, (byte)0x8C, (byte)0x44, (byte)0xCC, (byte)0xEE, (byte)0xEE,
//             (byte)0x3D, (byte)0x2C, (byte)0x9D, (byte)0x2C, (byte)0x12, (byte)0x41,
//             (byte)0x1E, (byte)0xF1, (byte)0xC5, (byte)0x32, (byte)0xC3, (byte)0xAA,
//             (byte)0x31, (byte)0x4A, (byte)0x52, (byte)0xD8, (byte)0xE8, (byte)0xAF,
//             (byte)0x42, (byte)0xF4, (byte)0x72, (byte)0xA1, (byte)0x2A, (byte)0x0D,
//             (byte)0x97, (byte)0xB1, (byte)0x31, (byte)0xB3,
//       };
//
//
// Change Log
// ----------
// 1) September 23, 2002 (Version 1.03)
//    - Fixed operator- to give correct data length.
//    - Added Lucas sequence generation.
//    - Added Strong Lucas Primality test.
//    - Added integer square root method.
//    - Added setBit/unsetBit methods.
//    - New isProbablePrime() method which do not require the
//      confident parameter.
//
// 2) August 29, 2002 (Version 1.02)
//    - Fixed bug in the exponentiation of negative numbers.
//    - Faster modular exponentiation using Barrett reduction.
//    - Added getBytes() method.
//    - Fixed bug in ToHexString method.
//    - Added overloading of ^ operator.
//    - Faster computation of Jacobi symbol.
//
// 3) August 19, 2002 (Version 1.01)
//    - Big integer is stored and manipulated as unsigned integers (4 bytes) instead of
//      individual bytes this gives significant performance improvement.
//    - Updated Fermat's Little Theorem test to use a^(p-1) mod p = 1
//    - Added isProbablePrime method.
//    - Updated documentation.
//
// 4) August 9, 2002 (Version 1.0)
//    - Initial Release.
//
//
// References
// [1] D. E. Knuth, "Seminumerical Algorithms", The Art of Computer Programming Vol. 2,
//     3rd Edition, Addison-Wesley, 1998.
//
// [2] K. H. Rosen, "Elementary Number Theory and Its Applications", 3rd Ed,
//     Addison-Wesley, 1993.
//
// [3] B. Schneier, "Applied Cryptography", 2nd Ed, John Wiley & Sons, 1996.
//
// [4] A. Menezes, P. van Oorschot, and S. Vanstone, "Handbook of Applied Cryptography",
//     CRC Press, 1996, www.cacr.math.uwaterloo.ca/hac
//
// [5] A. Bosselaers, R. Govaerts, and J. Vandewalle, "Comparison of Three Modular
//     Reduction Functions," Proc. CRYPTO'93, pp.175-186.
//
// [6] R. Baillie and S. S. Wagstaff Jr, "Lucas Pseudoprimes", Mathematics of Computation,
//     Vol. 35, No. 152, Oct 1980, pp. 1391-1417.
//
// [7] H. C. Williams, "�douard Lucas and Primality Testing", Canadian Mathematical
//     Society Series of Monographs and Advance Texts, vol. 22, John Wiley & Sons, New York,
//     NY, 1998.
//
// [8] P. Ribenboim, "The new book of prime number records", 3rd edition, Springer-Verlag,
//     New York, NY, 1995.
//
// [9] M. Joye and J.-J. Quisquater, "Efficient computation of full Lucas sequences",
//     Electronics Letters, 32(6), 1996, pp 537-538.
//
//************************************************************************************

using System;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace TW.Utility.CustomType
{
    [BigNumberEditor]
    [Serializable]
    public struct BigNumber : IFormattable
    {
        public static class Abbreviations
        {
            public static string[] AbbreviationArray { get; } =
            {
                "a",
                "b",
                "c",
                "d",
                "e",
                "f",
                "g",
                "h",
                "i",
                "j",
                "k",
                "l",
                "m",
                "n",
                "o",
                "p",
                "q",
                "r",
                "s",
                "t",
                "u",
                "v",
                "w",
                "x",
                "y",
                "z",
            };
            private static int AbbreviationLength { get; } = AbbreviationArray.Length;
            
            public static string FormExponent(int exponent)
            {
                return exponent switch
                {
                    <= 0 => "",
                    1 => "K",
                    2 => "M",
                    3 => "B",
                    4 => "T",
                    _ => GetAbbreviation(exponent - 5)
                };
                
                string GetAbbreviation(int index)
                {
                    string result = "";
                    while (index >= 0) {
                        int remainder = index % AbbreviationLength;
                        result = AbbreviationArray[remainder] + result;
                        index = index / AbbreviationLength - 1; 
                    }
                    return result;
                }
            }
            public static int ParseExponent(string exponent)
            {
                if (string.IsNullOrEmpty(exponent)) return 0;
                return exponent switch
                {
                    "K" => 1,
                    "M" => 2,
                    "B" => 3,
                    "T" => 4,
                    _ => ParseAbbreviation(exponent)
                };
                
                int ParseAbbreviation(string e)
                {
                    int result = 0;
                    for (int i = 0; i < e.Length; i++)
                    {
                        result = result * AbbreviationLength + (AbbreviationArray.ToList().IndexOf(e[i].ToString()) + 1);
                    }
                    return result + 4;
                }
            }

            public static bool IsExponent(string exponent)
            {
                if (string.IsNullOrEmpty(exponent)) return true;
                return exponent switch
                {
                    "K" => true,
                    "M" => true,
                    "B" => true,
                    "T" => true,
                    _ => IsAbbreviation(exponent)
                };
                bool IsAbbreviation(string e)
                {
                    foreach (char c in e)
                    {
                        if (!AbbreviationArray.Contains(c.ToString()))
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
        }

        public double coefficient;
        public int exponent;


        //***********************************************************************
        // Constructor (Default value for BigNumber is 0
        //***********************************************************************

        //public BigNumber() {
        //    v = 0;
        //    m = 0;
        //}


        //***********************************************************************
        // Constructor (Default value provided by long)
        //***********************************************************************

        public BigNumber(long value)
        {
            coefficient = value;
            exponent = 0;
        }

        //***********************************************************************
        // Constructor (Default value provided by ulong)
        //***********************************************************************

        public BigNumber(ulong value)
        {
            coefficient = value;
            exponent = 0;
        }

        //***********************************************************************
        // Constructor (Default value provided by float)
        //***********************************************************************
        public BigNumber(float value)
        {
            coefficient = value;
            exponent = 0;
        }

        //***********************************************************************
        // Constructor (Default value provided by double)
        //***********************************************************************
        public BigNumber(double value)
        {
            coefficient = value;
            exponent = 0;
        }

        public void Init(ulong value)
        {
            coefficient = value;
            exponent = 0;
        }


        //***********************************************************************
        // Constructor (Default value provided by BigNumber)
        //***********************************************************************

        public BigNumber(BigNumber bi)
        {
            coefficient = bi.coefficient;
            exponent = bi.exponent;
        }

        public BigNumber(double coefficient, int exponent)
        {
            this.coefficient = coefficient;
            this.exponent = exponent;
        }


        //***********************************************************************
        // Constructor (Default value provided by a string of digits of the
        //              specified base)
        //
        // Example (base 10)
        // -----------------
        // To initialize "a" with the default value of 1234 in base 10
        //      BigNumber a = new BigNumber("1234", 10)
        //
        // To initialize "a" with the default value of -1234
        //      BigNumber a = new BigNumber("-1234", 10)
        //
        // Example (base 16)
        // -----------------
        // To initialize "a" with the default value of 0x1D4F in base 16
        //      BigNumber a = new BigNumber("1D4F", 16)
        //
        // To initialize "a" with the default value of -0x1D4F
        //      BigNumber a = new BigNumber("-1D4F", 16)
        //
        // Note that string values are specified in the <sign><magnitude>
        // format.
        //
        //***********************************************************************

        public BigNumber(string value, int radix)
        {
            BigNumber bi = ParseFromCharacterFormat(value);
            coefficient = bi.coefficient;
            exponent = bi.exponent;
        }

        public BigNumber(string value)
        {
            BigNumber bi = ParseFromCharacterFormat(value);
            coefficient = bi.coefficient;
            exponent = bi.exponent;
        }

        public static implicit operator BigNumber(long value)
        {
            return new BigNumber(value);
        }

        public static implicit operator BigNumber(ulong value)
        {
            return new BigNumber(value);
        }

        public static implicit operator BigNumber(int value)
        {
            return new BigNumber((long)value);
        }

        public static implicit operator BigNumber(uint value)
        {
            return new BigNumber((ulong)value);
        }

        public static implicit operator BigNumber(float value)
        {
            return new BigNumber((float)value);
        }

        public static implicit operator BigNumber(double value)
        {
            return new BigNumber((double)value);
        }

        public static bool operator ==(BigNumber bi1, BigNumber bi2)
        {
            return bi1.exponent == bi2.exponent && Math.Abs(bi1.coefficient - bi2.coefficient) < 0.00000000001;
        }


        public static bool operator !=(BigNumber bi1, BigNumber bi2)
        {
            return !(bi1.Equals(bi2));
        }


        public override bool Equals(object o)
        {
            BigNumber bi = (BigNumber)o;
            return this == bi;
        }


        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }


        //***********************************************************************
        // Overloading of inequality operator
        //***********************************************************************

        public static bool operator >(BigNumber bi1, BigNumber bi2)
        {
            int max = Math.Max(bi1.exponent, bi2.exponent);

            double v1 = bi1.coefficient;
            double v2 = bi2.coefficient;
            for (int i = 0; i < max - bi1.exponent; i++)
            {
                v1 /= 1000;
            }

            for (int i = 0; i < max - bi2.exponent; i++)
            {
                v2 /= 1000;
            }

            return v1 > v2;
        }

        public static bool operator <(BigNumber bi1, BigNumber bi2)
        {
            int max = Math.Max(bi1.exponent, bi2.exponent);

            double v1 = bi1.coefficient;
            double v2 = bi2.coefficient;

            for (int i = 0; i < max - bi1.exponent; i++)
            {
                v1 /= 1000;
            }

            for (int i = 0; i < max - bi2.exponent; i++)
            {
                v2 /= 1000;
            }

            return v1 < v2;
        }


        public static bool operator >=(BigNumber bi1, BigNumber bi2)
        {
            return (bi1 == bi2 || bi1 > bi2);
        }


        public static bool operator <=(BigNumber bi1, BigNumber bi2)
        {
            return (bi1 == bi2 || bi1 < bi2);
        }


        //***********************************************************************
        // Private function that supports the division of two numbers with
        // a divisor that has more than 1 digit.
        //
        // Algorithm taken from [1]
        //***********************************************************************

        //private static void multiByteDivide(BigNumber bi1, BigNumber bi2,
        //                                    BigNumber outQuotient, BigNumber outRemainder) {
        //    uint[] result = new uint[maxLength]; UnityEngine.Debug.Log("Alloc");

        //    int remainderLen = bi1.dataLength + 1;
        //    uint[] remainder = new uint[remainderLen];

        //    uint mask = 0x80000000;
        //    uint val = bi2.data[bi2.dataLength - 1];
        //    int shift = 0, resultPos = 0;

        //    while (mask != 0 && (val & mask) == 0) {
        //        shift++; mask >>= 1;
        //    }

        //    //Console.WriteLine("shift = {0}", shift);
        //    //Console.WriteLine("Before bi1 Len = {0}, bi2 Len = {1}", bi1.dataLength, bi2.dataLength);

        //    for (int i = 0; i < bi1.dataLength; i++)
        //        remainder[i] = bi1.data[i];
        //    shiftLeft(remainder, shift);
        //    bi2 = bi2 << shift;

        //    /*
        //    Console.WriteLine("bi1 Len = {0}, bi2 Len = {1}", bi1.dataLength, bi2.dataLength);
        //    Console.WriteLine("dividend = " + bi1 + "\ndivisor = " + bi2);
        //    for(int q = remainderLen - 1; q >= 0; q--)
        //            Console.Write("{0:x2}", remainder[q]);
        //    Console.WriteLine();
        //    */

        //    int j = remainderLen - bi2.dataLength;
        //    int pos = remainderLen - 1;

        //    ulong firstDivisorByte = bi2.data[bi2.dataLength - 1];
        //    ulong secondDivisorByte = bi2.data[bi2.dataLength - 2];

        //    int divisorLen = bi2.dataLength + 1;
        //    uint[] dividendPart = new uint[divisorLen];

        //    while (j > 0) {
        //        ulong dividend = ((ulong)remainder[pos] << 32) + (ulong)remainder[pos - 1];
        //        //Console.WriteLine("dividend = {0}", dividend);

        //        ulong q_hat = dividend / firstDivisorByte;
        //        ulong r_hat = dividend % firstDivisorByte;

        //        //Console.WriteLine("q_hat = {0:X}, r_hat = {1:X}", q_hat, r_hat);

        //        bool done = false;
        //        while (!done) {
        //            done = true;

        //            if (q_hat == 0x100000000 ||
        //               (q_hat * secondDivisorByte) > ((r_hat << 32) + remainder[pos - 2])) {
        //                q_hat--;
        //                r_hat += firstDivisorByte;

        //                if (r_hat < 0x100000000)
        //                    done = false;
        //            }
        //        }

        //        for (int h = 0; h < divisorLen; h++)
        //            dividendPart[h] = remainder[pos - h];

        //        BigNumber kk = BigNumber.GetBigNumber(dividendPart);
        //        BigNumber ss = bi2.multiply((long)q_hat);

        //        //Console.WriteLine("ss before = " + ss);
        //        while (ss > kk) {
        //            q_hat--;
        //            ss.subtractPermanent(bi2);
        //            //Console.WriteLine(ss);
        //        }
        //        BigNumber yy = kk.subtract(ss);

        //        //Console.WriteLine("ss = " + ss);
        //        //Console.WriteLine("kk = " + kk);
        //        //Console.WriteLine("yy = " + yy);

        //        for (int h = 0; h < divisorLen; h++)
        //            remainder[pos - h] = yy.data[bi2.dataLength - h];

        //        /*
        //        Console.WriteLine("dividend = ");
        //        for(int q = remainderLen - 1; q >= 0; q--)
        //                Console.Write("{0:x2}", remainder[q]);
        //        Console.WriteLine("\n************ q_hat = {0:X}\n", q_hat);
        //        */

        //        result[resultPos++] = (uint)q_hat;

        //        pos--;
        //        j--;
        //    }

        //    outQuotient.dataLength = resultPos;
        //    int y = 0;
        //    for (int x = outQuotient.dataLength - 1; x >= 0; x--, y++)
        //        outQuotient.data[y] = result[x];
        //    for (; y < maxLength; y++)
        //        outQuotient.data[y] = 0;

        //    while (outQuotient.dataLength > 1 && outQuotient.data[outQuotient.dataLength - 1] == 0)
        //        outQuotient.dataLength--;

        //    if (outQuotient.dataLength == 0)
        //        outQuotient.dataLength = 1;

        //    outRemainder.dataLength = shiftRight(remainder, shift);

        //    for (y = 0; y < outRemainder.dataLength; y++)
        //        outRemainder.data[y] = remainder[y];
        //    for (; y < maxLength; y++)
        //        outRemainder.data[y] = 0;
        //}


        //***********************************************************************
        // Private function that supports the division of two numbers with
        // a divisor that has only 1 digit.
        //***********************************************************************

        //private static void singleByteDivide(BigNumber bi1, BigNumber bi2,
        //                                     BigNumber outQuotient, BigNumber outRemainder) {
        //    uint[] result = new uint[maxLength]; UnityEngine.Debug.Log("Alloc");
        //    int resultPos = 0;

        //    // copy dividend to reminder
        //    for (int i = 0; i < maxLength; i++)
        //        outRemainder.data[i] = bi1.data[i];
        //    outRemainder.dataLength = bi1.dataLength;

        //    while (outRemainder.dataLength > 1 && outRemainder.data[outRemainder.dataLength - 1] == 0)
        //        outRemainder.dataLength--;

        //    ulong divisor = (ulong)bi2.data[0];
        //    int pos = outRemainder.dataLength - 1;
        //    ulong dividend = (ulong)outRemainder.data[pos];

        //    //Console.WriteLine("divisor = " + divisor + " dividend = " + dividend);
        //    //Console.WriteLine("divisor = " + bi2 + "\ndividend = " + bi1);

        //    if (dividend >= divisor) {
        //        ulong quotient = dividend / divisor;
        //        result[resultPos++] = (uint)quotient;

        //        outRemainder.data[pos] = (uint)(dividend % divisor);
        //    }
        //    pos--;

        //    while (pos >= 0) {
        //        //Console.WriteLine(pos);

        //        dividend = ((ulong)outRemainder.data[pos + 1] << 32) + (ulong)outRemainder.data[pos];
        //        ulong quotient = dividend / divisor;
        //        result[resultPos++] = (uint)quotient;

        //        outRemainder.data[pos + 1] = 0;
        //        outRemainder.data[pos--] = (uint)(dividend % divisor);
        //        //Console.WriteLine(">>>> " + bi1);
        //    }

        //    outQuotient.dataLength = resultPos;
        //    int j = 0;
        //    for (int i = outQuotient.dataLength - 1; i >= 0; i--, j++)
        //        outQuotient.data[j] = result[i];
        //    for (; j < maxLength; j++)
        //        outQuotient.data[j] = 0;

        //    while (outQuotient.dataLength > 1 && outQuotient.data[outQuotient.dataLength - 1] == 0)
        //        outQuotient.dataLength--;

        //    if (outQuotient.dataLength == 0)
        //        outQuotient.dataLength = 1;

        //    while (outRemainder.dataLength > 1 && outRemainder.data[outRemainder.dataLength - 1] == 0)
        //        outRemainder.dataLength--;
        //}


        //***********************************************************************
        // Overloading of division operator
        //***********************************************************************

        public static BigNumber operator /(BigNumber bi1, BigNumber bi2)
        {
            BigNumber ret = new BigNumber(bi1);

            return ret.Divide(bi2);
        }

        public static BigNumber operator +(BigNumber bi1, BigNumber bi2)
        {
            BigNumber ret = new BigNumber(bi1);
            return ret.Add(bi2);
        }

        public static BigNumber operator -(BigNumber bi1, BigNumber bi2)
        {
            BigNumber ret = new BigNumber(bi1);

            return ret.Subtract(bi2);
        }

        public static BigNumber operator *(BigNumber bi1, BigNumber bi2)
        {
            BigNumber ret = new BigNumber(bi1);

            return ret.Multiply(bi2);
        }
        
        public static BigNumber operator -(BigNumber bi1)
        {
            return new BigNumber(-bi1.coefficient, bi1.exponent);
        }

        //***********************************************************************
        // Overloading of modulus operator
        //***********************************************************************

        //public static BigNumber operator %(BigNumber bi1, BigNumber bi2) {
        //    BigNumber quotient = BigNumber.GetBigNumber();
        //    BigNumber remainder = BigNumber.GetBigNumber(bi1);

        //    int lastPos = maxLength - 1;
        //    bool dividendNeg = false;

        //    if ((bi1.data[lastPos] & 0x80000000) != 0)     // bi1 negative
        //            {
        //        bi1 = bi1.negative();
        //        dividendNeg = true;
        //    }
        //    if ((bi2.data[lastPos] & 0x80000000) != 0)     // bi2 negative
        //        bi2 = bi2.negative();

        //    if (bi1 < bi2) {
        //        return remainder;
        //    } else {
        //        if (bi2.dataLength == 1)
        //            singleByteDivide(bi1, bi2, quotient, remainder);
        //        else
        //            multiByteDivide(bi1, bi2, quotient, remainder);

        //        if (dividendNeg)
        //            return remainder.negative();

        //        return remainder;
        //    }
        //}


        //***********************************************************************
        // Overloading of bitwise AND operator
        //***********************************************************************

        //public static BigNumber operator &(BigNumber bi1, BigNumber bi2) {
        //    BigNumber result = BigNumber.GetBigNumber();

        //    int len = (bi1.dataLength > bi2.dataLength) ? bi1.dataLength : bi2.dataLength;

        //    for (int i = 0; i < len; i++) {
        //        uint sum = (uint)(bi1.data[i] & bi2.data[i]);
        //        result.data[i] = sum;
        //    }

        //    result.dataLength = maxLength;

        //    while (result.dataLength > 1 && result.data[result.dataLength - 1] == 0)
        //        result.dataLength--;

        //    return result;
        //}


        //***********************************************************************
        // Overloading of bitwise OR operator
        //***********************************************************************

        //public static BigNumber operator |(BigNumber bi1, BigNumber bi2) {
        //    BigNumber result = BigNumber.GetBigNumber();

        //    int len = (bi1.dataLength > bi2.dataLength) ? bi1.dataLength : bi2.dataLength;

        //    for (int i = 0; i < len; i++) {
        //        uint sum = (uint)(bi1.data[i] | bi2.data[i]);
        //        result.data[i] = sum;
        //    }

        //    result.dataLength = maxLength;

        //    while (result.dataLength > 1 && result.data[result.dataLength - 1] == 0)
        //        result.dataLength--;

        //    return result;
        //}


        //***********************************************************************
        // Overloading of bitwise XOR operator
        //***********************************************************************

        //public static BigNumber operator ^(BigNumber bi1, BigNumber bi2) {
        //    BigNumber result = BigNumber.GetBigNumber();

        //    int len = (bi1.dataLength > bi2.dataLength) ? bi1.dataLength : bi2.dataLength;

        //    for (int i = 0; i < len; i++) {
        //        uint sum = (uint)(bi1.data[i] ^ bi2.data[i]);
        //        result.data[i] = sum;
        //    }

        //    result.dataLength = maxLength;

        //    while (result.dataLength > 1 && result.data[result.dataLength - 1] == 0)
        //        result.dataLength--;

        //    return result;
        //}


        //***********************************************************************
        // Returns max(this, bi)
        //***********************************************************************

        public BigNumber Max(BigNumber bi)
        {
            return this > bi ? this : bi;
        }


        //***********************************************************************
        // Returns min(this, bi)
        //***********************************************************************

        public BigNumber Min(BigNumber bi)
        {
            return this < bi ? this : bi;
        }


        //***********************************************************************
        // Returns the absolute value
        //***********************************************************************

        //public BigNumber abs() {
        //    if ((this.data[maxLength - 1] & 0x80000000) != 0)
        //        return (this.negative());
        //    else
        //        return (BigNumber.GetBigNumber(this));
        //}


        //***********************************************************************
        // Returns a string representing the BigNumber in base 10.
        //***********************************************************************

        public override string ToString()
        {
            double vv = coefficient;
            int mm = exponent;
            while (vv < 1 && exponent > 0)
            {
                vv *= 1000;
                mm--;
            }

            while (vv >= 1000)
            {
                vv /= 1000;
                mm++;
            }

            return vv.ToString(CultureInfo.InvariantCulture) + Abbreviations.FormExponent(mm);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            format ??= ToString();

            return format switch
            {
                "+#;-#;+0" when this > 0 => $"+{ToString()}",
                "+#;-#;+0" when this < 0 => $"{ToString()}",
                "+#;-#;+0" => $"+0",
                _ => ToString()
            };
        }

        public string ToStringUI()
        {
            double vv = coefficient;
            int mm = exponent;

            while (vv < 1 && mm > 0)
            {
                vv *= 1000;
                mm--;
            }

            while (vv >= 1000)
            {
                vv /= 1000;
                mm++;
            }

            int digit = Math.Clamp(3 - Math.Round(vv, 0).ToString(CultureInfo.InvariantCulture).Length, 0, 3);
            string s = Math.Round(vv, digit).ToString(CultureInfo.InvariantCulture);

            return s + Abbreviations.FormExponent(mm);
        }

        public string ToStringUIFloor()
        {
            int mm = exponent;
            double vv = Math.Round(coefficient, 3 * (exponent < 5 ? exponent : 5));

            while (vv < 1 && mm > 0)
            {
                vv *= 1000;
                mm--;
            }

            while (vv >= 1000)
            {
                vv /= 1000;
                mm++;
            }

            int digit = Math.Clamp(4 - Math.Round(vv, 0).ToString(CultureInfo.InvariantCulture).Length, 0, 4);
            string s = Math.Round(vv, digit).ToString(CultureInfo.InvariantCulture);

            return s + Abbreviations.FormExponent(mm);
        }

        //***********************************************************************
        // Returns a string representing the BigNumber in sign-and-magnitude
        // format in the specified radix.
        //
        // Example
        // -------
        // If the value of BigNumber is -255 in base 10, then
        // ToString(16) returns "-FF"
        //
        //***********************************************************************

        //public string ToString(int radix) {
        //    if (radix < 2 || radix > 36)
        //        throw (new ArgumentException("Radix must be >= 2 and <= 36"));

        //    string charSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        //    string result = "";

        //    BigNumber a = this;

        //    bool negative = false;
        //    if ((a.data[maxLength - 1] & 0x80000000) != 0) {
        //        negative = true;
        //        try {
        //            a.selfNegative();
        //        } catch (Exception) { }
        //    }

        //    BigNumber quotient = BigNumber.GetBigNumber(0);
        //    BigNumber remainder = BigNumber.GetBigNumber(0);
        //    BigNumber biRadix = BigNumber.GetBigNumber(radix);

        //    if (a.dataLength == 1 && a.data[0] == 0)
        //        result = "0";
        //    else {
        //        while (a.dataLength > 1 || (a.dataLength == 1 && a.data[0] != 0)) {
        //            singleByteDivide(a, biRadix, quotient, remainder);

        //            if (remainder.data[0] < 10)
        //                result = remainder.data[0] + result;
        //            else
        //                result = charSet[(int)remainder.data[0] - 10] + result;

        //            a.Release();
        //            a = GetBigNumber(quotient);
        //        }
        //        if (negative)
        //            result = "-" + result;
        //    }

        //    Release(remainder);
        //    Release(biRadix);
        //    return result;
        //}


        //***********************************************************************
        // Returns a hex string showing the contains of the BigNumber
        //
        // Examples
        // -------
        // 1) If the value of BigNumber is 255 in base 10, then
        //    ToHexString() returns "FF"
        //
        // 2) If the value of BigNumber is -255 in base 10, then
        //    ToHexString() returns ".....FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF01",
        //    which is the 2's complement representation of -255.
        //
        //***********************************************************************

        //public string ToHexString() {
        //    string result = data[dataLength - 1].ToString("X");

        //    for (int i = dataLength - 2; i >= 0; i--) {
        //        result += data[i].ToString("X8");
        //    }

        //    return result;
        //}


        //***********************************************************************
        // Modulo Exponentiation
        //***********************************************************************

        //public BigNumber modPow(BigNumber exp, BigNumber n) {
        //    if ((exp.data[maxLength - 1] & 0x80000000) != 0)
        //        throw (new ArithmeticException("Positive exponents only."));

        //    BigNumber resultNum = 1;
        //    BigNumber tempNum;
        //    bool thisNegative = false;

        //    if ((this.data[maxLength - 1] & 0x80000000) != 0)   // negative this
        //        {
        //        tempNum = this.negative() % n;
        //        thisNegative = true;
        //    } else
        //        tempNum = this % n;  // ensures (tempNum * tempNum) < b^(2k)

        //    if ((n.data[maxLength - 1] & 0x80000000) != 0)   // negative n
        //        n = n.negative();

        //    // calculate constant = b^(2k) / m
        //    BigNumber constant = BigNumber.GetBigNumber();

        //    int i = n.dataLength << 1;
        //    constant.data[i] = 0x00000001;
        //    constant.dataLength = i + 1;

        //    constant = constant.divide(n);
        //    int totalBits = exp.bitCount();
        //    int count = 0;

        //    // perform squaring and multiply exponentiation
        //    for (int pos = 0; pos < exp.dataLength; pos++) {
        //        uint mask = 0x01;
        //        //Console.WriteLine("pos = " + pos);

        //        for (int index = 0; index < 32; index++) {
        //            if ((exp.data[pos] & mask) != 0)
        //                resultNum = BarrettReduction(resultNum.multiply(tempNum), n, constant);

        //            mask <<= 1;

        //            tempNum = BarrettReduction(tempNum.multiply(tempNum), n, constant);


        //            if (tempNum.dataLength == 1 && tempNum.data[0] == 1) {
        //                if (thisNegative && (exp.data[0] & 0x1) != 0)    //odd exp
        //                    return resultNum.negative();
        //                return resultNum;
        //            }
        //            count++;
        //            if (count == totalBits)
        //                break;
        //        }
        //    }

        //    if (thisNegative && (exp.data[0] & 0x1) != 0)    //odd exp
        //        return resultNum.negative();

        //    return resultNum;
        //}


        //***********************************************************************
        // Fast calculation of modular reduction using Barrett's reduction.
        // Requires x < b^(2k), where b is the base.  In this case, base is
        // 2^32 (uint).
        //
        // Reference [4]
        //***********************************************************************

        public BigNumber Sqrt()
        {
            BigNumber result = new BigNumber(this);
            if (result.exponent % 2 == 0)
            {
                result.coefficient = Math.Sqrt((double)result.coefficient);
                result.exponent /= 2;
            }
            else
            {
                result.coefficient = Math.Sqrt((double)result.coefficient * 1000);
                result.exponent = (result.exponent - 1) / 2;
            }

            return result;
        }

        public BigNumber SqrtPermanent()
        {
            coefficient = Math.Sqrt(coefficient);
            return this;
        }

        public BigNumber Normalize()
        {
            BigNumber ret = new BigNumber(this);
            int safetyCounter = 1000; // Safety counter to prevent infinite loops

            while (ret is { coefficient: < 1, exponent: > 0 } && safetyCounter > 0)
            {
                ret.coefficient *= 1000;
                ret.exponent--;
                safetyCounter--;
            }

            safetyCounter = 1000; // Reset safety counter

            while (ret.coefficient >= 1000 && safetyCounter > 0)
            {
                ret.coefficient /= 1000;
                ret.exponent++;
                safetyCounter--;
            }

            return ret;
        }

        public string ToString2()
        {
            double vv = coefficient;
            int mm = exponent;
            double num = coefficient;
            double maxConvert = 10000000;
            bool isOver10Million = false;
            for (int i = 0; i < mm; i++)
            {
                num *= 1000;
                if (num >= maxConvert)
                {
                    isOver10Million = true;
                    break;
                }
            }

            if (mm == 0)
            {
                if (num >= maxConvert)
                {
                    isOver10Million = true;
                }
            }

            if (!isOver10Million)
            {
                return ((int)num).ToString();
            }

            while (vv < 1 && mm > 0)
            {
                vv *= 1000;
                mm--;
            }

            while (vv >= 1000)
            {
                vv /= 1000;
                mm++;
            }

            string s = Math.Round(vv, 3).ToString();
            if (s.IndexOf('.') != -1)
            {
                while (s.Length > 1 && s[^1] == '0')
                {
                    s = s.Remove(s.Length - 1);
                }

                if (s.Length > 1 && s[^1] == '.')
                {
                    s = s.Remove(s.Length - 1);
                }
            }

            int index = s.IndexOf('.');
            if (index != -1 && index < 3)
            {
                return s[..(s.Length > 5 ? 5 : s.Length)] + Abbreviations.FormExponent(mm);
            }
            else
            {
                return s[..(s.Length > 3 ? 3 : s.Length)] + Abbreviations.FormExponent(mm);
            }
        }

        public string ToString3()
        {
            double vv = coefficient;
            int mm = exponent;
            double num = coefficient;
            double maxConvert = 1000;
            bool isOver10Million = false;
            for (int i = 0; i < mm; i++)
            {
                num *= 1000;
                if (num >= maxConvert)
                {
                    isOver10Million = true;
                    break;
                }
            }

            if (mm == 0)
            {
                if (num >= maxConvert)
                {
                    isOver10Million = true;
                }
            }

            if (!isOver10Million)
            {
                return ((int)num).ToString();
            }

            while (vv < 1 && mm > 0)
            {
                vv *= 1000;
                mm--;
            }

            while (vv >= 1000)
            {
                vv /= 1000;
                mm++;
            }

            string s = Math.Round(vv, 3).ToString();
            if (s.IndexOf('.') != -1)
            {
                while (s.Length > 1 && s[^1] == '0')
                {
                    s = s.Remove(s.Length - 1);
                }

                if (s.Length > 1 && s[^1] == '.')
                {
                    s = s.Remove(s.Length - 1);
                }
            }

            int index = s.IndexOf('.');
            if (index != -1 && index < 3)
            {
                return s.Substring(0, s.Length > 5 ? 5 : s.Length) + Abbreviations.FormExponent(mm);
            }
            else
            {
                return s.Substring(0, s.Length > 3 ? 3 : s.Length) + Abbreviations.FormExponent(mm);
            }
        }

        public string ToCharacterFormat()
        {
            double vv = coefficient;
            int mm = exponent;
            while (vv < 1 && mm > 0)
            {
                vv *= 1000;
                mm--;
            }

            while (vv >= 1000)
            {
                vv /= 1000;
                mm++;
            }

            string s = Math.Round(vv, 2).ToString();
            if (s.IndexOf('.') != -1)
            {
                while (s.Length > 1 && s[^1] == '0')
                {
                    s = s.Remove(s.Length - 1);
                }

                if (s.Length > 1 && s[^1] == '.')
                {
                    s = s.Remove(s.Length - 1);
                }
            }

            int index = s.IndexOf('.');
            if (index != -1 && index < 3)
            {
                return s.Substring(0, s.Length > 4 ? 4 : s.Length) + " " + Abbreviations.FormExponent(mm);
            }
            else
            {
                return s.Substring(0, s.Length > 3 ? 3 : s.Length) + " " + Abbreviations.FormExponent(mm);
            }
        }

        public int ToInt()
        {
            //UnityEngine.Debug.Log("ToInt " + this.ToString());
            double vv = coefficient;
            for (int i = 0; i < exponent; i++)
            {
                vv *= 1000;
            }

            return (int)vv;
        }

        public long ToLong()
        {
            //UnityEngine.Debug.Log("ToInt " + this.ToString());
            double vv = coefficient;
            for (int i = 0; i < exponent; i++)
            {
                vv *= 1000;
            }

            return (long)vv;
        }

        public BigNumber ToIntBigNumber()
        {
            int length = Mathf.Clamp(exponent * 3, 0, 15);
            double vv = Math.Round(coefficient, length);
            return new BigNumber(vv, exponent);
        }

        public BigNumber MultiplyWithFloat(float f)
        {
            BigNumber bi = new BigNumber(this);
            bi.coefficient *= f;
            return bi;
        }

        public BigNumber MultiplyWithFloatPermanent(float f)
        {
            coefficient *= f;
            return this;
        }

        public BigNumber MultiplyInt(int i)
        {
            BigNumber bi = new BigNumber(this);
            bi.coefficient *= i;
            return bi;
        }

        public BigNumber MultiplyIntPermanent(int i, bool isReleaseVal = true)
        {
            coefficient *= i;
            return this;
        }

        public static BigNumber ParseFromDouble(double d)
        {
            return new BigNumber(d);
        }

        public static BigNumber ParseFromCharacterFormat(string s)
        {
            int abbreviationsLength = 0;
            for (int i = s.Length - 1; i >= 0; i--)
            {
                if (char.IsLetter(s[i]))
                {
                    abbreviationsLength++;
                }
                else
                {
                    break;
                }
            }
            string s1 = s[^abbreviationsLength..];
            int num = Abbreviations.ParseExponent(s1);

            if (s1 == "")
            {
                //UnityEngine.Debug.Log(s);
                return new BigNumber(double.Parse(s, CultureInfo.InvariantCulture), 0);
            }

            int m = num;
            string s3 = s[..^s1.Length];
            double v = double.Parse(s3, CultureInfo.InvariantCulture);
            return new BigNumber(v, m);
        }

        public static BigNumber ZERO => new BigNumber(0);

        public static BigNumber Clamp(BigNumber value, BigNumber min, BigNumber max)
        {
            if (value > max)
            {
                return max;
            }

            if (value < min)
            {
                return min;
            }

            return value;
        }
        
        public BigNumber PowPermanent(int exp)
        {
            BigNumber result = this.Pow(exp);
            CopyData(result);
            return this;
        }

        public BigNumber Pow(int exp)
        {
            switch (exp)
            {
                case < 0:
                    throw new ArithmeticException("Negative exponent");
                case 0:
                    return new BigNumber(1);
            }

            if (this == 0) return this;

            BigNumber ret = new BigNumber(this);

            double rv = ret.coefficient;
            int rm = ret.exponent;
            while (exp != 1)
            {
                ret.coefficient *= rv;
                ret.exponent += rm;
                while (ret.coefficient >= 1000)
                {
                    ret.coefficient /= 1000;
                    ret.exponent++;
                }

                exp--;
            }

            return ret.Normalize();
        }
        
        public BigNumber Pow(float exp)
        {
            switch (exp)
            {
                case < 0:
                    throw new ArithmeticException("Negative exponent");
                case 0:
                    return new BigNumber(1);
            }
            if (this == 0) return this;
            BigNumber ret = new BigNumber(this);
            int tempE = Mathf.FloorToInt(exp);
            float tempF = exp - tempE;
            
            double rv = ret.coefficient;
            int rm = ret.exponent;
            while (tempE != 1)
            {
                ret.coefficient *= rv;
                ret.exponent += rm;
                while (ret.coefficient >= 1000)
                {
                    ret.coefficient /= 1000;
                    ret.exponent++;
                }
                tempE--;
            }
            ret.coefficient = ret.coefficient * Math.Pow(rv, tempF) * Math.Pow(1000, rm * tempF);
            return ret.Normalize();
        
        }   

        internal BigNumber DividePermanent(BigNumber amount, bool isRelease = false)
        {
            BigNumber result = this.Divide(amount, isRelease);
            CopyData(result);
            return this;
        }

        internal BigNumber Divide(BigNumber amount, bool isReleaseVal = false)
        {
            int max = Math.Max(exponent, amount.exponent);
            double v1 = coefficient;
            double v2 = amount.coefficient;
            int m1 = exponent;
            int m2 = amount.exponent;
            for (int i = 0; i < max - m1; i++)
            {
                v1 /= 1000;
            }

            for (int i = 0; i < max - m2; i++)
            {
                v2 /= 1000;
            }

            BigNumber ret = (new BigNumber(v1 / v2, 0)).Normalize();
            return ret;
        }

        internal BigNumber MultiplyPermanent(BigNumber amount, bool isReleaseVal = false)
        {
            BigNumber result = this.Multiply(amount, isReleaseVal);
            CopyData(result);
            return this;
        }

        internal BigNumber Multiply(BigNumber amount, bool isReleaseVal = false)
        {
            return (new BigNumber(coefficient * amount.coefficient, amount.exponent + exponent)).Normalize();
        }

        internal BigNumber SubtractPermanent(BigNumber amount, bool isRelease = false)
        {
            BigNumber result = this.Subtract(amount, isRelease);
            CopyData(result);
            return this;
        }

        internal BigNumber Subtract(BigNumber amount, bool isRelease = false)
        {
            int max = Math.Max(exponent, amount.exponent);
            double v1 = coefficient;
            double v2 = amount.coefficient;
            int m1 = exponent;
            int m2 = amount.exponent;
            for (int i = 0; i < max - m1; i++)
            {
                v1 /= 1000;
            }

            for (int i = 0; i < max - m2; i++)
            {
                v2 /= 1000;
            }

            BigNumber ret = (new BigNumber(v1 - v2, max)).Normalize();
            return ret;
        }

        internal BigNumber AddPermanent(BigNumber amount, bool isReleaseVal = false)
        {
            BigNumber result = this.Add(amount, isReleaseVal);
            CopyData(result);
            return this;
        }

        public BigNumber Add(BigNumber amount, bool isReleaseVal = false) //throws ArithmeticException
        {
            int max = Math.Max(exponent, amount.exponent);
            double v1 = coefficient;
            double v2 = amount.coefficient;
            int m1 = exponent;
            int m2 = amount.exponent;
            for (int i = 0; i < max - m1; i++)
            {
                v1 /= 1000f;
            }

            for (int i = 0; i < max - m2; i++)
            {
                v2 /= 1000f;
            }

            BigNumber ret = (new BigNumber(v1 + v2, max)).Normalize();
            return ret;
        }

        private BigNumber CopyData(BigNumber val)
        {
            coefficient = val.coefficient;
            exponent = val.exponent;
            return this;
        }

        public float ToFloat()
        {
            double d = coefficient;
            for (int i = 0; i < exponent; i++)
            {
                d *= 1000;
            }

            return (float)d;
        }

        public double ToDouble()
        {
            double d = coefficient;
            for (int i = 0; i < exponent; i++)
            {
                d *= 1000;
            }

            return d;
        }

        public BigNumber RoundToInt()
        {
            int mm = exponent;
            double vv = Math.Round(coefficient, 3 * (exponent < 5 ? exponent : 5));
            return new BigNumber(vv, mm);
        }
        
        public static BigNumber Lerp(BigNumber from, BigNumber to, float t)
        {
            return from + (to - from) * t;
        }
        public static BigNumber Min(BigNumber a, BigNumber b)
        {
            return a < b ? a : b;
        }
        public static BigNumber Max(BigNumber a, BigNumber b)
        {
            return a > b ? a : b;
        }
        public static BigNumber Abs(BigNumber a)
        {
            return a < 0 ? -a : a;
        }
    }
}