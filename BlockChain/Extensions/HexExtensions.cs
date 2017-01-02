using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BlockChain.Extensions {

    public static class HexExtensions {

        static HexExtensions() {
            HexValues = new string[256];

            for (var i = 0; i < 256; i++)
                HexValues[i] = i.ToString("x2", CultureInfo.InvariantCulture);
        }

        static readonly string[] HexValues;

        public static string ToHex(this byte[] value) {

            var builder = new StringBuilder(value.Length << 1);

            foreach (var @char in value)
                builder.Append(HexValues[@char]);

            return builder.ToString();
        }

        public static string ToHexString(this string str) {
            var sb = new StringBuilder();

            var bytes = Encoding.Unicode.GetBytes(str);
            foreach (var t in bytes) {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString(); // returns: "48656C6C6F20776F726C64" for "Hello world"
        }
        /*
        public static byte[] ToByteArray(this string hexString) {
            var bytes = new byte[hexString.Length / 2];

            for (var i = 0; i < bytes.Length; i++) {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return bytes;
        }
        */
        public static byte[] ToByteArray(this string hex) {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}
