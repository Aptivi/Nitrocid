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

using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.ShellPacks.Tools;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.ShellPacks.Shells.HTTP.Commands
{
    /// <summary>
    /// Shows the current user agent
    /// </summary>
    class CurrAgentCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "curragent";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_COMMAND_CURRAGENT_DESC");

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string currentUa = HttpTools.HttpGetCurrentUserAgent();
            TextWriterColor.Write(currentUa);
            return 0;
        }

    }
}
