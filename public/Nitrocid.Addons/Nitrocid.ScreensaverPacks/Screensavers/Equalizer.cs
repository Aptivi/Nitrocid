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

using System.Collections.Generic;
using System.Linq;
using Terminaux.Colors;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Screensaver;
using Terminaux.Writer.FancyWriters;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.ConsoleBase;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Threading;
using Terminaux.Base;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for Equalizer
    /// </summary>
    public static class EqualizerSettings
    {

        /// <summary>
        /// [Equalizer] How many milliseconds to wait before going to next equalizer preset?
        /// </summary>
        public static int EqualizerNextScreenDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.EqualizerNextScreenDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                ScreensaverPackInit.SaversConfig.EqualizerNextScreenDelay = value;
            }
        }

    }

    /// <summary>
    /// Display code for Equalizer
    /// </summary>
    public class EqualizerDisplay : BaseScreensaver, IScreensaver
    {

        // Equalizer presets
        private readonly Dictionary<string, (double, double, double)> presets = new()
        {
            // Name              60,    910,    14k (from -12 to 12 in decibels)
            { "Normal",         (3,     0,      3) },
            { "Classical",      (5,     -2,     4) },
            { "Dance",          (6,     2,      1) },
            { "Electronic",     (0,     4,      10) },
            { "Flat",           (0,     0,      0) },
            { "Folk",           (3,     0,      -1) },
            { "Heavy Metal",    (4,     9,      0) },
            { "Hip-Hop",        (5,     0,      3) },
            { "Jazz",           (4,     -2,     5) },
            { "Pop",            (-1,    5,      -2) },
            { "Rock",           (5,     -1,     5) },
            { "Small Speakers", (-1.7,  0.5,    2) }
        };

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Equalizer";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;
            ConsoleWrapper.Clear();

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
            int infoMessageHeight = ConsoleWrapper.WindowHeight - 2;
            string infoMessage = $"<< {presetName} >>";
            int infoMessageWidth = ConsoleWrapper.WindowWidth / 2 - infoMessage.Length / 2;
            ProgressBarVerticalColor.WriteVerticalProgress(bassHeight, oneSixthOfConsoleWidth, 1, 6, new Color(ConsoleColors.Red1));
            ProgressBarVerticalColor.WriteVerticalProgress(midHeight, oneSixthOfConsoleWidth * 3, 1, 6, new Color(ConsoleColors.Pink1));
            ProgressBarVerticalColor.WriteVerticalProgress(trebleHeight, oneSixthOfConsoleWidth * 5, 1, 6, new Color(ConsoleColors.Blue1));
            TextWriterWhereColor.WriteWhere(infoMessage, infoMessageWidth, infoMessageHeight);
            ThreadManager.SleepNoBlock(EqualizerSettings.EqualizerNextScreenDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
