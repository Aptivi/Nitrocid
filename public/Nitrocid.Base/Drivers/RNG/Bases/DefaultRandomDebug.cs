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

using Nitrocid.Base.Kernel.Debugging;
using System.Security.Cryptography;

namespace Nitrocid.Base.Drivers.RNG.Bases
{
    internal class DefaultRandomDebug : BaseRandomDriver, IRandomDriver
    {
        /// <inheritdoc/>
        public override string DriverName => "DefaultDebug";

        /// <inheritdoc/>
        public override DriverTypes DriverType => DriverTypes.RNG;

        /// <inheritdoc/>
        public override int Random() => Random(int.MaxValue - 1);

        /// <inheritdoc/>
        public override int Random(int max) => Random(0, max);

        /// <inheritdoc/>
        public override int Random(int min, int max)
        {
            int num = RandomNumberGenerator.GetInt32(min, max + 1);
            DebugWriter.WriteDebug(DebugLevel.I, "[Num = {0}, Type = {1}, min = {2}, max = {3}]", vars: [num, nameof(Random), min, max + 1]);
            return num;
        }

        /// <inheritdoc/>
        public override short RandomShort() => RandomShort(short.MaxValue);

        /// <inheritdoc/>
        public override short RandomShort(short max) => RandomShort(0, max);

        /// <inheritdoc/>
        public override short RandomShort(short min, short max)
        {
            short num = (short)Random(min, max);
            DebugWriter.WriteDebug(DebugLevel.I, "[Num = {0}, Type = {1}, min = {2}, max = {3}]", vars: [num, nameof(RandomShort), min, max]);
            return num;
        }

        /// <inheritdoc/>
        public override int RandomIdx() => RandomIdx(int.MaxValue);

        /// <inheritdoc/>
        public override int RandomIdx(int max) => RandomIdx(0, max);

        /// <inheritdoc/>
        public override int RandomIdx(int min, int max)
        {
            int num = RandomNumberGenerator.GetInt32(min, max);
            DebugWriter.WriteDebug(DebugLevel.I, "[Num = {0}, Type = {1}, min = {2}, max = {3}]", vars: [num, nameof(RandomIdx), min, max]);
            return num;
        }

        /// <inheritdoc/>
        public override double RandomDouble()
        {
            double num = Random() / (double)(int.MaxValue - 1);
            DebugWriter.WriteDebug(DebugLevel.I, "[Num = {0}, Type = {1}]", vars: [num, nameof(RandomDouble)]);
            return num;
        }

        /// <inheritdoc/>
        public override double RandomDouble(double max)
        {
            double num = RandomDouble() * max;
            DebugWriter.WriteDebug(DebugLevel.I, "[Num = {0}, Type = {1}, max = {2}]", vars: [num, nameof(RandomDouble), max]);
            return num;
        }

        /// <inheritdoc/>
        public override bool RandomChance(double prob)
        {
            bool status = RandomDouble() < prob;
            DebugWriter.WriteDebug(DebugLevel.I, "[Value = {0}, Type = {1}, prob = {2}]", vars: [status, nameof(RandomChance), prob]);
            return status;
        }

        /// <inheritdoc/>
        public override bool RandomChance(int probPercent)
        {
            bool status = RandomChance((probPercent >= 0 && probPercent <= 100 ? probPercent : Random(100)) / 100d);
            DebugWriter.WriteDebug(DebugLevel.I, "[Value = {0}, Type = {1}, probPercent = {2}]", vars: [status, nameof(RandomChance), probPercent]);
            return status;
        }

        /// <inheritdoc/>
        public override bool RandomRussianRoulette()
        {
            bool status = RandomShort() % 6 == 0;
            DebugWriter.WriteDebug(DebugLevel.I, "[Value = {0}, Type = {1}]", vars: [status, nameof(RandomRussianRoulette)]);
            return status;
        }

        /// <inheritdoc/>
        public override bool RandomBoolean()
        {
            bool status = random.Next(2) == 1;
            DebugWriter.WriteDebug(DebugLevel.I, "[Value = {0}, Type = {1}]", vars: [status, nameof(RandomBoolean)]);
            return status;
        }
    }
}
