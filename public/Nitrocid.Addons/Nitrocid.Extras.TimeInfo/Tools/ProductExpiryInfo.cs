//
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

using Nitrocid.Kernel.Time;
using Nitrocid.Kernel.Time.Converters;
using System;

namespace Nitrocid.Extras.TimeInfo.Tools
{
    /// <summary>
    /// Product expiry information
    /// </summary>
    public class ProductExpiryInfo
    {
        /// <summary>
        /// Product production date
        /// </summary>
        public DateTimeOffset ProductionDate { get; }

        /// <summary>
        /// Product expiry time span to add to the production date
        /// </summary>
        public TimeSpan ExpirySpan { get; }

        /// <summary>
        /// Expiry date of the product (or best before)
        /// </summary>
        public DateTimeOffset ExpiryDate =>
            ProductionDate + ExpirySpan;

        /// <summary>
        /// Gets the product health percentage
        /// </summary>
        /// <returns>Proruct health percentage according to the current time, or 0 if it's expired, or -1 if it's preproduction</returns>
        public double GetProductHealth()
        {
            // Get the milliseconds for each of production, current, and expiry dates since the Unix epoch
            double productionMs = ProductionDate.ToUnixTimeMilliseconds();
            double currentMs = TimeDateConverters.DateToUnixMs(TimeDateTools.KernelDateTimeUtc);
            double expiryMs = ExpiryDate.ToUnixTimeMilliseconds();

            // Check to see if the product is not produced yet or if it's expired
            if (currentMs < productionMs)
                return -1;
            if (currentMs >= expiryMs)
                return 0;

            // Calculate the product health by comparing the current date with the production and the expiry
            // millisecond values.
            double expiryPercentage = (currentMs - productionMs) / (expiryMs - productionMs) * 100;
            double health = 100 - expiryPercentage;
            return health;
        }

        /// <summary>
        /// Makes a new instance of the product expiry info
        /// </summary>
        /// <param name="production">Production date (usually found at either the side or the bottom of the product)</param>
        /// <param name="expirySpan">Time span in which the product will be considered expired (use this when products say to the effect of "Best for one year after the production date")</param>
        /// <exception cref="ArgumentException"></exception>
        public ProductExpiryInfo(DateTimeOffset production, TimeSpan expirySpan)
        {
            if (expirySpan < TimeSpan.Zero)
                throw new ArgumentException("Expiry date may not be before the production date");
            ProductionDate = production;
            ExpirySpan = expirySpan;
        }
    }
}
