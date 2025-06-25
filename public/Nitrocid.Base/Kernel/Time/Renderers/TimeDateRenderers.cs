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

using System;
using System.Globalization;
using Nitrocid.Base.Kernel.Configuration;
using Terminaux.Writer.CyclicWriters.Graphical.Calendaring;
using Nitrocid.Base.Languages;

namespace Nitrocid.Base.Kernel.Time.Renderers
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
        public static string RenderTime() =>
            Config.MainConfig.LongTimeDate
            ? TimeDateTools.KernelDateTime.ToString(CultureManager.CurrentCulture.DateTimeFormat.LongTimePattern, CultureManager.CurrentCulture)
            : TimeDateTools.KernelDateTime.ToString(CultureManager.CurrentCulture.DateTimeFormat.ShortTimePattern, CultureManager.CurrentCulture);

        /// <summary>
        /// Renders the current time based on kernel config (long or short) and current culture
        /// </summary>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A long or short time</returns>
        public static string RenderTime(FormatType FormatType) =>
            FormatType == FormatType.Long
            ? TimeDateTools.KernelDateTime.ToString(CultureManager.CurrentCulture.DateTimeFormat.LongTimePattern, CultureManager.CurrentCulture)
            : TimeDateTools.KernelDateTime.ToString(CultureManager.CurrentCulture.DateTimeFormat.ShortTimePattern, CultureManager.CurrentCulture);

        /// <summary>
        /// Renders the current time based on specified culture
        /// </summary>
        /// <param name="Cult">A culture.</param>
        /// <returns>A time</returns>
        public static string RenderTime(CultureInfo Cult) =>
            Config.MainConfig.LongTimeDate
            ? TimeDateTools.KernelDateTime.ToString(Cult.DateTimeFormat.LongTimePattern, Cult)
            : TimeDateTools.KernelDateTime.ToString(Cult.DateTimeFormat.ShortTimePattern, Cult);

        /// <summary>
        /// Renders the current time based on specified culture
        /// </summary>
        /// <param name="Cult">A culture.</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A time</returns>
        public static string RenderTime(CultureInfo Cult, FormatType FormatType) =>
            FormatType == FormatType.Long
            ? TimeDateTools.KernelDateTime.ToString(Cult.DateTimeFormat.LongTimePattern, Cult)
            : TimeDateTools.KernelDateTime.ToString(Cult.DateTimeFormat.ShortTimePattern, Cult);

        /// <summary>
        /// Renders the current time based on specified culture
        /// </summary>
        /// <param name="calendar">A base calendar</param>
        /// <returns>A time</returns>
        public static string RenderTime(BaseCalendar calendar) =>
            Config.MainConfig.LongTimeDate
            ? TimeDateTools.KernelDateTime.ToString(calendar.Culture.DateTimeFormat.LongTimePattern, calendar.Culture)
            : TimeDateTools.KernelDateTime.ToString(calendar.Culture.DateTimeFormat.ShortTimePattern, calendar.Culture);

        /// <summary>
        /// Renders the current time based on specified culture
        /// </summary>
        /// <param name="calendar">A base calendar</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A time</returns>
        public static string RenderTime(BaseCalendar calendar, FormatType FormatType) =>
            FormatType == FormatType.Long
            ? TimeDateTools.KernelDateTime.ToString(calendar.Culture.DateTimeFormat.LongTimePattern, calendar.Culture)
            : TimeDateTools.KernelDateTime.ToString(calendar.Culture.DateTimeFormat.ShortTimePattern, calendar.Culture);

        /// <summary>
        /// Renders the time based on specified time using the kernel config (long or short) and current culture
        /// </summary>
        /// <param name="DT">Specified time</param>
        /// <returns>A long or short time</returns>
        public static string RenderTime(DateTime DT) =>
            Config.MainConfig.LongTimeDate
            ? DT.ToString(CultureManager.CurrentCulture.DateTimeFormat.LongTimePattern, CultureManager.CurrentCulture)
            : DT.ToString(CultureManager.CurrentCulture.DateTimeFormat.ShortTimePattern, CultureManager.CurrentCulture);

        /// <summary>
        /// Renders the time based on specified time using the kernel config (long or short) and current culture
        /// </summary>
        /// <param name="DT">Specified time</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A long or short time</returns>
        public static string RenderTime(DateTime DT, FormatType FormatType) =>
            FormatType == FormatType.Long
            ? DT.ToString(CultureManager.CurrentCulture.DateTimeFormat.LongTimePattern, CultureManager.CurrentCulture)
            : DT.ToString(CultureManager.CurrentCulture.DateTimeFormat.ShortTimePattern, CultureManager.CurrentCulture);

        /// <summary>
        /// Renders the time based on specified date and culture using the kernel config (long or short)
        /// </summary>
        /// <param name="DT">Specified time</param>
        /// <param name="Cult">A culture</param>
        /// <returns>A time</returns>
        public static string RenderTime(DateTime DT, CultureInfo Cult) =>
            Config.MainConfig.LongTimeDate
            ? DT.ToString(Cult.DateTimeFormat.LongTimePattern, Cult)
            : DT.ToString(Cult.DateTimeFormat.ShortTimePattern, Cult);

        /// <summary>
        /// Renders the time based on specified date and culture using the kernel config (long or short)
        /// </summary>
        /// <param name="DT">Specified time</param>
        /// <param name="Cult">A culture</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A time</returns>
        public static string RenderTime(DateTime DT, CultureInfo Cult, FormatType FormatType) =>
            FormatType == FormatType.Long
            ? DT.ToString(Cult.DateTimeFormat.LongTimePattern, Cult)
            : DT.ToString(Cult.DateTimeFormat.ShortTimePattern, Cult);

        /// <summary>
        /// Renders the time based on specified date and culture using the kernel config (long or short)
        /// </summary>
        /// <param name="DT">Specified time</param>
        /// <param name="calendar">A base calendar</param>
        /// <returns>A time</returns>
        public static string RenderTime(DateTime DT, BaseCalendar calendar) =>
            Config.MainConfig.LongTimeDate
            ? DT.ToString(calendar.Culture.DateTimeFormat.LongTimePattern, calendar.Culture)
            : DT.ToString(calendar.Culture.DateTimeFormat.ShortTimePattern, calendar.Culture);

        /// <summary>
        /// Renders the time based on specified date and culture using the kernel config (long or short)
        /// </summary>
        /// <param name="DT">Specified time</param>
        /// <param name="calendar">A base calendar</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A time</returns>
        public static string RenderTime(DateTime DT, BaseCalendar calendar, FormatType FormatType) =>
            FormatType == FormatType.Long
            ? DT.ToString(calendar.Culture.DateTimeFormat.LongTimePattern, calendar.Culture)
            : DT.ToString(calendar.Culture.DateTimeFormat.ShortTimePattern, calendar.Culture);

        /// <summary>
        /// Renders the current date based on kernel config (long or short) and current culture
        /// </summary>
        /// <returns>A long or short date</returns>
        public static string RenderDate() =>
            Config.MainConfig.LongTimeDate
            ? TimeDateTools.KernelDateTime.ToString(CultureManager.CurrentCulture.DateTimeFormat.LongDatePattern, CultureManager.CurrentCulture)
            : TimeDateTools.KernelDateTime.ToString(CultureManager.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureManager.CurrentCulture);

        /// <summary>
        /// Renders the current date based on kernel config (long or short) and current culture
        /// </summary>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A long or short date</returns>
        public static string RenderDate(FormatType FormatType) =>
            FormatType == FormatType.Long
            ? TimeDateTools.KernelDateTime.ToString(CultureManager.CurrentCulture.DateTimeFormat.LongDatePattern, CultureManager.CurrentCulture)
            : TimeDateTools.KernelDateTime.ToString(CultureManager.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureManager.CurrentCulture);

        /// <summary>
        /// Renders the current date based on specified culture
        /// </summary>
        /// <param name="Cult">A culture.</param>
        /// <returns>A date</returns>
        public static string RenderDate(CultureInfo Cult) =>
            Config.MainConfig.LongTimeDate
            ? TimeDateTools.KernelDateTime.ToString(Cult.DateTimeFormat.LongDatePattern, Cult)
            : TimeDateTools.KernelDateTime.ToString(Cult.DateTimeFormat.ShortDatePattern, Cult);

        /// <summary>
        /// Renders the current date based on specified culture
        /// </summary>
        /// <param name="Cult">A culture.</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A date</returns>
        public static string RenderDate(CultureInfo Cult, FormatType FormatType) =>
            FormatType == FormatType.Long
            ? TimeDateTools.KernelDateTime.ToString(Cult.DateTimeFormat.LongDatePattern, Cult)
            : TimeDateTools.KernelDateTime.ToString(Cult.DateTimeFormat.ShortDatePattern, Cult);

        /// <summary>
        /// Renders the current date based on specified culture
        /// </summary>
        /// <param name="calendar">A base calendar</param>
        /// <returns>A date</returns>
        public static string RenderDate(BaseCalendar calendar) =>
            Config.MainConfig.LongTimeDate
            ? TimeDateTools.KernelDateTime.ToString(calendar.Culture.DateTimeFormat.LongDatePattern, calendar.Culture)
            : TimeDateTools.KernelDateTime.ToString(calendar.Culture.DateTimeFormat.ShortDatePattern, calendar.Culture);

        /// <summary>
        /// Renders the current date based on specified culture
        /// </summary>
        /// <param name="calendar">A base calendar</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A date</returns>
        public static string RenderDate(BaseCalendar calendar, FormatType FormatType) =>
            FormatType == FormatType.Long
            ? TimeDateTools.KernelDateTime.ToString(calendar.Culture.DateTimeFormat.LongDatePattern, calendar.Culture)
            : TimeDateTools.KernelDateTime.ToString(calendar.Culture.DateTimeFormat.ShortDatePattern, calendar.Culture);

        /// <summary>
        /// Renders the date based on specified date using the kernel config (long or short) and current culture
        /// </summary>
        /// <param name="DT">Specified date</param>
        /// <returns>A long or short date</returns>
        public static string RenderDate(DateTime DT) =>
            Config.MainConfig.LongTimeDate
            ? DT.ToString(CultureManager.CurrentCulture.DateTimeFormat.LongDatePattern, CultureManager.CurrentCulture)
            : DT.ToString(CultureManager.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureManager.CurrentCulture);

        /// <summary>
        /// Renders the date based on specified date using the kernel config (long or short) and current culture
        /// </summary>
        /// <param name="DT">Specified date</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A long or short date</returns>
        public static string RenderDate(DateTime DT, FormatType FormatType) =>
            FormatType == FormatType.Long
            ? DT.ToString(CultureManager.CurrentCulture.DateTimeFormat.LongDatePattern, CultureManager.CurrentCulture)
            : DT.ToString(CultureManager.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureManager.CurrentCulture);

        /// <summary>
        /// Renders the date based on specified date and culture using the kernel config (long or short)
        /// </summary>
        /// <param name="DT">Specified date</param>
        /// <param name="Cult">A culture</param>
        /// <returns>A date</returns>
        public static string RenderDate(DateTime DT, CultureInfo Cult) =>
            Config.MainConfig.LongTimeDate
            ? DT.ToString(Cult.DateTimeFormat.LongDatePattern, Cult)
            : DT.ToString(Cult.DateTimeFormat.ShortDatePattern, Cult);

        /// <summary>
        /// Renders the date based on specified date and culture using the kernel config (long or short)
        /// </summary>
        /// <param name="DT">Specified date</param>
        /// <param name="Cult">A culture</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A date</returns>
        public static string RenderDate(DateTime DT, CultureInfo Cult, FormatType FormatType) =>
            FormatType == FormatType.Long
            ? DT.ToString(Cult.DateTimeFormat.LongDatePattern, Cult)
            : DT.ToString(Cult.DateTimeFormat.ShortDatePattern, Cult);

        /// <summary>
        /// Renders the date based on specified date and culture using the kernel config (long or short)
        /// </summary>
        /// <param name="DT">Specified date</param>
        /// <param name="calendar">A base calendar</param>
        /// <returns>A date</returns>
        public static string RenderDate(DateTime DT, BaseCalendar calendar) =>
            Config.MainConfig.LongTimeDate
            ? DT.ToString(calendar.Culture.DateTimeFormat.LongDatePattern, calendar.Culture)
            : DT.ToString(calendar.Culture.DateTimeFormat.ShortDatePattern, calendar.Culture);

        /// <summary>
        /// Renders the date based on specified date and culture using the kernel config (long or short)
        /// </summary>
        /// <param name="DT">Specified date</param>
        /// <param name="calendar">A base calendar</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A date</returns>
        public static string RenderDate(DateTime DT, BaseCalendar calendar, FormatType FormatType) =>
            FormatType == FormatType.Long
            ? DT.ToString(calendar.Culture.DateTimeFormat.LongDatePattern, calendar.Culture)
            : DT.ToString(calendar.Culture.DateTimeFormat.ShortDatePattern, calendar.Culture);

        /// <summary>
        /// Renders the current time and date based on kernel config (long or short) and current culture
        /// </summary>
        /// <returns>A long or short time and date</returns>
        public static string Render() =>
            Config.MainConfig.LongTimeDate
            ? TimeDateTools.KernelDateTime.ToString(CultureManager.CurrentCulture.DateTimeFormat.FullDateTimePattern, CultureManager.CurrentCulture)
            : TimeDateTools.KernelDateTime.ToString(CultureManager.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureManager.CurrentCulture) + " - " + TimeDateTools.KernelDateTime.ToString(CultureManager.CurrentCulture.DateTimeFormat.ShortTimePattern, CultureManager.CurrentCulture);

        /// <summary>
        /// Renders the current time and date based on kernel config (long or short) and current culture
        /// </summary>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A long or short time and date</returns>
        public static string Render(FormatType FormatType) =>
            FormatType == FormatType.Long
            ? TimeDateTools.KernelDateTime.ToString(CultureManager.CurrentCulture.DateTimeFormat.FullDateTimePattern, CultureManager.CurrentCulture)
            : TimeDateTools.KernelDateTime.ToString(CultureManager.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureManager.CurrentCulture) + " - " + TimeDateTools.KernelDateTime.ToString(CultureManager.CurrentCulture.DateTimeFormat.ShortTimePattern, CultureManager.CurrentCulture);

        /// <summary>
        /// Renders the current time and date based on specified culture
        /// </summary>
        /// <param name="Cult">A culture.</param>
        /// <returns>A time and date</returns>
        public static string Render(CultureInfo Cult) =>
            Config.MainConfig.LongTimeDate
            ? TimeDateTools.KernelDateTime.ToString(Cult.DateTimeFormat.FullDateTimePattern, Cult)
            : TimeDateTools.KernelDateTime.ToString(Cult.DateTimeFormat.ShortDatePattern, Cult) + " - " + TimeDateTools.KernelDateTime.ToString(Cult.DateTimeFormat.ShortTimePattern, Cult);

        /// <summary>
        /// Renders the current time and date based on specified culture
        /// </summary>
        /// <param name="calendar">A base calendar</param>
        /// <returns>A time and date</returns>
        public static string Render(BaseCalendar calendar) =>
            Config.MainConfig.LongTimeDate
            ? TimeDateTools.KernelDateTime.ToString(calendar.Culture.DateTimeFormat.FullDateTimePattern, calendar.Culture)
            : TimeDateTools.KernelDateTime.ToString(calendar.Culture.DateTimeFormat.ShortDatePattern, calendar.Culture) + " - " + TimeDateTools.KernelDateTime.ToString(calendar.Culture.DateTimeFormat.ShortTimePattern, calendar.Culture);

        /// <summary>
        /// Renders the current time and date based on specified culture
        /// </summary>
        /// <param name="Cult">A culture</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A time and date</returns>
        public static string Render(CultureInfo Cult, FormatType FormatType) =>
            FormatType == FormatType.Long
            ? TimeDateTools.KernelDateTime.ToString(Cult.DateTimeFormat.FullDateTimePattern, Cult)
            : TimeDateTools.KernelDateTime.ToString(Cult.DateTimeFormat.ShortDatePattern, Cult) + " - " + TimeDateTools.KernelDateTime.ToString(Cult.DateTimeFormat.ShortTimePattern, Cult);

        /// <summary>
        /// Renders the current time and date based on specified culture
        /// </summary>
        /// <param name="calendar">A base calendar</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A time and date</returns>
        public static string Render(BaseCalendar calendar, FormatType FormatType) =>
            FormatType == FormatType.Long
            ? TimeDateTools.KernelDateTime.ToString(calendar.Culture.DateTimeFormat.FullDateTimePattern, calendar.Culture)
            : TimeDateTools.KernelDateTime.ToString(calendar.Culture.DateTimeFormat.ShortDatePattern, calendar.Culture) + " - " + TimeDateTools.KernelDateTime.ToString(calendar.Culture.DateTimeFormat.ShortTimePattern, calendar.Culture);

        /// <summary>
        /// Renders the current time based on specified custom format
        /// </summary>
        /// <param name="CustomFormat">A custom format for rendering the time</param>
        /// <returns>A time</returns>
        public static string Render(string CustomFormat) =>
            TimeDateTools.KernelDateTime.ToString(CustomFormat, CultureManager.CurrentCulture);

        /// <summary>
        /// Renders the current time based on specified culture and custom format
        /// </summary>
        /// <param name="Cult">A culture.</param>
        /// <param name="CustomFormat">A custom format for rendering the time</param>
        /// <returns>A time</returns>
        public static string Render(CultureInfo Cult, string CustomFormat) =>
            TimeDateTools.KernelDateTime.ToString(CustomFormat, Cult);

        /// <summary>
        /// Renders the current time based on specified culture and custom format
        /// </summary>
        /// <param name="calendar">A base calendar</param>
        /// <param name="CustomFormat">A custom format for rendering the time</param>
        /// <returns>A time</returns>
        public static string Render(BaseCalendar calendar, string CustomFormat) =>
            TimeDateTools.KernelDateTime.ToString(CustomFormat, calendar.Culture);

        /// <summary>
        /// Renders the time and date based on specified time using the kernel config (long or short) and current culture
        /// </summary>
        /// <param name="DT">Specified time and date</param>
        /// <returns>A long or short time and date</returns>
        public static string Render(DateTime DT) =>
            Config.MainConfig.LongTimeDate
            ? DT.ToString(CultureManager.CurrentCulture.DateTimeFormat.FullDateTimePattern, CultureManager.CurrentCulture)
            : DT.ToString(CultureManager.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureManager.CurrentCulture) + " - " + DT.ToString(CultureManager.CurrentCulture.DateTimeFormat.ShortTimePattern, CultureManager.CurrentCulture);

        /// <summary>
        /// Renders the time and date based on specified time using the kernel config (long or short) and current culture
        /// </summary>
        /// <param name="DT">Specified time and date</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A long or short time and date</returns>
        public static string Render(DateTime DT, FormatType FormatType) =>
            FormatType == FormatType.Long
            ? DT.ToString(CultureManager.CurrentCulture.DateTimeFormat.FullDateTimePattern, CultureManager.CurrentCulture)
            : DT.ToString(CultureManager.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureManager.CurrentCulture) + " - " + DT.ToString(CultureManager.CurrentCulture.DateTimeFormat.ShortTimePattern, CultureManager.CurrentCulture);

        /// <summary>
        /// Renders the time and date based on specified date and culture using the kernel config (long or short)
        /// </summary>
        /// <param name="DT">Specified time and date</param>
        /// <param name="Cult">A culture</param>
        /// <returns>A time and date</returns>
        public static string Render(DateTime DT, CultureInfo Cult) =>
            Config.MainConfig.LongTimeDate
            ? DT.ToString(Cult.DateTimeFormat.FullDateTimePattern, Cult)
            : DT.ToString(Cult.DateTimeFormat.ShortDatePattern, Cult) + " - " + DT.ToString(Cult.DateTimeFormat.ShortTimePattern, Cult);

        /// <summary>
        /// Renders the time and date based on specified date and culture using the kernel config (long or short)
        /// </summary>
        /// <param name="DT">Specified time and date</param>
        /// <param name="Cult">A culture</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A time and date</returns>
        public static string Render(DateTime DT, CultureInfo Cult, FormatType FormatType) =>
            FormatType == FormatType.Long
            ? DT.ToString(Cult.DateTimeFormat.FullDateTimePattern, Cult)
            : DT.ToString(Cult.DateTimeFormat.ShortDatePattern, Cult) + " - " + DT.ToString(Cult.DateTimeFormat.ShortTimePattern, Cult);

        /// <summary>
        /// Renders the time and date based on specified date and culture using the kernel config (long or short)
        /// </summary>
        /// <param name="DT">Specified time and date</param>
        /// <param name="calendar">A base calendar</param>
        /// <returns>A time and date</returns>
        public static string Render(DateTime DT, BaseCalendar calendar) =>
            Config.MainConfig.LongTimeDate
            ? DT.ToString(calendar.Culture.DateTimeFormat.FullDateTimePattern, calendar.Culture)
            : DT.ToString(calendar.Culture.DateTimeFormat.ShortDatePattern, calendar.Culture) + " - " + DT.ToString(calendar.Culture.DateTimeFormat.ShortTimePattern, calendar.Culture);

        /// <summary>
        /// Renders the time and date based on specified date and culture using the kernel config (long or short)
        /// </summary>
        /// <param name="DT">Specified time and date</param>
        /// <param name="calendar">A base calendar</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A time and date</returns>
        public static string Render(DateTime DT, BaseCalendar calendar, FormatType FormatType) =>
            FormatType == FormatType.Long
            ? DT.ToString(calendar.Culture.DateTimeFormat.FullDateTimePattern, calendar.Culture)
            : DT.ToString(calendar.Culture.DateTimeFormat.ShortDatePattern, calendar.Culture) + " - " + DT.ToString(calendar.Culture.DateTimeFormat.ShortTimePattern, calendar.Culture);

        /// <summary>
        /// Renders the current time based on current culture using the custom format
        /// </summary>
        /// <param name="DT">Specified time and date</param>
        /// <param name="CustomFormat">A custom format for rendering the time</param>
        /// <returns>A time</returns>
        public static string Render(DateTime DT, string CustomFormat) =>
            DT.ToString(CustomFormat, CultureManager.CurrentCulture);

        /// <summary>
        /// Renders the current time based on specified date and culture using the custom format
        /// </summary>
        /// <param name="DT">Specified time and date</param>
        /// <param name="Cult">A culture</param>
        /// <param name="CustomFormat">A custom format for rendering the time</param>
        /// <returns>A time</returns>
        public static string Render(DateTime DT, CultureInfo Cult, string CustomFormat) =>
            DT.ToString(CustomFormat, Cult);

        /// <summary>
        /// Renders the current time based on specified date and culture using the custom format
        /// </summary>
        /// <param name="DT">Specified time and date</param>
        /// <param name="calendar">A base calendar</param>
        /// <param name="CustomFormat">A custom format for rendering the time</param>
        /// <returns>A time</returns>
        public static string Render(DateTime DT, BaseCalendar calendar, string CustomFormat) =>
            DT.ToString(CustomFormat, calendar.Culture);

    }
}
