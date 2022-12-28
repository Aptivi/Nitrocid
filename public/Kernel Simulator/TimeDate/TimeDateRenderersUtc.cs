
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

using System;
using System.Globalization;
using KS.Kernel;
using KS.Languages;

namespace KS.TimeDate
{
    /// <summary>
    /// Time and date rendering module (UTC)
    /// </summary>
    public static class TimeDateRenderersUtc
    {

        /// <summary>
        /// Renders the current time based on kernel config (long or short) and current culture
        /// </summary>
        /// <returns>A long or short time</returns>
        public static string RenderTimeUtc()
        {
            if (Flags.LongTimeDate)
            {
                return TimeDate.KernelDateTimeUtc.ToString(CultureManager.CurrentCult.DateTimeFormat.LongTimePattern, CultureManager.CurrentCult);
            }
            else
            {
                return TimeDate.KernelDateTimeUtc.ToString(CultureManager.CurrentCult.DateTimeFormat.ShortTimePattern, CultureManager.CurrentCult);
            }
        }

        /// <summary>
        /// Renders the current time based on kernel config (long or short) and current culture
        /// </summary>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A long or short time</returns>
        public static string RenderTimeUtc(TimeDate.FormatType FormatType)
        {
            if (FormatType == TimeDate.FormatType.Long)
            {
                return TimeDate.KernelDateTimeUtc.ToString(CultureManager.CurrentCult.DateTimeFormat.LongTimePattern, CultureManager.CurrentCult);
            }
            else
            {
                return TimeDate.KernelDateTimeUtc.ToString(CultureManager.CurrentCult.DateTimeFormat.ShortTimePattern, CultureManager.CurrentCult);
            }
        }

        /// <summary>
        /// Renders the current time based on specified culture
        /// </summary>
        /// <param name="Cult">A culture.</param>
        /// <returns>A time</returns>
        public static string RenderTimeUtc(CultureInfo Cult)
        {
            if (Flags.LongTimeDate)
            {
                return TimeDate.KernelDateTimeUtc.ToString(Cult.DateTimeFormat.LongTimePattern, Cult);
            }
            else
            {
                return TimeDate.KernelDateTimeUtc.ToString(Cult.DateTimeFormat.ShortTimePattern, Cult);
            }
        }

        /// <summary>
        /// Renders the current time based on specified culture
        /// </summary>
        /// <param name="Cult">A culture.</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A time</returns>
        public static string RenderTimeUtc(CultureInfo Cult, TimeDate.FormatType FormatType)
        {
            if (FormatType == TimeDate.FormatType.Long)
            {
                return TimeDate.KernelDateTimeUtc.ToString(Cult.DateTimeFormat.LongTimePattern, Cult);
            }
            else
            {
                return TimeDate.KernelDateTimeUtc.ToString(Cult.DateTimeFormat.ShortTimePattern, Cult);
            }
        }

        /// <summary>
        /// Renders the time based on specified time using the kernel config (long or short) and current culture
        /// </summary>
        /// <param name="DT">Specified time</param>
        /// <returns>A long or short time</returns>
        public static string RenderTimeUtc(DateTime DT)
        {
            if (Flags.LongTimeDate)
            {
                return DT.ToUniversalTime().ToString(CultureManager.CurrentCult.DateTimeFormat.LongTimePattern, CultureManager.CurrentCult);
            }
            else
            {
                return DT.ToUniversalTime().ToString(CultureManager.CurrentCult.DateTimeFormat.ShortTimePattern, CultureManager.CurrentCult);
            }
        }

        /// <summary>
        /// Renders the time based on specified time using the kernel config (long or short) and current culture
        /// </summary>
        /// <param name="DT">Specified time</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A long or short time</returns>
        public static string RenderTimeUtc(DateTime DT, TimeDate.FormatType FormatType)
        {
            if (FormatType == TimeDate.FormatType.Long)
            {
                return DT.ToUniversalTime().ToString(CultureManager.CurrentCult.DateTimeFormat.LongTimePattern, CultureManager.CurrentCult);
            }
            else
            {
                return DT.ToUniversalTime().ToString(CultureManager.CurrentCult.DateTimeFormat.ShortTimePattern, CultureManager.CurrentCult);
            }
        }

        /// <summary>
        /// Renders the time based on specified date and culture using the kernel config (long or short)
        /// </summary>
        /// <param name="DT">Specified time</param>
        /// <param name="Cult">A culture</param>
        /// <returns>A time</returns>
        public static string RenderTimeUtc(DateTime DT, CultureInfo Cult)
        {
            if (Flags.LongTimeDate)
            {
                return DT.ToUniversalTime().ToString(Cult.DateTimeFormat.LongTimePattern, Cult);
            }
            else
            {
                return DT.ToUniversalTime().ToString(Cult.DateTimeFormat.ShortTimePattern, Cult);
            }
        }

        /// <summary>
        /// Renders the time based on specified date and culture using the kernel config (long or short)
        /// </summary>
        /// <param name="DT">Specified time</param>
        /// <param name="Cult">A culture</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A time</returns>
        public static string RenderTimeUtc(DateTime DT, CultureInfo Cult, TimeDate.FormatType FormatType)
        {
            if (FormatType == TimeDate.FormatType.Long)
            {
                return DT.ToUniversalTime().ToString(Cult.DateTimeFormat.LongTimePattern, Cult);
            }
            else
            {
                return DT.ToUniversalTime().ToString(Cult.DateTimeFormat.ShortTimePattern, Cult);
            }
        }

        /// <summary>
        /// Renders the current date based on kernel config (long or short) and current culture
        /// </summary>
        /// <returns>A long or short date</returns>
        public static string RenderDateUtc()
        {
            if (Flags.LongTimeDate)
            {
                return TimeDate.KernelDateTimeUtc.ToString(CultureManager.CurrentCult.DateTimeFormat.LongDatePattern, CultureManager.CurrentCult);
            }
            else
            {
                return TimeDate.KernelDateTimeUtc.ToString(CultureManager.CurrentCult.DateTimeFormat.ShortDatePattern, CultureManager.CurrentCult);
            }
        }

        /// <summary>
        /// Renders the current date based on kernel config (long or short) and current culture
        /// </summary>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A long or short date</returns>
        public static string RenderDateUtc(TimeDate.FormatType FormatType)
        {
            if (FormatType == TimeDate.FormatType.Long)
            {
                return TimeDate.KernelDateTimeUtc.ToString(CultureManager.CurrentCult.DateTimeFormat.LongDatePattern, CultureManager.CurrentCult);
            }
            else
            {
                return TimeDate.KernelDateTimeUtc.ToString(CultureManager.CurrentCult.DateTimeFormat.ShortDatePattern, CultureManager.CurrentCult);
            }
        }

        /// <summary>
        /// Renders the current date based on specified culture
        /// </summary>
        /// <param name="Cult">A culture.</param>
        /// <returns>A date</returns>
        public static string RenderDateUtc(CultureInfo Cult)
        {
            if (Flags.LongTimeDate)
            {
                return TimeDate.KernelDateTimeUtc.ToString(Cult.DateTimeFormat.LongDatePattern, Cult);
            }
            else
            {
                return TimeDate.KernelDateTimeUtc.ToString(Cult.DateTimeFormat.ShortDatePattern, Cult);
            }
        }

        /// <summary>
        /// Renders the current date based on specified culture
        /// </summary>
        /// <param name="Cult">A culture.</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A date</returns>
        public static string RenderDateUtc(CultureInfo Cult, TimeDate.FormatType FormatType)
        {
            if (FormatType == TimeDate.FormatType.Long)
            {
                return TimeDate.KernelDateTimeUtc.ToString(Cult.DateTimeFormat.LongDatePattern, Cult);
            }
            else
            {
                return TimeDate.KernelDateTimeUtc.ToString(Cult.DateTimeFormat.ShortDatePattern, Cult);
            }
        }

        /// <summary>
        /// Renders the date based on specified date using the kernel config (long or short) and current culture
        /// </summary>
        /// <param name="DT">Specified date</param>
        /// <returns>A long or short date</returns>
        public static string RenderDateUtc(DateTime DT)
        {
            if (Flags.LongTimeDate)
            {
                return DT.ToUniversalTime().ToString(CultureManager.CurrentCult.DateTimeFormat.LongDatePattern, CultureManager.CurrentCult);
            }
            else
            {
                return DT.ToUniversalTime().ToString(CultureManager.CurrentCult.DateTimeFormat.ShortDatePattern, CultureManager.CurrentCult);
            }
        }

        /// <summary>
        /// Renders the date based on specified date using the kernel config (long or short) and current culture
        /// </summary>
        /// <param name="DT">Specified date</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A long or short date</returns>
        public static string RenderDateUtc(DateTime DT, TimeDate.FormatType FormatType)
        {
            if (FormatType == TimeDate.FormatType.Long)
            {
                return DT.ToUniversalTime().ToString(CultureManager.CurrentCult.DateTimeFormat.LongDatePattern, CultureManager.CurrentCult);
            }
            else
            {
                return DT.ToUniversalTime().ToString(CultureManager.CurrentCult.DateTimeFormat.ShortDatePattern, CultureManager.CurrentCult);
            }
        }

        /// <summary>
        /// Renders the date based on specified date and culture using the kernel config (long or short)
        /// </summary>
        /// <param name="DT">Specified date</param>
        /// <param name="Cult">A culture</param>
        /// <returns>A date</returns>
        public static string RenderDateUtc(DateTime DT, CultureInfo Cult)
        {
            if (Flags.LongTimeDate)
            {
                return DT.ToUniversalTime().ToString(Cult.DateTimeFormat.LongDatePattern, Cult);
            }
            else
            {
                return DT.ToUniversalTime().ToString(Cult.DateTimeFormat.ShortDatePattern, Cult);
            }
        }

        /// <summary>
        /// Renders the date based on specified date and culture using the kernel config (long or short)
        /// </summary>
        /// <param name="DT">Specified date</param>
        /// <param name="Cult">A culture</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A date</returns>
        public static string RenderDateUtc(DateTime DT, CultureInfo Cult, TimeDate.FormatType FormatType)
        {
            if (FormatType == TimeDate.FormatType.Long)
            {
                return DT.ToUniversalTime().ToString(Cult.DateTimeFormat.LongDatePattern, Cult);
            }
            else
            {
                return DT.ToUniversalTime().ToString(Cult.DateTimeFormat.ShortDatePattern, Cult);
            }
        }

        /// <summary>
        /// Renders the current time and date based on kernel config (long or short) and current culture
        /// </summary>
        /// <returns>A long or short time and date</returns>
        public static string RenderUtc()
        {
            if (Flags.LongTimeDate)
            {
                return TimeDate.KernelDateTimeUtc.ToString(CultureManager.CurrentCult.DateTimeFormat.FullDateTimePattern, CultureManager.CurrentCult);
            }
            else
            {
                return TimeDate.KernelDateTimeUtc.ToString(CultureManager.CurrentCult.DateTimeFormat.ShortDatePattern, CultureManager.CurrentCult) + " - " + TimeDate.KernelDateTimeUtc.ToString(CultureManager.CurrentCult.DateTimeFormat.ShortTimePattern, CultureManager.CurrentCult);
            }
        }

        /// <summary>
        /// Renders the current time and date based on kernel config (long or short) and current culture
        /// </summary>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A long or short time and date</returns>
        public static string RenderUtc(TimeDate.FormatType FormatType)
        {
            if (FormatType == TimeDate.FormatType.Long)
            {
                return TimeDate.KernelDateTimeUtc.ToString(CultureManager.CurrentCult.DateTimeFormat.FullDateTimePattern, CultureManager.CurrentCult);
            }
            else
            {
                return TimeDate.KernelDateTimeUtc.ToString(CultureManager.CurrentCult.DateTimeFormat.ShortDatePattern, CultureManager.CurrentCult) + " - " + TimeDate.KernelDateTimeUtc.ToString(CultureManager.CurrentCult.DateTimeFormat.ShortTimePattern, CultureManager.CurrentCult);
            }
        }

        /// <summary>
        /// Renders the current time and date based on specified culture
        /// </summary>
        /// <param name="Cult">A culture.</param>
        /// <returns>A time and date</returns>
        public static string RenderUtc(CultureInfo Cult)
        {
            if (Flags.LongTimeDate)
            {
                return TimeDate.KernelDateTimeUtc.ToString(Cult.DateTimeFormat.FullDateTimePattern, Cult);
            }
            else
            {
                return TimeDate.KernelDateTimeUtc.ToString(Cult.DateTimeFormat.ShortDatePattern, Cult) + " - " + TimeDate.KernelDateTimeUtc.ToString(Cult.DateTimeFormat.ShortTimePattern, Cult);
            }
        }

        /// <summary>
        /// Renders the current time and date based on specified culture
        /// </summary>
        /// <param name="Cult">A culture.</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A time and date</returns>
        public static string RenderUtc(CultureInfo Cult, TimeDate.FormatType FormatType)
        {
            if (FormatType == TimeDate.FormatType.Long)
            {
                return TimeDate.KernelDateTimeUtc.ToString(Cult.DateTimeFormat.FullDateTimePattern, Cult);
            }
            else
            {
                return TimeDate.KernelDateTimeUtc.ToString(Cult.DateTimeFormat.ShortDatePattern, Cult) + " - " + TimeDate.KernelDateTimeUtc.ToString(Cult.DateTimeFormat.ShortTimePattern, Cult);
            }
        }

        /// <summary>
        /// Renders the time and date based on specified time using the kernel config (long or short) and current culture
        /// </summary>
        /// <param name="DT">Specified time and date</param>
        /// <returns>A long or short time and date</returns>
        public static string RenderUtc(DateTime DT)
        {
            if (Flags.LongTimeDate)
            {
                return DT.ToUniversalTime().ToString(CultureManager.CurrentCult.DateTimeFormat.FullDateTimePattern, CultureManager.CurrentCult);
            }
            else
            {
                return DT.ToUniversalTime().ToString(CultureManager.CurrentCult.DateTimeFormat.ShortDatePattern, CultureManager.CurrentCult) + " - " + DT.ToUniversalTime().ToString(CultureManager.CurrentCult.DateTimeFormat.ShortTimePattern, CultureManager.CurrentCult);
            }
        }

        /// <summary>
        /// Renders the time and date based on specified time using the kernel config (long or short) and current culture
        /// </summary>
        /// <param name="DT">Specified time and date</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A long or short time and date</returns>
        public static string RenderUtc(DateTime DT, TimeDate.FormatType FormatType)
        {
            if (FormatType == TimeDate.FormatType.Long)
            {
                return DT.ToUniversalTime().ToString(CultureManager.CurrentCult.DateTimeFormat.FullDateTimePattern, CultureManager.CurrentCult);
            }
            else
            {
                return DT.ToUniversalTime().ToString(CultureManager.CurrentCult.DateTimeFormat.ShortDatePattern, CultureManager.CurrentCult) + " - " + DT.ToUniversalTime().ToString(CultureManager.CurrentCult.DateTimeFormat.ShortTimePattern, CultureManager.CurrentCult);
            }
        }

        /// <summary>
        /// Renders the time and date based on specified date and culture using the kernel config (long or short)
        /// </summary>
        /// <param name="DT">Specified time and date</param>
        /// <param name="Cult">A culture</param>
        /// <returns>A time and date</returns>
        public static string RenderUtc(DateTime DT, CultureInfo Cult)
        {
            if (Flags.LongTimeDate)
            {
                return DT.ToUniversalTime().ToString(Cult.DateTimeFormat.FullDateTimePattern, Cult);
            }
            else
            {
                return DT.ToUniversalTime().ToString(Cult.DateTimeFormat.ShortDatePattern, Cult) + " - " + DT.ToUniversalTime().ToString(Cult.DateTimeFormat.ShortTimePattern, Cult);
            }
        }

        /// <summary>
        /// Renders the time and date based on specified date and culture using the kernel config (long or short)
        /// </summary>
        /// <param name="DT">Specified time and date</param>
        /// <param name="Cult">A culture</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A time and date</returns>
        public static string RenderUtc(DateTime DT, CultureInfo Cult, TimeDate.FormatType FormatType)
        {
            if (FormatType == TimeDate.FormatType.Long)
            {
                return DT.ToUniversalTime().ToString(Cult.DateTimeFormat.FullDateTimePattern, Cult);
            }
            else
            {
                return DT.ToUniversalTime().ToString(Cult.DateTimeFormat.ShortDatePattern, Cult) + " - " + DT.ToUniversalTime().ToString(Cult.DateTimeFormat.ShortTimePattern, Cult);
            }
        }

    }
}
