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
    internal class DefaultRandom : BaseRandomDriver, IRandomDriver
    {
        /// <inheritdoc/>
        public override DriverTypes DriverType => DriverTypes.RNG;

        /// <inheritdoc/>
        public override int Random() =>
            Random(int.MaxValue - 1);

        /// <inheritdoc/>
        public override int Random(int max) =>
            Random(0, max);

        /// <inheritdoc/>
        public override int Random(int min, int max) =>
            RandomNumberGenerator.GetInt32(min, max + 1);

        /// <inheritdoc/>
        public override short RandomShort() =>
            RandomShort(short.MaxValue);

        /// <inheritdoc/>
        public override short RandomShort(short max) =>
            RandomShort(0, max);

        /// <inheritdoc/>
        public override short RandomShort(short min, short max) =>
            (short)Random(min, max);

        /// <inheritdoc/>
        public override int RandomIdx() =>
            RandomIdx(int.MaxValue);

        /// <inheritdoc/>
        public override int RandomIdx(int max) =>
            RandomIdx(0, max);

        /// <inheritdoc/>
        public override int RandomIdx(int min, int max) =>
            RandomNumberGenerator.GetInt32(min, max);

        /// <inheritdoc/>
        public override double RandomDouble() =>
            Random() / (double)(int.MaxValue - 1);

        /// <inheritdoc/>
        public override double RandomDouble(double max) =>
            RandomDouble() * max;

        /// <inheritdoc/>
        public override bool RandomChance(double prob) =>
            RandomDouble() < prob;

        /// <inheritdoc/>
        public override bool RandomChance(int probPercent) =>
            RandomChance((probPercent >= 0 && probPercent <= 100 ? probPercent : Random(100)) / 100d);

        /// <inheritdoc/>
        public override bool RandomRussianRoulette() =>
            RandomShort() % 6 == 0;

        /// <inheritdoc/>
        public override bool RandomBoolean() =>
            Random(1) == 1;
    }
}
