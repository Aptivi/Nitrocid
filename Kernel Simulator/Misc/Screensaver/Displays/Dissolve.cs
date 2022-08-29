
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
using System.Linq;
using System.Threading;
using ColorSeq;
using KS.ConsoleBase.Colors;
using KS.Kernel.Debugging;

namespace KS.Misc.Screensaver.Displays
{
    public static class DissolveSettings
    {

        private static bool _dissolve255Colors;
        private static bool _dissolveTrueColor = true;
        private static string _dissolveBackgroundColor = new Color((int)ConsoleColor.Black).PlainSequence;
        private static int _dissolveMinimumRedColorLevel = 0;
        private static int _dissolveMinimumGreenColorLevel = 0;
        private static int _dissolveMinimumBlueColorLevel = 0;
        private static int _dissolveMinimumColorLevel = 0;
        private static int _dissolveMaximumRedColorLevel = 255;
        private static int _dissolveMaximumGreenColorLevel = 255;
        private static int _dissolveMaximumBlueColorLevel = 255;
        private static int _dissolveMaximumColorLevel = 255;

        /// <summary>
        /// [Dissolve] Enable 255 color support. Has a higher priority than 16 color support.
        /// </summary>
        public static bool Dissolve255Colors
        {
            get
            {
                return _dissolve255Colors;
            }
            set
            {
                _dissolve255Colors = value;
            }
        }
        /// <summary>
        /// [Dissolve] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool DissolveTrueColor
        {
            get
            {
                return _dissolveTrueColor;
            }
            set
            {
                _dissolveTrueColor = value;
            }
        }
        /// <summary>
        /// [Dissolve] Screensaver background color
        /// </summary>
        public static string DissolveBackgroundColor
        {
            get
            {
                return _dissolveBackgroundColor;
            }
            set
            {
                _dissolveBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Dissolve] The minimum red color level (true color)
        /// </summary>
        public static int DissolveMinimumRedColorLevel
        {
            get
            {
                return _dissolveMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _dissolveMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Dissolve] The minimum green color level (true color)
        /// </summary>
        public static int DissolveMinimumGreenColorLevel
        {
            get
            {
                return _dissolveMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _dissolveMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Dissolve] The minimum blue color level (true color)
        /// </summary>
        public static int DissolveMinimumBlueColorLevel
        {
            get
            {
                return _dissolveMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _dissolveMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Dissolve] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int DissolveMinimumColorLevel
        {
            get
            {
                return _dissolveMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = _dissolve255Colors | _dissolveTrueColor ? 255 : 15;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _dissolveMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Dissolve] The maximum red color level (true color)
        /// </summary>
        public static int DissolveMaximumRedColorLevel
        {
            get
            {
                return _dissolveMaximumRedColorLevel;
            }
            set
            {
                if (value <= _dissolveMinimumRedColorLevel)
                    value = _dissolveMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _dissolveMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Dissolve] The maximum green color level (true color)
        /// </summary>
        public static int DissolveMaximumGreenColorLevel
        {
            get
            {
                return _dissolveMaximumGreenColorLevel;
            }
            set
            {
                if (value <= _dissolveMinimumGreenColorLevel)
                    value = _dissolveMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _dissolveMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Dissolve] The maximum blue color level (true color)
        /// </summary>
        public static int DissolveMaximumBlueColorLevel
        {
            get
            {
                return _dissolveMaximumBlueColorLevel;
            }
            set
            {
                if (value <= _dissolveMinimumBlueColorLevel)
                    value = _dissolveMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _dissolveMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Dissolve] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int DissolveMaximumColorLevel
        {
            get
            {
                return _dissolveMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = _dissolve255Colors | _dissolveTrueColor ? 255 : 15;
                if (value <= _dissolveMinimumColorLevel)
                    value = _dissolveMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _dissolveMaximumColorLevel = value;
            }
        }

    }

    public class DissolveDisplay : BaseScreensaver, IScreensaver
    {

        private Random RandomDriver;
        private bool ColorFilled;
        private int CurrentWindowWidth;
        private int CurrentWindowHeight;
        private bool ResizeSyncing;
        private readonly List<Tuple<int, int>> CoveredPositions = new();

        public override string ScreensaverName { get; set; } = "Dissolve";

        public override Dictionary<string, object> ScreensaverSettings { get; set; }

        public override void ScreensaverPreparation()
        {
            // Variable preparations
            RandomDriver = new Random();
            CurrentWindowWidth = ConsoleBase.ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleBase.ConsoleWrapper.WindowHeight;
            ColorTools.SetConsoleColor(new Color(DissolveSettings.DissolveBackgroundColor), true, true);
            ConsoleBase.ConsoleWrapper.Clear();
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleBase.ConsoleWrapper.WindowWidth, ConsoleBase.ConsoleWrapper.WindowHeight);
        }

        public override void ScreensaverLogic()
        {
            ConsoleBase.ConsoleWrapper.CursorVisible = false;
            if (ColorFilled)
                Thread.Sleep(1);
            int EndLeft = ConsoleBase.ConsoleWrapper.WindowWidth - 1;
            int EndTop = ConsoleBase.ConsoleWrapper.WindowHeight - 1;
            int Left = RandomDriver.Next(ConsoleBase.ConsoleWrapper.WindowWidth);
            int Top = RandomDriver.Next(ConsoleBase.ConsoleWrapper.WindowHeight);
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Dissolving: {0}", ColorFilled);
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "End left: {0} | End top: {1}", EndLeft, EndTop);
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got left: {0} | Got top: {1}", Left, Top);

            // Fill the color if not filled
            if (!ColorFilled)
            {
                // NOTICE: Mono seems to have a bug in KS.ConsoleBase.ConsoleWrapper.CursorLeft and KS.ConsoleBase.ConsoleWrapper.CursorTop when printing with VT escape sequences. For info, seek EB#2:7.
                if (!(ConsoleBase.ConsoleWrapper.CursorLeft >= EndLeft & ConsoleBase.ConsoleWrapper.CursorTop >= EndTop))
                {
                    if (DissolveSettings.DissolveTrueColor)
                    {
                        int RedColorNum = RandomDriver.Next(DissolveSettings.DissolveMinimumRedColorLevel, DissolveSettings.DissolveMaximumRedColorLevel);
                        int GreenColorNum = RandomDriver.Next(DissolveSettings.DissolveMinimumGreenColorLevel, DissolveSettings.DissolveMaximumGreenColorLevel);
                        int BlueColorNum = RandomDriver.Next(DissolveSettings.DissolveMinimumBlueColorLevel, DissolveSettings.DissolveMaximumBlueColorLevel);
                        DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                        if (CurrentWindowHeight != ConsoleBase.ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleBase.ConsoleWrapper.WindowWidth)
                            ResizeSyncing = true;
                        if (!ResizeSyncing)
                        {
                            ColorTools.SetConsoleColor(Color.Empty);
                            ColorTools.SetConsoleColor(new Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}"), true, true);
                            ConsoleBase.ConsoleWrapper.Write(" ");
                        }
                        else
                        {
                            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "We're refilling...");
                            ColorFilled = false;
                            ColorTools.SetConsoleColor(new Color(DissolveSettings.DissolveBackgroundColor), true, true);
                            ConsoleBase.ConsoleWrapper.Clear();
                            CoveredPositions.Clear();
                        }
                    }
                    else if (DissolveSettings.Dissolve255Colors)
                    {
                        int ColorNum = RandomDriver.Next(DissolveSettings.DissolveMinimumColorLevel, DissolveSettings.DissolveMaximumColorLevel);
                        DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                        if (CurrentWindowHeight != ConsoleBase.ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleBase.ConsoleWrapper.WindowWidth)
                            ResizeSyncing = true;
                        if (!ResizeSyncing)
                        {
                            ColorTools.SetConsoleColor(Color.Empty);
                            ColorTools.SetConsoleColor(new Color(ColorNum), true, true);
                            ConsoleBase.ConsoleWrapper.Write(" ");
                        }
                        else
                        {
                            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "We're refilling...");
                            ColorFilled = false;
                            ColorTools.SetConsoleColor(new Color(DissolveSettings.DissolveBackgroundColor), true, true);
                            ConsoleBase.ConsoleWrapper.Clear();
                            CoveredPositions.Clear();
                        }
                    }
                    else
                    {
                        if (CurrentWindowHeight != ConsoleBase.ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleBase.ConsoleWrapper.WindowWidth)
                            ResizeSyncing = true;
                        if (!ResizeSyncing)
                        {
                            ColorTools.SetConsoleColor(new Color(DissolveSettings.DissolveBackgroundColor), true, true);
                            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ConsoleBase.ConsoleWrapper.BackgroundColor);
                            ConsoleBase.ConsoleWrapper.Write(" ");
                        }
                        else
                        {
                            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "We're refilling...");
                            ColorFilled = false;
                            ColorTools.SetConsoleColor(new Color(DissolveSettings.DissolveBackgroundColor), true, true);
                            ConsoleBase.ConsoleWrapper.Clear();
                            CoveredPositions.Clear();
                        }
                    }
                }
                else
                {
                    DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "We're now dissolving... L: {0} = {1} | T: {2} = {3}", ConsoleBase.ConsoleWrapper.CursorLeft, EndLeft, ConsoleBase.ConsoleWrapper.CursorTop, EndTop);
                    ColorFilled = true;
                }
            }
            else
            {
                if (!CoveredPositions.Any(t => t.Item1 == Left & t.Item2 == Top))
                {
                    DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Covered position {0}", Left + " - " + Top);
                    CoveredPositions.Add(new Tuple<int, int>(Left, Top));
                    DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Covered positions: {0}/{1}", CoveredPositions.Count, (EndLeft + 1) * (EndTop + 1));
                }
                if (CurrentWindowHeight != ConsoleBase.ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleBase.ConsoleWrapper.WindowWidth)
                    ResizeSyncing = true;
                if (!ResizeSyncing)
                {
                    ConsoleBase.ConsoleWrapper.SetCursorPosition(Left, Top);
                    ColorTools.SetConsoleColor(new Color(DissolveSettings.DissolveBackgroundColor), true, true);
                    ConsoleBase.ConsoleWrapper.Write(" ");
                    if (CoveredPositions.Count == (EndLeft + 1) * (EndTop + 1))
                    {
                        DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "We're refilling...");
                        ColorFilled = false;
                        ColorTools.SetConsoleColor(new Color(DissolveSettings.DissolveBackgroundColor), true, true);
                        ConsoleBase.ConsoleWrapper.Clear();
                        CoveredPositions.Clear();
                    }
                }
                else
                {
                    DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "We're refilling...");
                    ColorFilled = false;
                    ColorTools.SetConsoleColor(new Color(DissolveSettings.DissolveBackgroundColor), true, true);
                    ConsoleBase.ConsoleWrapper.Clear();
                    CoveredPositions.Clear();
                }
            }

            // Reset resize sync
            ResizeSyncing = false;
            CurrentWindowWidth = ConsoleBase.ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleBase.ConsoleWrapper.WindowHeight;
        }

    }
}