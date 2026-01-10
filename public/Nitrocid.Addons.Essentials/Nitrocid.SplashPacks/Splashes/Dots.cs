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

using System.Threading;
using Terminaux.Colors;
using Terminaux.Sequences;
using System.Text;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Splash;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Base;
using Terminaux.Themes.Colors;
using Terminaux.Colors.Data;
using Terminaux.Colors.Transformation.Contrast;

namespace Nitrocid.SplashPacks.Splashes
{
    class SplashDots : BaseSplash, ISplash
    {

        private int dotStep = 0;
        private int currMs = 0;

        // Standalone splash information
        public override string SplashName => "Dots";

        // Actual logic
        public override string Display(SplashContext context)
        {
            var builder = new StringBuilder();
            try
            {
                bool noAppend = true;
                currMs++;
                if (currMs >= 10)
                {
                    noAppend = false;
                    currMs = 0;
                }
                Color firstColor = ThemeColorsTools.GetColor(ThemeColorType.Background).Brightness == ColorBrightness.Light ? new(ConsoleColors.Black) : new(ConsoleColors.White);
                Color secondColor = ThemeColorsTools.GetColor(ThemeColorType.Success);
                DebugWriter.WriteDebug(DebugLevel.I, "Splash displaying.");
                Color firstDotColor = dotStep >= 1 ? secondColor : firstColor;
                Color secondDotColor = dotStep >= 2 ? secondColor : firstColor;
                Color thirdDotColor = dotStep >= 3 ? secondColor : firstColor;
                Color fourthDotColor = dotStep >= 4 ? secondColor : firstColor;
                Color fifthDotColor = dotStep >= 5 ? secondColor : firstColor;

                // Write the three dots
                string dots =
                    $"{firstDotColor.VTSequenceForeground()}* " +
                    $"{secondDotColor.VTSequenceForeground()}* " +
                    $"{thirdDotColor.VTSequenceForeground()}* " +
                    $"{fourthDotColor.VTSequenceForeground()}* " +
                    $"{fifthDotColor.VTSequenceForeground()}*";
                int dotsPosX = ConsoleWrapper.WindowWidth / 2 - VtSequenceTools.FilterVTSequences(dots).Length / 2;
                int dotsPosY = ConsoleWrapper.WindowHeight - 2;
                builder.Append(TextWriterWhereColor.RenderWhere(dots, dotsPosX, dotsPosY));
                if (!noAppend)
                {
                    dotStep++;
                    if (dotStep > 5)
                        dotStep = 0;
                }
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash done.");
            }
            return builder.ToString();
        }

    }
}
