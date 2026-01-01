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

using Nitrocid.Kernel.Debugging;

namespace Nitrocid.Drivers.RNG.Bases
{
    internal class StandardRandomDebug : BaseRandomDriver, IRandomDriver
    {
        /// <inheritdoc/>
        public override string DriverName => "StandardDebug";

        /// <inheritdoc/>
        public override DriverTypes DriverType => DriverTypes.RNG;

        /// <inheritdoc/>
        public override int Random()
        {
            int num = base.Random();
            DebugWriter.WriteDebug(DebugLevel.I, "[Num = {0}, Type = {1}]", num, nameof(Random));
            return num;
        }

        /// <inheritdoc/>
        public override int Random(int max)
        {
            int num = base.Random(max);
            DebugWriter.WriteDebug(DebugLevel.I, "[Num = {0}, Type = {1}, max = {2}]", num, nameof(Random), max);
            return num;
        }

        /// <inheritdoc/>
        public override int Random(int min, int max)
        {
            int num = random.Next(min, max + 1);
            DebugWriter.WriteDebug(DebugLevel.I, "[Num = {0}, Type = {1}, min = {2}, max = {3}]", num, nameof(Random), min, max + 1);
            return num;
        }

        /// <inheritdoc/>
        public override short RandomShort()
        {
            short num = base.RandomShort();
            DebugWriter.WriteDebug(DebugLevel.I, "[Num = {0}, Type = {1}]", num, nameof(RandomShort));
            return num;
        }

        /// <inheritdoc/>
        public override short RandomShort(short max)
        {
            short num = base.RandomShort(max);
            DebugWriter.WriteDebug(DebugLevel.I, "[Num = {0}, Type = {1}, max = {2}]", num, nameof(RandomShort), max);
            return num;
        }

        /// <inheritdoc/>
        public override short RandomShort(short min, short max)
        {
            short num = (short)base.Random(min, max);
            DebugWriter.WriteDebug(DebugLevel.I, "[Num = {0}, Type = {1}, min = {2}, max = {3}]", num, nameof(RandomShort), min, max);
            return num;
        }

        /// <inheritdoc/>
        public override int RandomIdx()
        {
            int num = base.RandomIdx();
            DebugWriter.WriteDebug(DebugLevel.I, "[Num = {0}, Type = {1}]", num, nameof(RandomIdx));
            return num;
        }

        /// <inheritdoc/>
        public override int RandomIdx(int max)
        {
            int num = base.RandomIdx(max);
            DebugWriter.WriteDebug(DebugLevel.I, "[Num = {0}, Type = {1}, max = {2}]", num, nameof(RandomIdx), max);
            return num;
        }

        /// <inheritdoc/>
        public override int RandomIdx(int min, int max)
        {
            int num = random.Next(min, max);
            DebugWriter.WriteDebug(DebugLevel.I, "[Num = {0}, Type = {1}, min = {2}, max = {3}]", num, nameof(RandomIdx), min, max);
            return num;
        }

        /// <inheritdoc/>
        public override double RandomDouble()
        {
            double num = random.NextDouble();
            DebugWriter.WriteDebug(DebugLevel.I, "[Num = {0}, Type = {1}]", num, nameof(RandomDouble));
            return num;
        }

        /// <inheritdoc/>
        public override double RandomDouble(double max)
        {
            double num = random.NextDouble() * max;
            DebugWriter.WriteDebug(DebugLevel.I, "[Num = {0}, Type = {1}, max = {2}]", num, nameof(RandomDouble), max);
            return num;
        }

        /// <inheritdoc/>
        public override bool RandomChance(double prob)
        {
            bool status = random.NextDouble() < prob;
            DebugWriter.WriteDebug(DebugLevel.I, "[Value = {0}, Type = {1}, prob = {2}]", status, nameof(RandomChance), prob);
            return status;
        }

        /// <inheritdoc/>
        public override bool RandomChance(int probPercent)
        {
            bool status = base.RandomChance((probPercent >= 0 && probPercent <= 100 ? probPercent : base.Random(100)) / 100d);
            DebugWriter.WriteDebug(DebugLevel.I, "[Value = {0}, Type = {1}, probPercent = {2}]", status, nameof(RandomChance), probPercent);
            return status;
        }

        /// <inheritdoc/>
        public override bool RandomRussianRoulette()
        {
            bool status = base.RandomShort() % 6 == 0;
            DebugWriter.WriteDebug(DebugLevel.I, "[Value = {0}, Type = {1}]", status, nameof(RandomRussianRoulette));
            return status;
        }

        /// <inheritdoc/>
        public override bool RandomBoolean()
        {
            bool status = random.Next(2) == 1;
            DebugWriter.WriteDebug(DebugLevel.I, "[Value = {0}, Type = {1}]", status, nameof(RandomBoolean));
            return status;
        }
    }
}
