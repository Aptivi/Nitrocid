//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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
using Terminaux.Colors.Themes;
using Nitrocid.Base.Kernel;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Configuration.Instances;
using Nitrocid.Base.Kernel.Configuration.Settings;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Reflection.Internal;
using Nitrocid.ScreensaverPacks.Screensavers;
using System;
using System.Runtime.Versioning;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Textify.Data.Figlet;

namespace Nitrocid.ScreensaverPacks.Settings
{
    /// <summary>
    /// Screensaver kernel configuration instance
    /// </summary>
    public class ExtraSaversConfig : BaseKernelConfig
    {
        /// <inheritdoc/>
        [JsonIgnore]
        public override SettingsEntry[] SettingsEntries
        {
            get
            {
                var dataStream = ResourcesManager.GetData("AddonSaverSettings.json", ResourcesType.Misc, typeof(ExtraSaversConfig).Assembly) ??
                    throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_SCREENSAVERPACKS_COMMON_SETTINGS_EXCEPTION_ENTRIESFAILED"));
                string dataString = ResourcesManager.ConvertToString(dataStream);
                return ConfigTools.GetSettingsEntries(dataString);
            }
        }

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
        private char progressClockUpperLeftCornerCharHours = '╭';
        private char progressClockUpperLeftCornerCharMinutes = '╭';
        private char progressClockUpperLeftCornerCharSeconds = '╭';
        private char progressClockUpperRightCornerCharHours = '╮';
        private char progressClockUpperRightCornerCharMinutes = '╮';
        private char progressClockUpperRightCornerCharSeconds = '╮';
        private char progressClockLowerLeftCornerCharHours = '╰';
        private char progressClockLowerLeftCornerCharMinutes = '╰';
        private char progressClockLowerLeftCornerCharSeconds = '╰';
        private char progressClockLowerRightCornerCharHours = '╯';
        private char progressClockLowerRightCornerCharMinutes = '╯';
        private char progressClockLowerRightCornerCharSeconds = '╯';
        private char progressClockUpperFrameCharHours = '─';
        private char progressClockUpperFrameCharMinutes = '─';
        private char progressClockUpperFrameCharSeconds = '─';
        private char progressClockLowerFrameCharHours = '─';
        private char progressClockLowerFrameCharMinutes = '─';
        private char progressClockLowerFrameCharSeconds = '─';
        private char progressClockLeftFrameCharHours = '│';
        private char progressClockLeftFrameCharMinutes = '│';
        private char progressClockLeftFrameCharSeconds = '│';
        private char progressClockRightFrameCharHours = '│';
        private char progressClockRightFrameCharMinutes = '│';
        private char progressClockRightFrameCharSeconds = '│';
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
        private int typewriterDelay = 50;
        private int typewriterNewScreenDelay = 3000;
        private string typewriterWrite = "Nitrocid KS";
        private int typewriterWritingSpeedMin = 50;
        private int typewriterWritingSpeedMax = 80;
        private bool typewriterShowArrowPos = true;
        private string typewriterTextColor = new Color(ConsoleColors.White).PlainSequence;

        /// <summary>
        /// [Typewriter] How many milliseconds to wait before making the next write?
        /// </summary>
        public int TypewriterDelay
        {
            get
            {
                return typewriterDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                typewriterDelay = value;
            }
        }
        /// <summary>
        /// [Typewriter] How many milliseconds to wait before writing the text in the new screen again?
        /// </summary>
        public int TypewriterNewScreenDelay
        {
            get
            {
                return typewriterNewScreenDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                typewriterNewScreenDelay = value;
            }
        }
        /// <summary>
        /// [Typewriter] Text for Typewriter. Longer is better.
        /// </summary>
        public string TypewriterWrite
        {
            get
            {
                return typewriterWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                typewriterWrite = value;
            }
        }
        /// <summary>
        /// [Typewriter] Minimum writing speed in WPM
        /// </summary>
        public int TypewriterWritingSpeedMin
        {
            get
            {
                return typewriterWritingSpeedMin;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                typewriterWritingSpeedMin = value;
            }
        }
        /// <summary>
        /// [Typewriter] Maximum writing speed in WPM
        /// </summary>
        public int TypewriterWritingSpeedMax
        {
            get
            {
                return typewriterWritingSpeedMax;
            }
            set
            {
                if (value <= 0)
                    value = 80;
                typewriterWritingSpeedMax = value;
            }
        }
        /// <summary>
        /// [Typewriter] Shows the typewriter letter column position by showing this key on the bottom of the screen: <code>^</code>
        /// </summary>
        public bool TypewriterShowArrowPos
        {
            get
            {
                return typewriterShowArrowPos;
            }
            set
            {
                typewriterShowArrowPos = value;
            }
        }
        /// <summary>
        /// [Typewriter] Text color
        /// </summary>
        public string TypewriterTextColor
        {
            get
            {
                return typewriterTextColor;
            }
            set
            {
                typewriterTextColor = new Color(value).PlainSequence;
            }
        }
        #endregion

        #region FlashColor
        private bool flashColorTrueColor = true;
        private int flashColorDelay = 20;
        private string flashColorBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private int flashColorMinimumRedColorLevel = 0;
        private int flashColorMinimumGreenColorLevel = 0;
        private int flashColorMinimumBlueColorLevel = 0;
        private int flashColorMinimumColorLevel = 0;
        private int flashColorMaximumRedColorLevel = 255;
        private int flashColorMaximumGreenColorLevel = 255;
        private int flashColorMaximumBlueColorLevel = 255;
        private int flashColorMaximumColorLevel = 255;

        /// <summary>
        /// [FlashColor] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool FlashColorTrueColor
        {
            get
            {
                return flashColorTrueColor;
            }
            set
            {
                flashColorTrueColor = value;
            }
        }
        /// <summary>
        /// [FlashColor] How many milliseconds to wait before making the next write?
        /// </summary>
        public int FlashColorDelay
        {
            get
            {
                return flashColorDelay;
            }
            set
            {
                if (value <= 0)
                    value = 20;
                flashColorDelay = value;
            }
        }
        /// <summary>
        /// [FlashColor] Screensaver background color
        /// </summary>
        public string FlashColorBackgroundColor
        {
            get
            {
                return flashColorBackgroundColor;
            }
            set
            {
                flashColorBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [FlashColor] The minimum red color level (true color)
        /// </summary>
        public int FlashColorMinimumRedColorLevel
        {
            get
            {
                return flashColorMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                flashColorMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The minimum green color level (true color)
        /// </summary>
        public int FlashColorMinimumGreenColorLevel
        {
            get
            {
                return flashColorMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                flashColorMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The minimum blue color level (true color)
        /// </summary>
        public int FlashColorMinimumBlueColorLevel
        {
            get
            {
                return flashColorMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                flashColorMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int FlashColorMinimumColorLevel
        {
            get
            {
                return flashColorMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                flashColorMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The maximum red color level (true color)
        /// </summary>
        public int FlashColorMaximumRedColorLevel
        {
            get
            {
                return flashColorMaximumRedColorLevel;
            }
            set
            {
                if (value <= flashColorMinimumRedColorLevel)
                    value = flashColorMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                flashColorMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The maximum green color level (true color)
        /// </summary>
        public int FlashColorMaximumGreenColorLevel
        {
            get
            {
                return flashColorMaximumGreenColorLevel;
            }
            set
            {
                if (value <= flashColorMinimumGreenColorLevel)
                    value = flashColorMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                flashColorMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The maximum blue color level (true color)
        /// </summary>
        public int FlashColorMaximumBlueColorLevel
        {
            get
            {
                return flashColorMaximumBlueColorLevel;
            }
            set
            {
                if (value <= flashColorMinimumBlueColorLevel)
                    value = flashColorMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                flashColorMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int FlashColorMaximumColorLevel
        {
            get
            {
                return flashColorMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= flashColorMinimumColorLevel)
                    value = flashColorMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                flashColorMaximumColorLevel = value;
            }
        }
        #endregion

        #region SpotWrite
        private int spotWriteDelay = 100;
        private int spotWriteNewScreenDelay = 3000;
        private string spotWriteWrite = "Nitrocid KS";
        private string spotWriteTextColor = new Color(ConsoleColors.White).PlainSequence;

        /// <summary>
        /// [SpotWrite] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SpotWriteDelay
        {
            get
            {
                return spotWriteDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                spotWriteDelay = value;
            }
        }
        /// <summary>
        /// [SpotWrite] Text for SpotWrite. Longer is better.
        /// </summary>
        public string SpotWriteWrite
        {
            get
            {
                return spotWriteWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                spotWriteWrite = value;
            }
        }
        /// <summary>
        /// [SpotWrite] How many milliseconds to wait before writing the text in the new screen again?
        /// </summary>
        public int SpotWriteNewScreenDelay
        {
            get
            {
                return spotWriteNewScreenDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                spotWriteNewScreenDelay = value;
            }
        }
        /// <summary>
        /// [SpotWrite] Text color
        /// </summary>
        public string SpotWriteTextColor
        {
            get
            {
                return spotWriteTextColor;
            }
            set
            {
                spotWriteTextColor = new Color(value).PlainSequence;
            }
        }
        #endregion

        #region Ramp
        private bool rampTrueColor = true;
        private int rampDelay = 20;
        private int rampNextRampDelay = 250;
        private char rampUpperLeftCornerChar = '╭';
        private char rampUpperRightCornerChar = '╮';
        private char rampLowerLeftCornerChar = '╰';
        private char rampLowerRightCornerChar = '╯';
        private char rampUpperFrameChar = '─';
        private char rampLowerFrameChar = '─';
        private char rampLeftFrameChar = '│';
        private char rampRightFrameChar = '│';
        private int rampMinimumRedColorLevelStart = 0;
        private int rampMinimumGreenColorLevelStart = 0;
        private int rampMinimumBlueColorLevelStart = 0;
        private int rampMinimumColorLevelStart = 0;
        private int rampMaximumRedColorLevelStart = 255;
        private int rampMaximumGreenColorLevelStart = 255;
        private int rampMaximumBlueColorLevelStart = 255;
        private int rampMaximumColorLevelStart = 255;
        private int rampMinimumRedColorLevelEnd = 0;
        private int rampMinimumGreenColorLevelEnd = 0;
        private int rampMinimumBlueColorLevelEnd = 0;
        private int rampMinimumColorLevelEnd = 0;
        private int rampMaximumRedColorLevelEnd = 255;
        private int rampMaximumGreenColorLevelEnd = 255;
        private int rampMaximumBlueColorLevelEnd = 255;
        private int rampMaximumColorLevelEnd = 255;
        private string rampUpperLeftCornerColor = "7";
        private string rampUpperRightCornerColor = "7";
        private string rampLowerLeftCornerColor = "7";
        private string rampLowerRightCornerColor = "7";
        private string rampUpperFrameColor = "7";
        private string rampLowerFrameColor = "7";
        private string rampLeftFrameColor = "7";
        private string rampRightFrameColor = "7";
        private bool rampUseBorderColors;

        /// <summary>
        /// [Ramp] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool RampTrueColor
        {
            get => rampTrueColor;
            set => rampTrueColor = value;
        }
        /// <summary>
        /// [Ramp] How many milliseconds to wait before making the next write?
        /// </summary>
        public int RampDelay
        {
            get => rampDelay;
            set
            {
                if (value <= 0)
                    value = 20;
                rampDelay = value;
            }
        }
        /// <summary>
        /// [Ramp] How many milliseconds to wait before starting the next ramp?
        /// </summary>
        public int RampNextRampDelay
        {
            get => rampNextRampDelay;
            set
            {
                if (value <= 0)
                    value = 250;
                rampNextRampDelay = value;
            }
        }
        /// <summary>
        /// [Ramp] Upper left corner character 
        /// </summary>
        public char RampUpperLeftCornerChar
        {
            get => rampUpperLeftCornerChar;
            set => rampUpperLeftCornerChar = value;
        }
        /// <summary>
        /// [Ramp] Upper right corner character 
        /// </summary>
        public char RampUpperRightCornerChar
        {
            get => rampUpperRightCornerChar;
            set => rampUpperRightCornerChar = value;
        }
        /// <summary>
        /// [Ramp] Lower left corner character 
        /// </summary>
        public char RampLowerLeftCornerChar
        {
            get => rampLowerLeftCornerChar;
            set => rampLowerLeftCornerChar = value;
        }
        /// <summary>
        /// [Ramp] Lower right corner character 
        /// </summary>
        public char RampLowerRightCornerChar
        {
            get => rampLowerRightCornerChar;
            set => rampLowerRightCornerChar = value;
        }
        /// <summary>
        /// [Ramp] Upper frame character 
        /// </summary>
        public char RampUpperFrameChar
        {
            get => rampUpperFrameChar;
            set => rampUpperFrameChar = value;
        }
        /// <summary>
        /// [Ramp] Lower frame character 
        /// </summary>
        public char RampLowerFrameChar
        {
            get => rampLowerFrameChar;
            set => rampLowerFrameChar = value;
        }
        /// <summary>
        /// [Ramp] Left frame character 
        /// </summary>
        public char RampLeftFrameChar
        {
            get => rampLeftFrameChar;
            set => rampLeftFrameChar = value;
        }
        /// <summary>
        /// [Ramp] Right frame character 
        /// </summary>
        public char RampRightFrameChar
        {
            get => rampRightFrameChar;
            set => rampRightFrameChar = value;
        }
        /// <summary>
        /// [Ramp] The minimum red color level (true color - start)
        /// </summary>
        public int RampMinimumRedColorLevelStart
        {
            get => rampMinimumRedColorLevelStart;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                rampMinimumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum green color level (true color - start)
        /// </summary>
        public int RampMinimumGreenColorLevelStart
        {
            get => rampMinimumGreenColorLevelStart;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                rampMinimumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum blue color level (true color - start)
        /// </summary>
        public int RampMinimumBlueColorLevelStart
        {
            get => rampMinimumBlueColorLevelStart;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                rampMinimumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum color level (255 colors or 16 colors - start)
        /// </summary>
        public int RampMinimumColorLevelStart
        {
            get => rampMinimumColorLevelStart;
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                rampMinimumColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum red color level (true color - start)
        /// </summary>
        public int RampMaximumRedColorLevelStart
        {
            get => rampMaximumRedColorLevelStart;
            set
            {
                if (value <= rampMinimumRedColorLevelStart)
                    value = rampMinimumRedColorLevelStart;
                if (value > 255)
                    value = 255;
                rampMaximumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum green color level (true color - start)
        /// </summary>
        public int RampMaximumGreenColorLevelStart
        {
            get => rampMaximumGreenColorLevelStart;
            set
            {
                if (value <= rampMinimumGreenColorLevelStart)
                    value = rampMinimumGreenColorLevelStart;
                if (value > 255)
                    value = 255;
                rampMaximumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum blue color level (true color - start)
        /// </summary>
        public int RampMaximumBlueColorLevelStart
        {
            get => rampMaximumBlueColorLevelStart;
            set
            {
                if (value <= rampMinimumBlueColorLevelStart)
                    value = rampMinimumBlueColorLevelStart;
                if (value > 255)
                    value = 255;
                rampMaximumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum color level (255 colors or 16 colors - start)
        /// </summary>
        public int RampMaximumColorLevelStart
        {
            get => rampMaximumColorLevelStart;
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= rampMinimumColorLevelStart)
                    value = rampMinimumColorLevelStart;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                rampMaximumColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum red color level (true color - end)
        /// </summary>
        public int RampMinimumRedColorLevelEnd
        {
            get => rampMinimumRedColorLevelEnd;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                rampMinimumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum green color level (true color - end)
        /// </summary>
        public int RampMinimumGreenColorLevelEnd
        {
            get => rampMinimumGreenColorLevelEnd;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                rampMinimumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum blue color level (true color - end)
        /// </summary>
        public int RampMinimumBlueColorLevelEnd
        {
            get => rampMinimumBlueColorLevelEnd;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                rampMinimumBlueColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum color level (255 colors or 16 colors - end)
        /// </summary>
        public int RampMinimumColorLevelEnd
        {
            get => rampMinimumColorLevelEnd;
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                rampMinimumColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum red color level (true color - end)
        /// </summary>
        public int RampMaximumRedColorLevelEnd
        {
            get => rampMaximumRedColorLevelEnd;
            set
            {
                if (value <= rampMinimumRedColorLevelEnd)
                    value = rampMinimumRedColorLevelEnd;
                if (value > 255)
                    value = 255;
                rampMaximumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum green color level (true color - end)
        /// </summary>
        public int RampMaximumGreenColorLevelEnd
        {
            get => rampMaximumGreenColorLevelEnd;
            set
            {
                if (value <= rampMinimumGreenColorLevelEnd)
                    value = rampMinimumGreenColorLevelEnd;
                if (value > 255)
                    value = 255;
                rampMaximumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum blue color level (true color - end)
        /// </summary>
        public int RampMaximumBlueColorLevelEnd
        {
            get => rampMaximumBlueColorLevelEnd;
            set
            {
                if (value <= rampMinimumBlueColorLevelEnd)
                    value = rampMinimumBlueColorLevelEnd;
                if (value > 255)
                    value = 255;
                rampMaximumBlueColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum color level (255 colors or 16 colors - end)
        /// </summary>
        public int RampMaximumColorLevelEnd
        {
            get => rampMaximumColorLevelEnd;
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= rampMinimumColorLevelEnd)
                    value = rampMinimumColorLevelEnd;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                rampMaximumColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] Upper left corner color.
        /// </summary>
        public string RampUpperLeftCornerColor
        {
            get => rampUpperLeftCornerColor;
            set => rampUpperLeftCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Ramp] Upper right corner color.
        /// </summary>
        public string RampUpperRightCornerColor
        {
            get => rampUpperRightCornerColor;
            set => rampUpperRightCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Ramp] Lower left corner color.
        /// </summary>
        public string RampLowerLeftCornerColor
        {
            get => rampLowerLeftCornerColor;
            set => rampLowerLeftCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Ramp] Lower right corner color.
        /// </summary>
        public string RampLowerRightCornerColor
        {
            get => rampLowerRightCornerColor;
            set => rampLowerRightCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Ramp] Upper frame color.
        /// </summary>
        public string RampUpperFrameColor
        {
            get => rampUpperFrameColor;
            set => rampUpperFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Ramp] Lower frame color.
        /// </summary>
        public string RampLowerFrameColor
        {
            get => rampLowerFrameColor;
            set => rampLowerFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Ramp] Left frame color.
        /// </summary>
        public string RampLeftFrameColor
        {
            get => rampLeftFrameColor;
            set => rampLeftFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Ramp] Right frame color.
        /// </summary>
        public string RampRightFrameColor
        {
            get => rampRightFrameColor;
            set => rampRightFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Ramp] Use the border colors.
        /// </summary>
        public bool RampUseBorderColors
        {
            get => rampUseBorderColors;
            set => rampUseBorderColors = value;
        }
        #endregion

        #region StackBox
        private bool stackBoxTrueColor = true;
        private int stackBoxDelay = 10;
        private int stackBoxMinimumRedColorLevel = 0;
        private int stackBoxMinimumGreenColorLevel = 0;
        private int stackBoxMinimumBlueColorLevel = 0;
        private int stackBoxMinimumColorLevel = 0;
        private int stackBoxMaximumRedColorLevel = 255;
        private int stackBoxMaximumGreenColorLevel = 255;
        private int stackBoxMaximumBlueColorLevel = 255;
        private int stackBoxMaximumColorLevel = 255;
        private bool stackBoxFill = true;

        /// <summary>
        /// [StackBox] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool StackBoxTrueColor
        {
            get
            {
                return stackBoxTrueColor;
            }
            set
            {
                stackBoxTrueColor = value;
            }
        }
        /// <summary>
        /// [StackBox] How many milliseconds to wait before making the next write?
        /// </summary>
        public int StackBoxDelay
        {
            get
            {
                return stackBoxDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                stackBoxDelay = value;
            }
        }
        /// <summary>
        /// [StackBox] Whether to fill in the boxes drawn, or only draw the outline
        /// </summary>
        public bool StackBoxFill
        {
            get
            {
                return stackBoxFill;
            }
            set
            {
                stackBoxFill = value;
            }
        }
        /// <summary>
        /// [StackBox] The minimum red color level (true color)
        /// </summary>
        public int StackBoxMinimumRedColorLevel
        {
            get
            {
                return stackBoxMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                stackBoxMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [StackBox] The minimum green color level (true color)
        /// </summary>
        public int StackBoxMinimumGreenColorLevel
        {
            get
            {
                return stackBoxMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                stackBoxMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [StackBox] The minimum blue color level (true color)
        /// </summary>
        public int StackBoxMinimumBlueColorLevel
        {
            get
            {
                return stackBoxMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                stackBoxMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [StackBox] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int StackBoxMinimumColorLevel
        {
            get
            {
                return stackBoxMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                stackBoxMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [StackBox] The maximum red color level (true color)
        /// </summary>
        public int StackBoxMaximumRedColorLevel
        {
            get
            {
                return stackBoxMaximumRedColorLevel;
            }
            set
            {
                if (value <= stackBoxMinimumRedColorLevel)
                    value = stackBoxMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                stackBoxMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [StackBox] The maximum green color level (true color)
        /// </summary>
        public int StackBoxMaximumGreenColorLevel
        {
            get
            {
                return stackBoxMaximumGreenColorLevel;
            }
            set
            {
                if (value <= stackBoxMinimumGreenColorLevel)
                    value = stackBoxMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                stackBoxMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [StackBox] The maximum blue color level (true color)
        /// </summary>
        public int StackBoxMaximumBlueColorLevel
        {
            get
            {
                return stackBoxMaximumBlueColorLevel;
            }
            set
            {
                if (value <= stackBoxMinimumBlueColorLevel)
                    value = stackBoxMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                stackBoxMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [StackBox] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int StackBoxMaximumColorLevel
        {
            get
            {
                return stackBoxMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= stackBoxMinimumColorLevel)
                    value = stackBoxMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                stackBoxMaximumColorLevel = value;
            }
        }
        #endregion

        #region Snaker
        private bool snakerTrueColor = true;
        private int snakerDelay = 100;
        private int snakerStageDelay = 5000;
        private int snakerMinimumRedColorLevel = 0;
        private int snakerMinimumGreenColorLevel = 0;
        private int snakerMinimumBlueColorLevel = 0;
        private int snakerMinimumColorLevel = 0;
        private int snakerMaximumRedColorLevel = 255;
        private int snakerMaximumGreenColorLevel = 255;
        private int snakerMaximumBlueColorLevel = 255;
        private int snakerMaximumColorLevel = 255;

        /// <summary>
        /// [Snaker] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool SnakerTrueColor
        {
            get
            {
                return snakerTrueColor;
            }
            set
            {
                snakerTrueColor = value;
            }
        }
        /// <summary>
        /// [Snaker] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SnakerDelay
        {
            get
            {
                return snakerDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                snakerDelay = value;
            }
        }
        /// <summary>
        /// [Snaker] How many milliseconds to wait before making the next stage?
        /// </summary>
        public int SnakerStageDelay
        {
            get
            {
                return snakerStageDelay;
            }
            set
            {
                if (value <= 0)
                    value = 5000;
                snakerStageDelay = value;
            }
        }
        /// <summary>
        /// [Snaker] The minimum red color level (true color)
        /// </summary>
        public int SnakerMinimumRedColorLevel
        {
            get
            {
                return snakerMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                snakerMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Snaker] The minimum green color level (true color)
        /// </summary>
        public int SnakerMinimumGreenColorLevel
        {
            get
            {
                return snakerMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                snakerMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Snaker] The minimum blue color level (true color)
        /// </summary>
        public int SnakerMinimumBlueColorLevel
        {
            get
            {
                return snakerMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                snakerMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Snaker] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int SnakerMinimumColorLevel
        {
            get
            {
                return snakerMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                snakerMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Snaker] The maximum red color level (true color)
        /// </summary>
        public int SnakerMaximumRedColorLevel
        {
            get
            {
                return snakerMaximumRedColorLevel;
            }
            set
            {
                if (value <= snakerMinimumRedColorLevel)
                    value = snakerMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                snakerMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Snaker] The maximum green color level (true color)
        /// </summary>
        public int SnakerMaximumGreenColorLevel
        {
            get
            {
                return snakerMaximumGreenColorLevel;
            }
            set
            {
                if (value <= snakerMinimumGreenColorLevel)
                    value = snakerMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                snakerMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Snaker] The maximum blue color level (true color)
        /// </summary>
        public int SnakerMaximumBlueColorLevel
        {
            get
            {
                return snakerMaximumBlueColorLevel;
            }
            set
            {
                if (value <= snakerMinimumBlueColorLevel)
                    value = snakerMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                snakerMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Snaker] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int SnakerMaximumColorLevel
        {
            get
            {
                return snakerMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= snakerMinimumColorLevel)
                    value = snakerMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                snakerMaximumColorLevel = value;
            }
        }
        #endregion

        #region BarRot
        private bool barRotTrueColor = true;
        private bool barRotUseBorderColors;
        private string barRotRightFrameColor = "192;192;192";
        private int barRotDelay = 10;
        private int barRotNextRampDelay = 250;
        private char barRotUpperLeftCornerChar = '╭';
        private char barRotUpperRightCornerChar = '╮';
        private char barRotLowerLeftCornerChar = '╰';
        private char barRotLowerRightCornerChar = '╯';
        private char barRotUpperFrameChar = '─';
        private char barRotLowerFrameChar = '─';
        private char barRotLeftFrameChar = '│';
        private char barRotRightFrameChar = '│';
        private int barRotMinimumRedColorLevelStart = 0;
        private int barRotMinimumGreenColorLevelStart = 0;
        private int barRotMinimumBlueColorLevelStart = 0;
        private int barRotMaximumRedColorLevelStart = 255;
        private int barRotMaximumGreenColorLevelStart = 255;
        private int barRotMaximumBlueColorLevelStart = 255;
        private int barRotMinimumRedColorLevelEnd = 0;
        private int barRotMinimumGreenColorLevelEnd = 0;
        private int barRotMinimumBlueColorLevelEnd = 0;
        private int barRotMaximumRedColorLevelEnd = 255;
        private int barRotMaximumGreenColorLevelEnd = 255;
        private int barRotMaximumBlueColorLevelEnd = 255;
        private string barRotUpperLeftCornerColor = "192;192;192";
        private string barRotUpperRightCornerColor = "192;192;192";
        private string barRotLowerLeftCornerColor = "192;192;192";
        private string barRotLowerRightCornerColor = "192;192;192";
        private string barRotUpperFrameColor = "192;192;192";
        private string barRotLowerFrameColor = "192;192;192";
        private string barRotLeftFrameColor = "192;192;192";

        /// <summary>
        /// [BarRot] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool BarRotTrueColor
        {
            get => barRotTrueColor;
            set => barRotTrueColor = value;
        }
        /// <summary>
        /// [BarRot] How many milliseconds to wait before making the next write?
        /// </summary>
        public int BarRotDelay
        {
            get => barRotDelay;
            set
            {
                if (value <= 0)
                    value = 10;
                barRotDelay = value;
            }
        }
        /// <summary>
        /// [BarRot] How many milliseconds to wait before rotting the next ramp's one end?
        /// </summary>
        public int BarRotNextRampDelay
        {
            get => barRotNextRampDelay;
            set
            {
                if (value <= 0)
                    value = 250;
                barRotNextRampDelay = value;
            }
        }
        /// <summary>
        /// [BarRot] Upper left corner character 
        /// </summary>
        public char BarRotUpperLeftCornerChar
        {
            get => barRotUpperLeftCornerChar;
            set => barRotUpperLeftCornerChar = value;
        }
        /// <summary>
        /// [BarRot] Upper right corner character 
        /// </summary>
        public char BarRotUpperRightCornerChar
        {
            get => barRotUpperRightCornerChar;
            set => barRotUpperRightCornerChar = value;
        }
        /// <summary>
        /// [BarRot] Lower left corner character 
        /// </summary>
        public char BarRotLowerLeftCornerChar
        {
            get => barRotLowerLeftCornerChar;
            set => barRotLowerLeftCornerChar = value;
        }
        /// <summary>
        /// [BarRot] Lower right corner character 
        /// </summary>
        public char BarRotLowerRightCornerChar
        {
            get => barRotLowerRightCornerChar;
            set => barRotLowerRightCornerChar = value;
        }
        /// <summary>
        /// [BarRot] Upper frame character 
        /// </summary>
        public char BarRotUpperFrameChar
        {
            get => barRotUpperFrameChar;
            set => barRotUpperFrameChar = value;
        }
        /// <summary>
        /// [BarRot] Lower frame character 
        /// </summary>
        public char BarRotLowerFrameChar
        {
            get => barRotLowerFrameChar;
            set => barRotLowerFrameChar = value;
        }
        /// <summary>
        /// [BarRot] Left frame character 
        /// </summary>
        public char BarRotLeftFrameChar
        {
            get => barRotLeftFrameChar;
            set => barRotLeftFrameChar = value;
        }
        /// <summary>
        /// [BarRot] Right frame character 
        /// </summary>
        public char BarRotRightFrameChar
        {
            get => barRotRightFrameChar;
            set => barRotRightFrameChar = value;
        }
        /// <summary>
        /// [BarRot] The minimum red color level (true color - start)
        /// </summary>
        public int BarRotMinimumRedColorLevelStart
        {
            get => barRotMinimumRedColorLevelStart;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                barRotMinimumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [BarRot] The minimum green color level (true color - start)
        /// </summary>
        public int BarRotMinimumGreenColorLevelStart
        {
            get => barRotMinimumGreenColorLevelStart;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                barRotMinimumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [BarRot] The minimum blue color level (true color - start)
        /// </summary>
        public int BarRotMinimumBlueColorLevelStart
        {
            get => barRotMinimumBlueColorLevelStart;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                barRotMinimumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [BarRot] The maximum red color level (true color - start)
        /// </summary>
        public int BarRotMaximumRedColorLevelStart
        {
            get => barRotMaximumRedColorLevelStart;
            set
            {
                if (value <= barRotMinimumRedColorLevelStart)
                    value = barRotMinimumRedColorLevelStart;
                if (value > 255)
                    value = 255;
                barRotMaximumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [BarRot] The maximum green color level (true color - start)
        /// </summary>
        public int BarRotMaximumGreenColorLevelStart
        {
            get => barRotMaximumGreenColorLevelStart;
            set
            {
                if (value <= barRotMinimumGreenColorLevelStart)
                    value = barRotMinimumGreenColorLevelStart;
                if (value > 255)
                    value = 255;
                barRotMaximumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [BarRot] The maximum blue color level (true color - start)
        /// </summary>
        public int BarRotMaximumBlueColorLevelStart
        {
            get => barRotMaximumBlueColorLevelStart;
            set
            {
                if (value <= barRotMinimumBlueColorLevelStart)
                    value = barRotMinimumBlueColorLevelStart;
                if (value > 255)
                    value = 255;
                barRotMaximumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [BarRot] The minimum red color level (true color - end)
        /// </summary>
        public int BarRotMinimumRedColorLevelEnd
        {
            get => barRotMinimumRedColorLevelEnd;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                barRotMinimumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [BarRot] The minimum green color level (true color - end)
        /// </summary>
        public int BarRotMinimumGreenColorLevelEnd
        {
            get => barRotMinimumGreenColorLevelEnd;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                barRotMinimumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [BarRot] The minimum blue color level (true color - end)
        /// </summary>
        public int BarRotMinimumBlueColorLevelEnd
        {
            get => barRotMinimumBlueColorLevelEnd;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                barRotMinimumBlueColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [BarRot] The maximum red color level (true color - end)
        /// </summary>
        public int BarRotMaximumRedColorLevelEnd
        {
            get => barRotMaximumRedColorLevelEnd;
            set
            {
                if (value <= barRotMinimumRedColorLevelEnd)
                    value = barRotMinimumRedColorLevelEnd;
                if (value > 255)
                    value = 255;
                barRotMaximumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [BarRot] The maximum green color level (true color - end)
        /// </summary>
        public int BarRotMaximumGreenColorLevelEnd
        {
            get => barRotMaximumGreenColorLevelEnd;
            set
            {
                if (value <= barRotMinimumGreenColorLevelEnd)
                    value = barRotMinimumGreenColorLevelEnd;
                if (value > 255)
                    value = 255;
                barRotMaximumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [BarRot] The maximum blue color level (true color - end)
        /// </summary>
        public int BarRotMaximumBlueColorLevelEnd
        {
            get => barRotMaximumBlueColorLevelEnd;
            set
            {
                if (value <= barRotMinimumBlueColorLevelEnd)
                    value = barRotMinimumBlueColorLevelEnd;
                if (value > 255)
                    value = 255;
                barRotMaximumBlueColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [BarRot] Upper left corner color.
        /// </summary>
        public string BarRotUpperLeftCornerColor
        {
            get => barRotUpperLeftCornerColor;
            set => barRotUpperLeftCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [BarRot] Upper right corner color.
        /// </summary>
        public string BarRotUpperRightCornerColor
        {
            get => barRotUpperRightCornerColor;
            set => barRotUpperRightCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [BarRot] Lower left corner color.
        /// </summary>
        public string BarRotLowerLeftCornerColor
        {
            get => barRotLowerLeftCornerColor;
            set => barRotLowerLeftCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [BarRot] Lower right corner color.
        /// </summary>
        public string BarRotLowerRightCornerColor
        {
            get => barRotLowerRightCornerColor;
            set => barRotLowerRightCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [BarRot] Upper frame color.
        /// </summary>
        public string BarRotUpperFrameColor
        {
            get => barRotUpperFrameColor;
            set => barRotUpperFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [BarRot] Lower frame color.
        /// </summary>
        public string BarRotLowerFrameColor
        {
            get => barRotLowerFrameColor;
            set => barRotLowerFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [BarRot] Left frame color.
        /// </summary>
        public string BarRotLeftFrameColor
        {
            get => barRotLeftFrameColor;
            set => barRotLeftFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [BarRot] Right frame color.
        /// </summary>
        public string BarRotRightFrameColor
        {
            get => barRotRightFrameColor;
            set => barRotRightFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [BarRot] Use the border colors.
        /// </summary>
        public bool BarRotUseBorderColors
        {
            get => barRotUseBorderColors;
            set => barRotUseBorderColors = value;
        }
        #endregion

        #region Fireworks
        private bool fireworksTrueColor = true;
        private int fireworksDelay = 50;
        private int fireworksRadius = 5;
        private int fireworksMinimumRedColorLevel = 0;
        private int fireworksMinimumGreenColorLevel = 0;
        private int fireworksMinimumBlueColorLevel = 0;
        private int fireworksMinimumColorLevel = 0;
        private int fireworksMaximumRedColorLevel = 255;
        private int fireworksMaximumGreenColorLevel = 255;
        private int fireworksMaximumBlueColorLevel = 255;
        private int fireworksMaximumColorLevel = 255;

        /// <summary>
        /// [Fireworks] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool FireworksTrueColor
        {
            get
            {
                return fireworksTrueColor;
            }
            set
            {
                fireworksTrueColor = value;
            }
        }
        /// <summary>
        /// [Fireworks] How many milliseconds to wait before making the next write?
        /// </summary>
        public int FireworksDelay
        {
            get
            {
                return fireworksDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                fireworksDelay = value;
            }
        }
        /// <summary>
        /// [Fireworks] The radius of the explosion
        /// </summary>
        public int FireworksRadius
        {
            get
            {
                return fireworksRadius;
            }
            set
            {
                if (value <= 0)
                    value = 5;
                fireworksRadius = value;
            }
        }
        /// <summary>
        /// [Fireworks] The minimum red color level (true color)
        /// </summary>
        public int FireworksMinimumRedColorLevel
        {
            get
            {
                return fireworksMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                fireworksMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Fireworks] The minimum green color level (true color)
        /// </summary>
        public int FireworksMinimumGreenColorLevel
        {
            get
            {
                return fireworksMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                fireworksMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Fireworks] The minimum blue color level (true color)
        /// </summary>
        public int FireworksMinimumBlueColorLevel
        {
            get
            {
                return fireworksMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                fireworksMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Fireworks] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int FireworksMinimumColorLevel
        {
            get
            {
                return fireworksMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                fireworksMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Fireworks] The maximum red color level (true color)
        /// </summary>
        public int FireworksMaximumRedColorLevel
        {
            get
            {
                return fireworksMaximumRedColorLevel;
            }
            set
            {
                if (value <= fireworksMinimumRedColorLevel)
                    value = fireworksMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                fireworksMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Fireworks] The maximum green color level (true color)
        /// </summary>
        public int FireworksMaximumGreenColorLevel
        {
            get
            {
                return fireworksMaximumGreenColorLevel;
            }
            set
            {
                if (value <= fireworksMinimumGreenColorLevel)
                    value = fireworksMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                fireworksMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Fireworks] The maximum blue color level (true color)
        /// </summary>
        public int FireworksMaximumBlueColorLevel
        {
            get
            {
                return fireworksMaximumBlueColorLevel;
            }
            set
            {
                if (value <= fireworksMinimumBlueColorLevel)
                    value = fireworksMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                fireworksMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Fireworks] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int FireworksMaximumColorLevel
        {
            get
            {
                return fireworksMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= fireworksMinimumColorLevel)
                    value = fireworksMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                fireworksMaximumColorLevel = value;
            }
        }
        #endregion

        #region Figlet
        private bool figletTrueColor = true;
        private int figletDelay = 1000;
        private string figletText = "Nitrocid KS";
        private string figletFont = "small";
        private bool figletRainbowMode;
        private int figletMinimumRedColorLevel = 0;
        private int figletMinimumGreenColorLevel = 0;
        private int figletMinimumBlueColorLevel = 0;
        private int figletMinimumColorLevel = 0;
        private int figletMaximumRedColorLevel = 255;
        private int figletMaximumGreenColorLevel = 255;
        private int figletMaximumBlueColorLevel = 255;
        private int figletMaximumColorLevel = 255;

        /// <summary>
        /// [Figlet] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool FigletTrueColor
        {
            get
            {
                return figletTrueColor;
            }
            set
            {
                figletTrueColor = value;
            }
        }
        /// <summary>
        /// [Figlet] How many milliseconds to wait before making the next write?
        /// </summary>
        public int FigletDelay
        {
            get
            {
                return figletDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                figletDelay = value;
            }
        }
        /// <summary>
        /// [Figlet] Text for Figlet. Shorter is better.
        /// </summary>
        public string FigletText
        {
            get
            {
                return figletText;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                figletText = value;
            }
        }
        /// <summary>
        /// [Figlet] Figlet font supported by the figlet library used.
        /// </summary>
        public string FigletFont
        {
            get
            {
                return figletFont;
            }
            set
            {
                figletFont = FigletTools.GetFigletFonts().ContainsKey(value) ? value : "small";
            }
        }
        /// <summary>
        /// [Figlet] Enables the rainbow colors mode
        /// </summary>
        public bool FigletRainbowMode
        {
            get
            {
                return figletRainbowMode;
            }
            set
            {
                figletRainbowMode = value;
            }
        }
        /// <summary>
        /// [Figlet] The minimum red color level (true color)
        /// </summary>
        public int FigletMinimumRedColorLevel
        {
            get
            {
                return figletMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                figletMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Figlet] The minimum green color level (true color)
        /// </summary>
        public int FigletMinimumGreenColorLevel
        {
            get
            {
                return figletMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                figletMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Figlet] The minimum blue color level (true color)
        /// </summary>
        public int FigletMinimumBlueColorLevel
        {
            get
            {
                return figletMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                figletMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Figlet] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int FigletMinimumColorLevel
        {
            get
            {
                return figletMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                figletMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Figlet] The maximum red color level (true color)
        /// </summary>
        public int FigletMaximumRedColorLevel
        {
            get
            {
                return figletMaximumRedColorLevel;
            }
            set
            {
                if (value <= figletMinimumRedColorLevel)
                    value = figletMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                figletMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Figlet] The maximum green color level (true color)
        /// </summary>
        public int FigletMaximumGreenColorLevel
        {
            get
            {
                return figletMaximumGreenColorLevel;
            }
            set
            {
                if (value <= figletMinimumGreenColorLevel)
                    value = figletMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                figletMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Figlet] The maximum blue color level (true color)
        /// </summary>
        public int FigletMaximumBlueColorLevel
        {
            get
            {
                return figletMaximumBlueColorLevel;
            }
            set
            {
                if (value <= figletMinimumBlueColorLevel)
                    value = figletMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                figletMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Figlet] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int FigletMaximumColorLevel
        {
            get
            {
                return figletMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= figletMinimumColorLevel)
                    value = figletMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                figletMaximumColorLevel = value;
            }
        }
        #endregion

        #region FlashText
        private bool flashTextTrueColor = true;
        private int flashTextDelay = 100;
        private string flashTextWrite = "Nitrocid KS";
        private string flashTextBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private int flashTextMinimumRedColorLevel = 0;
        private int flashTextMinimumGreenColorLevel = 0;
        private int flashTextMinimumBlueColorLevel = 0;
        private int flashTextMinimumColorLevel = 0;
        private int flashTextMaximumRedColorLevel = 255;
        private int flashTextMaximumGreenColorLevel = 255;
        private int flashTextMaximumBlueColorLevel = 255;
        private int flashTextMaximumColorLevel = 255;

        /// <summary>
        /// [FlashText] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool FlashTextTrueColor
        {
            get
            {
                return flashTextTrueColor;
            }
            set
            {
                flashTextTrueColor = value;
            }
        }
        /// <summary>
        /// [FlashText] How many milliseconds to wait before making the next write?
        /// </summary>
        public int FlashTextDelay
        {
            get
            {
                return flashTextDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                flashTextDelay = value;
            }
        }
        /// <summary>
        /// [FlashText] Text for FlashText. Shorter is better.
        /// </summary>
        public string FlashTextWrite
        {
            get
            {
                return flashTextWrite;
            }
            set
            {
                flashTextWrite = value;
            }
        }
        /// <summary>
        /// [FlashText] Screensaver background color
        /// </summary>
        public string FlashTextBackgroundColor
        {
            get
            {
                return flashTextBackgroundColor;
            }
            set
            {
                flashTextBackgroundColor = value;
            }
        }
        /// <summary>
        /// [FlashText] The minimum red color level (true color)
        /// </summary>
        public int FlashTextMinimumRedColorLevel
        {
            get
            {
                return flashTextMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                flashTextMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashText] The minimum green color level (true color)
        /// </summary>
        public int FlashTextMinimumGreenColorLevel
        {
            get
            {
                return flashTextMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                flashTextMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashText] The minimum blue color level (true color)
        /// </summary>
        public int FlashTextMinimumBlueColorLevel
        {
            get
            {
                return flashTextMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                flashTextMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashText] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int FlashTextMinimumColorLevel
        {
            get
            {
                return flashTextMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                flashTextMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashText] The maximum red color level (true color)
        /// </summary>
        public int FlashTextMaximumRedColorLevel
        {
            get
            {
                return flashTextMaximumRedColorLevel;
            }
            set
            {
                if (value <= flashTextMinimumRedColorLevel)
                    value = flashTextMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                flashTextMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashText] The maximum green color level (true color)
        /// </summary>
        public int FlashTextMaximumGreenColorLevel
        {
            get
            {
                return flashTextMaximumGreenColorLevel;
            }
            set
            {
                if (value <= flashTextMinimumGreenColorLevel)
                    value = flashTextMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                flashTextMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashText] The maximum blue color level (true color)
        /// </summary>
        public int FlashTextMaximumBlueColorLevel
        {
            get
            {
                return flashTextMaximumBlueColorLevel;
            }
            set
            {
                if (value <= flashTextMinimumBlueColorLevel)
                    value = flashTextMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                flashTextMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashText] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int FlashTextMaximumColorLevel
        {
            get
            {
                return flashTextMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= flashTextMinimumColorLevel)
                    value = flashTextMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                flashTextMaximumColorLevel = value;
            }
        }
        #endregion

        #region Noise
        private int noiseNewScreenDelay = 5000;
        private int noiseDensity = 40;

        /// <summary>
        /// [Noise] How many milliseconds to wait before making the new screen?
        /// </summary>
        public int NoiseNewScreenDelay
        {
            get
            {
                return noiseNewScreenDelay;
            }
            set
            {
                if (value <= 0)
                    value = 5000;
                noiseNewScreenDelay = value;
            }
        }
        /// <summary>
        /// [Noise] The noise density in percent
        /// </summary>
        public int NoiseDensity
        {
            get
            {
                return noiseDensity;
            }
            set
            {
                if (value < 0)
                    value = 40;
                if (value > 100)
                    value = 40;
                noiseDensity = value;
            }
        }
        #endregion

        #region DateAndTime
        private bool dateAndTimeTrueColor = true;
        private int dateAndTimeDelay = 1000;
        private int dateAndTimeMinimumRedColorLevel = 0;
        private int dateAndTimeMinimumGreenColorLevel = 0;
        private int dateAndTimeMinimumBlueColorLevel = 0;
        private int dateAndTimeMinimumColorLevel = 0;
        private int dateAndTimeMaximumRedColorLevel = 255;
        private int dateAndTimeMaximumGreenColorLevel = 255;
        private int dateAndTimeMaximumBlueColorLevel = 255;
        private int dateAndTimeMaximumColorLevel = 255;

        /// <summary>
        /// [DateAndTime] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool DateAndTimeTrueColor
        {
            get
            {
                return dateAndTimeTrueColor;
            }
            set
            {
                dateAndTimeTrueColor = value;
            }
        }
        /// <summary>
        /// [DateAndTime] How many milliseconds to wait before making the next write?
        /// </summary>
        public int DateAndTimeDelay
        {
            get
            {
                return dateAndTimeDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                dateAndTimeDelay = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The minimum red color level (true color)
        /// </summary>
        public int DateAndTimeMinimumRedColorLevel
        {
            get
            {
                return dateAndTimeMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                dateAndTimeMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The minimum green color level (true color)
        /// </summary>
        public int DateAndTimeMinimumGreenColorLevel
        {
            get
            {
                return dateAndTimeMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                dateAndTimeMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The minimum blue color level (true color)
        /// </summary>
        public int DateAndTimeMinimumBlueColorLevel
        {
            get
            {
                return dateAndTimeMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                dateAndTimeMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int DateAndTimeMinimumColorLevel
        {
            get
            {
                return dateAndTimeMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                dateAndTimeMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The maximum red color level (true color)
        /// </summary>
        public int DateAndTimeMaximumRedColorLevel
        {
            get
            {
                return dateAndTimeMaximumRedColorLevel;
            }
            set
            {
                if (value <= dateAndTimeMinimumRedColorLevel)
                    value = dateAndTimeMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                dateAndTimeMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The maximum green color level (true color)
        /// </summary>
        public int DateAndTimeMaximumGreenColorLevel
        {
            get
            {
                return dateAndTimeMaximumGreenColorLevel;
            }
            set
            {
                if (value <= dateAndTimeMinimumGreenColorLevel)
                    value = dateAndTimeMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                dateAndTimeMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The maximum blue color level (true color)
        /// </summary>
        public int DateAndTimeMaximumBlueColorLevel
        {
            get
            {
                return dateAndTimeMaximumBlueColorLevel;
            }
            set
            {
                if (value <= dateAndTimeMinimumBlueColorLevel)
                    value = dateAndTimeMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                dateAndTimeMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int DateAndTimeMaximumColorLevel
        {
            get
            {
                return dateAndTimeMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= dateAndTimeMinimumColorLevel)
                    value = dateAndTimeMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                dateAndTimeMaximumColorLevel = value;
            }
        }
        #endregion

        #region Glitch
        private int glitchDelay = 10;
        private int glitchDensity = 40;

        /// <summary>
        /// [Glitch] How many milliseconds to wait before making the next write?
        /// </summary>
        public int GlitchDelay
        {
            get
            {
                return glitchDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                glitchDelay = value;
            }
        }
        /// <summary>
        /// [Glitch] The glitch density in percent
        /// </summary>
        public int GlitchDensity
        {
            get
            {
                return glitchDensity;
            }
            set
            {
                if (value < 0)
                    value = 40;
                if (value > 100)
                    value = 40;
                glitchDensity = value;
            }
        }
        #endregion

        #region FallingLine
        private bool fallingLineTrueColor = true;
        private int fallingLineDelay = 10;
        private int fallingLineMaxSteps = 25;
        private int fallingLineMinimumRedColorLevel = 0;
        private int fallingLineMinimumGreenColorLevel = 0;
        private int fallingLineMinimumBlueColorLevel = 0;
        private int fallingLineMinimumColorLevel = 0;
        private int fallingLineMaximumRedColorLevel = 255;
        private int fallingLineMaximumGreenColorLevel = 255;
        private int fallingLineMaximumBlueColorLevel = 255;
        private int fallingLineMaximumColorLevel = 255;

        /// <summary>
        /// [FallingLine] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool FallingLineTrueColor
        {
            get
            {
                return fallingLineTrueColor;
            }
            set
            {
                fallingLineTrueColor = value;
            }
        }
        /// <summary>
        /// [FallingLine] How many milliseconds to wait before making the next write?
        /// </summary>
        public int FallingLineDelay
        {
            get
            {
                return fallingLineDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                fallingLineDelay = value;
            }
        }
        /// <summary>
        /// [FallingLine] How many fade steps to do?
        /// </summary>
        public int FallingLineMaxSteps
        {
            get
            {
                return fallingLineMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                fallingLineMaxSteps = value;
            }
        }
        /// <summary>
        /// [FallingLine] The minimum red color level (true color)
        /// </summary>
        public int FallingLineMinimumRedColorLevel
        {
            get
            {
                return fallingLineMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                fallingLineMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FallingLine] The minimum green color level (true color)
        /// </summary>
        public int FallingLineMinimumGreenColorLevel
        {
            get
            {
                return fallingLineMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                fallingLineMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FallingLine] The minimum blue color level (true color)
        /// </summary>
        public int FallingLineMinimumBlueColorLevel
        {
            get
            {
                return fallingLineMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                fallingLineMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FallingLine] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int FallingLineMinimumColorLevel
        {
            get
            {
                return fallingLineMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                fallingLineMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [FallingLine] The maximum red color level (true color)
        /// </summary>
        public int FallingLineMaximumRedColorLevel
        {
            get
            {
                return fallingLineMaximumRedColorLevel;
            }
            set
            {
                if (value <= fallingLineMinimumRedColorLevel)
                    value = fallingLineMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                fallingLineMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FallingLine] The maximum green color level (true color)
        /// </summary>
        public int FallingLineMaximumGreenColorLevel
        {
            get
            {
                return fallingLineMaximumGreenColorLevel;
            }
            set
            {
                if (value <= fallingLineMinimumGreenColorLevel)
                    value = fallingLineMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                fallingLineMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FallingLine] The maximum blue color level (true color)
        /// </summary>
        public int FallingLineMaximumBlueColorLevel
        {
            get
            {
                return fallingLineMaximumBlueColorLevel;
            }
            set
            {
                if (value <= fallingLineMinimumBlueColorLevel)
                    value = fallingLineMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                fallingLineMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FallingLine] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int FallingLineMaximumColorLevel
        {
            get
            {
                return fallingLineMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= fallingLineMinimumColorLevel)
                    value = fallingLineMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                fallingLineMaximumColorLevel = value;
            }
        }
        #endregion

        #region Indeterminate
        private bool indeterminateTrueColor = true;
        private int indeterminateDelay = 20;
        private char indeterminateUpperLeftCornerChar = '╭';
        private char indeterminateUpperRightCornerChar = '╮';
        private char indeterminateLowerLeftCornerChar = '╰';
        private char indeterminateLowerRightCornerChar = '╯';
        private char indeterminateUpperFrameChar = '─';
        private char indeterminateLowerFrameChar = '─';
        private char indeterminateLeftFrameChar = '│';
        private char indeterminateRightFrameChar = '│';
        private int indeterminateMinimumRedColorLevel = 0;
        private int indeterminateMinimumGreenColorLevel = 0;
        private int indeterminateMinimumBlueColorLevel = 0;
        private int indeterminateMinimumColorLevel = 0;
        private int indeterminateMaximumRedColorLevel = 255;
        private int indeterminateMaximumGreenColorLevel = 255;
        private int indeterminateMaximumBlueColorLevel = 255;
        private int indeterminateMaximumColorLevel = 255;
        private string indeterminateUpperLeftCornerColor = "7";
        private string indeterminateUpperRightCornerColor = "7";
        private string indeterminateLowerLeftCornerColor = "7";
        private string indeterminateLowerRightCornerColor = "7";
        private string indeterminateUpperFrameColor = "7";
        private string indeterminateLowerFrameColor = "7";
        private string indeterminateLeftFrameColor = "7";
        private string indeterminateRightFrameColor = "7";
        private bool indeterminateUseBorderColors;

        /// <summary>
        /// [Indeterminate] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool IndeterminateTrueColor
        {
            get => indeterminateTrueColor;
            set => indeterminateTrueColor = value;
        }
        /// <summary>
        /// [Indeterminate] How many milliseconds to wait before making the next write?
        /// </summary>
        public int IndeterminateDelay
        {
            get => indeterminateDelay;
            set
            {
                if (value <= 0)
                    value = 20;
                indeterminateDelay = value;
            }
        }
        /// <summary>
        /// [Indeterminate] Upper left corner character 
        /// </summary>
        public char IndeterminateUpperLeftCornerChar
        {
            get => indeterminateUpperLeftCornerChar;
            set => indeterminateUpperLeftCornerChar = value;
        }
        /// <summary>
        /// [Indeterminate] Upper right corner character 
        /// </summary>
        public char IndeterminateUpperRightCornerChar
        {
            get => indeterminateUpperRightCornerChar;
            set => indeterminateUpperRightCornerChar = value;
        }
        /// <summary>
        /// [Indeterminate] Lower left corner character 
        /// </summary>
        public char IndeterminateLowerLeftCornerChar
        {
            get => indeterminateLowerLeftCornerChar;
            set => indeterminateLowerLeftCornerChar = value;
        }
        /// <summary>
        /// [Indeterminate] Lower right corner character 
        /// </summary>
        public char IndeterminateLowerRightCornerChar
        {
            get => indeterminateLowerRightCornerChar;
            set => indeterminateLowerRightCornerChar = value;
        }
        /// <summary>
        /// [Indeterminate] Upper frame character 
        /// </summary>
        public char IndeterminateUpperFrameChar
        {
            get => indeterminateUpperFrameChar;
            set => indeterminateUpperFrameChar = value;
        }
        /// <summary>
        /// [Indeterminate] Lower frame character 
        /// </summary>
        public char IndeterminateLowerFrameChar
        {
            get => indeterminateLowerFrameChar;
            set => indeterminateLowerFrameChar = value;
        }
        /// <summary>
        /// [Indeterminate] Left frame character 
        /// </summary>
        public char IndeterminateLeftFrameChar
        {
            get => indeterminateLeftFrameChar;
            set => indeterminateLeftFrameChar = value;
        }
        /// <summary>
        /// [Indeterminate] Right frame character 
        /// </summary>
        public char IndeterminateRightFrameChar
        {
            get => indeterminateRightFrameChar;
            set => indeterminateRightFrameChar = value;
        }
        /// <summary>
        /// [Indeterminate] The minimum red color level (true color)
        /// </summary>
        public int IndeterminateMinimumRedColorLevel
        {
            get => indeterminateMinimumRedColorLevel;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                indeterminateMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The minimum green color level (true color)
        /// </summary>
        public int IndeterminateMinimumGreenColorLevel
        {
            get => indeterminateMinimumGreenColorLevel;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                indeterminateMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The minimum blue color level (true color)
        /// </summary>
        public int IndeterminateMinimumBlueColorLevel
        {
            get => indeterminateMinimumBlueColorLevel;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                indeterminateMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int IndeterminateMinimumColorLevel
        {
            get => indeterminateMinimumColorLevel;
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                indeterminateMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The maximum red color level (true color)
        /// </summary>
        public int IndeterminateMaximumRedColorLevel
        {
            get => indeterminateMaximumRedColorLevel;
            set
            {
                if (value <= indeterminateMinimumRedColorLevel)
                    value = indeterminateMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                indeterminateMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The maximum green color level (true color)
        /// </summary>
        public int IndeterminateMaximumGreenColorLevel
        {
            get => indeterminateMaximumGreenColorLevel;
            set
            {
                if (value <= indeterminateMinimumGreenColorLevel)
                    value = indeterminateMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                indeterminateMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The maximum blue color level (true color)
        /// </summary>
        public int IndeterminateMaximumBlueColorLevel
        {
            get => indeterminateMaximumBlueColorLevel;
            set
            {
                if (value <= indeterminateMinimumBlueColorLevel)
                    value = indeterminateMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                indeterminateMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int IndeterminateMaximumColorLevel
        {
            get => indeterminateMaximumColorLevel;
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= indeterminateMinimumColorLevel)
                    value = indeterminateMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                indeterminateMaximumColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] Upper left corner color.
        /// </summary>
        public string IndeterminateUpperLeftCornerColor
        {
            get => indeterminateUpperLeftCornerColor;
            set => indeterminateUpperLeftCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Indeterminate] Upper right corner color.
        /// </summary>
        public string IndeterminateUpperRightCornerColor
        {
            get => indeterminateUpperRightCornerColor;
            set => indeterminateUpperRightCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Indeterminate] Lower left corner color.
        /// </summary>
        public string IndeterminateLowerLeftCornerColor
        {
            get => indeterminateLowerLeftCornerColor;
            set => indeterminateLowerLeftCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Indeterminate] Lower right corner color.
        /// </summary>
        public string IndeterminateLowerRightCornerColor
        {
            get => indeterminateLowerRightCornerColor;
            set => indeterminateLowerRightCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Indeterminate] Upper frame color.
        /// </summary>
        public string IndeterminateUpperFrameColor
        {
            get => indeterminateUpperFrameColor;
            set => indeterminateUpperFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Indeterminate] Lower frame color.
        /// </summary>
        public string IndeterminateLowerFrameColor
        {
            get => indeterminateLowerFrameColor;
            set => indeterminateLowerFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Indeterminate] Left frame color.
        /// </summary>
        public string IndeterminateLeftFrameColor
        {
            get => indeterminateLeftFrameColor;
            set => indeterminateLeftFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Indeterminate] Right frame color.
        /// </summary>
        public string IndeterminateRightFrameColor
        {
            get => indeterminateRightFrameColor;
            set => indeterminateRightFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Indeterminate] Use the border colors.
        /// </summary>
        public bool IndeterminateUseBorderColors
        {
            get => indeterminateUseBorderColors;
            set => indeterminateUseBorderColors = value;
        }
        #endregion

        #region Pulse
        private int pulseDelay = 50;
        private int pulseMaxSteps = 25;
        private int pulseMinimumRedColorLevel = 0;
        private int pulseMinimumGreenColorLevel = 0;
        private int pulseMinimumBlueColorLevel = 0;
        private int pulseMaximumRedColorLevel = 255;
        private int pulseMaximumGreenColorLevel = 255;
        private int pulseMaximumBlueColorLevel = 255;

        /// <summary>
        /// [Pulse] How many milliseconds to wait before making the next write?
        /// </summary>
        public int PulseDelay
        {
            get
            {
                return pulseDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                pulseDelay = value;
            }
        }
        /// <summary>
        /// [Pulse] How many fade steps to do?
        /// </summary>
        public int PulseMaxSteps
        {
            get
            {
                return pulseMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                pulseMaxSteps = value;
            }
        }
        /// <summary>
        /// [Pulse] The minimum red color level (true color)
        /// </summary>
        public int PulseMinimumRedColorLevel
        {
            get
            {
                return pulseMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                pulseMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Pulse] The minimum green color level (true color)
        /// </summary>
        public int PulseMinimumGreenColorLevel
        {
            get
            {
                return pulseMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                pulseMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Pulse] The minimum blue color level (true color)
        /// </summary>
        public int PulseMinimumBlueColorLevel
        {
            get
            {
                return pulseMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                pulseMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Pulse] The maximum red color level (true color)
        /// </summary>
        public int PulseMaximumRedColorLevel
        {
            get
            {
                return pulseMaximumRedColorLevel;
            }
            set
            {
                if (value <= pulseMinimumRedColorLevel)
                    value = pulseMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                pulseMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Pulse] The maximum green color level (true color)
        /// </summary>
        public int PulseMaximumGreenColorLevel
        {
            get
            {
                return pulseMaximumGreenColorLevel;
            }
            set
            {
                if (value <= pulseMinimumGreenColorLevel)
                    value = pulseMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                pulseMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Pulse] The maximum blue color level (true color)
        /// </summary>
        public int PulseMaximumBlueColorLevel
        {
            get
            {
                return pulseMaximumBlueColorLevel;
            }
            set
            {
                if (value <= pulseMinimumBlueColorLevel)
                    value = pulseMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                pulseMaximumBlueColorLevel = value;
            }
        }
        #endregion

        #region BeatPulse
        private bool beatPulseTrueColor = true;
        private int beatPulseDelay = 50;
        private bool beatPulseCycleColors = true;
        private string beatPulseBeatColor = "17";
        private int beatPulseMaxSteps = 25;
        private int beatPulseMinimumRedColorLevel = 0;
        private int beatPulseMinimumGreenColorLevel = 0;
        private int beatPulseMinimumBlueColorLevel = 0;
        private int beatPulseMinimumColorLevel = 0;
        private int beatPulseMaximumRedColorLevel = 255;
        private int beatPulseMaximumGreenColorLevel = 255;
        private int beatPulseMaximumBlueColorLevel = 255;
        private int beatPulseMaximumColorLevel = 255;

        /// <summary>
        /// [BeatPulse] Enable truecolor support. Has a higher priority than 255 color support. Please note that it only works if color cycling is enabled.
        /// </summary>
        public bool BeatPulseTrueColor
        {
            get
            {
                return beatPulseTrueColor;
            }
            set
            {
                beatPulseTrueColor = value;
            }
        }
        /// <summary>
        /// [BeatPulse] Enable color cycling (uses RNG. If disabled, uses the <see cref="BeatPulseBeatColor"/> color.)
        /// </summary>
        public bool BeatPulseCycleColors
        {
            get
            {
                return beatPulseCycleColors;
            }
            set
            {
                beatPulseCycleColors = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The color of beats. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public string BeatPulseBeatColor
        {
            get
            {
                return beatPulseBeatColor;
            }
            set
            {
                beatPulseBeatColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BeatPulse] How many milliseconds to wait before making the next write?
        /// </summary>
        public int BeatPulseDelay
        {
            get
            {
                return beatPulseDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                beatPulseDelay = value;
            }
        }
        /// <summary>
        /// [BeatPulse] How many fade steps to do?
        /// </summary>
        public int BeatPulseMaxSteps
        {
            get
            {
                return beatPulseMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                beatPulseMaxSteps = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The minimum red color level (true color)
        /// </summary>
        public int BeatPulseMinimumRedColorLevel
        {
            get
            {
                return beatPulseMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                beatPulseMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The minimum green color level (true color)
        /// </summary>
        public int BeatPulseMinimumGreenColorLevel
        {
            get
            {
                return beatPulseMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                beatPulseMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The minimum blue color level (true color)
        /// </summary>
        public int BeatPulseMinimumBlueColorLevel
        {
            get
            {
                return beatPulseMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                beatPulseMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int BeatPulseMinimumColorLevel
        {
            get
            {
                return beatPulseMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                beatPulseMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The maximum red color level (true color)
        /// </summary>
        public int BeatPulseMaximumRedColorLevel
        {
            get
            {
                return beatPulseMaximumRedColorLevel;
            }
            set
            {
                if (value <= beatPulseMinimumRedColorLevel)
                    value = beatPulseMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                beatPulseMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The maximum green color level (true color)
        /// </summary>
        public int BeatPulseMaximumGreenColorLevel
        {
            get
            {
                return beatPulseMaximumGreenColorLevel;
            }
            set
            {
                if (value <= beatPulseMinimumGreenColorLevel)
                    value = beatPulseMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                beatPulseMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The maximum blue color level (true color)
        /// </summary>
        public int BeatPulseMaximumBlueColorLevel
        {
            get
            {
                return beatPulseMaximumBlueColorLevel;
            }
            set
            {
                if (value <= beatPulseMinimumBlueColorLevel)
                    value = beatPulseMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                beatPulseMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int BeatPulseMaximumColorLevel
        {
            get
            {
                return beatPulseMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= beatPulseMinimumColorLevel)
                    value = beatPulseMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                beatPulseMaximumColorLevel = value;
            }
        }
        #endregion

        #region EdgePulse
        private int edgePulseDelay = 50;
        private int edgePulseMaxSteps = 25;
        private int edgePulseMinimumRedColorLevel = 0;
        private int edgePulseMinimumGreenColorLevel = 0;
        private int edgePulseMinimumBlueColorLevel = 0;
        private int edgePulseMaximumRedColorLevel = 255;
        private int edgePulseMaximumGreenColorLevel = 255;
        private int edgePulseMaximumBlueColorLevel = 255;

        /// <summary>
        /// [EdgePulse] How many milliseconds to wait before making the next write?
        /// </summary>
        public int EdgePulseDelay
        {
            get
            {
                return edgePulseDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                edgePulseDelay = value;
            }
        }
        /// <summary>
        /// [EdgePulse] How many fade steps to do?
        /// </summary>
        public int EdgePulseMaxSteps
        {
            get
            {
                return edgePulseMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                edgePulseMaxSteps = value;
            }
        }
        /// <summary>
        /// [EdgePulse] The minimum red color level (true color)
        /// </summary>
        public int EdgePulseMinimumRedColorLevel
        {
            get
            {
                return edgePulseMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                edgePulseMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [EdgePulse] The minimum green color level (true color)
        /// </summary>
        public int EdgePulseMinimumGreenColorLevel
        {
            get
            {
                return edgePulseMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                edgePulseMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [EdgePulse] The minimum blue color level (true color)
        /// </summary>
        public int EdgePulseMinimumBlueColorLevel
        {
            get
            {
                return edgePulseMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                edgePulseMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [EdgePulse] The maximum red color level (true color)
        /// </summary>
        public int EdgePulseMaximumRedColorLevel
        {
            get
            {
                return edgePulseMaximumRedColorLevel;
            }
            set
            {
                if (value <= edgePulseMinimumRedColorLevel)
                    value = edgePulseMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                edgePulseMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [EdgePulse] The maximum green color level (true color)
        /// </summary>
        public int EdgePulseMaximumGreenColorLevel
        {
            get
            {
                return edgePulseMaximumGreenColorLevel;
            }
            set
            {
                if (value <= edgePulseMinimumGreenColorLevel)
                    value = edgePulseMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                edgePulseMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [EdgePulse] The maximum blue color level (true color)
        /// </summary>
        public int EdgePulseMaximumBlueColorLevel
        {
            get
            {
                return edgePulseMaximumBlueColorLevel;
            }
            set
            {
                if (value <= edgePulseMinimumBlueColorLevel)
                    value = edgePulseMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                edgePulseMaximumBlueColorLevel = value;
            }
        }
        #endregion

        #region BeatEdgePulse
        private bool beatEdgePulseTrueColor = true;
        private int beatEdgePulseDelay = 50;
        private bool beatEdgePulseCycleColors = true;
        private string beatEdgePulseBeatColor = "17";
        private int beatEdgePulseMaxSteps = 25;
        private int beatEdgePulseMinimumRedColorLevel = 0;
        private int beatEdgePulseMinimumGreenColorLevel = 0;
        private int beatEdgePulseMinimumBlueColorLevel = 0;
        private int beatEdgePulseMinimumColorLevel = 0;
        private int beatEdgePulseMaximumRedColorLevel = 255;
        private int beatEdgePulseMaximumGreenColorLevel = 255;
        private int beatEdgePulseMaximumBlueColorLevel = 255;
        private int beatEdgePulseMaximumColorLevel = 255;

        /// <summary>
        /// [BeatEdgePulse] Enable truecolor support. Has a higher priority than 255 color support. Please note that it only works if color cycling is enabled.
        /// </summary>
        public bool BeatEdgePulseTrueColor
        {
            get
            {
                return beatEdgePulseTrueColor;
            }
            set
            {
                beatEdgePulseTrueColor = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] Enable color cycling (uses RNG. If disabled, uses the <see cref="BeatEdgePulseBeatColor"/> color.)
        /// </summary>
        public bool BeatEdgePulseCycleColors
        {
            get
            {
                return beatEdgePulseCycleColors;
            }
            set
            {
                beatEdgePulseCycleColors = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The color of beats. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public string BeatEdgePulseBeatColor
        {
            get
            {
                return beatEdgePulseBeatColor;
            }
            set
            {
                beatEdgePulseBeatColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] How many milliseconds to wait before making the next write?
        /// </summary>
        public int BeatEdgePulseDelay
        {
            get
            {
                return beatEdgePulseDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                beatEdgePulseDelay = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] How many fade steps to do?
        /// </summary>
        public int BeatEdgePulseMaxSteps
        {
            get
            {
                return beatEdgePulseMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                beatEdgePulseMaxSteps = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The minimum red color level (true color)
        /// </summary>
        public int BeatEdgePulseMinimumRedColorLevel
        {
            get
            {
                return beatEdgePulseMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                beatEdgePulseMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The minimum green color level (true color)
        /// </summary>
        public int BeatEdgePulseMinimumGreenColorLevel
        {
            get
            {
                return beatEdgePulseMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                beatEdgePulseMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The minimum blue color level (true color)
        /// </summary>
        public int BeatEdgePulseMinimumBlueColorLevel
        {
            get
            {
                return beatEdgePulseMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                beatEdgePulseMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int BeatEdgePulseMinimumColorLevel
        {
            get
            {
                return beatEdgePulseMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                beatEdgePulseMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The maximum red color level (true color)
        /// </summary>
        public int BeatEdgePulseMaximumRedColorLevel
        {
            get
            {
                return beatEdgePulseMaximumRedColorLevel;
            }
            set
            {
                if (value <= beatEdgePulseMinimumRedColorLevel)
                    value = beatEdgePulseMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                beatEdgePulseMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The maximum green color level (true color)
        /// </summary>
        public int BeatEdgePulseMaximumGreenColorLevel
        {
            get
            {
                return beatEdgePulseMaximumGreenColorLevel;
            }
            set
            {
                if (value <= beatEdgePulseMinimumGreenColorLevel)
                    value = beatEdgePulseMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                beatEdgePulseMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The maximum blue color level (true color)
        /// </summary>
        public int BeatEdgePulseMaximumBlueColorLevel
        {
            get
            {
                return beatEdgePulseMaximumBlueColorLevel;
            }
            set
            {
                if (value <= beatEdgePulseMinimumBlueColorLevel)
                    value = beatEdgePulseMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                beatEdgePulseMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int BeatEdgePulseMaximumColorLevel
        {
            get
            {
                return beatEdgePulseMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= beatEdgePulseMinimumColorLevel)
                    value = beatEdgePulseMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                beatEdgePulseMaximumColorLevel = value;
            }
        }
        #endregion

        #region GradientRot
        private int gradientRotDelay = 10;
        private int gradientRotNextRampDelay = 250;
        private int gradientRotMinimumRedColorLevelStart = 0;
        private int gradientRotMinimumGreenColorLevelStart = 0;
        private int gradientRotMinimumBlueColorLevelStart = 0;
        private int gradientRotMaximumRedColorLevelStart = 255;
        private int gradientRotMaximumGreenColorLevelStart = 255;
        private int gradientRotMaximumBlueColorLevelStart = 255;
        private int gradientRotMinimumRedColorLevelEnd = 0;
        private int gradientRotMinimumGreenColorLevelEnd = 0;
        private int gradientRotMinimumBlueColorLevelEnd = 0;
        private int gradientRotMaximumRedColorLevelEnd = 255;
        private int gradientRotMaximumGreenColorLevelEnd = 255;
        private int gradientRotMaximumBlueColorLevelEnd = 255;

        /// <summary>
        /// [GradientRot] How many milliseconds to wait before making the next write?
        /// </summary>
        public int GradientRotDelay
        {
            get
            {
                return gradientRotDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                gradientRotDelay = value;
            }
        }
        /// <summary>
        /// [GradientRot] How many milliseconds to wait before rotting the next screen?
        /// </summary>
        public int GradientRotNextRampDelay
        {
            get
            {
                return gradientRotNextRampDelay;
            }
            set
            {
                if (value <= 0)
                    value = 250;
                gradientRotNextRampDelay = value;
            }
        }
        /// <summary>
        /// [GradientRot] The minimum red color level (true color - start)
        /// </summary>
        public int GradientRotMinimumRedColorLevelStart
        {
            get
            {
                return gradientRotMinimumRedColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                gradientRotMinimumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [GradientRot] The minimum green color level (true color - start)
        /// </summary>
        public int GradientRotMinimumGreenColorLevelStart
        {
            get
            {
                return gradientRotMinimumGreenColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                gradientRotMinimumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [GradientRot] The minimum blue color level (true color - start)
        /// </summary>
        public int GradientRotMinimumBlueColorLevelStart
        {
            get
            {
                return gradientRotMinimumBlueColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                gradientRotMinimumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [GradientRot] The maximum red color level (true color - start)
        /// </summary>
        public int GradientRotMaximumRedColorLevelStart
        {
            get
            {
                return gradientRotMaximumRedColorLevelStart;
            }
            set
            {
                if (value <= gradientRotMinimumRedColorLevelStart)
                    value = gradientRotMinimumRedColorLevelStart;
                if (value > 255)
                    value = 255;
                gradientRotMaximumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [GradientRot] The maximum green color level (true color - start)
        /// </summary>
        public int GradientRotMaximumGreenColorLevelStart
        {
            get
            {
                return gradientRotMaximumGreenColorLevelStart;
            }
            set
            {
                if (value <= gradientRotMinimumGreenColorLevelStart)
                    value = gradientRotMinimumGreenColorLevelStart;
                if (value > 255)
                    value = 255;
                gradientRotMaximumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [GradientRot] The maximum blue color level (true color - start)
        /// </summary>
        public int GradientRotMaximumBlueColorLevelStart
        {
            get
            {
                return gradientRotMaximumBlueColorLevelStart;
            }
            set
            {
                if (value <= gradientRotMinimumBlueColorLevelStart)
                    value = gradientRotMinimumBlueColorLevelStart;
                if (value > 255)
                    value = 255;
                gradientRotMaximumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [GradientRot] The minimum red color level (true color - end)
        /// </summary>
        public int GradientRotMinimumRedColorLevelEnd
        {
            get
            {
                return gradientRotMinimumRedColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                gradientRotMinimumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [GradientRot] The minimum green color level (true color - end)
        /// </summary>
        public int GradientRotMinimumGreenColorLevelEnd
        {
            get
            {
                return gradientRotMinimumGreenColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                gradientRotMinimumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [GradientRot] The minimum blue color level (true color - end)
        /// </summary>
        public int GradientRotMinimumBlueColorLevelEnd
        {
            get
            {
                return gradientRotMinimumBlueColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                gradientRotMinimumBlueColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [GradientRot] The maximum red color level (true color - end)
        /// </summary>
        public int GradientRotMaximumRedColorLevelEnd
        {
            get
            {
                return gradientRotMaximumRedColorLevelEnd;
            }
            set
            {
                if (value <= gradientRotMinimumRedColorLevelEnd)
                    value = gradientRotMinimumRedColorLevelEnd;
                if (value > 255)
                    value = 255;
                gradientRotMaximumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [GradientRot] The maximum green color level (true color - end)
        /// </summary>
        public int GradientRotMaximumGreenColorLevelEnd
        {
            get
            {
                return gradientRotMaximumGreenColorLevelEnd;
            }
            set
            {
                if (value <= gradientRotMinimumGreenColorLevelEnd)
                    value = gradientRotMinimumGreenColorLevelEnd;
                if (value > 255)
                    value = 255;
                gradientRotMaximumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [GradientRot] The maximum blue color level (true color - end)
        /// </summary>
        public int GradientRotMaximumBlueColorLevelEnd
        {
            get
            {
                return gradientRotMaximumBlueColorLevelEnd;
            }
            set
            {
                if (value <= gradientRotMinimumBlueColorLevelEnd)
                    value = gradientRotMinimumBlueColorLevelEnd;
                if (value > 255)
                    value = 255;
                gradientRotMaximumBlueColorLevelEnd = value;
            }
        }
        #endregion

        #region Gradient
        private int gradientNextRotDelay = 3000;
        private int gradientMinimumRedColorLevelStart = 0;
        private int gradientMinimumGreenColorLevelStart = 0;
        private int gradientMinimumBlueColorLevelStart = 0;
        private int gradientMaximumRedColorLevelStart = 255;
        private int gradientMaximumGreenColorLevelStart = 255;
        private int gradientMaximumBlueColorLevelStart = 255;
        private int gradientMinimumRedColorLevelEnd = 0;
        private int gradientMinimumGreenColorLevelEnd = 0;
        private int gradientMinimumBlueColorLevelEnd = 0;
        private int gradientMaximumRedColorLevelEnd = 255;
        private int gradientMaximumGreenColorLevelEnd = 255;
        private int gradientMaximumBlueColorLevelEnd = 255;

        /// <summary>
        /// [Gradient] How many milliseconds to wait before rotting the next screen?
        /// </summary>
        public int GradientNextRotDelay
        {
            get
            {
                return gradientNextRotDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                gradientNextRotDelay = value;
            }
        }
        /// <summary>
        /// [Gradient] The minimum red color level (true color - start)
        /// </summary>
        public int GradientMinimumRedColorLevelStart
        {
            get
            {
                return gradientMinimumRedColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                gradientMinimumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Gradient] The minimum green color level (true color - start)
        /// </summary>
        public int GradientMinimumGreenColorLevelStart
        {
            get
            {
                return gradientMinimumGreenColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                gradientMinimumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Gradient] The minimum blue color level (true color - start)
        /// </summary>
        public int GradientMinimumBlueColorLevelStart
        {
            get
            {
                return gradientMinimumBlueColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                gradientMinimumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Gradient] The maximum red color level (true color - start)
        /// </summary>
        public int GradientMaximumRedColorLevelStart
        {
            get
            {
                return gradientMaximumRedColorLevelStart;
            }
            set
            {
                if (value <= gradientMinimumRedColorLevelStart)
                    value = gradientMinimumRedColorLevelStart;
                if (value > 255)
                    value = 255;
                gradientMaximumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Gradient] The maximum green color level (true color - start)
        /// </summary>
        public int GradientMaximumGreenColorLevelStart
        {
            get
            {
                return gradientMaximumGreenColorLevelStart;
            }
            set
            {
                if (value <= gradientMinimumGreenColorLevelStart)
                    value = gradientMinimumGreenColorLevelStart;
                if (value > 255)
                    value = 255;
                gradientMaximumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Gradient] The maximum blue color level (true color - start)
        /// </summary>
        public int GradientMaximumBlueColorLevelStart
        {
            get
            {
                return gradientMaximumBlueColorLevelStart;
            }
            set
            {
                if (value <= gradientMinimumBlueColorLevelStart)
                    value = gradientMinimumBlueColorLevelStart;
                if (value > 255)
                    value = 255;
                gradientMaximumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Gradient] The minimum red color level (true color - end)
        /// </summary>
        public int GradientMinimumRedColorLevelEnd
        {
            get
            {
                return gradientMinimumRedColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                gradientMinimumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Gradient] The minimum green color level (true color - end)
        /// </summary>
        public int GradientMinimumGreenColorLevelEnd
        {
            get
            {
                return gradientMinimumGreenColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                gradientMinimumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Gradient] The minimum blue color level (true color - end)
        /// </summary>
        public int GradientMinimumBlueColorLevelEnd
        {
            get
            {
                return gradientMinimumBlueColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                gradientMinimumBlueColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Gradient] The maximum red color level (true color - end)
        /// </summary>
        public int GradientMaximumRedColorLevelEnd
        {
            get
            {
                return gradientMaximumRedColorLevelEnd;
            }
            set
            {
                if (value <= gradientMinimumRedColorLevelEnd)
                    value = gradientMinimumRedColorLevelEnd;
                if (value > 255)
                    value = 255;
                gradientMaximumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Gradient] The maximum green color level (true color - end)
        /// </summary>
        public int GradientMaximumGreenColorLevelEnd
        {
            get
            {
                return gradientMaximumGreenColorLevelEnd;
            }
            set
            {
                if (value <= gradientMinimumGreenColorLevelEnd)
                    value = gradientMinimumGreenColorLevelEnd;
                if (value > 255)
                    value = 255;
                gradientMaximumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Gradient] The maximum blue color level (true color - end)
        /// </summary>
        public int GradientMaximumBlueColorLevelEnd
        {
            get
            {
                return gradientMaximumBlueColorLevelEnd;
            }
            set
            {
                if (value <= gradientMinimumBlueColorLevelEnd)
                    value = gradientMinimumBlueColorLevelEnd;
                if (value > 255)
                    value = 255;
                gradientMaximumBlueColorLevelEnd = value;
            }
        }
        #endregion

        #region Starfield
        private int starfieldDelay = 10;

        /// <summary>
        /// [Starfield] How many milliseconds to wait before making the next write?
        /// </summary>
        public int StarfieldDelay
        {
            get
            {
                return starfieldDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                starfieldDelay = value;
            }
        }
        #endregion

        #region Siren
        private int sirenDelay = 500;
        private string sirenStyle = "Cop";

        /// <summary>
        /// [Siren] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SirenDelay
        {
            get
            {
                return sirenDelay;
            }
            set
            {
                if (value <= 0)
                    value = 500;
                sirenDelay = value;
            }
        }

        /// <summary>
        /// [Siren] The siren style
        /// </summary>
        public string SirenStyle
        {
            get
            {
                return sirenStyle;
            }
            set
            {
                sirenStyle = SirenDisplay.sirens.ContainsKey(value) ? value : "Cop";
            }
        }
        #endregion

        #region Spin
        private int spinDelay = 10;

        /// <summary>
        /// [Spin] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SpinDelay
        {
            get
            {
                return spinDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                spinDelay = value;
            }
        }
        #endregion

        #region SnakeFill
        private bool snakeFillTrueColor = true;
        private int snakeFillDelay = 10;
        private int snakeFillMinimumRedColorLevel = 0;
        private int snakeFillMinimumGreenColorLevel = 0;
        private int snakeFillMinimumBlueColorLevel = 0;
        private int snakeFillMinimumColorLevel = 0;
        private int snakeFillMaximumRedColorLevel = 255;
        private int snakeFillMaximumGreenColorLevel = 255;
        private int snakeFillMaximumBlueColorLevel = 255;
        private int snakeFillMaximumColorLevel = 255;

        /// <summary>
        /// [SnakeFill] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool SnakeFillTrueColor
        {
            get
            {
                return snakeFillTrueColor;
            }
            set
            {
                snakeFillTrueColor = value;
            }
        }
        /// <summary>
        /// [SnakeFill] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SnakeFillDelay
        {
            get
            {
                return snakeFillDelay;
            }
            set
            {
                snakeFillDelay = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The minimum red color level (true color)
        /// </summary>
        public int SnakeFillMinimumRedColorLevel
        {
            get
            {
                return snakeFillMinimumRedColorLevel;
            }
            set
            {
                snakeFillMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The minimum green color level (true color)
        /// </summary>
        public int SnakeFillMinimumGreenColorLevel
        {
            get
            {
                return snakeFillMinimumGreenColorLevel;
            }
            set
            {
                snakeFillMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The minimum blue color level (true color)
        /// </summary>
        public int SnakeFillMinimumBlueColorLevel
        {
            get
            {
                return snakeFillMinimumBlueColorLevel;
            }
            set
            {
                snakeFillMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int SnakeFillMinimumColorLevel
        {
            get
            {
                return snakeFillMinimumColorLevel;
            }
            set
            {
                snakeFillMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The maximum red color level (true color)
        /// </summary>
        public int SnakeFillMaximumRedColorLevel
        {
            get
            {
                return snakeFillMaximumRedColorLevel;
            }
            set
            {
                snakeFillMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The maximum green color level (true color)
        /// </summary>
        public int SnakeFillMaximumGreenColorLevel
        {
            get
            {
                return snakeFillMaximumGreenColorLevel;
            }
            set
            {
                snakeFillMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The maximum blue color level (true color)
        /// </summary>
        public int SnakeFillMaximumBlueColorLevel
        {
            get
            {
                return snakeFillMaximumBlueColorLevel;
            }
            set
            {
                snakeFillMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int SnakeFillMaximumColorLevel
        {
            get
            {
                return snakeFillMaximumColorLevel;
            }
            set
            {
                snakeFillMaximumColorLevel = value;
            }
        }
        #endregion

        #region Equalizer
        private int equalizerNextScreenDelay = 3000;

        /// <summary>
        /// [Equalizer] How many milliseconds to wait before going to next equalizer preset?
        /// </summary>
        public int EqualizerNextScreenDelay
        {
            get
            {
                return equalizerNextScreenDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                equalizerNextScreenDelay = value;
            }
        }
        #endregion

        #region BSOD
        private int bsodDelay = 10000;

        /// <summary>
        /// [BSOD] How many beats per minute to wait before making the next write?
        /// </summary>
        public int BSODDelay
        {
            get
            {
                return bsodDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10000;
                bsodDelay = value;
            }
        }
        #endregion

        #region Memdump
        private int memdumpDelay = 500;

        /// <summary>
        /// [Memdump] How many milliseconds to wait before making the next write?
        /// </summary>
        public int MemdumpDelay
        {
            get
            {
                return memdumpDelay;
            }
            set
            {
                if (value <= 0)
                    value = 500;
                memdumpDelay = value;
            }
        }
        #endregion

        #region ExcaliBeats
        private bool excaliBeatsTrueColor = true;
        private int excaliBeatsDelay = 140;
        private bool excaliBeatsCycleColors = true;
        private bool excaliBeatsExplicit = true;
        private bool excaliBeatsTranceMode;
        private string excaliBeatsBeatColor = "17";
        private int excaliBeatsMaxSteps = 25;
        private int excaliBeatsMinimumRedColorLevel = 0;
        private int excaliBeatsMinimumGreenColorLevel = 0;
        private int excaliBeatsMinimumBlueColorLevel = 0;
        private int excaliBeatsMinimumColorLevel = 0;
        private int excaliBeatsMaximumRedColorLevel = 255;
        private int excaliBeatsMaximumGreenColorLevel = 255;
        private int excaliBeatsMaximumBlueColorLevel = 255;
        private int excaliBeatsMaximumColorLevel = 255;

        /// <summary>
        /// [ExcaliBeats] Enable truecolor support. Has a higher priority than 255 color support. Please note that it only works if color cycling is enabled.
        /// </summary>
        public bool ExcaliBeatsTrueColor
        {
            get
            {
                return excaliBeatsTrueColor;
            }
            set
            {
                excaliBeatsTrueColor = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] Enable color cycling (uses RNG. If disabled, uses the <see cref="ExcaliBeatsBeatColor"/> color.)
        /// </summary>
        public bool ExcaliBeatsCycleColors
        {
            get
            {
                return excaliBeatsCycleColors;
            }
            set
            {
                excaliBeatsCycleColors = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] Explicitly change the text to Excalibur
        /// </summary>
        public bool ExcaliBeatsExplicit
        {
            get
            {
                return excaliBeatsExplicit;
            }
            set
            {
                excaliBeatsExplicit = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] [Linux only] Trance mode - Multiplies the BPM by 2 to simulate the trance music style
        /// </summary>
        public bool ExcaliBeatsTranceMode
        {
            get
            {
                return excaliBeatsTranceMode;
            }
            [UnsupportedOSPlatform("windows")]
            set
            {
                if (KernelPlatform.IsOnUnix())
                    excaliBeatsTranceMode = value;
                else
                    excaliBeatsTranceMode = false;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The color of beats. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public string ExcaliBeatsBeatColor
        {
            get
            {
                return excaliBeatsBeatColor;
            }
            set
            {
                excaliBeatsBeatColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [ExcaliBeats] How many beats per minute to wait before making the next write?
        /// </summary>
        public int ExcaliBeatsDelay
        {
            get
            {
                return excaliBeatsDelay;
            }
            set
            {
                if (value <= 0)
                    value = 140;
                excaliBeatsDelay = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] How many fade steps to do?
        /// </summary>
        public int ExcaliBeatsMaxSteps
        {
            get
            {
                return excaliBeatsMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                excaliBeatsMaxSteps = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The minimum red color level (true color)
        /// </summary>
        public int ExcaliBeatsMinimumRedColorLevel
        {
            get
            {
                return excaliBeatsMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                excaliBeatsMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The minimum green color level (true color)
        /// </summary>
        public int ExcaliBeatsMinimumGreenColorLevel
        {
            get
            {
                return excaliBeatsMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                excaliBeatsMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The minimum blue color level (true color)
        /// </summary>
        public int ExcaliBeatsMinimumBlueColorLevel
        {
            get
            {
                return excaliBeatsMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                excaliBeatsMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int ExcaliBeatsMinimumColorLevel
        {
            get
            {
                return excaliBeatsMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                excaliBeatsMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The maximum red color level (true color)
        /// </summary>
        public int ExcaliBeatsMaximumRedColorLevel
        {
            get
            {
                return excaliBeatsMaximumRedColorLevel;
            }
            set
            {
                if (value <= excaliBeatsMinimumRedColorLevel)
                    value = excaliBeatsMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                excaliBeatsMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The maximum green color level (true color)
        /// </summary>
        public int ExcaliBeatsMaximumGreenColorLevel
        {
            get
            {
                return excaliBeatsMaximumGreenColorLevel;
            }
            set
            {
                if (value <= excaliBeatsMinimumGreenColorLevel)
                    value = excaliBeatsMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                excaliBeatsMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The maximum blue color level (true color)
        /// </summary>
        public int ExcaliBeatsMaximumBlueColorLevel
        {
            get
            {
                return excaliBeatsMaximumBlueColorLevel;
            }
            set
            {
                if (value <= excaliBeatsMinimumBlueColorLevel)
                    value = excaliBeatsMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                excaliBeatsMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int ExcaliBeatsMaximumColorLevel
        {
            get
            {
                return excaliBeatsMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= excaliBeatsMinimumColorLevel)
                    value = excaliBeatsMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                excaliBeatsMaximumColorLevel = value;
            }
        }
        #endregion

        #region BarWave
        private bool barWaveTrueColor = true;
        private int barWaveDelay = 100;
        private double barWaveFrequencyLevel = 2;
        private int barWaveMinimumRedColorLevel = 0;
        private int barWaveMinimumGreenColorLevel = 0;
        private int barWaveMinimumBlueColorLevel = 0;
        private int barWaveMinimumColorLevel = 0;
        private int barWaveMaximumRedColorLevel = 255;
        private int barWaveMaximumGreenColorLevel = 255;
        private int barWaveMaximumBlueColorLevel = 255;
        private int barWaveMaximumColorLevel = 255;

        /// <summary>
        /// [BarWave] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool BarWaveTrueColor
        {
            get
            {
                return barWaveTrueColor;
            }
            set
            {
                barWaveTrueColor = value;
            }
        }
        /// <summary>
        /// [BarWave] The level of the frequency. This is the denominator of the Pi value (3.1415926...) in mathematics, defined by <see cref="Math.PI"/>.
        /// </summary>
        public double BarWaveFrequencyLevel
        {
            get
            {
                return barWaveFrequencyLevel;
            }
            set
            {
                if (value <= 0)
                    value = 2;
                barWaveFrequencyLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] How many milliseconds to wait before making the next write?
        /// </summary>
        public int BarWaveDelay
        {
            get
            {
                return barWaveDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                barWaveDelay = value;
            }
        }
        /// <summary>
        /// [BarWave] The minimum red color level (true color)
        /// </summary>
        public int BarWaveMinimumRedColorLevel
        {
            get
            {
                return barWaveMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                barWaveMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] The minimum green color level (true color)
        /// </summary>
        public int BarWaveMinimumGreenColorLevel
        {
            get
            {
                return barWaveMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                barWaveMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] The minimum blue color level (true color)
        /// </summary>
        public int BarWaveMinimumBlueColorLevel
        {
            get
            {
                return barWaveMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                barWaveMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int BarWaveMinimumColorLevel
        {
            get
            {
                return barWaveMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                barWaveMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] The maximum red color level (true color)
        /// </summary>
        public int BarWaveMaximumRedColorLevel
        {
            get
            {
                return barWaveMaximumRedColorLevel;
            }
            set
            {
                if (value <= barWaveMaximumRedColorLevel)
                    value = barWaveMaximumRedColorLevel;
                if (value > 255)
                    value = 255;
                barWaveMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] The maximum green color level (true color)
        /// </summary>
        public int BarWaveMaximumGreenColorLevel
        {
            get
            {
                return barWaveMaximumGreenColorLevel;
            }
            set
            {
                if (value <= barWaveMaximumGreenColorLevel)
                    value = barWaveMaximumGreenColorLevel;
                if (value > 255)
                    value = 255;
                barWaveMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] The maximum blue color level (true color)
        /// </summary>
        public int BarWaveMaximumBlueColorLevel
        {
            get
            {
                return barWaveMaximumBlueColorLevel;
            }
            set
            {
                if (value <= barWaveMaximumBlueColorLevel)
                    value = barWaveMaximumBlueColorLevel;
                if (value > 255)
                    value = 255;
                barWaveMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int BarWaveMaximumColorLevel
        {
            get
            {
                return barWaveMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= barWaveMaximumColorLevel)
                    value = barWaveMaximumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                barWaveMaximumColorLevel = value;
            }
        }
        #endregion

        #region Wave
        private int waveDelay = 100;
        private double waveFrequencyLevel = 3;
        private int waveMinimumRedColorLevel = 0;
        private int waveMinimumGreenColorLevel = 0;
        private int waveMinimumBlueColorLevel = 0;
        private int waveMinimumColorLevel = 0;
        private int waveMaximumRedColorLevel = 255;
        private int waveMaximumGreenColorLevel = 255;
        private int waveMaximumBlueColorLevel = 255;
        private int waveMaximumColorLevel = 255;

        /// <summary>
        /// [Wave] How many milliseconds to wait before making the next write?
        /// </summary>
        public int WaveDelay
        {
            get
            {
                return waveDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                waveDelay = value;
            }
        }
        /// <summary>
        /// [Wave] The level of the frequency. This is the denominator of the Pi value (3.1415926...) in mathematics, defined by <see cref="Math.PI"/>.
        /// </summary>
        public double WaveFrequencyLevel
        {
            get
            {
                return waveFrequencyLevel;
            }
            set
            {
                if (value <= 0)
                    value = 3;
                waveFrequencyLevel = value;
            }
        }
        /// <summary>
        /// [Wave] The minimum red color level (true color)
        /// </summary>
        public int WaveMinimumRedColorLevel
        {
            get
            {
                return waveMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                waveMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Wave] The minimum green color level (true color)
        /// </summary>
        public int WaveMinimumGreenColorLevel
        {
            get
            {
                return waveMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                waveMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Wave] The minimum blue color level (true color)
        /// </summary>
        public int WaveMinimumBlueColorLevel
        {
            get
            {
                return waveMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                waveMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Wave] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int WaveMinimumColorLevel
        {
            get
            {
                return waveMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                waveMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Wave] The maximum red color level (true color)
        /// </summary>
        public int WaveMaximumRedColorLevel
        {
            get
            {
                return waveMaximumRedColorLevel;
            }
            set
            {
                if (value <= waveMaximumRedColorLevel)
                    value = waveMaximumRedColorLevel;
                if (value > 255)
                    value = 255;
                waveMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Wave] The maximum green color level (true color)
        /// </summary>
        public int WaveMaximumGreenColorLevel
        {
            get
            {
                return waveMaximumGreenColorLevel;
            }
            set
            {
                if (value <= waveMaximumGreenColorLevel)
                    value = waveMaximumGreenColorLevel;
                if (value > 255)
                    value = 255;
                waveMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Wave] The maximum blue color level (true color)
        /// </summary>
        public int WaveMaximumBlueColorLevel
        {
            get
            {
                return waveMaximumBlueColorLevel;
            }
            set
            {
                if (value <= waveMaximumBlueColorLevel)
                    value = waveMaximumBlueColorLevel;
                if (value > 255)
                    value = 255;
                waveMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Wave] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int WaveMaximumColorLevel
        {
            get
            {
                return waveMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= waveMaximumColorLevel)
                    value = waveMaximumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                waveMaximumColorLevel = value;
            }
        }
        #endregion

        #region Mesmerize
        private int mesmerizeDelay = 10;
        private int mesmerizeMinimumRedColorLevel = 0;
        private int mesmerizeMinimumGreenColorLevel = 0;
        private int mesmerizeMinimumBlueColorLevel = 0;
        private int mesmerizeMinimumColorLevel = 0;
        private int mesmerizeMaximumRedColorLevel = 255;
        private int mesmerizeMaximumGreenColorLevel = 255;
        private int mesmerizeMaximumBlueColorLevel = 255;
        private int mesmerizeMaximumColorLevel = 255;

        /// <summary>
        /// [Mesmerize] How many milliseconds to wait before making the next write?
        /// </summary>
        public int MesmerizeDelay
        {
            get
            {
                return mesmerizeDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                mesmerizeDelay = value;
            }
        }
        /// <summary>
        /// [Mesmerize] The minimum red color level (true color)
        /// </summary>
        public int MesmerizeMinimumRedColorLevel
        {
            get
            {
                return mesmerizeMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                mesmerizeMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Mesmerize] The minimum green color level (true color)
        /// </summary>
        public int MesmerizeMinimumGreenColorLevel
        {
            get
            {
                return mesmerizeMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                mesmerizeMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Mesmerize] The minimum blue color level (true color)
        /// </summary>
        public int MesmerizeMinimumBlueColorLevel
        {
            get
            {
                return mesmerizeMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                mesmerizeMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Mesmerize] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int MesmerizeMinimumColorLevel
        {
            get
            {
                return mesmerizeMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                mesmerizeMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Mesmerize] The maximum red color level (true color)
        /// </summary>
        public int MesmerizeMaximumRedColorLevel
        {
            get
            {
                return mesmerizeMaximumRedColorLevel;
            }
            set
            {
                if (value <= mesmerizeMaximumRedColorLevel)
                    value = mesmerizeMaximumRedColorLevel;
                if (value > 255)
                    value = 255;
                mesmerizeMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Mesmerize] The maximum green color level (true color)
        /// </summary>
        public int MesmerizeMaximumGreenColorLevel
        {
            get
            {
                return mesmerizeMaximumGreenColorLevel;
            }
            set
            {
                if (value <= mesmerizeMaximumGreenColorLevel)
                    value = mesmerizeMaximumGreenColorLevel;
                if (value > 255)
                    value = 255;
                mesmerizeMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Mesmerize] The maximum blue color level (true color)
        /// </summary>
        public int MesmerizeMaximumBlueColorLevel
        {
            get
            {
                return mesmerizeMaximumBlueColorLevel;
            }
            set
            {
                if (value <= mesmerizeMaximumBlueColorLevel)
                    value = mesmerizeMaximumBlueColorLevel;
                if (value > 255)
                    value = 255;
                mesmerizeMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Mesmerize] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int MesmerizeMaximumColorLevel
        {
            get
            {
                return mesmerizeMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= mesmerizeMaximumColorLevel)
                    value = mesmerizeMaximumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                mesmerizeMaximumColorLevel = value;
            }
        }
        #endregion

        #region Aurora
        private int auroraDelay = 100;

        /// <summary>
        /// [Aurora] How many milliseconds to wait before making the next write?
        /// </summary>
        public int AuroraDelay
        {
            get
            {
                return auroraDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                auroraDelay = value;
            }
        }
        #endregion

        #region Lightning
        private int lightningDelay = 100;
        private int lightningStrikeProbability = 5;

        /// <summary>
        /// [Lightning] How many milliseconds to wait before making the next write?
        /// </summary>
        public int LightningDelay
        {
            get
            {
                return lightningDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                lightningDelay = value;
            }
        }
        /// <summary>
        /// [Lightning] Chance, in percent, to strike
        /// </summary>
        public int LightningStrikeProbability
        {
            get
            {
                return lightningStrikeProbability;
            }
            set
            {
                if (value <= 0)
                    value = 5;
                lightningStrikeProbability = value;
            }
        }
        #endregion

        #region Bloom
        private int bloomDelay = 50;
        private bool bloomDarkColors;
        private int bloomSteps = 100;

        /// <summary>
        /// [Bloom] How many milliseconds to wait before making the next write?
        /// </summary>
        public int BloomDelay
        {
            get
            {
                return bloomDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                bloomDelay = value;
            }
        }
        /// <summary>
        /// [Bloom] Whether to use dark colors or not
        /// </summary>
        public bool BloomDarkColors
        {
            get
            {
                return bloomDarkColors;
            }
            set
            {
                bloomDarkColors = value;
            }
        }
        /// <summary>
        /// [Bloom] How many color steps for transitioning between two colors?
        /// </summary>
        public int BloomSteps
        {
            get
            {
                return bloomSteps;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                bloomSteps = value;
            }
        }
        #endregion

        #region WordHasher
        private bool wordHasherTrueColor = true;
        private int wordHasherDelay = 1000;
        private int wordHasherMinimumRedColorLevel = 0;
        private int wordHasherMinimumGreenColorLevel = 0;
        private int wordHasherMinimumBlueColorLevel = 0;
        private int wordHasherMinimumColorLevel = 0;
        private int wordHasherMaximumRedColorLevel = 255;
        private int wordHasherMaximumGreenColorLevel = 255;
        private int wordHasherMaximumBlueColorLevel = 255;
        private int wordHasherMaximumColorLevel = 255;

        /// <summary>
        /// [WordHasher] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool WordHasherTrueColor
        {
            get
            {
                return wordHasherTrueColor;
            }
            set
            {
                wordHasherTrueColor = value;
            }
        }
        /// <summary>
        /// [WordHasher] How many milliseconds to wait before making the next write?
        /// </summary>
        public int WordHasherDelay
        {
            get
            {
                return wordHasherDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                wordHasherDelay = value;
            }
        }
        /// <summary>
        /// [WordHasher] The minimum red color level (true color)
        /// </summary>
        public int WordHasherMinimumRedColorLevel
        {
            get
            {
                return wordHasherMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                wordHasherMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasher] The minimum green color level (true color)
        /// </summary>
        public int WordHasherMinimumGreenColorLevel
        {
            get
            {
                return wordHasherMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                wordHasherMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasher] The minimum blue color level (true color)
        /// </summary>
        public int WordHasherMinimumBlueColorLevel
        {
            get
            {
                return wordHasherMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                wordHasherMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasher] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int WordHasherMinimumColorLevel
        {
            get
            {
                return wordHasherMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                wordHasherMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasher] The maximum red color level (true color)
        /// </summary>
        public int WordHasherMaximumRedColorLevel
        {
            get
            {
                return wordHasherMaximumRedColorLevel;
            }
            set
            {
                if (value <= wordHasherMinimumRedColorLevel)
                    value = wordHasherMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                wordHasherMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasher] The maximum green color level (true color)
        /// </summary>
        public int WordHasherMaximumGreenColorLevel
        {
            get
            {
                return wordHasherMaximumGreenColorLevel;
            }
            set
            {
                if (value <= wordHasherMinimumGreenColorLevel)
                    value = wordHasherMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                wordHasherMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasher] The maximum blue color level (true color)
        /// </summary>
        public int WordHasherMaximumBlueColorLevel
        {
            get
            {
                return wordHasherMaximumBlueColorLevel;
            }
            set
            {
                if (value <= wordHasherMinimumBlueColorLevel)
                    value = wordHasherMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                wordHasherMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasher] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int WordHasherMaximumColorLevel
        {
            get
            {
                return wordHasherMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= wordHasherMinimumColorLevel)
                    value = wordHasherMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                wordHasherMaximumColorLevel = value;
            }
        }
        #endregion

        #region SquareCorner
        private int squareCornerDelay = 10;
        private int squareCornerFadeOutDelay = 3000;
        private int squareCornerMaxSteps = 25;
        private int squareCornerMinimumRedColorLevel = 0;
        private int squareCornerMinimumGreenColorLevel = 0;
        private int squareCornerMinimumBlueColorLevel = 0;
        private int squareCornerMaximumRedColorLevel = 255;
        private int squareCornerMaximumGreenColorLevel = 255;
        private int squareCornerMaximumBlueColorLevel = 255;

        /// <summary>
        /// [SquareCorner] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SquareCornerDelay
        {
            get
            {
                return squareCornerDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                squareCornerDelay = value;
            }
        }
        /// <summary>
        /// [SquareCorner] How many milliseconds to wait before fading the square out?
        /// </summary>
        public int SquareCornerFadeOutDelay
        {
            get
            {
                return squareCornerFadeOutDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                squareCornerFadeOutDelay = value;
            }
        }
        /// <summary>
        /// [SquareCorner] How many fade steps to do?
        /// </summary>
        public int SquareCornerMaxSteps
        {
            get
            {
                return squareCornerMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                squareCornerMaxSteps = value;
            }
        }
        /// <summary>
        /// [SquareCorner] The minimum red color level (true color)
        /// </summary>
        public int SquareCornerMinimumRedColorLevel
        {
            get
            {
                return squareCornerMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                squareCornerMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [SquareCorner] The minimum green color level (true color)
        /// </summary>
        public int SquareCornerMinimumGreenColorLevel
        {
            get
            {
                return squareCornerMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                squareCornerMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [SquareCorner] The minimum blue color level (true color)
        /// </summary>
        public int SquareCornerMinimumBlueColorLevel
        {
            get
            {
                return squareCornerMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                squareCornerMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [SquareCorner] The maximum red color level (true color)
        /// </summary>
        public int SquareCornerMaximumRedColorLevel
        {
            get
            {
                return squareCornerMaximumRedColorLevel;
            }
            set
            {
                if (value <= squareCornerMinimumRedColorLevel)
                    value = squareCornerMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                squareCornerMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [SquareCorner] The maximum green color level (true color)
        /// </summary>
        public int SquareCornerMaximumGreenColorLevel
        {
            get
            {
                return squareCornerMaximumGreenColorLevel;
            }
            set
            {
                if (value <= squareCornerMinimumGreenColorLevel)
                    value = squareCornerMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                squareCornerMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [SquareCorner] The maximum blue color level (true color)
        /// </summary>
        public int SquareCornerMaximumBlueColorLevel
        {
            get
            {
                return squareCornerMaximumBlueColorLevel;
            }
            set
            {
                if (value <= squareCornerMinimumBlueColorLevel)
                    value = squareCornerMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                squareCornerMaximumBlueColorLevel = value;
            }
        }
        #endregion

        #region NumberScatter
        private int numberScatterDelay = 1;
        private string numberScatterBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private string numberScatterForegroundColor = new Color(ConsoleColors.Green).PlainSequence;

        /// <summary>
        /// [NumberScatter] How many milliseconds to wait before making the next write?
        /// </summary>
        public int NumberScatterDelay
        {
            get
            {
                return numberScatterDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                numberScatterDelay = value;
            }
        }
        /// <summary>
        /// [NumberScatter] Screensaver background color
        /// </summary>
        public string NumberScatterBackgroundColor
        {
            get
            {
                return numberScatterBackgroundColor;
            }
            set
            {
                numberScatterBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [NumberScatter] Screensaver foreground color
        /// </summary>
        public string NumberScatterForegroundColor
        {
            get
            {
                return numberScatterForegroundColor;
            }
            set
            {
                numberScatterForegroundColor = new Color(value).PlainSequence;
            }
        }
        #endregion

        #region Quote
        private bool quoteTrueColor = true;
        private int quoteDelay = 10000;
        private int quoteMinimumRedColorLevel = 0;
        private int quoteMinimumGreenColorLevel = 0;
        private int quoteMinimumBlueColorLevel = 0;
        private int quoteMinimumColorLevel = 0;
        private int quoteMaximumRedColorLevel = 255;
        private int quoteMaximumGreenColorLevel = 255;
        private int quoteMaximumBlueColorLevel = 255;
        private int quoteMaximumColorLevel = 255;

        /// <summary>
        /// [Quote] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool QuoteTrueColor
        {
            get
            {
                return quoteTrueColor;
            }
            set
            {
                quoteTrueColor = value;
            }
        }
        /// <summary>
        /// [Quote] How many milliseconds to wait before making the next write?
        /// </summary>
        public int QuoteDelay
        {
            get
            {
                return quoteDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10000;
                quoteDelay = value;
            }
        }
        /// <summary>
        /// [Quote] The minimum red color level (true color)
        /// </summary>
        public int QuoteMinimumRedColorLevel
        {
            get
            {
                return quoteMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                quoteMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Quote] The minimum green color level (true color)
        /// </summary>
        public int QuoteMinimumGreenColorLevel
        {
            get
            {
                return quoteMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                quoteMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Quote] The minimum blue color level (true color)
        /// </summary>
        public int QuoteMinimumBlueColorLevel
        {
            get
            {
                return quoteMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                quoteMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Quote] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int QuoteMinimumColorLevel
        {
            get
            {
                return quoteMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                quoteMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Quote] The maximum red color level (true color)
        /// </summary>
        public int QuoteMaximumRedColorLevel
        {
            get
            {
                return quoteMaximumRedColorLevel;
            }
            set
            {
                if (value <= quoteMinimumRedColorLevel)
                    value = quoteMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                quoteMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Quote] The maximum green color level (true color)
        /// </summary>
        public int QuoteMaximumGreenColorLevel
        {
            get
            {
                return quoteMaximumGreenColorLevel;
            }
            set
            {
                if (value <= quoteMinimumGreenColorLevel)
                    value = quoteMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                quoteMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Quote] The maximum blue color level (true color)
        /// </summary>
        public int QuoteMaximumBlueColorLevel
        {
            get
            {
                return quoteMaximumBlueColorLevel;
            }
            set
            {
                if (value <= quoteMinimumBlueColorLevel)
                    value = quoteMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                quoteMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Quote] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int QuoteMaximumColorLevel
        {
            get
            {
                return quoteMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= quoteMinimumColorLevel)
                    value = quoteMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                quoteMaximumColorLevel = value;
            }
        }
        #endregion

        #region BoxGrid
        private int boxGridDelay = 5000;
        private int boxGridMinimumRedColorLevel = 0;
        private int boxGridMinimumGreenColorLevel = 0;
        private int boxGridMinimumBlueColorLevel = 0;
        private int boxGridMaximumRedColorLevel = 255;
        private int boxGridMaximumGreenColorLevel = 255;
        private int boxGridMaximumBlueColorLevel = 255;

        /// <summary>
        /// [BoxGrid] How many milliseconds to wait before making the next write?
        /// </summary>
        public int BoxGridDelay
        {
            get
            {
                return boxGridDelay;
            }
            set
            {
                if (value <= 0)
                    value = 5000;
                boxGridDelay = value;
            }
        }
        /// <summary>
        /// [BoxGrid] The minimum red color level (true color)
        /// </summary>
        public int BoxGridMinimumRedColorLevel
        {
            get
            {
                return boxGridMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                boxGridMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BoxGrid] The minimum green color level (true color)
        /// </summary>
        public int BoxGridMinimumGreenColorLevel
        {
            get
            {
                return boxGridMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                boxGridMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BoxGrid] The minimum blue color level (true color)
        /// </summary>
        public int BoxGridMinimumBlueColorLevel
        {
            get
            {
                return boxGridMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                boxGridMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BoxGrid] The maximum red color level (true color)
        /// </summary>
        public int BoxGridMaximumRedColorLevel
        {
            get
            {
                return boxGridMaximumRedColorLevel;
            }
            set
            {
                if (value <= boxGridMinimumRedColorLevel)
                    value = boxGridMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                boxGridMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BoxGrid] The maximum green color level (true color)
        /// </summary>
        public int BoxGridMaximumGreenColorLevel
        {
            get
            {
                return boxGridMaximumGreenColorLevel;
            }
            set
            {
                if (value <= boxGridMinimumGreenColorLevel)
                    value = boxGridMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                boxGridMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BoxGrid] The maximum blue color level (true color)
        /// </summary>
        public int BoxGridMaximumBlueColorLevel
        {
            get
            {
                return boxGridMaximumBlueColorLevel;
            }
            set
            {
                if (value <= boxGridMinimumBlueColorLevel)
                    value = boxGridMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                boxGridMaximumBlueColorLevel = value;
            }
        }
        #endregion

        #region ColorBleed
        private bool colorBleedTrueColor = true;
        private int colorBleedDelay = 10;
        private int colorBleedMaxSteps = 25;
        private int colorBleedDropChance = 40;
        private int colorBleedMinimumRedColorLevel = 0;
        private int colorBleedMinimumGreenColorLevel = 0;
        private int colorBleedMinimumBlueColorLevel = 0;
        private int colorBleedMinimumColorLevel = 0;
        private int colorBleedMaximumRedColorLevel = 255;
        private int colorBleedMaximumGreenColorLevel = 255;
        private int colorBleedMaximumBlueColorLevel = 255;
        private int colorBleedMaximumColorLevel = 255;

        /// <summary>
        /// [ColorBleed] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool ColorBleedTrueColor
        {
            get
            {
                return colorBleedTrueColor;
            }
            set
            {
                colorBleedTrueColor = value;
            }
        }
        /// <summary>
        /// [ColorBleed] How many milliseconds to wait before making the next write?
        /// </summary>
        public int ColorBleedDelay
        {
            get
            {
                return colorBleedDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                colorBleedDelay = value;
            }
        }
        /// <summary>
        /// [ColorBleed] How many fade steps to do?
        /// </summary>
        public int ColorBleedMaxSteps
        {
            get
            {
                return colorBleedMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                colorBleedMaxSteps = value;
            }
        }
        /// <summary>
        /// [ColorBleed] Chance to drop a new falling color
        /// </summary>
        public int ColorBleedDropChance
        {
            get
            {
                return colorBleedDropChance;
            }
            set
            {
                if (value <= 0)
                    value = 40;
                colorBleedDropChance = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The minimum red color level (true color)
        /// </summary>
        public int ColorBleedMinimumRedColorLevel
        {
            get
            {
                return colorBleedMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                colorBleedMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The minimum green color level (true color)
        /// </summary>
        public int ColorBleedMinimumGreenColorLevel
        {
            get
            {
                return colorBleedMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                colorBleedMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The minimum blue color level (true color)
        /// </summary>
        public int ColorBleedMinimumBlueColorLevel
        {
            get
            {
                return colorBleedMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                colorBleedMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int ColorBleedMinimumColorLevel
        {
            get
            {
                return colorBleedMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                colorBleedMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The maximum red color level (true color)
        /// </summary>
        public int ColorBleedMaximumRedColorLevel
        {
            get
            {
                return colorBleedMaximumRedColorLevel;
            }
            set
            {
                if (value <= colorBleedMinimumRedColorLevel)
                    value = colorBleedMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                colorBleedMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The maximum green color level (true color)
        /// </summary>
        public int ColorBleedMaximumGreenColorLevel
        {
            get
            {
                return colorBleedMaximumGreenColorLevel;
            }
            set
            {
                if (value <= colorBleedMinimumGreenColorLevel)
                    value = colorBleedMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                colorBleedMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The maximum blue color level (true color)
        /// </summary>
        public int ColorBleedMaximumBlueColorLevel
        {
            get
            {
                return colorBleedMaximumBlueColorLevel;
            }
            set
            {
                if (value <= colorBleedMinimumBlueColorLevel)
                    value = colorBleedMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                colorBleedMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int ColorBleedMaximumColorLevel
        {
            get
            {
                return colorBleedMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= colorBleedMinimumColorLevel)
                    value = colorBleedMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                colorBleedMaximumColorLevel = value;
            }
        }
        #endregion

        #region Text
        private bool textTrueColor = true;
        private int textDelay = 1000;
        private string textWrite = "Nitrocid KS";
        private bool textRainbowMode;
        private int textMinimumRedColorLevel = 0;
        private int textMinimumGreenColorLevel = 0;
        private int textMinimumBlueColorLevel = 0;
        private int textMinimumColorLevel = 0;
        private int textMaximumRedColorLevel = 255;
        private int textMaximumGreenColorLevel = 255;
        private int textMaximumBlueColorLevel = 255;
        private int textMaximumColorLevel = 255;

        /// <summary>
        /// [Text] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool TextTrueColor
        {
            get
            {
                return textTrueColor;
            }
            set
            {
                textTrueColor = value;
            }
        }
        /// <summary>
        /// [Text] How many milliseconds to wait before making the next write?
        /// </summary>
        public int TextDelay
        {
            get
            {
                return textDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                textDelay = value;
            }
        }
        /// <summary>
        /// [Text] Text for Bouncing Text. Shorter is better.
        /// </summary>
        public string TextWrite
        {
            get
            {
                return textWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                textWrite = value;
            }
        }
        /// <summary>
        /// [Text] Enables the rainbow colors mode
        /// </summary>
        public bool TextRainbowMode
        {
            get
            {
                return textRainbowMode;
            }
            set
            {
                textRainbowMode = value;
            }
        }
        /// <summary>
        /// [Text] The minimum red color level (true color)
        /// </summary>
        public int TextMinimumRedColorLevel
        {
            get
            {
                return textMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                textMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Text] The minimum green color level (true color)
        /// </summary>
        public int TextMinimumGreenColorLevel
        {
            get
            {
                return textMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                textMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Text] The minimum blue color level (true color)
        /// </summary>
        public int TextMinimumBlueColorLevel
        {
            get
            {
                return textMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                textMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Text] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int TextMinimumColorLevel
        {
            get
            {
                return textMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                textMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Text] The maximum red color level (true color)
        /// </summary>
        public int TextMaximumRedColorLevel
        {
            get
            {
                return textMaximumRedColorLevel;
            }
            set
            {
                if (value <= textMinimumRedColorLevel)
                    value = textMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                textMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Text] The maximum green color level (true color)
        /// </summary>
        public int TextMaximumGreenColorLevel
        {
            get
            {
                return textMaximumGreenColorLevel;
            }
            set
            {
                if (value <= textMinimumGreenColorLevel)
                    value = textMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                textMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Text] The maximum blue color level (true color)
        /// </summary>
        public int TextMaximumBlueColorLevel
        {
            get
            {
                return textMaximumBlueColorLevel;
            }
            set
            {
                if (value <= textMinimumBlueColorLevel)
                    value = textMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                textMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Text] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int TextMaximumColorLevel
        {
            get
            {
                return textMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= textMinimumColorLevel)
                    value = textMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                textMaximumColorLevel = value;
            }
        }
        #endregion

        #region TextBox
        private bool textBoxTrueColor = true;
        private int textBoxDelay = 1000;
        private string textBoxWrite = "Nitrocid KS";
        private bool textBoxRainbowMode;
        private int textBoxMinimumRedColorLevel = 0;
        private int textBoxMinimumGreenColorLevel = 0;
        private int textBoxMinimumBlueColorLevel = 0;
        private int textBoxMinimumColorLevel = 0;
        private int textBoxMaximumRedColorLevel = 255;
        private int textBoxMaximumGreenColorLevel = 255;
        private int textBoxMaximumBlueColorLevel = 255;
        private int textBoxMaximumColorLevel = 255;

        /// <summary>
        /// [TextBox] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool TextBoxTrueColor
        {
            get
            {
                return textBoxTrueColor;
            }
            set
            {
                textBoxTrueColor = value;
            }
        }
        /// <summary>
        /// [TextBox] How many milliseconds to wait before making the next write?
        /// </summary>
        public int TextBoxDelay
        {
            get
            {
                return textBoxDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                textBoxDelay = value;
            }
        }
        /// <summary>
        /// [TextBox] TextBox for Bouncing TextBox. Shorter is better.
        /// </summary>
        public string TextBoxWrite
        {
            get
            {
                return textBoxWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                textBoxWrite = value;
            }
        }
        /// <summary>
        /// [TextBox] Enables the rainbow colors mode
        /// </summary>
        public bool TextBoxRainbowMode
        {
            get
            {
                return textBoxRainbowMode;
            }
            set
            {
                textBoxRainbowMode = value;
            }
        }
        /// <summary>
        /// [TextBox] The minimum red color level (true color)
        /// </summary>
        public int TextBoxMinimumRedColorLevel
        {
            get
            {
                return textBoxMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                textBoxMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [TextBox] The minimum green color level (true color)
        /// </summary>
        public int TextBoxMinimumGreenColorLevel
        {
            get
            {
                return textBoxMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                textBoxMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [TextBox] The minimum blue color level (true color)
        /// </summary>
        public int TextBoxMinimumBlueColorLevel
        {
            get
            {
                return textBoxMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                textBoxMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [TextBox] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int TextBoxMinimumColorLevel
        {
            get
            {
                return textBoxMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                textBoxMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [TextBox] The maximum red color level (true color)
        /// </summary>
        public int TextBoxMaximumRedColorLevel
        {
            get
            {
                return textBoxMaximumRedColorLevel;
            }
            set
            {
                if (value <= textBoxMinimumRedColorLevel)
                    value = textBoxMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                textBoxMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [TextBox] The maximum green color level (true color)
        /// </summary>
        public int TextBoxMaximumGreenColorLevel
        {
            get
            {
                return textBoxMaximumGreenColorLevel;
            }
            set
            {
                if (value <= textBoxMinimumGreenColorLevel)
                    value = textBoxMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                textBoxMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [TextBox] The maximum blue color level (true color)
        /// </summary>
        public int TextBoxMaximumBlueColorLevel
        {
            get
            {
                return textBoxMaximumBlueColorLevel;
            }
            set
            {
                if (value <= textBoxMinimumBlueColorLevel)
                    value = textBoxMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                textBoxMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [TextBox] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int TextBoxMaximumColorLevel
        {
            get
            {
                return textBoxMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= textBoxMinimumColorLevel)
                    value = textBoxMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                textBoxMaximumColorLevel = value;
            }
        }
        #endregion

        #region WordHasherWrite
        private bool wordHasherWriteTrueColor = true;
        private int wordHasherWriteDelay = 1000;
        private int wordHasherWriteMinimumRedColorLevel = 0;
        private int wordHasherWriteMinimumGreenColorLevel = 0;
        private int wordHasherWriteMinimumBlueColorLevel = 0;
        private int wordHasherWriteMinimumColorLevel = 0;
        private int wordHasherWriteMaximumRedColorLevel = 255;
        private int wordHasherWriteMaximumGreenColorLevel = 255;
        private int wordHasherWriteMaximumBlueColorLevel = 255;
        private int wordHasherWriteMaximumColorLevel = 255;

        /// <summary>
        /// [WordHasherWrite] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool WordHasherWriteTrueColor
        {
            get
            {
                return wordHasherWriteTrueColor;
            }
            set
            {
                wordHasherWriteTrueColor = value;
            }
        }
        /// <summary>
        /// [WordHasherWrite] How many milliseconds to wait before making the next write?
        /// </summary>
        public int WordHasherWriteDelay
        {
            get
            {
                return wordHasherWriteDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                wordHasherWriteDelay = value;
            }
        }
        /// <summary>
        /// [WordHasherWrite] The minimum red color level (true color)
        /// </summary>
        public int WordHasherWriteMinimumRedColorLevel
        {
            get
            {
                return wordHasherWriteMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                wordHasherWriteMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasherWrite] The minimum green color level (true color)
        /// </summary>
        public int WordHasherWriteMinimumGreenColorLevel
        {
            get
            {
                return wordHasherWriteMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                wordHasherWriteMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasherWrite] The minimum blue color level (true color)
        /// </summary>
        public int WordHasherWriteMinimumBlueColorLevel
        {
            get
            {
                return wordHasherWriteMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                wordHasherWriteMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasherWrite] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int WordHasherWriteMinimumColorLevel
        {
            get
            {
                return wordHasherWriteMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                wordHasherWriteMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasherWrite] The maximum red color level (true color)
        /// </summary>
        public int WordHasherWriteMaximumRedColorLevel
        {
            get
            {
                return wordHasherWriteMaximumRedColorLevel;
            }
            set
            {
                if (value <= wordHasherWriteMinimumRedColorLevel)
                    value = wordHasherWriteMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                wordHasherWriteMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasherWrite] The maximum green color level (true color)
        /// </summary>
        public int WordHasherWriteMaximumGreenColorLevel
        {
            get
            {
                return wordHasherWriteMaximumGreenColorLevel;
            }
            set
            {
                if (value <= wordHasherWriteMinimumGreenColorLevel)
                    value = wordHasherWriteMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                wordHasherWriteMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasherWrite] The maximum blue color level (true color)
        /// </summary>
        public int WordHasherWriteMaximumBlueColorLevel
        {
            get
            {
                return wordHasherWriteMaximumBlueColorLevel;
            }
            set
            {
                if (value <= wordHasherWriteMinimumBlueColorLevel)
                    value = wordHasherWriteMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                wordHasherWriteMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasherWrite] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int WordHasherWriteMaximumColorLevel
        {
            get
            {
                return wordHasherWriteMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= wordHasherWriteMinimumColorLevel)
                    value = wordHasherWriteMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                wordHasherWriteMaximumColorLevel = value;
            }
        }
        #endregion

        #region SirenTheme
        private int sirenThemeDelay = 500;
        private string sirenThemeStyle = "Default";

        /// <summary>
        /// [SirenTheme] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SirenThemeDelay
        {
            get
            {
                return sirenThemeDelay;
            }
            set
            {
                if (value <= 0)
                    value = 500;
                sirenThemeDelay = value;
            }
        }

        /// <summary>
        /// [SirenTheme] The siren style
        /// </summary>
        public string SirenThemeStyle
        {
            get
            {
                return sirenThemeStyle;
            }
            set
            {
                sirenThemeStyle = ThemeTools.GetInstalledThemes().ContainsKey(value) ? value : "Default";
            }
        }
        #endregion

        #region StarfieldWarp
        private int starfieldWarpDelay = 10;

        /// <summary>
        /// [StarfieldWarp] How many milliseconds to wait before making the next write?
        /// </summary>
        public int StarfieldWarpDelay
        {
            get
            {
                return starfieldWarpDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                starfieldWarpDelay = value;
            }
        }
        #endregion

        #region Speckles
        private int specklesDelay = 10;

        /// <summary>
        /// [Speckles] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SpecklesDelay
        {
            get
            {
                return specklesDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                specklesDelay = value;
            }
        }
        #endregion

        #region LetterScatter
        private int letterScatterDelay = 1;
        private string letterScatterBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private string letterScatterForegroundColor = new Color(ConsoleColors.Green).PlainSequence;

        /// <summary>
        /// [LetterScatter] How many milliseconds to wait before making the next write?
        /// </summary>
        public int LetterScatterDelay
        {
            get
            {
                return letterScatterDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                letterScatterDelay = value;
            }
        }
        /// <summary>
        /// [LetterScatter] Screensaver background color
        /// </summary>
        public string LetterScatterBackgroundColor
        {
            get
            {
                return letterScatterBackgroundColor;
            }
            set
            {
                letterScatterBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [LetterScatter] Screensaver foreground color
        /// </summary>
        public string LetterScatterForegroundColor
        {
            get
            {
                return letterScatterForegroundColor;
            }
            set
            {
                letterScatterForegroundColor = new Color(value).PlainSequence;
            }
        }
        #endregion

        #region MultiLines
        private bool multiLinesTrueColor = true;
        private int multiLinesDelay = 500;
        private string multiLinesLineChar = "-";
        private string multiLinesBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private int multiLinesMinimumRedColorLevel = 0;
        private int multiLinesMinimumGreenColorLevel = 0;
        private int multiLinesMinimumBlueColorLevel = 0;
        private int multiLinesMinimumColorLevel = 0;
        private int multiLinesMaximumRedColorLevel = 255;
        private int multiLinesMaximumGreenColorLevel = 255;
        private int multiLinesMaximumBlueColorLevel = 255;
        private int multiLinesMaximumColorLevel = 255;

        /// <summary>
        /// [MultiLines] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool MultiLinesTrueColor
        {
            get
            {
                return multiLinesTrueColor;
            }
            set
            {
                multiLinesTrueColor = value;
            }
        }
        /// <summary>
        /// [MultiLines] How many milliseconds to wait before making the next write?
        /// </summary>
        public int MultiLinesDelay
        {
            get
            {
                return multiLinesDelay;
            }
            set
            {
                if (value <= 0)
                    value = 500;
                multiLinesDelay = value;
            }
        }
        /// <summary>
        /// [MultiLines] Line character
        /// </summary>
        public string MultiLinesLineChar
        {
            get
            {
                return multiLinesLineChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "-";
                multiLinesLineChar = value;
            }
        }
        /// <summary>
        /// [MultiLines] Screensaver background color
        /// </summary>
        public string MultiLinesBackgroundColor
        {
            get
            {
                return multiLinesBackgroundColor;
            }
            set
            {
                multiLinesBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [MultiLines] The minimum red color level (true color)
        /// </summary>
        public int MultiLinesMinimumRedColorLevel
        {
            get
            {
                return multiLinesMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                multiLinesMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [MultiLines] The minimum green color level (true color)
        /// </summary>
        public int MultiLinesMinimumGreenColorLevel
        {
            get
            {
                return multiLinesMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                multiLinesMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [MultiLines] The minimum blue color level (true color)
        /// </summary>
        public int MultiLinesMinimumBlueColorLevel
        {
            get
            {
                return multiLinesMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                multiLinesMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [MultiLines] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int MultiLinesMinimumColorLevel
        {
            get
            {
                return multiLinesMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                multiLinesMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [MultiLines] The maximum red color level (true color)
        /// </summary>
        public int MultiLinesMaximumRedColorLevel
        {
            get
            {
                return multiLinesMaximumRedColorLevel;
            }
            set
            {
                if (value <= multiLinesMinimumRedColorLevel)
                    value = multiLinesMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                multiLinesMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [MultiLines] The maximum green color level (true color)
        /// </summary>
        public int MultiLinesMaximumGreenColorLevel
        {
            get
            {
                return multiLinesMaximumGreenColorLevel;
            }
            set
            {
                if (value <= multiLinesMinimumGreenColorLevel)
                    value = multiLinesMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                multiLinesMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [MultiLines] The maximum blue color level (true color)
        /// </summary>
        public int MultiLinesMaximumBlueColorLevel
        {
            get
            {
                return multiLinesMaximumBlueColorLevel;
            }
            set
            {
                if (value <= multiLinesMinimumBlueColorLevel)
                    value = multiLinesMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                multiLinesMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [MultiLines] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int MultiLinesMaximumColorLevel
        {
            get
            {
                return multiLinesMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= multiLinesMinimumColorLevel)
                    value = multiLinesMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                multiLinesMaximumColorLevel = value;
            }
        }
        #endregion

        #region LaserBeams
        private bool laserBeamsTrueColor = true;
        private int laserBeamsDelay = 500;
        private string laserBeamsLineChar = "-";
        private string laserBeamsBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private int laserBeamsMinimumRedColorLevel = 0;
        private int laserBeamsMinimumGreenColorLevel = 0;
        private int laserBeamsMinimumBlueColorLevel = 0;
        private int laserBeamsMinimumColorLevel = 0;
        private int laserBeamsMaximumRedColorLevel = 255;
        private int laserBeamsMaximumGreenColorLevel = 255;
        private int laserBeamsMaximumBlueColorLevel = 255;
        private int laserBeamsMaximumColorLevel = 255;

        /// <summary>
        /// [LaserBeams] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool LaserBeamsTrueColor
        {
            get
            {
                return laserBeamsTrueColor;
            }
            set
            {
                laserBeamsTrueColor = value;
            }
        }
        /// <summary>
        /// [LaserBeams] How many milliseconds to wait before making the next write?
        /// </summary>
        public int LaserBeamsDelay
        {
            get
            {
                return laserBeamsDelay;
            }
            set
            {
                if (value <= 0)
                    value = 500;
                laserBeamsDelay = value;
            }
        }
        /// <summary>
        /// [LaserBeams] Line character
        /// </summary>
        public string LaserBeamsLineChar
        {
            get
            {
                return laserBeamsLineChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "-";
                laserBeamsLineChar = value;
            }
        }
        /// <summary>
        /// [LaserBeams] Screensaver background color
        /// </summary>
        public string LaserBeamsBackgroundColor
        {
            get
            {
                return laserBeamsBackgroundColor;
            }
            set
            {
                laserBeamsBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [LaserBeams] The minimum red color level (true color)
        /// </summary>
        public int LaserBeamsMinimumRedColorLevel
        {
            get
            {
                return laserBeamsMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                laserBeamsMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [LaserBeams] The minimum green color level (true color)
        /// </summary>
        public int LaserBeamsMinimumGreenColorLevel
        {
            get
            {
                return laserBeamsMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                laserBeamsMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [LaserBeams] The minimum blue color level (true color)
        /// </summary>
        public int LaserBeamsMinimumBlueColorLevel
        {
            get
            {
                return laserBeamsMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                laserBeamsMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [LaserBeams] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int LaserBeamsMinimumColorLevel
        {
            get
            {
                return laserBeamsMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                laserBeamsMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [LaserBeams] The maximum red color level (true color)
        /// </summary>
        public int LaserBeamsMaximumRedColorLevel
        {
            get
            {
                return laserBeamsMaximumRedColorLevel;
            }
            set
            {
                if (value <= laserBeamsMinimumRedColorLevel)
                    value = laserBeamsMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                laserBeamsMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [LaserBeams] The maximum green color level (true color)
        /// </summary>
        public int LaserBeamsMaximumGreenColorLevel
        {
            get
            {
                return laserBeamsMaximumGreenColorLevel;
            }
            set
            {
                if (value <= laserBeamsMinimumGreenColorLevel)
                    value = laserBeamsMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                laserBeamsMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [LaserBeams] The maximum blue color level (true color)
        /// </summary>
        public int LaserBeamsMaximumBlueColorLevel
        {
            get
            {
                return laserBeamsMaximumBlueColorLevel;
            }
            set
            {
                if (value <= laserBeamsMinimumBlueColorLevel)
                    value = laserBeamsMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                laserBeamsMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [LaserBeams] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int LaserBeamsMaximumColorLevel
        {
            get
            {
                return laserBeamsMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= laserBeamsMinimumColorLevel)
                    value = laserBeamsMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                laserBeamsMaximumColorLevel = value;
            }
        }
        #endregion

        #region TextWander
        private bool textWanderTrueColor = true;
        private int textWanderDelay = 1000;
        private string textWanderWrite = "Nitrocid KS";
        private int textWanderMinimumRedColorLevel = 0;
        private int textWanderMinimumGreenColorLevel = 0;
        private int textWanderMinimumBlueColorLevel = 0;
        private int textWanderMinimumColorLevel = 0;
        private int textWanderMaximumRedColorLevel = 255;
        private int textWanderMaximumGreenColorLevel = 255;
        private int textWanderMaximumBlueColorLevel = 255;
        private int textWanderMaximumColorLevel = 255;

        /// <summary>
        /// [TextWander] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool TextWanderTrueColor
        {
            get
            {
                return textWanderTrueColor;
            }
            set
            {
                textWanderTrueColor = value;
            }
        }
        /// <summary>
        /// [TextWander] How many milliseconds to wait before making the next write?
        /// </summary>
        public int TextWanderDelay
        {
            get
            {
                return textWanderDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                textWanderDelay = value;
            }
        }
        /// <summary>
        /// [TextWander] TextWander for Bouncing TextWander. Shorter is better.
        /// </summary>
        public string TextWanderWrite
        {
            get
            {
                return textWanderWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                textWanderWrite = value;
            }
        }
        /// <summary>
        /// [TextWander] The minimum red color level (true color)
        /// </summary>
        public int TextWanderMinimumRedColorLevel
        {
            get
            {
                return textWanderMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                textWanderMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [TextWander] The minimum green color level (true color)
        /// </summary>
        public int TextWanderMinimumGreenColorLevel
        {
            get
            {
                return textWanderMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                textWanderMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [TextWander] The minimum blue color level (true color)
        /// </summary>
        public int TextWanderMinimumBlueColorLevel
        {
            get
            {
                return textWanderMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                textWanderMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [TextWander] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int TextWanderMinimumColorLevel
        {
            get
            {
                return textWanderMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                textWanderMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [TextWander] The maximum red color level (true color)
        /// </summary>
        public int TextWanderMaximumRedColorLevel
        {
            get
            {
                return textWanderMaximumRedColorLevel;
            }
            set
            {
                if (value <= textWanderMinimumRedColorLevel)
                    value = textWanderMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                textWanderMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [TextWander] The maximum green color level (true color)
        /// </summary>
        public int TextWanderMaximumGreenColorLevel
        {
            get
            {
                return textWanderMaximumGreenColorLevel;
            }
            set
            {
                if (value <= textWanderMinimumGreenColorLevel)
                    value = textWanderMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                textWanderMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [TextWander] The maximum blue color level (true color)
        /// </summary>
        public int TextWanderMaximumBlueColorLevel
        {
            get
            {
                return textWanderMaximumBlueColorLevel;
            }
            set
            {
                if (value <= textWanderMinimumBlueColorLevel)
                    value = textWanderMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                textWanderMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [TextWander] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int TextWanderMaximumColorLevel
        {
            get
            {
                return textWanderMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= textWanderMinimumColorLevel)
                    value = textWanderMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                textWanderMaximumColorLevel = value;
            }
        }
        #endregion

        #region Swivel
        private int swivelDelay = 100;
        private double swivelHorizontalFrequencyLevel = 3;
        private double swivelVerticalFrequencyLevel = 8;
        private int swivelMinimumRedColorLevel = 0;
        private int swivelMinimumGreenColorLevel = 0;
        private int swivelMinimumBlueColorLevel = 0;
        private int swivelMinimumColorLevel = 0;
        private int swivelMaximumRedColorLevel = 255;
        private int swivelMaximumGreenColorLevel = 255;
        private int swivelMaximumBlueColorLevel = 255;
        private int swivelMaximumColorLevel = 255;

        /// <summary>
        /// [Swivel] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SwivelDelay
        {
            get
            {
                return swivelDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                swivelDelay = value;
            }
        }
        /// <summary>
        /// [Swivel] The level of the horizontal frequency. This is the denominator of the Pi value (3.1415926...) in mathematics, defined by <see cref="Math.PI"/>. Use this to create beautiful wavy swivels!
        /// </summary>
        public double SwivelHorizontalFrequencyLevel
        {
            get
            {
                return swivelHorizontalFrequencyLevel;
            }
            set
            {
                if (value <= 0)
                    value = 3;
                swivelHorizontalFrequencyLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The level of the vertical frequency. This is the denominator of the Pi value (3.1415926...) in mathematics, defined by <see cref="Math.PI"/>. Use this to create beautiful wavy swivels!
        /// </summary>
        public double SwivelVerticalFrequencyLevel
        {
            get
            {
                return swivelVerticalFrequencyLevel;
            }
            set
            {
                if (value <= 0)
                    value = 8;
                swivelVerticalFrequencyLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The minimum red color level (true color)
        /// </summary>
        public int SwivelMinimumRedColorLevel
        {
            get
            {
                return swivelMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                swivelMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The minimum green color level (true color)
        /// </summary>
        public int SwivelMinimumGreenColorLevel
        {
            get
            {
                return swivelMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                swivelMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The minimum blue color level (true color)
        /// </summary>
        public int SwivelMinimumBlueColorLevel
        {
            get
            {
                return swivelMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                swivelMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int SwivelMinimumColorLevel
        {
            get
            {
                return swivelMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                swivelMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The maximum red color level (true color)
        /// </summary>
        public int SwivelMaximumRedColorLevel
        {
            get
            {
                return swivelMaximumRedColorLevel;
            }
            set
            {
                if (value <= swivelMaximumRedColorLevel)
                    value = swivelMaximumRedColorLevel;
                if (value > 255)
                    value = 255;
                swivelMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The maximum green color level (true color)
        /// </summary>
        public int SwivelMaximumGreenColorLevel
        {
            get
            {
                return swivelMaximumGreenColorLevel;
            }
            set
            {
                if (value <= swivelMaximumGreenColorLevel)
                    value = swivelMaximumGreenColorLevel;
                if (value > 255)
                    value = 255;
                swivelMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The maximum blue color level (true color)
        /// </summary>
        public int SwivelMaximumBlueColorLevel
        {
            get
            {
                return swivelMaximumBlueColorLevel;
            }
            set
            {
                if (value <= swivelMaximumBlueColorLevel)
                    value = swivelMaximumBlueColorLevel;
                if (value > 255)
                    value = 255;
                swivelMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int SwivelMaximumColorLevel
        {
            get
            {
                return swivelMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= swivelMaximumColorLevel)
                    value = swivelMaximumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                swivelMaximumColorLevel = value;
            }
        }
        #endregion

        #region DoorShift
        private bool doorShiftTrueColor = true;
        private int doorShiftDelay = 10;
        private string doorShiftBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private int doorShiftMinimumRedColorLevel = 0;
        private int doorShiftMinimumGreenColorLevel = 0;
        private int doorShiftMinimumBlueColorLevel = 0;
        private int doorShiftMinimumColorLevel = 0;
        private int doorShiftMaximumRedColorLevel = 255;
        private int doorShiftMaximumGreenColorLevel = 255;
        private int doorShiftMaximumBlueColorLevel = 255;
        private int doorShiftMaximumColorLevel = 255;

        /// <summary>
        /// [DoorShift] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool DoorShiftTrueColor
        {
            get
            {
                return doorShiftTrueColor;
            }
            set
            {
                doorShiftTrueColor = value;
            }
        }
        /// <summary>
        /// [DoorShift] How many milliseconds to wait before making the next write?
        /// </summary>
        public int DoorShiftDelay
        {
            get
            {
                return doorShiftDelay;
            }
            set
            {
                doorShiftDelay = value;
            }
        }
        /// <summary>
        /// [DoorShift] Screensaver background color
        /// </summary>
        public string DoorShiftBackgroundColor
        {
            get
            {
                return doorShiftBackgroundColor;
            }
            set
            {
                doorShiftBackgroundColor = value;
            }
        }
        /// <summary>
        /// [DoorShift] The minimum red color level (true color)
        /// </summary>
        public int DoorShiftMinimumRedColorLevel
        {
            get
            {
                return doorShiftMinimumRedColorLevel;
            }
            set
            {
                doorShiftMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [DoorShift] The minimum green color level (true color)
        /// </summary>
        public int DoorShiftMinimumGreenColorLevel
        {
            get
            {
                return doorShiftMinimumGreenColorLevel;
            }
            set
            {
                doorShiftMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [DoorShift] The minimum blue color level (true color)
        /// </summary>
        public int DoorShiftMinimumBlueColorLevel
        {
            get
            {
                return doorShiftMinimumBlueColorLevel;
            }
            set
            {
                doorShiftMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [DoorShift] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int DoorShiftMinimumColorLevel
        {
            get
            {
                return doorShiftMinimumColorLevel;
            }
            set
            {
                doorShiftMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [DoorShift] The maximum red color level (true color)
        /// </summary>
        public int DoorShiftMaximumRedColorLevel
        {
            get
            {
                return doorShiftMaximumRedColorLevel;
            }
            set
            {
                doorShiftMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [DoorShift] The maximum green color level (true color)
        /// </summary>
        public int DoorShiftMaximumGreenColorLevel
        {
            get
            {
                return doorShiftMaximumGreenColorLevel;
            }
            set
            {
                doorShiftMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [DoorShift] The maximum blue color level (true color)
        /// </summary>
        public int DoorShiftMaximumBlueColorLevel
        {
            get
            {
                return doorShiftMaximumBlueColorLevel;
            }
            set
            {
                doorShiftMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [DoorShift] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int DoorShiftMaximumColorLevel
        {
            get
            {
                return doorShiftMaximumColorLevel;
            }
            set
            {
                doorShiftMaximumColorLevel = value;
            }
        }
        #endregion

        #region GradientBloom
        private int gradientBloomDelay = 50;
        private bool gradientBloomDarkColors;
        private int gradientBloomSteps = 100;

        /// <summary>
        /// [GradientBloom] How many milliseconds to wait before making the next write?
        /// </summary>
        public int GradientBloomDelay
        {
            get
            {
                return gradientBloomDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                gradientBloomDelay = value;
            }
        }
        /// <summary>
        /// [GradientBloom] Whether to use dark colors or not
        /// </summary>
        public bool GradientBloomDarkColors
        {
            get
            {
                return gradientBloomDarkColors;
            }
            set
            {
                gradientBloomDarkColors = value;
            }
        }
        /// <summary>
        /// [GradientBloom] How many color steps for transitioning between two colors?
        /// </summary>
        public int GradientBloomSteps
        {
            get
            {
                return gradientBloomSteps;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                gradientBloomSteps = value;
            }
        }
        #endregion

        #region SkyComet
        private int skyCometDelay = 10;

        /// <summary>
        /// [SkyComet] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SkyCometDelay
        {
            get
            {
                return skyCometDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                skyCometDelay = value;
            }
        }
        #endregion

        #region Diamond
        private int diamondDelay = 500;

        /// <summary>
        /// [Diamond] How many milliseconds to wait before making the next write?
        /// </summary>
        public int DiamondDelay
        {
            get
            {
                return diamondDelay;
            }
            set
            {
                if (value <= 0)
                    value = 500;
                diamondDelay = value;
            }
        }
        #endregion

        #region HueBack
        private int hueBackDelay = 50;
        private int hueBackSaturation = 100;
        private int hueBackLuminance = 50;

        /// <summary>
        /// [HueBack] How many milliseconds to wait before making the next write?
        /// </summary>
        public int HueBackDelay
        {
            get
            {
                return hueBackDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                hueBackDelay = value;
            }
        }
        /// <summary>
        /// [HueBack] How intense is the color?
        /// </summary>
        public int HueBackSaturation
        {
            get
            {
                return hueBackSaturation;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                if (value > 100)
                    value = 100;
                hueBackSaturation = value;
            }
        }
        /// <summary>
        /// [HueBack] How light is the color?
        /// </summary>
        public int HueBackLuminance
        {
            get
            {
                return hueBackLuminance;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                if (value > 100)
                    value = 100;
                hueBackLuminance = value;
            }
        }
        #endregion

        #region HueBackGradient
        private int hueBackGradientDelay = 50;
        private int hueBackGradientSaturation = 100;
        private int hueBackGradientLuminance = 50;

        /// <summary>
        /// [HueBackGradient] How many milliseconds to wait before making the next write?
        /// </summary>
        public int HueBackGradientDelay
        {
            get
            {
                return hueBackGradientDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                hueBackGradientDelay = value;
            }
        }
        /// <summary>
        /// [HueBackGradient] How intense is the color?
        /// </summary>
        public int HueBackGradientSaturation
        {
            get
            {
                return hueBackGradientSaturation;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                if (value > 100)
                    value = 100;
                hueBackGradientSaturation = value;
            }
        }
        /// <summary>
        /// [HueBackGradient] How light is the color?
        /// </summary>
        public int HueBackGradientLuminance
        {
            get
            {
                return hueBackGradientLuminance;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                if (value > 100)
                    value = 100;
                hueBackGradientLuminance = value;
            }
        }
        #endregion

        #region Particles
        private bool particlesTrueColor = true;
        private int particlesDelay = 1;
        private int particlesDensity = 25;
        private int particlesMinimumRedColorLevel = 0;
        private int particlesMinimumGreenColorLevel = 0;
        private int particlesMinimumBlueColorLevel = 0;
        private int particlesMinimumColorLevel = 0;
        private int particlesMaximumRedColorLevel = 255;
        private int particlesMaximumGreenColorLevel = 255;
        private int particlesMaximumBlueColorLevel = 255;
        private int particlesMaximumColorLevel = 255;

        /// <summary>
        /// [Particles] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool ParticlesTrueColor
        {
            get
            {
                return particlesTrueColor;
            }
            set
            {
                particlesTrueColor = value;
            }
        }
        /// <summary>
        /// [Particles] How many milliseconds to wait before making the next write?
        /// </summary>
        public int ParticlesDelay
        {
            get
            {
                return particlesDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                particlesDelay = value;
            }
        }
        /// <summary>
        /// [Particles] How dense are the particles?
        /// </summary>
        public int ParticlesDensity
        {
            get
            {
                return particlesDensity;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                particlesDensity = value;
            }
        }
        /// <summary>
        /// [Particles] The minimum red color level (true color)
        /// </summary>
        public int ParticlesMinimumRedColorLevel
        {
            get
            {
                return particlesMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                particlesMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Particles] The minimum green color level (true color)
        /// </summary>
        public int ParticlesMinimumGreenColorLevel
        {
            get
            {
                return particlesMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                particlesMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Particles] The minimum blue color level (true color)
        /// </summary>
        public int ParticlesMinimumBlueColorLevel
        {
            get
            {
                return particlesMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                particlesMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Particles] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int ParticlesMinimumColorLevel
        {
            get
            {
                return particlesMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                particlesMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Particles] The maximum red color level (true color)
        /// </summary>
        public int ParticlesMaximumRedColorLevel
        {
            get
            {
                return particlesMaximumRedColorLevel;
            }
            set
            {
                if (value <= particlesMinimumRedColorLevel)
                    value = particlesMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                particlesMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Particles] The maximum green color level (true color)
        /// </summary>
        public int ParticlesMaximumGreenColorLevel
        {
            get
            {
                return particlesMaximumGreenColorLevel;
            }
            set
            {
                if (value <= particlesMinimumGreenColorLevel)
                    value = particlesMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                particlesMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Particles] The maximum blue color level (true color)
        /// </summary>
        public int ParticlesMaximumBlueColorLevel
        {
            get
            {
                return particlesMaximumBlueColorLevel;
            }
            set
            {
                if (value <= particlesMinimumBlueColorLevel)
                    value = particlesMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                particlesMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Particles] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int ParticlesMaximumColorLevel
        {
            get
            {
                return particlesMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= particlesMinimumColorLevel)
                    value = particlesMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                particlesMaximumColorLevel = value;
            }
        }
        #endregion

        #region WorldClock
        private bool worldClockTrueColor = true;
        private int worldClockDelay = 1000;
        private int worldClockNextZoneRefreshes = 5;
        private int worldClockMinimumRedColorLevel = 0;
        private int worldClockMinimumGreenColorLevel = 0;
        private int worldClockMinimumBlueColorLevel = 0;
        private int worldClockMinimumColorLevel = 0;
        private int worldClockMaximumRedColorLevel = 255;
        private int worldClockMaximumGreenColorLevel = 255;
        private int worldClockMaximumBlueColorLevel = 255;
        private int worldClockMaximumColorLevel = 255;

        /// <summary>
        /// [WorldClock] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool WorldClockTrueColor
        {
            get
            {
                return worldClockTrueColor;
            }
            set
            {
                worldClockTrueColor = value;
            }
        }
        /// <summary>
        /// [WorldClock] How many milliseconds to wait before making the next write?
        /// </summary>
        public int WorldClockDelay
        {
            get
            {
                return worldClockDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                worldClockDelay = value;
            }
        }
        /// <summary>
        /// [WorldClock] How many refreshes before making the next write?
        /// </summary>
        public int WorldClockNextZoneRefreshes
        {
            get
            {
                return worldClockNextZoneRefreshes;
            }
            set
            {
                if (value <= 0)
                    value = 5;
                worldClockNextZoneRefreshes = value;
            }
        }
        /// <summary>
        /// [WorldClock] The minimum red color level (true color)
        /// </summary>
        public int WorldClockMinimumRedColorLevel
        {
            get
            {
                return worldClockMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                worldClockMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [WorldClock] The minimum green color level (true color)
        /// </summary>
        public int WorldClockMinimumGreenColorLevel
        {
            get
            {
                return worldClockMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                worldClockMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [WorldClock] The minimum blue color level (true color)
        /// </summary>
        public int WorldClockMinimumBlueColorLevel
        {
            get
            {
                return worldClockMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                worldClockMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [WorldClock] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int WorldClockMinimumColorLevel
        {
            get
            {
                return worldClockMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                worldClockMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [WorldClock] The maximum red color level (true color)
        /// </summary>
        public int WorldClockMaximumRedColorLevel
        {
            get
            {
                return worldClockMaximumRedColorLevel;
            }
            set
            {
                if (value <= worldClockMinimumRedColorLevel)
                    value = worldClockMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                worldClockMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [WorldClock] The maximum green color level (true color)
        /// </summary>
        public int WorldClockMaximumGreenColorLevel
        {
            get
            {
                return worldClockMaximumGreenColorLevel;
            }
            set
            {
                if (value <= worldClockMinimumGreenColorLevel)
                    value = worldClockMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                worldClockMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [WorldClock] The maximum blue color level (true color)
        /// </summary>
        public int WorldClockMaximumBlueColorLevel
        {
            get
            {
                return worldClockMaximumBlueColorLevel;
            }
            set
            {
                if (value <= worldClockMinimumBlueColorLevel)
                    value = worldClockMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                worldClockMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [WorldClock] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int WorldClockMaximumColorLevel
        {
            get
            {
                return worldClockMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= worldClockMinimumColorLevel)
                    value = worldClockMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                worldClockMaximumColorLevel = value;
            }
        }
        #endregion

        #region FillFade
        private bool fillFadeTrueColor = true;
        private int fillFadeMinimumRedColorLevel = 0;
        private int fillFadeMinimumGreenColorLevel = 0;
        private int fillFadeMinimumBlueColorLevel = 0;
        private int fillFadeMinimumColorLevel = 0;
        private int fillFadeMaximumGreenColorLevel = 255;
        private int fillFadeMaximumBlueColorLevel = 255;
        private int fillFadeMaximumColorLevel = 255;
        private int fillFadeMaximumRedColorLevel = 255;

        /// <summary>
        /// [FillFade] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool FillFadeTrueColor
        {
            get
            {
                return fillFadeTrueColor;
            }
            set
            {
                fillFadeTrueColor = value;
            }
        }
        /// <summary>
        /// [FillFade] The minimum red color level (true color)
        /// </summary>
        public int FillFadeMinimumRedColorLevel
        {
            get
            {
                return fillFadeMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                fillFadeMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FillFade] The minimum green color level (true color)
        /// </summary>
        public int FillFadeMinimumGreenColorLevel
        {
            get
            {
                return fillFadeMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                fillFadeMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FillFade] The minimum blue color level (true color)
        /// </summary>
        public int FillFadeMinimumBlueColorLevel
        {
            get
            {
                return fillFadeMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                fillFadeMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FillFade] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int FillFadeMinimumColorLevel
        {
            get
            {
                return fillFadeMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                fillFadeMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [FillFade] The maximum red color level (true color)
        /// </summary>
        public int FillFadeMaximumRedColorLevel
        {
            get
            {
                return fillFadeMaximumRedColorLevel;
            }
            set
            {
                if (value <= fillFadeMinimumRedColorLevel)
                    value = fillFadeMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                fillFadeMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FillFade] The maximum green color level (true color)
        /// </summary>
        public int FillFadeMaximumGreenColorLevel
        {
            get
            {
                return fillFadeMaximumGreenColorLevel;
            }
            set
            {
                if (value <= fillFadeMinimumGreenColorLevel)
                    value = fillFadeMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                fillFadeMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FillFade] The maximum blue color level (true color)
        /// </summary>
        public int FillFadeMaximumBlueColorLevel
        {
            get
            {
                return fillFadeMaximumBlueColorLevel;
            }
            set
            {
                if (value <= fillFadeMinimumBlueColorLevel)
                    value = fillFadeMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                fillFadeMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FillFade] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int FillFadeMaximumColorLevel
        {
            get
            {
                return fillFadeMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= fillFadeMinimumColorLevel)
                    value = fillFadeMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                fillFadeMaximumColorLevel = value;
            }
        }
        #endregion

        #region DanceLines
        private bool danceLinesTrueColor = true;
        private int danceLinesDelay = 50;
        private string danceLinesLineChar = "-";
        private string danceLinesBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private int danceLinesMinimumRedColorLevel = 0;
        private int danceLinesMinimumGreenColorLevel = 0;
        private int danceLinesMinimumBlueColorLevel = 0;
        private int danceLinesMinimumColorLevel = 0;
        private int danceLinesMaximumRedColorLevel = 255;
        private int danceLinesMaximumGreenColorLevel = 255;
        private int danceLinesMaximumBlueColorLevel = 255;
        private int danceLinesMaximumColorLevel = 255;

        /// <summary>
        /// [DanceLines] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool DanceLinesTrueColor
        {
            get
            {
                return danceLinesTrueColor;
            }
            set
            {
                danceLinesTrueColor = value;
            }
        }
        /// <summary>
        /// [DanceLines] How many milliseconds to wait before making the next write?
        /// </summary>
        public int DanceLinesDelay
        {
            get
            {
                return danceLinesDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                danceLinesDelay = value;
            }
        }
        /// <summary>
        /// [DanceLines] Line character
        /// </summary>
        public string DanceLinesLineChar
        {
            get
            {
                return danceLinesLineChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "-";
                danceLinesLineChar = value;
            }
        }
        /// <summary>
        /// [DanceLines] Screensaver background color
        /// </summary>
        public string DanceLinesBackgroundColor
        {
            get
            {
                return danceLinesBackgroundColor;
            }
            set
            {
                danceLinesBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [DanceLines] The minimum red color level (true color)
        /// </summary>
        public int DanceLinesMinimumRedColorLevel
        {
            get
            {
                return danceLinesMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                danceLinesMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceLines] The minimum green color level (true color)
        /// </summary>
        public int DanceLinesMinimumGreenColorLevel
        {
            get
            {
                return danceLinesMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                danceLinesMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceLines] The minimum blue color level (true color)
        /// </summary>
        public int DanceLinesMinimumBlueColorLevel
        {
            get
            {
                return danceLinesMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                danceLinesMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceLines] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int DanceLinesMinimumColorLevel
        {
            get
            {
                return danceLinesMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                danceLinesMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceLines] The maximum red color level (true color)
        /// </summary>
        public int DanceLinesMaximumRedColorLevel
        {
            get
            {
                return danceLinesMaximumRedColorLevel;
            }
            set
            {
                if (value <= danceLinesMinimumRedColorLevel)
                    value = danceLinesMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                danceLinesMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceLines] The maximum green color level (true color)
        /// </summary>
        public int DanceLinesMaximumGreenColorLevel
        {
            get
            {
                return danceLinesMaximumGreenColorLevel;
            }
            set
            {
                if (value <= danceLinesMinimumGreenColorLevel)
                    value = danceLinesMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                danceLinesMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceLines] The maximum blue color level (true color)
        /// </summary>
        public int DanceLinesMaximumBlueColorLevel
        {
            get
            {
                return danceLinesMaximumBlueColorLevel;
            }
            set
            {
                if (value <= danceLinesMinimumBlueColorLevel)
                    value = danceLinesMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                danceLinesMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceLines] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int DanceLinesMaximumColorLevel
        {
            get
            {
                return danceLinesMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= danceLinesMinimumColorLevel)
                    value = danceLinesMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                danceLinesMaximumColorLevel = value;
            }
        }
        #endregion

        #region Mazer
        private int mazerNewMazeDelay = 10000;
        private int mazerGenerationSpeed = 20;
        private bool mazerHighlightUncovered = false;
        private bool mazerUseSchwartzian = true;

        /// <summary>
        /// [Mazer] How many milliseconds to wait before generating a new maze?
        /// </summary>
        public int MazerNewMazeDelay
        {
            get
            {
                return mazerNewMazeDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10000;
                mazerNewMazeDelay = value;
            }
        }
        /// <summary>
        /// [Mazer] Maze generation speed in milliseconds
        /// </summary>
        public int MazerGenerationSpeed
        {
            get
            {
                return mazerGenerationSpeed;
            }
            set
            {
                if (value <= 0)
                    value = 20;
                mazerGenerationSpeed = value;
            }
        }
        /// <summary>
        /// [Mazer] If enabled, highlights the non-covered positions with the gray background color. Otherwise, they render as boxes.
        /// </summary>
        public bool MazerHighlightUncovered
        {
            get => mazerHighlightUncovered;
            set => mazerHighlightUncovered = value;
        }
        /// <summary>
        /// [Mazer] Specifies whether to choose the <seealso href="http://en.wikipedia.org/wiki/Schwartzian_transform">Schwartzian transform</seealso> or to use <see cref="Random.Shuffle{T}(T[])"/>
        /// </summary>
        public bool MazerUseSchwartzian
        {
            get => mazerUseSchwartzian;
            set => mazerUseSchwartzian = value;
        }
        #endregion

        #region TwoSpins
        private bool twoSpinsTrueColor = true;
        private int twoSpinsDelay = 25;
        private int twoSpinsMinimumRedColorLevel = 0;
        private int twoSpinsMinimumGreenColorLevel = 0;
        private int twoSpinsMinimumBlueColorLevel = 0;
        private int twoSpinsMinimumColorLevel = 0;
        private int twoSpinsMaximumRedColorLevel = 255;
        private int twoSpinsMaximumGreenColorLevel = 255;
        private int twoSpinsMaximumBlueColorLevel = 255;
        private int twoSpinsMaximumColorLevel = 255;

        /// <summary>
        /// [TwoSpins] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool TwoSpinsTrueColor
        {
            get
            {
                return twoSpinsTrueColor;
            }
            set
            {
                twoSpinsTrueColor = value;
            }
        }
        /// <summary>
        /// [TwoSpins] How many milliseconds to wait before making the next write?
        /// </summary>
        public int TwoSpinsDelay
        {
            get
            {
                return twoSpinsDelay;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                twoSpinsDelay = value;
            }
        }
        /// <summary>
        /// [TwoSpins] The minimum red color level (true color)
        /// </summary>
        public int TwoSpinsMinimumRedColorLevel
        {
            get
            {
                return twoSpinsMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                twoSpinsMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [TwoSpins] The minimum green color level (true color)
        /// </summary>
        public int TwoSpinsMinimumGreenColorLevel
        {
            get
            {
                return twoSpinsMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                twoSpinsMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [TwoSpins] The minimum blue color level (true color)
        /// </summary>
        public int TwoSpinsMinimumBlueColorLevel
        {
            get
            {
                return twoSpinsMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                twoSpinsMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [TwoSpins] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int TwoSpinsMinimumColorLevel
        {
            get
            {
                return twoSpinsMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                twoSpinsMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [TwoSpins] The maximum red color level (true color)
        /// </summary>
        public int TwoSpinsMaximumRedColorLevel
        {
            get
            {
                return twoSpinsMaximumRedColorLevel;
            }
            set
            {
                if (value <= twoSpinsMinimumRedColorLevel)
                    value = twoSpinsMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                twoSpinsMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [TwoSpins] The maximum green color level (true color)
        /// </summary>
        public int TwoSpinsMaximumGreenColorLevel
        {
            get
            {
                return twoSpinsMaximumGreenColorLevel;
            }
            set
            {
                if (value <= twoSpinsMinimumGreenColorLevel)
                    value = twoSpinsMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                twoSpinsMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [TwoSpins] The maximum blue color level (true color)
        /// </summary>
        public int TwoSpinsMaximumBlueColorLevel
        {
            get
            {
                return twoSpinsMaximumBlueColorLevel;
            }
            set
            {
                if (value <= twoSpinsMinimumBlueColorLevel)
                    value = twoSpinsMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                twoSpinsMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [TwoSpins] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int TwoSpinsMaximumColorLevel
        {
            get
            {
                return twoSpinsMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= twoSpinsMinimumColorLevel)
                    value = twoSpinsMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                twoSpinsMaximumColorLevel = value;
            }
        }
        #endregion

        #region CommitMilestone
        private bool commitMilestoneTrueColor = true;
        private int commitMilestoneDelay = 1000;
        private bool commitMilestoneRainbowMode;
        private int commitMilestoneMinimumRedColorLevel = 0;
        private int commitMilestoneMinimumGreenColorLevel = 0;
        private int commitMilestoneMinimumBlueColorLevel = 0;
        private int commitMilestoneMinimumColorLevel = 0;
        private int commitMilestoneMaximumRedColorLevel = 255;
        private int commitMilestoneMaximumGreenColorLevel = 255;
        private int commitMilestoneMaximumBlueColorLevel = 255;
        private int commitMilestoneMaximumColorLevel = 255;

        /// <summary>
        /// [CommitMilestone] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool CommitMilestoneTrueColor
        {
            get
            {
                return commitMilestoneTrueColor;
            }
            set
            {
                commitMilestoneTrueColor = value;
            }
        }
        /// <summary>
        /// [CommitMilestone] How many milliseconds to wait before making the next write?
        /// </summary>
        public int CommitMilestoneDelay
        {
            get
            {
                return commitMilestoneDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                commitMilestoneDelay = value;
            }
        }
        /// <summary>
        /// [CommitMilestone] Enables the rainbow colors mode
        /// </summary>
        public bool CommitMilestoneRainbowMode
        {
            get
            {
                return commitMilestoneRainbowMode;
            }
            set
            {
                commitMilestoneRainbowMode = value;
            }
        }
        /// <summary>
        /// [CommitMilestone] The minimum red color level (true color)
        /// </summary>
        public int CommitMilestoneMinimumRedColorLevel
        {
            get
            {
                return commitMilestoneMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                commitMilestoneMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [CommitMilestone] The minimum green color level (true color)
        /// </summary>
        public int CommitMilestoneMinimumGreenColorLevel
        {
            get
            {
                return commitMilestoneMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                commitMilestoneMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [CommitMilestone] The minimum blue color level (true color)
        /// </summary>
        public int CommitMilestoneMinimumBlueColorLevel
        {
            get
            {
                return commitMilestoneMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                commitMilestoneMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [CommitMilestone] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int CommitMilestoneMinimumColorLevel
        {
            get
            {
                return commitMilestoneMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                commitMilestoneMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [CommitMilestone] The maximum red color level (true color)
        /// </summary>
        public int CommitMilestoneMaximumRedColorLevel
        {
            get
            {
                return commitMilestoneMaximumRedColorLevel;
            }
            set
            {
                if (value <= commitMilestoneMinimumRedColorLevel)
                    value = commitMilestoneMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                commitMilestoneMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [CommitMilestone] The maximum green color level (true color)
        /// </summary>
        public int CommitMilestoneMaximumGreenColorLevel
        {
            get
            {
                return commitMilestoneMaximumGreenColorLevel;
            }
            set
            {
                if (value <= commitMilestoneMinimumGreenColorLevel)
                    value = commitMilestoneMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                commitMilestoneMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [CommitMilestone] The maximum blue color level (true color)
        /// </summary>
        public int CommitMilestoneMaximumBlueColorLevel
        {
            get
            {
                return commitMilestoneMaximumBlueColorLevel;
            }
            set
            {
                if (value <= commitMilestoneMinimumBlueColorLevel)
                    value = commitMilestoneMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                commitMilestoneMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [CommitMilestone] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int CommitMilestoneMaximumColorLevel
        {
            get
            {
                return commitMilestoneMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= commitMilestoneMinimumColorLevel)
                    value = commitMilestoneMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                commitMilestoneMaximumColorLevel = value;
            }
        }
        #endregion

        #region Spray
        private int sprayDelay = 10;

        /// <summary>
        /// [Spray] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SprayDelay
        {
            get
            {
                return sprayDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                sprayDelay = value;
            }
        }
        #endregion

        #region ZebraShift
        private bool zebraShiftTrueColor = true;
        private int zebraShiftDelay = 25;
        private int zebraShiftMinimumRedColorLevel = 0;
        private int zebraShiftMinimumGreenColorLevel = 0;
        private int zebraShiftMinimumBlueColorLevel = 0;
        private int zebraShiftMinimumColorLevel = 0;
        private int zebraShiftMaximumRedColorLevel = 255;
        private int zebraShiftMaximumGreenColorLevel = 255;
        private int zebraShiftMaximumBlueColorLevel = 255;
        private int zebraShiftMaximumColorLevel = 255;

        /// <summary>
        /// [ZebraShift] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool ZebraShiftTrueColor
        {
            get
            {
                return zebraShiftTrueColor;
            }
            set
            {
                zebraShiftTrueColor = value;
            }
        }
        /// <summary>
        /// [ZebraShift] How many milliseconds to wait before making the next write?
        /// </summary>
        public int ZebraShiftDelay
        {
            get
            {
                return zebraShiftDelay;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                zebraShiftDelay = value;
            }
        }
        /// <summary>
        /// [ZebraShift] The minimum red color level (true color)
        /// </summary>
        public int ZebraShiftMinimumRedColorLevel
        {
            get
            {
                return zebraShiftMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                zebraShiftMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ZebraShift] The minimum green color level (true color)
        /// </summary>
        public int ZebraShiftMinimumGreenColorLevel
        {
            get
            {
                return zebraShiftMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                zebraShiftMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ZebraShift] The minimum blue color level (true color)
        /// </summary>
        public int ZebraShiftMinimumBlueColorLevel
        {
            get
            {
                return zebraShiftMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                zebraShiftMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ZebraShift] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int ZebraShiftMinimumColorLevel
        {
            get
            {
                return zebraShiftMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                zebraShiftMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [ZebraShift] The maximum red color level (true color)
        /// </summary>
        public int ZebraShiftMaximumRedColorLevel
        {
            get
            {
                return zebraShiftMaximumRedColorLevel;
            }
            set
            {
                if (value <= zebraShiftMinimumRedColorLevel)
                    value = zebraShiftMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                zebraShiftMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ZebraShift] The maximum green color level (true color)
        /// </summary>
        public int ZebraShiftMaximumGreenColorLevel
        {
            get
            {
                return zebraShiftMaximumGreenColorLevel;
            }
            set
            {
                if (value <= zebraShiftMinimumGreenColorLevel)
                    value = zebraShiftMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                zebraShiftMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ZebraShift] The maximum blue color level (true color)
        /// </summary>
        public int ZebraShiftMaximumBlueColorLevel
        {
            get
            {
                return zebraShiftMaximumBlueColorLevel;
            }
            set
            {
                if (value <= zebraShiftMinimumBlueColorLevel)
                    value = zebraShiftMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                zebraShiftMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ZebraShift] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int ZebraShiftMaximumColorLevel
        {
            get
            {
                return zebraShiftMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= zebraShiftMinimumColorLevel)
                    value = zebraShiftMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                zebraShiftMaximumColorLevel = value;
            }
        }
        #endregion

        #region AnalogClock
        private bool analogClockTrueColor = true;
        private int analogClockDelay = 1000;
        private bool analogClockShowSecondsHand = true;
        private int analogClockMinimumRedColorLevel = 0;
        private int analogClockMinimumGreenColorLevel = 0;
        private int analogClockMinimumBlueColorLevel = 0;
        private int analogClockMinimumColorLevel = 0;
        private int analogClockMaximumRedColorLevel = 255;
        private int analogClockMaximumGreenColorLevel = 255;
        private int analogClockMaximumBlueColorLevel = 255;
        private int analogClockMaximumColorLevel = 255;

        /// <summary>
        /// [AnalogClock] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool AnalogClockTrueColor
        {
            get
            {
                return analogClockTrueColor;
            }
            set
            {
                analogClockTrueColor = value;
            }
        }
        /// <summary>
        /// [AnalogClock] How many milliseconds to wait before making the next write?
        /// </summary>
        public int AnalogClockDelay
        {
            get
            {
                return analogClockDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                analogClockDelay = value;
            }
        }
        /// <summary>
        /// [AnalogClock] Shows the seconds hand.
        /// </summary>
        public bool AnalogClockShowSecondsHand
        {
            get
            {
                return analogClockShowSecondsHand;
            }
            set
            {
                analogClockShowSecondsHand = value;
            }
        }
        /// <summary>
        /// [AnalogClock] The minimum red color level (true color)
        /// </summary>
        public int AnalogClockMinimumRedColorLevel
        {
            get
            {
                return analogClockMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                analogClockMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [AnalogClock] The minimum green color level (true color)
        /// </summary>
        public int AnalogClockMinimumGreenColorLevel
        {
            get
            {
                return analogClockMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                analogClockMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [AnalogClock] The minimum blue color level (true color)
        /// </summary>
        public int AnalogClockMinimumBlueColorLevel
        {
            get
            {
                return analogClockMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                analogClockMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [AnalogClock] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int AnalogClockMinimumColorLevel
        {
            get
            {
                return analogClockMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                analogClockMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [AnalogClock] The maximum red color level (true color)
        /// </summary>
        public int AnalogClockMaximumRedColorLevel
        {
            get
            {
                return analogClockMaximumRedColorLevel;
            }
            set
            {
                if (value <= analogClockMinimumRedColorLevel)
                    value = analogClockMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                analogClockMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [AnalogClock] The maximum green color level (true color)
        /// </summary>
        public int AnalogClockMaximumGreenColorLevel
        {
            get
            {
                return analogClockMaximumGreenColorLevel;
            }
            set
            {
                if (value <= analogClockMinimumGreenColorLevel)
                    value = analogClockMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                analogClockMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [AnalogClock] The maximum blue color level (true color)
        /// </summary>
        public int AnalogClockMaximumBlueColorLevel
        {
            get
            {
                return analogClockMaximumBlueColorLevel;
            }
            set
            {
                if (value <= analogClockMinimumBlueColorLevel)
                    value = analogClockMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                analogClockMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [AnalogClock] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int AnalogClockMaximumColorLevel
        {
            get
            {
                return analogClockMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= analogClockMinimumColorLevel)
                    value = analogClockMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                analogClockMaximumColorLevel = value;
            }
        }
        #endregion

        #region Following
        private bool followingTrueColor = true;
        private int followingDelay = 100;
        private int followingMinimumRedColorLevel = 0;
        private int followingMinimumGreenColorLevel = 0;
        private int followingMinimumBlueColorLevel = 0;
        private int followingMinimumColorLevel = 0;
        private int followingMaximumRedColorLevel = 255;
        private int followingMaximumGreenColorLevel = 255;
        private int followingMaximumBlueColorLevel = 255;
        private int followingMaximumColorLevel = 255;

        /// <summary>
        /// [Following] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool FollowingTrueColor
        {
            get
            {
                return followingTrueColor;
            }
            set
            {
                followingTrueColor = value;
            }
        }
        /// <summary>
        /// [Following] How many milliseconds to wait before making the next write?
        /// </summary>
        public int FollowingDelay
        {
            get
            {
                return followingDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                followingDelay = value;
            }
        }
        /// <summary>
        /// [Following] The minimum red color level (true color)
        /// </summary>
        public int FollowingMinimumRedColorLevel
        {
            get
            {
                return followingMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                followingMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Following] The minimum green color level (true color)
        /// </summary>
        public int FollowingMinimumGreenColorLevel
        {
            get
            {
                return followingMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                followingMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Following] The minimum blue color level (true color)
        /// </summary>
        public int FollowingMinimumBlueColorLevel
        {
            get
            {
                return followingMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                followingMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Following] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int FollowingMinimumColorLevel
        {
            get
            {
                return followingMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                followingMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Following] The maximum red color level (true color)
        /// </summary>
        public int FollowingMaximumRedColorLevel
        {
            get
            {
                return followingMaximumRedColorLevel;
            }
            set
            {
                if (value <= followingMaximumRedColorLevel)
                    value = followingMaximumRedColorLevel;
                if (value > 255)
                    value = 255;
                followingMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Following] The maximum green color level (true color)
        /// </summary>
        public int FollowingMaximumGreenColorLevel
        {
            get
            {
                return followingMaximumGreenColorLevel;
            }
            set
            {
                if (value <= followingMaximumGreenColorLevel)
                    value = followingMaximumGreenColorLevel;
                if (value > 255)
                    value = 255;
                followingMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Following] The maximum blue color level (true color)
        /// </summary>
        public int FollowingMaximumBlueColorLevel
        {
            get
            {
                return followingMaximumBlueColorLevel;
            }
            set
            {
                if (value <= followingMaximumBlueColorLevel)
                    value = followingMaximumBlueColorLevel;
                if (value > 255)
                    value = 255;
                followingMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Following] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int FollowingMaximumColorLevel
        {
            get
            {
                return followingMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= followingMaximumColorLevel)
                    value = followingMaximumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                followingMaximumColorLevel = value;
            }
        }
        #endregion

        #region Aberration
        private int aberrationDelay = 100;
        private int aberrationProbability = 5;

        /// <summary>
        /// [Aberration] How many milliseconds to wait before making the next write?
        /// </summary>
        public int AberrationDelay
        {
            get
            {
                return aberrationDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                aberrationDelay = value;
            }
        }
        /// <summary>
        /// [Aberration] Chance, in percent, for the screen glitch to occur
        /// </summary>
        public int AberrationProbability
        {
            get
            {
                return aberrationProbability;
            }
            set
            {
                if (value <= 0)
                    value = 5;
                aberrationProbability = value;
            }
        }
        #endregion

        #region Omen
        private int omenDelay = 100;
        private string omenWrite = "Nitrocid KS";
        private int omenMaximumBackColorLevel = 32;
        private int omenMaximumLineColorLevel = 64;
        private int omenMaximumTextColorLevel = 128;

        /// <summary>
        /// [Omen] How many milliseconds to wait before making the next write?
        /// </summary>
        public int OmenDelay
        {
            get
            {
                return omenDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                omenDelay = value;
            }
        }
        /// <summary>
        /// [Omen] Text for Omen. Shorter is better.
        /// </summary>
        public string OmenWrite
        {
            get
            {
                return omenWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                omenWrite = value;
            }
        }
        /// <summary>
        /// [Omen] Maximum background color level to use
        /// </summary>
        public int OmenMaximumBackColorLevel
        {
            get
            {
                return omenMaximumBackColorLevel;
            }
            set
            {
                if (value <= 0 || value > 255)
                    value = 32;
                omenMaximumBackColorLevel = value;
            }
        }
        /// <summary>
        /// [Omen] Maximum line color level to use
        /// </summary>
        public int OmenMaximumLineColorLevel
        {
            get
            {
                return omenMaximumLineColorLevel;
            }
            set
            {
                if (value <= 0 || value > 255)
                    value = 64;
                omenMaximumLineColorLevel = value;
            }
        }
        /// <summary>
        /// [Omen] Maximum text color level to use
        /// </summary>
        public int OmenMaximumTextColorLevel
        {
            get
            {
                return omenMaximumTextColorLevel;
            }
            set
            {
                if (value <= 0 || value > 255)
                    value = 64;
                omenMaximumTextColorLevel = value;
            }
        }
        #endregion

        #region GlitterChar
        private int glitterCharDelay = 1;
        private string glitterCharBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private string glitterCharForegroundColor = new Color(ConsoleColors.Green).PlainSequence;

        /// <summary>
        /// [GlitterChar] How many milliseconds to wait before making the next write?
        /// </summary>
        public int GlitterCharDelay
        {
            get
            {
                return glitterCharDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                glitterCharDelay = value;
            }
        }
        /// <summary>
        /// [GlitterChar] Screensaver background color
        /// </summary>
        public string GlitterCharBackgroundColor
        {
            get
            {
                return glitterCharBackgroundColor;
            }
            set
            {
                glitterCharBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [GlitterChar] Screensaver foreground color
        /// </summary>
        public string GlitterCharForegroundColor
        {
            get
            {
                return glitterCharForegroundColor;
            }
            set
            {
                glitterCharForegroundColor = new Color(value).PlainSequence;
            }
        }
        #endregion

        #region Clochroma
        private bool clochromaBright = false;
        private int clochromaDelay = 1000;

        /// <summary>
        /// [Clochroma] Whether to use bright or dark version.
        /// </summary>
        public bool ClochromaBright
        {
            get
            {
                return clochromaBright;
            }
            set
            {
                clochromaBright = value;
            }
        }
        /// <summary>
        /// [Clochroma] How many milliseconds to wait before making the next write?
        /// </summary>
        public int ClochromaDelay
        {
            get
            {
                return clochromaDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                clochromaDelay = value;
            }
        }
        #endregion

        #region DanceNumbers
        private bool danceNumbersTrueColor = true;
        private int danceNumbersDelay = 50;
        private string danceNumbersBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private int danceNumbersMinimumRedColorLevel = 0;
        private int danceNumbersMinimumGreenColorLevel = 0;
        private int danceNumbersMinimumBlueColorLevel = 0;
        private int danceNumbersMinimumColorLevel = 0;
        private int danceNumbersMaximumRedColorLevel = 255;
        private int danceNumbersMaximumGreenColorLevel = 255;
        private int danceNumbersMaximumBlueColorLevel = 255;
        private int danceNumbersMaximumColorLevel = 255;

        /// <summary>
        /// [DanceNumbers] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool DanceNumbersTrueColor
        {
            get
            {
                return danceNumbersTrueColor;
            }
            set
            {
                danceNumbersTrueColor = value;
            }
        }
        /// <summary>
        /// [DanceNumbers] How many milliseconds to wait before making the next write?
        /// </summary>
        public int DanceNumbersDelay
        {
            get
            {
                return danceNumbersDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                danceNumbersDelay = value;
            }
        }
        /// <summary>
        /// [DanceNumbers] Screensaver background color
        /// </summary>
        public string DanceNumbersBackgroundColor
        {
            get
            {
                return danceNumbersBackgroundColor;
            }
            set
            {
                danceNumbersBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [DanceNumbers] The minimum red color level (true color)
        /// </summary>
        public int DanceNumbersMinimumRedColorLevel
        {
            get
            {
                return danceNumbersMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                danceNumbersMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceNumbers] The minimum green color level (true color)
        /// </summary>
        public int DanceNumbersMinimumGreenColorLevel
        {
            get
            {
                return danceNumbersMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                danceNumbersMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceNumbers] The minimum blue color level (true color)
        /// </summary>
        public int DanceNumbersMinimumBlueColorLevel
        {
            get
            {
                return danceNumbersMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                danceNumbersMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceNumbers] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int DanceNumbersMinimumColorLevel
        {
            get
            {
                return danceNumbersMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                danceNumbersMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceNumbers] The maximum red color level (true color)
        /// </summary>
        public int DanceNumbersMaximumRedColorLevel
        {
            get
            {
                return danceNumbersMaximumRedColorLevel;
            }
            set
            {
                if (value <= danceNumbersMinimumRedColorLevel)
                    value = danceNumbersMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                danceNumbersMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceNumbers] The maximum green color level (true color)
        /// </summary>
        public int DanceNumbersMaximumGreenColorLevel
        {
            get
            {
                return danceNumbersMaximumGreenColorLevel;
            }
            set
            {
                if (value <= danceNumbersMinimumGreenColorLevel)
                    value = danceNumbersMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                danceNumbersMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceNumbers] The maximum blue color level (true color)
        /// </summary>
        public int DanceNumbersMaximumBlueColorLevel
        {
            get
            {
                return danceNumbersMaximumBlueColorLevel;
            }
            set
            {
                if (value <= danceNumbersMinimumBlueColorLevel)
                    value = danceNumbersMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                danceNumbersMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceNumbers] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int DanceNumbersMaximumColorLevel
        {
            get
            {
                return danceNumbersMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= danceNumbersMinimumColorLevel)
                    value = danceNumbersMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                danceNumbersMaximumColorLevel = value;
            }
        }
        #endregion

        #region Trails
        private int trailsDelay = 100;
        private double trailsHorizontalFrequencyLevel = 3;
        private double trailsVerticalFrequencyLevel = 8;
        private double trailsTrailLength = 10;
        private int trailsMinimumRedColorLevel = 0;
        private int trailsMinimumGreenColorLevel = 0;
        private int trailsMinimumBlueColorLevel = 0;
        private int trailsMinimumColorLevel = 0;
        private int trailsMaximumRedColorLevel = 255;
        private int trailsMaximumGreenColorLevel = 255;
        private int trailsMaximumBlueColorLevel = 255;
        private int trailsMaximumColorLevel = 255;

        /// <summary>
        /// [Trails] How many milliseconds to wait before making the next write?
        /// </summary>
        public int TrailsDelay
        {
            get
            {
                return trailsDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                trailsDelay = value;
            }
        }
        /// <summary>
        /// [Trails] The level of the horizontal frequency. This is the denominator of the Pi value (3.1415926...) in mathematics, defined by <see cref="Math.PI"/>. Use this to create beautiful wavy swivels!
        /// </summary>
        public double TrailsHorizontalFrequencyLevel
        {
            get
            {
                return trailsHorizontalFrequencyLevel;
            }
            set
            {
                if (value <= 0)
                    value = 3;
                trailsHorizontalFrequencyLevel = value;
            }
        }
        /// <summary>
        /// [Trails] The level of the vertical frequency. This is the denominator of the Pi value (3.1415926...) in mathematics, defined by <see cref="Math.PI"/>. Use this to create beautiful wavy swivels!
        /// </summary>
        public double TrailsVerticalFrequencyLevel
        {
            get
            {
                return trailsVerticalFrequencyLevel;
            }
            set
            {
                if (value <= 0)
                    value = 8;
                trailsVerticalFrequencyLevel = value;
            }
        }
        /// <summary>
        /// [Trails] The length of the trail
        /// </summary>
        public double TrailsTrailLength
        {
            get
            {
                return trailsTrailLength;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                trailsTrailLength = value;
            }
        }
        /// <summary>
        /// [Trails] The minimum red color level (true color)
        /// </summary>
        public int TrailsMinimumRedColorLevel
        {
            get
            {
                return trailsMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                trailsMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Trails] The minimum green color level (true color)
        /// </summary>
        public int TrailsMinimumGreenColorLevel
        {
            get
            {
                return trailsMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                trailsMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Trails] The minimum blue color level (true color)
        /// </summary>
        public int TrailsMinimumBlueColorLevel
        {
            get
            {
                return trailsMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                trailsMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Trails] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int TrailsMinimumColorLevel
        {
            get
            {
                return trailsMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                trailsMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Trails] The maximum red color level (true color)
        /// </summary>
        public int TrailsMaximumRedColorLevel
        {
            get
            {
                return trailsMaximumRedColorLevel;
            }
            set
            {
                if (value <= trailsMaximumRedColorLevel)
                    value = trailsMaximumRedColorLevel;
                if (value > 255)
                    value = 255;
                trailsMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Trails] The maximum green color level (true color)
        /// </summary>
        public int TrailsMaximumGreenColorLevel
        {
            get
            {
                return trailsMaximumGreenColorLevel;
            }
            set
            {
                if (value <= trailsMaximumGreenColorLevel)
                    value = trailsMaximumGreenColorLevel;
                if (value > 255)
                    value = 255;
                trailsMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Trails] The maximum blue color level (true color)
        /// </summary>
        public int TrailsMaximumBlueColorLevel
        {
            get
            {
                return trailsMaximumBlueColorLevel;
            }
            set
            {
                if (value <= trailsMaximumBlueColorLevel)
                    value = trailsMaximumBlueColorLevel;
                if (value > 255)
                    value = 255;
                trailsMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Trails] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int TrailsMaximumColorLevel
        {
            get
            {
                return trailsMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= trailsMaximumColorLevel)
                    value = trailsMaximumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                trailsMaximumColorLevel = value;
            }
        }
        #endregion

        #region PointTrack
        private int pointTrackDelay = 100;
        private double pointTrackHorizontalFrequencyLevel = 3;
        private double pointTrackVerticalFrequencyLevel = 8;
        private int pointTrackMinimumRedColorLevel = 0;
        private int pointTrackMinimumGreenColorLevel = 0;
        private int pointTrackMinimumBlueColorLevel = 0;
        private int pointTrackMinimumColorLevel = 0;
        private int pointTrackMaximumRedColorLevel = 255;
        private int pointTrackMaximumGreenColorLevel = 255;
        private int pointTrackMaximumBlueColorLevel = 255;
        private int pointTrackMaximumColorLevel = 255;

        /// <summary>
        /// [PointTrack] How many milliseconds to wait before making the next write?
        /// </summary>
        public int PointTrackDelay
        {
            get
            {
                return pointTrackDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                pointTrackDelay = value;
            }
        }
        /// <summary>
        /// [PointTrack] The level of the horizontal frequency. This is the denominator of the Pi value (3.1415926...) in mathematics, defined by <see cref="Math.PI"/>. Use this to create beautiful wavy swivels!
        /// </summary>
        public double PointTrackHorizontalFrequencyLevel
        {
            get
            {
                return pointTrackHorizontalFrequencyLevel;
            }
            set
            {
                if (value <= 0)
                    value = 3;
                pointTrackHorizontalFrequencyLevel = value;
            }
        }
        /// <summary>
        /// [PointTrack] The level of the vertical frequency. This is the denominator of the Pi value (3.1415926...) in mathematics, defined by <see cref="Math.PI"/>. Use this to create beautiful wavy swivels!
        /// </summary>
        public double PointTrackVerticalFrequencyLevel
        {
            get
            {
                return pointTrackVerticalFrequencyLevel;
            }
            set
            {
                if (value <= 0)
                    value = 8;
                pointTrackVerticalFrequencyLevel = value;
            }
        }
        /// <summary>
        /// [PointTrack] The minimum red color level (true color)
        /// </summary>
        public int PointTrackMinimumRedColorLevel
        {
            get
            {
                return pointTrackMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                pointTrackMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [PointTrack] The minimum green color level (true color)
        /// </summary>
        public int PointTrackMinimumGreenColorLevel
        {
            get
            {
                return pointTrackMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                pointTrackMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [PointTrack] The minimum blue color level (true color)
        /// </summary>
        public int PointTrackMinimumBlueColorLevel
        {
            get
            {
                return pointTrackMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                pointTrackMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [PointTrack] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int PointTrackMinimumColorLevel
        {
            get
            {
                return pointTrackMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                pointTrackMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [PointTrack] The maximum red color level (true color)
        /// </summary>
        public int PointTrackMaximumRedColorLevel
        {
            get
            {
                return pointTrackMaximumRedColorLevel;
            }
            set
            {
                if (value <= pointTrackMaximumRedColorLevel)
                    value = pointTrackMaximumRedColorLevel;
                if (value > 255)
                    value = 255;
                pointTrackMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [PointTrack] The maximum green color level (true color)
        /// </summary>
        public int PointTrackMaximumGreenColorLevel
        {
            get
            {
                return pointTrackMaximumGreenColorLevel;
            }
            set
            {
                if (value <= pointTrackMaximumGreenColorLevel)
                    value = pointTrackMaximumGreenColorLevel;
                if (value > 255)
                    value = 255;
                pointTrackMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [PointTrack] The maximum blue color level (true color)
        /// </summary>
        public int PointTrackMaximumBlueColorLevel
        {
            get
            {
                return pointTrackMaximumBlueColorLevel;
            }
            set
            {
                if (value <= pointTrackMaximumBlueColorLevel)
                    value = pointTrackMaximumBlueColorLevel;
                if (value > 255)
                    value = 255;
                pointTrackMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [PointTrack] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int PointTrackMaximumColorLevel
        {
            get
            {
                return pointTrackMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= pointTrackMaximumColorLevel)
                    value = pointTrackMaximumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                pointTrackMaximumColorLevel = value;
            }
        }
        #endregion

        #region BigLetter
        private bool bigLetterTrueColor = true;
        private int bigLetterDelay = 1000;
        private string bigLetterFont = "small";
        private bool bigLetterRainbowMode;
        private int bigLetterMinimumRedColorLevel = 0;
        private int bigLetterMinimumGreenColorLevel = 0;
        private int bigLetterMinimumBlueColorLevel = 0;
        private int bigLetterMinimumColorLevel = 0;
        private int bigLetterMaximumRedColorLevel = 255;
        private int bigLetterMaximumGreenColorLevel = 255;
        private int bigLetterMaximumBlueColorLevel = 255;
        private int bigLetterMaximumColorLevel = 255;

        /// <summary>
        /// [BigLetter] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool BigLetterTrueColor
        {
            get
            {
                return bigLetterTrueColor;
            }
            set
            {
                bigLetterTrueColor = value;
            }
        }
        /// <summary>
        /// [BigLetter] How many milliseconds to wait before making the next write?
        /// </summary>
        public int BigLetterDelay
        {
            get
            {
                return bigLetterDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                bigLetterDelay = value;
            }
        }
        /// <summary>
        /// [BigLetter] BigLetter font supported by the figlet library used.
        /// </summary>
        public string BigLetterFont
        {
            get
            {
                return bigLetterFont;
            }
            set
            {
                bigLetterFont = FigletTools.GetFigletFonts().ContainsKey(value) ? value : "small";
            }
        }
        /// <summary>
        /// [BigLetter] Enables the rainbow colors mode
        /// </summary>
        public bool BigLetterRainbowMode
        {
            get
            {
                return bigLetterRainbowMode;
            }
            set
            {
                bigLetterRainbowMode = value;
            }
        }
        /// <summary>
        /// [BigLetter] The minimum red color level (true color)
        /// </summary>
        public int BigLetterMinimumRedColorLevel
        {
            get
            {
                return bigLetterMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                bigLetterMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BigLetter] The minimum green color level (true color)
        /// </summary>
        public int BigLetterMinimumGreenColorLevel
        {
            get
            {
                return bigLetterMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                bigLetterMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BigLetter] The minimum blue color level (true color)
        /// </summary>
        public int BigLetterMinimumBlueColorLevel
        {
            get
            {
                return bigLetterMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                bigLetterMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BigLetter] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int BigLetterMinimumColorLevel
        {
            get
            {
                return bigLetterMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                bigLetterMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [BigLetter] The maximum red color level (true color)
        /// </summary>
        public int BigLetterMaximumRedColorLevel
        {
            get
            {
                return bigLetterMaximumRedColorLevel;
            }
            set
            {
                if (value <= bigLetterMinimumRedColorLevel)
                    value = bigLetterMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                bigLetterMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BigLetter] The maximum green color level (true color)
        /// </summary>
        public int BigLetterMaximumGreenColorLevel
        {
            get
            {
                return bigLetterMaximumGreenColorLevel;
            }
            set
            {
                if (value <= bigLetterMinimumGreenColorLevel)
                    value = bigLetterMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                bigLetterMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BigLetter] The maximum blue color level (true color)
        /// </summary>
        public int BigLetterMaximumBlueColorLevel
        {
            get
            {
                return bigLetterMaximumBlueColorLevel;
            }
            set
            {
                if (value <= bigLetterMinimumBlueColorLevel)
                    value = bigLetterMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                bigLetterMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BigLetter] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int BigLetterMaximumColorLevel
        {
            get
            {
                return bigLetterMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= bigLetterMinimumColorLevel)
                    value = bigLetterMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                bigLetterMaximumColorLevel = value;
            }
        }
        #endregion

        #region WordSlot
        private bool wordSlotTrueColor = true;
        private int wordSlotDelay = 1000;
        private int wordSlotMinimumRedColorLevel = 0;
        private int wordSlotMinimumGreenColorLevel = 0;
        private int wordSlotMinimumBlueColorLevel = 0;
        private int wordSlotMinimumColorLevel = 0;
        private int wordSlotMaximumRedColorLevel = 255;
        private int wordSlotMaximumGreenColorLevel = 255;
        private int wordSlotMaximumBlueColorLevel = 255;
        private int wordSlotMaximumColorLevel = 255;

        /// <summary>
        /// [WordSlot] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool WordSlotTrueColor
        {
            get
            {
                return wordSlotTrueColor;
            }
            set
            {
                wordSlotTrueColor = value;
            }
        }
        /// <summary>
        /// [WordSlot] How many milliseconds to wait before making the next write?
        /// </summary>
        public int WordSlotDelay
        {
            get
            {
                return wordSlotDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                wordSlotDelay = value;
            }
        }
        /// <summary>
        /// [WordSlot] The minimum red color level (true color)
        /// </summary>
        public int WordSlotMinimumRedColorLevel
        {
            get
            {
                return wordSlotMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                wordSlotMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [WordSlot] The minimum green color level (true color)
        /// </summary>
        public int WordSlotMinimumGreenColorLevel
        {
            get
            {
                return wordSlotMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                wordSlotMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [WordSlot] The minimum blue color level (true color)
        /// </summary>
        public int WordSlotMinimumBlueColorLevel
        {
            get
            {
                return wordSlotMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                wordSlotMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [WordSlot] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int WordSlotMinimumColorLevel
        {
            get
            {
                return wordSlotMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                wordSlotMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [WordSlot] The maximum red color level (true color)
        /// </summary>
        public int WordSlotMaximumRedColorLevel
        {
            get
            {
                return wordSlotMaximumRedColorLevel;
            }
            set
            {
                if (value <= wordSlotMinimumRedColorLevel)
                    value = wordSlotMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                wordSlotMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [WordSlot] The maximum green color level (true color)
        /// </summary>
        public int WordSlotMaximumGreenColorLevel
        {
            get
            {
                return wordSlotMaximumGreenColorLevel;
            }
            set
            {
                if (value <= wordSlotMinimumGreenColorLevel)
                    value = wordSlotMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                wordSlotMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [WordSlot] The maximum blue color level (true color)
        /// </summary>
        public int WordSlotMaximumBlueColorLevel
        {
            get
            {
                return wordSlotMaximumBlueColorLevel;
            }
            set
            {
                if (value <= wordSlotMinimumBlueColorLevel)
                    value = wordSlotMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                wordSlotMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [WordSlot] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int WordSlotMaximumColorLevel
        {
            get
            {
                return wordSlotMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= wordSlotMinimumColorLevel)
                    value = wordSlotMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                wordSlotMaximumColorLevel = value;
            }
        }
        #endregion

        #region Neons
        private int neonsDelay = 1000;
        private string neonsFont = "small";

        /// <summary>
        /// [Neons] How many milliseconds to wait before making the next write?
        /// </summary>
        public int NeonsDelay
        {
            get
            {
                return neonsDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                neonsDelay = value;
            }
        }
        /// <summary>
        /// [Neons] Neons font supported by the neons library used.
        /// </summary>
        public string NeonsFont
        {
            get
            {
                return neonsFont;
            }
            set
            {
                neonsFont = FigletTools.GetFigletFonts().ContainsKey(value) ? value : "small";
            }
        }
        #endregion

        #region Progresses
        private bool progressesTrueColor = true;
        private bool progressesCycleColors = true;
        private int progressesCycleColorsTicks = 20;
        private string progressesThirdProgressColor = "4";
        private string progressesSecondProgressColor = "5";
        private string progressesFirstProgressColor = "6";
        private string progressesProgressColor = "7";
        private int progressesDelay = 500;
        private char progressesUpperLeftCornerCharFirst = '╭';
        private char progressesUpperLeftCornerCharSecond = '╭';
        private char progressesUpperLeftCornerCharThird = '╭';
        private char progressesUpperRightCornerCharFirst = '╮';
        private char progressesUpperRightCornerCharSecond = '╮';
        private char progressesUpperRightCornerCharThird = '╮';
        private char progressesLowerLeftCornerCharFirst = '╰';
        private char progressesLowerLeftCornerCharSecond = '╰';
        private char progressesLowerLeftCornerCharThird = '╰';
        private char progressesLowerRightCornerCharFirst = '╯';
        private char progressesLowerRightCornerCharSecond = '╯';
        private char progressesLowerRightCornerCharThird = '╯';
        private char progressesUpperFrameCharFirst = '─';
        private char progressesUpperFrameCharSecond = '─';
        private char progressesUpperFrameCharThird = '─';
        private char progressesLowerFrameCharFirst = '─';
        private char progressesLowerFrameCharSecond = '─';
        private char progressesLowerFrameCharThird = '─';
        private char progressesLeftFrameCharFirst = '│';
        private char progressesLeftFrameCharSecond = '│';
        private char progressesLeftFrameCharThird = '│';
        private char progressesRightFrameCharFirst = '│';
        private char progressesRightFrameCharSecond = '│';
        private char progressesRightFrameCharThird = '│';
        private string progressesInfoTextFirst = "";
        private string progressesInfoTextSecond = "";
        private string progressesInfoTextThird = "";
        private int progressesMinimumRedColorLevelFirst = 0;
        private int progressesMinimumGreenColorLevelFirst = 0;
        private int progressesMinimumBlueColorLevelFirst = 0;
        private int progressesMinimumColorLevelFirst = 1;
        private int progressesMaximumRedColorLevelFirst = 255;
        private int progressesMaximumGreenColorLevelFirst = 255;
        private int progressesMaximumBlueColorLevelFirst = 255;
        private int progressesMaximumColorLevelFirst = 255;
        private int progressesMinimumRedColorLevelSecond = 0;
        private int progressesMinimumGreenColorLevelSecond = 0;
        private int progressesMinimumBlueColorLevelSecond = 0;
        private int progressesMinimumColorLevelSecond = 1;
        private int progressesMaximumRedColorLevelSecond = 255;
        private int progressesMaximumGreenColorLevelSecond = 255;
        private int progressesMaximumBlueColorLevelSecond = 255;
        private int progressesMaximumColorLevelSecond = 255;
        private int progressesMinimumRedColorLevelThird = 0;
        private int progressesMinimumGreenColorLevelThird = 0;
        private int progressesMinimumBlueColorLevelThird = 0;
        private int progressesMinimumColorLevelThird = 1;
        private int progressesMaximumRedColorLevelThird = 255;
        private int progressesMaximumGreenColorLevelThird = 255;
        private int progressesMaximumBlueColorLevelThird = 255;
        private int progressesMaximumColorLevelThird = 255;
        private int progressesMinimumRedColorLevel = 0;
        private int progressesMinimumGreenColorLevel = 0;
        private int progressesMinimumBlueColorLevel = 0;
        private int progressesMinimumColorLevel = 1;
        private int progressesMaximumRedColorLevel = 255;
        private int progressesMaximumGreenColorLevel = 255;
        private int progressesMaximumBlueColorLevel = 255;
        private int progressesMaximumColorLevel = 255;

        /// <summary>
        /// [Progresses] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool ProgressesTrueColor
        {
            get => progressesTrueColor;
            set => progressesTrueColor = value;
        }
        /// <summary>
        /// [Progresses] Enable color cycling (uses RNG. If disabled, uses the <see cref="ProgressesThirdProgressColor"/>, <see cref="ProgressesSecondProgressColor"/>, and <see cref="ProgressesFirstProgressColor"/> colors.)
        /// </summary>
        public bool ProgressesCycleColors
        {
            get => progressesCycleColors;
            set => progressesCycleColors = value;
        }
        /// <summary>
        /// [Progresses] The color of third progress bar. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public string ProgressesThirdProgressColor
        {
            get => progressesThirdProgressColor;
            set => progressesThirdProgressColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Progresses] The color of second progress bar. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public string ProgressesSecondProgressColor
        {
            get => progressesSecondProgressColor;
            set => progressesSecondProgressColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Progresses] The color of first progress bar. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public string ProgressesFirstProgressColor
        {
            get => progressesFirstProgressColor;
            set => progressesFirstProgressColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Progresses] The color of date information. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public string ProgressesProgressColor
        {
            get => progressesProgressColor;
            set => progressesProgressColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Progresses] If color cycling is enabled, how many ticks before changing colors? 1 tick = 0.5 third
        /// </summary>
        public long ProgressesCycleColorsTicks
        {
            get => progressesCycleColorsTicks;
            set
            {
                if (value <= 0L)
                    value = 20L;
                progressesCycleColorsTicks = (int)value;
            }
        }
        /// <summary>
        /// [Progresses] How many milliseconds to wait before making the next write?
        /// </summary>
        public int ProgressesDelay
        {
            get => progressesDelay;
            set
            {
                if (value <= 0)
                    value = 500;
                progressesDelay = value;
            }
        }
        /// <summary>
        /// [Progresses] Upper left corner character for the first bar
        /// </summary>
        public char ProgressesUpperLeftCornerCharFirst
        {
            get => progressesUpperLeftCornerCharFirst;
            set => progressesUpperLeftCornerCharFirst = value;
        }
        /// <summary>
        /// [Progresses] Upper left corner character for the second bar
        /// </summary>
        public char ProgressesUpperLeftCornerCharSecond
        {
            get => progressesUpperLeftCornerCharSecond;
            set => progressesUpperLeftCornerCharSecond = value;
        }
        /// <summary>
        /// [Progresses] Upper left corner character for the third bar
        /// </summary>
        public char ProgressesUpperLeftCornerCharThird
        {
            get => progressesUpperLeftCornerCharThird;
            set => progressesUpperLeftCornerCharThird = value;
        }
        /// <summary>
        /// [Progresses] Upper right corner character for the first bar
        /// </summary>
        public char ProgressesUpperRightCornerCharFirst
        {
            get => progressesUpperRightCornerCharFirst;
            set => progressesUpperRightCornerCharFirst = value;
        }
        /// <summary>
        /// [Progresses] Upper right corner character for the second bar
        /// </summary>
        public char ProgressesUpperRightCornerCharSecond
        {
            get => progressesUpperRightCornerCharSecond;
            set => progressesUpperRightCornerCharSecond = value;
        }
        /// <summary>
        /// [Progresses] Upper right corner character for the third bar
        /// </summary>
        public char ProgressesUpperRightCornerCharThird
        {
            get => progressesUpperRightCornerCharThird;
            set => progressesUpperRightCornerCharThird = value;
        }
        /// <summary>
        /// [Progresses] Lower left corner character for the first bar
        /// </summary>
        public char ProgressesLowerLeftCornerCharFirst
        {
            get => progressesLowerLeftCornerCharFirst;
            set => progressesLowerLeftCornerCharFirst = value;
        }
        /// <summary>
        /// [Progresses] Lower left corner character for the second bar
        /// </summary>
        public char ProgressesLowerLeftCornerCharSecond
        {
            get => progressesLowerLeftCornerCharSecond;
            set => progressesLowerLeftCornerCharSecond = value;
        }
        /// <summary>
        /// [Progresses] Lower left corner character for the third bar
        /// </summary>
        public char ProgressesLowerLeftCornerCharThird
        {
            get => progressesLowerLeftCornerCharThird;
            set => progressesLowerLeftCornerCharThird = value;
        }
        /// <summary>
        /// [Progresses] Lower right corner character for the first bar
        /// </summary>
        public char ProgressesLowerRightCornerCharFirst
        {
            get => progressesLowerRightCornerCharFirst;
            set => progressesLowerRightCornerCharFirst = value;
        }
        /// <summary>
        /// [Progresses] Lower right corner character for the second bar
        /// </summary>
        public char ProgressesLowerRightCornerCharSecond
        {
            get => progressesLowerRightCornerCharSecond;
            set => progressesLowerRightCornerCharSecond = value;
        }
        /// <summary>
        /// [Progresses] Lower right corner character for the third bar
        /// </summary>
        public char ProgressesLowerRightCornerCharThird
        {
            get => progressesLowerRightCornerCharThird;
            set => progressesLowerRightCornerCharThird = value;
        }
        /// <summary>
        /// [Progresses] Upper frame character for the first bar
        /// </summary>
        public char ProgressesUpperFrameCharFirst
        {
            get => progressesUpperFrameCharFirst;
            set => progressesUpperFrameCharFirst = value;
        }
        /// <summary>
        /// [Progresses] Upper frame character for the second bar
        /// </summary>
        public char ProgressesUpperFrameCharSecond
        {
            get => progressesUpperFrameCharSecond;
            set => progressesUpperFrameCharSecond = value;
        }
        /// <summary>
        /// [Progresses] Upper frame character for the third bar
        /// </summary>
        public char ProgressesUpperFrameCharThird
        {
            get => progressesUpperFrameCharThird;
            set => progressesUpperFrameCharThird = value;
        }
        /// <summary>
        /// [Progresses] Lower frame character for the first bar
        /// </summary>
        public char ProgressesLowerFrameCharFirst
        {
            get => progressesLowerFrameCharFirst;
            set => progressesLowerFrameCharFirst = value;
        }
        /// <summary>
        /// [Progresses] Lower frame character for the second bar
        /// </summary>
        public char ProgressesLowerFrameCharSecond
        {
            get => progressesLowerFrameCharSecond;
            set => progressesLowerFrameCharSecond = value;
        }
        /// <summary>
        /// [Progresses] Lower frame character for the third bar
        /// </summary>
        public char ProgressesLowerFrameCharThird
        {
            get => progressesLowerFrameCharThird;
            set => progressesLowerFrameCharThird = value;
        }
        /// <summary>
        /// [Progresses] Left frame character for the first bar
        /// </summary>
        public char ProgressesLeftFrameCharFirst
        {
            get => progressesLeftFrameCharFirst;
            set => progressesLeftFrameCharFirst = value;
        }
        /// <summary>
        /// [Progresses] Left frame character for the second bar
        /// </summary>
        public char ProgressesLeftFrameCharSecond
        {
            get => progressesLeftFrameCharSecond;
            set => progressesLeftFrameCharSecond = value;
        }
        /// <summary>
        /// [Progresses] Left frame character for the third bar
        /// </summary>
        public char ProgressesLeftFrameCharThird
        {
            get => progressesLeftFrameCharThird;
            set => progressesLeftFrameCharThird = value;
        }
        /// <summary>
        /// [Progresses] Right frame character for the first bar
        /// </summary>
        public char ProgressesRightFrameCharFirst
        {
            get => progressesRightFrameCharFirst;
            set => progressesRightFrameCharFirst = value;
        }
        /// <summary>
        /// [Progresses] Right frame character for the second bar
        /// </summary>
        public char ProgressesRightFrameCharSecond
        {
            get => progressesRightFrameCharSecond;
            set => progressesRightFrameCharSecond = value;
        }
        /// <summary>
        /// [Progresses] Right frame character for the third bar
        /// </summary>
        public char ProgressesRightFrameCharThird
        {
            get => progressesRightFrameCharThird;
            set => progressesRightFrameCharThird = value;
        }
        /// <summary>
        /// [Progresses] Information text for the first bar
        /// </summary>
        public string ProgressesInfoTextFirst
        {
            get => progressesInfoTextFirst;
            set => progressesInfoTextFirst = value;
        }
        /// <summary>
        /// [Progresses] Information text for the second bar
        /// </summary>
        public string ProgressesInfoTextSecond
        {
            get => progressesInfoTextSecond;
            set => progressesInfoTextSecond = value;
        }
        /// <summary>
        /// [Progresses] Information text for the third bar
        /// </summary>
        public string ProgressesInfoTextThird
        {
            get => progressesInfoTextThird;
            set => progressesInfoTextThird = value;
        }
        /// <summary>
        /// [Progresses] The minimum red color level (true color - first)
        /// </summary>
        public int ProgressesMinimumRedColorLevelFirst
        {
            get => progressesMinimumRedColorLevelFirst;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressesMinimumRedColorLevelFirst = value;
            }
        }
        /// <summary>
        /// [Progresses] The minimum green color level (true color - first)
        /// </summary>
        public int ProgressesMinimumGreenColorLevelFirst
        {
            get => progressesMinimumGreenColorLevelFirst;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressesMinimumGreenColorLevelFirst = value;
            }
        }
        /// <summary>
        /// [Progresses] The minimum blue color level (true color - first)
        /// </summary>
        public int ProgressesMinimumBlueColorLevelFirst
        {
            get => progressesMinimumBlueColorLevelFirst;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressesMinimumBlueColorLevelFirst = value;
            }
        }
        /// <summary>
        /// [Progresses] The minimum color level (255 colors or 16 colors - first)
        /// </summary>
        public int ProgressesMinimumColorLevelFirst
        {
            get => progressesMinimumColorLevelFirst;
            set
            {
                int FinalMinimumLevel = 255;
                if (value < 0)
                    value = 1;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                progressesMinimumColorLevelFirst = value;
            }
        }
        /// <summary>
        /// [Progresses] The maximum red color level (true color - first)
        /// </summary>
        public int ProgressesMaximumRedColorLevelFirst
        {
            get => progressesMaximumRedColorLevelFirst;
            set
            {
                if (value <= progressesMinimumRedColorLevelFirst)
                    value = progressesMinimumRedColorLevelFirst;
                if (value > 255)
                    value = 255;
                progressesMaximumRedColorLevelFirst = value;
            }
        }
        /// <summary>
        /// [Progresses] The maximum green color level (true color - first)
        /// </summary>
        public int ProgressesMaximumGreenColorLevelFirst
        {
            get => progressesMaximumGreenColorLevelFirst;
            set
            {
                if (value <= progressesMinimumGreenColorLevelFirst)
                    value = progressesMinimumGreenColorLevelFirst;
                if (value > 255)
                    value = 255;
                progressesMaximumGreenColorLevelFirst = value;
            }
        }
        /// <summary>
        /// [Progresses] The maximum blue color level (true color - first)
        /// </summary>
        public int ProgressesMaximumBlueColorLevelFirst
        {
            get => progressesMaximumBlueColorLevelFirst;
            set
            {
                if (value <= progressesMinimumBlueColorLevelFirst)
                    value = progressesMinimumBlueColorLevelFirst;
                if (value > 255)
                    value = 255;
                progressesMaximumBlueColorLevelFirst = value;
            }
        }
        /// <summary>
        /// [Progresses] The maximum color level (255 colors or 16 colors - first)
        /// </summary>
        public int ProgressesMaximumColorLevelFirst
        {
            get => progressesMaximumColorLevelFirst;
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= progressesMinimumColorLevelFirst)
                    value = progressesMinimumColorLevelFirst;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                progressesMaximumColorLevelFirst = value;
            }
        }
        /// <summary>
        /// [Progresses] The minimum red color level (true color - second)
        /// </summary>
        public int ProgressesMinimumRedColorLevelSecond
        {
            get => progressesMinimumRedColorLevelSecond;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressesMinimumRedColorLevelSecond = value;
            }
        }
        /// <summary>
        /// [Progresses] The minimum green color level (true color - second)
        /// </summary>
        public int ProgressesMinimumGreenColorLevelSecond
        {
            get => progressesMinimumGreenColorLevelSecond;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressesMinimumGreenColorLevelSecond = value;
            }
        }
        /// <summary>
        /// [Progresses] The minimum blue color level (true color - second)
        /// </summary>
        public int ProgressesMinimumBlueColorLevelSecond
        {
            get => progressesMinimumBlueColorLevelSecond;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressesMinimumBlueColorLevelSecond = value;
            }
        }
        /// <summary>
        /// [Progresses] The minimum color level (255 colors or 16 colors - second)
        /// </summary>
        public int ProgressesMinimumColorLevelSecond
        {
            get => progressesMinimumColorLevelSecond;
            set
            {
                int FinalMinimumLevel = 255;
                if (value < 0)
                    value = 1;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                progressesMinimumColorLevelSecond = value;
            }
        }
        /// <summary>
        /// [Progresses] The maximum red color level (true color - second)
        /// </summary>
        public int ProgressesMaximumRedColorLevelSecond
        {
            get => progressesMaximumRedColorLevelSecond;
            set
            {
                if (value <= progressesMinimumRedColorLevelSecond)
                    value = progressesMinimumRedColorLevelSecond;
                if (value > 255)
                    value = 255;
                progressesMaximumRedColorLevelSecond = value;
            }
        }
        /// <summary>
        /// [Progresses] The maximum green color level (true color - second)
        /// </summary>
        public int ProgressesMaximumGreenColorLevelSecond
        {
            get => progressesMaximumGreenColorLevelSecond;
            set
            {
                if (value <= progressesMinimumGreenColorLevelSecond)
                    value = progressesMinimumGreenColorLevelSecond;
                if (value > 255)
                    value = 255;
                progressesMaximumGreenColorLevelSecond = value;
            }
        }
        /// <summary>
        /// [Progresses] The maximum blue color level (true color - second)
        /// </summary>
        public int ProgressesMaximumBlueColorLevelSecond
        {
            get => progressesMaximumBlueColorLevelSecond;
            set
            {
                if (value <= progressesMinimumBlueColorLevelSecond)
                    value = progressesMinimumBlueColorLevelSecond;
                if (value > 255)
                    value = 255;
                progressesMaximumBlueColorLevelSecond = value;
            }
        }
        /// <summary>
        /// [Progresses] The maximum color level (255 colors or 16 colors - second)
        /// </summary>
        public int ProgressesMaximumColorLevelSecond
        {
            get => progressesMaximumColorLevelSecond;
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= progressesMinimumColorLevelSecond)
                    value = progressesMinimumColorLevelSecond;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                progressesMaximumColorLevelSecond = value;
            }
        }
        /// <summary>
        /// [Progresses] The minimum red color level (true color - third)
        /// </summary>
        public int ProgressesMinimumRedColorLevelThird
        {
            get => progressesMinimumRedColorLevelThird;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressesMinimumRedColorLevelThird = value;
            }
        }
        /// <summary>
        /// [Progresses] The minimum green color level (true color - third)
        /// </summary>
        public int ProgressesMinimumGreenColorLevelThird
        {
            get => progressesMinimumGreenColorLevelThird;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressesMinimumGreenColorLevelThird = value;
            }
        }
        /// <summary>
        /// [Progresses] The minimum blue color level (true color - third)
        /// </summary>
        public int ProgressesMinimumBlueColorLevelThird
        {
            get => progressesMinimumBlueColorLevelThird;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressesMinimumBlueColorLevelThird = value;
            }
        }
        /// <summary>
        /// [Progresses] The minimum color level (255 colors or 16 colors - third)
        /// </summary>
        public int ProgressesMinimumColorLevelThird
        {
            get => progressesMinimumColorLevelThird;
            set
            {
                int FinalMinimumLevel = 255;
                if (value < 0)
                    value = 1;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                progressesMinimumColorLevelThird = value;
            }
        }
        /// <summary>
        /// [Progresses] The maximum red color level (true color - third)
        /// </summary>
        public int ProgressesMaximumRedColorLevelThird
        {
            get => progressesMaximumRedColorLevelThird;
            set
            {
                if (value <= progressesMinimumRedColorLevelThird)
                    value = progressesMinimumRedColorLevelThird;
                if (value > 255)
                    value = 255;
                progressesMaximumRedColorLevelThird = value;
            }
        }
        /// <summary>
        /// [Progresses] The maximum green color level (true color - third)
        /// </summary>
        public int ProgressesMaximumGreenColorLevelThird
        {
            get => progressesMaximumGreenColorLevelThird;
            set
            {
                if (value <= progressesMinimumGreenColorLevelThird)
                    value = progressesMinimumGreenColorLevelThird;
                if (value > 255)
                    value = 255;
                progressesMaximumGreenColorLevelThird = value;
            }
        }
        /// <summary>
        /// [Progresses] The maximum blue color level (true color - third)
        /// </summary>
        public int ProgressesMaximumBlueColorLevelThird
        {
            get => progressesMaximumBlueColorLevelThird;
            set
            {
                if (value <= progressesMinimumBlueColorLevelThird)
                    value = progressesMinimumBlueColorLevelThird;
                if (value > 255)
                    value = 255;
                progressesMaximumBlueColorLevelThird = value;
            }
        }
        /// <summary>
        /// [Progresses] The maximum color level (255 colors or 16 colors - third)
        /// </summary>
        public int ProgressesMaximumColorLevelThird
        {
            get => progressesMaximumColorLevelThird;
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= progressesMinimumColorLevelThird)
                    value = progressesMinimumColorLevelThird;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                progressesMaximumColorLevelThird = value;
            }
        }
        /// <summary>
        /// [Progresses] The minimum red color level (true color)
        /// </summary>
        public int ProgressesMinimumRedColorLevel
        {
            get => progressesMinimumRedColorLevel;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressesMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Progresses] The minimum green color level (true color)
        /// </summary>
        public int ProgressesMinimumGreenColorLevel
        {
            get => progressesMinimumGreenColorLevel;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressesMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Progresses] The minimum blue color level (true color)
        /// </summary>
        public int ProgressesMinimumBlueColorLevel
        {
            get => progressesMinimumBlueColorLevel;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressesMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Progresses] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int ProgressesMinimumColorLevel
        {
            get => progressesMinimumColorLevel;
            set
            {
                int FinalMinimumLevel = 255;
                if (value < 0)
                    value = 1;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                progressesMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Progresses] The maximum red color level (true color)
        /// </summary>
        public int ProgressesMaximumRedColorLevel
        {
            get => progressesMaximumRedColorLevel;
            set
            {
                if (value <= progressesMinimumRedColorLevel)
                    value = progressesMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                progressesMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Progresses] The maximum green color level (true color)
        /// </summary>
        public int ProgressesMaximumGreenColorLevel
        {
            get => progressesMaximumGreenColorLevel;
            set
            {
                if (value <= progressesMinimumGreenColorLevel)
                    value = progressesMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                progressesMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Progresses] The maximum blue color level (true color)
        /// </summary>
        public int ProgressesMaximumBlueColorLevel
        {
            get => progressesMaximumBlueColorLevel;
            set
            {
                if (value <= progressesMinimumBlueColorLevel)
                    value = progressesMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                progressesMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Progresses] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int ProgressesMaximumColorLevel
        {
            get => progressesMaximumColorLevel;
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= progressesMinimumColorLevel)
                    value = progressesMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                progressesMaximumColorLevel = value;
            }
        }
        #endregion

        #region ClochromaAnalog
        private bool clochromaAnalogTrueColor = true;
        private int clochromaAnalogDelay = 1000;
        private bool clochromaAnalogShowSecondsHand = true;
        private int clochromaAnalogMinimumRedColorLevel = 0;
        private int clochromaAnalogMinimumGreenColorLevel = 0;
        private int clochromaAnalogMinimumBlueColorLevel = 0;
        private int clochromaAnalogMinimumColorLevel = 0;
        private int clochromaAnalogMaximumRedColorLevel = 255;
        private int clochromaAnalogMaximumGreenColorLevel = 255;
        private int clochromaAnalogMaximumBlueColorLevel = 255;
        private int clochromaAnalogMaximumColorLevel = 255;
        private bool clochromaAnalogBright = false;

        /// <summary>
        /// [ClochromaAnalog] Whether to use bright or dark version.
        /// </summary>
        public bool ClochromaAnalogBright
        {
            get
            {
                return clochromaAnalogBright;
            }
            set
            {
                clochromaAnalogBright = value;
            }
        }
        /// <summary>
        /// [ClochromaAnalog] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool ClochromaAnalogTrueColor
        {
            get
            {
                return clochromaAnalogTrueColor;
            }
            set
            {
                clochromaAnalogTrueColor = value;
            }
        }
        /// <summary>
        /// [ClochromaAnalog] How many milliseconds to wait before making the next write?
        /// </summary>
        public int ClochromaAnalogDelay
        {
            get
            {
                return clochromaAnalogDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                clochromaAnalogDelay = value;
            }
        }
        /// <summary>
        /// [ClochromaAnalog] Shows the seconds hand.
        /// </summary>
        public bool ClochromaAnalogShowSecondsHand
        {
            get
            {
                return clochromaAnalogShowSecondsHand;
            }
            set
            {
                clochromaAnalogShowSecondsHand = value;
            }
        }
        /// <summary>
        /// [ClochromaAnalog] The minimum red color level (true color)
        /// </summary>
        public int ClochromaAnalogMinimumRedColorLevel
        {
            get
            {
                return clochromaAnalogMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                clochromaAnalogMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ClochromaAnalog] The minimum green color level (true color)
        /// </summary>
        public int ClochromaAnalogMinimumGreenColorLevel
        {
            get
            {
                return clochromaAnalogMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                clochromaAnalogMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ClochromaAnalog] The minimum blue color level (true color)
        /// </summary>
        public int ClochromaAnalogMinimumBlueColorLevel
        {
            get
            {
                return clochromaAnalogMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                clochromaAnalogMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ClochromaAnalog] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int ClochromaAnalogMinimumColorLevel
        {
            get
            {
                return clochromaAnalogMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                clochromaAnalogMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [ClochromaAnalog] The maximum red color level (true color)
        /// </summary>
        public int ClochromaAnalogMaximumRedColorLevel
        {
            get
            {
                return clochromaAnalogMaximumRedColorLevel;
            }
            set
            {
                if (value <= clochromaAnalogMinimumRedColorLevel)
                    value = clochromaAnalogMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                clochromaAnalogMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ClochromaAnalog] The maximum green color level (true color)
        /// </summary>
        public int ClochromaAnalogMaximumGreenColorLevel
        {
            get
            {
                return clochromaAnalogMaximumGreenColorLevel;
            }
            set
            {
                if (value <= clochromaAnalogMinimumGreenColorLevel)
                    value = clochromaAnalogMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                clochromaAnalogMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ClochromaAnalog] The maximum blue color level (true color)
        /// </summary>
        public int ClochromaAnalogMaximumBlueColorLevel
        {
            get
            {
                return clochromaAnalogMaximumBlueColorLevel;
            }
            set
            {
                if (value <= clochromaAnalogMinimumBlueColorLevel)
                    value = clochromaAnalogMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                clochromaAnalogMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ClochromaAnalog] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int ClochromaAnalogMaximumColorLevel
        {
            get
            {
                return clochromaAnalogMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= clochromaAnalogMinimumColorLevel)
                    value = clochromaAnalogMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                clochromaAnalogMaximumColorLevel = value;
            }
        }
        #endregion

        #region DigitalClock
        private bool digitalClockTrueColor = true;
        private int digitalClockDelay = 1000;
        private int digitalClockMinimumRedColorLevel = 0;
        private int digitalClockMinimumGreenColorLevel = 0;
        private int digitalClockMinimumBlueColorLevel = 0;
        private int digitalClockMinimumColorLevel = 0;
        private int digitalClockMaximumRedColorLevel = 255;
        private int digitalClockMaximumGreenColorLevel = 255;
        private int digitalClockMaximumBlueColorLevel = 255;
        private int digitalClockMaximumColorLevel = 255;

        /// <summary>
        /// [DigitalClock] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool DigitalClockTrueColor
        {
            get
            {
                return digitalClockTrueColor;
            }
            set
            {
                digitalClockTrueColor = value;
            }
        }
        /// <summary>
        /// [DigitalClock] How many milliseconds to wait before making the next write?
        /// </summary>
        public int DigitalClockDelay
        {
            get
            {
                return digitalClockDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                digitalClockDelay = value;
            }
        }
        /// <summary>
        /// [DigitalClock] The minimum red color level (true color)
        /// </summary>
        public int DigitalClockMinimumRedColorLevel
        {
            get
            {
                return digitalClockMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                digitalClockMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [DigitalClock] The minimum green color level (true color)
        /// </summary>
        public int DigitalClockMinimumGreenColorLevel
        {
            get
            {
                return digitalClockMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                digitalClockMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [DigitalClock] The minimum blue color level (true color)
        /// </summary>
        public int DigitalClockMinimumBlueColorLevel
        {
            get
            {
                return digitalClockMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                digitalClockMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [DigitalClock] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int DigitalClockMinimumColorLevel
        {
            get
            {
                return digitalClockMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                digitalClockMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [DigitalClock] The maximum red color level (true color)
        /// </summary>
        public int DigitalClockMaximumRedColorLevel
        {
            get
            {
                return digitalClockMaximumRedColorLevel;
            }
            set
            {
                if (value <= digitalClockMinimumRedColorLevel)
                    value = digitalClockMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                digitalClockMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [DigitalClock] The maximum green color level (true color)
        /// </summary>
        public int DigitalClockMaximumGreenColorLevel
        {
            get
            {
                return digitalClockMaximumGreenColorLevel;
            }
            set
            {
                if (value <= digitalClockMinimumGreenColorLevel)
                    value = digitalClockMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                digitalClockMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [DigitalClock] The maximum blue color level (true color)
        /// </summary>
        public int DigitalClockMaximumBlueColorLevel
        {
            get
            {
                return digitalClockMaximumBlueColorLevel;
            }
            set
            {
                if (value <= digitalClockMinimumBlueColorLevel)
                    value = digitalClockMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                digitalClockMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [DigitalClock] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int DigitalClockMaximumColorLevel
        {
            get
            {
                return digitalClockMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= digitalClockMinimumColorLevel)
                    value = digitalClockMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                digitalClockMaximumColorLevel = value;
            }
        }
        #endregion

        #region Pi
        private bool piTrueColor = true;
        private int piDelay = 20;
        private int piMinimumRedColorLevel = 0;
        private int piMinimumGreenColorLevel = 0;
        private int piMinimumBlueColorLevel = 0;
        private int piMinimumColorLevel = 0;
        private int piMaximumRedColorLevel = 255;
        private int piMaximumGreenColorLevel = 255;
        private int piMaximumBlueColorLevel = 255;
        private int piMaximumColorLevel = 255;

        /// <summary>
        /// [Pi] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool PiTrueColor
        {
            get
            {
                return piTrueColor;
            }
            set
            {
                piTrueColor = value;
            }
        }
        /// <summary>
        /// [Pi] How many milliseconds to wait before making the next write?
        /// </summary>
        public int PiDelay
        {
            get
            {
                return piDelay;
            }
            set
            {
                if (value <= 0)
                    value = 20;
                piDelay = value;
            }
        }
        /// <summary>
        /// [Pi] The minimum red color level (true color)
        /// </summary>
        public int PiMinimumRedColorLevel
        {
            get
            {
                return piMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                piMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Pi] The minimum green color level (true color)
        /// </summary>
        public int PiMinimumGreenColorLevel
        {
            get
            {
                return piMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                piMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Pi] The minimum blue color level (true color)
        /// </summary>
        public int PiMinimumBlueColorLevel
        {
            get
            {
                return piMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                piMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Pi] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int PiMinimumColorLevel
        {
            get
            {
                return piMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                piMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Pi] The maximum red color level (true color)
        /// </summary>
        public int PiMaximumRedColorLevel
        {
            get
            {
                return piMaximumRedColorLevel;
            }
            set
            {
                if (value <= piMinimumRedColorLevel)
                    value = piMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                piMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Pi] The maximum green color level (true color)
        /// </summary>
        public int PiMaximumGreenColorLevel
        {
            get
            {
                return piMaximumGreenColorLevel;
            }
            set
            {
                if (value <= piMinimumGreenColorLevel)
                    value = piMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                piMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Pi] The maximum blue color level (true color)
        /// </summary>
        public int PiMaximumBlueColorLevel
        {
            get
            {
                return piMaximumBlueColorLevel;
            }
            set
            {
                if (value <= piMinimumBlueColorLevel)
                    value = piMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                piMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Pi] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int PiMaximumColorLevel
        {
            get
            {
                return piMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= piMinimumColorLevel)
                    value = piMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                piMaximumColorLevel = value;
            }
        }
        #endregion
    }
}
