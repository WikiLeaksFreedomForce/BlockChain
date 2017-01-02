namespace BlockChain.Enums {

    // https://en.bitcoin.it/wiki/Script

    public static class Op{

        // Constants
        // When talking about scripts, these value-pushing words are usually omitted.

        public const byte OP_0 = 0x00;
        public const byte OP_FALSE = OP_0;
        public const byte OP_PUSHDATA1 = 0x4c;
        public const byte OP_PUSHDATA2 = 0x4d;
        public const byte OP_PUSHDATA4 = 0x4e;
        public const byte OP_1NEGATE = 0x4f;
        public const byte OP_1 = 0x51;
        public const byte OP_TRUE = OP_1;
        public const byte OP_2  = 0x52;
        public const byte OP_3  = 0x53;
        public const byte OP_4  = 0x54;
        public const byte OP_5  = 0x55;
        public const byte OP_6  = 0x56;
        public const byte OP_7  = 0x57;
        public const byte OP_8  = 0x58;
        public const byte OP_9  = 0x59;
        public const byte OP_10 = 0x5a;
        public const byte OP_11 = 0x5b;
        public const byte OP_12 = 0x5c;
        public const byte OP_13 = 0x5d;
        public const byte OP_14 = 0x5e;
        public const byte OP_15 = 0x5f;
        public const byte OP_16 = 0x60;

        public const byte OP_SEPARATOR = 0x41;

        // Flow control

        public const byte OP_NOP = 0x61;
        public const byte OP_IF = 0x63;
        public const byte OP_NOTIF = 0x64;
        public const byte OP_ELSE = 0x67;
        public const byte OP_ENDIF = 0x68;
        public const byte OP_VERIFY = 0x69;
        public const byte OP_RETURN = 0x6a;

        // Stack

        public const byte OP_TOALTSTACK = 0x6b;
        public const byte OP_FROMALTSTACK = 0x6c;
        public const byte OP_2DROP = 0x6d;
        public const byte OP_2DUP = 0x6e;
        public const byte OP_3DUP = 0x6f;
        public const byte OP_2OVER = 0x70;
        public const byte OP_2ROT = 0x71;
        public const byte OP_2SWAP = 0x72;
        public const byte OP_IFDUP = 0x73;
        public const byte OP_DEPTH = 0x74;
        public const byte OP_DROP = 0x75;
        public const byte OP_DUP = 0x76;
        public const byte OP_NIP = 0x77;
        public const byte OP_OVER = 0x78;
        public const byte OP_PICK = 0x79;
        public const byte OP_ROLL = 0x7a;
        public const byte OP_ROT = 0x7b;
        public const byte OP_SWAP = 0x7c;
        public const byte OP_TUCK = 0x7d;

        // Splice
        // If any opcode marked as disabled is present in a script, it must abort and fail.

        public const byte OP_CAT = 0x7e;
        public const byte OP_SUBSTR = 0x7f;
        public const byte OP_LEFT = 0x80;
        public const byte OP_RIGHT = 0x81;
        public const byte OP_SIZE = 0x82;

        // Bitwise logic
        // If any opcode marked as disabled is present in a script, it must abort and fail.
        public const byte OP_INVERT = 0x83;
        public const byte OP_AND = 0x84;
        public const byte OP_OR = 0x85;
        public const byte OP_XOR = 0x86;
        public const byte OP_EQUAL = 0x87;
        public const byte OP_EQUALVERIFY = 0x88;

        // Arithmetic
        // Note: Arithmetic inputs are limited to signed 32-bit integers, but may overflow their output.
        // If any input value for any of these commands is longer than 4 bytes, the script must abort and fail. If any opcode marked as disabled is present in a script - it must also abort and fail.

        public const byte OP_1ADD = 0x8b;
        public const byte OP_1SUB = 0x8c;
        public const byte OP_2MUL = 0x8d;
        public const byte OP_2DIV = 0x8e;
        public const byte OP_NEGATE = 0x8f;
        public const byte OP_ABS = 0x90;
        public const byte OP_NOT = 0x91;
        public const byte OP_0NOTEQUAL = 0x92;

        public const byte OP_ADD = 0x93;
        public const byte OP_SUB = 0x94;
        public const byte OP_MUL = 0x95;
        public const byte OP_DIV = 0x96;
        public const byte OP_MOD = 0x97;
        public const byte OP_LSHIFT = 0x98;
        public const byte OP_RSHIFT = 0x99;

        public const byte OP_BOOLAND = 0x9a;
        public const byte OP_BOOLOR = 0x9b;
        public const byte OP_NUMEQUAL = 0x9c;
        public const byte OP_NUMEQUALVERIFY = 0x9d;
        public const byte OP_NUMNOTEQUAL = 0x9e;
        public const byte OP_LESSTHAN = 0x9f;
        public const byte OP_GREATERTHAN = 0xa0;
        public const byte OP_LESSTHANOREQUAL = 0xa1;
        public const byte OP_GREATERTHANOREQUAL = 0xa2;
        public const byte OP_MIN = 0xa3;
        public const byte OP_MAX = 0xa4;

        public const byte OP_WITHIN = 0xa5;

        // Crypto
        public const byte OP_RIPEMD160 = 0xa6;
        public const byte OP_SHA1 = 0xa7;
        public const byte OP_SHA256 = 0xa8;
        public const byte OP_HASH160 = 0xa9;
        public const byte OP_HASH256 = 0xaa;
        public const byte OP_CODESEPARATOR = 0xab;
        public const byte OP_CHECKSIG = 0xac;
        public const byte OP_CHECKSIGVERIFY = 0xad;
        public const byte OP_CHECKMULTISIG = 0xae;
        public const byte OP_CHECKMULTISIGVERIFY = 0xaf;
        
        // expansion
        public const byte OP_NOP1 = 0xb0;
        public const byte OP_NOP2 = 0xb1;
        public const byte OP_CHECKLOCKTIMEVERIFY = OP_NOP2;
        public const byte OP_NOP3 = 0xb2;
        public const byte OP_NOP4 = 0xb3;
        public const byte OP_NOP5 = 0xb4;
        public const byte OP_NOP6 = 0xb5;
        public const byte OP_NOP7 = 0xb6;
        public const byte OP_NOP8 = 0xb7;
        public const byte OP_NOP9 = 0xb8;
        public const byte OP_NOP10 = 0xb9;
        
        
        // template matching params
        public const byte OP_SMALLINTEGER = 0xfa;
        public const byte OP_PUBKEYS = 0xfb;

        // Pseudo-words
        // These words are used internally for assisting with transaction matching.They are invalid if used in actual scripts.

        public const byte OP_PUBKEYHASH = 0xfd;
        public const byte OP_PUBKEY = 0xfe;
        public const byte OP_INVALIDOPCODE = 0xff;

        // Reserved words
        // Any opcode not assigned is also reserved.Using an unassigned opcode makes the transaction invalid.

        public const byte OP_RESERVED = 0x50;
        public const byte OP_VER = 0x62;
        public const byte OP_VERIF = 0x65;
        public const byte OP_VERNOTIF = 0x66;
        public const byte OP_RESERVED1 = 0x89;
        public const byte OP_RESERVED2 = 0x8a;

    }
}
