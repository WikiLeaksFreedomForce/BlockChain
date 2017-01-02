using System;

namespace BlockChain.AesKeyFind {

    public class AesKeyFind {
        
        static long gThreshold = 10;
        static bool gVerbose = false;
        static bool gProgress = true;

        static unsafe void print_key(UInt32* map, int num_bits, int address) {
            if (gVerbose) {
                Console.WriteLine("FOUND POSSIBLE %d-BIT KEY AT BYTE %zx \n\n", num_bits, address);
                Console.WriteLine("KEY: ");
            }

            int num_words = num_bits / 32;
            for (int col = 0; col < num_words; col++){
                Util.print_word(map[col]);
            }

            Console.WriteLine("\n");


            if (gVerbose) {
                Console.WriteLine("\n");
                Console.WriteLine("EXTENDED KEY: \n");

                int num_roundkeys = 0;
                if (num_bits == 256) num_roundkeys = 15;
                if (num_bits == 128) num_roundkeys = 11;
                for (int row = 0; row < num_roundkeys; row++) {
                    for (int column = 0; column < 4; column++) {
                        Util.print_word(map[(4 * row + column)]);
                    }
                    Console.WriteLine("\n");
                }

                Console.WriteLine("\n");
                Console.WriteLine("CONSTRAINTS ON ROWS:\n");

                for (int row = 1; row < num_roundkeys; row++) {
                    for (int column = 0; column < num_words; column++) {
                        if (num_bits == 256 && row == 7 && column >= 4) break;
                        if (column == 0)
                            Util.print_word(Aes.key_core(map[num_words * row - 1], row) ^
                                    map[num_words * (row - 1)] ^
                                    map[num_words * row]);
                        else if (column == 4)
                            Util.print_word(Aes.sbox_bytes(map[num_words * row + 3]) ^
                                    map[num_words * (row - 1) + 4] ^
                                    map[num_words * row + 4]);
                        else
                            Util.print_word(map[num_words * row + column - 1] ^
                                    map[num_words * (row - 1) + column] ^
                                    map[num_words * row + column]);
                    }
                    Console.WriteLine("\n");
                }
                Console.WriteLine("\n");
            }
        }


        // Simple entropy test
        //
        // Returns true if the 176 bytes starting at location bmap[i] contain
        // more than 8 repeats of any byte.  This is a primitive measure of
        // entropy, but it works well enough.  The function keeps track of a
        // sliding window of byte counts.

        static bool new_call = true;
        static int[] byte_freq = new int[256];

        static bool entropy(byte[] bmap, int i)
        {
            if (new_call) {
                for (int x = 0; x < 176; x++){
                    byte_freq[bmap[x]]++;
                }

                new_call = false;
            }

            bool test = false;
            for (int b=0; b<=0xFF; b++) {
                if (byte_freq[b] > 8) {
                    test = true;
                    break;
                }
            }
            byte_freq[bmap[i]]--;
            byte_freq[bmap[i+176]]++;
            return test;
        }

        public static unsafe void find_keys(byte[] bmap, int last)
        {
            int percent = 0;
            int increment = last / 100;

            if (gProgress)
                print_progress(percent);

            fixed (byte* pmap = bmap){
                for (int i = 0; i < last; i++){
                    if (entropy(bmap, i)) continue;

                    UInt32* map = (UInt32*) &(pmap[i]);

                    // Check distance from 256-bit AES key
                    UInt32 xor_count_256 = 0;
                    for (int row = 1; row < 8; row++){
                        for (int column = 0; column < 8; column++){
                            if (row == 7 && column == 4) break;
                            if (column == 0)
                                xor_count_256 += Util.popcount(Aes.key_core(map[8*row - 1], row) ^
                                                               map[8*(row - 1)] ^
                                                               map[8*row]);
                            else if (column == 4)
                                xor_count_256 += Util.popcount(Aes.sbox_bytes(map[8*row + 3]) ^
                                                               map[8*(row - 1) + 4] ^
                                                               map[8*row + 4]);
                            else
                                xor_count_256 += Util.popcount(map[8*row + column - 1] ^
                                                               map[8*(row - 1) + column] ^
                                                               map[8*row + column]);
                        }
                        if (xor_count_256 > gThreshold)
                            break;
                    }
                    if (xor_count_256 <= gThreshold)
                        print_key(map, 256, i);

                    // Check distance from 128-bit AES key
                    UInt32 xor_count_128 = 0;
                    for (int row = 1; row < 11; row++){
                        for (int column = 0; column < 4; column++){
                            if (column == 0)
                                xor_count_128 += Util.popcount(Aes.key_core(map[4*row - 1], row) ^
                                                               map[4*(row - 1)] ^
                                                               map[4*row]);
                            else
                                xor_count_128 += Util.popcount((map[4*row + column - 1] ^
                                                                map[4*(row - 1) + column]) ^
                                                               map[4*row + column]);
                        }
                        if (xor_count_128 > gThreshold)
                            break;
                    }
                    if (xor_count_128 < gThreshold)
                        print_key(map, 128, i);

                    if (gProgress){
                        int pct = (increment > 0) ? i/increment : i*100/last;
                        if (pct > percent){
                            percent = pct;
                            print_progress(percent);
                        }
                    }
                }

                if (gProgress){
                    print_progress(100);
                }
            }
        }

        static void print_progress(int percent) {
            Console.WriteLine("Keyfind progress: %zu%%\r", percent);
        }

    }
}
