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

using Nitrocid.Base.Files;
using Nitrocid.Base.Files.Unix;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Unix permissions
    /// </summary>
    class UnixPermCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "unixperm";

        // TODO: NKS_SHELL_SHELLS_UESH_COMMAND_UNIXPERM_DESC -> Sets Unix permissions of a file
        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_UNIXPERM_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "num", new()
                    {
                        // TODO: NKS_SHELL_SHELLS_UESH_COMMAND_UNIXPERMCALC_ARGUMENT_REPRESENTATION_NUM_DESC -> Read, write, or execute permissions as "chmod" number
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_UNIXPERMCALC_ARGUMENT_REPRESENTATION_NUM_DESC",
                        IsNumeric = true,
                    }),
                    new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                    {
                        // TODO: NKS_SHELL_SHELLS_UESH_COMMAND_UNIXPERM_ARGUMENT_FILE_DESC -> Path to file to set Unix permissions
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_UNIXPERM_ARGUMENT_FILE_DESC"
                    }),
                ],
                [
                    // TODO: NKS_SHELL_SHELLS_UESH_COMMAND_UNIXPERM_SWITCH_SETUID_DESC -> Set user ID
                    new SwitchInfo("setuid", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_UNIXPERM_SWITCH_SETUID_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    }),
                    // TODO: NKS_SHELL_SHELLS_UESH_COMMAND_UNIXPERM_SWITCH_SETGID_DESC -> Set group ID
                    new SwitchInfo("setgid", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_UNIXPERM_SWITCH_SETGID_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    }),
                    // TODO: NKS_SHELL_SHELLS_UESH_COMMAND_UNIXPERM_SWITCH_STICKY_DESC -> Set sticky bit
                    new SwitchInfo("sticky", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_UNIXPERM_SWITCH_STICKY_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    }),
                ]),
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            // Parse the permissions number and get the descriptors
            int chmodNum = int.Parse(parameters.ArgumentsList[0]);
            var descriptors = UnixPermissionManager.GetDescriptors(chmodNum);
            string file = parameters.ArgumentsList[1];

            // Get special permissions
            bool hasSetUid = parameters.ContainsSwitch("-setuid");
            bool hasSetGid = parameters.ContainsSwitch("-setgid");
            bool hasSticky = parameters.ContainsSwitch("-sticky");
            var special = UnixPermissionSpecial.None;
            if (hasSetUid)
                special |= UnixPermissionSpecial.SetUid;
            if (hasSetGid)
                special |= UnixPermissionSpecial.SetGid;
            if (hasSticky)
                special |= UnixPermissionSpecial.Sticky;

            // Set the permissions
            FilesystemTools.SetUnixFileMode(file, descriptors, special);
            return 0;
        }

    }
}
