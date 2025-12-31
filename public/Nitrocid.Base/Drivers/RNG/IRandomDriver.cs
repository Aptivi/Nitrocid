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

namespace Nitrocid.Base.Drivers.RNG
{
    /// <summary>
    /// Random driver interface for drivers
    /// </summary>
    public interface IRandomDriver : IDriver
    {
        /// <summary>
        /// Gets a random number from 0 to <see cref="int.MaxValue"/>
        /// </summary>
        int Random();

        /// <summary>
        /// Gets a random number from 0 to <paramref name="max"/>
        /// </summary>
        /// <param name="max">Max number</param>
        int Random(int max);

        /// <summary>
        /// Gets a random number from <paramref name="min"/> to <paramref name="max"/>
        /// </summary>
        /// <param name="min">Min number</param>
        /// <param name="max">Max number</param>
        int Random(int min, int max);

        /// <summary>
        /// Gets a random number from 0 to <see cref="short.MaxValue"/>
        /// </summary>
        short RandomShort();

        /// <summary>
        /// Gets a random number from 0 to <paramref name="max"/>
        /// </summary>
        /// <param name="max">Max number</param>
        short RandomShort(short max);

        /// <summary>
        /// Gets a random number from <paramref name="min"/> to <paramref name="max"/>
        /// </summary>
        /// <param name="min">Min number</param>
        /// <param name="max">Max number</param>
        short RandomShort(short min, short max);

        /// <summary>
        /// Gets a random index number from 0 to <see cref="int.MaxValue"/> - 1
        /// </summary>
        int RandomIdx();

        /// <summary>
        /// Gets a random index number from 0 to <paramref name="max"/> - 1
        /// </summary>
        /// <param name="max">Max number</param>
        int RandomIdx(int max);

        /// <summary>
        /// Gets a random index number from <paramref name="min"/> to <paramref name="max"/> - 1
        /// </summary>
        /// <param name="min">Min number</param>
        /// <param name="max">Max number</param>
        int RandomIdx(int min, int max);

        /// <summary>
        /// Gets a random double-precision number from 0 to less than 1.0
        /// </summary>
        double RandomDouble();

        /// <summary>
        /// Gets a random double-precision number from 0 to less than <paramref name="max"/>
        /// </summary>
        /// <param name="max">Max number</param>
        double RandomDouble(double max);

        /// <summary>
        /// Gets a random chance from the probability by raw value
        /// </summary>
        /// <param name="prob">Probability</param>
        bool RandomChance(double prob);

        /// <summary>
        /// Gets a random chance from the probability by percentage
        /// </summary>
        /// <param name="probPercent">Probability in percent (from 0 to 100)</param>
        /// <remarks>If the specified probability by percent is larger than 100% or smaller than 0%, then the probability by percentage
        /// will be set to a random value from 0% to 100%, depending on the driver implementation.</remarks>
        bool RandomChance(int probPercent);

        /// <summary>
        /// Random Russian Roulette!
        /// </summary>
        /// <returns>True if you're unlucky; otherwise, false if lucky.</returns>
        bool RandomRussianRoulette();

        /// <summary>
        /// Gets a random boolean value
        /// </summary>
        /// <returns>True or false.</returns>
        bool RandomBoolean();
    }
}
