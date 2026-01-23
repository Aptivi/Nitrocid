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

using System.Collections.Generic;
using System.Linq;
using Nitrocid.Drivers.RNG;
using Nitrocid.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors.Data;
using Terminaux.Colors.Transformation;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Writer.CyclicWriters.Simple;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Equalizer
    /// </summary>
    public class EqualizerDisplay : BaseScreensaver, IScreensaver
    {

        // Equalizer presets
        private readonly Dictionary<string, (double, double, double)> presets = new()
        {
            // Name              60,    910,    14k (from -12 to 12 in decibels)
            { "Normal",         (3,     0,      3)  },
            { "Classical",      (5,     -2,     4)  },
            { "Dance",          (6,     2,      1)  },
            { "Electronic",     (0,     4,      10) },
            { "Flat",           (0,     0,      0)  },
            { "Folk",           (3,     0,      -1) },
            { "Heavy Metal",    (4,     9,      0)  },
            { "Hip-Hop",        (5,     0,      3)  },
            { "Jazz",           (4,     -2,     5)  },
            { "Pop",            (-1,    5,      -2) },
            { "Rock",           (5,     -1,     5)  },
            { "Small Speakers", (-1.7,  0.5,    2)  }
        };

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Equalizer";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Get the bass, mid, and treble percentages for our vertical progress bars to form a fuzzy equalizer
            int presetIndex = RandomDriver.RandomIdx(presets.Count);
            string presetName = presets.ElementAt(presetIndex).Key;
            double bassFreq = presets[presetName].Item1 + 12;
            double midFreq = presets[presetName].Item2 + 12;
            double trebleFreq = presets[presetName].Item3 + 12;
            double bassHeight = 100 * (bassFreq / 24);
            double midHeight = 100 * (midFreq / 24);
            double trebleHeight = 100 * (trebleFreq / 24);

            // Get the console height and width needed to draw the progress bar
            int oneSixthOfConsoleWidth = ConsoleWrapper.WindowWidth / 6;

            // Draw the bass bar
            var bassMeter = new SimpleProgress((int)bassHeight, 100)
            {
                Vertical = true,
                Height = ConsoleWrapper.WindowHeight - 4,
                ProgressActiveForegroundColor = ConsoleColors.Red1,
                ProgressForegroundColor = TransformationTools.GetDarkBackground(ConsoleColors.Red1),
            };
            TextWriterRaw.WriteRaw(RendererTools.RenderRenderable(bassMeter, new(oneSixthOfConsoleWidth, 1)));

            // Draw the mid bar
            var midMeter = new SimpleProgress((int)midHeight, 100)
            {
                Vertical = true,
                Height = ConsoleWrapper.WindowHeight - 4,
                ProgressActiveForegroundColor = ConsoleColors.Pink1,
                ProgressForegroundColor = TransformationTools.GetDarkBackground(ConsoleColors.Pink1),
            };
            TextWriterRaw.WriteRaw(RendererTools.RenderRenderable(midMeter, new(oneSixthOfConsoleWidth * 3, 1)));

            // Draw the treble bar
            var trebleMeter = new SimpleProgress((int)trebleHeight, 100)
            {
                Vertical = true,
                Height = ConsoleWrapper.WindowHeight - 4,
                ProgressActiveForegroundColor = ConsoleColors.Blue1,
                ProgressForegroundColor = TransformationTools.GetDarkBackground(ConsoleColors.Blue1),
            };
            TextWriterRaw.WriteRaw(RendererTools.RenderRenderable(trebleMeter, new(oneSixthOfConsoleWidth * 5, 1)));

            // Write the preset name
            int infoMessageHeight = ConsoleWrapper.WindowHeight - 2;
            string infoMessage = "\x1b[1K" + $"<< {presetName} >>" + "\x1b[K";
            int infoMessageWidth = ConsoleWrapper.WindowWidth / 2 - ConsoleChar.EstimateCellWidth(infoMessage) / 2;
            TextWriterWhereColor.WriteWhere(infoMessage, infoMessageWidth, infoMessageHeight);
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.EqualizerNextScreenDelay);
        }

    }
}
