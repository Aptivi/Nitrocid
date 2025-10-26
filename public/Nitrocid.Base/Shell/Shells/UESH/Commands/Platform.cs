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

using System;
using System.Runtime.InteropServices;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using Nitrocid.Base.Kernel;

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

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            bool ShowName = parameters.SwitchesList.Length > 0 && parameters.ContainsSwitch("-n") || parameters.SwitchesList.Length == 0;
            bool ShowVersion = parameters.SwitchesList.Length > 0 && parameters.ContainsSwitch("-v");
            bool ShowBits = parameters.SwitchesList.Length > 0 && parameters.ContainsSwitch("-b");
            bool ShowCoreClr = parameters.SwitchesList.Length > 0 && parameters.ContainsSwitch("-c");
            bool ShowRid = parameters.SwitchesList.Length > 0 && parameters.ContainsSwitch("-r");

            // Get the platform info according to the provided switches
            if (ShowName)
            {
                string platform =
                    KernelPlatform.IsOnWindows() ? "Windows" :
                    KernelPlatform.IsOnUnix() ? "Unix" :
                    KernelPlatform.IsOnMacOS() ? "macOS" :
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
