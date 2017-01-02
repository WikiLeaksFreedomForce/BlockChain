using System;
using System.Linq;
using System.Security.Cryptography;
using BlockChain.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlockChain.Test {
    [TestClass]
    public class AddressTest {

        [TestMethod]
        public void Address_1GhqLp4fmVH5yMp82rwcXSnsWV2E7kS1jN(){
            var script = new PublicKeyScript("76a914ac42e0f55f4f26cb4d350add34ae8f15aa95382b88ac".ToByteArray());

            Assert.AreEqual(script.Address, "1GhqLp4fmVH5yMp82rwcXSnsWV2E7kS1jN");

            // 8144F4B1
        }

        [TestMethod]
        public void Address_2() {
            var script = new PublicKeyScript("76a914ac42e0f55f4f26cb4d350add34ae8f15aa95382b88ac".ToByteArray());
            Assert.AreEqual(script.Address, "1GhqLp4fmVH5yMp82rwcXSnsWV2E7kS1jN");
        }

        [TestMethod]
        public void Address_3() {
            var script = new PublicKeyScript("76a914d5a4f0fa0032e67c74ab8d81acca1b29b3a0d52288ac".ToByteArray());
            Assert.AreEqual(script.Address, "1LUeXBdkWa6VmiTkLTtyB5SBQUvrCnS5ye");

            // D23448BD
        }
    }
}
