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

using Nitrocid.Base.Languages;
using Nitrocid.Base.Network.Transfer;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Returns your public IP exposed to the internet
    /// </summary>
    /// <remarks>
    /// If you want to know your public IP exposed to the internet, you can use this command to show it. Unlike private IP addresses, you'll need to connect to the internet to be able to get your external IP.
    /// </remarks>
    class ExtIpCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "extip";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_EXTIP_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new SwitchInfo("quiet", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SWITCH_QUIET_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false,
                    }),
                ], true)
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            variableValue = NetworkTransfer.DownloadString("http://ipv4.icanhazip.com/", false).TrimEnd('\n');
            if (!parameters.ContainsSwitch("-quiet"))
                TextWriterColor.Write(variableValue);
            return 0;
        }
    }
}
