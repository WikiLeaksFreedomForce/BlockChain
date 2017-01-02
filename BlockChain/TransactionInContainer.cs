using System.Collections.Generic;
using System.Linq;

namespace BlockChain {

    public class TransactionInContainer : List<TransactionIn>{
        public BitcoinValue Amount => BitcoinValue.FromSatoshis(this.Where(x => x.TxOut != null).Sum(o => o.TxOut.Value.Satoshis));

        public byte[] GetFileBytes() {

            var data = new List<byte>();

            foreach (var txIn in this) {
                data.AddRange(txIn.Script.Inner);
            }

            return data.ToArray();
        }
    }
}
