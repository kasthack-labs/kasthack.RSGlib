using System.Text;
using System;
using System.Runtime.InteropServices;
namespace RandomStringGenerator
{
    public class StaticASCIIStringExpression : IExpression
    {
        readonly ASCIIEncoding _enc;
        readonly byte[] _buf;
        public override string ToString() {
            return GetString();
        }
        public StaticASCIIStringExpression(string str, ASCIIEncoding enc = null) {
            _enc = enc ?? new ASCIIEncoding();
            this._buf = _enc.GetBytes(str);
        }
        public StaticASCIIStringExpression(char[] str, ASCIIEncoding enc = null) {
            _enc = enc ?? new ASCIIEncoding();
            this._buf = _enc.GetBytes(str);
        }
        public StaticASCIIStringExpression(byte[] str, ASCIIEncoding enc = null) {
            _enc = enc ?? new ASCIIEncoding();
            this._buf = str;
        }
        public string GetString() {
            return this._enc.GetString(this._buf);
        }
        public char[] GetChars() {
            return this._enc.GetChars(this._buf);
        }
        public byte[] GetAsciiBytes() {
            return this._buf;
        }
        public byte[] GetEncodingBytes(Encoding enc) {
            return enc.GetBytes(this._enc.GetChars(this._buf));
        }
        public System.Collections.Generic.IEnumerable<byte[]> EnumAsciiBuffers() {
            return new[] { GetAsciiBytes() };
        }
        public System.Collections.Generic.IEnumerable<string> EnumStrings() {
            return new[] { GetString() };
        }
        public unsafe void ComputeStringLength(ref int* outputdata) {
			*outputdata++ = this._buf.Length;
        }
        public int ComputeMaxLenForSize() {
            return 1;
        }
		public unsafe void GetAsciiBytesInsert(ref int* size, ref byte* outputBuffer) {
			var p = new IntPtr(outputBuffer);
			Marshal.Copy(this._buf, 0, p, *size);
			outputBuffer += *size++;
		}
		public unsafe void GetAsciiInsert(ref int* size, ref char* outputBuffer) {
			fixed ( byte* tmpbuf = this._buf ) {
				var start = tmpbuf;
				var end = tmpbuf + *size++;
				do
					*outputBuffer++ = (char)*start++;
				while ( start < end );
			}
		}
	}
}