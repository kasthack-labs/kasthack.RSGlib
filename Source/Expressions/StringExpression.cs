// RandomStringGenerator / RandomStringGenerator / StringExpression.cs
// Written by kasthack
// ( 2013.09.21 )

using System;
using System.Text;
using RandomStringGenerator.Helpers;

namespace RandomStringGenerator.Expressions {
    [Serializable]
    public class StringExpression : IExpression {
        public StringFormat Format;
        private int _max;
        private int _min;
        [System.Diagnostics.DebuggerNonUserCode]
        public StringExpression() { }
        public int Min { private get { return this._min; } set { this._min = value; } }
        public int Max { get { return this._max - 1; } set { this._max = value + 1; } }
        public byte[] GetAsciiBytes() {
            switch ( this.Format ) {
                case StringFormat.Decimal:
                    return Generators.RandomAsciiBytes( this.Min, this.Max, Generators.HexCharsBytes, 0, 9 );
                case StringFormat.Hexadecimal:
                    return Generators.RandomAsciiBytes( this.Min, this.Max, Generators.HexCharsBytes, 0, 15 );
                case StringFormat.Letters:
                    return Generators.RandomAsciiBytes( this.Min, this.Max, Generators.ASCIICharsBytes, 10, 61 );
                case StringFormat.LowerCase:
                    return Generators.RandomAsciiBytes( this.Min, this.Max, Generators.ASCIICharsBytes, 10, 35 );
                case StringFormat.Random:
                    return Generators.RandomAsciiBytes( this.Min, this.Max );
                case StringFormat.Std:
                    return Generators.RandomAsciiBytes( this.Min, this.Max, Generators.ASCIICharsBytes, 0, 61 );
                case StringFormat.UpperCase:
                    return Generators.RandomAsciiBytes( this.Min, this.Max, Generators.ASCIICharsBytes, 36, 61 );
                case StringFormat.Urlencode:
                    return Generators.RandomUtfUrlEncodeStringBytes( this.Min, this.Max );
                default:
                    throw new ArgumentException( "Bad string format" );
            }
        }
        public byte[] GetEncodingBytes( Encoding enc ) {
            byte[] output;
            switch ( this.Format ) {
                case StringFormat.Decimal:
                    output = enc.GetBytes( Generators.RandomAscii( this.Min, this.Max, Generators.HexChars, 0, 9 ) );
                    break;
                case StringFormat.Hexadecimal:
                    output = enc.GetBytes( Generators.RandomAscii( this.Min, this.Max, Generators.HexChars, 0, 15 ) );
                    break;
                case StringFormat.Letters:
                    output = enc.GetBytes( Generators.RandomAscii( this.Min, this.Max, Generators.ASCIIChars, 10, 61 ) );
                    break;
                case StringFormat.LowerCase:
                    output = enc.GetBytes( Generators.RandomAscii( this.Min, this.Max, Generators.ASCIIChars, 10, 35 ) );
                    break;
                case StringFormat.Random:
                    output = enc.GetBytes( Generators.RandomAscii( this.Min, this.Max ) );
                    break;
                case StringFormat.Std:
                    output = enc.GetBytes( Generators.RandomAscii( this.Min, this.Max, Generators.ASCIIChars, 0, 61 ) );
                    break;
                case StringFormat.UpperCase:
                    output = enc.GetBytes( Generators.RandomAscii( this.Min, this.Max, Generators.ASCIIChars, 36, 61 ) );
                    break;
                case StringFormat.Urlencode:
                    output = enc.GetBytes( Generators.RandomUtfUrlEncodeString( this.Min, this.Max ) );
                    break;
                default:
                    throw new ArgumentException( "Bad string format" );
            }
            return output;
        }
        public char[] GetChars() {
            switch ( this.Format ) {
                case StringFormat.Decimal:
                    return Generators.RandomAscii( this.Min, this.Max, Generators.HexChars, 0, 9 );
                case StringFormat.Hexadecimal:
                    return Generators.RandomAscii( this.Min, this.Max, Generators.HexChars, 0, 15 );
                case StringFormat.Letters:
                    return Generators.RandomAscii( this.Min, this.Max, Generators.ASCIIChars, 10, 61 );
                case StringFormat.LowerCase:
                    return Generators.RandomAscii( this.Min, this.Max, Generators.ASCIIChars, 10, 35 );
                case StringFormat.Random:
                    return Generators.RandomAscii( this.Min, this.Max );
                case StringFormat.Std:
                    return Generators.RandomAscii( this.Min, this.Max, Generators.ASCIIChars, 0, 61 );
                case StringFormat.UpperCase:
                    return Generators.RandomAscii( this.Min, this.Max, Generators.ASCIIChars, 36, 61 );
                case StringFormat.Urlencode:
                    return Generators.RandomUtfUrlEncodeString( this.Min, this.Max );
                default:
                    throw new ArgumentException( "Bad string format" );
            }
        }
        public unsafe void GetInsertLength( ref int* outputdata ) { *outputdata++ = Generators.Random.Next( this._min, this._max ) * ( this.Format == StringFormat.Urlencode ? 6 : 1 ); }
        public int ComputeLengthDataSize() { return 1; }
        public unsafe void InsertAsciiBytes( ref int* sizeData, ref byte* outputBuffer ) {
            var len = *sizeData++;
            fixed ( byte* chars = Generators.ASCIICharsBytes )
                switch ( this.Format ) {
                    case StringFormat.Decimal:
                        Generators.RandomAsciiBytesInsert( outputBuffer, len, chars, 9 );
                        break;
                    case StringFormat.Hexadecimal:
                        Generators.RandomAsciiBytesInsert( outputBuffer, len, chars, 15 );
                        break;
                    case StringFormat.Letters:
                        Generators.RandomAsciiBytesInsert( outputBuffer, len, chars + 10, 51 );
                        break;
                    case StringFormat.LowerCase:
                        Generators.RandomAsciiBytesInsert( outputBuffer, len, chars + 10, 25 );
                        break;
                    case StringFormat.Random:
                        Generators.RandomAsciiBytesInsert( outputBuffer, len, chars, 93 );
                        break;
                    case StringFormat.Std:
                        Generators.RandomAsciiBytesInsert( outputBuffer, len, chars, 61 );
                        break;
                    case StringFormat.UpperCase:
                        Generators.RandomAsciiBytesInsert( outputBuffer, len, chars + 36, 25 );
                        break;
                    case StringFormat.Urlencode:
                        Generators.RandomUTFURLEncodeStringBytesInsert( outputBuffer, len / 6 );
                        break;
                    default:
                        throw new ArgumentException( "Bad string format" );
                }
            outputBuffer += len;
        }
        public unsafe void InsertAsciiChars( ref int* sizeData, ref char* outputBuffer ) {
            var len = *sizeData++;
            fixed ( char* chars = Generators.ASCIIChars )
                switch ( this.Format ) {
                    case StringFormat.Decimal:
                        Generators.RandomAsciiInsert( outputBuffer, len, chars, 9 );
                        break;
                    case StringFormat.Hexadecimal:
                        Generators.RandomAsciiInsert( outputBuffer, len, chars, 15 );
                        break;
                    case StringFormat.Letters:
                        Generators.RandomAsciiInsert( outputBuffer, len, chars + 10, 51 );
                        break;
                    case StringFormat.LowerCase:
                        Generators.RandomAsciiInsert( outputBuffer, len, chars + 10, 25 );
                        break;
                    case StringFormat.Random:
                        Generators.RandomAsciiInsert( outputBuffer, len, chars, 93 );
                        break;
                    case StringFormat.Std:
                        Generators.RandomAsciiInsert( outputBuffer, len, chars, 61 );
                        break;
                    case StringFormat.UpperCase:
                        Generators.RandomAsciiInsert( outputBuffer, len, chars + 36, 25 );
                        break;
                    case StringFormat.Urlencode:
                        Generators.RandomUtfUrlEncodeStringInsert( outputBuffer, len / 6 );
                        break;
                    default:
                        throw new ArgumentException( "Bad string format" );
                }
            outputBuffer += len;
        }
        public string Decompile() {
            char c;
            switch ( this.Format ) {
                case StringFormat.Decimal:
                    c = 'D';
                    break;
                case StringFormat.Hexadecimal:
                    c = 'H';
                    break;
                case StringFormat.Letters:
                    c = 'L';
                    break;
                case StringFormat.LowerCase:
                    c = 'a';
                    break;
                case StringFormat.Random:
                    c = 'R';
                    break;
                case StringFormat.Std:
                    c = 'S';
                    break;
                case StringFormat.UpperCase:
                    c = 'A';
                    break;
                case StringFormat.Urlencode:
                    c = 'U';
                    break;
                default:
                    throw new ArgumentException( "Bad string format" );
            }
            return String.Format( @"{0}S:{2}:{3}:{4}{1}", @"{", @"}", c, this.Min, this.Max );
        }

        public override string ToString() {
            switch ( this.Format ) {
                case StringFormat.Decimal:
                    return new string( Generators.RandomAscii( this.Min, this.Max, Generators.HexChars, 0, 9 ) );
                case StringFormat.Hexadecimal:
                    return new string( Generators.RandomAscii( this.Min, this.Max, Generators.HexChars, 0, 15 ) );
                case StringFormat.Letters:
                    return new string( Generators.RandomAscii( this.Min, this.Max, Generators.ASCIIChars, 10, 61 ) );
                case StringFormat.LowerCase:
                    return new string( Generators.RandomAscii( this.Min, this.Max, Generators.ASCIIChars, 10, 35 ) );
                case StringFormat.Random:
                    return new string( Generators.RandomAscii( this.Min, this.Max ) );
                case StringFormat.Std:
                    return new string( Generators.RandomAscii( this.Min, this.Max, Generators.ASCIIChars, 0, 61 ) );
                case StringFormat.UpperCase:
                    return new string( Generators.RandomAscii( this.Min, this.Max, Generators.ASCIIChars, 36, 61 ) );
                case StringFormat.Urlencode:
                    return new string( Generators.RandomUtfUrlEncodeString( this.Min, this.Max ) );
                default:
                    throw new ArgumentException( "Bad string format" );
            }
        }
    }
}