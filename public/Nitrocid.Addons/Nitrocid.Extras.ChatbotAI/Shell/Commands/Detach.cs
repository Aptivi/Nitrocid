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
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;

namespace Nitrocid.Extras.ChatbotAI.Shell.Commands
{
    /// <summary>
    /// Detaches the shell from the current working server
    /// </summary>
    /// <remarks>
    /// If you want to detach the shell from the current working server, but don't want to disconnect from it, use this command.
    /// <br></br>
    /// </remarks>
    class DetachCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "detach";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_CHATBOTAI_SHELL_COMMAND_DETACH_DESC");

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            var chatbotShell = (ChatbotShell?)shell ??
                throw new KernelException(KernelExceptionType.ChatbotAI, LanguageTools.GetLocalized("NKS_CHATBOTAI_EXCEPTION_LASTSHELLTYPEMISMATCH"));
            chatbotShell.detaching = true;
            ShellManager.KillShell();
            return 0;
        }

    }
}
