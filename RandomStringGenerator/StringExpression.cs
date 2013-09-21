﻿// RandomStringGenerator / RandomStringGenerator / StringExpression.cs
// Written by kasthack
// ( 2013.09.21 )
using System;
using System.Text;
namespace RandomStringGenerator
{
    public class StringExpression : IExpression
    {
        public StringFormat Format;
        private int _max;
        private int _min;
        [System.Diagnostics.DebuggerNonUserCode]
        public StringExpression() { }
        public int Min { private get { return this._min; } set { this._min = value + 1; } }
        public int Max { get { return this._max; } set { this._max = value + 1; } }
        public byte[] GetAsciiBytes() {
            switch ( Format ) {
                case StringFormat.Decimal:
                    return Generators.RandomASCIIBytes(Min, Max, Generators.HexCharsBytes, 0, 9);
                case StringFormat.Hexadecimal:
                    return Generators.RandomASCIIBytes(Min, Max, Generators.HexCharsBytes, 0, 15);
                case StringFormat.Letters:
                    return Generators.RandomASCIIBytes(Min, Max, Generators.ASCIICharsBytes, 10, 61);
                case StringFormat.LowerCase:
                    return Generators.RandomASCIIBytes(Min, Max, Generators.ASCIICharsBytes, 10, 35);
                case StringFormat.Random:
                    return Generators.RandomASCIIBytes(Min, Max);
                case StringFormat.Std:
                    return Generators.RandomASCIIBytes(Min, Max, Generators.ASCIICharsBytes, 0, 61);
                case StringFormat.UpperCase:
                    return Generators.RandomASCIIBytes(Min, Max, Generators.ASCIICharsBytes, 36, 61);
                case StringFormat.Urlencode:
                    return Generators.RandomUTFURLEncodeStringBytes(Min, Max);
                default:
                    throw new ArgumentException("Bad string format");
            }
        }
        public byte[] GetEncodingBytes(Encoding enc) {
            byte[] output;
            switch ( Format ) {
                case StringFormat.Decimal:
                    output = enc.GetBytes(Generators.RandomASCII(Min, Max, Generators.HexChars, 0, 9));
                    break;
                case StringFormat.Hexadecimal:
                    output = enc.GetBytes(Generators.RandomASCII(Min, Max, Generators.HexChars, 0, 15));
                    break;
                case StringFormat.Letters:
                    output = enc.GetBytes(Generators.RandomASCII(Min, Max, Generators.ASCIIChars, 10, 61));
                    break;
                case StringFormat.LowerCase:
                    output = enc.GetBytes(Generators.RandomASCII(Min, Max, Generators.ASCIIChars, 10, 35));
                    break;
                case StringFormat.Random:
                    output = enc.GetBytes(Generators.RandomASCII(Min, Max));
                    break;
                case StringFormat.Std:
                    output = enc.GetBytes(Generators.RandomASCII(Min, Max, Generators.ASCIIChars, 0, 61));
                    break;
                case StringFormat.UpperCase:
                    output = enc.GetBytes(Generators.RandomASCII(Min, Max, Generators.ASCIIChars, 36, 61));
                    break;
                case StringFormat.Urlencode:
                    output = enc.GetBytes(Generators.RandomUTFURLEncodeString(Min, Max));
                    break;
                default:
                    throw new ArgumentException("Bad string format");
            }
            return output;
        }
        public char[] GetChars() {
            switch ( Format ) {
                case StringFormat.Decimal:
                    return Generators.RandomASCII(Min, Max, Generators.HexChars, 0, 9);
                case StringFormat.Hexadecimal:
                    return Generators.RandomASCII(Min, Max, Generators.HexChars, 0, 15);
                case StringFormat.Letters:
                    return Generators.RandomASCII(Min, Max, Generators.ASCIIChars, 10, 61);
                case StringFormat.LowerCase:
                    return Generators.RandomASCII(Min, Max, Generators.ASCIIChars, 10, 35);
                case StringFormat.Random:
                    return Generators.RandomASCII(Min, Max);
                case StringFormat.Std:
                    return Generators.RandomASCII(Min, Max, Generators.ASCIIChars, 0, 61);
                case StringFormat.UpperCase:
                    return Generators.RandomASCII(Min, Max, Generators.ASCIIChars, 36, 61);
                case StringFormat.Urlencode:
                    return Generators.RandomUTFURLEncodeString(Min, Max);
                default:
                    throw new ArgumentException("Bad string format");
            }
        }
        public string GetString() {
            switch ( Format ) {
                case StringFormat.Decimal:
                    return new string(Generators.RandomASCII(Min, Max, Generators.HexChars, 0, 9));
                case StringFormat.Hexadecimal:
                    return new string(Generators.RandomASCII(Min, Max, Generators.HexChars, 0, 15));
                case StringFormat.Letters:
                    return new string(Generators.RandomASCII(Min, Max, Generators.ASCIIChars, 10, 61));
                case StringFormat.LowerCase:
                    return new string(Generators.RandomASCII(Min, Max, Generators.ASCIIChars, 10, 35));
                case StringFormat.Random:
                    return new string(Generators.RandomASCII(Min, Max));
                case StringFormat.Std:
                    return new string(Generators.RandomASCII(Min, Max, Generators.ASCIIChars, 0, 61));
                case StringFormat.UpperCase:
                    return new string(Generators.RandomASCII(Min, Max, Generators.ASCIIChars, 36, 61));
                case StringFormat.Urlencode:
                    return new string(Generators.RandomUTFURLEncodeString(Min, Max));
                default:
                    throw new ArgumentException("Bad string format");
            }
        }
        public System.Collections.Generic.IEnumerable<byte[]> EnumAsciiBuffers() { return new[] {GetAsciiBytes()}; }
        public System.Collections.Generic.IEnumerable<string> EnumStrings() { return new string[] {GetString()}; }
        public unsafe void ComputeStringLength(ref int* outputdata) { *outputdata++ = Generators.Random.Next(this._min, this._max) * ( Format == StringFormat.Urlencode ? 6 : 1 ); }
        public int ComputeMaxLenForSize() { return 1; }
        public unsafe void GetAsciiBytesInsert(ref int* size, ref byte* outputBuffer) {
            int len = *size++;
            fixed ( byte* chars = Generators.ASCIICharsBytes )
                switch ( Format ) {
                    case StringFormat.Decimal:
                        Generators.RandomASCIIBytesInsert(outputBuffer, len, chars, 9);
                        break;
                    case StringFormat.Hexadecimal:
                        Generators.RandomASCIIBytesInsert(outputBuffer, len, chars, 15);
                        break;
                    case StringFormat.Letters:
                        Generators.RandomASCIIBytesInsert(outputBuffer, len, chars + 10, 51);
                        break;
                    case StringFormat.LowerCase:
                        Generators.RandomASCIIBytesInsert(outputBuffer, len, chars + 10, 25);
                        break;
                    case StringFormat.Random:
                        Generators.RandomASCIIBytesInsert(outputBuffer, len, chars, 93);
                        break;
                    case StringFormat.Std:
                        Generators.RandomASCIIBytesInsert(outputBuffer, len, chars, 61);
                        break;
                    case StringFormat.UpperCase:
                        Generators.RandomASCIIBytesInsert(outputBuffer, len, chars + 36, 25);
                        break;
                    case StringFormat.Urlencode:
                        Generators.RandomUTFURLEncodeStringBytesInsert(outputBuffer, len / 6);
                        break;
                    default:
                        throw new ArgumentException("Bad string format");
                }
            outputBuffer += len;
        }
        public unsafe void GetAsciiInsert(ref int* size, ref char* outputBuffer) {
            int len = *size++;
            fixed ( char* chars = Generators.ASCIIChars )
                switch ( Format ) {
                    case StringFormat.Decimal:
                        Generators.RandomASCIIInsert(outputBuffer, len, chars, 9);
                        break;
                    case StringFormat.Hexadecimal:
                        Generators.RandomASCIIInsert(outputBuffer, len, chars, 15);
                        break;
                    case StringFormat.Letters:
                        Generators.RandomASCIIInsert(outputBuffer, len, chars + 10, 51);
                        break;
                    case StringFormat.LowerCase:
                        Generators.RandomASCIIInsert(outputBuffer, len, chars + 10, 25);
                        break;
                    case StringFormat.Random:
                        Generators.RandomASCIIInsert(outputBuffer, len, chars, 93);
                        break;
                    case StringFormat.Std:
                        Generators.RandomASCIIInsert(outputBuffer, len, chars, 61);
                        break;
                    case StringFormat.UpperCase:
                        Generators.RandomASCIIInsert(outputBuffer, len, chars + 36, 25);
                        break;
                    case StringFormat.Urlencode:
                        Generators.RandomUTFURLEncodeStringInsert(outputBuffer, len / 6);
                        break;
                    default:
                        throw new ArgumentException("Bad string format");
                }
            outputBuffer += len;
        }
        public override string ToString() { return GetString(); }
    }
}