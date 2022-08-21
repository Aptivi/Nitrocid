using System;
using System.Collections.Generic;
using ColorSeq;

// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using Extensification.IntegerExts;
using KS.ConsoleBase.Colors;
using KS.Misc.Threading;
using KS.Misc.Writers.DebugWriters;

namespace KS.Misc.Screensaver.Displays
{
    public static class FireworksSettings
    {

        private static bool _fireworks255Colors;
        private static bool _fireworksTrueColor = true;
        private static int _fireworksDelay = 10;
        private static int _fireworksRadius = 5;
        private static int _fireworksMinimumRedColorLevel = 0;
        private static int _fireworksMinimumGreenColorLevel = 0;
        private static int _fireworksMinimumBlueColorLevel = 0;
        private static int _fireworksMinimumColorLevel = 0;
        private static int _fireworksMaximumRedColorLevel = 255;
        private static int _fireworksMaximumGreenColorLevel = 255;
        private static int _fireworksMaximumBlueColorLevel = 255;
        private static int _fireworksMaximumColorLevel = 255;

        /// <summary>
        /// [Fireworks] Enable 255 color support. Has a higher priority than 16 color support.
        /// </summary>
        public static bool Fireworks255Colors
        {
            get
            {
                return _fireworks255Colors;
            }
            set
            {
                _fireworks255Colors = value;
            }
        }
        /// <summary>
        /// [Fireworks] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool FireworksTrueColor
        {
            get
            {
                return _fireworksTrueColor;
            }
            set
            {
                _fireworksTrueColor = value;
            }
        }
        /// <summary>
        /// [Fireworks] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int FireworksDelay
        {
            get
            {
                return _fireworksDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                _fireworksDelay = value;
            }
        }
        /// <summary>
        /// [Fireworks] The radius of the explosion
        /// </summary>
        public static int FireworksRadius
        {
            get
            {
                return _fireworksRadius;
            }
            set
            {
                if (value <= 0)
                    value = 5;
                _fireworksRadius = value;
            }
        }
        /// <summary>
        /// [Fireworks] The minimum red color level (true color)
        /// </summary>
        public static int FireworksMinimumRedColorLevel
        {
            get
            {
                return _fireworksMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _fireworksMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Fireworks] The minimum green color level (true color)
        /// </summary>
        public static int FireworksMinimumGreenColorLevel
        {
            get
            {
                return _fireworksMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _fireworksMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Fireworks] The minimum blue color level (true color)
        /// </summary>
        public static int FireworksMinimumBlueColorLevel
        {
            get
            {
                return _fireworksMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _fireworksMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Fireworks] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int FireworksMinimumColorLevel
        {
            get
            {
                return _fireworksMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = _fireworks255Colors | _fireworksTrueColor ? 255 : 15;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _fireworksMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Fireworks] The maximum red color level (true color)
        /// </summary>
        public static int FireworksMaximumRedColorLevel
        {
            get
            {
                return _fireworksMaximumRedColorLevel;
            }
            set
            {
                if (value <= _fireworksMinimumRedColorLevel)
                    value = _fireworksMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _fireworksMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Fireworks] The maximum green color level (true color)
        /// </summary>
        public static int FireworksMaximumGreenColorLevel
        {
            get
            {
                return _fireworksMaximumGreenColorLevel;
            }
            set
            {
                if (value <= _fireworksMinimumGreenColorLevel)
                    value = _fireworksMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _fireworksMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Fireworks] The maximum blue color level (true color)
        /// </summary>
        public static int FireworksMaximumBlueColorLevel
        {
            get
            {
                return _fireworksMaximumBlueColorLevel;
            }
            set
            {
                if (value <= _fireworksMinimumBlueColorLevel)
                    value = _fireworksMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _fireworksMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Fireworks] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int FireworksMaximumColorLevel
        {
            get
            {
                return _fireworksMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = _fireworks255Colors | _fireworksTrueColor ? 255 : 15;
                if (value <= _fireworksMinimumColorLevel)
                    value = _fireworksMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _fireworksMaximumColorLevel = value;
            }
        }

    }

    public class FireworksDisplay : BaseScreensaver, IScreensaver
    {

        private Random RandomDriver;
        private int CurrentWindowWidth;
        private int CurrentWindowHeight;
        private bool ResizeSyncing;

        public override string ScreensaverName { get; set; } = "Fireworks";

        public override Dictionary<string, object> ScreensaverSettings { get; set; }

        public override void ScreensaverPreparation()
        {
            // Variable preparations
            RandomDriver = new Random();
            CurrentWindowWidth = Console.WindowWidth;
            CurrentWindowHeight = Console.WindowHeight;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
            DebugWriter.Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight);
        }

        public override void ScreensaverLogic()
        {
            Console.CursorVisible = false;
            // Variables
            int HalfHeight = (int)Math.Round(Console.WindowHeight / 2d);
            int LaunchPositionX = RandomDriver.Next(Console.WindowWidth);
            int LaunchPositionY = Console.WindowHeight - 1;
            int IgnitePositionX = RandomDriver.Next(Console.WindowWidth);
            int IgnitePositionY = RandomDriver.Next(HalfHeight, (int)Math.Round(HalfHeight * 1.5d));
            LaunchPositionX.SwapIfSourceLarger(ref IgnitePositionX);
            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Launch position {0}, {1}", LaunchPositionX, LaunchPositionY);
            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Ignite position {0}, {1}", IgnitePositionX, IgnitePositionY);

            // Thresholds
            int FireworkThresholdX = IgnitePositionX - LaunchPositionX;
            int FireworkThresholdY = Math.Abs(IgnitePositionY - LaunchPositionY);
            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Position thresholds (X: {0}, Y: {1})", FireworkThresholdX, FireworkThresholdY);
            double FireworkStepsX = FireworkThresholdX / (double)FireworkThresholdY;
            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "{0} steps", FireworkStepsX);
            int FireworkRadius = FireworksSettings.FireworksRadius >= 0 & FireworksSettings.FireworksRadius <= 10 ? FireworksSettings.FireworksRadius : 5;
            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Radius: {0} blocks", FireworkRadius);
            var IgniteColor = new Color(255, 255, 255);

            // Select a color
            Console.Clear();
            if (FireworksSettings.FireworksTrueColor)
            {
                int RedColorNum = RandomDriver.Next(FireworksSettings.FireworksMinimumRedColorLevel, FireworksSettings.FireworksMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Next(FireworksSettings.FireworksMinimumGreenColorLevel, FireworksSettings.FireworksMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Next(FireworksSettings.FireworksMinimumBlueColorLevel, FireworksSettings.FireworksMaximumBlueColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                IgniteColor = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else if (FireworksSettings.Fireworks255Colors)
            {
                int color = RandomDriver.Next(FireworksSettings.FireworksMinimumColorLevel, FireworksSettings.FireworksMaximumColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", color);
                IgniteColor = new Color(color);
            }

            // Launch the rocket
            if (!ResizeSyncing)
            {
                double CurrentX = LaunchPositionX;
                int CurrentY = LaunchPositionY;
                while (!(CurrentX >= IgnitePositionX & CurrentY <= IgnitePositionY))
                {
                    if (CurrentWindowHeight != Console.WindowHeight | CurrentWindowWidth != Console.WindowWidth)
                        ResizeSyncing = true;
                    if (ResizeSyncing)
                        break;
                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Current position: {0}, {1}", CurrentX, CurrentY);
                    Console.SetCursorPosition((int)Math.Round(CurrentX), CurrentY);
                    Console.Write(" ");

                    // Delay writing
                    ThreadManager.SleepNoBlock(FireworksSettings.FireworksDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Clear();
                    ColorTools.SetConsoleColor(new Color(255, 255, 255), true, true);

                    // Change positions
                    CurrentX += FireworkStepsX;
                    CurrentY -= 1;
                }
            }

            // Blow it up!
            if (!ResizeSyncing)
            {
                for (int Radius = 0, loopTo = FireworkRadius; Radius <= loopTo; Radius++)
                {
                    if (CurrentWindowHeight != Console.WindowHeight | CurrentWindowWidth != Console.WindowWidth)
                        ResizeSyncing = true;
                    if (ResizeSyncing)
                        break;

                    // Variables
                    int UpperParticleY = IgnitePositionY + 1 + Radius;
                    int LowerParticleY = IgnitePositionY - 1 - Radius;
                    int LeftParticleX = IgnitePositionX - 1 - Radius * 2;
                    int RightParticleX = IgnitePositionX + 1 + Radius * 2;
                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Upper particle position: {0}", UpperParticleY);
                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Lower particle position: {0}", LowerParticleY);
                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Left particle position: {0}", LeftParticleX);
                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Right particle position: {0}", RightParticleX);

                    // Draw the explosion
                    ColorTools.SetConsoleColor(IgniteColor, true, true);
                    if (UpperParticleY < Console.WindowHeight)
                    {
                        DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Making upper particle at {0}, {1}", IgnitePositionX, UpperParticleY);
                        Console.SetCursorPosition(IgnitePositionX, UpperParticleY);
                        Console.Write(" ");
                    }
                    if (LowerParticleY < Console.WindowHeight)
                    {
                        DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Making lower particle at {0}, {1}", IgnitePositionX, LowerParticleY);
                        Console.SetCursorPosition(IgnitePositionX, LowerParticleY);
                        Console.Write(" ");
                    }
                    if (LeftParticleX < Console.WindowWidth)
                    {
                        DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Making left particle at {0}, {1}", LeftParticleX, IgnitePositionY);
                        Console.SetCursorPosition(LeftParticleX, IgnitePositionY);
                        Console.Write(" ");
                    }
                    if (RightParticleX < Console.WindowWidth)
                    {
                        DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Making right particle at {0}, {1}", RightParticleX, IgnitePositionY);
                        Console.SetCursorPosition(RightParticleX, IgnitePositionY);
                        Console.Write(" ");
                    }

                    // Delay writing
                    ThreadManager.SleepNoBlock(FireworksSettings.FireworksDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Clear();
                }
            }

            // Reset resize sync
            ResizeSyncing = false;
            CurrentWindowWidth = Console.WindowWidth;
            CurrentWindowHeight = Console.WindowHeight;
        }

    }
}