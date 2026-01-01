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
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Base;
using Nitrocid.Kernel.Configuration;
using Terminaux.Writer.CyclicWriters;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Colors.Transformation;
using Nitrocid.ConsoleBase.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display for BarWave
    /// </summary>
    public class BarWaveDisplay : BaseScreensaver, IScreensaver
    {
        private double TimeSecs = 0.0d;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "BarWave";

        /// <inheritdoc/>
        public override void ScreensaverPreparation() =>
            KernelColorTools.LoadBackground();

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // First, prepare how many bars to render according to the console size
            int BarHeight = ConsoleWrapper.WindowHeight - 4;
            int BarWidthOutside = 3;
            int BarCount = ConsoleWrapper.WindowWidth / 4;

            // Then, go ahead and make these bars wave themselves.
            int[] CurrentPos = new int[BarCount];
            double Frequency = Math.PI / ScreensaverPackInit.SaversConfig.BarWaveFrequencyLevel;

            // Set the current positions
            for (int i = 0; i < BarCount; i++)
            {
                TimeSecs += 0.1;
                if (TimeSecs > 2.0)
                    TimeSecs = 0.0;
                CurrentPos[i] = (int)Math.Abs(BarHeight * Math.Cos(Frequency * TimeSecs));
            }

            // Render the bars
            for (int i = 0; i < BarCount; i++)
            {
                int ThisBarLeft = (BarWidthOutside + 1) * i + 1;
                int Pos = (int)(100 * (CurrentPos[i] / (double)BarHeight));
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.BarWaveMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.BarWaveMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.BarWaveMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.BarWaveMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.BarWaveMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.BarWaveMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                if (!ConsoleResizeHandler.WasResized(false))
                {
                    var progress = new SimpleProgress(Pos, 100)
                    {
                        Vertical = true,
                        Height = 4,
                        ProgressActiveForegroundColor = ColorStorage,
                        ProgressForegroundColor = TransformationTools.GetDarkBackground(ColorStorage),
                    };
                    TextWriterRaw.WriteRaw(ContainerTools.RenderRenderable(progress, new(ThisBarLeft, 1)));
                }
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.BarWaveDelay);
        }

        /// <inheritdoc/>
        public override void ScreensaverOutro()
        {
            TimeSecs = 0.0d;
        }

    }
}
