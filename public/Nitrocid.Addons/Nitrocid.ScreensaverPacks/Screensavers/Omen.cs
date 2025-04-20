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

using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Colors.Gradients;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Omen
    /// </summary>
    public class OmenDisplay : BaseScreensaver, IScreensaver
    {
        private int step = 0;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Omen";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            step = 0;
            ColorTools.LoadBackDry(ConsoleColors.Black);
            ConsoleWrapper.CursorVisible = false;
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Get the colors
            var currentBackColor = new Color(ScreensaverPackInit.SaversConfig.OmenMaximumBackColorLevel * step / 100, 0, 0);
            var currentLineColor = new Color(ScreensaverPackInit.SaversConfig.OmenMaximumLineColorLevel * step / 100, 0, 0);
            var currentTextColor = new Color(ScreensaverPackInit.SaversConfig.OmenMaximumTextColorLevel * step / 100, 0, 0);

            // Get the gradient BG in terms of a console width
            var bgGradient = ColorGradients.GetGradients(ConsoleColors.Black, currentBackColor, ConsoleWrapper.WindowHeight - 1);

            // Wrap the text in two lines and get a line
            string[] wrapped = ConsoleMisc.GetWrappedSentencesByWords(ScreensaverPackInit.SaversConfig.OmenWrite, ConsoleWrapper.WindowWidth / 2);
            string line = new('─', ConsoleWrapper.WindowWidth / 4);

            // Render the BG, the line, and the two lines of text
            int linePosY = ConsoleWrapper.WindowHeight - 5;
            int firstLinePosY = ConsoleWrapper.WindowHeight - 4;
            int secondLinePosY = ConsoleWrapper.WindowHeight - 3;
            for (int i = ConsoleWrapper.WindowHeight - 1; i >= 0; i--)
            {
                string renderedText =
                    i == secondLinePosY && wrapped.Length > 1 ? wrapped[1] :
                    i == firstLinePosY && wrapped.Length > 0 ? wrapped[0] :
                    i == linePosY ? line : "";
                var gradient = bgGradient[i];
                ColorTools.SetConsoleColorDry(currentLineColor);
                ColorTools.SetConsoleColorDry(gradient.IntermediateColor, true, true);
                TextWriterWhereColor.WriteWherePlain(new(' ', ConsoleWrapper.WindowWidth), 0, i);
                ColorTools.SetConsoleColorDry(currentTextColor);
                var omenText = new AlignedText()
                {
                    Top = i,
                    Text = renderedText,
                    ForegroundColor = currentTextColor,
                    BackgroundColor = gradient.IntermediateColor,
                    Settings = new()
                    {
                        Alignment = TextAlignment.Middle,
                    }
                };
                TextWriterRaw.WriteRaw(omenText.Render());
            }

            // If step is 100, stop, but render in case of a resize.
            if (step != 100)
                step++;
            ThreadManager.SleepNoBlock(ScreensaverPackInit.SaversConfig.OmenDelay);
        }
    }
}
