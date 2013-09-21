using System;
using System.Text;
namespace RandomStringGenerator
{
	public class StringExpression : IExpression
	{
		public StringFormat Format;
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
		public StringExpression() {
		}
		public byte[] GetAsciiBytes() {
			switch ( Format ) {
				case StringFormat.Decimal:
					return Generators.RandomASCIIBytes(Min, Max, Generators._hex_chars_bytes, 0, 9);
				case StringFormat.Hexadecimal:
					return Generators.RandomASCIIBytes(Min, Max, Generators._hex_chars_bytes, 0, 15);
				case StringFormat.Letters:
					return Generators.RandomASCIIBytes(Min, Max, Generators._ascii_chars_bytes, 10, 61);
				case StringFormat.LowerCase:
					return Generators.RandomASCIIBytes(Min, Max, Generators._ascii_chars_bytes, 10, 35);
				case StringFormat.Random:
					return Generators.RandomASCIIBytes(Min, Max);
				case StringFormat.Std:
					return Generators.RandomASCIIBytes(Min, Max, Generators._ascii_chars_bytes, 0, 61);
				case StringFormat.UpperCase:
					return Generators.RandomASCIIBytes(Min, Max, Generators._ascii_chars_bytes, 36, 61);
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
					output = enc.GetBytes(Generators.RandomASCII(Min, Max, Generators._hex_chars, 0, 9));
					break;
				case StringFormat.Hexadecimal:
					output = enc.GetBytes(Generators.RandomASCII(Min, Max, Generators._hex_chars, 0, 15));
					break;
				case StringFormat.Letters:
					output = enc.GetBytes(Generators.RandomASCII(Min, Max, Generators._ascii_chars, 10, 61));
					break;
				case StringFormat.LowerCase:
					output = enc.GetBytes(Generators.RandomASCII(Min, Max, Generators._ascii_chars, 10, 35));
					break;
				case StringFormat.Random:
					output = enc.GetBytes(Generators.RandomASCII(Min, Max));
					break;
				case StringFormat.Std:
					output = enc.GetBytes(Generators.RandomASCII(Min, Max, Generators._ascii_chars, 0, 61));
					break;
				case StringFormat.UpperCase:
					output = enc.GetBytes(Generators.RandomASCII(Min, Max, Generators._ascii_chars, 36, 61));
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
					return Generators.RandomASCII(Min, Max, Generators._hex_chars, 0, 9);
				case StringFormat.Hexadecimal:
					return Generators.RandomASCII(Min, Max, Generators._hex_chars, 0, 15);
				case StringFormat.Letters:
					return Generators.RandomASCII(Min, Max, Generators._ascii_chars, 10, 61);
				case StringFormat.LowerCase:
					return Generators.RandomASCII(Min, Max, Generators._ascii_chars, 10, 35);
				case StringFormat.Random:
					return Generators.RandomASCII(Min, Max);
				case StringFormat.Std:
					return Generators.RandomASCII(Min, Max, Generators._ascii_chars, 0, 61);
				case StringFormat.UpperCase:
					return Generators.RandomASCII(Min, Max, Generators._ascii_chars, 36, 61);
				case StringFormat.Urlencode:
					return Generators.RandomUTFURLEncodeString(Min, Max);
				default:
					throw new ArgumentException("Bad string format");
			}
		}
		public string GetString() {
			switch ( Format ) {
				case StringFormat.Decimal:
					return new string(Generators.RandomASCII(Min, Max, Generators._hex_chars, 0, 9));
				case StringFormat.Hexadecimal:
					return new string(Generators.RandomASCII(Min, Max, Generators._hex_chars, 0, 15));
				case StringFormat.Letters:
					return new string(Generators.RandomASCII(Min, Max, Generators._ascii_chars, 10, 61));
				case StringFormat.LowerCase:
					return new string(Generators.RandomASCII(Min, Max, Generators._ascii_chars, 10, 35));
				case StringFormat.Random:
					return new string(Generators.RandomASCII(Min, Max));
				case StringFormat.Std:
					return new string(Generators.RandomASCII(Min, Max, Generators._ascii_chars, 0, 61));
				case StringFormat.UpperCase:
					return new string(Generators.RandomASCII(Min, Max, Generators._ascii_chars, 36, 61));
				case StringFormat.Urlencode:
					return new string(Generators.RandomUTFURLEncodeString(Min, Max));
				default:
					throw new ArgumentException("Bad string format");
			}
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
		public unsafe void ComputeStringLength(ref int* outputdata) {
			*outputdata++ = Generators.random.Next(_Min, _Max) * ( Format == StringFormat.Urlencode ? 6 : 1 );
		}
		public int ComputeMaxLenForSize() {
			return 1;
		}
		public unsafe void GetAsciiBytesInsert(ref int* _Size, ref byte* _OutputBuffer) {
			int __len = *_Size++;
			fixed(byte* __chars = Generators._ascii_chars_bytes)
			switch ( Format ) {
				case StringFormat.Decimal:
					Generators.RandomASCIIBytesInsert(_OutputBuffer, __len, __chars, 9);
					break;
				case StringFormat.Hexadecimal:
					Generators.RandomASCIIBytesInsert(_OutputBuffer, __len, __chars, 15);
					break;
				case StringFormat.Letters:
					Generators.RandomASCIIBytesInsert(_OutputBuffer, __len, __chars+ 10, 51);
					break;
				case StringFormat.LowerCase:
					Generators.RandomASCIIBytesInsert(_OutputBuffer, __len, __chars+ 10, 25);
					break;
				case StringFormat.Random:
					Generators.RandomASCIIBytesInsert(_OutputBuffer, __len, __chars, 93);
					break;
				case StringFormat.Std:
					Generators.RandomASCIIBytesInsert(_OutputBuffer, __len, __chars, 61);
					break;
				case StringFormat.UpperCase:
					Generators.RandomASCIIBytesInsert(_OutputBuffer, __len, __chars+ 36, 25);
					break;
				case StringFormat.Urlencode:
					Generators.RandomUTFURLEncodeStringBytesInsert(_OutputBuffer, __len / 6);
					break;
				default:
					throw new ArgumentException("Bad string format");
			}
			_OutputBuffer += __len;
		}
		public unsafe void GetAsciiInsert(ref int* _Size, ref char* _OutputBuffer) {
			int __len = *_Size++;
			fixed ( char* __chars = Generators._ascii_chars )
				switch ( Format ) {
					case StringFormat.Decimal:
						Generators.RandomASCIIInsert(_OutputBuffer, __len, __chars, 9);
						break;
					case StringFormat.Hexadecimal:
						Generators.RandomASCIIInsert(_OutputBuffer, __len, __chars, 15);
						break;
					case StringFormat.Letters:
						Generators.RandomASCIIInsert(_OutputBuffer, __len, __chars + 10, 51);
						break;
					case StringFormat.LowerCase:
						Generators.RandomASCIIInsert(_OutputBuffer, __len, __chars + 10, 25);
						break;
					case StringFormat.Random:
						Generators.RandomASCIIInsert(_OutputBuffer, __len, __chars, 93);
						break;
					case StringFormat.Std:
						Generators.RandomASCIIInsert(_OutputBuffer, __len, __chars, 61);
						break;
					case StringFormat.UpperCase:
						Generators.RandomASCIIInsert(_OutputBuffer, __len, __chars + 36, 25);
						break;
					case StringFormat.Urlencode:
						Generators.RandomUTFURLEncodeStringInsert(_OutputBuffer, __len / 6);
						break;
					default:
						throw new ArgumentException("Bad string format");
				}
			_OutputBuffer += __len;
		}
	}
}
