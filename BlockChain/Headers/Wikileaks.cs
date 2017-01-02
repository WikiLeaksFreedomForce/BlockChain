namespace BlockChain.Headers {

    class Wikileaks : IExtensions {
        public byte[] Header => new byte[]{ 0x57, 0x69, 0x6b, 0x69, 0x6c, 0x65, 0x61, 0x6b, 0x73 };
        public byte[] Footer => null;
        public string Extension => "wiki";
    }
}
