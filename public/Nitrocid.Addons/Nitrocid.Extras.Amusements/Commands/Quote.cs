//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Extras.Amusements.Amusements.Quotes;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Nitrocid.Languages;

namespace Nitrocid.Extras.Amusements.Commands
{
    /// <summary>
    /// Print a quote
    /// </summary>
    /// <remarks>
    /// If you're looking for random quotes, look no further than this command, because it fetches quotes from the internet.
    /// </remarks>
    class QuoteCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "quote";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_AMUSEMENTS_COMMAND_QUOTE_DESC");

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            TextWriterColor.Write(RandomQuotes.RenderQuote());
            return 0;
        }

    }
}
