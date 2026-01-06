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
using Terminaux.Colors;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using Nitrocid.Base.Drivers.RNG;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Nitrocid.Base.Kernel.Configuration;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Fireworks
    /// </summary>
    public class FireworksDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Fireworks";

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
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Launch position {0}, {1}", vars: [LaunchPositionX, LaunchPositionY]);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Ignite position {0}, {1}", vars: [IgnitePositionX, IgnitePositionY]);

            // Thresholds
            int FireworkThresholdX = IgnitePositionX - LaunchPositionX;
            int FireworkThresholdY = Math.Abs(IgnitePositionY - LaunchPositionY);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Position thresholds (X: {0}, Y: {1})", vars: [FireworkThresholdX, FireworkThresholdY]);
            double FireworkStepsX = FireworkThresholdX / (double)FireworkThresholdY;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "{0} steps", vars: [FireworkStepsX]);
            int FireworkRadius = ScreensaverPackInit.SaversConfig.FireworksRadius >= 0 & ScreensaverPackInit.SaversConfig.FireworksRadius <= 10 ? ScreensaverPackInit.SaversConfig.FireworksRadius : 5;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Radius: {0} blocks", vars: [FireworkRadius]);
            Color IgniteColor;

            // Select a color
            ConsoleWrapper.Clear();
            if (ScreensaverPackInit.SaversConfig.FireworksTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.FireworksMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.FireworksMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.FireworksMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.FireworksMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.FireworksMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.FireworksMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                IgniteColor = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int color = RandomDriver.Random(ScreensaverPackInit.SaversConfig.FireworksMinimumColorLevel, ScreensaverPackInit.SaversConfig.FireworksMaximumColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color ({0})", vars: [color]);
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
                    if (ScreensaverManager.Bailing)
                        return;
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Current position: {0}, {1}", vars: [CurrentX, CurrentY]);
                    ConsoleWrapper.SetCursorPosition((int)Math.Round(CurrentX), CurrentY);
                    ConsoleWrapper.Write(" ");

                    // Delay writing
                    ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.FireworksDelay);
                    ColorTools.LoadBackDry(new Color(ConsoleColors.Black));
                    ColorTools.SetConsoleColorDry(new Color(255, 255, 255), true);

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
                    if (ScreensaverManager.Bailing)
                        return;

                    // Variables
                    int UpperParticleY = IgnitePositionY + 1 + Radius;
                    int LowerParticleY = IgnitePositionY - 1 - Radius;
                    int LeftParticleX = IgnitePositionX - 1 - Radius * 2;
                    int RightParticleX = IgnitePositionX + 1 + Radius * 2;
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Upper particle position: {0}", vars: [UpperParticleY]);
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Lower particle position: {0}", vars: [LowerParticleY]);
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Left particle position: {0}", vars: [LeftParticleX]);
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Right particle position: {0}", vars: [RightParticleX]);

                    // Draw the explosion
                    ColorTools.SetConsoleColorDry(IgniteColor, true);
                    if (UpperParticleY < ConsoleWrapper.WindowHeight && UpperParticleY >= 0)
                    {
                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Making upper particle at {0}, {1}", vars: [IgnitePositionX, UpperParticleY]);
                        ConsoleWrapper.SetCursorPosition(IgnitePositionX, UpperParticleY);
                        ConsoleWrapper.Write(" ");
                    }
                    if (LowerParticleY < ConsoleWrapper.WindowHeight && LowerParticleY >= 0)
                    {
                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Making lower particle at {0}, {1}", vars: [IgnitePositionX, LowerParticleY]);
                        ConsoleWrapper.SetCursorPosition(IgnitePositionX, LowerParticleY);
                        ConsoleWrapper.Write(" ");
                    }
                    if (LeftParticleX < ConsoleWrapper.WindowWidth && LeftParticleX >= 0)
                    {
                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Making left particle at {0}, {1}", vars: [LeftParticleX, IgnitePositionY]);
                        ConsoleWrapper.SetCursorPosition(LeftParticleX, IgnitePositionY);
                        ConsoleWrapper.Write(" ");
                    }
                    if (RightParticleX < ConsoleWrapper.WindowWidth && RightParticleX >= 0)
                    {
                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Making right particle at {0}, {1}", vars: [RightParticleX, IgnitePositionY]);
                        ConsoleWrapper.SetCursorPosition(RightParticleX, IgnitePositionY);
                        ConsoleWrapper.Write(" ");
                    }

                    // Delay writing
                    ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.FireworksDelay);
                    ColorTools.LoadBackDry(new Color(ConsoleColors.Black));
                }
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
        }

    }
}
