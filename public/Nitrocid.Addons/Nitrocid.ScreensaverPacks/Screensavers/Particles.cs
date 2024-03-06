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

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for Particles
    /// </summary>
    public static class ParticlesSettings
    {

        /// <summary>
        /// [Particles] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool ParticlesTrueColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ParticlesTrueColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.ParticlesTrueColor = value;
            }
        }
        /// <summary>
        /// [Particles] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int ParticlesDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ParticlesDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                ScreensaverPackInit.SaversConfig.ParticlesDelay = value;
            }
        }
        /// <summary>
        /// [Particles] How dense are the particles?
        /// </summary>
        public static int ParticlesDensity
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ParticlesDensity;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                ScreensaverPackInit.SaversConfig.ParticlesDensity = value;
            }
        }
        /// <summary>
        /// [Particles] The minimum red color level (true color)
        /// </summary>
        public static int ParticlesMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ParticlesMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ParticlesMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Particles] The minimum green color level (true color)
        /// </summary>
        public static int ParticlesMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ParticlesMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ParticlesMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Particles] The minimum blue color level (true color)
        /// </summary>
        public static int ParticlesMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ParticlesMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ParticlesMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Particles] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int ParticlesMinimumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ParticlesMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.ParticlesMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Particles] The maximum red color level (true color)
        /// </summary>
        public static int ParticlesMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ParticlesMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.ParticlesMinimumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.ParticlesMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ParticlesMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Particles] The maximum green color level (true color)
        /// </summary>
        public static int ParticlesMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ParticlesMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.ParticlesMinimumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.ParticlesMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ParticlesMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Particles] The maximum blue color level (true color)
        /// </summary>
        public static int ParticlesMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ParticlesMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.ParticlesMinimumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.ParticlesMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ParticlesMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Particles] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int ParticlesMaximumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ParticlesMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.ParticlesMinimumColorLevel)
                    value = ScreensaverPackInit.SaversConfig.ParticlesMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.ParticlesMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for Particles
    /// </summary>
    public class ParticlesDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Particles";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Get how many particles we need to draw
            int cells = ConsoleWrapper.WindowWidth * ConsoleWrapper.WindowHeight;
            int particlesNeeded = cells / ParticlesSettings.ParticlesDensity;
            var particlesBuffer = new StringBuilder();
            for (int i = 0; i < particlesNeeded; i++)
            {
                // Make a particle color
                Color ColorStorage;
                if (ParticlesSettings.ParticlesTrueColor)
                {
                    int RedColorNum = RandomDriver.Random(ParticlesSettings.ParticlesMinimumRedColorLevel, ParticlesSettings.ParticlesMaximumRedColorLevel);
                    int GreenColorNum = RandomDriver.Random(ParticlesSettings.ParticlesMinimumGreenColorLevel, ParticlesSettings.ParticlesMaximumGreenColorLevel);
                    int BlueColorNum = RandomDriver.Random(ParticlesSettings.ParticlesMinimumBlueColorLevel, ParticlesSettings.ParticlesMaximumBlueColorLevel);
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                    ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                    particlesBuffer.Append(ColorStorage.VTSequenceBackgroundTrueColor);
                }
                else
                {
                    int ColorNum = RandomDriver.Random(ParticlesSettings.ParticlesMinimumColorLevel, ParticlesSettings.ParticlesMaximumColorLevel);
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                    ColorStorage = new Color(ColorNum);
                    particlesBuffer.Append(ColorStorage.VTSequenceBackgroundTrueColor);
                }

                // Select position to draw the particles
                int Left = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
                int Top = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", Left, Top);
                particlesBuffer.Append(CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + 1));
                particlesBuffer.Append(' ');
            }
            TextWriterRaw.WritePlain(particlesBuffer.ToString(), false);

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ThreadManager.SleepNoBlock(ParticlesSettings.ParticlesDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            ColorTools.LoadBack();
        }

    }
}
