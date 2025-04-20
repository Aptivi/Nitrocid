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

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Drivers.RNG;
using Nitrocid.Extras.Amusements.Amusements.Quotes;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
using System.Linq;
using Terminaux.Colors;
using Textify.General;
using Terminaux.Base;

namespace Nitrocid.Extras.Amusements.Screensavers
{
    /// <summary>
    /// Display code for Quote
    /// </summary>
    public class QuoteDisplay : BaseScreensaver, IScreensaver
    {

        private string lastQuote = "";

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Quote";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Get the color and positions
            Color quoteColor = ChangeQuoteColor();
            string renderedQuote = RandomQuotes.RenderQuote();
            string[] quoteSplit = renderedQuote.SplitNewLines();
            int maxLength = quoteSplit.Max((quote) => quote.Length);
            int halfConsoleY = ConsoleWrapper.WindowHeight / 2 - quoteSplit.Length / 2;
            int quotePosX = ConsoleWrapper.WindowWidth / 2 - maxLength / 2;

            // Clear old quote
            string[] oldQuoteSplit = lastQuote.SplitNewLines().Select((str) => new string(' ', str.Length)).ToArray();
            int maxOldLength = oldQuoteSplit.Max((quote) => quote.Length);
            int oldHalfConsoleY = ConsoleWrapper.WindowHeight / 2 - oldQuoteSplit.Length / 2;
            int oldQuotePosX = ConsoleWrapper.WindowWidth / 2 - maxOldLength / 2;
            for (int i = 0; i < oldQuoteSplit.Length; i++)
            {
                int currentY = oldHalfConsoleY + i;
                string str = oldQuoteSplit[i];
                TextWriterWhereColor.WriteWhereColor(str, oldQuotePosX, currentY, quoteColor);
            }

            // Write quote
            for (int i = 0; i < quoteSplit.Length; i++)
            {
                int currentY = halfConsoleY + i;
                string str = quoteSplit[i];
                TextWriterWhereColor.WriteWhereColor(str, quotePosX, currentY, quoteColor);
            }

            // Delay
            lastQuote = renderedQuote;
            ThreadManager.SleepNoBlock(AmusementsInit.SaversConfig.QuoteDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

        /// <summary>
        /// Changes the color of the quote
        /// </summary>
        public Color ChangeQuoteColor()
        {
            Color ColorInstance;
            if (AmusementsInit.SaversConfig.QuoteTrueColor)
            {
                int RedColorNum = RandomDriver.Random(AmusementsInit.SaversConfig.QuoteMinimumRedColorLevel, AmusementsInit.SaversConfig.QuoteMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(AmusementsInit.SaversConfig.QuoteMinimumGreenColorLevel, AmusementsInit.SaversConfig.QuoteMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(AmusementsInit.SaversConfig.QuoteMinimumBlueColorLevel, AmusementsInit.SaversConfig.QuoteMaximumBlueColorLevel);
                ColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(AmusementsInit.SaversConfig.QuoteMinimumColorLevel, AmusementsInit.SaversConfig.QuoteMaximumColorLevel);
                ColorInstance = new Color(ColorNum);
            }
            return ColorInstance;
        }

    }
}
