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

using System;
using Textify.Data.Figlet;
using Terminaux.Colors;
using Nitrocid.Misc.Screensaver;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Time.Timezones;
using Nitrocid.Drivers.RNG;
using Terminaux.Base;
using Terminaux.Writer.CyclicWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for WorldClock
    /// </summary>
    public class WorldClockDisplay : BaseScreensaver, IScreensaver
    {
        string timeZoneName = "";
        int times;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "WorldClock";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            base.ScreensaverPreparation();
            var timeZone = TimeZones.GetTimeZoneNames();
            timeZoneName = timeZone[RandomDriver.RandomIdx(timeZone.Length)];
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Change color
            var color = ChangeWorldClockColor();

            // Write word and hash
            if (times > ScreensaverPackInit.SaversConfig.WorldClockNextZoneRefreshes)
            {
                times = 0;
                var timeZone = TimeZones.GetTimeZoneNames();
                timeZoneName = timeZone[RandomDriver.RandomIdx(timeZone.Length)];
            }
            string time = TimeZoneRenderers.GetZoneTimeTimeString(timeZoneName);
            string date = TimeZoneRenderers.GetZoneTimeDateString(timeZoneName);
            var figFont = FigletTools.GetFigletFont("small");
            int figHeight = FigletTools.GetFigletHeight(time, figFont) / 2;
            int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
            int hashY = ConsoleWrapper.WindowHeight / 2 + figHeight + 2;
            ConsoleWrapper.Clear();
            var wordText = new AlignedFigletText(figFont)
            {
                Top = consoleY,
                Text = time,
                ForegroundColor = color,
                Settings = new()
                {
                    Alignment = TextAlignment.Middle,
                }
            };
            TextWriterRaw.WriteRaw(wordText.Render());
            TextWriterWhereColor.WriteWhereColor($"{date} @ {timeZoneName}", (int)Math.Round(ConsoleWrapper.WindowWidth / 2d - $"{date} @ {timeZoneName}".Length / 2d), hashY, color);

            // Delay
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.WorldClockDelay);
            times++;
        }

        /// <summary>
        /// Changes the color of word and its hash
        /// </summary>
        private Color ChangeWorldClockColor()
        {
            Color ColorInstance;
            if (ScreensaverPackInit.SaversConfig.WorldClockTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.WorldClockMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.WorldClockMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.WorldClockMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.WorldClockMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.WorldClockMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.WorldClockMaximumBlueColorLevel);
                ColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.WorldClockMinimumColorLevel, ScreensaverPackInit.SaversConfig.WorldClockMaximumColorLevel);
                ColorInstance = new Color(ColorNum);
            }
            return ColorInstance;
        }

    }
}
