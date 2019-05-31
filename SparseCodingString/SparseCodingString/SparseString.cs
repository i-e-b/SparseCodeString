using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace SparseCodingString
{
    /// <summary>
    /// A tree-based string representation (read-only)
    /// </summary>
    /// <remarks>
    /// The purpose of this is to allow the use of very-wide character sets (like 32 bit UCS encoding)
    /// and maintain the ability to index directly into characters by location, while keeping
    /// memory use as low as possible.
    /// This is done with a compact "wavelet tree" representation
    /// ( https://en.wikipedia.org/wiki/Wavelet_Tree )
    /// <para></para>
    /// Future improvements:
    /// - append
    /// - substring
    /// - change-in-place
    /// - efficient search
    /// </remarks>
    public class SparseString
    {
        public int StringLength;

        [NotNull]public uint[] CharDictionary;
        [NotNull]public byte[] StringCoeffs;

        public SparseString(string str)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            StringLength = str.Length;

            // get distinct characters
            var uset = new HashSet<uint>();
            foreach (char c in str) { uset.Add(c); }
            // TODO: Sort the string so most common chars come first.
            CharDictionary = uset.ToArray();

            // Note: this isn't as compact as it could be, to make indexing simpler
            // The coefficients start 1-bit per char, and half in size per round. Number of rounds is ceil(log2(dictSize)).
            var rounds = (int)Math.Ceiling(Math.Log(CharDictionary.Length, 2));
            var bits = rounds * StringLength;

            var bytes = (int)Math.Ceiling(bits / 8.0);
            StringCoeffs = new byte[bytes];

            // rough building of coeffs
            // we repeatedly split the dictionary, and write which 'side' the target is on:
            //     Example: input string = "ADACBBAD"
            //     D=[A=0,B=0,   C=1,D=1]; Coef=[01010001]
            //     D=[A=0, B=1, C=0, D=1]; Coef=[01001101]
            // We can then read in spans to get the index into the dictionary.

            int index = 0;
            int initSpan = NextPow2(CharDictionary.Length);
            for (int span = initSpan; span > 1; span /= 2)
            {
                int halfspan = (int)Math.Ceiling(span / 2.0);
                for (int i = 0; i < StringLength; i++)
                {
                    var dictIdx = Find(CharDictionary, str[i]);
                    var value = (dictIdx % span) >= halfspan;
                    SetBit(StringCoeffs, index, value);
                    index++;
                }
            }
        }

        public static SparseString FromString(string str)
        {
            return new SparseString(str);
        }
        
        public char CharAt(int i)
        {
            if (i < 0 || i >= StringLength) return (char)0;

            var rounds = (int)Math.Ceiling(Math.Log(CharDictionary.Length, 2));
            var dictIdx = 0;

            for (int b = 0; b < rounds; b++)
            {
                dictIdx <<= 1;
                if (GetBit(StringCoeffs, b* StringLength + i)) dictIdx |= 1;
            }

            return (char)CharDictionary[dictIdx];
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            var rounds = (int)Math.Ceiling(Math.Log(CharDictionary.Length, 2));

            // for each position in the output
            for (int i = 0; i < StringLength; i++)
            {
                var dictIdx = 0;
                // for each bit
                for (int b = 0; b < rounds; b++)
                {
                    dictIdx <<= 1;
                    if (GetBit(StringCoeffs, b* StringLength + i)) dictIdx |= 1;
                }

                sb.Append((char)CharDictionary[dictIdx]);
            }

            return sb.ToString();
        }


        int NextPow2(int c) {
            c--;
            c |= c >> 1;
            c |= c >> 2;
            c |= c >> 4;
            c |= c >> 8;
            c |= c >> 16;
            return ++c;
        }

        private int Find([NotNull]uint[] dict, uint c)
        {
            for (int i = 0; i < dict.Length; i++)
            {
                if (c == dict[i]) return i;
            }
            return -1;
        }

        private bool GetBit([NotNull]byte[] bytes, int index)
        {
            var ai = index / 8;
            var sh = index % 8;
            var v = (byte)(1 << sh);
            return (bytes[ai] & v) != 0;
        }

        private void SetBit([NotNull]byte[] bytes, int index, bool value)
        {
            var ai = index / 8;
            var sh = index % 8;
            var v = (byte)(1 << sh);
            if (value) {
                bytes[ai] |= v;
            } else {
                bytes[ai] &= (byte)~v;
            }
        }

        public int ByteSize()
        {
            return StringCoeffs.Length + (CharDictionary.Length * 4) + 4 + 2;
            // final `+4+2` is for string length and dict length, which are implicit to the arrays.
            // assumes less that 65k unique characters in the string
        }
    }
}
