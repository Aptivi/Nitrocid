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

using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Colors;
using Nitrocid.Base.Kernel.Configuration;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for LineUp
    /// </summary>
    public class LineUpDisplay : BaseScreensaver, IScreensaver
    {
        // TODO: NKS_SCREENSAVERPACKS_LINEUP_SETTINGS_DESC -> Shows two lines that come from the bottom to the top while filling the console
        private Color line1Color = Color.Empty;
        private Color line2Color = Color.Empty;
        private int line1PosY = -1;
        private int line2PosY = -1;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "LineUp";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            line1Color = SelectColor();
            line2Color = SelectColor();
            line1PosY = ConsoleWrapper.WindowHeight + RandomDriver.Random(-1, 10);
            line2PosY = ConsoleWrapper.WindowHeight + RandomDriver.Random(-1, 25);
            DebugWriter.WriteDebug(DebugLevel.I, "Line 1 position: {0}", vars: [line1PosY]);
            DebugWriter.WriteDebug(DebugLevel.I, "Line 2 position: {0}", vars: [line1PosY]);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Draw a line
            string lineString = !string.IsNullOrWhiteSpace(ScreensaverPackInit.SaversConfig.LineUpLineChar) ? ScreensaverPackInit.SaversConfig.LineUpLineChar : "-";
            string line = new(lineString[0], ConsoleWrapper.WindowWidth);
            string clearLine = new(' ', ConsoleWrapper.WindowWidth);
            if (!ConsoleResizeHandler.WasResized(false))
            {
                if (line1PosY < ConsoleWrapper.WindowHeight)
                    TextWriterWhereColor.WriteWhereColor(line, 0, line1PosY, line1Color);
                if (line2PosY < ConsoleWrapper.WindowHeight)
                    TextWriterWhereColor.WriteWhereColor(line, 0, line2PosY, line2Color);
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.LineUpDelay);
            if (!ConsoleResizeHandler.WasResized(false))
            {
                if (line1PosY < ConsoleWrapper.WindowHeight)
                    TextWriterWhereColor.WriteWherePlain(clearLine, 0, line1PosY);
                if (line2PosY < ConsoleWrapper.WindowHeight)
                    TextWriterWhereColor.WriteWherePlain(clearLine, 0, line2PosY);
            }

            // Reset the position if needed
            line1PosY--;
            if (line1PosY < 0)
            {
                line1PosY = ConsoleWrapper.WindowHeight + RandomDriver.Random(-1, 10);
                line1Color = SelectColor();
            }
            line2PosY--;
            if (line2PosY < 0)
            {
                line2PosY = ConsoleWrapper.WindowHeight + RandomDriver.Random(-1, 25);
                line2Color = SelectColor();
            }
        }

        private Color SelectColor()
        {
            if (ScreensaverPackInit.SaversConfig.LineUpTrueColor)
            {
                ColorTools.LoadBackDry(new Color(ScreensaverPackInit.SaversConfig.LineUpBackgroundColor));
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.LineUpMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.LineUpMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.LineUpMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.LineUpMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.LineUpMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.LineUpMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                return new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                ColorTools.LoadBackDry(new Color(ScreensaverPackInit.SaversConfig.LineUpBackgroundColor));
                int color = RandomDriver.Random(ScreensaverPackInit.SaversConfig.LineUpMinimumColorLevel, ScreensaverPackInit.SaversConfig.LineUpMaximumColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color ({0})", vars: [color]);
                return new Color(color);
            }
        }
    }
}
