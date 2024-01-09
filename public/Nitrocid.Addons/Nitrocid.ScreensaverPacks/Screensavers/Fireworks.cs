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

using System;
using Terminaux.Colors;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Screensaver;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Threading;
using Terminaux.Base;
using Terminaux.Colors.Data;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for Fireworks
    /// </summary>
    public static class FireworksSettings
    {

        /// <summary>
        /// [Fireworks] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool FireworksTrueColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FireworksTrueColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.FireworksTrueColor = value;
            }
        }
        /// <summary>
        /// [Fireworks] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int FireworksDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FireworksDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                ScreensaverPackInit.SaversConfig.FireworksDelay = value;
            }
        }
        /// <summary>
        /// [Fireworks] The radius of the explosion
        /// </summary>
        public static int FireworksRadius
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FireworksRadius;
            }
            set
            {
                if (value <= 0)
                    value = 5;
                ScreensaverPackInit.SaversConfig.FireworksRadius = value;
            }
        }
        /// <summary>
        /// [Fireworks] The minimum red color level (true color)
        /// </summary>
        public static int FireworksMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FireworksMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FireworksMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Fireworks] The minimum green color level (true color)
        /// </summary>
        public static int FireworksMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FireworksMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FireworksMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Fireworks] The minimum blue color level (true color)
        /// </summary>
        public static int FireworksMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FireworksMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FireworksMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Fireworks] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int FireworksMinimumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FireworksMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.FireworksMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Fireworks] The maximum red color level (true color)
        /// </summary>
        public static int FireworksMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FireworksMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.FireworksMinimumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.FireworksMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FireworksMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Fireworks] The maximum green color level (true color)
        /// </summary>
        public static int FireworksMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FireworksMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.FireworksMinimumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.FireworksMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FireworksMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Fireworks] The maximum blue color level (true color)
        /// </summary>
        public static int FireworksMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FireworksMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.FireworksMinimumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.FireworksMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FireworksMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Fireworks] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int FireworksMaximumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FireworksMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.FireworksMinimumColorLevel)
                    value = ScreensaverPackInit.SaversConfig.FireworksMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.FireworksMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for Fireworks
    /// </summary>
    public class FireworksDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Fireworks";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Variables
            int HalfHeight = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
            int LaunchPositionX = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
            int LaunchPositionY = ConsoleWrapper.WindowHeight - 1;
            int IgnitePositionX = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
            int IgnitePositionY = RandomDriver.Random(HalfHeight, (int)Math.Round(HalfHeight * 1.5d));
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Launch position {0}, {1}", LaunchPositionX, LaunchPositionY);
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Ignite position {0}, {1}", IgnitePositionX, IgnitePositionY);

            // Thresholds
            int FireworkThresholdX = IgnitePositionX - LaunchPositionX;
            int FireworkThresholdY = Math.Abs(IgnitePositionY - LaunchPositionY);
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Position thresholds (X: {0}, Y: {1})", FireworkThresholdX, FireworkThresholdY);
            double FireworkStepsX = FireworkThresholdX / (double)FireworkThresholdY;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "{0} steps", FireworkStepsX);
            int FireworkRadius = FireworksSettings.FireworksRadius >= 0 & FireworksSettings.FireworksRadius <= 10 ? FireworksSettings.FireworksRadius : 5;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Radius: {0} blocks", FireworkRadius);
            Color IgniteColor;

            // Select a color
            ConsoleWrapper.Clear();
            if (FireworksSettings.FireworksTrueColor)
            {
                int RedColorNum = RandomDriver.Random(FireworksSettings.FireworksMinimumRedColorLevel, FireworksSettings.FireworksMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(FireworksSettings.FireworksMinimumGreenColorLevel, FireworksSettings.FireworksMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(FireworksSettings.FireworksMinimumBlueColorLevel, FireworksSettings.FireworksMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                IgniteColor = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int color = RandomDriver.Random(FireworksSettings.FireworksMinimumColorLevel, FireworksSettings.FireworksMaximumColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color ({0})", color);
                IgniteColor = new Color(color);
            }

            // Launch the rocket
            if (!ConsoleResizeHandler.WasResized(false))
            {
                double CurrentX = LaunchPositionX;
                int CurrentY = LaunchPositionY;
                while (CurrentX != IgnitePositionX && CurrentY != IgnitePositionY)
                {
                    if (ConsoleResizeHandler.WasResized(false))
                        break;
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Current position: {0}, {1}", CurrentX, CurrentY);
                    ConsoleWrapper.SetCursorPosition((int)Math.Round(CurrentX), CurrentY);
                    ConsoleWrapper.Write(" ");

                    // Delay writing
                    ThreadManager.SleepNoBlock(FireworksSettings.FireworksDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                    ColorTools.LoadBack(new Color(ConsoleColors.Black));
                    ColorTools.SetConsoleColor(new Color(255, 255, 255), true);

                    // Change positions
                    CurrentX += FireworkStepsX;
                    CurrentY -= 1;
                }
            }

            // Blow it up!
            if (!ConsoleResizeHandler.WasResized(false))
            {
                for (int Radius = 0; Radius <= FireworkRadius; Radius++)
                {
                    if (ConsoleResizeHandler.WasResized(false))
                        break;

                    // Variables
                    int UpperParticleY = IgnitePositionY + 1 + Radius;
                    int LowerParticleY = IgnitePositionY - 1 - Radius;
                    int LeftParticleX = IgnitePositionX - 1 - Radius * 2;
                    int RightParticleX = IgnitePositionX + 1 + Radius * 2;
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Upper particle position: {0}", UpperParticleY);
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Lower particle position: {0}", LowerParticleY);
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Left particle position: {0}", LeftParticleX);
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Right particle position: {0}", RightParticleX);

                    // Draw the explosion
                    ColorTools.SetConsoleColor(IgniteColor, true);
                    if (UpperParticleY < ConsoleWrapper.WindowHeight && UpperParticleY >= 0)
                    {
                        DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Making upper particle at {0}, {1}", IgnitePositionX, UpperParticleY);
                        ConsoleWrapper.SetCursorPosition(IgnitePositionX, UpperParticleY);
                        ConsoleWrapper.Write(" ");
                    }
                    if (LowerParticleY < ConsoleWrapper.WindowHeight && LowerParticleY >= 0)
                    {
                        DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Making lower particle at {0}, {1}", IgnitePositionX, LowerParticleY);
                        ConsoleWrapper.SetCursorPosition(IgnitePositionX, LowerParticleY);
                        ConsoleWrapper.Write(" ");
                    }
                    if (LeftParticleX < ConsoleWrapper.WindowWidth && LeftParticleX >= 0)
                    {
                        DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Making left particle at {0}, {1}", LeftParticleX, IgnitePositionY);
                        ConsoleWrapper.SetCursorPosition(LeftParticleX, IgnitePositionY);
                        ConsoleWrapper.Write(" ");
                    }
                    if (RightParticleX < ConsoleWrapper.WindowWidth && RightParticleX >= 0)
                    {
                        DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Making right particle at {0}, {1}", RightParticleX, IgnitePositionY);
                        ConsoleWrapper.SetCursorPosition(RightParticleX, IgnitePositionY);
                        ConsoleWrapper.Write(" ");
                    }

                    // Delay writing
                    ThreadManager.SleepNoBlock(FireworksSettings.FireworksDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                    ColorTools.LoadBack(new Color(ConsoleColors.Black));
                }
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
        }

    }
}
