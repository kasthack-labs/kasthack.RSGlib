using System.Text;
namespace RandomStringGenerator
{
	public class IntExpression : IExpression
	{
		public NumberFormat Format;
		int _min, _max;
		public int Min {
			get { return this._min;}
			set {this._min = value + 1;}
		}
		public int Max {
			get {return this._max;}
			set {this._max = value + 1;}
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public IntExpression() {
		}
		/// <summary>
		/// Get string representation of expression execution result
		/// </summary>
		/// <returns>string result</returns>
		public string GetString() {
			return new string(Format == NumberFormat.Decimal ? Generators.IntToDecString(Generators.Random.Next(this._min, this._max)) :
			  Generators.IntToHexString(Generators.Random.Next(this._min, this._max)));
		}
		/// <summary>
		/// Get char array representation of expression execution result
		/// </summary>
		/// <returns>char[] result</returns>
		public char[] GetChars() {
			return Format == NumberFormat.Decimal ? Generators.IntToDecString(Generators.Random.Next(this._min, this._max)) :
			  Generators.IntToHexString(Generators.Random.Next(this._min, this._max));
		}
		/// <summary>
		/// Get native representation of expression execution result
		/// </summary>
		/// <returns>ascii bytes</returns>
		public byte[] GetAsciiBytes() {
			return Format == NumberFormat.Decimal ? Generators.IntToDecStringBytes(Generators.Random.Next(this._min, this._max)) :
			  Generators.IntToHexStringBytes(Generators.Random.Next(this._min, this._max));
		}
		/// <summary>
		/// Get bytes of result encoded with encoding
		/// </summary>
		/// <param name="enc">encoding for encoding, lol</param>
		/// <returns>bytes</returns>
		public byte[] GetEncodingBytes(Encoding enc) {
			return enc.GetBytes(Format == NumberFormat.Decimal ? Generators.IntToDecString(Generators.Random.Next(this._min, this._max)) :
			  Generators.IntToHexString(Generators.Random.Next(this._min, this._max)));
		}
		/// <summary>
		/// alias 4 GetString. 4 debugging
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return GetString();
		}
		public System.Collections.Generic.IEnumerable<byte[]> EnumAsciiBuffers() {
			return new[] { GetAsciiBytes() };
		}
		public System.Collections.Generic.IEnumerable<string> EnumStrings() {
			return new[] { GetString() };
		}
		public unsafe void ComputeStringLength(ref int* outputdata) {
			var value = Generators.Random.Next(this._min, this._max);
			*outputdata++ = value;
			*outputdata++ = Format == NumberFormat.Decimal ? Generators.GetDecStringLength(value) : Generators.GetHexStringLength(value);
			*outputdata++ = -value;
		}
		public int ComputeMaxLenForSize() {
			return 3;// 1 -cached value,  2 - __len,3 - cached value nuller,
			//bad idea but __i have nothin better
		}
		public unsafe void GetAsciiBytesInsert(ref int* size, ref byte* outputBuffer) {
			if ( Format == NumberFormat.Decimal ) {
				Generators.IntToDecStringBytesInsert(outputBuffer, *size++, (byte)*size++);
				outputBuffer -= *size++;
				return;
			}
			fixed (byte* hexPointer = Generators.HexCharsBytes)
				Generators.IntToHexStringBytesInsert(outputBuffer, hexPointer, *size++, (byte)*size++);
			outputBuffer -= *size++;
		}
		public unsafe void GetAsciiInsert(ref int* size, ref char* outputBuffer) {
			if ( Format == NumberFormat.Decimal ) {
				Generators.IntToDecStringInsert(outputBuffer, *size++, (byte)*size++);
				outputBuffer -= *size++;
				return;
			}
			fixed ( char* hexPointer = Generators.HexChars )
				Generators.IntToHexStringInsert(outputBuffer, hexPointer, *size++, (byte)*size++);
			outputBuffer -= *size++;
		}
	}
}
