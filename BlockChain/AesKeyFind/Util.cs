using System;

namespace BlockChain.AesKeyFind {
    class Util {

        const UInt64 m1  = 0x5555555555555555L; // binary: 0101...
        const UInt64 m2  = 0x3333333333333333L; // binary: 00110011..
        const UInt64 m4  = 0x0f0f0f0f0f0f0f0fL; // binary:  4 zeros,  4 ones ...
        const UInt64 h01 = 0x0101010101010101L; // the sum of 256 to the power of 0,1,2,3...

        public static UInt32 bit(UInt32 vector, int n) {
            return (vector >> n) & 1;
        }

        // Set byte n of vector to val.
        public static UInt32 set_byte(UInt32 vector, int n, byte val) {
            return (UInt32)((vector & ~(0xFF << (8 * n))) | (val << (8 * n)));
        }

        // Return byte n of vector.
        public static byte get_byte(UInt32 vector, int n) {
            return (byte)((vector >> (8 * n)) & 0xFF);
        }

        public static UInt32 popcount(UInt64 x) {
            x -= (x >> 1) & m1;         // put count of each 2 bits into those 2 bits
            x = (x & m2) + ((x >> 2) & m2); // put count of each 4 bits into those 4 bits
            x = (x + (x >> 4)) & m4;        // put count of each 8 bits into those 8 bits
            return (UInt32)((x * h01) >> 56);  // returns left 8 bits of x + (x<<8) + (x<<16) + (x<<24) + ...
        }

        public static void print_word(UInt32 word) {
            for (int @byte = 0; @byte < 4; @byte++)
                Console.Write("%02x", get_byte(word, @byte));
        }
    }
}
