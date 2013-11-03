// RandomStringGenerator / RandomStringGenerator / FormattedStringGenerator.cs
// Written by kasthack
// ( 2013.09.21 )
using System.Linq;
using System.Text;
namespace RandomStringGenerator
{
    public class FormattedStringGenerator : IExpression
    {
        public IExpression[] Expressions;
        /// <summary>
        /// Get string representation of expression execution result
        /// </summary>
        /// <returns>string result</returns>
        public string GetString() {
            if ( Expressions.Length == 1 )
                return Expressions[ 0 ].GetString();
            return new string(GetChars());
        }
        /// <summary>
        /// Get char array representation of expression execution result
        /// </summary>
        /// <returns>char[] result</returns>
        public unsafe char[] GetChars() {
            if ( Expressions.Length == 1 )
                return Expressions[ 0 ].GetChars();
            var outsize = 0;
            int* s;
            var sizeBuf = new int[this.ComputeLengthDataSize()]; //buffer 4 sizes
            long rcount;
            //get generation data
            fixed ( int* szb = sizeBuf ) {
                s = szb;
                GetInsertLength(ref s);
                rcount = s - szb;
            }
            //compute output length
            for (var i = 0; i < rcount; outsize += sizeBuf[ i++ ]) {}
            var buffer = new char[outsize];
            //gen!
            fixed ( int* szb = sizeBuf )
                fixed ( char* outb = buffer ) {
                    s = szb;
                    var b = outb;
                    InsertAsciiChars(ref s, ref b);
                }
            return buffer;
        }
        /// <summary>
        /// Get native representation of expression execution result
        /// </summary>
        /// <returns>ascii bytes</returns>		
        /*public byte[] GetAsciiBytes() {
            if ( Expressions.Length == 1 )
                return Expressions[0].GetAsciiBytes();
            return Functions.GetT<byte>(1, Functions.GetBytesF, Expressions);
        }*/
        /// <summary>
        /// Get bytes of result encoded with encoding
        /// DON'T USE IT.
        /// </summary>
        /// <param name="enc">encoding for encoding, lol</param>
        /// <returns>bytes</returns>
        public byte[] GetEncodingBytes(Encoding enc) {
            return this.Expressions.SelectMany(a => a.GetEncodingBytes(enc)).
                        ToArray();
            //return Functions.GetT<byte>(1, a => a.GetEncodingBytes(_enc), this.Expressions);
        }
        public System.Collections.Generic.IEnumerable<byte[]> EnumAsciiBuffers() { return Expressions.SelectMany(a => a.EnumAsciiBuffers()); }
        public System.Collections.Generic.IEnumerable<string> EnumStrings() { return Expressions.SelectMany(a => a.EnumStrings()); }
        public unsafe void GetInsertLength(ref int* outputdata) {
            var len = Expressions.Length;
            for (var i = 0; i < len; i++)
                Expressions[ i ].GetInsertLength(ref outputdata);
        }
        public int ComputeLengthDataSize() {
            int sum = 0, len = Expressions.Length;
            for (var i = 0; i < len; i++)
                sum += Expressions[ i ].ComputeLengthDataSize();
            return sum;
        }
        public unsafe byte[] GetAsciiBytes() {
//_GetPointedBytes() {
            if ( Expressions.Length == 1 )
                return Expressions[ 0 ].GetAsciiBytes();
            var outsize = 0;
            int* s;
            var sizeBuf = new int[this.ComputeLengthDataSize()]; //buffer 4 sizes
            long rcount;
            //get generation data
            fixed ( int* szb = sizeBuf ) {
                s = szb;
                GetInsertLength(ref s);
                rcount = s - szb;
            }
            //compute output length
            for (var i = 0; i < rcount; outsize += sizeBuf[ i++ ]) {}
            var buffer = new byte[outsize];
            //gen!
            fixed ( int* szb = sizeBuf )
                fixed ( byte* outb = buffer ) {
                    s = szb;
                    var b = outb;
                    InsertAsciiBytes(ref s, ref b);
                }
            return buffer;
        }
        public unsafe void InsertAsciiBytes(ref int* size, ref byte* outputBuffer) {
            var len = Expressions.Length;
            for (var i = 0; i < len; i++)
                Expressions[ i ].InsertAsciiBytes(ref size, ref outputBuffer);
        }
        public unsafe void InsertAsciiChars(ref int* size, ref char* outputBuffer) {
            var len = Expressions.Length;
            for (var i = 0; i < len; Expressions[ i++ ].InsertAsciiChars(ref size, ref outputBuffer)) {}
        }
        /// <summary>
        /// alias 4 GetString. 4 debugging
        /// </summary>
        /// <returns></returns>
        public override string ToString() { return GetString(); }
    }
}