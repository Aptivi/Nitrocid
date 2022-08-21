using System;
using System.Collections.Generic;
using ColorSeq;
using KS.ConsoleBase.Colors;
using KS.Misc.Threading;
using KS.Misc.Writers.DebugWriters;

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

namespace KS.Misc.Screensaver.Displays
{
    public class WindowsLogoDisplay : BaseScreensaver, IScreensaver
    {

        private int CurrentWindowWidth;
        private int CurrentWindowHeight;
        private bool ResizeSyncing;
        private bool Drawn;

        public override string ScreensaverName { get; set; } = "WindowsLogo";

        public override Dictionary<string, object> ScreensaverSettings { get; set; }

        public override void ScreensaverPreparation()
        {
            // Variable preparations
            CurrentWindowWidth = Console.WindowWidth;
            CurrentWindowHeight = Console.WindowHeight;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            DebugWriter.Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight);
        }

        public override void ScreensaverLogic()
        {
            Console.CursorVisible = false;
            if (ResizeSyncing)
            {
                Drawn = false;

                // Reset resize sync
                ResizeSyncing = false;
                CurrentWindowWidth = Console.WindowWidth;
                CurrentWindowHeight = Console.WindowHeight;
            }
            else
            {
                if (CurrentWindowHeight != Console.WindowHeight | CurrentWindowWidth != Console.WindowWidth)
                    ResizeSyncing = true;

                // Get the required positions for the four boxes
                int UpperLeftBoxEndX = (int)Math.Round(Console.WindowWidth / 2d - 1d);
                int UpperLeftBoxStartX = (int)Math.Round(UpperLeftBoxEndX / 2d);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Upper left box X position {0} -> {1}", UpperLeftBoxStartX, UpperLeftBoxEndX);

                int UpperLeftBoxStartY = 2;
                int UpperLeftBoxEndY = (int)Math.Round(Console.WindowHeight / 2d - 1d);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Upper left box Y position {0} -> {1}", UpperLeftBoxStartY, UpperLeftBoxEndY);

                int LowerLeftBoxEndX = (int)Math.Round(Console.WindowWidth / 2d - 1d);
                int LowerLeftBoxStartX = (int)Math.Round(LowerLeftBoxEndX / 2d);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Lower left box X position {0} -> {1}", LowerLeftBoxStartX, LowerLeftBoxEndX);

                int LowerLeftBoxStartY = (int)Math.Round(Console.WindowHeight / 2d + 1d);
                int LowerLeftBoxEndY = Console.WindowHeight - 2;
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Lower left box X position {0} -> {1}", LowerLeftBoxStartX, LowerLeftBoxEndX);

                int UpperRightBoxStartX = (int)Math.Round(Console.WindowWidth / 2d + 2d);
                int UpperRightBoxEndX = (int)Math.Round(Console.WindowWidth / 2d + UpperRightBoxStartX / 2d);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Upper right box X position {0} -> {1}", UpperRightBoxStartX, UpperRightBoxEndX);

                int UpperRightBoxStartY = 2;
                int UpperRightBoxEndY = (int)Math.Round(Console.WindowHeight / 2d - 1d);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Upper right box X position {0} -> {1}", UpperRightBoxStartX, UpperRightBoxEndX);

                int LowerRightBoxStartX = (int)Math.Round(Console.WindowWidth / 2d + 2d);
                int LowerRightBoxEndX = (int)Math.Round(Console.WindowWidth / 2d + LowerRightBoxStartX / 2d);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Lower right box X position {0} -> {1}", LowerRightBoxStartX, LowerRightBoxEndX);

                int LowerRightBoxStartY = (int)Math.Round(Console.WindowHeight / 2d + 1d);
                int LowerRightBoxEndY = Console.WindowHeight - 2;
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Lower right box X position {0} -> {1}", LowerRightBoxStartX, LowerRightBoxEndX);

                // Draw the Windows 11 logo
                if (!Drawn)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Clear();
                    ColorTools.SetConsoleColor(new Color($"0;120;212"), true, true);

                    // First, draw the upper left box
                    for (int X = UpperLeftBoxStartX, loopTo = UpperLeftBoxEndX; X <= loopTo; X++)
                    {
                        for (int Y = UpperLeftBoxStartY, loopTo1 = UpperLeftBoxEndY; Y <= loopTo1; Y++)
                        {
                            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Filling upper left box {0},{1}...", X, Y);
                            Console.SetCursorPosition(X, Y);
                            Console.Write(" ");
                        }
                    }

                    // Second, draw the lower left box
                    for (int X = LowerLeftBoxStartX, loopTo2 = LowerLeftBoxEndX; X <= loopTo2; X++)
                    {
                        for (int Y = LowerLeftBoxStartY, loopTo3 = LowerLeftBoxEndY; Y <= loopTo3; Y++)
                        {
                            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Filling lower left box {0},{1}...", X, Y);
                            Console.SetCursorPosition(X, Y);
                            Console.Write(" ");
                        }
                    }

                    // Third, draw the upper right box
                    for (int X = UpperRightBoxStartX, loopTo4 = UpperRightBoxEndX; X <= loopTo4; X++)
                    {
                        for (int Y = UpperRightBoxStartY, loopTo5 = UpperRightBoxEndY; Y <= loopTo5; Y++)
                        {
                            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Filling upper right box {0},{1}...", X, Y);
                            Console.SetCursorPosition(X, Y);
                            Console.Write(" ");
                        }
                    }

                    // Fourth, draw the lower right box
                    for (int X = LowerRightBoxStartX, loopTo6 = LowerRightBoxEndX; X <= loopTo6; X++)
                    {
                        for (int Y = LowerRightBoxStartY, loopTo7 = LowerRightBoxEndY; Y <= loopTo7; Y++)
                        {
                            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Filling lower right box {0},{1}...", X, Y);
                            Console.SetCursorPosition(X, Y);
                            Console.Write(" ");
                        }
                    }

                    // Set drawn
                    Drawn = true;
                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Drawn!");
                }
            }
            if (Drawn)
                ThreadManager.SleepNoBlock(1000L, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}