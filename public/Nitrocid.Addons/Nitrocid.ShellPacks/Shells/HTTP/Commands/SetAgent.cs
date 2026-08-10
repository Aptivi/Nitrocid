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
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;

namespace Nitrocid.ShellPacks.Shells.HTTP.Commands
{
    /// <summary>
    /// Sets the current user agent
    /// </summary>
    class SetAgentCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "setagent";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_COMMAND_SETAGENT_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "userAgent", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_HTTP_COMMAND_SETAGENT_ARGUMENT_UA_DESC"
                    })
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            var httpShell = (HTTPShell?)shell ??
                throw new KernelException(KernelExceptionType.HTTPShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_LASTSHELLTYPEMISMATCH"));
            string uaToSet = parameters.ArgumentsList[0];
            httpShell.HttpSetUserAgent(uaToSet);
            return 0;
        }

    }
}
