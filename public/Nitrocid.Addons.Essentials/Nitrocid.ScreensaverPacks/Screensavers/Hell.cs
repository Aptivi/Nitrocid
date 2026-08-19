//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Colorimetry;
using Colorimetry.Data;
using Colorimetry.Gradients;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Threadify.Manager;
using System.Collections.Generic;
using Nitrocid.Base.Drivers.RNG;
using Colorimetry.Transformation;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Hell
    /// </summary>
    public class HellDisplay : BaseScreensaver, IScreensaver
    {
        private int step = 0;
        private List<(double x, double y, int decay)> fireParticles = [];

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            step = 0;
            ConsoleColoring.LoadBackDry(ConsoleColors.Black);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Get the fire light scale
            double lightScale = 1 + RandomDriver.RandomDouble();

            // Get the gradient BG in terms of a console width
            int red = (int)(ScreensaverPackInit.SaversConfig.HellMaximumBackColorLevel * step / 100d * lightScale);
            red = red <= 255 ? red : 255;
            var currentBackColor = new Color(red, 0, 0);
            var bgGradient = ColorGradients.GetGradients(ConsoleColors.Black, currentBackColor, ConsoleWrapper.WindowHeight - 1);

            // Check to see if we need to add a particle
            bool particleNeeded = RandomDriver.RandomBoolean();
            if (step == 100 && particleNeeded)
            {
                int x = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
                int y = ConsoleWrapper.WindowHeight - 1;
                int decay = RandomDriver.Random(30, 70);
                fireParticles.Add((x, y, decay));
            }

            // Render the BG
            for (int i = ConsoleWrapper.WindowHeight - 1; i >= 0; i--)
            {
                var gradient = bgGradient[i];
                ConsoleColoring.SetConsoleColorDry(gradient.IntermediateColor, true, true);
                TextWriterWhereColor.WriteWherePlain(new(' ', ConsoleWrapper.WindowWidth), 0, i);
            }

            // Render the particles if necessary
            for (int i = 0; i < fireParticles.Count; i++)
            {
                var (x, y, decay) = fireParticles[i];

                // Get the particle color according to decay factor
                var particleColor = TransformationTools.BlendColor(ConsoleColors.Orange1, ConsoleColors.Black, decay / 100d);
                int finalDecay = decay + 2;

                // Make the particle move left, but make it also move up and down.
                bool particleMoveDown = RandomDriver.RandomChance(20);
                double finalX = x - RandomDriver.RandomDouble(2);
                double finalY = y + (particleMoveDown ? RandomDriver.RandomDouble(2) : -RandomDriver.RandomDouble(2));

                // Render the particle, and apply changes
                TextWriterWhereColor.WriteWhereColorBack(" ", (int)finalX, (int)finalY, ConsoleColors.Black, particleColor);
                fireParticles[i] = (finalX, finalY, finalDecay);
            }

            // Check the decays and positions
            for (int i = fireParticles.Count - 1; i >= 0; i--)
            {
                var (x, y, decay) = fireParticles[i];
                if (x < 0 || y < 0 || y >= ConsoleWrapper.WindowWidth || decay >= 100)
                {
                    fireParticles.RemoveAt(i);
                    continue;
                }
            }

            // If step is 100, stop, but render in case of a resize.
            if (step != 100)
                step++;
            ThreadManager.SleepNoBlock(ScreensaverPackInit.SaversConfig.HellDelay);
        }
    }
}
