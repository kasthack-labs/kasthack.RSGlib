using System.Linq;
using System.Text;
namespace RandomStringGenerator {
    public class RepeatExpression : IExpression {
        int _min, _max;
        public int Min {
            get {return this._min;}
            set {this._min = value + 1;}
        }
        public int Max {
            get {return this._max;}
            set {this._max = value + 1;}
        }
        public IExpression[] Expressions;

        public string GetString() {return new string( GetChars() );}
        public byte[] GetAsciiBytes() {return GetAsciiBytes( Generators.Random.Next( this._min, this._max ) );}
        public unsafe byte[] GetAsciiBytes( int repeatCount ) {
            if ( repeatCount == 0 ) return new byte[] { };
            if ( Expressions.Length == 1 && repeatCount == 1 )
                return Expressions[ 0 ].GetAsciiBytes();
            var outsize = 0;
            var sizeLen = CompLen() * repeatCount + 2;
            int* s;
            var sizeBuf = new int[ sizeLen ];//buffer 4 sizes
            long rcount;
            //get generation data
            fixed ( int* szb = sizeBuf ) {
                s = szb;
                ComputeStringLength( ref s );
                rcount = s - szb;
            }
            //compute output length
            for ( var i = 0; i < rcount; outsize += sizeBuf[ i++ ] ) {}
            var buffer = new byte[ outsize ];
            //gen!
            fixed ( int* szb = sizeBuf ) {
                fixed ( byte* outb = buffer ) {
                    s = szb;
                    var b = outb;
                    GetAsciiBytesInsert( ref s, ref b );
                }
            }
            return buffer;
        }
        public char[] GetChars() {
            return GetChars( Generators.Random.Next( this._min, this._max ) );
        }
        public unsafe char[] GetChars( int repeatCount ) {
            //same as get ascii bytes but with chars
            if ( repeatCount == 0 )
                return new char[] { };
            if ( Expressions.Length == 1 && repeatCount == 1 )
                return Expressions[ 0 ].GetChars();
            var outsize = 0;
            var sizeLen = CompLen() * repeatCount + 2;
            int* s;
            var sizeBuf = new int[ sizeLen ];//buffer 4 sizes
            long rcount;
            //get generation data
            fixed ( int* szb = sizeBuf ) {
                s = szb;
                ComputeStringLength( ref s );
                rcount = s - szb;
            }
            //compute output length
            for ( var i = 0; i < rcount; outsize += sizeBuf[ i++ ] ) {}
            var buffer = new char[ outsize ];
            //gen!
            fixed ( int* szb = sizeBuf ) {
                fixed ( char* outb = buffer ) {
                    s = szb;
                    var b = outb;
                    GetAsciiInsert( ref s, ref b );
                }
            }
            return buffer;
        }
        public byte[] GetEncodingBytes( Encoding enc ) {
            return GetEncodingBytes( enc, Generators.Random.Next( this._min, this._max ) );
        }
        public byte[] GetEncodingBytes( Encoding enc, int repeatCount ) {
            //return Functions.GetT<byte>(_RepeatCount, a => a.GetEncodingBytes(_enc), this.Expressions);
            return this.Expressions.SelectMany( a => a.GetEncodingBytes( enc ) ).ToArray();
        }
        int CompLen() {
            int sum = 0, len = Expressions.Length;
            for ( var i = 0; i < len; i++ )
                sum += Expressions[ i ].ComputeMaxLenForSize();
            return sum;
        }
        public override string ToString() {
            return GetString();
        }
        public System.Collections.Generic.IEnumerable<byte[]> EnumAsciiBuffers() {
            return Enumerable.Range( 0, Generators.Random.Next( this._min, this._max ) ).SelectMany( a => Expressions.SelectMany( b => b.EnumAsciiBuffers() ) ).ToArray();
        }
        public System.Collections.Generic.IEnumerable<string> EnumStrings() {
            return Enumerable.Range( 0, Generators.Random.Next( this._min, this._max ) ).
            SelectMany(
             a => Expressions.SelectMany( b => b.EnumStrings() )
            );
        }
        public unsafe void ComputeStringLength( ref int* outputdata ) {
            int len = Expressions.Length, value = Generators.Random.Next( this._min, this._max );
            *outputdata++ = value;
            *outputdata++ = -value;
            for ( var j = 0; j < value; j++ )
                for ( var i = 0; i < len; i++ )
                    Expressions[ i ].ComputeStringLength( ref outputdata );
        }
        public int ComputeMaxLenForSize() {
            int sum = 0, len = Expressions.Length;
            for ( var i = 0; i < len; i++ )
                sum += Expressions[ i ].ComputeMaxLenForSize();
            return sum * this._max + 2;//inner expressions+repeat count 
        }
        public unsafe void GetAsciiBytesInsert( ref int* size, ref byte* outputBuffer ) {
            int len = Expressions.Length, rpt = *size++;
            size++;
            for ( var j = 0; j < rpt; j++ )
                for ( var i = 0; i < len; i++ )
                    Expressions[ i ].GetAsciiBytesInsert( ref size, ref outputBuffer );
        }
        public unsafe void GetAsciiInsert( ref int* size, ref char* outputBuffer ) {
            int len = Expressions.Length, rpt = *size;
            size+=2;
            for ( var j = 0; j < rpt; j++ )
                for ( var i = 0; i < len; i++ )
                    Expressions[ i ].GetAsciiInsert( ref size, ref outputBuffer );
        }
    }
}
