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

using Nitrocid.Base.Kernel.Power;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Shuts down a remote computer
    /// </summary>
    /// <remarks>
    /// If you want to shut down a remote Nitrocid instance hosted by another computer, you can do so using this command.
    /// </remarks>
    class RShutdownCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "rshutdown";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_RSHUTDOWN_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "address", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_REXEC_ARGUMENT_HOSTNAME_DESC"
                    }),
                    new CommandArgumentPart(true, "port", new CommandArgumentPartOptions()
                    {
                        IsNumeric = true,
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_REXEC_ARGUMENT_PORT_DESC"
                    }),
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string address = parameters.ArgumentsList[0];
            if (parameters.ArgumentsList.Length == 1)
                PowerManager.PowerManage(PowerMode.RemoteShutdown, address);
            else
            {
                string portNumStr = parameters.ArgumentsList[1];
                PowerManager.PowerManage(PowerMode.RemoteShutdown, address, int.Parse(portNumStr));
            }
            return 0;
        }

    }
}
