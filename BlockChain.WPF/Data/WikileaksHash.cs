using System.Security.Cryptography;
using BlockChain.Extensions;

namespace BlockChain.WPF.Data {
    public class WikileaksHash {

        static readonly RIPEMD160 Hasher = RIPEMD160.Create();

        public WikileaksHash(string hash, string description, string transaction = null){

            Hash = hash;
            Description = description;
            Transaction = transaction;

            var data = hash.ToByteArray();

            RipeMd160 = Hasher.ComputeHash(data, 0, data.Length);
        }

        public string Description { get; }
        public string Hash { get; }
        public string Transaction { get; set; }
        public byte[] RipeMd160 { get; }
    }
}
