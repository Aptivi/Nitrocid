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

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Base;
using Nitrocid.Kernel.Configuration;
using Terminaux.Colors.Transformation;
using Nitrocid.Languages;
using System.Globalization;
using Nitrocid.Kernel.Time;
using Terminaux.Writer.CyclicWriters.Graphical;

namespace Nitrocid.ScreensaverPacks.Screensavers
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
                var calendar = new Calendars()
                {
                    HeaderColor = CalendarColor,
                    ValueColor = CalendarColor,
                    SeparatorColor = TransformationTools.GetDarkBackground(CalendarColor),
                    Year = TimeDateTools.KernelDateTime.Year,
                    Month = TimeDateTools.KernelDateTime.Month,
                    Left = 2,
                    Top = 1,
                    Width = ConsoleWrapper.WindowWidth - 4,
                    Height = ConsoleWrapper.WindowHeight - 4,
                    Culture =
                        ScreensaverPackInit.SaversConfig.CalendarUseSystemCulture ?
                        CultureManager.CurrentCulture :
                        cultures.TryGetValue(ScreensaverPackInit.SaversConfig.CalendarCultureName, out CultureInfo? value) ? value :
                        CultureManager.CurrentCulture,
                };
                TextWriterRaw.WriteRaw(calendar.Render());
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.CalendarDelay);
        }

        /// <summary>
        /// Changes the color of calendar
        /// </summary>
        public Color ChangeCalendarColor()
        {
            Color ColorInstance;
            if (ScreensaverPackInit.SaversConfig.CalendarTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.CalendarMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.CalendarMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.CalendarMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.CalendarMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.CalendarMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.CalendarMaximumBlueColorLevel);
                ColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.CalendarMinimumColorLevel, ScreensaverPackInit.SaversConfig.CalendarMaximumColorLevel);
                ColorInstance = new Color(ColorNum);
            }
            return ColorInstance;
        }
    }
}
