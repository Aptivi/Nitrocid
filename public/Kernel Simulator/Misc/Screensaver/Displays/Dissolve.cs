
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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
using KS.ConsoleBase;
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for Dissolve
    /// </summary>
    public static class DissolveSettings
    {

        private static bool _TrueColor = true;
        private static string _BackgroundColor = new Color((int)ConsoleColor.Black).PlainSequence;
        private static int _MinimumRedColorLevel = 0;
        private static int _MinimumGreenColorLevel = 0;
        private static int _MinimumBlueColorLevel = 0;
        private static int _MinimumColorLevel = 0;
        private static int _MaximumRedColorLevel = 255;
        private static int _MaximumGreenColorLevel = 255;
        private static int _MaximumBlueColorLevel = 255;
        private static int _MaximumColorLevel = 255;

        /// <summary>
        /// [Dissolve] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool DissolveTrueColor
        {
            get
            {
                return _TrueColor;
            }
            set
            {
                _TrueColor = value;
            }
        }
        /// <summary>
        /// [Dissolve] Screensaver background color
        /// </summary>
        public static string DissolveBackgroundColor
        {
            get
            {
                return _BackgroundColor;
            }
            set
            {
                _BackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Dissolve] The minimum red color level (true color)
        /// </summary>
        public static int DissolveMinimumRedColorLevel
        {
            get
            {
                return _MinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Dissolve] The minimum green color level (true color)
        /// </summary>
        public static int DissolveMinimumGreenColorLevel
        {
            get
            {
                return _MinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Dissolve] The minimum blue color level (true color)
        /// </summary>
        public static int DissolveMinimumBlueColorLevel
        {
            get
            {
                return _MinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Dissolve] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int DissolveMinimumColorLevel
        {
            get
            {
                return _MinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _MinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Dissolve] The maximum red color level (true color)
        /// </summary>
        public static int DissolveMaximumRedColorLevel
        {
            get
            {
                return _MaximumRedColorLevel;
            }
            set
            {
                if (value <= _MinimumRedColorLevel)
                    value = _MinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _MaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Dissolve] The maximum green color level (true color)
        /// </summary>
        public static int DissolveMaximumGreenColorLevel
        {
            get
            {
                return _MaximumGreenColorLevel;
            }
            set
            {
                if (value <= _MinimumGreenColorLevel)
                    value = _MinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _MaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Dissolve] The maximum blue color level (true color)
        /// </summary>
        public static int DissolveMaximumBlueColorLevel
        {
            get
            {
                return _MaximumBlueColorLevel;
            }
            set
            {
                if (value <= _MinimumBlueColorLevel)
                    value = _MinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _MaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Dissolve] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int DissolveMaximumColorLevel
        {
            get
            {
                return _MaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= _MinimumColorLevel)
                    value = _MinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _MaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for Dissolve
    /// </summary>
    public class DissolveDisplay : BaseScreensaver, IScreensaver
    {

        private bool ColorFilled;
        private readonly List<Tuple<int, int>> CoveredPositions = new();

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Dissolve";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ColorTools.LoadBack(new Color(DissolveSettings.DissolveBackgroundColor), true);
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;
            if (ColorFilled)
                Thread.Sleep(1);
            int EndLeft = ConsoleWrapper.WindowWidth - 1;
            int EndTop = ConsoleWrapper.WindowHeight - 1;
            int Left = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
            int Top = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Dissolving: {0}", ColorFilled);
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "End left: {0} | End top: {1}", EndLeft, EndTop);
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got left: {0} | Got top: {1}", Left, Top);

            // Fill the color if not filled
            if (!ColorFilled)
            {
                // NOTICE: Mono seems to have a bug in KS.ConsoleBase.ConsoleWrapper.CursorLeft and KS.ConsoleBase.ConsoleWrapper.CursorTop when printing with VT escape sequences. For info, seek EB#2:7.
                if (!(ConsoleWrapper.CursorLeft >= EndLeft & ConsoleWrapper.CursorTop >= EndTop))
                {
                    if (DissolveSettings.DissolveTrueColor)
                    {
                        int RedColorNum = RandomDriver.Random(DissolveSettings.DissolveMinimumRedColorLevel, DissolveSettings.DissolveMaximumRedColorLevel);
                        int GreenColorNum = RandomDriver.Random(DissolveSettings.DissolveMinimumGreenColorLevel, DissolveSettings.DissolveMaximumGreenColorLevel);
                        int BlueColorNum = RandomDriver.Random(DissolveSettings.DissolveMinimumBlueColorLevel, DissolveSettings.DissolveMaximumBlueColorLevel);
                        DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                        if (!ConsoleResizeListener.WasResized(false))
                        {
                            ColorTools.SetConsoleColor(Color.Empty);
                            ColorTools.SetConsoleColor(new Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}"), true, true);
                            ConsoleWrapper.Write(" ");
                            if (ConsoleWrapper.CursorLeft == ConsoleWrapper.WindowWidth - 1 &&
                                ConsoleWrapper.CursorTop < EndTop)
                            {
                                ConsoleWrapper.CursorLeft = 0;
                                ConsoleWrapper.CursorTop += 1;
                            }
                        }
                        else
                        {
                            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "We're refilling...");
                            ColorFilled = false;
                            ColorTools.LoadBack(new Color(DissolveSettings.DissolveBackgroundColor), true);
                            CoveredPositions.Clear();
                        }
                    }
                    else
                    {
                        int ColorNum = RandomDriver.Random(DissolveSettings.DissolveMinimumColorLevel, DissolveSettings.DissolveMaximumColorLevel);
                        DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                        if (!ConsoleResizeListener.WasResized(false))
                        {
                            ColorTools.SetConsoleColor(Color.Empty);
                            ColorTools.SetConsoleColor(new Color(ColorNum), true, true);
                            ConsoleWrapper.Write(" ");
                            if (ConsoleWrapper.CursorLeft == ConsoleWrapper.WindowWidth - 1 &&
                                ConsoleWrapper.CursorTop < EndTop)
                            {
                                ConsoleWrapper.CursorLeft = 0;
                                ConsoleWrapper.CursorTop += 1;
                            }
                        }
                        else
                        {
                            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "We're refilling...");
                            ColorFilled = false;
                            ColorTools.LoadBack(new Color(DissolveSettings.DissolveBackgroundColor), true);
                            CoveredPositions.Clear();
                        }
                    }
                }
                else
                {
                    DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "We're now dissolving... L: {0} = {1} | T: {2} = {3}", ConsoleWrapper.CursorLeft, EndLeft, ConsoleWrapper.CursorTop, EndTop);
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
                if (!ConsoleResizeListener.WasResized(false))
                {
                    ConsoleWrapper.SetCursorPosition(Left, Top);
                    ColorTools.SetConsoleColor(new Color(DissolveSettings.DissolveBackgroundColor), true, true);
                    ConsoleWrapper.Write(" ");
                    if (CoveredPositions.Count == (EndLeft + 1) * (EndTop + 1))
                    {
                        DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "We're refilling...");
                        ColorFilled = false;
                        ColorTools.LoadBack(new Color(DissolveSettings.DissolveBackgroundColor), true);
                        CoveredPositions.Clear();
                    }
                }
                else
                {
                    DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "We're refilling...");
                    ColorFilled = false;
                    ColorTools.LoadBack(new Color(DissolveSettings.DissolveBackgroundColor), true);
                    CoveredPositions.Clear();
                }
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
        }

    }
}
