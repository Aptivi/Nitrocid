using System;

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

namespace KS.TimeDate
{
    public static class TimeDateConverters
    {

        public readonly static DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        /// <summary>
        /// Converts the date to Unix time (seconds since 1970/1/1) (UTC)
        /// </summary>
        public static double DateToUnix(DateTime TargetDate)
        {
            return (TargetDate - UnixEpoch).TotalSeconds;
        }

        /// <summary>
        /// Converts the Unix time (seconds since 1970/1/1) to date (UTC)
        /// </summary>
        public static DateTime UnixToDate(double UnixTime)
        {
            return UnixEpoch.AddSeconds(UnixTime);
        }

    }
}