using System;
using System.Security.Cryptography;

namespace BlockChain.Extensions {

    public static class StringExtensions{

        public static string ToInnerBytes(this string text){

            var bytes = text.ToByteArray();

            return bytes.ToInnerBytes().ToHex();
        }
    }
}
