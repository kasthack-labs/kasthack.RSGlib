// RandomStringGenerator / RandomStringGenerator / RepeatExpression.cs
// Written by kasthack
// ( 2013.09.21 )

using System.Linq;
using System.Text;

namespace RandomStringGenerator.Expressions {
    public class RepeatExpression : IExpression {
        public IExpression[] Expressions;
        private int _max;
        private int _min;
        public int Min { get { return this._min; } set { this._min = value + 1; } }
        public int Max { get { return this._max; } set { this._max = value + 1; } }
        public string GetString() { return new string( this.GetChars() ); }
        public byte[] GetAsciiBytes() { return this.GetAsciiBytes( Generators.Random.Next( this._min, this._max ) ); }
        public char[] GetChars() { return this.GetChars( Generators.Random.Next( this._min, this._max ) ); }
        public byte[] GetEncodingBytes( Encoding enc ) { return this.GetEncodingBytes( enc, Generators.Random.Next( this._min, this._max ) ); }
        public System.Collections.Generic.IEnumerable<byte[]> EnumAsciiBuffers() {
            return Enumerable.Range( 0, Generators.Random.Next( this._min, this._max ) ).
                              SelectMany( a => this.Expressions.SelectMany( b => b.EnumAsciiBuffers() ) ).
                              ToArray();
        }
        public System.Collections.Generic.IEnumerable<string> EnumStrings() {
            return Enumerable.Range( 0, Generators.Random.Next( this._min, this._max ) ).
                              SelectMany(
                                         a => this.Expressions.SelectMany( b => b.EnumStrings() )
                );
        }
        public unsafe void GetInsertLength( ref int* outputdata ) {
            int len = this.Expressions.Length, value = Generators.Random.Next( this._min, this._max );
            *outputdata++ = value;
            *outputdata++ = -value;
            for ( var j = 0; j < value; j++ )
                for ( var i = 0; i < len; i++ )
                    this.Expressions[ i ].GetInsertLength( ref outputdata );
        }
        public int ComputeLengthDataSize() {
            int sum = 0, len = this.Expressions.Length;
            for ( var i = 0; i < len; i++ )
                sum += this.Expressions[ i ].ComputeLengthDataSize();
            return sum * this._max + 2; //inner expressions+repeat count 
        }
        public unsafe void InsertAsciiBytes( ref int* size, ref byte* outputBuffer ) {
            int len = this.Expressions.Length, rpt = *size++;
            size++;
            for ( var j = 0; j < rpt; j++ )
                for ( var i = 0; i < len; i++ )
                    this.Expressions[ i ].InsertAsciiBytes( ref size, ref outputBuffer );
        }
        public unsafe void InsertAsciiChars( ref int* size, ref char* outputBuffer ) {
            int len = this.Expressions.Length, rpt = *size;
            size += 2;
            for ( var j = 0; j < rpt; j++ )
                for ( var i = 0; i < len; i++ )
                    this.Expressions[ i ].InsertAsciiChars( ref size, ref outputBuffer );
        }
        public unsafe byte[] GetAsciiBytes( int repeatCount ) {
            if ( repeatCount == 0 ) return new byte[] { };
            if ( this.Expressions.Length == 1 && repeatCount == 1 )
                return this.Expressions[ 0 ].GetAsciiBytes();
            var outsize = 0;
            var sizeLen = this.CompLen() * repeatCount + 2;
            int* s;
            var sizeBuf = new int[ sizeLen ]; //buffer 4 sizes
            long rcount;
            //get generation data
            fixed ( int* szb = sizeBuf ) {
                s = szb;
                this.GetInsertLength( ref s );
                rcount = s - szb;
            }
            //compute output length
            for ( var i = 0; i < rcount; outsize += sizeBuf[ i++ ] ) { }
            var buffer = new byte[ outsize ];
            //gen!
            fixed ( int* szb = sizeBuf )
            fixed ( byte* outb = buffer ) {
                s = szb;
                var b = outb;
                this.InsertAsciiBytes( ref s, ref b );
            }
            return buffer;
        }
        public unsafe char[] GetChars( int repeatCount ) {
            //same as get ascii bytes but with chars
            if ( repeatCount == 0 )
                return new char[] { };
            if ( this.Expressions.Length == 1 && repeatCount == 1 )
                return this.Expressions[ 0 ].GetChars();
            var outsize = 0;
            var sizeLen = this.CompLen() * repeatCount + 2;
            int* s;
            var sizeBuf = new int[ sizeLen ]; //buffer 4 sizes
            long rcount;
            //get generation data
            fixed ( int* szb = sizeBuf ) {
                s = szb;
                this.GetInsertLength( ref s );
                rcount = s - szb;
            }
            //compute output length
            for ( var i = 0; i < rcount; outsize += sizeBuf[ i++ ] ) { }
            var buffer = new char[ outsize ];
            //gen!
            fixed ( int* szb = sizeBuf )
            fixed ( char* outb = buffer ) {
                s = szb;
                var b = outb;
                this.InsertAsciiChars( ref s, ref b );
            }
            return buffer;
        }
        public byte[] GetEncodingBytes( Encoding enc, int repeatCount ) {
            //return Functions.GetT<byte>(_RepeatCount, a => a.GetEncodingBytes(_enc), this.Expressions);
            return this.Expressions.SelectMany( a => a.GetEncodingBytes( enc ) ).
                        ToArray();
        }
        private int CompLen() {
            int sum = 0, len = this.Expressions.Length;
            for ( var i = 0; i < len; i++ )
                sum += this.Expressions[ i ].ComputeLengthDataSize();
            return sum;
        }
        public override string ToString() { return this.GetString(); }
    }
}