//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
using System.Text;
using Terminaux.Colors;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Base;
using Terminaux.Colors.Data;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for ZebraShift
    /// </summary>
    public static class ZebraShiftSettings
    {

        /// <summary>
        /// [ZebraShift] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool ZebraShiftTrueColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ZebraShiftTrueColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.ZebraShiftTrueColor = value;
            }
        }
        /// <summary>
        /// [ZebraShift] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int ZebraShiftDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ZebraShiftDelay;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                ScreensaverPackInit.SaversConfig.ZebraShiftDelay = value;
            }
        }
        /// <summary>
        /// [ZebraShift] The minimum red color level (true color)
        /// </summary>
        public static int ZebraShiftMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ZebraShiftMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ZebraShiftMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ZebraShift] The minimum green color level (true color)
        /// </summary>
        public static int ZebraShiftMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ZebraShiftMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ZebraShiftMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ZebraShift] The minimum blue color level (true color)
        /// </summary>
        public static int ZebraShiftMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ZebraShiftMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ZebraShiftMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ZebraShift] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int ZebraShiftMinimumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ZebraShiftMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.ZebraShiftMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [ZebraShift] The maximum red color level (true color)
        /// </summary>
        public static int ZebraShiftMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ZebraShiftMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.ZebraShiftMinimumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.ZebraShiftMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ZebraShiftMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ZebraShift] The maximum green color level (true color)
        /// </summary>
        public static int ZebraShiftMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ZebraShiftMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.ZebraShiftMinimumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.ZebraShiftMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ZebraShiftMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ZebraShift] The maximum blue color level (true color)
        /// </summary>
        public static int ZebraShiftMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ZebraShiftMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.ZebraShiftMinimumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.ZebraShiftMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ZebraShiftMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ZebraShift] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int ZebraShiftMaximumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ZebraShiftMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.ZebraShiftMinimumColorLevel)
                    value = ScreensaverPackInit.SaversConfig.ZebraShiftMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.ZebraShiftMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for ZebraShift
    /// </summary>
    public class ZebraShiftDisplay : BaseScreensaver, IScreensaver
    {

        private static Color firstLineColor = ConsoleColors.White;
        private static Color secondLineColor = ConsoleColors.Black;
        private static bool inverse = false;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "ZebraShift";

        /// <inheritdoc/>
        public override bool ScreensaverContainsFlashingImages { get; set; } = true;

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            base.ScreensaverPreparation();

            // Select two colors for two lines
            if (ZebraShiftSettings.ZebraShiftTrueColor)
            {
                int firstGroupRedColorNum = RandomDriver.Random(ZebraShiftSettings.ZebraShiftMinimumRedColorLevel, ZebraShiftSettings.ZebraShiftMaximumRedColorLevel);
                int firstGroupGreenColorNum = RandomDriver.Random(ZebraShiftSettings.ZebraShiftMinimumGreenColorLevel, ZebraShiftSettings.ZebraShiftMaximumGreenColorLevel);
                int firstGroupBlueColorNum = RandomDriver.Random(ZebraShiftSettings.ZebraShiftMinimumBlueColorLevel, ZebraShiftSettings.ZebraShiftMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color for first group (R;G;B: {0};{1};{2})", firstGroupRedColorNum, firstGroupGreenColorNum, firstGroupBlueColorNum);
                int secondGroupRedColorNum = RandomDriver.Random(ZebraShiftSettings.ZebraShiftMinimumRedColorLevel, ZebraShiftSettings.ZebraShiftMaximumRedColorLevel);
                int secondGroupGreenColorNum = RandomDriver.Random(ZebraShiftSettings.ZebraShiftMinimumGreenColorLevel, ZebraShiftSettings.ZebraShiftMaximumGreenColorLevel);
                int secondGroupBlueColorNum = RandomDriver.Random(ZebraShiftSettings.ZebraShiftMinimumBlueColorLevel, ZebraShiftSettings.ZebraShiftMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color for second group (R;G;B: {0};{1};{2})", secondGroupRedColorNum, secondGroupGreenColorNum, secondGroupBlueColorNum);
                var firstGroupColorStorage = new Color(firstGroupRedColorNum, firstGroupGreenColorNum, firstGroupBlueColorNum);
                var secondGroupColorStorage = new Color(secondGroupRedColorNum, secondGroupGreenColorNum, secondGroupBlueColorNum);
                firstLineColor = firstGroupColorStorage;
                secondLineColor = secondGroupColorStorage;
            }
            else
            {
                int firstGroupColorNum = RandomDriver.Random(ZebraShiftSettings.ZebraShiftMinimumColorLevel, ZebraShiftSettings.ZebraShiftMaximumColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color for first group ({0})", firstGroupColorNum);
                int secondGroupColorNum = RandomDriver.Random(ZebraShiftSettings.ZebraShiftMinimumColorLevel, ZebraShiftSettings.ZebraShiftMaximumColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color for second group ({0})", secondGroupColorNum);
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
                zebraBuffer.Append(color.VTSequenceBackground);
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
            ThreadManager.SleepNoBlock(ZebraShiftSettings.ZebraShiftDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
