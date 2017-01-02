using System;
using System.IO;
using System.Security.Cryptography;
using BlockChain.Enums;

namespace BlockChain.Extensions {

    public static class ByteArrayExtensions{

        public static uint ToUInt32(this byte[] data, ref int index){
            var result = BitConverter.ToUInt32(data, index);
            index += 4;
            return result;
        }

        public static int ToInt32(this byte[] data, int index) {
            return BitConverter.ToInt32(data, index);
        }

        public static uint ToUInt32(this byte[] data, int index) {
            return BitConverter.ToUInt32(data, index);
        }

        public static ulong ToUInt64(this byte[] data, ref int index){
            var result = BitConverter.ToUInt64(data, index);
            index += 8;
            return result;
        }

        public static long ToInt64(this byte[] data, ref int index){
            var result = BitConverter.ToInt64(data, index);
            index += 8;
            return result;
        }

        public static OutPoint ToOutPoint(this byte[] data, ref int index){
            return new OutPoint(data, ref index);
        }

        public static BitcoinValue ToBitcoin(this byte[] data, ref int index){
            return new BitcoinValue((decimal) data.ToInt64(ref index)/BitcoinValue.SatoshisPerBitcoin);
        }

        public static SignatureScript ToSignatureScript(this byte[] data, ref int index){
            var scriptLength = data.ToVariableInt(ref index);
            return new SignatureScript(data, ref index, (int) scriptLength);
        }

        public static PublicKeyScript ToPublicKeyScript(this byte[] data, ref int index){
            var scriptLength = data.ToVariableInt(ref index);
            return new PublicKeyScript(data, ref index, (int) scriptLength);
        }

        public static ulong ToVariableInt(this byte[] data, ref int index){

            var value = (ulong) data[index++];

            switch (value){
                case 0xFD:
                    value = BitConverter.ToUInt16(data, index);
                    index += 2;
                    break;

                case 0xFE:
                    value = BitConverter.ToUInt32(data, index);
                    index += 4;
                    break;

                case 0xFF:
                    value = BitConverter.ToUInt64(data, index);
                    index += 8;
                    break;
            }

            return value;
        }


        public static uint ToLengthInt(this byte[] data, ref int index) {

            uint value;

            switch (data[index++]) {
                case Op.OP_PUSHDATA1:
                    value = data[index];
                    index += 1;
                    break;

                case Op.OP_PUSHDATA2:
                    value = BitConverter.ToUInt16(data, index);
                    index += 2;
                    break;

                case Op.OP_PUSHDATA4:
                    value = BitConverter.ToUInt32(data, index);
                    index += 4;
                    break;

                default:
                    throw new InvalidDataException("The length should be PUSHDATAx");
            }

            return value;
        }

        public static DateTime ToDateTime(this byte[] data, ref int index){
            var value = ToUInt32(data, ref index);
            return DateTime.SpecifyKind(new DateTime(1970, 1, 1) + TimeSpan.FromSeconds(value), DateTimeKind.Utc);
        }

        public static int? Search(this byte[] haystack, byte[] needle) {
            var len = needle.Length;
            var limit = haystack.Length - len;
            for (var i = 0; i <= limit; i++) {
                var k = 0;
                for (; k < len; k++) {
                    if (needle[k] != haystack[i + k]) break;
                }
                if (k == len) return i;
            }
            return null;
        }

        public static SHA256 Sha256 = SHA256.Create();

        public static Hash ToSha256_2(this byte[] data, ref int index, int length){
            var hash = Sha256.ComputeHash(Sha256.ComputeHash(data, index, length));

            return new Hash(hash);
        }

        public static Hash ToHash(this byte[] data, ref int index, int length){
            return new Hash(data, ref index, length);
        }
    }
}
