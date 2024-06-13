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

using Newtonsoft.Json;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Configuration.Instances;
using Nitrocid.Kernel.Configuration.Settings;
using Nitrocid.Misc.Reflection.Internal;
using Nitrocid.ScreensaverPacks.Screensavers;
using System;
using Terminaux.Colors;
using Terminaux.Colors.Data;

namespace Nitrocid.ScreensaverPacks.Settings
{
    /// <summary>
    /// Screensaver kernel configuration instance
    /// </summary>
    public class ExtraSaversConfig : BaseKernelConfig, IKernelConfig
    {
        /// <inheritdoc/>
        [JsonIgnore]
        public override SettingsEntry[] SettingsEntries =>
            ConfigTools.GetSettingsEntries(ResourcesManager.GetData("AddonSaverSettings.json", ResourcesType.Misc, typeof(ExtraSaversConfig).Assembly));

        #region Matrix
        private int matrixDelay = 10;
        private int matrixMaxSteps = 25;

        /// <summary>
        /// [Matrix] How many milliseconds to wait before making the next write?
        /// </summary>
        public int MatrixDelay
        {
            get
            {
                return matrixDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                matrixDelay = value;
            }
        }

        /// <summary>
        /// [Matrix] How many fade steps to do?
        /// </summary>
        public int MatrixMaxSteps
        {
            get
            {
                return matrixMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                matrixMaxSteps = value;
            }
        }
        #endregion

        #region ColorMix
        private bool colorMixTrueColor = true;
        private int colorMixDelay = 1;
        private string colorMixBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private int colorMixMinimumRedColorLevel = 0;
        private int colorMixMinimumGreenColorLevel = 0;
        private int colorMixMinimumBlueColorLevel = 0;
        private int colorMixMinimumColorLevel = 0;
        private int colorMixMaximumRedColorLevel = 255;
        private int colorMixMaximumGreenColorLevel = 255;
        private int colorMixMaximumBlueColorLevel = 255;
        private int colorMixMaximumColorLevel = 255;

        /// <summary>
        /// [ColorMix] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool ColorMixTrueColor
        {
            get
            {
                return colorMixTrueColor;
            }
            set
            {
                colorMixTrueColor = value;
            }
        }

        /// <summary>
        /// [ColorMix] How many milliseconds to wait before making the next write?
        /// </summary>
        public int ColorMixDelay
        {
            get
            {
                return colorMixDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                colorMixDelay = value;
            }
        }

        /// <summary>
        /// [ColorMix] Screensaver background color
        /// </summary>
        public string ColorMixBackgroundColor
        {
            get
            {
                return colorMixBackgroundColor;
            }
            set
            {
                colorMixBackgroundColor = new Color(value).PlainSequence;
            }
        }

        /// <summary>
        /// [ColorMix] The minimum red color level (true color)
        /// </summary>
        public int ColorMixMinimumRedColorLevel
        {
            get
            {
                return colorMixMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                colorMixMinimumRedColorLevel = value;
            }
        }

        /// <summary>
        /// [ColorMix] The minimum green color level (true color)
        /// </summary>
        public int ColorMixMinimumGreenColorLevel
        {
            get
            {
                return colorMixMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                colorMixMinimumGreenColorLevel = value;
            }
        }

        /// <summary>
        /// [ColorMix] The minimum blue color level (true color)
        /// </summary>
        public int ColorMixMinimumBlueColorLevel
        {
            get
            {
                return colorMixMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                colorMixMinimumBlueColorLevel = value;
            }
        }

        /// <summary>
        /// [ColorMix] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int ColorMixMinimumColorLevel
        {
            get
            {
                return colorMixMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                colorMixMinimumColorLevel = value;
            }
        }

        /// <summary>
        /// [ColorMix] The maximum red color level (true color)
        /// </summary>
        public int ColorMixMaximumRedColorLevel
        {
            get
            {
                return colorMixMaximumRedColorLevel;
            }
            set
            {
                if (value <= colorMixMaximumRedColorLevel)
                    value = colorMixMaximumRedColorLevel;
                if (value > 255)
                    value = 255;
                colorMixMaximumRedColorLevel = value;
            }
        }

        /// <summary>
        /// [ColorMix] The maximum green color level (true color)
        /// </summary>
        public int ColorMixMaximumGreenColorLevel
        {
            get
            {
                return colorMixMaximumGreenColorLevel;
            }
            set
            {
                if (value <= colorMixMaximumGreenColorLevel)
                    value = colorMixMaximumGreenColorLevel;
                if (value > 255)
                    value = 255;
                colorMixMaximumGreenColorLevel = value;
            }
        }

        /// <summary>
        /// [ColorMix] The maximum blue color level (true color)
        /// </summary>
        public int ColorMixMaximumBlueColorLevel
        {
            get
            {
                return colorMixMaximumBlueColorLevel;
            }
            set
            {
                if (value <= colorMixMaximumBlueColorLevel)
                    value = colorMixMaximumBlueColorLevel;
                if (value > 255)
                    value = 255;
                colorMixMaximumBlueColorLevel = value;
            }
        }

        /// <summary>
        /// [ColorMix] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int ColorMixMaximumColorLevel
        {
            get
            {
                return colorMixMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= colorMixMaximumColorLevel)
                    value = colorMixMaximumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                colorMixMaximumColorLevel = value;
            }
        }
        #endregion

        #region Disco
        private bool discoTrueColor = true;
        private int discoDelay = 100;
        private bool discoUseBeatsPerMinute;
        private bool discoCycleColors;
        private bool discoEnableFedMode;
        private int discoMinimumRedColorLevel = 0;
        private int discoMinimumGreenColorLevel = 0;
        private int discoMinimumBlueColorLevel = 0;
        private int discoMinimumColorLevel = 0;
        private int discoMaximumRedColorLevel = 255;
        private int discoMaximumGreenColorLevel = 255;
        private int discoMaximumBlueColorLevel = 255;
        private int discoMaximumColorLevel = 255;

        /// <summary>
        /// [Disco] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool DiscoTrueColor
        {
            get
            {
                return discoTrueColor;
            }
            set
            {
                discoTrueColor = value;
            }
        }

        /// <summary>
        /// [Disco] How many milliseconds, or beats per minute, to wait before making the next write?
        /// </summary>
        public int DiscoDelay
        {
            get
            {
                return discoDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                discoDelay = value;
            }
        }

        /// <summary>
        /// [Disco] Whether to use the Beats Per Minute (1/4) to change the writing delay. If False, will use the standard milliseconds delay instead.
        /// </summary>
        public bool DiscoUseBeatsPerMinute
        {
            get
            {
                return discoUseBeatsPerMinute;
            }
            set
            {
                discoUseBeatsPerMinute = value;
            }
        }

        /// <summary>
        /// [Disco] Enable color cycling
        /// </summary>
        public bool DiscoCycleColors
        {
            get
            {
                return discoCycleColors;
            }
            set
            {
                discoCycleColors = value;
            }
        }

        /// <summary>
        /// [Disco] Uses the black and white cycle to produce the same effect as the legacy "fed" screensaver introduced back in v0.0.1
        /// </summary>
        public bool DiscoEnableFedMode
        {
            get
            {
                return discoEnableFedMode;
            }
            set
            {
                discoEnableFedMode = value;
            }
        }

        /// <summary>
        /// [Disco] The minimum red color level (true color)
        /// </summary>
        public int DiscoMinimumRedColorLevel
        {
            get
            {
                return discoMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                discoMinimumRedColorLevel = value;
            }
        }

        /// <summary>
        /// [Disco] The minimum green color level (true color)
        /// </summary>
        public int DiscoMinimumGreenColorLevel
        {
            get
            {
                return discoMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                discoMinimumGreenColorLevel = value;
            }
        }

        /// <summary>
        /// [Disco] The minimum blue color level (true color)
        /// </summary>
        public int DiscoMinimumBlueColorLevel
        {
            get
            {
                return discoMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                discoMinimumBlueColorLevel = value;
            }
        }

        /// <summary>
        /// [Disco] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int DiscoMinimumColorLevel
        {
            get
            {
                return discoMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                discoMinimumColorLevel = value;
            }
        }

        /// <summary>
        /// [Disco] The maximum red color level (true color)
        /// </summary>
        public int DiscoMaximumRedColorLevel
        {
            get
            {
                return discoMaximumRedColorLevel;
            }
            set
            {
                if (value <= discoMaximumRedColorLevel)
                    value = discoMaximumRedColorLevel;
                if (value > 255)
                    value = 255;
                discoMaximumRedColorLevel = value;
            }
        }

        /// <summary>
        /// [Disco] The maximum green color level (true color)
        /// </summary>
        public int DiscoMaximumGreenColorLevel
        {
            get
            {
                return discoMaximumGreenColorLevel;
            }
            set
            {
                if (value <= discoMaximumGreenColorLevel)
                    value = discoMaximumGreenColorLevel;
                if (value > 255)
                    value = 255;
                discoMaximumGreenColorLevel = value;
            }
        }

        /// <summary>
        /// [Disco] The maximum blue color level (true color)
        /// </summary>
        public int DiscoMaximumBlueColorLevel
        {
            get
            {
                return discoMaximumBlueColorLevel;
            }
            set
            {
                if (value <= discoMaximumBlueColorLevel)
                    value = discoMaximumBlueColorLevel;
                if (value > 255)
                    value = 255;
                discoMaximumBlueColorLevel = value;
            }
        }

        /// <summary>
        /// [Disco] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int DiscoMaximumColorLevel
        {
            get
            {
                return discoMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= discoMaximumColorLevel)
                    value = discoMaximumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                discoMaximumColorLevel = value;
            }
        }
        #endregion

        #region GlitterColor
        private bool glitterColorTrueColor = true;
        private int glitterColorDelay = 1;
        private int glitterColorMinimumRedColorLevel = 0;
        private int glitterColorMinimumGreenColorLevel = 0;
        private int glitterColorMinimumBlueColorLevel = 0;
        private int glitterColorMinimumColorLevel = 0;
        private int glitterColorMaximumRedColorLevel = 255;
        private int glitterColorMaximumGreenColorLevel = 255;
        private int glitterColorMaximumBlueColorLevel = 255;
        private int glitterColorMaximumColorLevel = 255;

        /// <summary>
        /// [GlitterColor] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool GlitterColorTrueColor
        {
            get
            {
                return glitterColorTrueColor;
            }
            set
            {
                glitterColorTrueColor = value;
            }
        }

        /// <summary>
        /// [GlitterColor] How many milliseconds to wait before making the next write?
        /// </summary>
        public int GlitterColorDelay
        {
            get
            {
                return glitterColorDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                glitterColorDelay = value;
            }
        }

        /// <summary>
        /// [GlitterColor] The minimum red color level (true color)
        /// </summary>
        public int GlitterColorMinimumRedColorLevel
        {
            get
            {
                return glitterColorMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                glitterColorMinimumRedColorLevel = value;
            }
        }

        /// <summary>
        /// [GlitterColor] The minimum green color level (true color)
        /// </summary>
        public int GlitterColorMinimumGreenColorLevel
        {
            get
            {
                return glitterColorMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                glitterColorMinimumGreenColorLevel = value;
            }
        }

        /// <summary>
        /// [GlitterColor] The minimum blue color level (true color)
        /// </summary>
        public int GlitterColorMinimumBlueColorLevel
        {
            get
            {
                return glitterColorMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                glitterColorMinimumBlueColorLevel = value;
            }
        }

        /// <summary>
        /// [GlitterColor] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int GlitterColorMinimumColorLevel
        {
            get
            {
                return glitterColorMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                glitterColorMinimumColorLevel = value;
            }
        }

        /// <summary>
        /// [GlitterColor] The maximum red color level (true color)
        /// </summary>
        public int GlitterColorMaximumRedColorLevel
        {
            get
            {
                return glitterColorMaximumRedColorLevel;
            }
            set
            {
                if (value <= glitterColorMinimumRedColorLevel)
                    value = glitterColorMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                glitterColorMaximumRedColorLevel = value;
            }
        }

        /// <summary>
        /// [GlitterColor] The maximum green color level (true color)
        /// </summary>
        public int GlitterColorMaximumGreenColorLevel
        {
            get
            {
                return glitterColorMaximumGreenColorLevel;
            }
            set
            {
                if (value <= glitterColorMinimumGreenColorLevel)
                    value = glitterColorMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                glitterColorMaximumGreenColorLevel = value;
            }
        }

        /// <summary>
        /// [GlitterColor] The maximum blue color level (true color)
        /// </summary>
        public int GlitterColorMaximumBlueColorLevel
        {
            get
            {
                return glitterColorMaximumBlueColorLevel;
            }
            set
            {
                if (value <= glitterColorMinimumBlueColorLevel)
                    value = glitterColorMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                glitterColorMaximumBlueColorLevel = value;
            }
        }

        /// <summary>
        /// [GlitterColor] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int GlitterColorMaximumColorLevel
        {
            get
            {
                return glitterColorMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= glitterColorMinimumColorLevel)
                    value = glitterColorMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                glitterColorMaximumColorLevel = value;
            }
        }
        #endregion

        #region Lines
        private bool linesTrueColor = true;
        private int linesDelay = 500;
        private string linesLineChar = "-";
        private string linesBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private int linesMinimumRedColorLevel = 0;
        private int linesMinimumGreenColorLevel = 0;
        private int linesMinimumBlueColorLevel = 0;
        private int linesMinimumColorLevel = 0;
        private int linesMaximumRedColorLevel = 255;
        private int linesMaximumGreenColorLevel = 255;
        private int linesMaximumBlueColorLevel = 255;
        private int linesMaximumColorLevel = 255;

        /// <summary>
        /// [Lines] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool LinesTrueColor
        {
            get
            {
                return linesTrueColor;
            }
            set
            {
                linesTrueColor = value;
            }
        }

        /// <summary>
        /// [Lines] How many milliseconds to wait before making the next write?
        /// </summary>
        public int LinesDelay
        {
            get
            {
                return linesDelay;
            }
            set
            {
                if (value <= 0)
                    value = 500;
                linesDelay = value;
            }
        }

        /// <summary>
        /// [Lines] Line character
        /// </summary>
        public string LinesLineChar
        {
            get
            {
                return linesLineChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "-";
                linesLineChar = value;
            }
        }

        /// <summary>
        /// [Lines] Screensaver background color
        /// </summary>
        public string LinesBackgroundColor
        {
            get
            {
                return linesBackgroundColor;
            }
            set
            {
                linesBackgroundColor = new Color(value).PlainSequence;
            }
        }

        /// <summary>
        /// [Lines] The minimum red color level (true color)
        /// </summary>
        public int LinesMinimumRedColorLevel
        {
            get
            {
                return linesMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                linesMinimumRedColorLevel = value;
            }
        }

        /// <summary>
        /// [Lines] The minimum green color level (true color)
        /// </summary>
        public int LinesMinimumGreenColorLevel
        {
            get
            {
                return linesMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                linesMinimumGreenColorLevel = value;
            }
        }

        /// <summary>
        /// [Lines] The minimum blue color level (true color)
        /// </summary>
        public int LinesMinimumBlueColorLevel
        {
            get
            {
                return linesMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                linesMinimumBlueColorLevel = value;
            }
        }

        /// <summary>
        /// [Lines] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int LinesMinimumColorLevel
        {
            get
            {
                return linesMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                linesMinimumColorLevel = value;
            }
        }

        /// <summary>
        /// [Lines] The maximum red color level (true color)
        /// </summary>
        public int LinesMaximumRedColorLevel
        {
            get
            {
                return linesMaximumRedColorLevel;
            }
            set
            {
                if (value <= linesMinimumRedColorLevel)
                    value = linesMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                linesMaximumRedColorLevel = value;
            }
        }

        /// <summary>
        /// [Lines] The maximum green color level (true color)
        /// </summary>
        public int LinesMaximumGreenColorLevel
        {
            get
            {
                return linesMaximumGreenColorLevel;
            }
            set
            {
                if (value <= linesMinimumGreenColorLevel)
                    value = linesMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                linesMaximumGreenColorLevel = value;
            }
        }

        /// <summary>
        /// [Lines] The maximum blue color level (true color)
        /// </summary>
        public int LinesMaximumBlueColorLevel
        {
            get
            {
                return linesMaximumBlueColorLevel;
            }
            set
            {
                if (value <= linesMinimumBlueColorLevel)
                    value = linesMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                linesMaximumBlueColorLevel = value;
            }
        }

        /// <summary>
        /// [Lines] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int LinesMaximumColorLevel
        {
            get
            {
                return linesMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= linesMinimumColorLevel)
                    value = linesMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                linesMaximumColorLevel = value;
            }
        }
        #endregion

        #region Dissolve
        private bool dissolveTrueColor = true;
        private string dissolveBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private int dissolveMinimumRedColorLevel = 0;
        private int dissolveMinimumGreenColorLevel = 0;
        private int dissolveMinimumBlueColorLevel = 0;
        private int dissolveMinimumColorLevel = 0;
        private int dissolveMaximumRedColorLevel = 255;
        private int dissolveMaximumGreenColorLevel = 255;
        private int dissolveMaximumBlueColorLevel = 255;
        private int dissolveMaximumColorLevel = 255;

        /// <summary>
        /// [Dissolve] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool DissolveTrueColor
        {
            get
            {
                return dissolveTrueColor;
            }
            set
            {
                dissolveTrueColor = value;
            }
        }
        /// <summary>
        /// [Dissolve] Screensaver background color
        /// </summary>
        public string DissolveBackgroundColor
        {
            get
            {
                return dissolveBackgroundColor;
            }
            set
            {
                dissolveBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Dissolve] The minimum red color level (true color)
        /// </summary>
        public int DissolveMinimumRedColorLevel
        {
            get
            {
                return dissolveMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                dissolveMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Dissolve] The minimum green color level (true color)
        /// </summary>
        public int DissolveMinimumGreenColorLevel
        {
            get
            {
                return dissolveMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                dissolveMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Dissolve] The minimum blue color level (true color)
        /// </summary>
        public int DissolveMinimumBlueColorLevel
        {
            get
            {
                return dissolveMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                dissolveMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Dissolve] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int DissolveMinimumColorLevel
        {
            get
            {
                return dissolveMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                dissolveMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Dissolve] The maximum red color level (true color)
        /// </summary>
        public int DissolveMaximumRedColorLevel
        {
            get
            {
                return dissolveMaximumRedColorLevel;
            }
            set
            {
                if (value <= dissolveMinimumRedColorLevel)
                    value = dissolveMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                dissolveMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Dissolve] The maximum green color level (true color)
        /// </summary>
        public int DissolveMaximumGreenColorLevel
        {
            get
            {
                return dissolveMaximumGreenColorLevel;
            }
            set
            {
                if (value <= dissolveMinimumGreenColorLevel)
                    value = dissolveMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                dissolveMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Dissolve] The maximum blue color level (true color)
        /// </summary>
        public int DissolveMaximumBlueColorLevel
        {
            get
            {
                return dissolveMaximumBlueColorLevel;
            }
            set
            {
                if (value <= dissolveMinimumBlueColorLevel)
                    value = dissolveMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                dissolveMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Dissolve] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int DissolveMaximumColorLevel
        {
            get
            {
                return dissolveMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= dissolveMinimumColorLevel)
                    value = dissolveMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                dissolveMaximumColorLevel = value;
            }
        }
        #endregion

        #region BouncingBlock
        private bool bouncingBlockTrueColor = true;
        private int bouncingBlockDelay = 10;
        private string bouncingBlockBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private string bouncingBlockForegroundColor = new Color(ConsoleColors.White).PlainSequence;
        private int bouncingBlockMinimumRedColorLevel = 0;
        private int bouncingBlockMinimumGreenColorLevel = 0;
        private int bouncingBlockMinimumBlueColorLevel = 0;
        private int bouncingBlockMinimumColorLevel = 0;
        private int bouncingBlockMaximumRedColorLevel = 255;
        private int bouncingBlockMaximumGreenColorLevel = 255;
        private int bouncingBlockMaximumBlueColorLevel = 255;
        private int bouncingBlockMaximumColorLevel = 255;

        /// <summary>
        /// [BouncingBlock] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool BouncingBlockTrueColor
        {
            get
            {
                return bouncingBlockTrueColor;
            }
            set
            {
                bouncingBlockTrueColor = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] How many milliseconds to wait before making the next write?
        /// </summary>
        public int BouncingBlockDelay
        {
            get
            {
                return bouncingBlockDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                bouncingBlockDelay = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] Screensaver background color
        /// </summary>
        public string BouncingBlockBackgroundColor
        {
            get
            {
                return bouncingBlockBackgroundColor;
            }
            set
            {
                bouncingBlockBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BouncingBlock] Screensaver foreground color
        /// </summary>
        public string BouncingBlockForegroundColor
        {
            get
            {
                return bouncingBlockForegroundColor;
            }
            set
            {
                bouncingBlockForegroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BouncingBlock] The minimum red color level (true color)
        /// </summary>
        public int BouncingBlockMinimumRedColorLevel
        {
            get
            {
                return bouncingBlockMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                bouncingBlockMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] The minimum green color level (true color)
        /// </summary>
        public int BouncingBlockMinimumGreenColorLevel
        {
            get
            {
                return bouncingBlockMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                bouncingBlockMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] The minimum blue color level (true color)
        /// </summary>
        public int BouncingBlockMinimumBlueColorLevel
        {
            get
            {
                return bouncingBlockMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                bouncingBlockMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int BouncingBlockMinimumColorLevel
        {
            get
            {
                return bouncingBlockMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                bouncingBlockMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] The maximum red color level (true color)
        /// </summary>
        public int BouncingBlockMaximumRedColorLevel
        {
            get
            {
                return bouncingBlockMaximumRedColorLevel;
            }
            set
            {
                if (value <= bouncingBlockMinimumRedColorLevel)
                    value = bouncingBlockMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                bouncingBlockMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] The maximum green color level (true color)
        /// </summary>
        public int BouncingBlockMaximumGreenColorLevel
        {
            get
            {
                return bouncingBlockMaximumGreenColorLevel;
            }
            set
            {
                if (value <= bouncingBlockMinimumGreenColorLevel)
                    value = bouncingBlockMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                bouncingBlockMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] The maximum blue color level (true color)
        /// </summary>
        public int BouncingBlockMaximumBlueColorLevel
        {
            get
            {
                return bouncingBlockMaximumBlueColorLevel;
            }
            set
            {
                if (value <= bouncingBlockMinimumBlueColorLevel)
                    value = bouncingBlockMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                bouncingBlockMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int BouncingBlockMaximumColorLevel
        {
            get
            {
                return bouncingBlockMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= bouncingBlockMinimumColorLevel)
                    value = bouncingBlockMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                bouncingBlockMaximumColorLevel = value;
            }
        }
        #endregion

        #region ProgressClock
        private bool progressClockTrueColor = true;
        private bool progressClockCycleColors = true;
        private int progressClockCycleColorsTicks = 20;
        private string progressClockSecondsProgressColor = "4";
        private string progressClockMinutesProgressColor = "5";
        private string progressClockHoursProgressColor = "6";
        private string progressClockProgressColor = "7";
        private int progressClockDelay = 500;
        private char progressClockUpperLeftCornerCharHours = '╔';
        private char progressClockUpperLeftCornerCharMinutes = '╔';
        private char progressClockUpperLeftCornerCharSeconds = '╔';
        private char progressClockUpperRightCornerCharHours = '╗';
        private char progressClockUpperRightCornerCharMinutes = '╗';
        private char progressClockUpperRightCornerCharSeconds = '╗';
        private char progressClockLowerLeftCornerCharHours = '╚';
        private char progressClockLowerLeftCornerCharMinutes = '╚';
        private char progressClockLowerLeftCornerCharSeconds = '╚';
        private char progressClockLowerRightCornerCharHours = '╝';
        private char progressClockLowerRightCornerCharMinutes = '╝';
        private char progressClockLowerRightCornerCharSeconds = '╝';
        private char progressClockUpperFrameCharHours = '═';
        private char progressClockUpperFrameCharMinutes = '═';
        private char progressClockUpperFrameCharSeconds = '═';
        private char progressClockLowerFrameCharHours = '═';
        private char progressClockLowerFrameCharMinutes = '═';
        private char progressClockLowerFrameCharSeconds = '═';
        private char progressClockLeftFrameCharHours = '║';
        private char progressClockLeftFrameCharMinutes = '║';
        private char progressClockLeftFrameCharSeconds = '║';
        private char progressClockRightFrameCharHours = '║';
        private char progressClockRightFrameCharMinutes = '║';
        private char progressClockRightFrameCharSeconds = '║';
        private string progressClockInfoTextHours = "";
        private string progressClockInfoTextMinutes = "";
        private string progressClockInfoTextSeconds = "";
        private int progressClockMinimumRedColorLevelHours = 0;
        private int progressClockMinimumGreenColorLevelHours = 0;
        private int progressClockMinimumBlueColorLevelHours = 0;
        private int progressClockMinimumColorLevelHours = 1;
        private int progressClockMaximumRedColorLevelHours = 255;
        private int progressClockMaximumGreenColorLevelHours = 255;
        private int progressClockMaximumBlueColorLevelHours = 255;
        private int progressClockMaximumColorLevelHours = 255;
        private int progressClockMinimumRedColorLevelMinutes = 0;
        private int progressClockMinimumGreenColorLevelMinutes = 0;
        private int progressClockMinimumBlueColorLevelMinutes = 0;
        private int progressClockMinimumColorLevelMinutes = 1;
        private int progressClockMaximumRedColorLevelMinutes = 255;
        private int progressClockMaximumGreenColorLevelMinutes = 255;
        private int progressClockMaximumBlueColorLevelMinutes = 255;
        private int progressClockMaximumColorLevelMinutes = 255;
        private int progressClockMinimumRedColorLevelSeconds = 0;
        private int progressClockMinimumGreenColorLevelSeconds = 0;
        private int progressClockMinimumBlueColorLevelSeconds = 0;
        private int progressClockMinimumColorLevelSeconds = 1;
        private int progressClockMaximumRedColorLevelSeconds = 255;
        private int progressClockMaximumGreenColorLevelSeconds = 255;
        private int progressClockMaximumBlueColorLevelSeconds = 255;
        private int progressClockMaximumColorLevelSeconds = 255;
        private int progressClockMinimumRedColorLevel = 0;
        private int progressClockMinimumGreenColorLevel = 0;
        private int progressClockMinimumBlueColorLevel = 0;
        private int progressClockMinimumColorLevel = 1;
        private int progressClockMaximumRedColorLevel = 255;
        private int progressClockMaximumGreenColorLevel = 255;
        private int progressClockMaximumBlueColorLevel = 255;
        private int progressClockMaximumColorLevel = 255;

        /// <summary>
        /// [ProgressClock] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool ProgressClockTrueColor
        {
            get => progressClockTrueColor;
            set => progressClockTrueColor = value;
        }
        /// <summary>
        /// [ProgressClock] Enable color cycling (uses RNG. If disabled, uses the <see cref="ProgressClockSecondsProgressColor"/>, <see cref="ProgressClockMinutesProgressColor"/>, and <see cref="ProgressClockHoursProgressColor"/> colors.)
        /// </summary>
        public bool ProgressClockCycleColors
        {
            get => progressClockCycleColors;
            set => progressClockCycleColors = value;
        }
        /// <summary>
        /// [ProgressClock] The color of seconds progress bar. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public string ProgressClockSecondsProgressColor
        {
            get => progressClockSecondsProgressColor;
            set => progressClockSecondsProgressColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [ProgressClock] The color of minutes progress bar. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public string ProgressClockMinutesProgressColor
        {
            get => progressClockMinutesProgressColor;
            set => progressClockMinutesProgressColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [ProgressClock] The color of hours progress bar. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public string ProgressClockHoursProgressColor
        {
            get => progressClockHoursProgressColor;
            set => progressClockHoursProgressColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [ProgressClock] The color of date information. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public string ProgressClockProgressColor
        {
            get => progressClockProgressColor;
            set => progressClockProgressColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [ProgressClock] If color cycling is enabled, how many ticks before changing colors? 1 tick = 0.5 seconds
        /// </summary>
        public long ProgressClockCycleColorsTicks
        {
            get => progressClockCycleColorsTicks;
            set
            {
                if (value <= 0L)
                    value = 20L;
                progressClockCycleColorsTicks = (int)value;
            }
        }
        /// <summary>
        /// [ProgressClock] How many milliseconds to wait before making the next write?
        /// </summary>
        public int ProgressClockDelay
        {
            get => progressClockDelay;
            set
            {
                if (value <= 0)
                    value = 500;
                progressClockDelay = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Upper left corner character for hours bar
        /// </summary>
        public char ProgressClockUpperLeftCornerCharHours
        {
            get => progressClockUpperLeftCornerCharHours;
            set => progressClockUpperLeftCornerCharHours = value;
        }
        /// <summary>
        /// [ProgressClock] Upper left corner character for minutes bar
        /// </summary>
        public char ProgressClockUpperLeftCornerCharMinutes
        {
            get => progressClockUpperLeftCornerCharMinutes;
            set => progressClockUpperLeftCornerCharMinutes = value;
        }
        /// <summary>
        /// [ProgressClock] Upper left corner character for seconds bar
        /// </summary>
        public char ProgressClockUpperLeftCornerCharSeconds
        {
            get => progressClockUpperLeftCornerCharSeconds;
            set => progressClockUpperLeftCornerCharSeconds = value;
        }
        /// <summary>
        /// [ProgressClock] Upper right corner character for hours bar
        /// </summary>
        public char ProgressClockUpperRightCornerCharHours
        {
            get => progressClockUpperRightCornerCharHours;
            set => progressClockUpperRightCornerCharHours = value;
        }
        /// <summary>
        /// [ProgressClock] Upper right corner character for minutes bar
        /// </summary>
        public char ProgressClockUpperRightCornerCharMinutes
        {
            get => progressClockUpperRightCornerCharMinutes;
            set => progressClockUpperRightCornerCharMinutes = value;
        }
        /// <summary>
        /// [ProgressClock] Upper right corner character for seconds bar
        /// </summary>
        public char ProgressClockUpperRightCornerCharSeconds
        {
            get => progressClockUpperRightCornerCharSeconds;
            set => progressClockUpperRightCornerCharSeconds = value;
        }
        /// <summary>
        /// [ProgressClock] Lower left corner character for hours bar
        /// </summary>
        public char ProgressClockLowerLeftCornerCharHours
        {
            get => progressClockLowerLeftCornerCharHours;
            set => progressClockLowerLeftCornerCharHours = value;
        }
        /// <summary>
        /// [ProgressClock] Lower left corner character for minutes bar
        /// </summary>
        public char ProgressClockLowerLeftCornerCharMinutes
        {
            get => progressClockLowerLeftCornerCharMinutes;
            set => progressClockLowerLeftCornerCharMinutes = value;
        }
        /// <summary>
        /// [ProgressClock] Lower left corner character for seconds bar
        /// </summary>
        public char ProgressClockLowerLeftCornerCharSeconds
        {
            get => progressClockLowerLeftCornerCharSeconds;
            set => progressClockLowerLeftCornerCharSeconds = value;
        }
        /// <summary>
        /// [ProgressClock] Lower right corner character for hours bar
        /// </summary>
        public char ProgressClockLowerRightCornerCharHours
        {
            get => progressClockLowerRightCornerCharHours;
            set => progressClockLowerRightCornerCharHours = value;
        }
        /// <summary>
        /// [ProgressClock] Lower right corner character for minutes bar
        /// </summary>
        public char ProgressClockLowerRightCornerCharMinutes
        {
            get => progressClockLowerRightCornerCharMinutes;
            set => progressClockLowerRightCornerCharMinutes = value;
        }
        /// <summary>
        /// [ProgressClock] Lower right corner character for seconds bar
        /// </summary>
        public char ProgressClockLowerRightCornerCharSeconds
        {
            get => progressClockLowerRightCornerCharSeconds;
            set => progressClockLowerRightCornerCharSeconds = value;
        }
        /// <summary>
        /// [ProgressClock] Upper frame character for hours bar
        /// </summary>
        public char ProgressClockUpperFrameCharHours
        {
            get => progressClockUpperFrameCharHours;
            set => progressClockUpperFrameCharHours = value;
        }
        /// <summary>
        /// [ProgressClock] Upper frame character for minutes bar
        /// </summary>
        public char ProgressClockUpperFrameCharMinutes
        {
            get => progressClockUpperFrameCharMinutes;
            set => progressClockUpperFrameCharMinutes = value;
        }
        /// <summary>
        /// [ProgressClock] Upper frame character for seconds bar
        /// </summary>
        public char ProgressClockUpperFrameCharSeconds
        {
            get => progressClockUpperFrameCharSeconds;
            set => progressClockUpperFrameCharSeconds = value;
        }
        /// <summary>
        /// [ProgressClock] Lower frame character for hours bar
        /// </summary>
        public char ProgressClockLowerFrameCharHours
        {
            get => progressClockLowerFrameCharHours;
            set => progressClockLowerFrameCharHours = value;
        }
        /// <summary>
        /// [ProgressClock] Lower frame character for minutes bar
        /// </summary>
        public char ProgressClockLowerFrameCharMinutes
        {
            get => progressClockLowerFrameCharMinutes;
            set => progressClockLowerFrameCharMinutes = value;
        }
        /// <summary>
        /// [ProgressClock] Lower frame character for seconds bar
        /// </summary>
        public char ProgressClockLowerFrameCharSeconds
        {
            get => progressClockLowerFrameCharSeconds;
            set => progressClockLowerFrameCharSeconds = value;
        }
        /// <summary>
        /// [ProgressClock] Left frame character for hours bar
        /// </summary>
        public char ProgressClockLeftFrameCharHours
        {
            get => progressClockLeftFrameCharHours;
            set => progressClockLeftFrameCharHours = value;
        }
        /// <summary>
        /// [ProgressClock] Left frame character for minutes bar
        /// </summary>
        public char ProgressClockLeftFrameCharMinutes
        {
            get => progressClockLeftFrameCharMinutes;
            set => progressClockLeftFrameCharMinutes = value;
        }
        /// <summary>
        /// [ProgressClock] Left frame character for seconds bar
        /// </summary>
        public char ProgressClockLeftFrameCharSeconds
        {
            get => progressClockLeftFrameCharSeconds;
            set => progressClockLeftFrameCharSeconds = value;
        }
        /// <summary>
        /// [ProgressClock] Right frame character for hours bar
        /// </summary>
        public char ProgressClockRightFrameCharHours
        {
            get => progressClockRightFrameCharHours;
            set => progressClockRightFrameCharHours = value;
        }
        /// <summary>
        /// [ProgressClock] Right frame character for minutes bar
        /// </summary>
        public char ProgressClockRightFrameCharMinutes
        {
            get => progressClockRightFrameCharMinutes;
            set => progressClockRightFrameCharMinutes = value;
        }
        /// <summary>
        /// [ProgressClock] Right frame character for seconds bar
        /// </summary>
        public char ProgressClockRightFrameCharSeconds
        {
            get => progressClockRightFrameCharSeconds;
            set => progressClockRightFrameCharSeconds = value;
        }
        /// <summary>
        /// [ProgressClock] Information text for hours bar
        /// </summary>
        public string ProgressClockInfoTextHours
        {
            get => progressClockInfoTextHours;
            set => progressClockInfoTextHours = value;
        }
        /// <summary>
        /// [ProgressClock] Information text for minutes bar
        /// </summary>
        public string ProgressClockInfoTextMinutes
        {
            get => progressClockInfoTextMinutes;
            set => progressClockInfoTextMinutes = value;
        }
        /// <summary>
        /// [ProgressClock] Information text for seconds bar
        /// </summary>
        public string ProgressClockInfoTextSeconds
        {
            get => progressClockInfoTextSeconds;
            set => progressClockInfoTextSeconds = value;
        }
        /// <summary>
        /// [ProgressClock] The minimum red color level (true color - hours)
        /// </summary>
        public int ProgressClockMinimumRedColorLevelHours
        {
            get => progressClockMinimumRedColorLevelHours;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressClockMinimumRedColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum green color level (true color - hours)
        /// </summary>
        public int ProgressClockMinimumGreenColorLevelHours
        {
            get => progressClockMinimumGreenColorLevelHours;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressClockMinimumGreenColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum blue color level (true color - hours)
        /// </summary>
        public int ProgressClockMinimumBlueColorLevelHours
        {
            get => progressClockMinimumBlueColorLevelHours;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressClockMinimumBlueColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum color level (255 colors or 16 colors - hours)
        /// </summary>
        public int ProgressClockMinimumColorLevelHours
        {
            get => progressClockMinimumColorLevelHours;
            set
            {
                int FinalMinimumLevel = 255;
                if (value < 0)
                    value = 1;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                progressClockMinimumColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum red color level (true color - hours)
        /// </summary>
        public int ProgressClockMaximumRedColorLevelHours
        {
            get => progressClockMaximumRedColorLevelHours;
            set
            {
                if (value <= progressClockMinimumRedColorLevelHours)
                    value = progressClockMinimumRedColorLevelHours;
                if (value > 255)
                    value = 255;
                progressClockMaximumRedColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum green color level (true color - hours)
        /// </summary>
        public int ProgressClockMaximumGreenColorLevelHours
        {
            get => progressClockMaximumGreenColorLevelHours;
            set
            {
                if (value <= progressClockMinimumGreenColorLevelHours)
                    value = progressClockMinimumGreenColorLevelHours;
                if (value > 255)
                    value = 255;
                progressClockMaximumGreenColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum blue color level (true color - hours)
        /// </summary>
        public int ProgressClockMaximumBlueColorLevelHours
        {
            get => progressClockMaximumBlueColorLevelHours;
            set
            {
                if (value <= progressClockMinimumBlueColorLevelHours)
                    value = progressClockMinimumBlueColorLevelHours;
                if (value > 255)
                    value = 255;
                progressClockMaximumBlueColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum color level (255 colors or 16 colors - hours)
        /// </summary>
        public int ProgressClockMaximumColorLevelHours
        {
            get => progressClockMaximumColorLevelHours;
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= progressClockMinimumColorLevelHours)
                    value = progressClockMinimumColorLevelHours;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                progressClockMaximumColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum red color level (true color - minutes)
        /// </summary>
        public int ProgressClockMinimumRedColorLevelMinutes
        {
            get => progressClockMinimumRedColorLevelMinutes;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressClockMinimumRedColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum green color level (true color - minutes)
        /// </summary>
        public int ProgressClockMinimumGreenColorLevelMinutes
        {
            get => progressClockMinimumGreenColorLevelMinutes;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressClockMinimumGreenColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum blue color level (true color - minutes)
        /// </summary>
        public int ProgressClockMinimumBlueColorLevelMinutes
        {
            get => progressClockMinimumBlueColorLevelMinutes;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressClockMinimumBlueColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum color level (255 colors or 16 colors - minutes)
        /// </summary>
        public int ProgressClockMinimumColorLevelMinutes
        {
            get => progressClockMinimumColorLevelMinutes;
            set
            {
                int FinalMinimumLevel = 255;
                if (value < 0)
                    value = 1;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                progressClockMinimumColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum red color level (true color - minutes)
        /// </summary>
        public int ProgressClockMaximumRedColorLevelMinutes
        {
            get => progressClockMaximumRedColorLevelMinutes;
            set
            {
                if (value <= progressClockMinimumRedColorLevelMinutes)
                    value = progressClockMinimumRedColorLevelMinutes;
                if (value > 255)
                    value = 255;
                progressClockMaximumRedColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum green color level (true color - minutes)
        /// </summary>
        public int ProgressClockMaximumGreenColorLevelMinutes
        {
            get => progressClockMaximumGreenColorLevelMinutes;
            set
            {
                if (value <= progressClockMinimumGreenColorLevelMinutes)
                    value = progressClockMinimumGreenColorLevelMinutes;
                if (value > 255)
                    value = 255;
                progressClockMaximumGreenColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum blue color level (true color - minutes)
        /// </summary>
        public int ProgressClockMaximumBlueColorLevelMinutes
        {
            get => progressClockMaximumBlueColorLevelMinutes;
            set
            {
                if (value <= progressClockMinimumBlueColorLevelMinutes)
                    value = progressClockMinimumBlueColorLevelMinutes;
                if (value > 255)
                    value = 255;
                progressClockMaximumBlueColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum color level (255 colors or 16 colors - minutes)
        /// </summary>
        public int ProgressClockMaximumColorLevelMinutes
        {
            get => progressClockMaximumColorLevelMinutes;
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= progressClockMinimumColorLevelMinutes)
                    value = progressClockMinimumColorLevelMinutes;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                progressClockMaximumColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum red color level (true color - seconds)
        /// </summary>
        public int ProgressClockMinimumRedColorLevelSeconds
        {
            get => progressClockMinimumRedColorLevelSeconds;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressClockMinimumRedColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum green color level (true color - seconds)
        /// </summary>
        public int ProgressClockMinimumGreenColorLevelSeconds
        {
            get => progressClockMinimumGreenColorLevelSeconds;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressClockMinimumGreenColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum blue color level (true color - seconds)
        /// </summary>
        public int ProgressClockMinimumBlueColorLevelSeconds
        {
            get => progressClockMinimumBlueColorLevelSeconds;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressClockMinimumBlueColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum color level (255 colors or 16 colors - seconds)
        /// </summary>
        public int ProgressClockMinimumColorLevelSeconds
        {
            get => progressClockMinimumColorLevelSeconds;
            set
            {
                int FinalMinimumLevel = 255;
                if (value < 0)
                    value = 1;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                progressClockMinimumColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum red color level (true color - seconds)
        /// </summary>
        public int ProgressClockMaximumRedColorLevelSeconds
        {
            get => progressClockMaximumRedColorLevelSeconds;
            set
            {
                if (value <= progressClockMinimumRedColorLevelSeconds)
                    value = progressClockMinimumRedColorLevelSeconds;
                if (value > 255)
                    value = 255;
                progressClockMaximumRedColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum green color level (true color - seconds)
        /// </summary>
        public int ProgressClockMaximumGreenColorLevelSeconds
        {
            get => progressClockMaximumGreenColorLevelSeconds;
            set
            {
                if (value <= progressClockMinimumGreenColorLevelSeconds)
                    value = progressClockMinimumGreenColorLevelSeconds;
                if (value > 255)
                    value = 255;
                progressClockMaximumGreenColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum blue color level (true color - seconds)
        /// </summary>
        public int ProgressClockMaximumBlueColorLevelSeconds
        {
            get => progressClockMaximumBlueColorLevelSeconds;
            set
            {
                if (value <= progressClockMinimumBlueColorLevelSeconds)
                    value = progressClockMinimumBlueColorLevelSeconds;
                if (value > 255)
                    value = 255;
                progressClockMaximumBlueColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum color level (255 colors or 16 colors - seconds)
        /// </summary>
        public int ProgressClockMaximumColorLevelSeconds
        {
            get => progressClockMaximumColorLevelSeconds;
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= progressClockMinimumColorLevelSeconds)
                    value = progressClockMinimumColorLevelSeconds;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                progressClockMaximumColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum red color level (true color)
        /// </summary>
        public int ProgressClockMinimumRedColorLevel
        {
            get => progressClockMinimumRedColorLevel;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressClockMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum green color level (true color)
        /// </summary>
        public int ProgressClockMinimumGreenColorLevel
        {
            get => progressClockMinimumGreenColorLevel;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressClockMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum blue color level (true color)
        /// </summary>
        public int ProgressClockMinimumBlueColorLevel
        {
            get => progressClockMinimumBlueColorLevel;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressClockMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int ProgressClockMinimumColorLevel
        {
            get => progressClockMinimumColorLevel;
            set
            {
                int FinalMinimumLevel = 255;
                if (value < 0)
                    value = 1;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                progressClockMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum red color level (true color)
        /// </summary>
        public int ProgressClockMaximumRedColorLevel
        {
            get => progressClockMaximumRedColorLevel;
            set
            {
                if (value <= progressClockMinimumRedColorLevel)
                    value = progressClockMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                progressClockMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum green color level (true color)
        /// </summary>
        public int ProgressClockMaximumGreenColorLevel
        {
            get => progressClockMaximumGreenColorLevel;
            set
            {
                if (value <= progressClockMinimumGreenColorLevel)
                    value = progressClockMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                progressClockMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum blue color level (true color)
        /// </summary>
        public int ProgressClockMaximumBlueColorLevel
        {
            get => progressClockMaximumBlueColorLevel;
            set
            {
                if (value <= progressClockMinimumBlueColorLevel)
                    value = progressClockMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                progressClockMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int ProgressClockMaximumColorLevel
        {
            get => progressClockMaximumColorLevel;
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= progressClockMinimumColorLevel)
                    value = progressClockMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                progressClockMaximumColorLevel = value;
            }
        }
        #endregion

        #region Lighter
        private bool lighterTrueColor = true;
        private int lighterDelay = 100;
        private int lighterMaxPositions = 10;
        private string lighterBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private int lighterMinimumRedColorLevel = 0;
        private int lighterMinimumGreenColorLevel = 0;
        private int lighterMinimumBlueColorLevel = 0;
        private int lighterMinimumColorLevel = 0;
        private int lighterMaximumRedColorLevel = 255;
        private int lighterMaximumGreenColorLevel = 255;
        private int lighterMaximumBlueColorLevel = 255;
        private int lighterMaximumColorLevel = 255;

        /// <summary>
        /// [Lighter] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool LighterTrueColor
        {
            get
            {
                return lighterTrueColor;
            }
            set
            {
                lighterTrueColor = value;
            }
        }
        /// <summary>
        /// [Lighter] How many milliseconds to wait before making the next write?
        /// </summary>
        public int LighterDelay
        {
            get
            {
                return lighterDelay;
            }
            set
            {
                lighterDelay = value;
            }
        }
        /// <summary>
        /// [Lighter] How many positions to write before starting to blacken them?
        /// </summary>
        public int LighterMaxPositions
        {
            get
            {
                return lighterMaxPositions;
            }
            set
            {
                lighterMaxPositions = value;
            }
        }
        /// <summary>
        /// [Lighter] Screensaver background color
        /// </summary>
        public string LighterBackgroundColor
        {
            get
            {
                return lighterBackgroundColor;
            }
            set
            {
                lighterBackgroundColor = value;
            }
        }
        /// <summary>
        /// [Lighter] The minimum red color level (true color)
        /// </summary>
        public int LighterMinimumRedColorLevel
        {
            get
            {
                return lighterMinimumRedColorLevel;
            }
            set
            {
                lighterMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Lighter] The minimum green color level (true color)
        /// </summary>
        public int LighterMinimumGreenColorLevel
        {
            get
            {
                return lighterMinimumGreenColorLevel;
            }
            set
            {
                lighterMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Lighter] The minimum blue color level (true color)
        /// </summary>
        public int LighterMinimumBlueColorLevel
        {
            get
            {
                return lighterMinimumBlueColorLevel;
            }
            set
            {
                lighterMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Lighter] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int LighterMinimumColorLevel
        {
            get
            {
                return lighterMinimumColorLevel;
            }
            set
            {
                lighterMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Lighter] The maximum red color level (true color)
        /// </summary>
        public int LighterMaximumRedColorLevel
        {
            get
            {
                return lighterMaximumRedColorLevel;
            }
            set
            {
                lighterMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Lighter] The maximum green color level (true color)
        /// </summary>
        public int LighterMaximumGreenColorLevel
        {
            get
            {
                return lighterMaximumGreenColorLevel;
            }
            set
            {
                lighterMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Lighter] The maximum blue color level (true color)
        /// </summary>
        public int LighterMaximumBlueColorLevel
        {
            get
            {
                return lighterMaximumBlueColorLevel;
            }
            set
            {
                lighterMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Lighter] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int LighterMaximumColorLevel
        {
            get
            {
                return lighterMaximumColorLevel;
            }
            set
            {
                lighterMaximumColorLevel = value;
            }
        }
        #endregion

        #region Wipe
        private bool wipeTrueColor = true;
        private int wipeDelay = 10;
        private int wipeWipesNeededToChangeDirection = 10;
        private string wipeBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private int wipeMinimumRedColorLevel = 0;
        private int wipeMinimumGreenColorLevel = 0;
        private int wipeMinimumBlueColorLevel = 0;
        private int wipeMinimumColorLevel = 0;
        private int wipeMaximumRedColorLevel = 255;
        private int wipeMaximumGreenColorLevel = 255;
        private int wipeMaximumBlueColorLevel = 255;
        private int wipeMaximumColorLevel = 255;

        /// <summary>
        /// [Wipe] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool WipeTrueColor
        {
            get
            {
                return wipeTrueColor;
            }
            set
            {
                wipeTrueColor = value;
            }
        }
        /// <summary>
        /// [Wipe] How many milliseconds to wait before making the next write?
        /// </summary>
        public int WipeDelay
        {
            get
            {
                return wipeDelay;
            }
            set
            {
                wipeDelay = value;
            }
        }
        /// <summary>
        /// [Wipe] How many wipes needed to change direction?
        /// </summary>
        public int WipeWipesNeededToChangeDirection
        {
            get
            {
                return wipeWipesNeededToChangeDirection;
            }
            set
            {
                wipeWipesNeededToChangeDirection = value;
            }
        }
        /// <summary>
        /// [Wipe] Screensaver background color
        /// </summary>
        public string WipeBackgroundColor
        {
            get
            {
                return wipeBackgroundColor;
            }
            set
            {
                wipeBackgroundColor = value;
            }
        }
        /// <summary>
        /// [Wipe] The minimum red color level (true color)
        /// </summary>
        public int WipeMinimumRedColorLevel
        {
            get
            {
                return wipeMinimumRedColorLevel;
            }
            set
            {
                wipeMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Wipe] The minimum green color level (true color)
        /// </summary>
        public int WipeMinimumGreenColorLevel
        {
            get
            {
                return wipeMinimumGreenColorLevel;
            }
            set
            {
                wipeMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Wipe] The minimum blue color level (true color)
        /// </summary>
        public int WipeMinimumBlueColorLevel
        {
            get
            {
                return wipeMinimumBlueColorLevel;
            }
            set
            {
                wipeMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Wipe] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int WipeMinimumColorLevel
        {
            get
            {
                return wipeMinimumColorLevel;
            }
            set
            {
                wipeMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Wipe] The maximum red color level (true color)
        /// </summary>
        public int WipeMaximumRedColorLevel
        {
            get
            {
                return wipeMaximumRedColorLevel;
            }
            set
            {
                wipeMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Wipe] The maximum green color level (true color)
        /// </summary>
        public int WipeMaximumGreenColorLevel
        {
            get
            {
                return wipeMaximumGreenColorLevel;
            }
            set
            {
                wipeMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Wipe] The maximum blue color level (true color)
        /// </summary>
        public int WipeMaximumBlueColorLevel
        {
            get
            {
                return wipeMaximumBlueColorLevel;
            }
            set
            {
                wipeMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Wipe] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int WipeMaximumColorLevel
        {
            get
            {
                return wipeMaximumColorLevel;
            }
            set
            {
                wipeMaximumColorLevel = value;
            }
        }
        #endregion

        #region SimpleMatrix
        private int simpleMatrixDelay = 1;

        /// <summary>
        /// [SimpleMatrix] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SimpleMatrixDelay
        {
            get
            {
                return simpleMatrixDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                simpleMatrixDelay = value;
            }
        }
        #endregion

        #region GlitterMatrix
        private int glitterMatrixDelay = 1;
        private string glitterMatrixBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private string glitterMatrixForegroundColor = new Color(ConsoleColors.Green).PlainSequence;

        /// <summary>
        /// [GlitterMatrix] How many milliseconds to wait before making the next write?
        /// </summary>
        public int GlitterMatrixDelay
        {
            get
            {
                return glitterMatrixDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                glitterMatrixDelay = value;
            }
        }
        /// <summary>
        /// [GlitterMatrix] Screensaver background color
        /// </summary>
        public string GlitterMatrixBackgroundColor
        {
            get
            {
                return glitterMatrixBackgroundColor;
            }
            set
            {
                glitterMatrixBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [GlitterMatrix] Screensaver foreground color
        /// </summary>
        public string GlitterMatrixForegroundColor
        {
            get
            {
                return glitterMatrixForegroundColor;
            }
            set
            {
                glitterMatrixForegroundColor = new Color(value).PlainSequence;
            }
        }
        #endregion

        #region BouncingText
        private bool bouncingTextTrueColor = true;
        private int bouncingTextDelay = 50;
        private string bouncingTextWrite = "Nitrocid KS";
        private string bouncingTextBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private string bouncingTextForegroundColor = new Color(ConsoleColors.White).PlainSequence;
        private int bouncingTextMinimumRedColorLevel = 0;
        private int bouncingTextMinimumGreenColorLevel = 0;
        private int bouncingTextMinimumBlueColorLevel = 0;
        private int bouncingTextMinimumColorLevel = 0;
        private int bouncingTextMaximumRedColorLevel = 255;
        private int bouncingTextMaximumGreenColorLevel = 255;
        private int bouncingTextMaximumBlueColorLevel = 255;
        private int bouncingTextMaximumColorLevel = 255;

        /// <summary>
        /// [BouncingText] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool BouncingTextTrueColor
        {
            get
            {
                return bouncingTextTrueColor;
            }
            set
            {
                bouncingTextTrueColor = value;
            }
        }
        /// <summary>
        /// [BouncingText] How many milliseconds to wait before making the next write?
        /// </summary>
        public int BouncingTextDelay
        {
            get
            {
                return bouncingTextDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                bouncingTextDelay = value;
            }
        }
        /// <summary>
        /// [BouncingText] Text for Bouncing Text. Shorter is better.
        /// </summary>
        public string BouncingTextWrite
        {
            get
            {
                return bouncingTextWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                bouncingTextWrite = value;
            }
        }
        /// <summary>
        /// [BouncingText] Screensaver background color
        /// </summary>
        public string BouncingTextBackgroundColor
        {
            get
            {
                return bouncingTextBackgroundColor;
            }
            set
            {
                bouncingTextBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BouncingText] Screensaver foreground color
        /// </summary>
        public string BouncingTextForegroundColor
        {
            get
            {
                return bouncingTextForegroundColor;
            }
            set
            {
                bouncingTextForegroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BouncingText] The minimum red color level (true color)
        /// </summary>
        public int BouncingTextMinimumRedColorLevel
        {
            get
            {
                return bouncingTextMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                bouncingTextMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingText] The minimum green color level (true color)
        /// </summary>
        public int BouncingTextMinimumGreenColorLevel
        {
            get
            {
                return bouncingTextMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                bouncingTextMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingText] The minimum blue color level (true color)
        /// </summary>
        public int BouncingTextMinimumBlueColorLevel
        {
            get
            {
                return bouncingTextMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                bouncingTextMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingText] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int BouncingTextMinimumColorLevel
        {
            get
            {
                return bouncingTextMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                bouncingTextMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingText] The maximum red color level (true color)
        /// </summary>
        public int BouncingTextMaximumRedColorLevel
        {
            get
            {
                return bouncingTextMaximumRedColorLevel;
            }
            set
            {
                if (value <= bouncingTextMinimumRedColorLevel)
                    value = bouncingTextMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                bouncingTextMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingText] The maximum green color level (true color)
        /// </summary>
        public int BouncingTextMaximumGreenColorLevel
        {
            get
            {
                return bouncingTextMaximumGreenColorLevel;
            }
            set
            {
                if (value <= bouncingTextMinimumGreenColorLevel)
                    value = bouncingTextMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                bouncingTextMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingText] The maximum blue color level (true color)
        /// </summary>
        public int BouncingTextMaximumBlueColorLevel
        {
            get
            {
                return bouncingTextMaximumBlueColorLevel;
            }
            set
            {
                if (value <= bouncingTextMinimumBlueColorLevel)
                    value = bouncingTextMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                bouncingTextMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingText] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int BouncingTextMaximumColorLevel
        {
            get
            {
                return bouncingTextMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= bouncingTextMinimumColorLevel)
                    value = bouncingTextMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                bouncingTextMaximumColorLevel = value;
            }
        }
        #endregion

        #region Fader
        private int faderDelay = 50;
        private int faderFadeOutDelay = 3000;
        private string faderWrite = "Nitrocid KS";
        private int faderMaxSteps = 25;
        private string faderBackgroundColor = new Color(0, 0, 0).PlainSequence;
        private int faderMinimumRedColorLevel = 0;
        private int faderMinimumGreenColorLevel = 0;
        private int faderMinimumBlueColorLevel = 0;
        private int faderMaximumRedColorLevel = 255;
        private int faderMaximumGreenColorLevel = 255;
        private int faderMaximumBlueColorLevel = 255;

        /// <summary>
        /// [Fader] How many milliseconds to wait before making the next write?
        /// </summary>
        public int FaderDelay
        {
            get
            {
                return faderDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                faderDelay = value;
            }
        }
        /// <summary>
        /// [Fader] How many milliseconds to wait before fading the text out?
        /// </summary>
        public int FaderFadeOutDelay
        {
            get
            {
                return faderFadeOutDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                faderFadeOutDelay = value;
            }
        }
        /// <summary>
        /// [Fader] Text for Fader. Shorter is better.
        /// </summary>
        public string FaderWrite
        {
            get
            {
                return faderWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                faderWrite = value;
            }
        }
        /// <summary>
        /// [Fader] How many fade steps to do?
        /// </summary>
        public int FaderMaxSteps
        {
            get
            {
                return faderMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                faderMaxSteps = value;
            }
        }
        /// <summary>
        /// [Fader] Screensaver background color
        /// </summary>
        public string FaderBackgroundColor
        {
            get
            {
                return faderBackgroundColor;
            }
            set
            {
                faderBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Fader] The minimum red color level (true color)
        /// </summary>
        public int FaderMinimumRedColorLevel
        {
            get
            {
                return faderMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                faderMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Fader] The minimum green color level (true color)
        /// </summary>
        public int FaderMinimumGreenColorLevel
        {
            get
            {
                return faderMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                faderMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Fader] The minimum blue color level (true color)
        /// </summary>
        public int FaderMinimumBlueColorLevel
        {
            get
            {
                return faderMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                faderMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Fader] The maximum red color level (true color)
        /// </summary>
        public int FaderMaximumRedColorLevel
        {
            get
            {
                return faderMaximumRedColorLevel;
            }
            set
            {
                if (value <= faderMinimumRedColorLevel)
                    value = faderMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                faderMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Fader] The maximum green color level (true color)
        /// </summary>
        public int FaderMaximumGreenColorLevel
        {
            get
            {
                return faderMaximumGreenColorLevel;
            }
            set
            {
                if (value <= faderMinimumGreenColorLevel)
                    value = faderMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                faderMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Fader] The maximum blue color level (true color)
        /// </summary>
        public int FaderMaximumBlueColorLevel
        {
            get
            {
                return faderMaximumBlueColorLevel;
            }
            set
            {
                if (value <= faderMinimumBlueColorLevel)
                    value = faderMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                faderMaximumBlueColorLevel = value;
            }
        }
        #endregion

        #region FaderBack
        private int faderBackDelay = 10;
        private int faderBackFadeOutDelay = 3000;
        private int faderBackMaxSteps = 25;
        private int faderBackMinimumRedColorLevel = 0;
        private int faderBackMinimumGreenColorLevel = 0;
        private int faderBackMinimumBlueColorLevel = 0;
        private int faderBackMaximumRedColorLevel = 255;
        private int faderBackMaximumGreenColorLevel = 255;
        private int faderBackMaximumBlueColorLevel = 255;

        /// <summary>
        /// [FaderBack] How many milliseconds to wait before making the next write?
        /// </summary>
        public int FaderBackDelay
        {
            get
            {
                return faderBackDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                faderBackDelay = value;
            }
        }
        /// <summary>
        /// [FaderBack] How many milliseconds to wait before fading the text out?
        /// </summary>
        public int FaderBackFadeOutDelay
        {
            get
            {
                return faderBackFadeOutDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                faderBackFadeOutDelay = value;
            }
        }
        /// <summary>
        /// [FaderBack] How many fade steps to do?
        /// </summary>
        public int FaderBackMaxSteps
        {
            get
            {
                return faderBackMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                faderBackMaxSteps = value;
            }
        }
        /// <summary>
        /// [FaderBack] The minimum red color level (true color)
        /// </summary>
        public int FaderBackMinimumRedColorLevel
        {
            get
            {
                return faderBackMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                faderBackMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FaderBack] The minimum green color level (true color)
        /// </summary>
        public int FaderBackMinimumGreenColorLevel
        {
            get
            {
                return faderBackMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                faderBackMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FaderBack] The minimum blue color level (true color)
        /// </summary>
        public int FaderBackMinimumBlueColorLevel
        {
            get
            {
                return faderBackMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                faderBackMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FaderBack] The maximum red color level (true color)
        /// </summary>
        public int FaderBackMaximumRedColorLevel
        {
            get
            {
                return faderBackMaximumRedColorLevel;
            }
            set
            {
                if (value <= faderBackMinimumRedColorLevel)
                    value = faderBackMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                faderBackMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FaderBack] The maximum green color level (true color)
        /// </summary>
        public int FaderBackMaximumGreenColorLevel
        {
            get
            {
                return faderBackMaximumGreenColorLevel;
            }
            set
            {
                if (value <= faderBackMinimumGreenColorLevel)
                    value = faderBackMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                faderBackMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FaderBack] The maximum blue color level (true color)
        /// </summary>
        public int FaderBackMaximumBlueColorLevel
        {
            get
            {
                return faderBackMaximumBlueColorLevel;
            }
            set
            {
                if (value <= faderBackMinimumBlueColorLevel)
                    value = faderBackMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                faderBackMaximumBlueColorLevel = value;
            }
        }
        #endregion

        #region BeatFader
        private bool beatFaderTrueColor = true;
        private int beatFaderDelay = 120;
        private bool beatFaderCycleColors = true;
        private string beatFaderBeatColor = "17";
        private int beatFaderMaxSteps = 25;
        private int beatFaderMinimumRedColorLevel = 0;
        private int beatFaderMinimumGreenColorLevel = 0;
        private int beatFaderMinimumBlueColorLevel = 0;
        private int beatFaderMinimumColorLevel = 0;
        private int beatFaderMaximumRedColorLevel = 255;
        private int beatFaderMaximumGreenColorLevel = 255;
        private int beatFaderMaximumBlueColorLevel = 255;
        private int beatFaderMaximumColorLevel = 255;

        /// <summary>
        /// [BeatFader] Enable truecolor support. Has a higher priority than 255 color support. Please note that it only works if color cycling is enabled.
        /// </summary>
        public bool BeatFaderTrueColor
        {
            get
            {
                return beatFaderTrueColor;
            }
            set
            {
                beatFaderTrueColor = value;
            }
        }
        /// <summary>
        /// [BeatFader] Enable color cycling (uses RNG. If disabled, uses the <see cref="BeatFaderBeatColor"/> color.)
        /// </summary>
        public bool BeatFaderCycleColors
        {
            get
            {
                return beatFaderCycleColors;
            }
            set
            {
                beatFaderCycleColors = value;
            }
        }
        /// <summary>
        /// [BeatFader] The color of beats. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public string BeatFaderBeatColor
        {
            get
            {
                return beatFaderBeatColor;
            }
            set
            {
                beatFaderBeatColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BeatFader] How many beats per minute to wait before making the next write?
        /// </summary>
        public int BeatFaderDelay
        {
            get
            {
                return beatFaderDelay;
            }
            set
            {
                if (value <= 0)
                    value = 120;
                beatFaderDelay = value;
            }
        }
        /// <summary>
        /// [BeatFader] How many fade steps to do?
        /// </summary>
        public int BeatFaderMaxSteps
        {
            get
            {
                return beatFaderMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                beatFaderMaxSteps = value;
            }
        }
        /// <summary>
        /// [BeatFader] The minimum red color level (true color)
        /// </summary>
        public int BeatFaderMinimumRedColorLevel
        {
            get
            {
                return beatFaderMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                beatFaderMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The minimum green color level (true color)
        /// </summary>
        public int BeatFaderMinimumGreenColorLevel
        {
            get
            {
                return beatFaderMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                beatFaderMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The minimum blue color level (true color)
        /// </summary>
        public int BeatFaderMinimumBlueColorLevel
        {
            get
            {
                return beatFaderMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                beatFaderMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int BeatFaderMinimumColorLevel
        {
            get
            {
                return beatFaderMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                beatFaderMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The maximum red color level (true color)
        /// </summary>
        public int BeatFaderMaximumRedColorLevel
        {
            get
            {
                return beatFaderMaximumRedColorLevel;
            }
            set
            {
                if (value <= beatFaderMinimumRedColorLevel)
                    value = beatFaderMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                beatFaderMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The maximum green color level (true color)
        /// </summary>
        public int BeatFaderMaximumGreenColorLevel
        {
            get
            {
                return beatFaderMaximumGreenColorLevel;
            }
            set
            {
                if (value <= beatFaderMinimumGreenColorLevel)
                    value = beatFaderMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                beatFaderMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The maximum blue color level (true color)
        /// </summary>
        public int BeatFaderMaximumBlueColorLevel
        {
            get
            {
                return beatFaderMaximumBlueColorLevel;
            }
            set
            {
                if (value <= beatFaderMinimumBlueColorLevel)
                    value = beatFaderMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                beatFaderMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int BeatFaderMaximumColorLevel
        {
            get
            {
                return beatFaderMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= beatFaderMinimumColorLevel)
                    value = beatFaderMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                beatFaderMaximumColorLevel = value;
            }
        }
        #endregion

        #region Typo
        private int typoDelay = 50;
        private int typoWriteAgainDelay = 3000;
        private string typoWrite = "Nitrocid KS";
        private int typoWritingSpeedMin = 50;
        private int typoWritingSpeedMax = 80;
        private int typoMissStrikePossibility = 20;
        private int typoMissPossibility = 10;
        private string typoTextColor = new Color(ConsoleColors.White).PlainSequence;

        /// <summary>
        /// [Typo] How many milliseconds to wait before making the next write?
        /// </summary>
        public int TypoDelay
        {
            get
            {
                return typoDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                typoDelay = value;
            }
        }
        /// <summary>
        /// [Typo] How many milliseconds to wait before writing the text again?
        /// </summary>
        public int TypoWriteAgainDelay
        {
            get
            {
                return typoWriteAgainDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                typoWriteAgainDelay = value;
            }
        }
        /// <summary>
        /// [Typo] Text for Typo. Longer is better.
        /// </summary>
        public string TypoWrite
        {
            get
            {
                return typoWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                typoWrite = value;
            }
        }
        /// <summary>
        /// [Typo] Minimum writing speed in WPM
        /// </summary>
        public int TypoWritingSpeedMin
        {
            get
            {
                return typoWritingSpeedMin;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                typoWritingSpeedMin = value;
            }
        }
        /// <summary>
        /// [Typo] Maximum writing speed in WPM
        /// </summary>
        public int TypoWritingSpeedMax
        {
            get
            {
                return typoWritingSpeedMax;
            }
            set
            {
                if (value <= 0)
                    value = 80;
                typoWritingSpeedMax = value;
            }
        }
        /// <summary>
        /// [Typo] Possibility that the writer made a typo in percent
        /// </summary>
        public int TypoMissStrikePossibility
        {
            get
            {
                return typoMissStrikePossibility;
            }
            set
            {
                if (value <= 0)
                    value = 20;
                typoMissStrikePossibility = value;
            }
        }
        /// <summary>
        /// [Typo] Possibility that the writer missed a character in percent
        /// </summary>
        public int TypoMissPossibility
        {
            get
            {
                return typoMissPossibility;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                typoMissPossibility = value;
            }
        }
        /// <summary>
        /// [Typo] Text color
        /// </summary>
        public string TypoTextColor
        {
            get
            {
                return typoTextColor;
            }
            set
            {
                typoTextColor = new Color(value).PlainSequence;
            }
        }
        #endregion

        #region Marquee
        private bool marqueeTrueColor = true;
        private int marqueeDelay = 10;
        private string marqueeWrite = "Nitrocid KS";
        private bool marqueeAlwaysCentered = true;
        private bool marqueeUseConsoleAPI;
        private string marqueeBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private int marqueeMinimumRedColorLevel = 0;
        private int marqueeMinimumGreenColorLevel = 0;
        private int marqueeMinimumBlueColorLevel = 0;
        private int marqueeMinimumColorLevel = 0;
        private int marqueeMaximumRedColorLevel = 255;
        private int marqueeMaximumGreenColorLevel = 255;
        private int marqueeMaximumBlueColorLevel = 255;
        private int marqueeMaximumColorLevel = 255;

        /// <summary>
        /// [Marquee] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool MarqueeTrueColor
        {
            get
            {
                return marqueeTrueColor;
            }
            set
            {
                marqueeTrueColor = value;
            }
        }
        /// <summary>
        /// [Marquee] How many milliseconds to wait before making the next write?
        /// </summary>
        public int MarqueeDelay
        {
            get
            {
                return marqueeDelay;
            }
            set
            {
                marqueeDelay = value;
            }
        }
        /// <summary>
        /// [Marquee] Text for Marquee. Shorter is better.
        /// </summary>
        public string MarqueeWrite
        {
            get
            {
                return marqueeWrite;
            }
            set
            {
                marqueeWrite = value;
            }
        }
        /// <summary>
        /// [Marquee] Whether the text is always on center.
        /// </summary>
        public bool MarqueeAlwaysCentered
        {
            get
            {
                return marqueeAlwaysCentered;
            }
            set
            {
                marqueeAlwaysCentered = value;
            }
        }
        /// <summary>
        /// [Marquee] Whether to use the KS.ConsoleBase.ConsoleWrapper.Clear() API (slow) or use the line-clearing VT sequence (fast).
        /// </summary>
        public bool MarqueeUseConsoleAPI
        {
            get
            {
                return marqueeUseConsoleAPI;
            }
            set
            {
                marqueeUseConsoleAPI = value;
            }
        }
        /// <summary>
        /// [Marquee] Screensaver background color
        /// </summary>
        public string MarqueeBackgroundColor
        {
            get
            {
                return marqueeBackgroundColor;
            }
            set
            {
                marqueeBackgroundColor = value;
            }
        }
        /// <summary>
        /// [Marquee] The minimum red color level (true color)
        /// </summary>
        public int MarqueeMinimumRedColorLevel
        {
            get
            {
                return marqueeMinimumRedColorLevel;
            }
            set
            {
                marqueeMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Marquee] The minimum green color level (true color)
        /// </summary>
        public int MarqueeMinimumGreenColorLevel
        {
            get
            {
                return marqueeMinimumGreenColorLevel;
            }
            set
            {
                marqueeMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Marquee] The minimum blue color level (true color)
        /// </summary>
        public int MarqueeMinimumBlueColorLevel
        {
            get
            {
                return marqueeMinimumBlueColorLevel;
            }
            set
            {
                marqueeMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Marquee] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int MarqueeMinimumColorLevel
        {
            get
            {
                return marqueeMinimumColorLevel;
            }
            set
            {
                marqueeMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Marquee] The maximum red color level (true color)
        /// </summary>
        public int MarqueeMaximumRedColorLevel
        {
            get
            {
                return marqueeMaximumRedColorLevel;
            }
            set
            {
                marqueeMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Marquee] The maximum green color level (true color)
        /// </summary>
        public int MarqueeMaximumGreenColorLevel
        {
            get
            {
                return marqueeMaximumGreenColorLevel;
            }
            set
            {
                marqueeMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Marquee] The maximum blue color level (true color)
        /// </summary>
        public int MarqueeMaximumBlueColorLevel
        {
            get
            {
                return marqueeMaximumBlueColorLevel;
            }
            set
            {
                marqueeMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Marquee] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int MarqueeMaximumColorLevel
        {
            get
            {
                return marqueeMaximumColorLevel;
            }
            set
            {
                marqueeMaximumColorLevel = value;
            }
        }
        #endregion

        #region Linotypo
        private int linotypoDelay = 50;
        private int linotypoNewScreenDelay = 3000;
        private string linotypoWrite = "Nitrocid KS";
        private int linotypoWritingSpeedMin = 50;
        private int linotypoWritingSpeedMax = 80;
        private int linotypoMissStrikePossibility = 1;
        private int linotypoTextColumns = 3;
        private int linotypoEtaoinThreshold = 5;
        private int linotypoEtaoinCappingPossibility = 5;
        private int linotypoEtaoinType = 0;
        private int linotypoMissPossibility = 10;
        private string linotypoTextColor = new Color(ConsoleColors.White).PlainSequence;

        /// <summary>
        /// [Linotypo] How many milliseconds to wait before making the next write?
        /// </summary>
        public int LinotypoDelay
        {
            get
            {
                return linotypoDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                linotypoDelay = value;
            }
        }
        /// <summary>
        /// [Linotypo] How many milliseconds to wait before writing the text in the new screen again?
        /// </summary>
        public int LinotypoNewScreenDelay
        {
            get
            {
                return linotypoNewScreenDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                linotypoNewScreenDelay = value;
            }
        }
        /// <summary>
        /// [Linotypo] Text for Linotypo. Longer is better.
        /// </summary>
        public string LinotypoWrite
        {
            get
            {
                return linotypoWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                linotypoWrite = value;
            }
        }
        /// <summary>
        /// [Linotypo] Minimum writing speed in WPM
        /// </summary>
        public int LinotypoWritingSpeedMin
        {
            get
            {
                return linotypoWritingSpeedMin;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                linotypoWritingSpeedMin = value;
            }
        }
        /// <summary>
        /// [Linotypo] Maximum writing speed in WPM
        /// </summary>
        public int LinotypoWritingSpeedMax
        {
            get
            {
                return linotypoWritingSpeedMax;
            }
            set
            {
                if (value <= 0)
                    value = 80;
                linotypoWritingSpeedMax = value;
            }
        }
        /// <summary>
        /// [Linotypo] Possibility that the writer made a typo in percent
        /// </summary>
        public int LinotypoMissStrikePossibility
        {
            get
            {
                return linotypoMissStrikePossibility;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                linotypoMissStrikePossibility = value;
            }
        }
        /// <summary>
        /// [Linotypo] The text columns to be printed.
        /// </summary>
        public int LinotypoTextColumns
        {
            get
            {
                return linotypoTextColumns;
            }
            set
            {
                if (value <= 0)
                    value = 3;
                if (value > 3)
                    value = 3;
                linotypoTextColumns = value;
            }
        }
        /// <summary>
        /// [Linotypo] How many characters to write before triggering the "line fill"?
        /// </summary>
        public int LinotypoEtaoinThreshold
        {
            get
            {
                return linotypoEtaoinThreshold;
            }
            set
            {
                if (value <= 0)
                    value = 5;
                if (value > 8)
                    value = 8;
                linotypoEtaoinThreshold = value;
            }
        }
        /// <summary>
        /// [Linotypo] Possibility that the Etaoin pattern will be printed in all caps in percent
        /// </summary>
        public int LinotypoEtaoinCappingPossibility
        {
            get
            {
                return linotypoEtaoinCappingPossibility;
            }
            set
            {
                if (value <= 0)
                    value = 5;
                linotypoEtaoinCappingPossibility = value;
            }
        }
        /// <summary>
        /// [Linotypo] Line fill pattern type
        /// </summary>
        public FillType LinotypoEtaoinType
        {
            get
            {
                return (FillType)linotypoEtaoinType;
            }
            set
            {
                linotypoEtaoinType = (int)value;
            }
        }
        /// <summary>
        /// [Linotypo] Possibility that the writer missed a character in percent
        /// </summary>
        public int LinotypoMissPossibility
        {
            get
            {
                return linotypoMissPossibility;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                linotypoMissPossibility = value;
            }
        }
        /// <summary>
        /// [Linotypo] Text color
        /// </summary>
        public string LinotypoTextColor
        {
            get
            {
                return linotypoTextColor;
            }
            set
            {
                linotypoTextColor = new Color(value).PlainSequence;
            }
        }
        #endregion

        #region Typewriter
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
        #endregion

        #region FlashColor
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
        #endregion

        #region SpotWrite
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
        #endregion

        #region Ramp
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
        #endregion

        #region StackBox
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
        #endregion

        #region Snaker
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
        #endregion

        #region BarRot
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
        #endregion

        #region Fireworks
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
        #endregion

        #region Figlet
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
        /// [Figlet] Enables the rainbow colors mode
        /// </summary>
        public bool FigletRainbowMode { get; set; }
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
        #endregion

        #region FlashText
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
        #endregion

        #region Noise
        /// <summary>
        /// [Noise] How many milliseconds to wait before making the new screen?
        /// </summary>
        public int NoiseNewScreenDelay { get; set; } = 5000;
        /// <summary>
        /// [Noise] The noise density in percent
        /// </summary>
        public int NoiseDensity { get; set; } = 40;
        #endregion

        #region DateAndTime
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
        #endregion

        #region Glitch
        /// <summary>
        /// [Glitch] How many milliseconds to wait before making the next write?
        /// </summary>
        public int GlitchDelay { get; set; } = 10;
        /// <summary>
        /// [Glitch] The Glitch density in percent
        /// </summary>
        public int GlitchDensity { get; set; } = 40;
        #endregion

        #region FallingLine
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
        #endregion

        #region Indeterminate
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
        #endregion

        #region Pulse
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
        #endregion

        #region BeatPulse
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
        #endregion

        #region EdgePulse
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
        #endregion

        #region BeatEdgePulse
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
        #endregion

        #region GradientRot
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
        #endregion

        #region Gradient
        /// <summary>
        /// [Gradient] How many milliseconds to wait before rotting the next screen?
        /// </summary>
        public int GradientNextRotDelay { get; set; } = 3000;
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
        #endregion

        #region Starfield
        /// <summary>
        /// [Starfield] How many milliseconds to wait before making the next write?
        /// </summary>
        public int StarfieldDelay { get; set; } = 10;
        #endregion

        #region Siren
        /// <summary>
        /// [Siren] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SirenDelay { get; set; } = 500;
        /// <summary>
        /// [Siren] The siren style
        /// </summary>
        public string SirenStyle { get; set; } = "Cop";
        #endregion

        #region Spin
        /// <summary>
        /// [Spin] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SpinDelay { get; set; } = 10;
        #endregion

        #region SnakeFill
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
        #endregion

        #region Equalizer
        /// <summary>
        /// [Equalizer] How many milliseconds to wait before going to next equalizer preset?
        /// </summary>
        public int EqualizerNextScreenDelay { get; set; } = 3000;
        #endregion

        #region BSOD
        /// <summary>
        /// [BSOD] How many beats per minute to wait before making the next write?
        /// </summary>
        public int BSODDelay { get; set; } = 10000;
        #endregion

        #region Memdump
        /// <summary>
        /// [Memdump] How many milliseconds to wait before making the next write?
        /// </summary>
        public int MemdumpDelay { get; set; } = 500;
        #endregion

        #region ExcaliBeats
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
        #endregion

        #region BarWave
        /// <summary>
        /// [BarWave] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool BarWaveTrueColor { get; set; } = true;
        /// <summary>
        /// [BarWave] How many milliseconds to wait before making the next write?
        /// </summary>
        public int BarWaveDelay { get; set; } = 100;
        /// <summary>
        /// [BarWave] The level of the frequency. This is the denominator of the Pi value (3.1415926...) in mathematics, defined by <see cref="Math.PI"/>.
        /// </summary>
        public double BarWaveFrequencyLevel { get; set; } = 2;
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
        #endregion

        #region Wave
        /// <summary>
        /// [Wave] How many milliseconds to wait before making the next write?
        /// </summary>
        public int WaveDelay { get; set; } = 100;
        /// <summary>
        /// [Wave] The level of the frequency. This is the denominator of the Pi value (3.1415926...) in mathematics, defined by <see cref="Math.PI"/>.
        /// </summary>
        public double WaveFrequencyLevel { get; set; } = 3;
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
        #endregion

        #region Mesmerize
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
        #endregion

        #region Aurora
        /// <summary>
        /// [Aurora] How many milliseconds to wait before making the next write?
        /// </summary>
        public int AuroraDelay { get; set; } = 100;
        #endregion

        #region Lightning
        /// <summary>
        /// [Lightning] How many milliseconds to wait before making the next write?
        /// </summary>
        public int LightningDelay { get; set; } = 100;
        /// <summary>
        /// [Lightning] Chance, in percent, to strike
        /// </summary>
        public int LightningStrikeProbability { get; set; } = 5;
        #endregion

        #region Bloom
        /// <summary>
        /// [Bloom] How many milliseconds to wait before making the next write?
        /// </summary>
        public int BloomDelay { get; set; } = 50;
        /// <summary>
        /// [Bloom] Whether to use dark colors or not
        /// </summary>
        public bool BloomDarkColors { get; set; }
        /// <summary>
        /// [Bloom] How many color steps for transitioning between two colors?
        /// </summary>
        public int BloomSteps { get; set; } = 100;
        #endregion

        #region WordHasher
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
        #endregion

        #region SquareCorner
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
        #endregion

        #region NumberScatter
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
        #endregion

        #region Quote
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
        #endregion

        #region BoxGrid
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
        #endregion

        #region ColorBleed
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
        #endregion

        #region Text
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
        /// [Text] Enables the rainbow colors mode
        /// </summary>
        public bool TextRainbowMode { get; set; }
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
        #endregion

        #region TextBox
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
        /// [TextBox] Enables the rainbow colors mode
        /// </summary>
        public bool TextBoxRainbowMode { get; set; }
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
        #endregion

        #region WordHasherWrite
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
        #endregion

        #region SirenTheme
        /// <summary>
        /// [SirenTheme] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SirenThemeDelay { get; set; } = 500;
        /// <summary>
        /// [SirenTheme] The siren style
        /// </summary>
        public string SirenThemeStyle { get; set; } = "Default";
        #endregion

        #region StarfieldWarp
        /// <summary>
        /// [StarfieldWarp] How many milliseconds to wait before making the next write?
        /// </summary>
        public int StarfieldWarpDelay { get; set; } = 10;
        #endregion

        #region Speckles
        /// <summary>
        /// [Speckles] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SpecklesDelay { get; set; } = 10;
        #endregion

        #region LetterScatter
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
        #endregion

        #region MultiLines
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
        #endregion

        #region LaserBeams
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
        #endregion

        #region TextWander
        /// <summary>
        /// [TextWander] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool TextWanderTrueColor { get; set; } = true;
        /// <summary>
        /// [TextWander] How many milliseconds to wait before making the next write?
        /// </summary>
        public int TextWanderDelay { get; set; } = 1000;
        /// <summary>
        /// [TextWander] Text to write. Shorter is better.
        /// </summary>
        public string TextWanderWrite { get; set; } = "Nitrocid KS";
        /// <summary>
        /// [TextWander] The minimum red color level (true color)
        /// </summary>
        public int TextWanderMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [TextWander] The minimum green color level (true color)
        /// </summary>
        public int TextWanderMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [TextWander] The minimum blue color level (true color)
        /// </summary>
        public int TextWanderMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [TextWander] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int TextWanderMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [TextWander] The maximum red color level (true color)
        /// </summary>
        public int TextWanderMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [TextWander] The maximum green color level (true color)
        /// </summary>
        public int TextWanderMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [TextWander] The maximum blue color level (true color)
        /// </summary>
        public int TextWanderMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [TextWander] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int TextWanderMaximumColorLevel { get; set; } = 255;
        #endregion

        #region Swivel
        /// <summary>
        /// [Swivel] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SwivelDelay { get; set; } = 100;
        /// <summary>
        /// [Swivel] The level of the horizontal frequency. This is the denominator of the Pi value (3.1415926...) in mathematics, defined by <see cref="Math.PI"/>. Use this to create beautiful wavy swivels!
        /// </summary>
        public double SwivelHorizontalFrequencyLevel { get; set; } = 3;
        /// <summary>
        /// [Swivel] The level of the vertical frequency. This is the denominator of the Pi value (3.1415926...) in mathematics, defined by <see cref="Math.PI"/>. Use this to create beautiful wavy swivels!
        /// </summary>
        public double SwivelVerticalFrequencyLevel { get; set; } = 8;
        /// <summary>
        /// [Swivel] The minimum red color level (true color)
        /// </summary>
        public int SwivelMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [Swivel] The minimum green color level (true color)
        /// </summary>
        public int SwivelMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [Swivel] The minimum blue color level (true color)
        /// </summary>
        public int SwivelMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [Swivel] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int SwivelMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [Swivel] The maximum red color level (true color)
        /// </summary>
        public int SwivelMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [Swivel] The maximum green color level (true color)
        /// </summary>
        public int SwivelMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [Swivel] The maximum blue color level (true color)
        /// </summary>
        public int SwivelMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [Swivel] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int SwivelMaximumColorLevel { get; set; } = 255;
        #endregion

        #region DoorShift
        /// <summary>
        /// [DoorShift] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool DoorShiftTrueColor { get; set; } = true;
        /// <summary>
        /// [DoorShift] How many milliseconds to wait before making the next write?
        /// </summary>
        public int DoorShiftDelay { get; set; } = 10;
        /// <summary>
        /// [DoorShift] Screensaver background color
        /// </summary>
        public string DoorShiftBackgroundColor { get; set; } = new Color(ConsoleColors.Black).PlainSequence;
        /// <summary>
        /// [DoorShift] The minimum red color level (true color)
        /// </summary>
        public int DoorShiftMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [DoorShift] The minimum green color level (true color)
        /// </summary>
        public int DoorShiftMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [DoorShift] The minimum blue color level (true color)
        /// </summary>
        public int DoorShiftMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [DoorShift] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int DoorShiftMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [DoorShift] The maximum red color level (true color)
        /// </summary>
        public int DoorShiftMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [DoorShift] The maximum green color level (true color)
        /// </summary>
        public int DoorShiftMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [DoorShift] The maximum blue color level (true color)
        /// </summary>
        public int DoorShiftMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [DoorShift] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int DoorShiftMaximumColorLevel { get; set; } = 255;
        #endregion

        #region GradientBloom
        /// <summary>
        /// [GradientBloom] How many milliseconds to wait before making the next write?
        /// </summary>
        public int GradientBloomDelay { get; set; } = 50;
        /// <summary>
        /// [GradientBloom] Whether to use dark colors or not
        /// </summary>
        public bool GradientBloomDarkColors { get; set; }
        /// <summary>
        /// [GradientBloom] How many color steps for transitioning between two colors?
        /// </summary>
        public int GradientBloomSteps { get; set; } = 100;
        #endregion

        #region SkyComet
        /// <summary>
        /// [SkyComet] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SkyCometDelay { get; set; } = 10;
        #endregion

        #region Diamond
        /// <summary>
        /// [Diamond] How many milliseconds to wait before making the next write?
        /// </summary>
        public int DiamondDelay { get; set; } = 500;
        #endregion

        #region HueBack
        /// <summary>
        /// [HueBack] How many milliseconds to wait before making the next write?
        /// </summary>
        public int HueBackDelay { get; set; } = 50;
        /// <summary>
        /// [HueBack] How intense is the color?
        /// </summary>
        public int HueBackSaturation { get; set; } = 100;
        /// <summary>
        /// [HueBack] How light is the color?
        /// </summary>
        public int HueBackLuminance { get; set; } = 50;
        #endregion

        #region HueBackGradient
        /// <summary>
        /// [HueBackGradient] How many milliseconds to wait before making the next write?
        /// </summary>
        public int HueBackGradientDelay { get; set; } = 50;
        /// <summary>
        /// [HueBackGradient] How intense is the color?
        /// </summary>
        public int HueBackGradientSaturation { get; set; } = 100;
        /// <summary>
        /// [HueBackGradient] How light is the color?
        /// </summary>
        public int HueBackGradientLuminance { get; set; } = 50;
        #endregion

        #region Particles
        /// <summary>
        /// [Particles] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool ParticlesTrueColor { get; set; } = true;
        /// <summary>
        /// [Particles] How many milliseconds to wait before making the next write?
        /// </summary>
        public int ParticlesDelay { get; set; } = 1;
        /// <summary>
        /// [Particles] How dense are the particles?
        /// </summary>
        public int ParticlesDensity { get; set; } = 25;
        /// <summary>
        /// [Particles] The minimum red color level (true color)
        /// </summary>
        public int ParticlesMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [Particles] The minimum green color level (true color)
        /// </summary>
        public int ParticlesMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [Particles] The minimum blue color level (true color)
        /// </summary>
        public int ParticlesMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [Particles] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int ParticlesMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [Particles] The maximum red color level (true color)
        /// </summary>
        public int ParticlesMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [Particles] The maximum green color level (true color)
        /// </summary>
        public int ParticlesMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [Particles] The maximum blue color level (true color)
        /// </summary>
        public int ParticlesMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [Particles] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int ParticlesMaximumColorLevel { get; set; } = 255;
        #endregion

        #region WorldClock
        /// <summary>
        /// [WorldClock] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool WorldClockTrueColor { get; set; } = true;
        /// <summary>
        /// [WorldClock] How many milliseconds to wait before making the next write?
        /// </summary>
        public int WorldClockDelay { get; set; } = 1000;
        /// <summary>
        /// [WorldClock] How many refreshes before going to the next time zone?
        /// </summary>
        public int WorldClockNextZoneRefreshes { get; set; } = 5;
        /// <summary>
        /// [WorldClock] The minimum red color level (true color)
        /// </summary>
        public int WorldClockMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [WorldClock] The minimum green color level (true color)
        /// </summary>
        public int WorldClockMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [WorldClock] The minimum blue color level (true color)
        /// </summary>
        public int WorldClockMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [WorldClock] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int WorldClockMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [WorldClock] The maximum red color level (true color)
        /// </summary>
        public int WorldClockMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [WorldClock] The maximum green color level (true color)
        /// </summary>
        public int WorldClockMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [WorldClock] The maximum blue color level (true color)
        /// </summary>
        public int WorldClockMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [WorldClock] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int WorldClockMaximumColorLevel { get; set; } = 255;
        #endregion

        #region FillFade
        /// <summary>
        /// [FillFade] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool FillFadeTrueColor { get; set; } = true;
        /// <summary>
        /// [FillFade] The minimum red color level (true color)
        /// </summary>
        public int FillFadeMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [FillFade] The minimum green color level (true color)
        /// </summary>
        public int FillFadeMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [FillFade] The minimum blue color level (true color)
        /// </summary>
        public int FillFadeMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [FillFade] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int FillFadeMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [FillFade] The maximum red color level (true color)
        /// </summary>
        public int FillFadeMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [FillFade] The maximum green color level (true color)
        /// </summary>
        public int FillFadeMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [FillFade] The maximum blue color level (true color)
        /// </summary>
        public int FillFadeMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [FillFade] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int FillFadeMaximumColorLevel { get; set; } = 255;
        #endregion

        #region DanceLines
        /// <summary>
        /// [DanceLines] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool DanceLinesTrueColor { get; set; } = true;
        /// <summary>
        /// [DanceLines] How many milliseconds to wait before making the next write?
        /// </summary>
        public int DanceLinesDelay { get; set; } = 50;
        /// <summary>
        /// [DanceLines] Line character
        /// </summary>
        public string DanceLinesLineChar { get; set; } = "-";
        /// <summary>
        /// [DanceLines] Screensaver background color
        /// </summary>
        public string DanceLinesBackgroundColor { get; set; } = new Color(ConsoleColors.Black).PlainSequence;
        /// <summary>
        /// [DanceLines] The minimum red color level (true color)
        /// </summary>
        public int DanceLinesMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [DanceLines] The minimum green color level (true color)
        /// </summary>
        public int DanceLinesMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [DanceLines] The minimum blue color level (true color)
        /// </summary>
        public int DanceLinesMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [DanceLines] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int DanceLinesMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [DanceLines] The maximum red color level (true color)
        /// </summary>
        public int DanceLinesMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [DanceLines] The maximum green color level (true color)
        /// </summary>
        public int DanceLinesMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [DanceLines] The maximum blue color level (true color)
        /// </summary>
        public int DanceLinesMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [DanceLines] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int DanceLinesMaximumColorLevel { get; set; } = 255;
        #endregion

        #region Mazer
        /// <summary>
        /// [Mazer] How many milliseconds to wait before generating a new maze?
        /// </summary>
        public int MazerNewMazeDelay { get; set; } = 10000;
        /// <summary>
        /// [Mazer] Maze generation speed in milliseconds
        /// </summary>
        public int MazerGenerationSpeed { get; set; } = 20;
        /// <summary>
        /// [Mazer] If enabled, highlights the non-covered positions with the gray background color. Otherwise, they render as boxes.
        /// </summary>
        public bool MazerHighlightUncovered { get; set; } = false;
        /// <summary>
        /// [Mazer] Specifies whether to choose the <seealso href="http://en.wikipedia.org/wiki/Schwartzian_transform">Schwartzian transform</seealso> or to use <see cref="Random.Shuffle{T}(T[])"/>
        /// </summary>
        public bool MazerUseSchwartzian { get; set; } = true;
        #endregion

        #region TwoSpins
        /// <summary>
        /// [TwoSpins] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool TwoSpinsTrueColor { get; set; } = true;
        /// <summary>
        /// [TwoSpins] How many milliseconds to wait before making the next write?
        /// </summary>
        public int TwoSpinsDelay { get; set; } = 25;
        /// <summary>
        /// [TwoSpins] The minimum red color level (true color)
        /// </summary>
        public int TwoSpinsMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [TwoSpins] The minimum green color level (true color)
        /// </summary>
        public int TwoSpinsMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [TwoSpins] The minimum blue color level (true color)
        /// </summary>
        public int TwoSpinsMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [TwoSpins] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int TwoSpinsMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [TwoSpins] The maximum red color level (true color)
        /// </summary>
        public int TwoSpinsMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [TwoSpins] The maximum green color level (true color)
        /// </summary>
        public int TwoSpinsMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [TwoSpins] The maximum blue color level (true color)
        /// </summary>
        public int TwoSpinsMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [TwoSpins] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int TwoSpinsMaximumColorLevel { get; set; } = 255;
        #endregion

        #region CommitMilestone
        /// <summary>
        /// [CommitMilestone] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool CommitMilestoneTrueColor { get; set; } = true;
        /// <summary>
        /// [CommitMilestone] How many milliseconds to wait before making the next write?
        /// </summary>
        public int CommitMilestoneDelay { get; set; } = 1000;
        /// <summary>
        /// [CommitMilestone] Enables the rainbow colors mode
        /// </summary>
        public bool CommitMilestoneRainbowMode { get; set; }
        /// <summary>
        /// [CommitMilestone] The minimum red color level (true color)
        /// </summary>
        public int CommitMilestoneMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [CommitMilestone] The minimum green color level (true color)
        /// </summary>
        public int CommitMilestoneMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [CommitMilestone] The minimum blue color level (true color)
        /// </summary>
        public int CommitMilestoneMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [CommitMilestone] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int CommitMilestoneMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [CommitMilestone] The maximum red color level (true color)
        /// </summary>
        public int CommitMilestoneMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [CommitMilestone] The maximum green color level (true color)
        /// </summary>
        public int CommitMilestoneMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [CommitMilestone] The maximum blue color level (true color)
        /// </summary>
        public int CommitMilestoneMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [CommitMilestone] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int CommitMilestoneMaximumColorLevel { get; set; } = 255;
        #endregion

        #region Spray
        /// <summary>
        /// [Spray] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SprayDelay { get; set; } = 10;
        #endregion

        #region ZebraShift
        /// <summary>
        /// [ZebraShift] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool ZebraShiftTrueColor { get; set; } = true;
        /// <summary>
        /// [ZebraShift] How many milliseconds to wait before making the next write?
        /// </summary>
        public int ZebraShiftDelay { get; set; } = 25;
        /// <summary>
        /// [ZebraShift] The minimum red color level (true color)
        /// </summary>
        public int ZebraShiftMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [ZebraShift] The minimum green color level (true color)
        /// </summary>
        public int ZebraShiftMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [ZebraShift] The minimum blue color level (true color)
        /// </summary>
        public int ZebraShiftMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [ZebraShift] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int ZebraShiftMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [ZebraShift] The maximum red color level (true color)
        /// </summary>
        public int ZebraShiftMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [ZebraShift] The maximum green color level (true color)
        /// </summary>
        public int ZebraShiftMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [ZebraShift] The maximum blue color level (true color)
        /// </summary>
        public int ZebraShiftMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [ZebraShift] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int ZebraShiftMaximumColorLevel { get; set; } = 255;
        #endregion

        #region AnalogClock
        /// <summary>
        /// [AnalogClock] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool AnalogClockTrueColor { get; set; } = true;
        /// <summary>
        /// [AnalogClock] How many milliseconds to wait before making the next write?
        /// </summary>
        public int AnalogClockDelay { get; set; } = 1000;
        /// <summary>
        /// [AnalogClock] Shows the seconds hand.
        /// </summary>
        public bool AnalogClockShowSecondsHand { get; set; } = true;
        /// <summary>
        /// [AnalogClock] The minimum red color level (true color)
        /// </summary>
        public int AnalogClockMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [AnalogClock] The minimum green color level (true color)
        /// </summary>
        public int AnalogClockMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [AnalogClock] The minimum blue color level (true color)
        /// </summary>
        public int AnalogClockMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [AnalogClock] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int AnalogClockMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [AnalogClock] The maximum red color level (true color)
        /// </summary>
        public int AnalogClockMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [AnalogClock] The maximum green color level (true color)
        /// </summary>
        public int AnalogClockMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [AnalogClock] The maximum blue color level (true color)
        /// </summary>
        public int AnalogClockMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [AnalogClock] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int AnalogClockMaximumColorLevel { get; set; } = 255;
        #endregion
    }
}
