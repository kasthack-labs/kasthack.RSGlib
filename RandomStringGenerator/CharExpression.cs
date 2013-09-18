﻿using System;
using System.Text;
namespace RandomStringGenerator {
	public class CharExpression : IExpression {
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
		public CharExpression() {
		}
		public byte[] GetAsciiBytes() {
			return new byte[] { (byte)Functions.random.Next( _Min, _Max ) };
		}
		public char[] GetChars() {
			return new char[] { (char)Functions.random.Next( _Min, _Max ) };
		}
		public byte[] GetEncodingBytes( Encoding enc ) {
			return enc.GetBytes( new char[] { (char)Functions.random.Next( _Min, _Max ) } );
		}
		public string GetString() {
			return ( (char)Functions.random.Next( _Min, _Max ) ).ToString();
		}
		public override string ToString() {
			return GetString();
		}
		public System.Collections.Generic.IEnumerable<byte[]> EnumAsciiBuffers() {
			return new byte[][] { GetAsciiBytes() };
		}
		public System.Collections.Generic.IEnumerable<string> EnumStrings() {
			return new string[] { GetString() };
		}
		public unsafe void ComputeStringLength( ref int* outputdata ) {
			*outputdata++ = 1;
		}
		public int ComputeMaxLenForSize() {
			return 1;
		}
		public unsafe void GetAsciiBytesInsert( ref int* _Size, ref byte* _OutputBuffer ) {
			*_OutputBuffer++ = (byte)Functions.random.Next( _Min, _Max );
			_Size++;
		}
		public unsafe void GetAsciiInsert( ref int* _Size, ref char* _OutputBuffer ) {
			*_OutputBuffer++ = (char)Functions.random.Next( _Min, _Max );
			_Size++;
		}
	}
}
