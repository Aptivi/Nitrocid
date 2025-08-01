﻿//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using System;

namespace Nitrocid.Base.Drivers.RNG
{
    /// <summary>
    /// Base random number generation driver
    /// </summary>
    public abstract class BaseRandomDriver : IRandomDriver
    {
        internal static readonly Random random = new();

        /// <inheritdoc/>
        public virtual string DriverName => "Default";

        /// <inheritdoc/>
        public virtual DriverTypes DriverType => DriverTypes.RNG;

        /// <inheritdoc/>
        public virtual bool DriverInternal => false;

        /// <inheritdoc/>
        public virtual int Random() =>
            Random(int.MaxValue - 1);

        /// <inheritdoc/>
        public virtual int Random(int max) =>
            Random(0, max);

        /// <inheritdoc/>
        public virtual int Random(int min, int max) =>
            random.Next(min, max + 1);

        /// <inheritdoc/>
        public virtual short RandomShort() =>
            RandomShort(short.MaxValue);

        /// <inheritdoc/>
        public virtual short RandomShort(short max) =>
            RandomShort(0, max);

        /// <inheritdoc/>
        public virtual short RandomShort(short min, short max) =>
            (short)Random(min, max);

        /// <inheritdoc/>
        public virtual int RandomIdx() =>
            RandomIdx(int.MaxValue);

        /// <inheritdoc/>
        public virtual int RandomIdx(int max) =>
            RandomIdx(0, max);

        /// <inheritdoc/>
        public virtual int RandomIdx(int min, int max) =>
            random.Next(min, max);

        /// <inheritdoc/>
        public virtual double RandomDouble() =>
            random.NextDouble();

        /// <inheritdoc/>
        public virtual double RandomDouble(double max) =>
            random.NextDouble() * max;

        /// <inheritdoc/>
        public virtual bool RandomChance(double prob) =>
            random.NextDouble() < prob;

        /// <inheritdoc/>
        public virtual bool RandomChance(int probPercent) =>
            RandomChance((probPercent >= 0 && probPercent <= 100 ? probPercent : Random(100)) / 100d);

        /// <inheritdoc/>
        public virtual bool RandomRussianRoulette() =>
            RandomShort() % 6 == 0;

        /// <inheritdoc/>
        public virtual bool RandomBoolean() =>
            random.Next(2) == 1;
    }
}
