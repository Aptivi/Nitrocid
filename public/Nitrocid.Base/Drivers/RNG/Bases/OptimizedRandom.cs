//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System.Security.Cryptography;

namespace Nitrocid.Base.Drivers.RNG.Bases
{
    // Refer to random.c in the References folder for more information.
    internal unsafe class OptimizedRandom : BaseRandomDriver, IRandomDriver
    {
        /// <summary>
        /// 5^15 (mod 65536)
        /// </summary>
        private const int MULTIPLIER = 18829;
        private const ushort PRESEED = 0xe5a1;

        /// <summary>
        /// 521-bit shift register
        /// </summary>
        private readonly byte[] shiftreg = new byte[66];
        private byte* randptr;

        /// <inheritdoc/>
        public override DriverTypes DriverType => DriverTypes.RNG;

        /// <inheritdoc/>
        public override string DriverName => "Optimized";

        /// <inheritdoc/>
        public override int Random() =>
            Random(int.MaxValue);

        /// <inheritdoc/>
        public override int Random(int max) =>
            (int)(SignedRandom() * max);

        /// <inheritdoc/>
        public override int Random(int min, int max) =>
            RandomNumberGenerator.GetInt32(min, max + 1);

        /// <inheritdoc/>
        public override short RandomShort() =>
            (short)UnsignedRandom();

        /// <inheritdoc/>
        public override short RandomShort(short max) =>
            (short)(SignedRandom() * max);

        /// <inheritdoc/>
        public override short RandomShort(short min, short max) =>
            (short)Random(min, max);

        /// <inheritdoc/>
        public override int RandomIdx() =>
            Random() - 1;

        /// <inheritdoc/>
        public override int RandomIdx(int max) =>
            (int)(SignedRandom() * max) - 1;

        /// <inheritdoc/>
        public override int RandomIdx(int min, int max) =>
            RandomNumberGenerator.GetInt32(min, max);

        /// <inheritdoc/>
        public override double RandomDouble() =>
            SignedRandom();

        /// <inheritdoc/>
        public override double RandomDouble(double max) =>
            SignedRandom() * max;

        /// <inheritdoc/>
        public override bool RandomChance(double prob) =>
            SignedRandom() < prob;

        /// <inheritdoc/>
        public override bool RandomChance(int probPercent) =>
            RandomChance((probPercent >= 0 && probPercent <= 100 ? probPercent : Random(100)) / 100d);

        /// <inheritdoc/>
        public override bool RandomRussianRoulette() =>
            RandomShort() % 6 == 0;

        /// <inheritdoc/>
        public override bool RandomBoolean() =>
            Random(1) == 1;

        #region Taken from random.c
        // Reference: http://ftp.grnet.gr/pub/lang/algorithms/c/jpl-c/random.c

        internal void Randize(int seed)
        {
            fixed (byte* reg = shiftreg)
            {
                int i;
                int* rseed;

                if (seed == 0)
                    seed = PRESEED;
                else if (seed < 0)
                {
                    rseed = (int*)&reg[16];
                    while ((seed = *++rseed) == 0)
                        ;
                }

                for (i = 0; i < 66; i++)
                {
                    reg[i++] = (byte)(seed *= MULTIPLIER & 0xff);
                    reg[i] = (byte)(seed >> 8 & 0xff);
                }

                reg[65] &= 0x80;
                Refill();
            }
        }

        internal void Refill()
        {
            fixed (byte* reg = shiftreg)
            {
                byte* p, q;
                byte cy0, cy1;
                int i;

                // Point at the first byte
                p = reg;
                q = &p[4];

                // Shift 4 * 8 = 32, and mod-2 add the register to the 32-bit shift.
                for (i = 0; i < 62; i++)
                    *p++ ^= *q++;

                // p should now be at byte 61:
                p--;
                q = reg;

                // Set carry bit to zero
                cy0 = 0;

                // For the remaining 32 bits, save carry for next shift, mod-2 add the 489-shifts, set carry for next shift,
                // and mod-2 the final bit.
                for (i = 0; i < 4; i++)
                {
                    cy1 = (*q & 1) != 0 ? (byte)0x80 : (byte)0;
                    *p++ ^= (byte)(*q++ >> 1 & 0x7f | cy0);
                    cy0 = cy1;
                }
                *p ^= cy0;

                // Point at the first number
                randptr = reg;
            }
        }

        internal uint UnsignedRandom()
        {
            fixed (byte* reg = shiftreg)
            {
                uint r;
                if (randptr < reg)
                    Randize(0);
                else if (randptr > reg + 62)
                    Refill();
                r = *randptr++;
                r += (uint)(*randptr++ << 8);
                return r;
            }
        }

        internal double SignedRandom() =>
            UnsignedRandom() / 65535.0;
        #endregion
    }
}
