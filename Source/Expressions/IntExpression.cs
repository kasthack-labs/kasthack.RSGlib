// RandomStringGenerator / RandomStringGenerator / IntExpression.cs
// Written by kasthack
// ( 2013.09.21 )

using System.Text;

namespace RandomStringGenerator.Expressions {
    public class IntExpression : IExpression {
        public NumberFormat Format;
        private int _max;
        private int _min;
        [System.Diagnostics.DebuggerNonUserCode]
        public IntExpression() { }
        public int Min { get { return this._min; } set { this._min = value + 1; } }
        public int Max { get { return this._max; } set { this._max = value + 1; } }
        /// <summary>
        /// Get string representation of expression execution result
        /// </summary>
        /// <returns>string result</returns>
        public string GetString() {
            return new string( this.Format == NumberFormat.Decimal ? Generators.IntToDecString( Generators.Random.Next( this._min, this._max ) ) :
                Generators.IntToHexString( Generators.Random.Next( this._min, this._max ) ) );
        }
        /// <summary>
        /// Get char array representation of expression execution result
        /// </summary>
        /// <returns>char[] result</returns>
        public char[] GetChars() {
            return this.Format == NumberFormat.Decimal ? Generators.IntToDecString( Generators.Random.Next( this._min, this._max ) ) :
                Generators.IntToHexString( Generators.Random.Next( this._min, this._max ) );
        }
        /// <summary>
        /// Get native representation of expression execution result
        /// </summary>
        /// <returns>ascii bytes</returns>
        public byte[] GetAsciiBytes() {
            return this.Format == NumberFormat.Decimal ? Generators.IntToDecStringBytes( Generators.Random.Next( this._min, this._max ) ) :
                Generators.IntToHexStringBytes( Generators.Random.Next( this._min, this._max ) );
        }
        /// <summary>
        /// Get bytes of result encoded with encoding
        /// </summary>
        /// <param name="enc">encoding for encoding, lol</param>
        /// <returns>bytes</returns>
        public byte[] GetEncodingBytes( Encoding enc ) {
            return enc.GetBytes( this.Format == NumberFormat.Decimal ? Generators.IntToDecString( Generators.Random.Next( this._min, this._max ) ) :
                Generators.IntToHexString( Generators.Random.Next( this._min, this._max ) ) );
        }
        public System.Collections.Generic.IEnumerable<byte[]> EnumAsciiBuffers() { return new[] { this.GetAsciiBytes() }; }
        public System.Collections.Generic.IEnumerable<string> EnumStrings() { return new[] { this.GetString() }; }
        public unsafe void GetInsertLength( ref int* outputdata ) {
            var value = Generators.Random.Next( this._min, this._max );
            *outputdata++ = value;
            *outputdata++ = this.Format == NumberFormat.Decimal ? Generators.GetDecStringLength( value ) : Generators.GetHexStringLength( value );
            *outputdata++ = -value;
        }
        public int ComputeLengthDataSize() {
            return 3; // 1 -cached value,  2 - __len,3 - cached value nuller,
            //bad idea but __i have nothin better
        }
        public unsafe void InsertAsciiBytes( ref int* size, ref byte* outputBuffer ) {
            if ( this.Format == NumberFormat.Decimal ) {
                Generators.IntToDecStringBytesInsert( outputBuffer, *size++, (byte) *size++ );
                outputBuffer -= *size++;
                return;
            }
            fixed ( byte* hexPointer = Generators.HexCharsBytes )
                Generators.IntToHexStringBytesInsert( outputBuffer, hexPointer, *size++, (byte) *size++ );
            outputBuffer -= *size++;
        }
        public unsafe void InsertAsciiChars( ref int* size, ref char* outputBuffer ) {
            if ( this.Format == NumberFormat.Decimal ) {
                Generators.IntToDecStringInsert( outputBuffer, *size++, (byte) *size++ );
                outputBuffer -= *size++;
                return;
            }
            fixed ( char* hexPointer = Generators.HexChars )
                Generators.IntToHexStringInsert( outputBuffer, hexPointer, *size++, (byte) *size++ );
            outputBuffer -= *size++;
        }
        /// <summary>
        /// alias 4 GetString. 4 debugging
        /// </summary>
        /// <returns></returns>
        public override string ToString() { return this.GetString(); }
    }
}