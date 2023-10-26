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

using KS.ConsoleBase;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers.RNG;
using KS.Kernel.Threading;
using KS.Misc.Screensaver;
using KS.Misc.Text;
using Nitrocid.Extras.Amusements.Amusements.Quotes;
using System.Linq;
using Terminaux.Colors;

namespace Nitrocid.Extras.Amusements.Screensavers
{
    /// <summary>
    /// Settings for Quote
    /// </summary>
    public static class QuoteSettings
    {

        /// <summary>
        /// [Quote] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool QuoteTrueColor
        {
            get
            {
                return AmusementsInit.SaversConfig.QuoteTrueColor;
            }
            set
            {
                AmusementsInit.SaversConfig.QuoteTrueColor = value;
            }
        }
        /// <summary>
        /// [Quote] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int QuoteDelay
        {
            get
            {
                return AmusementsInit.SaversConfig.QuoteDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10000;
                AmusementsInit.SaversConfig.QuoteDelay = value;
            }
        }
        /// <summary>
        /// [Quote] The minimum red color level (true color)
        /// </summary>
        public static int QuoteMinimumRedColorLevel
        {
            get
            {
                return AmusementsInit.SaversConfig.QuoteMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                AmusementsInit.SaversConfig.QuoteMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Quote] The minimum green color level (true color)
        /// </summary>
        public static int QuoteMinimumGreenColorLevel
        {
            get
            {
                return AmusementsInit.SaversConfig.QuoteMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                AmusementsInit.SaversConfig.QuoteMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Quote] The minimum blue color level (true color)
        /// </summary>
        public static int QuoteMinimumBlueColorLevel
        {
            get
            {
                return AmusementsInit.SaversConfig.QuoteMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                AmusementsInit.SaversConfig.QuoteMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Quote] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int QuoteMinimumColorLevel
        {
            get
            {
                return AmusementsInit.SaversConfig.QuoteMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                AmusementsInit.SaversConfig.QuoteMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Quote] The maximum red color level (true color)
        /// </summary>
        public static int QuoteMaximumRedColorLevel
        {
            get
            {
                return AmusementsInit.SaversConfig.QuoteMaximumRedColorLevel;
            }
            set
            {
                if (value <= AmusementsInit.SaversConfig.QuoteMinimumRedColorLevel)
                    value = AmusementsInit.SaversConfig.QuoteMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                AmusementsInit.SaversConfig.QuoteMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Quote] The maximum green color level (true color)
        /// </summary>
        public static int QuoteMaximumGreenColorLevel
        {
            get
            {
                return AmusementsInit.SaversConfig.QuoteMaximumGreenColorLevel;
            }
            set
            {
                if (value <= AmusementsInit.SaversConfig.QuoteMinimumGreenColorLevel)
                    value = AmusementsInit.SaversConfig.QuoteMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                AmusementsInit.SaversConfig.QuoteMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Quote] The maximum blue color level (true color)
        /// </summary>
        public static int QuoteMaximumBlueColorLevel
        {
            get
            {
                return AmusementsInit.SaversConfig.QuoteMaximumBlueColorLevel;
            }
            set
            {
                if (value <= AmusementsInit.SaversConfig.QuoteMinimumBlueColorLevel)
                    value = AmusementsInit.SaversConfig.QuoteMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                AmusementsInit.SaversConfig.QuoteMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Quote] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int QuoteMaximumColorLevel
        {
            get
            {
                return AmusementsInit.SaversConfig.QuoteMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= AmusementsInit.SaversConfig.QuoteMinimumColorLevel)
                    value = AmusementsInit.SaversConfig.QuoteMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                AmusementsInit.SaversConfig.QuoteMaximumColorLevel = value;
            }
        }

    }

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
            ThreadManager.SleepNoBlock(QuoteSettings.QuoteDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

        /// <summary>
        /// Changes the color of the quote
        /// </summary>
        public Color ChangeQuoteColor()
        {
            Color ColorInstance;
            if (QuoteSettings.QuoteTrueColor)
            {
                int RedColorNum = RandomDriver.Random(QuoteSettings.QuoteMinimumRedColorLevel, QuoteSettings.QuoteMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(QuoteSettings.QuoteMinimumGreenColorLevel, QuoteSettings.QuoteMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(QuoteSettings.QuoteMinimumBlueColorLevel, QuoteSettings.QuoteMaximumBlueColorLevel);
                ColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(QuoteSettings.QuoteMinimumColorLevel, QuoteSettings.QuoteMaximumColorLevel);
                ColorInstance = new Color(ColorNum);
            }
            return ColorInstance;
        }

    }
}
