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
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Screensaver;
using System.Text;
using Terminaux.Colors;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Nitrocid.Kernel.Configuration;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for ZebraShift
    /// </summary>
    public class ZebraShiftDisplay : BaseScreensaver, IScreensaver
    {

        private static Color firstLineColor = ConsoleColors.White;
        private static Color secondLineColor = ConsoleColors.Black;
        private static bool inverse = false;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "ZebraShift";

        /// <inheritdoc/>
        public override bool ScreensaverContainsFlashingImages =>
            true;

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            base.ScreensaverPreparation();

            // Select two colors for two lines
            if (ScreensaverPackInit.SaversConfig.ZebraShiftTrueColor)
            {
                int firstGroupRedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.ZebraShiftMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.ZebraShiftMaximumRedColorLevel);
                int firstGroupGreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.ZebraShiftMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.ZebraShiftMaximumGreenColorLevel);
                int firstGroupBlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.ZebraShiftMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.ZebraShiftMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color for first group (R;G;B: {0};{1};{2})", vars: [firstGroupRedColorNum, firstGroupGreenColorNum, firstGroupBlueColorNum]);
                int secondGroupRedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.ZebraShiftMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.ZebraShiftMaximumRedColorLevel);
                int secondGroupGreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.ZebraShiftMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.ZebraShiftMaximumGreenColorLevel);
                int secondGroupBlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.ZebraShiftMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.ZebraShiftMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color for second group (R;G;B: {0};{1};{2})", vars: [secondGroupRedColorNum, secondGroupGreenColorNum, secondGroupBlueColorNum]);
                var firstGroupColorStorage = new Color(firstGroupRedColorNum, firstGroupGreenColorNum, firstGroupBlueColorNum);
                var secondGroupColorStorage = new Color(secondGroupRedColorNum, secondGroupGreenColorNum, secondGroupBlueColorNum);
                firstLineColor = firstGroupColorStorage;
                secondLineColor = secondGroupColorStorage;
            }
            else
            {
                int firstGroupColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.ZebraShiftMinimumColorLevel, ScreensaverPackInit.SaversConfig.ZebraShiftMaximumColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color for first group ({0})", vars: [firstGroupColorNum]);
                int secondGroupColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.ZebraShiftMinimumColorLevel, ScreensaverPackInit.SaversConfig.ZebraShiftMaximumColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color for second group ({0})", vars: [secondGroupColorNum]);
                firstLineColor = firstGroupColorNum;
                secondLineColor = secondGroupColorNum;
            }
            inverse = false;
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Prepare the string buffer
            var zebraBuffer = new StringBuilder();

            // Now, make the two zebra lines
            int width = ConsoleWrapper.WindowWidth;
            int height = ConsoleWrapper.WindowHeight;
            var finalFirstColor = inverse ? secondLineColor : firstLineColor;
            var finalSecondColor = inverse ? firstLineColor : secondLineColor;
            for (int x = 0; x < width; x++)
            {
                // Select a color
                bool useOther = x % 2 == 0;
                var color = useOther ? finalSecondColor : finalFirstColor;

                // Fill the line vertically
                zebraBuffer.Append(color.VTSequenceBackground());
                for (int y = 0; y < height; y++)
                {
                    zebraBuffer.Append(
                        CsiSequences.GenerateCsiCursorPosition(x + 1, y + 1) +
                        " "
                    );
                }
            }
            inverse = !inverse;

            // Write the shift buffer
            TextWriterRaw.WritePlain(zebraBuffer.ToString(), false);

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.ZebraShiftDelay);
        }

    }
}
