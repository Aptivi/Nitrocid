
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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

namespace KS.Drivers.RNG
{
    /// <summary>
    /// Random driver module
    /// </summary>
    public static class RandomDriver
    {
        /// <summary>
        /// Gets a random number from 0 to <see cref="int.MaxValue"/>
        /// </summary>
        public static int Random() => DriverHandler.CurrentRandomDriver.Random();

        /// <summary>
        /// Gets a random number from 0 to <paramref name="max"/>
        /// </summary>
        /// <param name="max">Max number</param>
        public static int Random(int max) => DriverHandler.CurrentRandomDriver.Random(0, max);

        /// <summary>
        /// Gets a random number from <paramref name="min"/> to <paramref name="max"/>
        /// </summary>
        /// <param name="min">Min number</param>
        /// <param name="max">Max number</param>
        public static int Random(int min, int max) => DriverHandler.CurrentRandomDriver.Random(min, max);

        /// <summary>
        /// Gets a random number from 0 to <see cref="short.MaxValue"/>
        /// </summary>
        public static short RandomShort() => DriverHandler.CurrentRandomDriver.RandomShort(short.MaxValue);

        /// <summary>
        /// Gets a random number from 0 to <paramref name="max"/>
        /// </summary>
        /// <param name="max">Max number</param>
        public static short RandomShort(short max) => DriverHandler.CurrentRandomDriver.RandomShort(0, max);

        /// <summary>
        /// Gets a random number from <paramref name="min"/> to <paramref name="max"/>
        /// </summary>
        /// <param name="min">Min number</param>
        /// <param name="max">Max number</param>
        public static short RandomShort(short min, short max) => DriverHandler.CurrentRandomDriver.RandomShort(min, max);

        /// <summary>
        /// Gets a random index number from 0 to <see cref="int.MaxValue"/> - 1
        /// </summary>
        public static int RandomIdx() => DriverHandler.CurrentRandomDriver.RandomIdx(int.MaxValue);

        /// <summary>
        /// Gets a random index number from 0 to <paramref name="max"/> - 1
        /// </summary>
        /// <param name="max">Max number</param>
        public static int RandomIdx(int max) => DriverHandler.CurrentRandomDriver.RandomIdx(0, max);

        /// <summary>
        /// Gets a random index number from <paramref name="min"/> to <paramref name="max"/> - 1
        /// </summary>
        /// <param name="min">Min number</param>
        /// <param name="max">Max number</param>
        public static int RandomIdx(int min, int max) => DriverHandler.CurrentRandomDriver.RandomIdx(min, max);

        /// <summary>
        /// Gets a random double-precision number from 0 to less than 1.0
        /// </summary>
        public static double RandomDouble() => DriverHandler.CurrentRandomDriver.RandomDouble();

        /// <summary>
        /// Gets a random double-precision number from 0 to less than <paramref name="max"/>
        /// </summary>
        /// <param name="max">Max number</param>
        public static double RandomDouble(double max) => DriverHandler.CurrentRandomDriver.RandomDouble(max);

        /// <summary>
        /// Gets a random chance from the probability by raw value
        /// </summary>
        /// <param name="prob">Probability</param>
        public static bool RandomChance(double prob) => DriverHandler.CurrentRandomDriver.RandomChance(prob);

        /// <summary>
        /// Gets a random chance from the probability by percentage
        /// </summary>
        /// <param name="probPercent">Probability in percent (from 0 to 100)</param>
        /// <remarks>If the specified probability by percent is larger than 100% or smaller than 0%, then the probability by percentage will be set to a random value from 0% to 100%</remarks>
        public static bool RandomChance(int probPercent) => DriverHandler.CurrentRandomDriver.RandomChance(probPercent);

        /// <summary>
        /// Random Russian Roulette!
        /// </summary>
        /// <returns>True if you're unlucky; otherwise, false if lucky.</returns>
        public static bool RandomRussianRoulette() => DriverHandler.CurrentRandomDriver.RandomRussianRoulette();
    }
}
