//#define POINTERS
using System;
using System.Linq;
using System.Text.RegularExpressions;
using FRandom = FastRandom.FastRandom;
namespace RandomStringGenerator {
    public static class Generators {
        #region Variables
        public static readonly FRandom Random = new FRandom();
        #region magic!
        //public jfl
        public static readonly char[] HexChars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
        public static readonly char[] ASCIIChars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o',
  'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
  'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '!','"','#','$','%','&','\'','(',')','*','+',',','-','.',
  '/',':',';', '<','=','>','?','@','[','\\',']','^','_','`','{','|','}','~'};
        public static readonly byte[] ASCIICharsBytes = { 48,49,50,51,52,53,54,55,56,57,97,98,99,100,101,102,103,104,105,106,107,
   108,109,110,111,112,113,114,115,116,117,118,119,120,121,122,65,66,67,
   68,69,70,71,72,73,74,75,76,77,78,79,80,81,82,83,84,85,86,87,88,89,90,
   33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,58,59,60,61,62,63,64,91,92,93,94,95,96,123,124,125,126};
        public static readonly byte[] HexCharsBytes = { 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 97, 98, 99, 100, 101, 102, };
        /*
                public static ushort[] Offsets = { 0, 0, 33, 48, 65, 97 };
        */
        /*
                public static ushort[] Lengs = { 65535, 32, 32, 10, 26, 26 };
        */
        private const char C = '-';
        private const byte B = (byte) '-';
        static readonly char[] Zca = { '0' };
        static readonly byte[] Zba = { (byte) '0' };
        #endregion
        #endregion
        #region Legacy
        public static string RandomString() {
            return new string( RandomASCII( 1, 8, ASCIIChars, 0, 61 ) );
        }
        static string Matchev( Match m ) {
            //slow. just for prototype
            return new String( Enumerable.Repeat( ' ', m.Length ).ToArray() );
        }
        #endregion
        #region Shit(wrappers 4 wrappers
        /*parsers*/
        public static int QIntParse( char[] input ) {
            return QIntParse( input, 0, input.Length );
        }
        public static long QLongParse( char[] input ) {
            return QLongParse( input, 0, input.Length );
        }
        /*random strings*/
        public static char[] RandomASCII( int minLen, int maxLen ) {
            return RandomASCII( minLen, maxLen, ASCIIChars, 0, ASCIIChars.Length - 1 );
        }
        public static char[] RandomASCII( int minLen, int maxLen, char[] source, int startindex, int maxindex ) {
            return RandomASCII( Random.Next( minLen, maxLen + 1 ), source, startindex, maxindex );
        }
        public static char[] RandomUTFURLEncodeString( int minRealLen, int maxRealLen ) {
            return RandomUTFURLEncodeString( Random.Next( minRealLen, maxRealLen ) );
        }
        /*random_strings*/
        public static byte[] RandomASCIIBytes( int minLen, int maxLen ) {
            return RandomASCIIBytes( minLen, maxLen, ASCIICharsBytes, 0, ASCIICharsBytes.Length - 1 );
        }
        public static byte[] RandomASCIIBytes( int minLen, int maxLen, byte[] source, int startindex, int maxindex ) {
            return RandomASCIIBytes( Random.Next( minLen, maxLen + 1 ), source, startindex, maxindex );
        }
        public static byte[] RandomUTFURLEncodeStringBytes( int minRealLen, int maxRealLen ) {
            return RandomUTFURLEncodeStringBytes( Random.Next( minRealLen, maxRealLen ) );
        }
        #endregion
        #region unsafe Wrappers
        /*parsers*/
        public static unsafe long QLongParse( char[] input, int @from, int count ) {
            fixed ( char* cinput = input )
                return QLongParse( cinput + @from, count );
        }
        public static unsafe int QIntParse( char[] input, int @from, int count ) {
            fixed ( char* cinput = input )
                return QIntParse( cinput + @from, count );
        }
        /*to_strings*/
        public static unsafe char[] IntToHexString( long i ) {
            if ( i == 0 ) return Zca;
            var sz = GetHexStringLength( i );
            var output = new char[ sz ];
            fixed ( char* outArr = output )
            fixed ( char* srcArr = HexChars )
                IntToHexStringInsert( outArr, srcArr, i, sz );
            return output;
        }
        public static unsafe char[] IntToDecString( long i ) {
            if ( i == 0 ) return Zca;
            var sz = GetDecStringLength( i );
            var output = new char[ sz ];
            fixed ( char* outArr = output )
                IntToDecStringInsert( outArr, i, sz );
            return output;
        }
        public static unsafe char[] IntToHexString( int i ) {
            if ( i == 0 ) return Zca;
            var sz = GetHexStringLength( i );
            var output = new char[ sz ];
            fixed ( char* outArr = output )
            fixed ( char* srcArr = HexChars )
                IntToHexStringInsert( outArr, srcArr, i, sz );
            return output;
        }
        public static unsafe char[] IntToDecString( int i ) {
            if ( i == 0 ) return Zca;
            var sz = GetDecStringLength( i );
            var output = new char[ sz ];
            fixed ( char* outArr = output )
                IntToDecStringInsert( outArr, i, sz );
            return output;
        }
        /*real engine*/
        public static unsafe char[] RandomUTFURLEncodeString( int len ) {
            var output = new char[ len ];
            fixed ( char* outPointer = output )
                RandomUTFURLEncodeStringInsert( outPointer, len );
            return output;
        }
        public static unsafe char[] RandomASCII( int len, char[] source, int startindex, int maxindex ) {
            var output = new char[ len ];
            fixed ( char* outPointer = output )
            fixed ( char* csource = source )
                RandomASCIIInsert( outPointer, len, csource + startindex, maxindex - startindex );
            return output;
        }
        /*same but with bytes*/
        public static unsafe byte[] IntToHexStringBytes( long i ) {
            if ( i == 0 ) return Zba;
            var sz = GetHexStringLength( i );
            var output = new byte[ sz ];
            fixed ( byte* outArr = output )
            fixed ( byte* srcArr = HexCharsBytes )
                IntToHexStringBytesInsert( outArr, srcArr, i, sz );
            return output;
        }
        public static unsafe byte[] IntToDecStringBytes( long i ) {
            if ( i == 0 ) return Zba;
            var sz = GetDecStringLength( i );
            var output = new byte[ sz ];
            fixed ( byte* outArr = output )
                IntToDecStringBytesInsert( outArr, i, sz );
            return output;
        }
        public static unsafe byte[] IntToHexStringBytes( int i ) {
            if ( i == 0 ) return Zba;
            var sz = GetHexStringLength( i );
            var output = new byte[ sz ];
            fixed ( byte* outArr = output )
            fixed ( byte* srcArr = HexCharsBytes )
                IntToHexStringBytesInsert( outArr, srcArr, i, sz );
            return output;
        }
        public static unsafe byte[] IntToDecStringBytes( int _i ) {
            if ( _i == 0 ) return Zba;
            var sz = GetDecStringLength( _i );
            var output = new byte[ sz ];
            fixed ( byte* outArr = output )
                IntToDecStringBytesInsert( outArr, _i, sz );
            return output;
        }
        /*real generators*/
        public static unsafe byte[] RandomUTFURLEncodeStringBytes( int realLen ) {
            var output = new byte[ realLen ];
            fixed ( byte* outPointer = output )
                RandomUTFURLEncodeStringBytesInsert( outPointer, realLen );
            return output;
        }
        public static unsafe byte[] RandomASCIIBytes( int len, byte[] source, int startindex, int maxindex ) {
            var output = new byte[ len ];
            fixed ( byte* outPointer = output )
            fixed ( byte* csource = source )
                RandomASCIIBytesInsert( outPointer, len, csource + startindex, maxindex - startindex );
            return output;
        }
        #endregion
        #region Real Generators
        /*parsers*/
        public static unsafe int QIntParse( char* input, int count ) {
            var sum = 0;//, __cnt = 0;
            var end = input + count;
            var pos = true;
            if ( *input == '-' ) {
                pos = false;
                input++;
            }
            while ( input < end )
                sum = sum * 10 + *( input++ ) - 48;
            return pos ? sum : -sum;
        }
        public static unsafe long QLongParse( char* input, int count ) {
            long sum = 0;//, __cnt = 0;
            var end = input + count;
            var pos = true;
            if ( *input == '-' ) {
                pos = false;
                input++;
            }
            while ( input < end ) {
                sum *= 10;
                sum += ( (int) *input++ ) - 48;
            }
            return pos ? sum : -sum;
        }
        public static unsafe int FindChar( char* @from, char* end, char c ) {
            var cnt = 0;
            while ( @from < end && *@from != c ) {
                cnt++;
                @from++;
            };
            return cnt;
        }
        /*generators with pointers*/
        public static unsafe void RandomUTFURLEncodeStringBytesInsert( byte* ptr, int realLen ) {
            byte* end = ( ptr + realLen * 6 );
            ushort rnd;
            const byte pc = (byte) '%';

            uint bfr = 0;
            byte havenums = 0;
            fixed ( byte* hexChars = HexCharsBytes ) {
                while ( ptr < end ) {
                    //using temp uint to prevent unnecessary random generating
                    rnd = (ushort) bfr;
                    if ( havenums-- == 0 ) {
                        bfr = Random.NextUInt();
                        havenums = 1;
                        rnd = (ushort) ( bfr >> 16 );
                    }
                    //indexing to use out-of-order optimizations
                    *( ptr + 1 ) = pc;
                    *( ptr + 2 ) = *( hexChars + ( rnd & 0xf ) );
                    *( ptr + 3 ) = *( hexChars + ( ( rnd >> 4 ) & 0xf ) );
                    *( ptr + 4 ) = pc;
                    *( ptr + 5 ) = *( hexChars + ( ( rnd >> 8 ) & 0xf ) );
                    *( ptr + 6 ) = *( hexChars + ( ( rnd >> 12 ) & 0xf ) );
                    ptr += 6;
                }
            }
        }
        public static unsafe void RandomASCIIBytesInsert( byte* ptr, int len, byte* source, int maxindex ) {
            maxindex++;
            byte* end = ptr + len;
            while ( ptr < end ) *ptr++ = *( source + Random.Next( maxindex ) );
        }
        public static unsafe void RandomUTFURLEncodeStringInsert( char* ptr, int realLen ) {
            char* end = ( ptr + realLen * 6 );
            ushort __rnd;
            const char __pc = '%';

            uint bfr = 0;
            byte havenums = 0;
            fixed ( char* hexChars = HexChars ) {
                while ( ptr < end ) {
                    //using temp uint to prevent unnecessary random generating
                    __rnd = (ushort) bfr;
                    if ( havenums-- == 0 ) {
                        bfr = Random.NextUInt();
                        havenums = 1;
                        __rnd = (ushort) ( bfr >> 16 );
                    }
                    //indexing to use out-of-order optimizations
                    *( ptr + 1 ) = __pc;
                    *( ptr + 4 ) = __pc;
                    *( ptr + 2 ) = *( hexChars + ( __rnd & 0xf ) );
                    *( ptr + 3 ) = *( hexChars + ( ( __rnd >> 4 ) & 0xf ) );
                    *( ptr + 5 ) = *( hexChars + ( ( __rnd >> 8 ) & 0xf ) );
                    *( ptr + 6 ) = *( hexChars + ( ( __rnd >> 12 ) & 0xf ) );
                    ptr += 6;
                }
            }
        }
        public static unsafe void RandomASCIIInsert( char* ptr, int len, char* source, int maxindex ) {
            maxindex++;
            char* end = ( ptr + len );
            while ( ptr < end ) *ptr++ = *( source + Random.Next( maxindex ) );
        }
        /*get_to_string_size*/
        public static byte GetDecStringLength( int i ) {
            byte sz = 1;
            if ( i < 0 ) { sz++; i = -i; }
            while ( ( i /= 10 ) > 0 ) sz++;
            return sz;
        }
        public static byte GetDecStringLength( long i ) {
            byte sz = 1;
            if ( i < 0 ) { sz++; i = -i; }
            while ( ( i /= 10 ) > 0 ) ++sz;
            return sz;
        }
        public static byte GetHexStringLength( int i ) {
            byte sz = 1;
            if ( i < 0 ) { sz++; i = -i; }
            while ( ( i >>= 4 ) != 0 ) ++sz;
            return sz;
        }
        public static byte GetHexStringLength( long i ) {
            byte sz = 1;
            if ( i < 0 ) { sz++; i = -i; }
            while ( ( i >>= 4 ) > 0 ) sz++;
            return sz;
        }
        /*to_strings*/
        public static unsafe void IntToHexStringBytesInsert( byte* outarr, byte* sourceArr, int i, byte sz ) {
            if ( i < 0 ) i = -i;
            *outarr = B;
            outarr += sz;
            do *--outarr = *( sourceArr + ( i & 0x0f ) ); while ( ( i >>= 4 ) != 0 );
        }
        public static unsafe void IntToHexStringBytesInsert( byte* outArr, byte* sourceArr, long i, byte sz ) {
            if ( i < 0 ) i = -i;
            *outArr = B;
            outArr += sz;
            do *--outArr = *( sourceArr + ( i & 0x0f ) ); while ( ( i >>= 4 ) != 0 );
        }
        public static unsafe void IntToHexStringInsert( char* outArr, char* sourceArr, long i, byte sz ) {
            if ( i < 0 ) i = -i;
            *outArr = C;
            outArr += sz;
            do *--outArr = *( sourceArr + ( i & 0x0f ) ); while ( ( i >>= 4 ) != 0 );
        }
        public static unsafe void IntToHexStringInsert( char* outArr, char* sourceArr, int i, byte sz ) {
            if ( i < 0 ) i = -i;
            *outArr = C;
            outArr += sz;
            do *--outArr = *( sourceArr + ( i & 0x0f ) ); while ( ( i >>= 4 ) != 0 );
        }
        public static unsafe void IntToDecStringBytesInsert( byte* outArr, long i, byte sz ) {
            if ( i < 0 ) i = -i;
            *outArr = B;
            outArr += sz;
            do *--outArr = (byte) ( i % 10 + 48 ); while ( ( i /= 10 ) != 0 );
        }
        public static unsafe void IntToDecStringBytesInsert( byte* outArr, int i, byte sz ) {
            if ( i < 0 ) i = -i;
            *outArr = B;
            outArr += sz;
            do *--outArr = (byte) ( i % 10 + 48 ); while ( ( i /= 10 ) != 0 );
        }
        public static unsafe void IntToDecStringInsert( char* outarr, long i, byte sz ) {
            if ( i < 0 ) i = -i;
            *outarr = C;
            outarr += sz;
            do *--outarr = (char) ( i % 10 + 48 ); while ( ( i /= 10 ) != 0 );
        }
        public static unsafe void IntToDecStringInsert( char* outarr, int i, byte sz ) {
            if ( i < 0 ) i = -i;
            *outarr = C;
            outarr += sz;
            do *--outarr = (char) ( i % 10 + 48 ); while ( ( i /= 10 ) != 0 );
        }
        #endregion
    }
}
