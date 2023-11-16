//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System.Linq;
using System.Text;
using System.Threading;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Debugging;
using KS.Misc.Splash;
using KS.Misc.Text;
using Nitrocid.Extras.Amusements.Amusements.Quotes;
using Terminaux.Colors;

namespace Nitrocid.Extras.Amusements.Splashes
{
    class SplashQuote : BaseSplash, ISplash
    {

        // Standalone splash information
        public override string SplashName => "Quote";

        // Actual logic
        public override string Display(SplashContext context)
        {
            var builder = new StringBuilder();
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash displaying.");

                // Display the quote
                Color quoteColor = KernelColorTools.GetRandomColor(ColorType.TrueColor);
                string renderedQuote = RandomQuotes.RenderQuote();
                string[] quoteSplit = renderedQuote.SplitNewLines();
                int maxLength = quoteSplit.Max((quote) => quote.Length);
                int halfConsoleY = ConsoleWrapper.WindowHeight / 2 - quoteSplit.Length / 2;
                int quotePosX = ConsoleWrapper.WindowWidth / 2 - maxLength / 2;
                for (int i = 0; i < quoteSplit.Length; i++)
                {
                    int currentY = halfConsoleY + i;
                    string str = quoteSplit[i];
                    builder.Append(
                        quoteColor.VTSequenceForeground +
                        TextWriterWhereColor.RenderWherePlain(str, quotePosX, currentY)
                    );
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
