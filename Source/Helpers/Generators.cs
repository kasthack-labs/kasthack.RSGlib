// RandomStringGenerator / RandomStringGenerator / Generators.cs
// Written by kasthack
// ( 2013.09.21 )

namespace RandomStringGenerator.Helpers {
    public static class Generators {
        #region Variables
        public static readonly FastRandom.FastRandom Random = new FastRandom.FastRandom();
        
        internal static readonly char[] HexChars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
        internal static readonly char[] ASCIIChars = {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o',
            'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
            'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '!', '"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', ',', '-', '.',
            '/', ':', ';', '<', '=', '>', '?', '@', '[', '\\', ']', '^', '_', '`', '{', '|', '}', '~'
        };
        internal static readonly byte[] ASCIICharsBytes = {
            48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107,
            108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 65, 66, 67,
            68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90,
            33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 58, 59, 60, 61, 62, 63, 64, 91, 92, 93, 94, 95, 96, 123, 124, 125, 126
        };
        internal static readonly byte[] HexCharsBytes = { 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 97, 98, 99, 100, 101, 102 };
        private static readonly char[] Zca = { '0' };
        private static readonly byte[] Zba = { (byte) '0' };
        private const char C = '-';
        private const byte B = (byte) '-';
        #endregion
        #region Shit(wrappers 4 wrappers)
        /*parsers*/
        public static int QIntParse( char[] input ) { return QIntParse( input, 0, input.Length ); }
        public static long QLongParse( char[] input ) { return QLongParse( input, 0, input.Length ); }
        /*random strings*/
        public static char[] RandomAscii( int minLen, int maxLen ) { return RandomAscii( minLen, maxLen, ASCIIChars, 0, ASCIIChars.Length - 1 ); }
        public static char[] RandomAscii( int minLen, int maxLen, char[] source, int startindex, int maxindex ) { return RandomAscii( Random.Next( minLen, maxLen + 1 ), source, startindex, maxindex ); }
        public static char[] RandomUtfUrlEncodeString( int minRealLen, int maxRealLen ) { return RandomUtfUrlEncodeString( Random.Next( minRealLen, maxRealLen ) ); }
        /*random bytes*/
        public static byte[] RandomAsciiBytes( int minLen, int maxLen ) { return RandomAsciiBytes( minLen, maxLen, ASCIICharsBytes, 0, ASCIICharsBytes.Length - 1 ); }
        public static byte[] RandomAsciiBytes( int minLen, int maxLen, byte[] source, int startindex, int maxindex ) { return RandomAsciiBytes( Random.Next( minLen, maxLen + 1 ), source, startindex, maxindex ); }
        public static byte[] RandomUtfUrlEncodeStringBytes( int minRealLen, int maxRealLen ) { return RandomUtfUrlEncodeStringBytes( Random.Next( minRealLen, maxRealLen ) ); }
        #endregion
        #region unsafe Wrappers
        #region Parsers
        public static unsafe long QLongParse( char[] input, int @from, int count ) {
            fixed ( char* cinput = input )
                return QLongParse( cinput + @from, count );
        }
        public static unsafe int QIntParse( char[] input, int @from, int count ) {
            fixed ( char* cinput = input )
                return QIntParse( cinput + @from, count );
        }
        #endregion
        #region to strings/bytes
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
        public static unsafe byte[] IntToDecStringBytes( int i ) {
            if ( i == 0 ) return Zba;
            var sz = GetDecStringLength( i );
            var output = new byte[ sz ];
            fixed ( byte* outArr = output )
                IntToDecStringBytesInsert( outArr, i, sz );
            return output;
        }
        #endregion
        #region random chars
        public static unsafe char[] RandomUtfUrlEncodeString( int len ) {
            var output = new char[ len ];
            fixed ( char* outPointer = output )
                RandomUtfUrlEncodeStringInsert( outPointer, len );
            return output;
        }
        public static unsafe char[] RandomAscii( int len, char[] source, int startindex, int maxindex ) {
            var output = new char[ len ];
            fixed ( char* outPointer = output )
            fixed ( char* csource = source )
                RandomAsciiInsert( outPointer, len, csource + startindex, maxindex - startindex );
            return output;
        }
        public static unsafe byte[] RandomUtfUrlEncodeStringBytes( int realLen ) {
            var output = new byte[ realLen ];
            fixed ( byte* outPointer = output )
                RandomUTFURLEncodeStringBytesInsert( outPointer, realLen );
            return output;
        }
        public static unsafe byte[] RandomAsciiBytes( int len, byte[] sourceArr, int startindex, int maxindex ) {
            var output = new byte[ len ];
            fixed ( byte* outPointer = output )
            fixed ( byte* csource = sourceArr )
                RandomAsciiBytesInsert( outPointer, len, csource + startindex, maxindex - startindex );
            return output;
        }
        #endregion
        #endregion
        #region Real Generators
        /*parsers*/
        public static unsafe int QIntParse( char* input, int count ) {
            var sum = 0; //, __cnt = 0;
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
            long sum = 0; //, __cnt = 0;
            var end = input + count;
            var pos = true;
            if ( *input == '-' ) {
                pos = false;
                input++;
            }
            while ( input < end ) {
                sum *= 10;
                sum += *input++ - 48;
            }
            return pos ? sum : -sum;
        }
        public static unsafe int FindChar( char* start, char* end, char c ) {
            var cnt = 0;
            while ( start < end && *start != c ) {
                ++cnt;
                ++start;
            }
            return start < end ? cnt : -1;
        }
        /*generators with pointers*/
        public static unsafe void RandomUTFURLEncodeStringBytesInsert( byte* outarr, int realLen ) {
            var end = ( outarr + realLen * 6 );
            ushort rnd;
            const byte pc = (byte) '%';
            uint bfr = 0;
            byte havenums = 0;
            fixed ( byte* hexChars = HexCharsBytes )
                while ( outarr < end ) {
                    //using temp uint to prevent unnecessary random generating
                    rnd = (ushort) bfr;
                    if ( havenums-- == 0 ) {
                        bfr = Random.NextUInt();
                        havenums = 1;
                        rnd = (ushort) ( bfr >> 16 );
                    }
                    //indexing to use out-of-order optimizations
                    *( outarr + 1 ) = pc;
                    *( outarr + 2 ) = *( hexChars + ( rnd & 0xf ) );
                    *( outarr + 3 ) = *( hexChars + ( ( rnd >> 4 ) & 0xf ) );
                    *( outarr + 4 ) = pc;
                    *( outarr + 5 ) = *( hexChars + ( ( rnd >> 8 ) & 0xf ) );
                    *( outarr + 6 ) = *( hexChars + ( ( rnd >> 12 ) & 0xf ) );
                    outarr += 6;
                }
        }
        public static unsafe void RandomAsciiBytesInsert( byte* outarr, int len, byte* source, int maxindex ) {
            maxindex++;
            var end = outarr + len;
            while ( outarr < end ) *outarr++ = *( source + Random.Next( maxindex ) );
        }
        public static unsafe void RandomUtfUrlEncodeStringInsert( char* outarr, int realLen ) {
            var end = ( outarr + realLen * 6 );
            const char pc = '%';
            uint bfr = 0;
            byte havenums = 0;
            fixed ( char* hexChars = HexChars )
                while ( outarr < end ) {
                    //using temp uint to prevent unnecessary random generating
                    ushort rnd = (ushort) bfr;
                    if ( havenums-- == 0 ) {
                        bfr = Random.NextUInt();
                        havenums = 1;
                        rnd = (ushort) ( bfr >> 16 );
                    }
                    //indexing to use out-of-order optimizations
                    *( outarr + 1 ) = pc;
                    *( outarr + 2 ) = *( hexChars + ( rnd & 0xf ) );
                    *( outarr + 3 ) = *( hexChars + ( ( rnd >> 4 ) & 0xf ) );
                    *( outarr + 4 ) = pc;
                    *( outarr + 5 ) = *( hexChars + ( ( rnd >> 8 ) & 0xf ) );
                    *( outarr + 6 ) = *( hexChars + ( ( rnd >> 12 ) & 0xf ) );
                    outarr += 6;
                }
        }
        public static unsafe void RandomAsciiInsert( char* outarr, int writeLength, char* sourceArr, int maxSourceIndex ) {
            maxSourceIndex++;
            var end = ( outarr + writeLength );
            while ( outarr < end ) *outarr++ = *( sourceArr + Random.Next( maxSourceIndex ) );
        }
        /*get_to_string_size*/
        public static byte GetDecStringLength( int value ) {
            byte sz = 1;
            if ( value < 0 ) {
                sz++;
                value = -value;
            }
            while ( ( value /= 10 ) > 0 ) sz++;
            return sz;
        }
        public static byte GetDecStringLength( long value ) {
            byte sz = 1;
            if ( value < 0 ) {
                sz++;
                value = -value;
            }
            while ( ( value /= 10 ) > 0 ) ++sz;
            return sz;
        }
        public static byte GetHexStringLength( int value ) {
            byte sz = 1;
            if ( value < 0 ) {
                sz++;
                value = -value;
            }
            while ( ( value >>= 4 ) != 0 ) ++sz;
            return sz;
        }
        public static byte GetHexStringLength( long value ) {
            byte sz = 1;
            if ( value < 0 ) {
                sz++;
                value = -value;
            }
            while ( ( value >>= 4 ) > 0 ) sz++;
            return sz;
        }
        /*to_strings | bytes*/
        public static unsafe void IntToHexStringBytesInsert( byte* outarr, byte* sourceArr, int value, byte writeLength ) {
            if ( value < 0 ) value = -value;
            *outarr = B;
            outarr += writeLength;
            do *--outarr = *( sourceArr + ( value & 0x0f ) ); while ( ( value >>= 4 ) != 0 );
        }
        public static unsafe void IntToHexStringBytesInsert( byte* outArr, byte* sourceArr, long value, byte writeLength ) {
            if ( value < 0 ) value = -value;
            *outArr = B;
            outArr += writeLength;
            do *--outArr = *( sourceArr + ( value & 0x0f ) ); while ( ( value >>= 4 ) != 0 );
        }
        public static unsafe void IntToDecStringBytesInsert( byte* outArr, long value, byte writeLength ) {
            if ( value < 0 ) value = -value;
            *outArr = B;
            outArr += writeLength;
            do *--outArr = (byte) ( value % 10 + 48 ); while ( ( value /= 10 ) != 0 );
        }
        public static unsafe void IntToDecStringBytesInsert( byte* outArr, int value, byte writeLength ) {
            if ( value < 0 ) value = -value;
            *outArr = B;
            outArr += writeLength;
            do *--outArr = (byte) ( value % 10 + 48 ); while ( ( value /= 10 ) != 0 );
        }
        public static unsafe void IntToHexStringInsert( char* outArr, char* sourceArr, long value, byte writeLength ) {
            if ( value < 0 ) value = -value;
            *outArr = C;
            outArr += writeLength;
            do *--outArr = *( sourceArr + ( value & 0x0f ) ); while ( ( value >>= 4 ) != 0 );
        }
        public static unsafe void IntToHexStringInsert( char* outArr, char* sourceArr, int value, byte writeLength ) {
            if ( value < 0 ) value = -value;
            *outArr = C;
            outArr += writeLength;
            do *--outArr = *( sourceArr + ( value & 0x0f ) ); while ( ( value >>= 4 ) != 0 );
        }
        public static unsafe void IntToDecStringInsert( char* outarr, long value, byte writeLength ) {
            if ( value < 0 ) value = -value;
            *outarr = C;
            outarr += writeLength;
            do *--outarr = (char) ( value % 10 + 48 ); while ( ( value /= 10 ) != 0 );
        }
        public static unsafe void IntToDecStringInsert( char* outarr, int value, byte writeLength ) {
            if ( value < 0 ) value = -value;
            *outarr = C;
            outarr += writeLength;
            do *--outarr = (char) ( value % 10 + 48 ); while ( ( value /= 10 ) != 0 );
        }
        #endregion
    }
}