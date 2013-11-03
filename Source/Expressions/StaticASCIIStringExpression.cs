// RandomStringGenerator / RandomStringGenerator / StaticASCIIStringExpression.cs
// Written by kasthack
// ( 2013.09.21 )

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RandomStringGenerator.Expressions {
    public class StaticASCIIStringExpression : IExpression {
        private readonly byte[] _buf;
        private readonly ASCIIEncoding _enc;
        
        public StaticASCIIStringExpression( string str, ASCIIEncoding enc = null ) {
            this._enc = enc ?? new ASCIIEncoding();
            this._buf = this._enc.GetBytes( str );
        }
        public StaticASCIIStringExpression( char[] str, ASCIIEncoding enc = null ) {
            this._enc = enc ?? new ASCIIEncoding();
            this._buf = this._enc.GetBytes( str );
        }
        public StaticASCIIStringExpression( byte[] str, ASCIIEncoding enc = null ) {
            this._enc = enc ?? new ASCIIEncoding();
            this._buf = str;
        }
        
        public char[] GetChars() { return this._enc.GetChars( this._buf ); }
        public byte[] GetAsciiBytes() { return this._buf; }
        public byte[] GetEncodingBytes( Encoding enc ) { return enc.GetBytes( this._enc.GetChars( this._buf ) ); }
        
        public unsafe void GetInsertLength( ref int* outputdata ) { *outputdata++ = this._buf.Length; }
        public int ComputeLengthDataSize() { return 1; }
        public unsafe void InsertAsciiBytes( ref int* size, ref byte* outputBuffer ) {
            var p = new IntPtr( outputBuffer );
            Marshal.Copy( this._buf, 0, p, *size );
            outputBuffer += *size++;
        }
        public unsafe void InsertAsciiChars( ref int* size, ref char* outputBuffer ) {
            fixed ( byte* tmpbuf = this._buf ) {
                var start = tmpbuf;
                var end = tmpbuf + *size++;
                do
                    *outputBuffer++ = (char) *start++;
                while ( start < end );
            }
        }
        public override string ToString() { return this.GetString(); }
    }
}