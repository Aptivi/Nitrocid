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

using System;
using System.Runtime.InteropServices;
using Nitrocid.Base.Languages;
using SpecProbe.Software.Platform;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// This command prints your current platform
    /// </summary>
    /// <remarks>
    /// This command prints your current platform. If invoked with -set, will also set the indicated variable to the platform, depending on the switches passed.
    /// </remarks>
    class PlatformCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "platform";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_PLATFORM_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new SwitchInfo("n", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_PLATFORM_SWITCH_N_DESC", new SwitchOptions()
                    {
                        ConflictsWith = ["r", "v", "b", "c"],
                        AcceptsValues = false
                    }),
                    new SwitchInfo("v", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_PLATFORM_SWITCH_V_DESC", new SwitchOptions()
                    {
                        ConflictsWith = ["n", "r", "b", "c"],
                        AcceptsValues = false
                    }),
                    new SwitchInfo("b", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_PLATFORM_SWITCH_B_DESC", new SwitchOptions()
                    {
                        ConflictsWith = ["n", "v", "r", "c"],
                        AcceptsValues = false
                    }),
                    new SwitchInfo("c", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_PLATFORM_SWITCH_C_DESC", new SwitchOptions()
                    {
                        ConflictsWith = ["n", "v", "b", "r"],
                        AcceptsValues = false
                    }),
                    new SwitchInfo("r", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_PLATFORM_SWITCH_R_DESC", new SwitchOptions()
                    {
                        ConflictsWith = ["n", "v", "b", "c"],
                        AcceptsValues = false
                    })
                ], true)
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            bool ShowName = parameters.ContainsSwitch("-n") || parameters.SwitchesList.Length == 0;
            bool ShowVersion = parameters.ContainsSwitch("-v");
            bool ShowBits = parameters.ContainsSwitch("-b");
            bool ShowCoreClr = parameters.ContainsSwitch("-c");
            bool ShowRid = parameters.ContainsSwitch("-r");

            // Get the platform info according to the provided switches
            if (ShowName)
            {
                string platform =
                    PlatformHelper.IsOnWindows() ? "Windows" :
                    PlatformHelper.IsOnMacOS() ? "macOS" :
                    PlatformHelper.IsOnFreeBSD() ? "FreeBSD" :
                    PlatformHelper.IsOnUnix() ? "Unix" :
                    "Unknown";
                TextWriterColor.Write(platform);
                variableValue = platform;
            }
            else if (ShowVersion)
            {
                var platformVer = Environment.OSVersion.Version;
                string platformVerString = platformVer.ToString();
                bool result = long.TryParse($"{platformVer.Major:000}{platformVer.Minor:000}{platformVer.Build:000}{platformVer.Revision:000}", out long currentVersionDecimal);
                TextWriterColor.Write(platformVerString);
                if (!result)
                    return 6;
                variableValue = $"{currentVersionDecimal}";
            }
            else if (ShowBits)
            {
                string bits = RuntimeInformation.OSArchitecture.ToString();
                TextWriterColor.Write(bits);
                variableValue = bits;
            }
            else if (ShowCoreClr)
            {
                string framework = RuntimeInformation.FrameworkDescription;
                TextWriterColor.Write(framework);
                variableValue = framework;
            }
            else if (ShowRid)
            {
                string rid = RuntimeInformation.RuntimeIdentifier;
                TextWriterColor.Write(rid);
                variableValue = rid;
            }
            return 0;
        }

    }
}
