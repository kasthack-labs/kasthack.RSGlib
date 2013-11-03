﻿// RandomStringGenerator / RandomStringGenerator / CharExpression.cs
// Written by kasthack
// ( 2013.09.21 )
using System.Text;
namespace RandomStringGenerator
{
    public class CharExpression : IExpression
    {
        private int _max;
        private int _min;
        [System.Diagnostics.DebuggerNonUserCode]
        public CharExpression() { }
        public int Min { get { return this._min; } set { this._min = value + 1; } }
        public int Max { get { return this._max; } set { this._max = value + 1; } }
        public byte[] GetAsciiBytes() { return new[] {(byte) Generators.Random.Next(this._min, this._max)}; }
        public char[] GetChars() { return new[] {(char) Generators.Random.Next(this._min, this._max)}; }
        public byte[] GetEncodingBytes(Encoding enc) { return enc.GetBytes(new[] {(char) Generators.Random.Next(this._min, this._max)}); }
        public string GetString() { return ( (char) Generators.Random.Next(this._min, this._max) ).ToString(); }
        public System.Collections.Generic.IEnumerable<byte[]> EnumAsciiBuffers() { return new[] {GetAsciiBytes()}; }
        public System.Collections.Generic.IEnumerable<string> EnumStrings() { return new[] {GetString()}; }
        public unsafe void ComputeStringLength(ref int* outputdata) { *outputdata++ = 1; }
        public int ComputeMaxLenForSize() { return 1; }
        public unsafe void GetAsciiBytesInsert(ref int* size, ref byte* outputBuffer) {
            *outputBuffer++ = (byte) Generators.Random.Next(this._min, this._max);
            size++;
        }
        public unsafe void GetAsciiInsert(ref int* size, ref char* outputBuffer) {
            *outputBuffer++ = (char) Generators.Random.Next(this._min, this._max);
            size++;
        }
        public override string ToString() { return GetString(); }
    }
}