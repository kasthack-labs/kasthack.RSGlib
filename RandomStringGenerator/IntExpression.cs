﻿using System;
using System.Text;
namespace RandomStringGenerator
{
	public class IntExpression : IExpression
	{
		public NumberFormat Format;
		int _Min, _Max;
		public int Min {
			get {
				return _Min;
			}
			set {
				_Min = value + 1;
			}
		}
		public int Max {
			get {
				return _Max;
			}
			set {
				_Max = value + 1;
			}
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public IntExpression() {
		}
		/// <summary>
		/// Get string representation of expression execution result
		/// </summary>
		/// <returns>string result</returns>
		public string GetString() {
			return new string(Format == NumberFormat.Decimal ? Generators.IntToDecString(Generators.Random.Next(_Min, _Max)) :
			  Generators.IntToHexString(Generators.Random.Next(_Min, _Max)));
		}
		/// <summary>
		/// Get char array representation of expression execution result
		/// </summary>
		/// <returns>char[] result</returns>
		public char[] GetChars() {
			return Format == NumberFormat.Decimal ? Generators.IntToDecString(Generators.Random.Next(_Min, _Max)) :
			  Generators.IntToHexString(Generators.Random.Next(_Min, _Max));
		}
		/// <summary>
		/// Get native representation of expression execution result
		/// </summary>
		/// <returns>ascii bytes</returns>
		public byte[] GetAsciiBytes() {
			return Format == NumberFormat.Decimal ? Generators.IntToDecStringBytes(Generators.Random.Next(_Min, _Max)) :
			  Generators.IntToHexStringBytes(Generators.Random.Next(_Min, _Max));
		}
		/// <summary>
		/// Get bytes of result encoded with encoding
		/// </summary>
		/// <param name="_enc">encoding for encoding, lol</param>
		/// <returns>bytes</returns>
		public byte[] GetEncodingBytes(Encoding enc) {
			return enc.GetBytes(Format == NumberFormat.Decimal ? Generators.IntToDecString(Generators.Random.Next(_Min, _Max)) :
			  Generators.IntToHexString(Generators.Random.Next(_Min, _Max)));
		}
		/// <summary>
		/// alias 4 GetString. 4 debugging
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return GetString();
		}
		public System.Collections.Generic.IEnumerable<byte[]> EnumAsciiBuffers() {
			return new byte[][] { GetAsciiBytes() };
		}
		public System.Collections.Generic.IEnumerable<string> EnumStrings() {
			return new string[] { GetString() };
		}
		public unsafe void ComputeStringLength(ref int* _outputdata) {
			int __value = Generators.Random.Next(_Min, _Max);
			*_outputdata++ = __value;
			*_outputdata++ = Format == NumberFormat.Decimal ? Generators.GetDecStringLength(__value) : Generators.GetHexStringLength(__value);
			*_outputdata++ = -__value;
		}
		public int ComputeMaxLenForSize() {
			return 3;// 1 -cached value,  2 - __len,3 - cached value nuller,
			//bad idea but __i have nothin better
		}
		public unsafe void GetAsciiBytesInsert(ref int* _Size, ref byte* _OutputBuffer) {
			if ( Format == NumberFormat.Decimal ) {
				Generators.IntToDecStringBytesInsert(_OutputBuffer, *_Size++, (byte)*_Size++);
				_OutputBuffer -= *_Size++;
				return;
			}
			fixed (byte* __hex_pointer = Generators.HexCharsBytes)
				Generators.IntToHexStringBytesInsert(_OutputBuffer, __hex_pointer, *_Size++, (byte)*_Size++);
			_OutputBuffer -= *_Size++;
		}
		public unsafe void GetAsciiInsert(ref int* _Size, ref char* _OutputBuffer) {
			if ( Format == NumberFormat.Decimal ) {
				Generators.IntToDecStringInsert(_OutputBuffer, *_Size++, (byte)*_Size++);
				_OutputBuffer -= *_Size++;
				return;
			}
			fixed ( char* __hex_pointer = Generators.HexChars )
				Generators.IntToHexStringInsert(_OutputBuffer, __hex_pointer, *_Size++, (byte)*_Size++);
			_OutputBuffer -= *_Size++;
		}
	}
}
