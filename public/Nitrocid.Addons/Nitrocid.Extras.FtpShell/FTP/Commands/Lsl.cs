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

using Nitrocid.Files;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;

namespace Nitrocid.Extras.FtpShell.FTP.Commands
{
    /// <summary>
    /// Lists the contents of the current folder or the folder provided
    /// </summary>
    /// <remarks>
    /// You can see the list of the files and sub-directories contained in the current working directory if no directories are specified, or in the specified directory, if specified.
    /// <br></br>
    /// You can also see the list of the files and sub-directories contained in the previous directory of your current position.
    /// <br></br>
    /// <list type="table">
    /// <listheader>
    /// <term>Switches</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>-showdetails</term>
    /// <description>Shows the details of the files and folders</description>
    /// </item>
    /// <item>
    /// <term>-suppressmessages</term>
    /// <description>Suppresses the "unauthorized" messages</description>
    /// </item>
    /// </list>
    /// <br></br>
    /// </remarks>
    class LslCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "lsl";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FS_COMMAND_LSL_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(false, "dir", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_LOCALDIR_DESC"
                    })
                ],
                [
                    new SwitchInfo("showdetails", /* Localizable */ "NKS_SHELLPACKS_FTP_COMMAND_SWITCH_SHOWDETAILS_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    }),
                    new SwitchInfo("suppressmessages", /* Localizable */ "NKS_SHELLPACKS_FTP_COMMAND_SWITCH_SUPPRESSMESSAGES_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    })
                ])
            ];

        public override CommandFlags Flags =>
            CommandFlags.Wrappable;

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            bool ShowFileDetails = parameters.ContainsSwitch("-showdetails") || Config.MainConfig.ShowFileDetailsList;
            bool SuppressUnauthorizedMessage = parameters.ContainsSwitch("-suppressmessages") || Config.MainConfig.SuppressUnauthorizedMessages;
            if (parameters.ArgumentsList?.Length == 0)
                FilesystemTools.List(FTPShellCommon.FtpCurrentDirectory, ShowFileDetails, SuppressUnauthorizedMessage);
            else
            {
                foreach (string Directory in parameters.ArgumentsList ?? [])
                {
                    string direct = FilesystemTools.NeutralizePath(Directory);
                    FilesystemTools.List(direct, ShowFileDetails, SuppressUnauthorizedMessage);
                }
            }
            return 0;
        }

    }
}
