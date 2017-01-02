using System.Collections.Generic;
using System.Linq;
using BlockChain.Extensions;

namespace BlockChain {

    public class TransactionOutContainer : List<TransactionOut> {

        public int Unique
        {
            get
            {
                var outs = new List<string>();

                foreach (var txOut in this){
                    var address = txOut.Script.Raw.ToHex();

                    if (outs.Contains(address))
                        continue;

                    outs.Add(address);
                }

                return outs.Count;
            }
        }

        public BitcoinValue Amount => BitcoinValue.FromSatoshis(this.Sum(o => o.Value.Satoshis));

        public int SpentCount => this.Count(txOut => txOut.TxIn != null);

        public int ValueCount => this.Count(txOut => txOut.Value.Btc > 0.00001m );

        public byte[] GetFileBytes() {

            var data = new List<byte>();

            foreach (var txOut in this) {
                data.AddRange(txOut.Script.Inner);
            }

            return data.ToArray();
        }
    }
}
