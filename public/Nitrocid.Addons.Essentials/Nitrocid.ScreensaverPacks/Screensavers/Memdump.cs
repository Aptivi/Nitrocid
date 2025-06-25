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
using Terminaux.Colors;
using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Drivers.RNG;
using Terminaux.Base;
using Terminaux.Writer.CyclicWriters.Graphical;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Memdump
    /// </summary>
    public class MemdumpDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Memdump";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Two boxes, one for the initial color, and one for the bit-shifted color
            var dumpInitialColorBorder = new Border()
            {
                Left = 2,
                Top = 1,
                Width = 8,
                Height = 3,
            };
            var dumpShiftedColorBorder = new Border()
            {
                Left = 13,
                Top = 1,
                Width = 8,
                Height = 3,
            };
            TextWriterRaw.WriteRaw(
                dumpInitialColorBorder.Render() +
                dumpShiftedColorBorder.Render()
            );

            // Some positions
            int infoTop = 6;
            int initialInfoMargin = 4;
            int shiftedInfoMargin = 15;

            // Get the initial color values
            int initialR = RandomDriver.Random(255);
            int initialG = RandomDriver.Random(255);
            int initialB = RandomDriver.Random(255);
            var initialColor = new Color(initialR, initialG, initialB);

            // Get the hexadecimal value for display and decimal value for shifting
            string initialHex = $"{initialR:X2}{initialG:X2}{initialB:X2}";
            long initialValue = Convert.ToInt64($"{initialR}{initialG}{initialB}");

            // Get the colors from the decimal value by shifting the bits from it and then the hex
            int shiftedR = (int)(initialValue & 0xFF0000) >> 16;
            int shiftedG = (int)(initialValue & 0xFF00) >> 8;
            int shiftedB = (int)initialValue & 0xFF;
            string shiftedHex = $"{shiftedR:X2}{shiftedG:X2}{shiftedB:X2}";
            var shiftedColor = new Color(shiftedR, shiftedG, shiftedB);

            // Now, fill the colors!
            for (int i = 0; i < 3; i++)
                TextWriterWhereColor.WriteWhereColorBack(new string(' ', 8), 3, 2 + i, Color.Empty, initialColor);
            for (int i = 0; i < 3; i++)
                TextWriterWhereColor.WriteWhereColorBack(new string(' ', 8), 14, 2 + i, Color.Empty, shiftedColor);

            // Print the hexes
            TextWriterWhereColor.WriteWhereColor(initialHex, initialInfoMargin, infoTop, initialColor);
            TextWriterWhereColor.WriteWhereColor(shiftedHex, shiftedInfoMargin, infoTop, shiftedColor);

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.MemdumpDelay);
        }

    }
}
