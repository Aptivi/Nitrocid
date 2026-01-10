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

using System.Linq;
using System.Text;
using System.Threading;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Extras.Amusements.Amusements.Quotes;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Splash;
using Terminaux.Colors;
using Textify.General;
using Terminaux.Base;

namespace Nitrocid.Extras.Amusements.Splashes
{
    class SplashQuote : BaseSplash, ISplash
    {

        private bool _refresh = true;
        private string _selectedQuote = "";
        private Color _quoteColor = Color.Empty;

        // Standalone splash information
        public override string SplashName => "Quote";

        // Actual logic
        public override string Opening(SplashContext context)
        {
            if (_refresh)
            {
                _selectedQuote = RandomQuotes.RenderQuote();
                _quoteColor = ColorTools.GetRandomColor(ColorType.TrueColor);
            }
            _refresh = false;
            return base.Opening(context);
        }

        public override string Display(SplashContext context)
        {
            var builder = new StringBuilder();
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash displaying.");

                // Display the quote
                string[] quoteSplit = _selectedQuote.SplitNewLines();
                int maxLength = quoteSplit.Max((quote) => quote.Length);
                int halfConsoleY = ConsoleWrapper.WindowHeight / 2 - quoteSplit.Length / 2;
                int quotePosX = ConsoleWrapper.WindowWidth / 2 - maxLength / 2;
                for (int i = 0; i < quoteSplit.Length; i++)
                {
                    int currentY = halfConsoleY + i;
                    string str = quoteSplit[i];
                    builder.Append(
                        _quoteColor.VTSequenceForeground() +
                        TextWriterWhereColor.RenderWhere(str, quotePosX, currentY)
                    );
                }
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash done.");
            }
            return builder.ToString();
        }

        public override string Closing(SplashContext context, out bool delayRequired)
        {
            _refresh = true;
            return base.Closing(context, out delayRequired);
        }

    }
}
