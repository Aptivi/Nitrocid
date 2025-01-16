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
using Nitrocid.Kernel.Configuration;
using System.Text;
using Terminaux.Colors;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Base;
using Nitrocid.ConsoleBase.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Particles
    /// </summary>
    public class ParticlesDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Particles";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Get how many particles we need to draw
            int cells = ConsoleWrapper.WindowWidth * ConsoleWrapper.WindowHeight;
            int particlesNeeded = cells / ScreensaverPackInit.SaversConfig.ParticlesDensity;
            var particlesBuffer = new StringBuilder();
            for (int i = 0; i < particlesNeeded; i++)
            {
                // Make a particle color
                Color ColorStorage;
                if (ScreensaverPackInit.SaversConfig.ParticlesTrueColor)
                {
                    int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.ParticlesMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.ParticlesMaximumRedColorLevel);
                    int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.ParticlesMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.ParticlesMaximumGreenColorLevel);
                    int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.ParticlesMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.ParticlesMaximumBlueColorLevel);
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                    ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                    particlesBuffer.Append(ColorStorage.VTSequenceBackgroundTrueColor);
                }
                else
                {
                    int ColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.ParticlesMinimumColorLevel, ScreensaverPackInit.SaversConfig.ParticlesMaximumColorLevel);
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color ({0})", vars: [ColorNum]);
                    ColorStorage = new Color(ColorNum);
                    particlesBuffer.Append(ColorStorage.VTSequenceBackgroundTrueColor);
                }

                // Select position to draw the particles
                int Left = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
                int Top = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", vars: [Left, Top]);
                particlesBuffer.Append(CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + 1));
                particlesBuffer.Append(' ');
            }
            TextWriterRaw.WritePlain(particlesBuffer.ToString(), false);

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.ParticlesDelay);
            KernelColorTools.LoadBackground();
        }

    }
}
