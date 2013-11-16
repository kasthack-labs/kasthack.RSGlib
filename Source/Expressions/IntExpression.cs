// RandomStringGenerator / RandomStringGenerator / IntExpression.cs
// Written by kasthack
// ( 2013.09.21 )

using System;
using System.Text;
using RandomStringGenerator.Helpers;

namespace RandomStringGenerator.Expressions {

    [Serializable]
    public class IntExpression : IExpression {
        public NumberFormat Format;
        private int _max;
        private int _min;
        [System.Diagnostics.DebuggerNonUserCode]
        public IntExpression() { }
        public int Min { get { return this._min; } set { this._min = value; } }
        public int Max { get { return this._max - 1; } set { this._max = value + 1; } }
        public char[] GetChars() {
            return this.Format == NumberFormat.Decimal ? Generators.IntToDecString( Generators.Random.Next( this._min, this._max ) ) :
                Generators.IntToHexString( Generators.Random.Next( this._min, this._max ) );
        }
        public byte[] GetAsciiBytes() {
            return this.Format == NumberFormat.Decimal ? Generators.IntToDecStringBytes( Generators.Random.Next( this._min, this._max ) ) :
                Generators.IntToHexStringBytes( Generators.Random.Next( this._min, this._max ) );
        }
        public byte[] GetEncodingBytes( Encoding enc ) {
            return enc.GetBytes( this.Format == NumberFormat.Decimal ? Generators.IntToDecString( Generators.Random.Next( this._min, this._max ) ) :
                Generators.IntToHexString( Generators.Random.Next( this._min, this._max ) ) );
        }
        public unsafe void GetInsertLength( ref int* outputdata ) {
            var value = Generators.Random.Next( this._min, this._max );
            *outputdata++ = value;
            *outputdata++ = this.Format == NumberFormat.Decimal ? Generators.GetDecStringLength( value ) : Generators.GetHexStringLength( value );
            *outputdata++ = -value;
        }
        public int ComputeLengthDataSize() {
            return 3; // 1 -cached value,  2 - __len,3 - cached value nuller, //bad idea but __i have nothin better
        }
        public unsafe void InsertAsciiBytes( ref int* sizeData, ref byte* outputBuffer ) {
            if ( this.Format == NumberFormat.Decimal ) {
                Generators.IntToDecStringBytesInsert( outputBuffer, *sizeData++, (byte) *sizeData++ );
                outputBuffer -= *sizeData++;
                return;
            }
            fixed ( byte* hexPointer = Generators.HexCharsBytes )
                Generators.IntToHexStringBytesInsert( outputBuffer, hexPointer, *sizeData++, (byte) *sizeData++ );
            outputBuffer -= *sizeData++;
        }
        public unsafe void InsertAsciiChars( ref int* sizeData, ref char* outputBuffer ) {
            if ( this.Format == NumberFormat.Decimal ) {
                Generators.IntToDecStringInsert( outputBuffer, *sizeData++, (byte) *sizeData++ );
                outputBuffer -= *sizeData++;
                return;
            }
            fixed ( char* hexPointer = Generators.HexChars )
                Generators.IntToHexStringInsert( outputBuffer, hexPointer, *sizeData++, (byte) *sizeData++ );
            outputBuffer -= *sizeData++;
        }

        /// <summary>
        /// Show string from which it was compiled
        /// </summary>
        /// <returns></returns>
        public string Decompile() {
            return String.Format(
                @"{0}I:{2}:{3}:{4}{1}",
                @"{",
                @"}",
                this.Format==NumberFormat.Decimal?'D':'H',
                this.Min,
                this.Max
            );
        }

        public override string ToString() {
            return new string( this.Format == NumberFormat.Decimal ?
                Generators.IntToDecString( Generators.Random.Next( this._min, this._max ) ) :
                Generators.IntToHexString( Generators.Random.Next( this._min, this._max ) ) );
        }
    }
}