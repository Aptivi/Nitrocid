
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

using System;
using System.Collections.Generic;
using ColorSeq;
using KS.ConsoleBase.Colors;
using KS.Kernel.Debugging;
using KS.Misc.Threading;

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
            CurrentWindowWidth = ConsoleBase.ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleBase.ConsoleWrapper.WindowHeight;
            ConsoleBase.ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
            ConsoleBase.ConsoleWrapper.Clear();
            DebugWriter.Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleBase.ConsoleWrapper.WindowWidth, ConsoleBase.ConsoleWrapper.WindowHeight);
        }

        public override void ScreensaverLogic()
        {
            ConsoleBase.ConsoleWrapper.CursorVisible = false;
            if (ResizeSyncing)
            {
                Drawn = false;

                // Reset resize sync
                ResizeSyncing = false;
                CurrentWindowWidth = ConsoleBase.ConsoleWrapper.WindowWidth;
                CurrentWindowHeight = ConsoleBase.ConsoleWrapper.WindowHeight;
            }
            else
            {
                if (CurrentWindowHeight != ConsoleBase.ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleBase.ConsoleWrapper.WindowWidth)
                    ResizeSyncing = true;

                // Get the required positions for the four boxes
                int UpperLeftBoxEndX = (int)Math.Round(ConsoleBase.ConsoleWrapper.WindowWidth / 2d - 1d);
                int UpperLeftBoxStartX = (int)Math.Round(UpperLeftBoxEndX / 2d);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Upper left box X position {0} -> {1}", UpperLeftBoxStartX, UpperLeftBoxEndX);

                int UpperLeftBoxStartY = 2;
                int UpperLeftBoxEndY = (int)Math.Round(ConsoleBase.ConsoleWrapper.WindowHeight / 2d - 1d);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Upper left box Y position {0} -> {1}", UpperLeftBoxStartY, UpperLeftBoxEndY);

                int LowerLeftBoxEndX = (int)Math.Round(ConsoleBase.ConsoleWrapper.WindowWidth / 2d - 1d);
                int LowerLeftBoxStartX = (int)Math.Round(LowerLeftBoxEndX / 2d);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Lower left box X position {0} -> {1}", LowerLeftBoxStartX, LowerLeftBoxEndX);

                int LowerLeftBoxStartY = (int)Math.Round(ConsoleBase.ConsoleWrapper.WindowHeight / 2d + 1d);
                int LowerLeftBoxEndY = ConsoleBase.ConsoleWrapper.WindowHeight - 2;
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Lower left box X position {0} -> {1}", LowerLeftBoxStartX, LowerLeftBoxEndX);

                int UpperRightBoxStartX = (int)Math.Round(ConsoleBase.ConsoleWrapper.WindowWidth / 2d + 2d);
                int UpperRightBoxEndX = (int)Math.Round(ConsoleBase.ConsoleWrapper.WindowWidth / 2d + UpperRightBoxStartX / 2d);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Upper right box X position {0} -> {1}", UpperRightBoxStartX, UpperRightBoxEndX);

                int UpperRightBoxStartY = 2;
                int UpperRightBoxEndY = (int)Math.Round(ConsoleBase.ConsoleWrapper.WindowHeight / 2d - 1d);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Upper right box X position {0} -> {1}", UpperRightBoxStartX, UpperRightBoxEndX);

                int LowerRightBoxStartX = (int)Math.Round(ConsoleBase.ConsoleWrapper.WindowWidth / 2d + 2d);
                int LowerRightBoxEndX = (int)Math.Round(ConsoleBase.ConsoleWrapper.WindowWidth / 2d + LowerRightBoxStartX / 2d);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Lower right box X position {0} -> {1}", LowerRightBoxStartX, LowerRightBoxEndX);

                int LowerRightBoxStartY = (int)Math.Round(ConsoleBase.ConsoleWrapper.WindowHeight / 2d + 1d);
                int LowerRightBoxEndY = ConsoleBase.ConsoleWrapper.WindowHeight - 2;
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Lower right box X position {0} -> {1}", LowerRightBoxStartX, LowerRightBoxEndX);

                // Draw the Windows 11 logo
                if (!Drawn)
                {
                    ConsoleBase.ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
                    ConsoleBase.ConsoleWrapper.Clear();
                    ColorTools.SetConsoleColor(new Color($"0;120;212"), true, true);

                    // First, draw the upper left box
                    for (int X = UpperLeftBoxStartX, loopTo = UpperLeftBoxEndX; X <= loopTo; X++)
                    {
                        for (int Y = UpperLeftBoxStartY, loopTo1 = UpperLeftBoxEndY; Y <= loopTo1; Y++)
                        {
                            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Filling upper left box {0},{1}...", X, Y);
                            ConsoleBase.ConsoleWrapper.SetCursorPosition(X, Y);
                            ConsoleBase.ConsoleWrapper.Write(" ");
                        }
                    }

                    // Second, draw the lower left box
                    for (int X = LowerLeftBoxStartX, loopTo2 = LowerLeftBoxEndX; X <= loopTo2; X++)
                    {
                        for (int Y = LowerLeftBoxStartY, loopTo3 = LowerLeftBoxEndY; Y <= loopTo3; Y++)
                        {
                            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Filling lower left box {0},{1}...", X, Y);
                            ConsoleBase.ConsoleWrapper.SetCursorPosition(X, Y);
                            ConsoleBase.ConsoleWrapper.Write(" ");
                        }
                    }

                    // Third, draw the upper right box
                    for (int X = UpperRightBoxStartX, loopTo4 = UpperRightBoxEndX; X <= loopTo4; X++)
                    {
                        for (int Y = UpperRightBoxStartY, loopTo5 = UpperRightBoxEndY; Y <= loopTo5; Y++)
                        {
                            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Filling upper right box {0},{1}...", X, Y);
                            ConsoleBase.ConsoleWrapper.SetCursorPosition(X, Y);
                            ConsoleBase.ConsoleWrapper.Write(" ");
                        }
                    }

                    // Fourth, draw the lower right box
                    for (int X = LowerRightBoxStartX, loopTo6 = LowerRightBoxEndX; X <= loopTo6; X++)
                    {
                        for (int Y = LowerRightBoxStartY, loopTo7 = LowerRightBoxEndY; Y <= loopTo7; Y++)
                        {
                            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Filling lower right box {0},{1}...", X, Y);
                            ConsoleBase.ConsoleWrapper.SetCursorPosition(X, Y);
                            ConsoleBase.ConsoleWrapper.Write(" ");
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