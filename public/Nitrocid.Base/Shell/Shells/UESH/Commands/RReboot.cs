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
using Terminaux.Shell.Switches;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Restarts the remote kernel
    /// </summary>
    /// <remarks>
    /// This command restarts your simulated kernel in the remote instance and reloads all the config that are not loaded using reloadconfig.
    /// </remarks>
    class RRebootCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "rreboot";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_RREBOOT_DESC");

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
                ],
                [
                    new SwitchInfo("safe", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_REBOOT_SWITCH_SAFE_DESC", new()
                    {
                        AcceptsValues = false,
                        ConflictsWith = ["maintenance", "debug"]
                    }),
                    new SwitchInfo("maintenance", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_REBOOT_SWITCH_MAINTENANCE_DESC", new()
                    {
                        AcceptsValues = false,
                        ConflictsWith = ["safe", "debug"]
                    }),
                    new SwitchInfo("debug", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_REBOOT_SWITCH_DEBUG_DESC", new()
                    {
                        AcceptsValues = false,
                        ConflictsWith = ["safe", "maintenance"]
                    }),
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            bool debug = parameters.ContainsSwitch("-debug");
            bool safe = parameters.ContainsSwitch("-safe");
            bool maintenance = parameters.ContainsSwitch("-maintenance");
            PowerMode mode =
                debug ? PowerMode.RemoteRestartDebug :
                safe ? PowerMode.RemoteRestartSafe :
                maintenance ? PowerMode.RemoteRestartMaintenance :
                PowerMode.RemoteRestart;
            string address = parameters.ArgumentsList[0];
            if (parameters.ArgumentsList.Length == 1)
                PowerManager.PowerManage(mode, address);
            else
            {
                string portNumStr = parameters.ArgumentsList[1];
                PowerManager.PowerManage(mode, address, int.Parse(portNumStr));
            }
            return 0;
        }

    }
}
