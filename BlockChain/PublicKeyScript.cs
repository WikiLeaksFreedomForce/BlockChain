using System.Collections.Generic;
using System.Linq;
using BlockChain.Extensions;

namespace BlockChain
{
    public class PublicKeyScript : ByteArray {

        public PublicKeyScript(byte[] buffer) : base(buffer){
            
        }

        public PublicKeyScript(byte[] buffer, ref int index, int bytes) : base(buffer, ref index, bytes){
            
        }

        public string Address {
            get {
                var byteArray = new List<byte>();

                if (Raw.IsStandardTransactionToBitcoinAddress()){
                    byteArray.Add(0x00);
                }
                else if (Raw.IsOpHashEqual()){
                    byteArray.Add(0x05);
                }
                else{
                    return string.Empty;
                }

                byteArray.AddRange(Inner);
                byteArray.AddRange(Base58Encoding.GetCheckSum(byteArray.ToArray()));

                return Base58Encoding.Encode(byteArray.ToArray());
            }
        }

        public bool IsZero => Inner.All(x => x == 0x00);

        public bool IsOne
        {
            get
            {
                var inner = Inner;

                if(inner.Last() != 0x01)
                    return false;

                return inner.Take(Inner.Length - 1).All(x => x == 0x00);
            }
        }
    }
}