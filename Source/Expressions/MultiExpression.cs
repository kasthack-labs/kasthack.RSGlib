// RandomStringGenerator / RandomStringGenerator / MultiExpression.cs
// Written by kasthack
// ( 2013.09.21 )

using System;
using System.Linq;
using System.Text;

namespace RandomStringGenerator.Expressions {
    [Serializable]
    public class MultiExpression : IExpression {
        public IExpression[] Expressions;
        public unsafe char[] GetChars() {
            if ( this.Expressions.Length == 1 )
                return this.Expressions[ 0 ].GetChars();
            var outsize = 0;
            int* s;
            var sizeBuf = new int[ this.ComputeLengthDataSize() ]; //buffer 4 sizes
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
        public byte[] GetEncodingBytes( Encoding enc ) {
            return this.Expressions.SelectMany( a => a.GetEncodingBytes( enc ) ).ToArray();
        }
        public unsafe void GetInsertLength( ref int* outputdata ) {
            var len = this.Expressions.Length;
            for ( var i = 0; i < len; i++ )
                this.Expressions[ i ].GetInsertLength( ref outputdata );
        }
        public int ComputeLengthDataSize() {
            int sum = 0, len = this.Expressions.Length;
            for ( var i = 0; i < len; i++ )
                sum += this.Expressions[ i ].ComputeLengthDataSize();
            return sum;
        }
        public unsafe byte[] GetAsciiBytes() {
            if ( this.Expressions.Length == 1 )
                return this.Expressions[ 0 ].GetAsciiBytes();
            var outsize = 0;
            int* s;
            var sizeBuf = new int[ this.ComputeLengthDataSize() ]; //buffer 4 sizes
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
        public unsafe void InsertAsciiBytes( ref int* sizeData, ref byte* outputBuffer ) {
            var len = this.Expressions.Length;
            for ( var i = 0; i < len; i++ )
                this.Expressions[ i ].InsertAsciiBytes( ref sizeData, ref outputBuffer );
        }
        public unsafe void InsertAsciiChars( ref int* sizeData, ref char* outputBuffer ) {
            var len = this.Expressions.Length;
            for ( var i = 0; i < len; this.Expressions[ i++ ].InsertAsciiChars( ref sizeData, ref outputBuffer ) ) { }
        }

        /// <summary>
        /// Show string from which it was compiled
        /// </summary>
        /// <returns></returns>
        public string Decompile() {
            return String.Concat(Expressions.Select(a => a.Decompile()));
        }

        public override string ToString() {
            return this.Expressions.Length == 1
                ? this.Expressions[ 0 ].ToString()
                : new string( this.GetChars() );
        }
    }
}