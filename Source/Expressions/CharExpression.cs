// RandomStringGenerator / RandomStringGenerator / CharExpression.cs
// Written by kasthack
// ( 2013.09.21 )

using System;
using System.Text;
using RandomStringGenerator.Helpers;

namespace RandomStringGenerator.Expressions {
    [Serializable]
    public class CharExpression : IExpression {
        private int _max;
        private int _min;
        [System.Diagnostics.DebuggerNonUserCode]
        public CharExpression() { }
        public int Min { get { return this._min; } set { this._min = value; } }
        public int Max { get { return this._max-1; } set { this._max = value + 1; } }
        public byte[] GetAsciiBytes() { return new[] { (byte) Generators.Random.Next( this._min, this._max ) }; }
        public char[] GetChars() { return new[] { (char) Generators.Random.Next( this._min, this._max ) }; }
        public byte[] GetEncodingBytes( Encoding enc ) { return enc.GetBytes( new[] { (char) Generators.Random.Next( this._min, this._max ) } ); }
        public string GetString() { return ( (char) Generators.Random.Next( this._min, this._max ) ).ToString(); }
        public unsafe void GetInsertLength( ref int* outputdata ) { *outputdata++ = 1; }
        public int ComputeLengthDataSize() { return 1; }
        public unsafe void InsertAsciiBytes( ref int* size, ref byte* outputBuffer ) {
            *outputBuffer++ = (byte) Generators.Random.Next( this._min, this._max );
            size++;
        }
        public unsafe void InsertAsciiChars( ref int* size, ref char* outputBuffer ) {
            *outputBuffer++ = (char) Generators.Random.Next( this._min, this._max );
            size++;
        }

        /// <summary>
        /// Show string from which it was compiled
        /// </summary>
        /// <returns></returns>
        public string Decompile() {
            return String.Format( @"{0}C:{2}:{3}{1}", @"{", @"}", this.Min, this.Max );
        }

        public override string ToString() { return this.GetString(); }
    }
}