
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
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
    /// Time and date rendering module
    /// </summary>
    public static class TimeDateRenderers
    {

        /// <summary>
        /// Renders the current time based on kernel config (long or short) and current culture
        /// </summary>
        /// <returns>A long or short time</returns>
        public static string RenderTime()
        {
            if (Flags.LongTimeDate)
            {
                return TimeDate.KernelDateTime.ToString(CultureManager.CurrentCult.DateTimeFormat.LongTimePattern, CultureManager.CurrentCult);
            }
            else
            {
                return TimeDate.KernelDateTime.ToString(CultureManager.CurrentCult.DateTimeFormat.ShortTimePattern, CultureManager.CurrentCult);
            }
        }

        /// <summary>
        /// Renders the current time based on kernel config (long or short) and current culture
        /// </summary>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A long or short time</returns>
        public static string RenderTime(TimeDate.FormatType FormatType)
        {
            if (FormatType == TimeDate.FormatType.Long)
            {
                return TimeDate.KernelDateTime.ToString(CultureManager.CurrentCult.DateTimeFormat.LongTimePattern, CultureManager.CurrentCult);
            }
            else
            {
                return TimeDate.KernelDateTime.ToString(CultureManager.CurrentCult.DateTimeFormat.ShortTimePattern, CultureManager.CurrentCult);
            }
        }

        /// <summary>
        /// Renders the current time based on specified culture
        /// </summary>
        /// <param name="Cult">A culture.</param>
        /// <returns>A time</returns>
        public static string RenderTime(CultureInfo Cult)
        {
            if (Flags.LongTimeDate)
            {
                return TimeDate.KernelDateTime.ToString(Cult.DateTimeFormat.LongTimePattern, Cult);
            }
            else
            {
                return TimeDate.KernelDateTime.ToString(Cult.DateTimeFormat.ShortTimePattern, Cult);
            }
        }

        /// <summary>
        /// Renders the current time based on specified culture
        /// </summary>
        /// <param name="Cult">A culture.</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A time</returns>
        public static string RenderTime(CultureInfo Cult, TimeDate.FormatType FormatType)
        {
            if (FormatType == TimeDate.FormatType.Long)
            {
                return TimeDate.KernelDateTime.ToString(Cult.DateTimeFormat.LongTimePattern, Cult);
            }
            else
            {
                return TimeDate.KernelDateTime.ToString(Cult.DateTimeFormat.ShortTimePattern, Cult);
            }
        }

        /// <summary>
        /// Renders the time based on specified time using the kernel config (long or short) and current culture
        /// </summary>
        /// <param name="DT">Specified time</param>
        /// <returns>A long or short time</returns>
        public static string RenderTime(DateTime DT)
        {
            if (Flags.LongTimeDate)
            {
                return DT.ToString(CultureManager.CurrentCult.DateTimeFormat.LongTimePattern, CultureManager.CurrentCult);
            }
            else
            {
                return DT.ToString(CultureManager.CurrentCult.DateTimeFormat.ShortTimePattern, CultureManager.CurrentCult);
            }
        }

        /// <summary>
        /// Renders the time based on specified time using the kernel config (long or short) and current culture
        /// </summary>
        /// <param name="DT">Specified time</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A long or short time</returns>
        public static string RenderTime(DateTime DT, TimeDate.FormatType FormatType)
        {
            if (FormatType == TimeDate.FormatType.Long)
            {
                return DT.ToString(CultureManager.CurrentCult.DateTimeFormat.LongTimePattern, CultureManager.CurrentCult);
            }
            else
            {
                return DT.ToString(CultureManager.CurrentCult.DateTimeFormat.ShortTimePattern, CultureManager.CurrentCult);
            }
        }

        /// <summary>
        /// Renders the time based on specified date and culture using the kernel config (long or short)
        /// </summary>
        /// <param name="DT">Specified time</param>
        /// <param name="Cult">A culture</param>
        /// <returns>A time</returns>
        public static string RenderTime(DateTime DT, CultureInfo Cult)
        {
            if (Flags.LongTimeDate)
            {
                return DT.ToString(Cult.DateTimeFormat.LongTimePattern, Cult);
            }
            else
            {
                return DT.ToString(Cult.DateTimeFormat.ShortTimePattern, Cult);
            }
        }

        /// <summary>
        /// Renders the time based on specified date and culture using the kernel config (long or short)
        /// </summary>
        /// <param name="DT">Specified time</param>
        /// <param name="Cult">A culture</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A time</returns>
        public static string RenderTime(DateTime DT, CultureInfo Cult, TimeDate.FormatType FormatType)
        {
            if (FormatType == TimeDate.FormatType.Long)
            {
                return DT.ToString(Cult.DateTimeFormat.LongTimePattern, Cult);
            }
            else
            {
                return DT.ToString(Cult.DateTimeFormat.ShortTimePattern, Cult);
            }
        }

        /// <summary>
        /// Renders the current date based on kernel config (long or short) and current culture
        /// </summary>
        /// <returns>A long or short date</returns>
        public static string RenderDate()
        {
            if (Flags.LongTimeDate)
            {
                return TimeDate.KernelDateTime.ToString(CultureManager.CurrentCult.DateTimeFormat.LongDatePattern, CultureManager.CurrentCult);
            }
            else
            {
                return TimeDate.KernelDateTime.ToString(CultureManager.CurrentCult.DateTimeFormat.ShortDatePattern, CultureManager.CurrentCult);
            }
        }

        /// <summary>
        /// Renders the current date based on kernel config (long or short) and current culture
        /// </summary>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A long or short date</returns>
        public static string RenderDate(TimeDate.FormatType FormatType)
        {
            if (FormatType == TimeDate.FormatType.Long)
            {
                return TimeDate.KernelDateTime.ToString(CultureManager.CurrentCult.DateTimeFormat.LongDatePattern, CultureManager.CurrentCult);
            }
            else
            {
                return TimeDate.KernelDateTime.ToString(CultureManager.CurrentCult.DateTimeFormat.ShortDatePattern, CultureManager.CurrentCult);
            }
        }

        /// <summary>
        /// Renders the current date based on specified culture
        /// </summary>
        /// <param name="Cult">A culture.</param>
        /// <returns>A date</returns>
        public static string RenderDate(CultureInfo Cult)
        {
            if (Flags.LongTimeDate)
            {
                return TimeDate.KernelDateTime.ToString(Cult.DateTimeFormat.LongDatePattern, Cult);
            }
            else
            {
                return TimeDate.KernelDateTime.ToString(Cult.DateTimeFormat.ShortDatePattern, Cult);
            }
        }

        /// <summary>
        /// Renders the current date based on specified culture
        /// </summary>
        /// <param name="Cult">A culture.</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A date</returns>
        public static string RenderDate(CultureInfo Cult, TimeDate.FormatType FormatType)
        {
            if (FormatType == TimeDate.FormatType.Long)
            {
                return TimeDate.KernelDateTime.ToString(Cult.DateTimeFormat.LongDatePattern, Cult);
            }
            else
            {
                return TimeDate.KernelDateTime.ToString(Cult.DateTimeFormat.ShortDatePattern, Cult);
            }
        }

        /// <summary>
        /// Renders the date based on specified date using the kernel config (long or short) and current culture
        /// </summary>
        /// <param name="DT">Specified date</param>
        /// <returns>A long or short date</returns>
        public static string RenderDate(DateTime DT)
        {
            if (Flags.LongTimeDate)
            {
                return DT.ToString(CultureManager.CurrentCult.DateTimeFormat.LongDatePattern, CultureManager.CurrentCult);
            }
            else
            {
                return DT.ToString(CultureManager.CurrentCult.DateTimeFormat.ShortDatePattern, CultureManager.CurrentCult);
            }
        }

        /// <summary>
        /// Renders the date based on specified date using the kernel config (long or short) and current culture
        /// </summary>
        /// <param name="DT">Specified date</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A long or short date</returns>
        public static string RenderDate(DateTime DT, TimeDate.FormatType FormatType)
        {
            if (FormatType == TimeDate.FormatType.Long)
            {
                return DT.ToString(CultureManager.CurrentCult.DateTimeFormat.LongDatePattern, CultureManager.CurrentCult);
            }
            else
            {
                return DT.ToString(CultureManager.CurrentCult.DateTimeFormat.ShortDatePattern, CultureManager.CurrentCult);
            }
        }

        /// <summary>
        /// Renders the date based on specified date and culture using the kernel config (long or short)
        /// </summary>
        /// <param name="DT">Specified date</param>
        /// <param name="Cult">A culture</param>
        /// <returns>A date</returns>
        public static string RenderDate(DateTime DT, CultureInfo Cult)
        {
            if (Flags.LongTimeDate)
            {
                return DT.ToString(Cult.DateTimeFormat.LongDatePattern, Cult);
            }
            else
            {
                return DT.ToString(Cult.DateTimeFormat.ShortDatePattern, Cult);
            }
        }

        /// <summary>
        /// Renders the date based on specified date and culture using the kernel config (long or short)
        /// </summary>
        /// <param name="DT">Specified date</param>
        /// <param name="Cult">A culture</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A date</returns>
        public static string RenderDate(DateTime DT, CultureInfo Cult, TimeDate.FormatType FormatType)
        {
            if (FormatType == TimeDate.FormatType.Long)
            {
                return DT.ToString(Cult.DateTimeFormat.LongDatePattern, Cult);
            }
            else
            {
                return DT.ToString(Cult.DateTimeFormat.ShortDatePattern, Cult);
            }
        }

        /// <summary>
        /// Renders the current time and date based on kernel config (long or short) and current culture
        /// </summary>
        /// <returns>A long or short time and date</returns>
        public static string Render()
        {
            if (Flags.LongTimeDate)
            {
                return TimeDate.KernelDateTime.ToString(CultureManager.CurrentCult.DateTimeFormat.FullDateTimePattern, CultureManager.CurrentCult);
            }
            else
            {
                return TimeDate.KernelDateTime.ToString(CultureManager.CurrentCult.DateTimeFormat.ShortDatePattern, CultureManager.CurrentCult) + " - " + TimeDate.KernelDateTime.ToString(CultureManager.CurrentCult.DateTimeFormat.ShortTimePattern, CultureManager.CurrentCult);
            }
        }

        /// <summary>
        /// Renders the current time and date based on kernel config (long or short) and current culture
        /// </summary>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A long or short time and date</returns>
        public static string Render(TimeDate.FormatType FormatType)
        {
            if (FormatType == TimeDate.FormatType.Long)
            {
                return TimeDate.KernelDateTime.ToString(CultureManager.CurrentCult.DateTimeFormat.FullDateTimePattern, CultureManager.CurrentCult);
            }
            else
            {
                return TimeDate.KernelDateTime.ToString(CultureManager.CurrentCult.DateTimeFormat.ShortDatePattern, CultureManager.CurrentCult) + " - " + TimeDate.KernelDateTime.ToString(CultureManager.CurrentCult.DateTimeFormat.ShortTimePattern, CultureManager.CurrentCult);
            }
        }

        /// <summary>
        /// Renders the current time and date based on specified culture
        /// </summary>
        /// <param name="Cult">A culture.</param>
        /// <returns>A time and date</returns>
        public static string Render(CultureInfo Cult)
        {
            if (Flags.LongTimeDate)
            {
                return TimeDate.KernelDateTime.ToString(Cult.DateTimeFormat.FullDateTimePattern, Cult);
            }
            else
            {
                return TimeDate.KernelDateTime.ToString(Cult.DateTimeFormat.ShortDatePattern, Cult) + " - " + TimeDate.KernelDateTime.ToString(Cult.DateTimeFormat.ShortTimePattern, Cult);
            }
        }

        /// <summary>
        /// Renders the current time and date based on specified culture
        /// </summary>
        /// <param name="Cult">A culture.</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A time and date</returns>
        public static string Render(CultureInfo Cult, TimeDate.FormatType FormatType)
        {
            if (FormatType == TimeDate.FormatType.Long)
            {
                return TimeDate.KernelDateTime.ToString(Cult.DateTimeFormat.FullDateTimePattern, Cult);
            }
            else
            {
                return TimeDate.KernelDateTime.ToString(Cult.DateTimeFormat.ShortDatePattern, Cult) + " - " + TimeDate.KernelDateTime.ToString(Cult.DateTimeFormat.ShortTimePattern, Cult);
            }
        }

        /// <summary>
        /// Renders the time and date based on specified time using the kernel config (long or short) and current culture
        /// </summary>
        /// <param name="DT">Specified time and date</param>
        /// <returns>A long or short time and date</returns>
        public static string Render(DateTime DT)
        {
            if (Flags.LongTimeDate)
            {
                return DT.ToString(CultureManager.CurrentCult.DateTimeFormat.FullDateTimePattern, CultureManager.CurrentCult);
            }
            else
            {
                return DT.ToString(CultureManager.CurrentCult.DateTimeFormat.ShortDatePattern, CultureManager.CurrentCult) + " - " + DT.ToString(CultureManager.CurrentCult.DateTimeFormat.ShortTimePattern, CultureManager.CurrentCult);
            }
        }

        /// <summary>
        /// Renders the time and date based on specified time using the kernel config (long or short) and current culture
        /// </summary>
        /// <param name="DT">Specified time and date</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A long or short time and date</returns>
        public static string Render(DateTime DT, TimeDate.FormatType FormatType)
        {
            if (FormatType == TimeDate.FormatType.Long)
            {
                return DT.ToString(CultureManager.CurrentCult.DateTimeFormat.FullDateTimePattern, CultureManager.CurrentCult);
            }
            else
            {
                return DT.ToString(CultureManager.CurrentCult.DateTimeFormat.ShortDatePattern, CultureManager.CurrentCult) + " - " + DT.ToString(CultureManager.CurrentCult.DateTimeFormat.ShortTimePattern, CultureManager.CurrentCult);
            }
        }

        /// <summary>
        /// Renders the time and date based on specified date and culture using the kernel config (long or short)
        /// </summary>
        /// <param name="DT">Specified time and date</param>
        /// <param name="Cult">A culture</param>
        /// <returns>A time and date</returns>
        public static string Render(DateTime DT, CultureInfo Cult)
        {
            if (Flags.LongTimeDate)
            {
                return DT.ToString(Cult.DateTimeFormat.FullDateTimePattern, Cult);
            }
            else
            {
                return DT.ToString(Cult.DateTimeFormat.ShortDatePattern, Cult) + " - " + DT.ToString(Cult.DateTimeFormat.ShortTimePattern, Cult);
            }
        }

        /// <summary>
        /// Renders the time and date based on specified date and culture using the kernel config (long or short)
        /// </summary>
        /// <param name="DT">Specified time and date</param>
        /// <param name="Cult">A culture</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A time and date</returns>
        public static string Render(DateTime DT, CultureInfo Cult, TimeDate.FormatType FormatType)
        {
            if (FormatType == TimeDate.FormatType.Long)
            {
                return DT.ToString(Cult.DateTimeFormat.FullDateTimePattern, Cult);
            }
            else
            {
                return DT.ToString(Cult.DateTimeFormat.ShortDatePattern, Cult) + " - " + DT.ToString(Cult.DateTimeFormat.ShortTimePattern, Cult);
            }
        }

    }
}
