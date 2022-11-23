
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;

namespace KS.Drivers.RNG
{
    internal class DefaultRandom : IRandomDriver
    {
        private static readonly Random random = new();

        public string DriverName => "Default";

        public DriverTypes DriverType => DriverTypes.RNG;

        public int Random() => Random(int.MaxValue - 1);

        public int Random(int max) => Random(0, max);

        public int Random(int min, int max) => random.Next(min, max + 1);

        public short RandomShort() => RandomShort(short.MaxValue);

        public short RandomShort(short max) => RandomShort(0, max);

        public short RandomShort(short min, short max) => (short)Random(min, max);

        public int RandomIdx() => RandomIdx(int.MaxValue);

        public int RandomIdx(int max) => RandomIdx(0, max);

        public int RandomIdx(int min, int max) => random.Next(min, max);

        public double RandomDouble() => random.NextDouble();

        public double RandomDouble(double max) => random.NextDouble() * max;

        public bool RandomChance(double prob) => random.NextDouble() < prob;

        public bool RandomChance(int probPercent) => RandomChance((probPercent >= 0 && probPercent <= 100 ? probPercent : Random(100)) / 100d);

        public bool RandomRussianRoulette() => (RandomShort() % 6) == 0;
    }
}
