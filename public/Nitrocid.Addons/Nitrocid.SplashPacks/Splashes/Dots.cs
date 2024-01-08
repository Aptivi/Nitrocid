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

using System.Threading;
using Terminaux.Colors;
using Textify.Sequences.Tools;
using System.Text;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Splash;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.ConsoleBase;
using Terminaux.Base;

namespace Nitrocid.SplashPacks.Splashes
{
    class SplashDots : BaseSplash, ISplash
    {

        private int dotStep = 0;

        // Standalone splash information
        public override string SplashName => "Dots";

        // Private variables
        readonly Color firstColor = new(ConsoleColors.White);
        readonly Color secondColor = new(ConsoleColors.Cyan);

        // Actual logic
        public override string Display(SplashContext context)
        {
            var builder = new StringBuilder();
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash displaying.");
                Color firstDotColor = dotStep >= 1 ? secondColor : firstColor;
                Color secondDotColor = dotStep >= 2 ? secondColor : firstColor;
                Color thirdDotColor = dotStep >= 3 ? secondColor : firstColor;

                // Write the three dots
                string dots = $"{firstDotColor.VTSequenceForeground}* {secondDotColor.VTSequenceForeground}* {thirdDotColor.VTSequenceForeground}*";
                int dotsPosX = (ConsoleWrapper.WindowWidth / 2) - (VtSequenceTools.FilterVTSequences(dots).Length / 2);
                int dotsPosY = ConsoleWrapper.WindowHeight - 2;
                builder.Append(TextWriterWhereColor.RenderWherePlain(dots, dotsPosX, dotsPosY));
                dotStep++;
                if (dotStep > 3)
                    dotStep = 0;
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash done.");
            }
            return builder.ToString();
        }

    }
}
