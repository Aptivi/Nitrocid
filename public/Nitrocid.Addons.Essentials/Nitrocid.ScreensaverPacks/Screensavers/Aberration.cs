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

using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Misc.Screensaver;
using System.Text;
using Terminaux.Colors;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Aberration
    /// </summary>
    public class AberrationDisplay : BaseScreensaver, IScreensaver
    {
        private readonly Color[] glitchColors =
        [
            new(ConsoleColors.Red),
            new(ConsoleColors.Lime),
            new(ConsoleColors.Blue),
        ];

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Aberration";

        /// <inheritdoc/>
        public override bool ScreensaverContainsFlashingImages =>
            true;

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            ColorTools.LoadBackDry(0);
            ConsoleWrapper.Clear();
            ConsoleWrapper.CursorVisible = false;
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            bool glitching = RandomDriver.RandomChance(ScreensaverPackInit.SaversConfig.AberrationProbability);
            if (glitching)
            {
                // Draw the glitch when aberration happens
                var glitch = new StringBuilder();
                var color = glitchColors[RandomDriver.RandomIdx(glitchColors.Length)];
                int x = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
                glitch.Append(color.VTSequenceBackground);
                for (int y = 0; y < ConsoleWrapper.WindowHeight; y++)
                {
                    glitch.Append(
                        CsiSequences.GenerateCsiCursorPosition(x + 1, y + 1) +
                        " "
                    );
                }
                TextWriterRaw.WriteRaw(glitch.ToString());
            }
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.AberrationDelay);
            ColorTools.LoadBackDry(new Color(ConsoleColors.Black));
        }

    }
}
