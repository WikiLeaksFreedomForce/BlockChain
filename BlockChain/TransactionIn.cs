using System.Globalization;
using BlockChain.Extensions;

namespace BlockChain {

    public class TransactionIn {

        public OutPoint PreviousOutput { get; }
        public SignatureScript Script { get; }
        public uint Sequence { get; }
        public Transaction Transaction { get; set; }
        public TransactionOut TxOut { get; set; }

        public TransactionIn(byte[] buffer, ref int index, Transaction transaction){
            PreviousOutput = buffer.ToOutPoint(ref index);
            Script = buffer.ToSignatureScript(ref index);
            Sequence = buffer.ToUInt32(ref index);
            Transaction = transaction;
        }

        public override string ToString() {

            if (TxOut != null){
                return $"TxIn: {TxOut.Script.Address}";
            }

            if (Sequence == 0xFFFFFFFF)
                return "TxIn: (none)";

            return $"TxIn: {Sequence}";
        }
    }
}