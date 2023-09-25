
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using Terminaux.Colors;

namespace KS.Kernel.Configuration.Instances
{
    /// <summary>
    /// Screensaver kernel configuration instance
    /// </summary>
    public class KernelSaverConfig
    {
        /// <summary>
        /// [ColorMix] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool ColorMixTrueColor { get; set; } = true;
        /// <summary>
        /// [ColorMix] How many milliseconds to wait before making the next write?
        /// </summary>
        public int ColorMixDelay { get; set; } = 1;
        /// <summary>
        /// [ColorMix] Screensaver background color
        /// </summary>
        public string ColorMixBackgroundColor { get; set; } = new Color(ConsoleColors.Red).PlainSequence;
        /// <summary>
        /// [ColorMix] The minimum red color level (true color)
        /// </summary>
        public int ColorMixMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [ColorMix] The minimum green color level (true color)
        /// </summary>
        public int ColorMixMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [ColorMix] The minimum blue color level (true color)
        /// </summary>
        public int ColorMixMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [ColorMix] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int ColorMixMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [ColorMix] The maximum red color level (true color)
        /// </summary>
        public int ColorMixMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [ColorMix] The maximum green color level (true color)
        /// </summary>
        public int ColorMixMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [ColorMix] The maximum blue color level (true color)
        /// </summary>
        public int ColorMixMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [ColorMix] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int ColorMixMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [Disco] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool DiscoTrueColor { get; set; } = true;
        /// <summary>
        /// [Disco] How many milliseconds, or beats per minute, to wait before making the next write?
        /// </summary>
        public int DiscoDelay { get; set; } = 100;
        /// <summary>
        /// [Disco] Whether to use the Beats Per Minute (1/4) to change the writing delay. If False, will use the standard milliseconds delay instead.
        /// </summary>
        public bool DiscoUseBeatsPerMinute { get; set; }
        /// <summary>
        /// [Disco] Enable color cycling
        /// </summary>
        public bool DiscoCycleColors { get; set; }
        /// <summary>
        /// [Disco] Uses the black and white cycle to produce the same effect as the legacy "fed" screensaver introduced back in v0.0.1
        /// </summary>
        public bool DiscoEnableFedMode { get; set; }
        /// <summary>
        /// [Disco] The minimum red color level (true color)
        /// </summary>
        public int DiscoMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [Disco] The minimum green color level (true color)
        /// </summary>
        public int DiscoMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [Disco] The minimum blue color level (true color)
        /// </summary>
        public int DiscoMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [Disco] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int DiscoMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [Disco] The maximum red color level (true color)
        /// </summary>
        public int DiscoMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [Disco] The maximum green color level (true color)
        /// </summary>
        public int DiscoMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [Disco] The maximum blue color level (true color)
        /// </summary>
        public int DiscoMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [Disco] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int DiscoMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [GlitterColor] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool GlitterColorTrueColor { get; set; } = true;
        /// <summary>
        /// [GlitterColor] How many milliseconds to wait before making the next write?
        /// </summary>
        public int GlitterColorDelay { get; set; } = 1;
        /// <summary>
        /// [GlitterColor] The minimum red color level (true color)
        /// </summary>
        public int GlitterColorMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [GlitterColor] The minimum green color level (true color)
        /// </summary>
        public int GlitterColorMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [GlitterColor] The minimum blue color level (true color)
        /// </summary>
        public int GlitterColorMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [GlitterColor] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int GlitterColorMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [GlitterColor] The maximum red color level (true color)
        /// </summary>
        public int GlitterColorMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [GlitterColor] The maximum green color level (true color)
        /// </summary>
        public int GlitterColorMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [GlitterColor] The maximum blue color level (true color)
        /// </summary>
        public int GlitterColorMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [GlitterColor] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int GlitterColorMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [Lines] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool LinesTrueColor { get; set; } = true;
        /// <summary>
        /// [Lines] How many milliseconds to wait before making the next write?
        /// </summary>
        public int LinesDelay { get; set; } = 500;
        /// <summary>
        /// [Lines] Line character
        /// </summary>
        public string LinesLineChar { get; set; } = "-";
        /// <summary>
        /// [Lines] Screensaver background color
        /// </summary>
        public string LinesBackgroundColor { get; set; } = new Color(ConsoleColors.Black).PlainSequence;
        /// <summary>
        /// [Lines] The minimum red color level (true color)
        /// </summary>
        public int LinesMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [Lines] The minimum green color level (true color)
        /// </summary>
        public int LinesMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [Lines] The minimum blue color level (true color)
        /// </summary>
        public int LinesMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [Lines] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int LinesMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [Lines] The maximum red color level (true color)
        /// </summary>
        public int LinesMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [Lines] The maximum green color level (true color)
        /// </summary>
        public int LinesMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [Lines] The maximum blue color level (true color)
        /// </summary>
        public int LinesMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [Lines] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int LinesMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [Dissolve] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool DissolveTrueColor { get; set; } = true;
        /// <summary>
        /// [Dissolve] Screensaver background color
        /// </summary>
        public string DissolveBackgroundColor { get; set; } = new Color(ConsoleColors.Black).PlainSequence;
        /// <summary>
        /// [Dissolve] The minimum red color level (true color)
        /// </summary>
        public int DissolveMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [Dissolve] The minimum green color level (true color)
        /// </summary>
        public int DissolveMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [Dissolve] The minimum blue color level (true color)
        /// </summary>
        public int DissolveMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [Dissolve] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int DissolveMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [Dissolve] The maximum red color level (true color)
        /// </summary>
        public int DissolveMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [Dissolve] The maximum green color level (true color)
        /// </summary>
        public int DissolveMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [Dissolve] The maximum blue color level (true color)
        /// </summary>
        public int DissolveMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [Dissolve] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int DissolveMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [BouncingBlock] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool BouncingBlockTrueColor { get; set; } = true;
        /// <summary>
        /// [BouncingBlock] How many milliseconds to wait before making the next write?
        /// </summary>
        public int BouncingBlockDelay { get; set; } = 10;
        /// <summary>
        /// [BouncingBlock] Screensaver background color
        /// </summary>
        public string BouncingBlockBackgroundColor { get; set; } = new Color(ConsoleColors.Black).PlainSequence;
        /// <summary>
        /// [BouncingBlock] Screensaver foreground color
        /// </summary>
        public string BouncingBlockForegroundColor { get; set; } = new Color(ConsoleColors.White).PlainSequence;
        /// <summary>
        /// [BouncingBlock] The minimum red color level (true color)
        /// </summary>
        public int BouncingBlockMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [BouncingBlock] The minimum green color level (true color)
        /// </summary>
        public int BouncingBlockMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [BouncingBlock] The minimum blue color level (true color)
        /// </summary>
        public int BouncingBlockMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [BouncingBlock] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int BouncingBlockMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [BouncingBlock] The maximum red color level (true color)
        /// </summary>
        public int BouncingBlockMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [BouncingBlock] The maximum green color level (true color)
        /// </summary>
        public int BouncingBlockMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [BouncingBlock] The maximum blue color level (true color)
        /// </summary>
        public int BouncingBlockMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [BouncingBlock] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int BouncingBlockMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [ProgressClock] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool ProgressClockTrueColor { get; set; } = true;
        /// <summary>
        /// [ProgressClock] Enable color cycling (uses RNG. If disabled, uses the <see cref="ProgressClockSecondsProgressColor"/>, <see cref="ProgressClockMinutesProgressColor"/>, and <see cref="ProgressClockHoursProgressColor"/> colors.)
        /// </summary>
        public bool ProgressClockCycleColors { get; set; } = true;
        /// <summary>
        /// [ProgressClock] If color cycling is enabled, how many ticks before changing colors? 1 tick = 0.5 seconds
        /// </summary>
        public int ProgressClockCycleColorsTicks { get; set; } = 20;
        /// <summary>
        /// [ProgressClock] The color of seconds progress bar. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public string ProgressClockSecondsProgressColor { get; set; } = "4";
        /// <summary>
        /// [ProgressClock] The color of minutes progress bar. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public string ProgressClockMinutesProgressColor { get; set; } = "5";
        /// <summary>
        /// [ProgressClock] The color of hours progress bar. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public string ProgressClockHoursProgressColor { get; set; } = "6";
        /// <summary>
        /// [ProgressClock] The color of date information. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public string ProgressClockProgressColor { get; set; } = "7";
        /// <summary>
        /// [ProgressClock] How many milliseconds to wait before making the next write?
        /// </summary>
        public int ProgressClockDelay { get; set; } = 500;
        /// <summary>
        /// [ProgressClock] Upper left corner character for hours bar
        /// </summary>
        public char ProgressClockUpperLeftCornerCharHours { get; set; } = '╔';
        /// <summary>
        /// [ProgressClock] Upper left corner character for minutes bar
        /// </summary>
        public char ProgressClockUpperLeftCornerCharMinutes { get; set; } = '╔';
        /// <summary>
        /// [ProgressClock] Upper left corner character for seconds bar
        /// </summary>
        public char ProgressClockUpperLeftCornerCharSeconds { get; set; } = '╔';
        /// <summary>
        /// [ProgressClock] Upper right corner character for hours bar
        /// </summary>
        public char ProgressClockUpperRightCornerCharHours { get; set; } = '╗';
        /// <summary>
        /// [ProgressClock] Upper right corner character for minutes bar
        /// </summary>
        public char ProgressClockUpperRightCornerCharMinutes { get; set; } = '╗';
        /// <summary>
        /// [ProgressClock] Upper right corner character for seconds bar
        /// </summary>
        public char ProgressClockUpperRightCornerCharSeconds { get; set; } = '╗';
        /// <summary>
        /// [ProgressClock] Lower left corner character for hours bar
        /// </summary>
        public char ProgressClockLowerLeftCornerCharHours { get; set; } = '╚';
        /// <summary>
        /// [ProgressClock] Lower left corner character for minutes bar
        /// </summary>
        public char ProgressClockLowerLeftCornerCharMinutes { get; set; } = '╚';
        /// <summary>
        /// [ProgressClock] Lower left corner character for seconds bar
        /// </summary>
        public char ProgressClockLowerLeftCornerCharSeconds { get; set; } = '╚';
        /// <summary>
        /// [ProgressClock] Lower right corner character for hours bar
        /// </summary>
        public char ProgressClockLowerRightCornerCharHours { get; set; } = '╝';
        /// <summary>
        /// [ProgressClock] Lower right corner character for minutes bar
        /// </summary>
        public char ProgressClockLowerRightCornerCharMinutes { get; set; } = '╝';
        /// <summary>
        /// [ProgressClock] Lower right corner character for seconds bar
        /// </summary>
        public char ProgressClockLowerRightCornerCharSeconds { get; set; } = '╝';
        /// <summary>
        /// [ProgressClock] Upper frame character for hours bar
        /// </summary>
        public char ProgressClockUpperFrameCharHours { get; set; } = '═';
        /// <summary>
        /// [ProgressClock] Upper frame character for minutes bar
        /// </summary>
        public char ProgressClockUpperFrameCharMinutes { get; set; } = '═';
        /// <summary>
        /// [ProgressClock] Upper frame character for seconds bar
        /// </summary>
        public char ProgressClockUpperFrameCharSeconds { get; set; } = '═';
        /// <summary>
        /// [ProgressClock] Lower frame character for hours bar
        /// </summary>
        public char ProgressClockLowerFrameCharHours { get; set; } = '═';
        /// <summary>
        /// [ProgressClock] Lower frame character for minutes bar
        /// </summary>
        public char ProgressClockLowerFrameCharMinutes { get; set; } = '═';
        /// <summary>
        /// [ProgressClock] Lower frame character for seconds bar
        /// </summary>
        public char ProgressClockLowerFrameCharSeconds { get; set; } = '═';
        /// <summary>
        /// [ProgressClock] Left frame character for hours bar
        /// </summary>
        public char ProgressClockLeftFrameCharHours { get; set; } = '║';
        /// <summary>
        /// [ProgressClock] Left frame character for minutes bar
        /// </summary>
        public char ProgressClockLeftFrameCharMinutes { get; set; } = '║';
        /// <summary>
        /// [ProgressClock] Left frame character for seconds bar
        /// </summary>
        public char ProgressClockLeftFrameCharSeconds { get; set; } = '║';
        /// <summary>
        /// [ProgressClock] Right frame character for hours bar
        /// </summary>
        public char ProgressClockRightFrameCharHours { get; set; } = '║';
        /// <summary>
        /// [ProgressClock] Right frame character for minutes bar
        /// </summary>
        public char ProgressClockRightFrameCharMinutes { get; set; } = '║';
        /// <summary>
        /// [ProgressClock] Right frame character for seconds bar
        /// </summary>
        public char ProgressClockRightFrameCharSeconds { get; set; } = '║';
        /// <summary>
        /// [ProgressClock] Information text for hours bar
        /// </summary>
        public string ProgressClockInfoTextHours { get; set; } = "";
        /// <summary>
        /// [ProgressClock] Information text for minutes bar
        /// </summary>
        public string ProgressClockInfoTextMinutes { get; set; } = "";
        /// <summary>
        /// [ProgressClock] Information text for seconds bar
        /// </summary>
        public string ProgressClockInfoTextSeconds { get; set; } = "";
        /// <summary>
        /// [ProgressClock] The minimum red color level (true color - hours)
        /// </summary>
        public int ProgressClockMinimumRedColorLevelHours { get; set; } = 0;
        /// <summary>
        /// [ProgressClock] The minimum green color level (true color - hours)
        /// </summary>
        public int ProgressClockMinimumGreenColorLevelHours { get; set; } = 0;
        /// <summary>
        /// [ProgressClock] The minimum blue color level (true color - hours)
        /// </summary>
        public int ProgressClockMinimumBlueColorLevelHours { get; set; } = 0;
        /// <summary>
        /// [ProgressClock] The minimum color level (255 colors or 16 colors - hours)
        /// </summary>
        public int ProgressClockMinimumColorLevelHours { get; set; } = 1;
        /// <summary>
        /// [ProgressClock] The maximum red color level (true color - hours)
        /// </summary>
        public int ProgressClockMaximumRedColorLevelHours { get; set; } = 255;
        /// <summary>
        /// [ProgressClock] The maximum green color level (true color - hours)
        /// </summary>
        public int ProgressClockMaximumGreenColorLevelHours { get; set; } = 255;
        /// <summary>
        /// [ProgressClock] The maximum blue color level (true color - hours)
        /// </summary>
        public int ProgressClockMaximumBlueColorLevelHours { get; set; } = 255;
        /// <summary>
        /// [ProgressClock] The maximum color level (255 colors or 16 colors - hours)
        /// </summary>
        public int ProgressClockMaximumColorLevelHours { get; set; } = 255;
        /// <summary>
        /// [ProgressClock] The minimum red color level (true color - minutes)
        /// </summary>
        public int ProgressClockMinimumRedColorLevelMinutes { get; set; } = 0;
        /// <summary>
        /// [ProgressClock] The minimum green color level (true color - minutes)
        /// </summary>
        public int ProgressClockMinimumGreenColorLevelMinutes { get; set; } = 0;
        /// <summary>
        /// [ProgressClock] The minimum blue color level (true color - minutes)
        /// </summary>
        public int ProgressClockMinimumBlueColorLevelMinutes { get; set; } = 0;
        /// <summary>
        /// [ProgressClock] The minimum color level (255 colors or 16 colors - minutes)
        /// </summary>
        public int ProgressClockMinimumColorLevelMinutes { get; set; } = 1;
        /// <summary>
        /// [ProgressClock] The maximum red color level (true color - minutes)
        /// </summary>
        public int ProgressClockMaximumRedColorLevelMinutes { get; set; } = 255;
        /// <summary>
        /// [ProgressClock] The maximum green color level (true color - minutes)
        /// </summary>
        public int ProgressClockMaximumGreenColorLevelMinutes { get; set; } = 255;
        /// <summary>
        /// [ProgressClock] The maximum blue color level (true color - minutes)
        /// </summary>
        public int ProgressClockMaximumBlueColorLevelMinutes { get; set; } = 255;
        /// <summary>
        /// [ProgressClock] The maximum color level (255 colors or 16 colors - minutes)
        /// </summary>
        public int ProgressClockMaximumColorLevelMinutes { get; set; } = 255;
        /// <summary>
        /// [ProgressClock] The minimum red color level (true color - seconds)
        /// </summary>
        public int ProgressClockMinimumRedColorLevelSeconds { get; set; } = 0;
        /// <summary>
        /// [ProgressClock] The minimum green color level (true color - seconds)
        /// </summary>
        public int ProgressClockMinimumGreenColorLevelSeconds { get; set; } = 0;
        /// <summary>
        /// [ProgressClock] The minimum blue color level (true color - seconds)
        /// </summary>
        public int ProgressClockMinimumBlueColorLevelSeconds { get; set; } = 0;
        /// <summary>
        /// [ProgressClock] The minimum color level (255 colors or 16 colors - seconds)
        /// </summary>
        public int ProgressClockMinimumColorLevelSeconds { get; set; } = 1;
        /// <summary>
        /// [ProgressClock] The maximum red color level (true color - seconds)
        /// </summary>
        public int ProgressClockMaximumRedColorLevelSeconds { get; set; } = 255;
        /// <summary>
        /// [ProgressClock] The maximum green color level (true color - seconds)
        /// </summary>
        public int ProgressClockMaximumGreenColorLevelSeconds { get; set; } = 255;
        /// <summary>
        /// [ProgressClock] The maximum blue color level (true color - seconds)
        /// </summary>
        public int ProgressClockMaximumBlueColorLevelSeconds { get; set; } = 255;
        /// <summary>
        /// [ProgressClock] The maximum color level (255 colors or 16 colors - seconds)
        /// </summary>
        public int ProgressClockMaximumColorLevelSeconds { get; set; } = 255;
        /// <summary>
        /// [ProgressClock] The minimum red color level (true color)
        /// </summary>
        public int ProgressClockMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [ProgressClock] The minimum green color level (true color)
        /// </summary>
        public int ProgressClockMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [ProgressClock] The minimum blue color level (true color)
        /// </summary>
        public int ProgressClockMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [ProgressClock] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int ProgressClockMinimumColorLevel { get; set; } = 1;
        /// <summary>
        /// [ProgressClock] The maximum red color level (true color)
        /// </summary>
        public int ProgressClockMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [ProgressClock] The maximum green color level (true color)
        /// </summary>
        public int ProgressClockMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [ProgressClock] The maximum blue color level (true color)
        /// </summary>
        public int ProgressClockMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [ProgressClock] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int ProgressClockMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [Lighter] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool LighterTrueColor { get; set; } = true;
        /// <summary>
        /// [Lighter] How many milliseconds to wait before making the next write?
        /// </summary>
        public int LighterDelay { get; set; } = 100;
        /// <summary>
        /// [Lighter] How many positions to write before starting to blacken them?
        /// </summary>
        public int LighterMaxPositions { get; set; } = 10;
        /// <summary>
        /// [Lighter] Screensaver background color
        /// </summary>
        public string LighterBackgroundColor { get; set; } = new Color(ConsoleColors.Black).PlainSequence;
        /// <summary>
        /// [Lighter] The minimum red color level (true color)
        /// </summary>
        public int LighterMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [Lighter] The minimum green color level (true color)
        /// </summary>
        public int LighterMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [Lighter] The minimum blue color level (true color)
        /// </summary>
        public int LighterMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [Lighter] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int LighterMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [Lighter] The maximum red color level (true color)
        /// </summary>
        public int LighterMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [Lighter] The maximum green color level (true color)
        /// </summary>
        public int LighterMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [Lighter] The maximum blue color level (true color)
        /// </summary>
        public int LighterMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [Lighter] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int LighterMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [Wipe] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool WipeTrueColor { get; set; } = true;
        /// <summary>
        /// [Wipe] How many milliseconds to wait before making the next write?
        /// </summary>
        public int WipeDelay { get; set; } = 10;
        /// <summary>
        /// [Wipe] How many wipes needed to change direction?
        /// </summary>
        public int WipeWipesNeededToChangeDirection { get; set; } = 10;
        /// <summary>
        /// [Wipe] Screensaver background color
        /// </summary>
        public string WipeBackgroundColor { get; set; } = new Color(ConsoleColors.Black).PlainSequence;
        /// <summary>
        /// [Wipe] The minimum red color level (true color)
        /// </summary>
        public int WipeMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [Wipe] The minimum green color level (true color)
        /// </summary>
        public int WipeMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [Wipe] The minimum blue color level (true color)
        /// </summary>
        public int WipeMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [Wipe] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int WipeMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [Wipe] The maximum red color level (true color)
        /// </summary>
        public int WipeMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [Wipe] The maximum green color level (true color)
        /// </summary>
        public int WipeMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [Wipe] The maximum blue color level (true color)
        /// </summary>
        public int WipeMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [Wipe] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int WipeMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [SimpleMatrix] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SimpleMatrixDelay { get; set; } = 1;
        /// <summary>
        /// [GlitterMatrix] How many milliseconds to wait before making the next write?
        /// </summary>
        public int GlitterMatrixDelay { get; set; } = 1;
        /// <summary>
        /// [GlitterMatrix] Screensaver background color
        /// </summary>
        public string GlitterMatrixBackgroundColor { get; set; } = new Color(ConsoleColors.Black).PlainSequence;
        /// <summary>
        /// [GlitterMatrix] Screensaver foreground color
        /// </summary>
        public string GlitterMatrixForegroundColor { get; set; } = new Color(ConsoleColors.Green).PlainSequence;
        /// <summary>
        /// [BouncingText] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool BouncingTextTrueColor { get; set; } = true;
        /// <summary>
        /// [BouncingText] How many milliseconds to wait before making the next write?
        /// </summary>
        public int BouncingTextDelay { get; set; } = 50;
        /// <summary>
        /// [BouncingText] Text for Bouncing Text. Shorter is better.
        /// </summary>
        public string BouncingTextWrite { get; set; } = "Nitrocid KS";
        /// <summary>
        /// [BouncingText] Screensaver background color
        /// </summary>
        public string BouncingTextBackgroundColor { get; set; } = new Color(ConsoleColors.Black).PlainSequence;
        /// <summary>
        /// [BouncingText] Screensaver foreground color
        /// </summary>
        public string BouncingTextForegroundColor { get; set; } = new Color(ConsoleColors.White).PlainSequence;
        /// <summary>
        /// [BouncingText] The minimum red color level (true color)
        /// </summary>
        public int BouncingTextMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [BouncingText] The minimum green color level (true color)
        /// </summary>
        public int BouncingTextMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [BouncingText] The minimum blue color level (true color)
        /// </summary>
        public int BouncingTextMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [BouncingText] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int BouncingTextMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [BouncingText] The maximum red color level (true color)
        /// </summary>
        public int BouncingTextMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [BouncingText] The maximum green color level (true color)
        /// </summary>
        public int BouncingTextMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [BouncingText] The maximum blue color level (true color)
        /// </summary>
        public int BouncingTextMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [BouncingText] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int BouncingTextMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [Fader] How many milliseconds to wait before making the next write?
        /// </summary>
        public int FaderDelay { get; set; } = 50;
        /// <summary>
        /// [Fader] How many milliseconds to wait before fading the text out?
        /// </summary>
        public int FaderFadeOutDelay { get; set; } = 3000;
        /// <summary>
        /// [Fader] Text for Fader. Shorter is better.
        /// </summary>
        public string FaderWrite { get; set; } = "Nitrocid KS";
        /// <summary>
        /// [Fader] How many fade steps to do?
        /// </summary>
        public int FaderMaxSteps { get; set; } = 25;
        /// <summary>
        /// [Fader] Screensaver background color
        /// </summary>
        public string FaderBackgroundColor { get; set; } = new Color(0, 0, 0).PlainSequence;
        /// <summary>
        /// [Fader] The minimum red color level (true color)
        /// </summary>
        public int FaderMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [Fader] The minimum green color level (true color)
        /// </summary>
        public int FaderMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [Fader] The minimum blue color level (true color)
        /// </summary>
        public int FaderMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [Fader] The maximum red color level (true color)
        /// </summary>
        public int FaderMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [Fader] The maximum green color level (true color)
        /// </summary>
        public int FaderMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [Fader] The maximum blue color level (true color)
        /// </summary>
        public int FaderMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [FaderBack] How many milliseconds to wait before making the next write?
        /// </summary>
        public int FaderBackDelay { get; set; } = 10;
        /// <summary>
        /// [FaderBack] How many milliseconds to wait before fading the text out?
        /// </summary>
        public int FaderBackFadeOutDelay { get; set; } = 3000;
        /// <summary>
        /// [FaderBack] How many fade steps to do?
        /// </summary>
        public int FaderBackMaxSteps { get; set; } = 25;
        /// <summary>
        /// [FaderBack] The minimum red color level (true color)
        /// </summary>
        public int FaderBackMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [FaderBack] The minimum green color level (true color)
        /// </summary>
        public int FaderBackMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [FaderBack] The minimum blue color level (true color)
        /// </summary>
        public int FaderBackMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [FaderBack] The maximum red color level (true color)
        /// </summary>
        public int FaderBackMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [FaderBack] The maximum green color level (true color)
        /// </summary>
        public int FaderBackMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [FaderBack] The maximum blue color level (true color)
        /// </summary>
        public int FaderBackMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [BeatFader] Enable truecolor support. Has a higher priority than 255 color support. Please note that it only works if color cycling is enabled.
        /// </summary>
        public bool BeatFaderTrueColor { get; set; } = true;
        /// <summary>
        /// [BeatFader] How many beats per minute to wait before making the next write?
        /// </summary>
        public int BeatFaderDelay { get; set; } = 120;
        /// <summary>
        /// [BeatFader] Enable color cycling (uses RNG. If disabled, uses the <see cref="BeatFaderBeatColor"/> color.)
        /// </summary>
        public bool BeatFaderCycleColors { get; set; } = true;
        /// <summary>
        /// [BeatFader] The color of beats. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public string BeatFaderBeatColor { get; set; } = "17";
        /// <summary>
        /// [BeatFader] How many fade steps to do?
        /// </summary>
        public int BeatFaderMaxSteps { get; set; } = 25;
        /// <summary>
        /// [BeatFader] The minimum red color level (true color)
        /// </summary>
        public int BeatFaderMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [BeatFader] The minimum green color level (true color)
        /// </summary>
        public int BeatFaderMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [BeatFader] The minimum blue color level (true color)
        /// </summary>
        public int BeatFaderMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [BeatFader] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int BeatFaderMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [BeatFader] The maximum red color level (true color)
        /// </summary>
        public int BeatFaderMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [BeatFader] The maximum green color level (true color)
        /// </summary>
        public int BeatFaderMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [BeatFader] The maximum blue color level (true color)
        /// </summary>
        public int BeatFaderMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [BeatFader] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int BeatFaderMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [Typo] How many milliseconds to wait before making the next write?
        /// </summary>
        public int TypoDelay { get; set; } = 50;
        /// <summary>
        /// [Typo] How many milliseconds to wait before writing the text again?
        /// </summary>
        public int TypoWriteAgainDelay { get; set; } = 3000;
        /// <summary>
        /// [Typo] Text for Typo. Longer is better.
        /// </summary>
        public string TypoWrite { get; set; } = "Nitrocid KS";
        /// <summary>
        /// [Typo] Minimum writing speed in WPM
        /// </summary>
        public int TypoWritingSpeedMin { get; set; } = 50;
        /// <summary>
        /// [Typo] Maximum writing speed in WPM
        /// </summary>
        public int TypoWritingSpeedMax { get; set; } = 80;
        /// <summary>
        /// [Typo] Possibility that the writer made a typo in percent
        /// </summary>
        public int TypoMissStrikePossibility { get; set; } = 20;
        /// <summary>
        /// [Typo] Possibility that the writer missed a character in percent
        /// </summary>
        public int TypoMissPossibility { get; set; } = 10;
        /// <summary>
        /// [Typo] Text color
        /// </summary>
        public string TypoTextColor { get; set; } = new Color(ConsoleColors.White).PlainSequence;
        /// <summary>
        /// [Marquee] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool MarqueeTrueColor { get; set; } = true;
        /// <summary>
        /// [Marquee] How many milliseconds to wait before making the next write?
        /// </summary>
        public int MarqueeDelay { get; set; } = 10;
        /// <summary>
        /// [Marquee] Text for Marquee. Shorter is better.
        /// </summary>
        public string MarqueeWrite { get; set; } = "Nitrocid KS";
        /// <summary>
        /// [Marquee] Whether the text is always on center.
        /// </summary>
        public bool MarqueeAlwaysCentered { get; set; } = true;
        /// <summary>
        /// [Marquee] Whether to use the KS.ConsoleBase.ConsoleWrapper.Clear() API (slow) or use the line-clearing VT sequence (fast).
        /// </summary>
        public bool MarqueeUseConsoleAPI { get; set; }
        /// <summary>
        /// [Marquee] Screensaver background color
        /// </summary>
        public string MarqueeBackgroundColor { get; set; } = new Color(ConsoleColors.Black).PlainSequence;
        /// <summary>
        /// [Marquee] The minimum red color level (true color)
        /// </summary>
        public int MarqueeMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [Marquee] The minimum green color level (true color)
        /// </summary>
        public int MarqueeMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [Marquee] The minimum blue color level (true color)
        /// </summary>
        public int MarqueeMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [Marquee] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int MarqueeMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [Marquee] The maximum red color level (true color)
        /// </summary>
        public int MarqueeMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [Marquee] The maximum green color level (true color)
        /// </summary>
        public int MarqueeMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [Marquee] The maximum blue color level (true color)
        /// </summary>
        public int MarqueeMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [Marquee] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int MarqueeMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [Linotypo] How many milliseconds to wait before making the next write?
        /// </summary>
        public int LinotypoDelay { get; set; } = 50;
        /// <summary>
        /// [Linotypo] How many milliseconds to wait before writing the text in the new screen again?
        /// </summary>
        public int LinotypoNewScreenDelay { get; set; } = 3000;
        /// <summary>
        /// [Linotypo] Text for Linotypo. Longer is better.
        /// </summary>
        public string LinotypoWrite { get; set; } = "Nitrocid KS";
        /// <summary>
        /// [Linotypo] Minimum writing speed in WPM
        /// </summary>
        public int LinotypoWritingSpeedMin { get; set; } = 50;
        /// <summary>
        /// [Linotypo] Maximum writing speed in WPM
        /// </summary>
        public int LinotypoWritingSpeedMax { get; set; } = 80;
        /// <summary>
        /// [Linotypo] Possibility that the writer made a typo in percent
        /// </summary>
        public int LinotypoMissStrikePossibility { get; set; } = 1;
        /// <summary>
        /// [Linotypo] The text columns to be printed.
        /// </summary>
        public int LinotypoTextColumns { get; set; } = 3;
        /// <summary>
        /// [Linotypo] How many characters to write before triggering the "line fill"?
        /// </summary>
        public int LinotypoEtaoinThreshold { get; set; } = 5;
        /// <summary>
        /// [Linotypo] Possibility that the Etaoin pattern will be printed in all caps in percent
        /// </summary>
        public int LinotypoEtaoinCappingPossibility { get; set; } = 5;
        /// <summary>
        /// [Linotypo] Line fill pattern type
        /// </summary>
        public int LinotypoEtaoinType { get; set; } = 0;
        /// <summary>
        /// [Linotypo] Possibility that the writer missed a character in percent
        /// </summary>
        public int LinotypoMissPossibility { get; set; } = 10;
        /// <summary>
        /// [Linotypo] Text color
        /// </summary>
        public string LinotypoTextColor { get; set; } = new Color(ConsoleColors.White).PlainSequence;
        /// <summary>
        /// [Typewriter] How many milliseconds to wait before making the next write?
        /// </summary>
        public int TypewriterDelay { get; set; } = 50;
        /// <summary>
        /// [Typewriter] How many milliseconds to wait before writing the text in the new screen again?
        /// </summary>
        public int TypewriterNewScreenDelay { get; set; } = 3000;
        /// <summary>
        /// [Typewriter] Text for Typewriter. Longer is better.
        /// </summary>
        public string TypewriterWrite { get; set; } = "Nitrocid KS";
        /// <summary>
        /// [Typewriter] Minimum writing speed in WPM
        /// </summary>
        public int TypewriterWritingSpeedMin { get; set; } = 50;
        /// <summary>
        /// [Typewriter] Maximum writing speed in WPM
        /// </summary>
        public int TypewriterWritingSpeedMax { get; set; } = 80;
        /// <summary>
        /// [Typewriter] Shows the typewriter letter column position by showing this key on the bottom of the screen: <code>^</code>
        /// </summary>
        public bool TypewriterShowArrowPos { get; set; } = true;
        /// <summary>
        /// [Typewriter] Text color
        /// </summary>
        public string TypewriterTextColor { get; set; } = new Color(ConsoleColors.White).PlainSequence;
        /// <summary>
        /// [FlashColor] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool FlashColorTrueColor { get; set; } = true;
        /// <summary>
        /// [FlashColor] How many milliseconds to wait before making the next write?
        /// </summary>
        public int FlashColorDelay { get; set; } = 20;
        /// <summary>
        /// [FlashColor] Screensaver background color
        /// </summary>
        public string FlashColorBackgroundColor { get; set; } = new Color(ConsoleColors.Black).PlainSequence;
        /// <summary>
        /// [FlashColor] The minimum red color level (true color)
        /// </summary>
        public int FlashColorMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [FlashColor] The minimum green color level (true color)
        /// </summary>
        public int FlashColorMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [FlashColor] The minimum blue color level (true color)
        /// </summary>
        public int FlashColorMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [FlashColor] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int FlashColorMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [FlashColor] The maximum red color level (true color)
        /// </summary>
        public int FlashColorMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [FlashColor] The maximum green color level (true color)
        /// </summary>
        public int FlashColorMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [FlashColor] The maximum blue color level (true color)
        /// </summary>
        public int FlashColorMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [FlashColor] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int FlashColorMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [SpotWrite] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SpotWriteDelay { get; set; } = 100;
        /// <summary>
        /// [SpotWrite] How many milliseconds to wait before writing the text in the new screen again?
        /// </summary>
        public int SpotWriteNewScreenDelay { get; set; } = 3000;
        /// <summary>
        /// [SpotWrite] Text for SpotWrite. Longer is better.
        /// </summary>
        public string SpotWriteWrite { get; set; } = "Nitrocid KS";
        /// <summary>
        /// [SpotWrite] Text color
        /// </summary>
        public string SpotWriteTextColor { get; set; } = new Color(ConsoleColors.White).PlainSequence;
        /// <summary>
        /// [Ramp] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool RampTrueColor { get; set; } = true;
        /// <summary>
        /// [Ramp] How many milliseconds to wait before making the next write?
        /// </summary>
        public int RampDelay { get; set; } = 20;
        /// <summary>
        /// [Ramp] How many milliseconds to wait before starting the next ramp?
        /// </summary>
        public int RampNextRampDelay { get; set; } = 250;
        /// <summary>
        /// [Ramp] Upper left corner character 
        /// </summary>
        public char RampUpperLeftCornerChar { get; set; } = '╔';
        /// <summary>
        /// [Ramp] Upper right corner character 
        /// </summary>
        public char RampUpperRightCornerChar { get; set; } = '╗';
        /// <summary>
        /// [Ramp] Lower left corner character 
        /// </summary>
        public char RampLowerLeftCornerChar { get; set; } = '╚';
        /// <summary>
        /// [Ramp] Lower right corner character 
        /// </summary>
        public char RampLowerRightCornerChar { get; set; } = '╝';
        /// <summary>
        /// [Ramp] Upper frame character 
        /// </summary>
        public char RampUpperFrameChar { get; set; } = '═';
        /// <summary>
        /// [Ramp] Lower frame character 
        /// </summary>
        public char RampLowerFrameChar { get; set; } = '═';
        /// <summary>
        /// [Ramp] Left frame character 
        /// </summary>
        public char RampLeftFrameChar { get; set; } = '║';
        /// <summary>
        /// [Ramp] Right frame character 
        /// </summary>
        public char RampRightFrameChar { get; set; } = '║';
        /// <summary>
        /// [Ramp] The minimum red color level (true color - start)
        /// </summary>
        public int RampMinimumRedColorLevelStart { get; set; } = 0;
        /// <summary>
        /// [Ramp] The minimum green color level (true color - start)
        /// </summary>
        public int RampMinimumGreenColorLevelStart { get; set; } = 0;
        /// <summary>
        /// [Ramp] The minimum blue color level (true color - start)
        /// </summary>
        public int RampMinimumBlueColorLevelStart { get; set; } = 0;
        /// <summary>
        /// [Ramp] The minimum color level (255 colors or 16 colors - start)
        /// </summary>
        public int RampMinimumColorLevelStart { get; set; } = 0;
        /// <summary>
        /// [Ramp] The maximum red color level (true color - start)
        /// </summary>
        public int RampMaximumRedColorLevelStart { get; set; } = 255;
        /// <summary>
        /// [Ramp] The maximum green color level (true color - start)
        /// </summary>
        public int RampMaximumGreenColorLevelStart { get; set; } = 255;
        /// <summary>
        /// [Ramp] The maximum blue color level (true color - start)
        /// </summary>
        public int RampMaximumBlueColorLevelStart { get; set; } = 255;
        /// <summary>
        /// [Ramp] The maximum color level (255 colors or 16 colors - start)
        /// </summary>
        public int RampMaximumColorLevelStart { get; set; } = 255;
        /// <summary>
        /// [Ramp] The minimum red color level (true color - end)
        /// </summary>
        public int RampMinimumRedColorLevelEnd { get; set; } = 0;
        /// <summary>
        /// [Ramp] The minimum green color level (true color - end)
        /// </summary>
        public int RampMinimumGreenColorLevelEnd { get; set; } = 0;
        /// <summary>
        /// [Ramp] The minimum blue color level (true color - end)
        /// </summary>
        public int RampMinimumBlueColorLevelEnd { get; set; } = 0;
        /// <summary>
        /// [Ramp] The minimum color level (255 colors or 16 colors - end)
        /// </summary>
        public int RampMinimumColorLevelEnd { get; set; } = 0;
        /// <summary>
        /// [Ramp] The maximum red color level (true color - end)
        /// </summary>
        public int RampMaximumRedColorLevelEnd { get; set; } = 255;
        /// <summary>
        /// [Ramp] The maximum green color level (true color - end)
        /// </summary>
        public int RampMaximumGreenColorLevelEnd { get; set; } = 255;
        /// <summary>
        /// [Ramp] The maximum blue color level (true color - end)
        /// </summary>
        public int RampMaximumBlueColorLevelEnd { get; set; } = 255;
        /// <summary>
        /// [Ramp] The maximum color level (255 colors or 16 colors - end)
        /// </summary>
        public int RampMaximumColorLevelEnd { get; set; } = 255;
        /// <summary>
        /// [Ramp] Upper left corner color.
        /// </summary>
        public string RampUpperLeftCornerColor { get; set; } = "7";
        /// <summary>
        /// [Ramp] Upper right corner color.
        /// </summary>
        public string RampUpperRightCornerColor { get; set; } = "7";
        /// <summary>
        /// [Ramp] Lower left corner color.
        /// </summary>
        public string RampLowerLeftCornerColor { get; set; } = "7";
        /// <summary>
        /// [Ramp] Lower right corner color.
        /// </summary>
        public string RampLowerRightCornerColor { get; set; } = "7";
        /// <summary>
        /// [Ramp] Upper frame color.
        /// </summary>
        public string RampUpperFrameColor { get; set; } = "7";
        /// <summary>
        /// [Ramp] Lower frame color.
        /// </summary>
        public string RampLowerFrameColor { get; set; } = "7";
        /// <summary>
        /// [Ramp] Left frame color.
        /// </summary>
        public string RampLeftFrameColor { get; set; } = "7";
        /// <summary>
        /// [Ramp] Right frame color.
        /// </summary>
        public string RampRightFrameColor { get; set; } = "7";
        /// <summary>
        /// [Ramp] Use the border colors.
        /// </summary>
        public bool RampUseBorderColors { get; set; }
        /// <summary>
        /// [StackBox] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool StackBoxTrueColor { get; set; } = true;
        /// <summary>
        /// [StackBox] How many milliseconds to wait before making the next write?
        /// </summary>
        public int StackBoxDelay { get; set; } = 10;
        /// <summary>
        /// [StackBox] The minimum red color level (true color)
        /// </summary>
        public int StackBoxMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [StackBox] The minimum green color level (true color)
        /// </summary>
        public int StackBoxMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [StackBox] The minimum blue color level (true color)
        /// </summary>
        public int StackBoxMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [StackBox] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int StackBoxMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [StackBox] The maximum red color level (true color)
        /// </summary>
        public int StackBoxMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [StackBox] The maximum green color level (true color)
        /// </summary>
        public int StackBoxMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [StackBox] The maximum blue color level (true color)
        /// </summary>
        public int StackBoxMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [StackBox] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int StackBoxMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [StackBox] Whether to fill in the boxes drawn, or only draw the outline
        /// </summary>
        public bool StackBoxFill { get; set; } = true;
        /// <summary>
        /// [Snaker] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool SnakerTrueColor { get; set; } = true;
        /// <summary>
        /// [Snaker] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SnakerDelay { get; set; } = 100;
        /// <summary>
        /// [Snaker] How many milliseconds to wait before making the next stage?
        /// </summary>
        public int SnakerStageDelay { get; set; } = 5000;
        /// <summary>
        /// [Snaker] The minimum red color level (true color)
        /// </summary>
        public int SnakerMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [Snaker] The minimum green color level (true color)
        /// </summary>
        public int SnakerMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [Snaker] The minimum blue color level (true color)
        /// </summary>
        public int SnakerMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [Snaker] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int SnakerMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [Snaker] The maximum red color level (true color)
        /// </summary>
        public int SnakerMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [Snaker] The maximum green color level (true color)
        /// </summary>
        public int SnakerMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [Snaker] The maximum blue color level (true color)
        /// </summary>
        public int SnakerMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [Snaker] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int SnakerMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [BarRot] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool BarRotTrueColor { get; set; } = true;
        /// <summary>
        /// [BarRot] How many milliseconds to wait before making the next write?
        /// </summary>
        public int BarRotDelay { get; set; } = 10;
        /// <summary>
        /// [BarRot] How many milliseconds to wait before rotting the next ramp's one end?
        /// </summary>
        public int BarRotNextRampDelay { get; set; } = 250;
        /// <summary>
        /// [BarRot] Upper left corner character 
        /// </summary>
        public char BarRotUpperLeftCornerChar { get; set; } = '╔';
        /// <summary>
        /// [BarRot] Upper right corner character 
        /// </summary>
        public char BarRotUpperRightCornerChar { get; set; } = '╗';
        /// <summary>
        /// [BarRot] Lower left corner character 
        /// </summary>
        public char BarRotLowerLeftCornerChar { get; set; } = '╚';
        /// <summary>
        /// [BarRot] Lower right corner character 
        /// </summary>
        public char BarRotLowerRightCornerChar { get; set; } = '╝';
        /// <summary>
        /// [BarRot] Upper frame character 
        /// </summary>
        public char BarRotUpperFrameChar { get; set; } = '═';
        /// <summary>
        /// [BarRot] Lower frame character 
        /// </summary>
        public char BarRotLowerFrameChar { get; set; } = '═';
        /// <summary>
        /// [BarRot] Left frame character 
        /// </summary>
        public char BarRotLeftFrameChar { get; set; } = '║';
        /// <summary>
        /// [BarRot] Right frame character 
        /// </summary>
        public char BarRotRightFrameChar { get; set; } = '║';
        /// <summary>
        /// [BarRot] The minimum red color level (true color - start)
        /// </summary>
        public int BarRotMinimumRedColorLevelStart { get; set; } = 0;
        /// <summary>
        /// [BarRot] The minimum green color level (true color - start)
        /// </summary>
        public int BarRotMinimumGreenColorLevelStart { get; set; } = 0;
        /// <summary>
        /// [BarRot] The minimum blue color level (true color - start)
        /// </summary>
        public int BarRotMinimumBlueColorLevelStart { get; set; } = 0;
        /// <summary>
        /// [BarRot] The maximum red color level (true color - start)
        /// </summary>
        public int BarRotMaximumRedColorLevelStart { get; set; } = 255;
        /// <summary>
        /// [BarRot] The maximum green color level (true color - start)
        /// </summary>
        public int BarRotMaximumGreenColorLevelStart { get; set; } = 255;
        /// <summary>
        /// [BarRot] The maximum blue color level (true color - start)
        /// </summary>
        public int BarRotMaximumBlueColorLevelStart { get; set; } = 255;
        /// <summary>
        /// [BarRot] The minimum red color level (true color - end)
        /// </summary>
        public int BarRotMinimumRedColorLevelEnd { get; set; } = 0;
        /// <summary>
        /// [BarRot] The minimum green color level (true color - end)
        /// </summary>
        public int BarRotMinimumGreenColorLevelEnd { get; set; } = 0;
        /// <summary>
        /// [BarRot] The minimum blue color level (true color - end)
        /// </summary>
        public int BarRotMinimumBlueColorLevelEnd { get; set; } = 0;
        /// <summary>
        /// [BarRot] The maximum red color level (true color - end)
        /// </summary>
        public int BarRotMaximumRedColorLevelEnd { get; set; } = 255;
        /// <summary>
        /// [BarRot] The maximum green color level (true color - end)
        /// </summary>
        public int BarRotMaximumGreenColorLevelEnd { get; set; } = 255;
        /// <summary>
        /// [BarRot] The maximum blue color level (true color - end)
        /// </summary>
        public int BarRotMaximumBlueColorLevelEnd { get; set; } = 255;
        /// <summary>
        /// [BarRot] Upper left corner color.
        /// </summary>
        public string BarRotUpperLeftCornerColor { get; set; } = "192;192;192";
        /// <summary>
        /// [BarRot] Upper right corner color.
        /// </summary>
        public string BarRotUpperRightCornerColor { get; set; } = "192;192;192";
        /// <summary>
        /// [BarRot] Lower left corner color.
        /// </summary>
        public string BarRotLowerLeftCornerColor { get; set; } = "192;192;192";
        /// <summary>
        /// [BarRot] Lower right corner color.
        /// </summary>
        public string BarRotLowerRightCornerColor { get; set; } = "192;192;192";
        /// <summary>
        /// [BarRot] Upper frame color.
        /// </summary>
        public string BarRotUpperFrameColor { get; set; } = "192;192;192";
        /// <summary>
        /// [BarRot] Lower frame color.
        /// </summary>
        public string BarRotLowerFrameColor { get; set; } = "192;192;192";
        /// <summary>
        /// [BarRot] Left frame color.
        /// </summary>
        public string BarRotLeftFrameColor { get; set; } = "192;192;192";
        /// <summary>
        /// [BarRot] Right frame color.
        /// </summary>
        public string BarRotRightFrameColor { get; set; } = "192;192;192";
        /// <summary>
        /// [BarRot] Use the border colors.
        /// </summary>
        public bool BarRotUseBorderColors { get; set; }
        /// <summary>
        /// [Fireworks] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool FireworksTrueColor { get; set; } = true;
        /// <summary>
        /// [Fireworks] How many milliseconds to wait before making the next write?
        /// </summary>
        public int FireworksDelay { get; set; } = 50;
        /// <summary>
        /// [Fireworks] The radius of the explosion
        /// </summary>
        public int FireworksRadius { get; set; } = 5;
        /// <summary>
        /// [Fireworks] The minimum red color level (true color)
        /// </summary>
        public int FireworksMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [Fireworks] The minimum green color level (true color)
        /// </summary>
        public int FireworksMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [Fireworks] The minimum blue color level (true color)
        /// </summary>
        public int FireworksMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [Fireworks] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int FireworksMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [Fireworks] The maximum red color level (true color)
        /// </summary>
        public int FireworksMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [Fireworks] The maximum green color level (true color)
        /// </summary>
        public int FireworksMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [Fireworks] The maximum blue color level (true color)
        /// </summary>
        public int FireworksMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [Fireworks] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int FireworksMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [Figlet] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool FigletTrueColor { get; set; } = true;
        /// <summary>
        /// [Figlet] How many milliseconds to wait before making the next write?
        /// </summary>
        public int FigletDelay { get; set; } = 1000;
        /// <summary>
        /// [Figlet] Text for Figlet. Shorter is better.
        /// </summary>
        public string FigletText { get; set; } = "Nitrocid KS";
        /// <summary>
        /// [Figlet] Figlet font supported by the figlet library used.
        /// </summary>
        public string FigletFont { get; set; } = "small";
        /// <summary>
        /// [Figlet] The minimum red color level (true color)
        /// </summary>
        public int FigletMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [Figlet] The minimum green color level (true color)
        /// </summary>
        public int FigletMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [Figlet] The minimum blue color level (true color)
        /// </summary>
        public int FigletMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [Figlet] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int FigletMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [Figlet] The maximum red color level (true color)
        /// </summary>
        public int FigletMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [Figlet] The maximum green color level (true color)
        /// </summary>
        public int FigletMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [Figlet] The maximum blue color level (true color)
        /// </summary>
        public int FigletMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [Figlet] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int FigletMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [FlashText] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool FlashTextTrueColor { get; set; } = true;
        /// <summary>
        /// [FlashText] How many milliseconds to wait before making the next write?
        /// </summary>
        public int FlashTextDelay { get; set; } = 100;
        /// <summary>
        /// [FlashText] Text for FlashText. Shorter is better.
        /// </summary>
        public string FlashTextWrite { get; set; } = "Nitrocid KS";
        /// <summary>
        /// [FlashText] Screensaver background color
        /// </summary>
        public string FlashTextBackgroundColor { get; set; } = new Color(ConsoleColors.Black).PlainSequence;
        /// <summary>
        /// [FlashText] The minimum red color level (true color)
        /// </summary>
        public int FlashTextMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [FlashText] The minimum green color level (true color)
        /// </summary>
        public int FlashTextMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [FlashText] The minimum blue color level (true color)
        /// </summary>
        public int FlashTextMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [FlashText] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int FlashTextMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [FlashText] The maximum red color level (true color)
        /// </summary>
        public int FlashTextMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [FlashText] The maximum green color level (true color)
        /// </summary>
        public int FlashTextMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [FlashText] The maximum blue color level (true color)
        /// </summary>
        public int FlashTextMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [FlashText] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int FlashTextMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [Noise] How many milliseconds to wait before making the new screen?
        /// </summary>
        public int NoiseNewScreenDelay { get; set; } = 5000;
        /// <summary>
        /// [Noise] The noise density in percent
        /// </summary>
        public int NoiseDensity { get; set; } = 40;
        /// <summary>
        /// [PersonLookup] How many milliseconds to wait before getting the new name?
        /// </summary>
        public int PersonLookupDelay { get; set; } = 75;
        /// <summary>
        /// [PersonLookup] How many milliseconds to show the looked up name?
        /// </summary>
        public int PersonLookupLookedUpDelay { get; set; } = 10000;
        /// <summary>
        /// [PersonLookup] Minimum names count
        /// </summary>
        public int PersonLookupMinimumNames { get; set; } = 10;
        /// <summary>
        /// [PersonLookup] Maximum names count
        /// </summary>
        public int PersonLookupMaximumNames { get; set; } = 100;
        /// <summary>
        /// [PersonLookup] Minimum age years
        /// </summary>
        public int PersonLookupMinimumAgeYears { get; set; } = 18;
        /// <summary>
        /// [PersonLookup] Maximum age years
        /// </summary>
        public int PersonLookupMaximumAgeYears { get; set; } = 100;
        /// <summary>
        /// [DateAndTime] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool DateAndTimeTrueColor { get; set; } = true;
        /// <summary>
        /// [DateAndTime] How many milliseconds to wait before making the next write?
        /// </summary>
        public int DateAndTimeDelay { get; set; } = 1000;
        /// <summary>
        /// [DateAndTime] The minimum red color level (true color)
        /// </summary>
        public int DateAndTimeMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [DateAndTime] The minimum green color level (true color)
        /// </summary>
        public int DateAndTimeMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [DateAndTime] The minimum blue color level (true color)
        /// </summary>
        public int DateAndTimeMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [DateAndTime] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int DateAndTimeMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [DateAndTime] The maximum red color level (true color)
        /// </summary>
        public int DateAndTimeMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [DateAndTime] The maximum green color level (true color)
        /// </summary>
        public int DateAndTimeMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [DateAndTime] The maximum blue color level (true color)
        /// </summary>
        public int DateAndTimeMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [DateAndTime] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int DateAndTimeMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [Glitch] How many milliseconds to wait before making the next write?
        /// </summary>
        public int GlitchDelay { get; set; } = 10;
        /// <summary>
        /// [Glitch] The Glitch density in percent
        /// </summary>
        public int GlitchDensity { get; set; } = 40;
        /// <summary>
        /// [FallingLine] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool FallingLineTrueColor { get; set; } = true;
        /// <summary>
        /// [FallingLine] How many milliseconds to wait before making the next write?
        /// </summary>
        public int FallingLineDelay { get; set; } = 10;
        /// <summary>
        /// [FallingLine] How many fade steps to do?
        /// </summary>
        public int FallingLineMaxSteps { get; set; } = 25;
        /// <summary>
        /// [FallingLine] The minimum red color level (true color)
        /// </summary>
        public int FallingLineMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [FallingLine] The minimum green color level (true color)
        /// </summary>
        public int FallingLineMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [FallingLine] The minimum blue color level (true color)
        /// </summary>
        public int FallingLineMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [FallingLine] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int FallingLineMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [FallingLine] The maximum red color level (true color)
        /// </summary>
        public int FallingLineMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [FallingLine] The maximum green color level (true color)
        /// </summary>
        public int FallingLineMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [FallingLine] The maximum blue color level (true color)
        /// </summary>
        public int FallingLineMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [FallingLine] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int FallingLineMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [Indeterminate] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool IndeterminateTrueColor { get; set; } = true;
        /// <summary>
        /// [Indeterminate] How many milliseconds to wait before making the next write?
        /// </summary>
        public int IndeterminateDelay { get; set; } = 20;
        /// <summary>
        /// [Indeterminate] Upper left corner character 
        /// </summary>
        public char IndeterminateUpperLeftCornerChar { get; set; } = '╔';
        /// <summary>
        /// [Indeterminate] Upper right corner character 
        /// </summary>
        public char IndeterminateUpperRightCornerChar { get; set; } = '╗';
        /// <summary>
        /// [Indeterminate] Lower left corner character 
        /// </summary>
        public char IndeterminateLowerLeftCornerChar { get; set; } = '╚';
        /// <summary>
        /// [Indeterminate] Lower right corner character 
        /// </summary>
        public char IndeterminateLowerRightCornerChar { get; set; } = '╝';
        /// <summary>
        /// [Indeterminate] Upper frame character 
        /// </summary>
        public char IndeterminateUpperFrameChar { get; set; } = '═';
        /// <summary>
        /// [Indeterminate] Lower frame character 
        /// </summary>
        public char IndeterminateLowerFrameChar { get; set; } = '═';
        /// <summary>
        /// [Indeterminate] Left frame character 
        /// </summary>
        public char IndeterminateLeftFrameChar { get; set; } = '║';
        /// <summary>
        /// [Indeterminate] Right frame character 
        /// </summary>
        public char IndeterminateRightFrameChar { get; set; } = '║';
        /// <summary>
        /// [Indeterminate] The minimum red color level (true color)
        /// </summary>
        public int IndeterminateMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [Indeterminate] The minimum green color level (true color)
        /// </summary>
        public int IndeterminateMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [Indeterminate] The minimum blue color level (true color)
        /// </summary>
        public int IndeterminateMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [Indeterminate] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int IndeterminateMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [Indeterminate] The maximum red color level (true color)
        /// </summary>
        public int IndeterminateMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [Indeterminate] The maximum green color level (true color)
        /// </summary>
        public int IndeterminateMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [Indeterminate] The maximum blue color level (true color)
        /// </summary>
        public int IndeterminateMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [Indeterminate] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int IndeterminateMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [Indeterminate] Upper left corner color.
        /// </summary>
        public string IndeterminateUpperLeftCornerColor { get; set; } = "7";
        /// <summary>
        /// [Indeterminate] Upper right corner color.
        /// </summary>
        public string IndeterminateUpperRightCornerColor { get; set; } = "7";
        /// <summary>
        /// [Indeterminate] Lower left corner color.
        /// </summary>
        public string IndeterminateLowerLeftCornerColor { get; set; } = "7";
        /// <summary>
        /// [Indeterminate] Lower right corner color.
        /// </summary>
        public string IndeterminateLowerRightCornerColor { get; set; } = "7";
        /// <summary>
        /// [Indeterminate] Upper frame color.
        /// </summary>
        public string IndeterminateUpperFrameColor { get; set; } = "7";
        /// <summary>
        /// [Indeterminate] Lower frame color.
        /// </summary>
        public string IndeterminateLowerFrameColor { get; set; } = "7";
        /// <summary>
        /// [Indeterminate] Left frame color.
        /// </summary>
        public string IndeterminateLeftFrameColor { get; set; } = "7";
        /// <summary>
        /// [Indeterminate] Right frame color.
        /// </summary>
        public string IndeterminateRightFrameColor { get; set; } = "7";
        /// <summary>
        /// [Indeterminate] Use the border colors.
        /// </summary>
        public bool IndeterminateUseBorderColors { get; set; }
        /// <summary>
        /// [Pulse] How many milliseconds to wait before making the next write?
        /// </summary>
        public int PulseDelay { get; set; } = 50;
        /// <summary>
        /// [Pulse] How many fade steps to do?
        /// </summary>
        public int PulseMaxSteps { get; set; } = 25;
        /// <summary>
        /// [Pulse] The minimum red color level (true color)
        /// </summary>
        public int PulseMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [Pulse] The minimum green color level (true color)
        /// </summary>
        public int PulseMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [Pulse] The minimum blue color level (true color)
        /// </summary>
        public int PulseMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [Pulse] The maximum red color level (true color)
        /// </summary>
        public int PulseMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [Pulse] The maximum green color level (true color)
        /// </summary>
        public int PulseMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [Pulse] The maximum blue color level (true color)
        /// </summary>
        public int PulseMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [BeatPulse] Enable truecolor support. Has a higher priority than 255 color support. Please note that it only works if color cycling is enabled.
        /// </summary>
        public bool BeatPulseTrueColor { get; set; } = true;
        /// <summary>
        /// [BeatPulse] How many milliseconds to wait before making the next write?
        /// </summary>
        public int BeatPulseDelay { get; set; } = 50;
        /// <summary>
        /// [BeatPulse] Enable color cycling (uses RNG. If disabled, uses the <see cref="BeatPulseBeatColor"/> color.)
        /// </summary>
        public bool BeatPulseCycleColors { get; set; } = true;
        /// <summary>
        /// [BeatPulse] The color of beats. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public string BeatPulseBeatColor { get; set; } = "17";
        /// <summary>
        /// [BeatPulse] How many fade steps to do?
        /// </summary>
        public int BeatPulseMaxSteps { get; set; } = 25;
        /// <summary>
        /// [BeatPulse] The minimum red color level (true color)
        /// </summary>
        public int BeatPulseMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [BeatPulse] The minimum green color level (true color)
        /// </summary>
        public int BeatPulseMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [BeatPulse] The minimum blue color level (true color)
        /// </summary>
        public int BeatPulseMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [BeatPulse] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int BeatPulseMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [BeatPulse] The maximum red color level (true color)
        /// </summary>
        public int BeatPulseMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [BeatPulse] The maximum green color level (true color)
        /// </summary>
        public int BeatPulseMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [BeatPulse] The maximum blue color level (true color)
        /// </summary>
        public int BeatPulseMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [BeatPulse] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int BeatPulseMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [EdgePulse] How many milliseconds to wait before making the next write?
        /// </summary>
        public int EdgePulseDelay { get; set; } = 50;
        /// <summary>
        /// [EdgePulse] How many fade steps to do?
        /// </summary>
        public int EdgePulseMaxSteps { get; set; } = 25;
        /// <summary>
        /// [EdgePulse] The minimum red color level (true color)
        /// </summary>
        public int EdgePulseMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [EdgePulse] The minimum green color level (true color)
        /// </summary>
        public int EdgePulseMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [EdgePulse] The minimum blue color level (true color)
        /// </summary>
        public int EdgePulseMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [EdgePulse] The maximum red color level (true color)
        /// </summary>
        public int EdgePulseMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [EdgePulse] The maximum green color level (true color)
        /// </summary>
        public int EdgePulseMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [EdgePulse] The maximum blue color level (true color)
        /// </summary>
        public int EdgePulseMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [BeatEdgePulse] Enable truecolor support. Has a higher priority than 255 color support. Please note that it only works if color cycling is enabled.
        /// </summary>
        public bool BeatEdgePulseTrueColor { get; set; } = true;
        /// <summary>
        /// [BeatEdgePulse] How many milliseconds to wait before making the next write?
        /// </summary>
        public int BeatEdgePulseDelay { get; set; } = 50;
        /// <summary>
        /// [BeatEdgePulse] Enable color cycling (uses RNG. If disabled, uses the <see cref="BeatEdgePulseBeatColor"/> color.)
        /// </summary>
        public bool BeatEdgePulseCycleColors { get; set; } = true;
        /// <summary>
        /// [BeatEdgePulse] The color of beats. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public string BeatEdgePulseBeatColor { get; set; } = "17";
        /// <summary>
        /// [BeatEdgePulse] How many fade steps to do?
        /// </summary>
        public int BeatEdgePulseMaxSteps { get; set; } = 25;
        /// <summary>
        /// [BeatEdgePulse] The minimum red color level (true color)
        /// </summary>
        public int BeatEdgePulseMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [BeatEdgePulse] The minimum green color level (true color)
        /// </summary>
        public int BeatEdgePulseMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [BeatEdgePulse] The minimum blue color level (true color)
        /// </summary>
        public int BeatEdgePulseMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [BeatEdgePulse] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int BeatEdgePulseMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [BeatEdgePulse] The maximum red color level (true color)
        /// </summary>
        public int BeatEdgePulseMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [BeatEdgePulse] The maximum green color level (true color)
        /// </summary>
        public int BeatEdgePulseMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [BeatEdgePulse] The maximum blue color level (true color)
        /// </summary>
        public int BeatEdgePulseMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [BeatEdgePulse] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int BeatEdgePulseMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [GradientRot] How many milliseconds to wait before making the next write?
        /// </summary>
        public int GradientRotDelay { get; set; } = 10;
        /// <summary>
        /// [GradientRot] How many milliseconds to wait before rotting the next screen?
        /// </summary>
        public int GradientRotNextRampDelay { get; set; } = 250;
        /// <summary>
        /// [GradientRot] The minimum red color level (true color - start)
        /// </summary>
        public int GradientRotMinimumRedColorLevelStart { get; set; } = 0;
        /// <summary>
        /// [GradientRot] The minimum green color level (true color - start)
        /// </summary>
        public int GradientRotMinimumGreenColorLevelStart { get; set; } = 0;
        /// <summary>
        /// [GradientRot] The minimum blue color level (true color - start)
        /// </summary>
        public int GradientRotMinimumBlueColorLevelStart { get; set; } = 0;
        /// <summary>
        /// [GradientRot] The maximum red color level (true color - start)
        /// </summary>
        public int GradientRotMaximumRedColorLevelStart { get; set; } = 255;
        /// <summary>
        /// [GradientRot] The maximum green color level (true color - start)
        /// </summary>
        public int GradientRotMaximumGreenColorLevelStart { get; set; } = 255;
        /// <summary>
        /// [GradientRot] The maximum blue color level (true color - start)
        /// </summary>
        public int GradientRotMaximumBlueColorLevelStart { get; set; } = 255;
        /// <summary>
        /// [GradientRot] The minimum red color level (true color - end)
        /// </summary>
        public int GradientRotMinimumRedColorLevelEnd { get; set; } = 0;
        /// <summary>
        /// [GradientRot] The minimum green color level (true color - end)
        /// </summary>
        public int GradientRotMinimumGreenColorLevelEnd { get; set; } = 0;
        /// <summary>
        /// [GradientRot] The minimum blue color level (true color - end)
        /// </summary>
        public int GradientRotMinimumBlueColorLevelEnd { get; set; } = 0;
        /// <summary>
        /// [GradientRot] The maximum red color level (true color - end)
        /// </summary>
        public int GradientRotMaximumRedColorLevelEnd { get; set; } = 255;
        /// <summary>
        /// [GradientRot] The maximum green color level (true color - end)
        /// </summary>
        public int GradientRotMaximumGreenColorLevelEnd { get; set; } = 255;
        /// <summary>
        /// [GradientRot] The maximum blue color level (true color - end)
        /// </summary>
        public int GradientRotMaximumBlueColorLevelEnd { get; set; } = 255;
        /// <summary>
        /// [Gradient] How many milliseconds to wait before rotting the next screen?
        /// </summary>
        public int GradientNextRampDelay { get; set; } = 250;
        /// <summary>
        /// [Gradient] The minimum red color level (true color - start)
        /// </summary>
        public int GradientMinimumRedColorLevelStart { get; set; } = 0;
        /// <summary>
        /// [Gradient] The minimum green color level (true color - start)
        /// </summary>
        public int GradientMinimumGreenColorLevelStart { get; set; } = 0;
        /// <summary>
        /// [Gradient] The minimum blue color level (true color - start)
        /// </summary>
        public int GradientMinimumBlueColorLevelStart { get; set; } = 0;
        /// <summary>
        /// [Gradient] The maximum red color level (true color - start)
        /// </summary>
        public int GradientMaximumRedColorLevelStart { get; set; } = 255;
        /// <summary>
        /// [Gradient] The maximum green color level (true color - start)
        /// </summary>
        public int GradientMaximumGreenColorLevelStart { get; set; } = 255;
        /// <summary>
        /// [Gradient] The maximum blue color level (true color - start)
        /// </summary>
        public int GradientMaximumBlueColorLevelStart { get; set; } = 255;
        /// <summary>
        /// [Gradient] The minimum red color level (true color - end)
        /// </summary>
        public int GradientMinimumRedColorLevelEnd { get; set; } = 0;
        /// <summary>
        /// [Gradient] The minimum green color level (true color - end)
        /// </summary>
        public int GradientMinimumGreenColorLevelEnd { get; set; } = 0;
        /// <summary>
        /// [Gradient] The minimum blue color level (true color - end)
        /// </summary>
        public int GradientMinimumBlueColorLevelEnd { get; set; } = 0;
        /// <summary>
        /// [Gradient] The maximum red color level (true color - end)
        /// </summary>
        public int GradientMaximumRedColorLevelEnd { get; set; } = 255;
        /// <summary>
        /// [Gradient] The maximum green color level (true color - end)
        /// </summary>
        public int GradientMaximumGreenColorLevelEnd { get; set; } = 255;
        /// <summary>
        /// [Gradient] The maximum blue color level (true color - end)
        /// </summary>
        public int GradientMaximumBlueColorLevelEnd { get; set; } = 255;
        /// <summary>
        /// [Lightspeed] Enable color cycling
        /// </summary>
        public bool LightspeedCycleColors { get; set; }
        /// <summary>
        /// [Lightspeed] The minimum red color level (true color)
        /// </summary>
        public int LightspeedMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [Lightspeed] The minimum green color level (true color)
        /// </summary>
        public int LightspeedMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [Lightspeed] The minimum blue color level (true color)
        /// </summary>
        public int LightspeedMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [Lightspeed] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int LightspeedMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [Lightspeed] The maximum red color level (true color)
        /// </summary>
        public int LightspeedMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [Lightspeed] The maximum green color level (true color)
        /// </summary>
        public int LightspeedMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [Lightspeed] The maximum blue color level (true color)
        /// </summary>
        public int LightspeedMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [Lightspeed] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int LightspeedMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [Starfield] How many milliseconds to wait before making the next write?
        /// </summary>
        public int StarfieldDelay { get; set; } = 10;
        /// <summary>
        /// [Siren] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SirenDelay { get; set; } = 500;
        /// <summary>
        /// [Siren] The siren style
        /// </summary>
        public string SirenStyle { get; set; } = "Cop";
        /// <summary>
        /// [Spin] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SpinDelay { get; set; } = 10;
        /// <summary>
        /// [SnakeFill] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool SnakeFillTrueColor { get; set; } = true;
        /// <summary>
        /// [SnakeFill] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SnakeFillDelay { get; set; } = 10;
        /// <summary>
        /// [SnakeFill] The minimum red color level (true color)
        /// </summary>
        public int SnakeFillMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [SnakeFill] The minimum green color level (true color)
        /// </summary>
        public int SnakeFillMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [SnakeFill] The minimum blue color level (true color)
        /// </summary>
        public int SnakeFillMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [SnakeFill] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int SnakeFillMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [SnakeFill] The maximum red color level (true color)
        /// </summary>
        public int SnakeFillMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [SnakeFill] The maximum green color level (true color)
        /// </summary>
        public int SnakeFillMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [SnakeFill] The maximum blue color level (true color)
        /// </summary>
        public int SnakeFillMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [SnakeFill] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int SnakeFillMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [Equalizer] How many milliseconds to wait before going to next equalizer preset?
        /// </summary>
        public int EqualizerNextScreenDelay { get; set; } = 3000;
        /// <summary>
        /// [BSOD] How many beats per minute to wait before making the next write?
        /// </summary>
        public int BSODDelay { get; set; } = 10000;
        /// <summary>
        /// [Memdump] How many milliseconds to wait before making the next write?
        /// </summary>
        public int MemdumpDelay { get; set; } = 500;
        /// <summary>
        /// [Lyrics] How many milliseconds to wait before the next lyric?
        /// </summary>
        public int LyricsDelay { get; set; } = 10000;
        /// <summary>
        /// [ExcaliBeats] Enable truecolor support. Has a higher priority than 255 color support. Please note that it only works if color cycling is enabled.
        /// </summary>
        public bool ExcaliBeatsTrueColor { get; set; } = true;
        /// <summary>
        /// [ExcaliBeats] How many beats per minute to wait before making the next write?
        /// </summary>
        public int ExcaliBeatsDelay { get; set; } = 140;
        /// <summary>
        /// [ExcaliBeats] Enable color cycling (uses RNG. If disabled, uses the <see cref="ExcaliBeatsBeatColor"/> color.)
        /// </summary>
        public bool ExcaliBeatsCycleColors { get; set; } = true;
        /// <summary>
        /// [ExcaliBeats] Explicitly change the text to Excalibur
        /// </summary>
        public bool ExcaliBeatsExplicit { get; set; } = true;
        /// <summary>
        /// [ExcaliBeats] [Linux only] Trance mode - Multiplies the BPM by 2 to simulate the trance music style
        /// </summary>
        public bool ExcaliBeatsTranceMode { get; set; }
        /// <summary>
        /// [ExcaliBeats] The color of beats. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public string ExcaliBeatsBeatColor { get; set; } = "17";
        /// <summary>
        /// [ExcaliBeats] How many fade steps to do?
        /// </summary>
        public int ExcaliBeatsMaxSteps { get; set; } = 25;
        /// <summary>
        /// [ExcaliBeats] The minimum red color level (true color)
        /// </summary>
        public int ExcaliBeatsMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [ExcaliBeats] The minimum green color level (true color)
        /// </summary>
        public int ExcaliBeatsMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [ExcaliBeats] The minimum blue color level (true color)
        /// </summary>
        public int ExcaliBeatsMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [ExcaliBeats] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int ExcaliBeatsMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [ExcaliBeats] The maximum red color level (true color)
        /// </summary>
        public int ExcaliBeatsMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [ExcaliBeats] The maximum green color level (true color)
        /// </summary>
        public int ExcaliBeatsMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [ExcaliBeats] The maximum blue color level (true color)
        /// </summary>
        public int ExcaliBeatsMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [ExcaliBeats] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int ExcaliBeatsMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [BarWave] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool BarWaveTrueColor { get; set; } = true;
        /// <summary>
        /// [BarWave] How many milliseconds to wait before making the next write?
        /// </summary>
        public int BarWaveDelay { get; set; } = 1;
        /// <summary>
        /// [BarWave] The minimum red color level (true color)
        /// </summary>
        public int BarWaveMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [BarWave] The minimum green color level (true color)
        /// </summary>
        public int BarWaveMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [BarWave] The minimum blue color level (true color)
        /// </summary>
        public int BarWaveMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [BarWave] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int BarWaveMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [BarWave] The maximum red color level (true color)
        /// </summary>
        public int BarWaveMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [BarWave] The maximum green color level (true color)
        /// </summary>
        public int BarWaveMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [BarWave] The maximum blue color level (true color)
        /// </summary>
        public int BarWaveMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [BarWave] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int BarWaveMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [Wave] How many milliseconds to wait before making the next write?
        /// </summary>
        public int WaveDelay { get; set; } = 100;
        /// <summary>
        /// [Wave] The minimum red color level (true color)
        /// </summary>
        public int WaveMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [Wave] The minimum green color level (true color)
        /// </summary>
        public int WaveMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [Wave] The minimum blue color level (true color)
        /// </summary>
        public int WaveMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [Wave] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int WaveMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [Wave] The maximum red color level (true color)
        /// </summary>
        public int WaveMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [Wave] The maximum green color level (true color)
        /// </summary>
        public int WaveMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [Wave] The maximum blue color level (true color)
        /// </summary>
        public int WaveMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [Wave] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int WaveMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [Mesmerize] How many milliseconds to wait before making the next write?
        /// </summary>
        public int MesmerizeDelay { get; set; } = 10;
        /// <summary>
        /// [Mesmerize] The minimum red color level (true color)
        /// </summary>
        public int MesmerizeMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [Mesmerize] The minimum green color level (true color)
        /// </summary>
        public int MesmerizeMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [Mesmerize] The minimum blue color level (true color)
        /// </summary>
        public int MesmerizeMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [Mesmerize] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int MesmerizeMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [Mesmerize] The maximum red color level (true color)
        /// </summary>
        public int MesmerizeMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [Mesmerize] The maximum green color level (true color)
        /// </summary>
        public int MesmerizeMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [Mesmerize] The maximum blue color level (true color)
        /// </summary>
        public int MesmerizeMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [Mesmerize] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int MesmerizeMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [Aurora] How many milliseconds to wait before making the next write?
        /// </summary>
        public int AuroraDelay { get; set; } = 100;
        /// <summary>
        /// [Matrix] How many milliseconds to wait before making the next write?
        /// </summary>
        public int MatrixDelay { get; set; } = 10;
        /// <summary>
        /// [Matrix] How many fade steps to do?
        /// </summary>
        public int MatrixMaxSteps { get; set; } = 25;
        /// <summary>
        /// [Lightning] How many milliseconds to wait before making the next write?
        /// </summary>
        public int LightningDelay { get; set; } = 100;
        /// <summary>
        /// [Lightning] Chance, in percent, to strike
        /// </summary>
        public int LightningStrikeProbability { get; set; } = 5;
        /// <summary>
        /// [Bloom] How many milliseconds to wait before making the next write?
        /// </summary>
        public int BloomDelay { get; set; } = 50;
        /// <summary>
        /// [Bloom] Whether to use dark colors or not
        /// </summary>
        public bool BloomDarkColors { get; set; }
        /// <summary>
        /// [WordHasher] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool WordHasherTrueColor { get; set; } = true;
        /// <summary>
        /// [WordHasher] How many milliseconds to wait before making the next write?
        /// </summary>
        public int WordHasherDelay { get; set; } = 1000;
        /// <summary>
        /// [WordHasher] The minimum red color level (true color)
        /// </summary>
        public int WordHasherMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [WordHasher] The minimum green color level (true color)
        /// </summary>
        public int WordHasherMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [WordHasher] The minimum blue color level (true color)
        /// </summary>
        public int WordHasherMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [WordHasher] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int WordHasherMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [WordHasher] The maximum red color level (true color)
        /// </summary>
        public int WordHasherMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [WordHasher] The maximum green color level (true color)
        /// </summary>
        public int WordHasherMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [WordHasher] The maximum blue color level (true color)
        /// </summary>
        public int WordHasherMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [WordHasher] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int WordHasherMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [SquareCorner] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SquareCornerDelay { get; set; } = 10;
        /// <summary>
        /// [SquareCorner] How many milliseconds to wait before fading the square out?
        /// </summary>
        public int SquareCornerFadeOutDelay { get; set; } = 3000;
        /// <summary>
        /// [SquareCorner] How many fade steps to do?
        /// </summary>
        public int SquareCornerMaxSteps { get; set; } = 25;
        /// <summary>
        /// [SquareCorner] The minimum red color level (true color)
        /// </summary>
        public int SquareCornerMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [SquareCorner] The minimum green color level (true color)
        /// </summary>
        public int SquareCornerMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [SquareCorner] The minimum blue color level (true color)
        /// </summary>
        public int SquareCornerMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [SquareCorner] The maximum red color level (true color)
        /// </summary>
        public int SquareCornerMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [SquareCorner] The maximum green color level (true color)
        /// </summary>
        public int SquareCornerMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [SquareCorner] The maximum blue color level (true color)
        /// </summary>
        public int SquareCornerMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [NumberScatter] How many milliseconds to wait before making the next write?
        /// </summary>
        public int NumberScatterDelay { get; set; } = 1;
        /// <summary>
        /// [NumberScatter] Screensaver background color
        /// </summary>
        public string NumberScatterBackgroundColor { get; set; } = new Color(ConsoleColors.Black).PlainSequence;
        /// <summary>
        /// [NumberScatter] Screensaver foreground color
        /// </summary>
        public string NumberScatterForegroundColor { get; set; } = new Color(ConsoleColors.Green).PlainSequence;
        /// <summary>
        /// [Quote] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool QuoteTrueColor { get; set; } = true;
        /// <summary>
        /// [Quote] How many milliseconds to wait before making the next write?
        /// </summary>
        public int QuoteDelay { get; set; } = 10000;
        /// <summary>
        /// [Quote] The minimum red color level (true color)
        /// </summary>
        public int QuoteMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [Quote] The minimum green color level (true color)
        /// </summary>
        public int QuoteMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [Quote] The minimum blue color level (true color)
        /// </summary>
        public int QuoteMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [Quote] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int QuoteMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [Quote] The maximum red color level (true color)
        /// </summary>
        public int QuoteMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [Quote] The maximum green color level (true color)
        /// </summary>
        public int QuoteMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [Quote] The maximum blue color level (true color)
        /// </summary>
        public int QuoteMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [Quote] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int QuoteMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [BoxGrid] How many milliseconds to wait before making the next write?
        /// </summary>
        public int BoxGridDelay { get; set; } = 5000;
        /// <summary>
        /// [BoxGrid] The minimum red color level (true color)
        /// </summary>
        public int BoxGridMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [BoxGrid] The minimum green color level (true color)
        /// </summary>
        public int BoxGridMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [BoxGrid] The minimum blue color level (true color)
        /// </summary>
        public int BoxGridMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [BoxGrid] The maximum red color level (true color)
        /// </summary>
        public int BoxGridMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [BoxGrid] The maximum green color level (true color)
        /// </summary>
        public int BoxGridMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [BoxGrid] The maximum blue color level (true color)
        /// </summary>
        public int BoxGridMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [ColorBleed] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool ColorBleedTrueColor { get; set; } = true;
        /// <summary>
        /// [ColorBleed] How many milliseconds to wait before making the next write?
        /// </summary>
        public int ColorBleedDelay { get; set; } = 10;
        /// <summary>
        /// [ColorBleed] How many fade steps to do?
        /// </summary>
        public int ColorBleedMaxSteps { get; set; } = 25;
        /// <summary>
        /// [ColorBleed] Chance to drop a new falling color
        /// </summary>
        public int ColorBleedDropChance { get; set; } = 40;
        /// <summary>
        /// [ColorBleed] The minimum red color level (true color)
        /// </summary>
        public int ColorBleedMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [ColorBleed] The minimum green color level (true color)
        /// </summary>
        public int ColorBleedMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [ColorBleed] The minimum blue color level (true color)
        /// </summary>
        public int ColorBleedMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [ColorBleed] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int ColorBleedMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [ColorBleed] The maximum red color level (true color)
        /// </summary>
        public int ColorBleedMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [ColorBleed] The maximum green color level (true color)
        /// </summary>
        public int ColorBleedMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [ColorBleed] The maximum blue color level (true color)
        /// </summary>
        public int ColorBleedMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [ColorBleed] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int ColorBleedMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [MatrixBleed] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool MatrixBleedTrueColor { get; set; } = true;
        /// <summary>
        /// [MatrixBleed] How many milliseconds to wait before making the next write?
        /// </summary>
        public int MatrixBleedDelay { get; set; } = 10;
        /// <summary>
        /// [MatrixBleed] How many fade steps to do?
        /// </summary>
        public int MatrixBleedMaxSteps { get; set; } = 25;
        /// <summary>
        /// [MatrixBleed] Chance to drop a new falling matrix
        /// </summary>
        public int MatrixBleedDropChance { get; set; } = 40;
        /// <summary>
        /// [MatrixBleed] The minimum red color level (true color)
        /// </summary>
        public int MatrixBleedMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [MatrixBleed] The minimum green color level (true color)
        /// </summary>
        public int MatrixBleedMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [MatrixBleed] The minimum blue color level (true color)
        /// </summary>
        public int MatrixBleedMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [MatrixBleed] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int MatrixBleedMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [MatrixBleed] The maximum red color level (true color)
        /// </summary>
        public int MatrixBleedMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [MatrixBleed] The maximum green color level (true color)
        /// </summary>
        public int MatrixBleedMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [MatrixBleed] The maximum blue color level (true color)
        /// </summary>
        public int MatrixBleedMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [MatrixBleed] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int MatrixBleedMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [Text] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool TextTrueColor { get; set; } = true;
        /// <summary>
        /// [Text] How many milliseconds to wait before making the next write?
        /// </summary>
        public int TextDelay { get; set; } = 1000;
        /// <summary>
        /// [Text] Text for Bouncing Text. Shorter is better.
        /// </summary>
        public string TextWrite { get; set; } = "Nitrocid KS";
        /// <summary>
        /// [Text] The minimum red color level (true color)
        /// </summary>
        public int TextMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [Text] The minimum green color level (true color)
        /// </summary>
        public int TextMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [Text] The minimum blue color level (true color)
        /// </summary>
        public int TextMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [Text] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int TextMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [Text] The maximum red color level (true color)
        /// </summary>
        public int TextMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [Text] The maximum green color level (true color)
        /// </summary>
        public int TextMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [Text] The maximum blue color level (true color)
        /// </summary>
        public int TextMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [Text] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int TextMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [TextBox] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool TextBoxTrueColor { get; set; } = true;
        /// <summary>
        /// [TextBox] How many milliseconds to wait before making the next write?
        /// </summary>
        public int TextBoxDelay { get; set; } = 1000;
        /// <summary>
        /// [TextBox] TextBox for Bouncing TextBox. Shorter is better.
        /// </summary>
        public string TextBoxWrite { get; set; } = "Nitrocid KS";
        /// <summary>
        /// [TextBox] The minimum red color level (true color)
        /// </summary>
        public int TextBoxMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [TextBox] The minimum green color level (true color)
        /// </summary>
        public int TextBoxMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [TextBox] The minimum blue color level (true color)
        /// </summary>
        public int TextBoxMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [TextBox] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int TextBoxMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [TextBox] The maximum red color level (true color)
        /// </summary>
        public int TextBoxMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [TextBox] The maximum green color level (true color)
        /// </summary>
        public int TextBoxMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [TextBox] The maximum blue color level (true color)
        /// </summary>
        public int TextBoxMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [TextBox] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int TextBoxMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [WordHasherWrite] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool WordHasherWriteTrueColor { get; set; } = true;
        /// <summary>
        /// [WordHasherWrite] How many milliseconds to wait before making the next write?
        /// </summary>
        public int WordHasherWriteDelay { get; set; } = 1000;
        /// <summary>
        /// [WordHasherWrite] The minimum red color level (true color)
        /// </summary>
        public int WordHasherWriteMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [WordHasherWrite] The minimum green color level (true color)
        /// </summary>
        public int WordHasherWriteMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [WordHasherWrite] The minimum blue color level (true color)
        /// </summary>
        public int WordHasherWriteMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [WordHasherWrite] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int WordHasherWriteMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [WordHasherWrite] The maximum red color level (true color)
        /// </summary>
        public int WordHasherWriteMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [WordHasherWrite] The maximum green color level (true color)
        /// </summary>
        public int WordHasherWriteMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [WordHasherWrite] The maximum blue color level (true color)
        /// </summary>
        public int WordHasherWriteMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [WordHasherWrite] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int WordHasherWriteMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [SirenTheme] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SirenThemeDelay { get; set; } = 500;
        /// <summary>
        /// [SirenTheme] The siren style
        /// </summary>
        public string SirenThemeStyle { get; set; } = "Default";
        /// <summary>
        /// [StarfieldWarp] How many milliseconds to wait before making the next write?
        /// </summary>
        public int StarfieldWarpDelay { get; set; } = 10;
        /// <summary>
        /// [Speckles] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SpecklesDelay { get; set; } = 10;
        /// <summary>
        /// [LetterScatter] How many milliseconds to wait before making the next write?
        /// </summary>
        public int LetterScatterDelay { get; set; } = 1;
        /// <summary>
        /// [LetterScatter] Screensaver background color
        /// </summary>
        public string LetterScatterBackgroundColor { get; set; } = new Color(ConsoleColors.Black).PlainSequence;
        /// <summary>
        /// [LetterScatter] Screensaver foreground color
        /// </summary>
        public string LetterScatterForegroundColor { get; set; } = new Color(ConsoleColors.Green).PlainSequence;
        /// <summary>
        /// [MultiLines] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool MultiLinesTrueColor { get; set; } = true;
        /// <summary>
        /// [MultiLines] How many milliseconds to wait before making the next write?
        /// </summary>
        public int MultiLinesDelay { get; set; } = 500;
        /// <summary>
        /// [MultiLines] Line character
        /// </summary>
        public string MultiLinesLineChar { get; set; } = "-";
        /// <summary>
        /// [MultiLines] Screensaver background color
        /// </summary>
        public string MultiLinesBackgroundColor { get; set; } = new Color(ConsoleColors.Black).PlainSequence;
        /// <summary>
        /// [MultiLines] The minimum red color level (true color)
        /// </summary>
        public int MultiLinesMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [MultiLines] The minimum green color level (true color)
        /// </summary>
        public int MultiLinesMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [MultiLines] The minimum blue color level (true color)
        /// </summary>
        public int MultiLinesMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [MultiLines] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int MultiLinesMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [MultiLines] The maximum red color level (true color)
        /// </summary>
        public int MultiLinesMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [MultiLines] The maximum green color level (true color)
        /// </summary>
        public int MultiLinesMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [MultiLines] The maximum blue color level (true color)
        /// </summary>
        public int MultiLinesMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [MultiLines] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int MultiLinesMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [LaserBeams] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool LaserBeamsTrueColor { get; set; } = true;
        /// <summary>
        /// [LaserBeams] How many milliseconds to wait before making the next write?
        /// </summary>
        public int LaserBeamsDelay { get; set; } = 500;
        /// <summary>
        /// [LaserBeams] Line character
        /// </summary>
        public string LaserBeamsLineChar { get; set; } = "-";
        /// <summary>
        /// [LaserBeams] Screensaver background color
        /// </summary>
        public string LaserBeamsBackgroundColor { get; set; } = new Color(ConsoleColors.Black).PlainSequence;
        /// <summary>
        /// [LaserBeams] The minimum red color level (true color)
        /// </summary>
        public int LaserBeamsMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [LaserBeams] The minimum green color level (true color)
        /// </summary>
        public int LaserBeamsMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [LaserBeams] The minimum blue color level (true color)
        /// </summary>
        public int LaserBeamsMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [LaserBeams] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int LaserBeamsMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [LaserBeams] The maximum red color level (true color)
        /// </summary>
        public int LaserBeamsMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [LaserBeams] The maximum green color level (true color)
        /// </summary>
        public int LaserBeamsMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [LaserBeams] The maximum blue color level (true color)
        /// </summary>
        public int LaserBeamsMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [LaserBeams] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int LaserBeamsMaximumColorLevel { get; set; } = 255;
    }
}
