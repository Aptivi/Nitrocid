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
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Time.Renderers;
using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Base;
using Nitrocid.Base.Kernel.Time;
using Textify.Data.Figlet;
using System;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Simple;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for BlockClock
    /// </summary>
    public class BlockClockDisplay : BaseScreensaver, IScreensaver
    {
        // TODO: NKS_SCREENSAVERPACKS_BLOCKCLOCK_SETTINGS_DESC -> Shows three blocks: one for hours, one for minutes, and one for seconds
        private Color blockColorHours = Color.Empty;
        private Color blockColorMinutes = Color.Empty;
        private Color blockColorSeconds = Color.Empty;
        private Color blockColorInfo = Color.Empty;
        private string lastRenderedDate = "";
        private string lastRenderedTime = "";

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "BlockClock";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Set the colors for blocks
            blockColorHours = ChangeColor();
            blockColorMinutes = ChangeColor();
            blockColorSeconds = ChangeColor();
            blockColorInfo = ChangeColor();

            // Base preparation
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Get the hours, minutes, and seconds
            int hours = TimeDateTools.KernelDateTime.Hour;
            int minutes = TimeDateTools.KernelDateTime.Minute;
            int seconds = TimeDateTools.KernelDateTime.Second;
            string hoursStr = $"{hours:00}";
            string minutesStr = $"{minutes:00}";
            string secondsStr = $"{seconds:00}";

            // Get the figlet and their widths, then get the maximum width
            var figletFont = FigletFonts.GetByName("small");
            int hoursWidth = FigletTools.GetFigletWidth(hoursStr, figletFont);
            int minutesWidth = FigletTools.GetFigletWidth(minutesStr, figletFont);
            int secondsWidth = FigletTools.GetFigletWidth(secondsStr, figletFont);
            int hoursHeight = FigletTools.GetFigletHeight(hoursStr, figletFont);
            int minutesHeight = FigletTools.GetFigletHeight(minutesStr, figletFont);
            int secondsHeight = FigletTools.GetFigletHeight(secondsStr, figletFont);
            int maxWidth = Math.Max(Math.Max(hoursWidth, minutesWidth), secondsWidth);
            int maxHeight = Math.Max(Math.Max(hoursHeight, minutesHeight), secondsHeight);

            // Get the width and position of blocks, and get the total width
            int blockMinutesX = (ConsoleWrapper.WindowWidth / 2) - (maxWidth / 2) - 1;
            int blockHoursX = blockMinutesX - 3 - maxWidth;
            int blockSecondsX = blockMinutesX + maxWidth + 3;
            int blockY = (ConsoleWrapper.WindowHeight / 2) - (maxHeight / 2) - 3;
            int blockPanelWidth = blockSecondsX - blockHoursX + maxWidth + 2;
            
            // Get the X position of the date
            string renderedDate = TimeDateRenderers.RenderDate();
            int datePosX = ConsoleWrapper.WindowWidth / 2 - renderedDate.Length / 2;
            int oldDatePosX = ConsoleWrapper.WindowWidth / 2 - lastRenderedDate.Length / 2;

            // Check to see if the figlet fits
            bool needsFiglet = ConsoleWrapper.WindowWidth > blockPanelWidth && blockY >= 0;
            if (needsFiglet)
            {
                // Erase previous blocks
                var erasedBlocks = new Eraser()
                {
                    Left = 0,
                    Top = blockY,
                    Width = ConsoleWrapper.WindowWidth,
                    Height = maxHeight + 2,
                };
                TextWriterRaw.WriteRaw(erasedBlocks.Render());

                // Create blocks and render them
                var hoursBlock = new Border()
                {
                    Color = blockColorHours,
                    Left = blockHoursX,
                    Top = blockY,
                    Width = maxWidth,
                    Height = maxHeight,
                };
                var minutesBlock = new Border()
                {
                    Color = blockColorMinutes,
                    Left = blockMinutesX,
                    Top = blockY,
                    Width = maxWidth,
                    Height = maxHeight,
                };
                var secondsBlock = new Border()
                {
                    Color = blockColorSeconds,
                    Left = blockSecondsX,
                    Top = blockY,
                    Width = maxWidth,
                    Height = maxHeight,
                };
                TextWriterRaw.WriteRaw(hoursBlock.Render());
                TextWriterRaw.WriteRaw(minutesBlock.Render());
                TextWriterRaw.WriteRaw(secondsBlock.Render());

                // Render the figlet fonts
                var hoursText = new AlignedFigletText(figletFont, hoursStr)
                {
                    ForegroundColor = blockColorHours,
                    Left = blockHoursX + 1,
                    Top = blockY + 1
                };
                var minutesText = new AlignedFigletText(figletFont, minutesStr)
                {
                    ForegroundColor = blockColorMinutes,
                    Left = blockMinutesX + 1,
                    Top = blockY + 1
                };
                var secondsText = new AlignedFigletText(figletFont, secondsStr)
                {
                    ForegroundColor = blockColorSeconds,
                    Left = blockSecondsX + 1,
                    Top = blockY + 1
                };
                TextWriterRaw.WriteRaw(hoursText.Render());
                TextWriterRaw.WriteRaw(minutesText.Render());
                TextWriterRaw.WriteRaw(secondsText.Render());

                // Clear old date/time
                TextWriterWhereColor.WriteWhereColor(new string(' ', lastRenderedDate.Length), oldDatePosX, blockY + maxHeight + 3, blockColorInfo);

                // Write date and time
                TextWriterWhereColor.WriteWhereColor(renderedDate, datePosX, blockY + maxHeight + 3, blockColorInfo);
            }
            else
            {
                // Render the date and the time
                Color timeColor = ChangeColor();
                string renderedTime = TimeDateRenderers.RenderTime();
                int halfConsoleY = (int)(ConsoleWrapper.WindowHeight / 2d);
                int timePosX = ConsoleWrapper.WindowWidth / 2 - renderedTime.Length / 2;

                // Clear old date/time
                int oldTimePosX = ConsoleWrapper.WindowWidth / 2 - lastRenderedTime.Length / 2;
                TextWriterWhereColor.WriteWhereColor(new string(' ', lastRenderedDate.Length), oldDatePosX, halfConsoleY, timeColor);
                TextWriterWhereColor.WriteWhereColor(new string(' ', lastRenderedTime.Length), oldTimePosX, halfConsoleY + 1, timeColor);

                // Write date and time
                TextWriterWhereColor.WriteWhereColor(renderedDate, datePosX, halfConsoleY, timeColor);
                TextWriterWhereColor.WriteWhereColor(renderedTime, timePosX, halfConsoleY + 1, timeColor);
                lastRenderedTime = renderedTime;
            }
            lastRenderedDate = renderedDate;

            // Delay
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.BlockClockDelay);
        }

        private Color ChangeColor()
        {
            Color ColorInstance;
            if (ScreensaverPackInit.SaversConfig.BlockClockTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.BlockClockMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.BlockClockMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.BlockClockMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.BlockClockMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.BlockClockMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.BlockClockMaximumBlueColorLevel);
                ColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.BlockClockMinimumColorLevel, ScreensaverPackInit.SaversConfig.BlockClockMaximumColorLevel);
                ColorInstance = new Color(ColorNum);
            }
            return ColorInstance;
        }
    }
}
