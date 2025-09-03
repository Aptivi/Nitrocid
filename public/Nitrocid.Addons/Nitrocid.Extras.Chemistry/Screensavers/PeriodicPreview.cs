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

using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Nitrocid.Kernel.Configuration;
using ChemiStar;
using ChemiStar.Data;
using System.Text;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Base.Extensions;
using Textify.Data.Figlet;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Writer.CyclicWriters.Simple;

namespace Nitrocid.Extras.Chemistry.Screensavers
{
    /// <summary>
    /// Display code for PeriodicPreview
    /// </summary>
    public class PeriodicPreviewDisplay : BaseScreensaver, IScreensaver
    {
        private static readonly SubstanceInfo[] substances = PeriodicTableParser.GetSubstances();

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Periodic Preview";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Select a random chemical element
            var substance = substances[RandomDriver.RandomIdx(substances.Length)];
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Selected element: {0} [{1}, {2}]", vars: [substance.Name, substance.Symbol, substance.AtomicNumber]);

            // Validate color
            Color color = "#" + (string.IsNullOrWhiteSpace(substance.ColorHex) ? "FFFFFF" : substance.ColorHex);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Element color: {0} [{1}, {2}]", vars: [color.Hex, color.PlainSequence]);

            // Make a box frame from available information that represents what would appear in the element preview in
            // chemistry books (Big text is symbol, bottom text is name, upper left corner is atomic number, and upper
            // right corner is atomic mass)
            var periodicItem = new StringBuilder();
            int width = ConsoleWrapper.WindowWidth / 2;
            int height = ConsoleWrapper.WindowHeight - 4;
            int posX = ConsoleWrapper.WindowWidth / 2 - width / 2;
            int posY = 1;
            var border = new Border()
            {
                Left = posX,
                Top = posY,
                Width = width,
                Height = height,
                Color = ConsoleColors.Black,
                BackgroundColor = color,
            };
            periodicItem.Append(border.Render());

            // Render the element properties in small fonts first
            int elementAtomicNumberPosX = posX + 3;
            int elementAtomicMassPosX = posX + width - substance.AtomicMass.ToString().Length - 1;
            int elementAtomicPosY = posY + 2;
            int elementNamePosX = ConsoleWrapper.WindowWidth / 2 - ConsoleChar.EstimateCellWidth(substance.Name) / 2 + 1;
            int elementNamePosY = height;
            periodicItem.Append(
                TextWriterWhereColor.RenderWhereColorBack($"{substance.AtomicNumber}", elementAtomicNumberPosX, elementAtomicPosY, ConsoleColors.Black, color) +
                TextWriterWhereColor.RenderWhereColorBack($"{substance.AtomicMass}", elementAtomicMassPosX, elementAtomicPosY, ConsoleColors.Black, color) +
                TextWriterWhereColor.RenderWhereColorBack(substance.Name, elementNamePosX, elementNamePosY, ConsoleColors.Black, color)
            );

            // Render the element symbol
            var font = FigletFonts.GetByName("BANNER2");
            int elementSymbolFigletWidth = FigletTools.GetFigletWidth(substance.Symbol, font);
            int elementSymbolFigletHeight = FigletTools.GetFigletHeight(substance.Symbol, font);
            int elementSymbolFigletPosX = ConsoleWrapper.WindowWidth / 2 - elementSymbolFigletWidth / 2 + 1;
            int elementSymbolFigletPosY = ConsoleWrapper.WindowHeight / 2 - elementSymbolFigletHeight / 2 - 1;
            var figletSubstance = new FigletText(font)
            {
                Text = substance.Symbol,
                ForegroundColor = ConsoleColors.Black,
                BackgroundColor = color,
            };
            periodicItem.Append(RendererTools.RenderRenderable(figletSubstance, new(elementSymbolFigletPosX, elementSymbolFigletPosY)));

            // Render the chemical element preview
            TextWriterRaw.WriteRaw(periodicItem.ToString());

            // Delay for 10 seconds (this should be enough for almost everyone to be able to read the element)
            ScreensaverManager.Delay(10000);

            // Clear the scene
            KernelColorTools.LoadBackground();

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
        }

    }
}
