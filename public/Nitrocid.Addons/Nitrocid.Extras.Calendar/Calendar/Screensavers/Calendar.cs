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

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Base;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Languages;
using System.Globalization;
using Nitrocid.Base.Kernel.Time;
using Terminaux.Colors.Themes.Colors;

namespace Nitrocid.Extras.Calendar.Calendar.Screensavers
{
    /// <summary>
    /// Display code for Calendar
    /// </summary>
    public class CalendarDisplay : BaseScreensaver, IScreensaver
    {
        private Color? CalendarColor;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Calendar";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            CalendarColor = null;
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Define the color
            if (CalendarColor is null)
            {
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Defining color...");
                CalendarColor = ChangeCalendarColor();
            }
            if (!ConsoleResizeHandler.WasResized(false))
            {
                var cultures = CultureManager.GetCulturesDictionary();
                var calendar = new FullCalendar()
                {
                    HeaderColor = ThemeColorsTools.GetColor(ThemeColorType.TuiForeground),
                    TodayColor = ThemeColorsTools.GetColor(ThemeColorType.TodayDay),
                    WeekendColor = ThemeColorsTools.GetColor(ThemeColorType.WeekendDay),
                    ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText),
                    Year = TimeDateTools.KernelDateTime.Year,
                    Month = TimeDateTools.KernelDateTime.Month,
                    Left = ConsoleWrapper.WindowWidth / 2 - FullCalendar.calendarWidth / 2,
                    Top = ConsoleWrapper.WindowHeight / 2 - FullCalendar.calendarHeight / 2 - 1,
                    Culture =
                        CalendarInit.CalendarConfig.CalendarUseSystemCulture ?
                        CultureManager.CurrentCulture :
                        cultures.TryGetValue(CalendarInit.CalendarConfig.CalendarCultureName, out CultureInfo? value) ? value :
                        CultureManager.CurrentCulture,
                };
                TextWriterRaw.WriteRaw(calendar.Render());
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(CalendarInit.CalendarConfig.CalendarDelay);
        }

        /// <summary>
        /// Changes the color of calendar
        /// </summary>
        public Color ChangeCalendarColor()
        {
            Color ColorInstance;
            if (CalendarInit.CalendarConfig.CalendarTrueColor)
            {
                int RedColorNum = RandomDriver.Random(CalendarInit.CalendarConfig.CalendarMinimumRedColorLevel, CalendarInit.CalendarConfig.CalendarMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(CalendarInit.CalendarConfig.CalendarMinimumGreenColorLevel, CalendarInit.CalendarConfig.CalendarMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(CalendarInit.CalendarConfig.CalendarMinimumBlueColorLevel, CalendarInit.CalendarConfig.CalendarMaximumBlueColorLevel);
                ColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(CalendarInit.CalendarConfig.CalendarMinimumColorLevel, CalendarInit.CalendarConfig.CalendarMaximumColorLevel);
                ColorInstance = new Color(ColorNum);
            }
            return ColorInstance;
        }
    }
}
