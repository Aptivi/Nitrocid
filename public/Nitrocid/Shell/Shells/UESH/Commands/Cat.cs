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
using Nitrocid.Files;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Prints files to console.
    /// </summary>
    /// <remarks>
    /// This command lets you print the contents of a text file to the console.
    /// <br></br>
    /// <list type="table">
    /// <listheader>
    /// <term>Switches</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>-lines</term>
    /// <description>Prints the line numbers alongside the contents</description>
    /// </item>
    /// <item>
    /// <term>-nolines</term>
    /// <description>Prints only the contents</description>
    /// </item>
    /// </list>
    /// <br></br>
    /// </remarks>
    class CatCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "cat";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CAT_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "file", new()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CAT_ARGUMENT_FILE_DESC"
                    }),
                ],
                [
                    new SwitchInfo("lines", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CAT_SWITCH_LINES_DESC", new SwitchOptions()
                    {
                        ConflictsWith = ["nolines"],
                        AcceptsValues = false
                    }),
                    new SwitchInfo("nolines", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CAT_SWITCH_NOLINES_DESC", new SwitchOptions()
                    {
                        ConflictsWith = ["lines"],
                        AcceptsValues = false
                    }),
                    new SwitchInfo("plain", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CAT_SWITCH_PLAIN_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    })
                ])
            ];

        public override CommandFlags Flags =>
            CommandFlags.RedirectionSupported | CommandFlags.Wrappable;

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            try
            {
                bool printLines = (parameters.ContainsSwitch("-lines") || Config.MainConfig.PrintLineNumbers) && !parameters.ContainsSwitch("-nolines");
                bool forcePlain = parameters.ContainsSwitch("-plain");
                string filePath = parameters.ArgumentsList[0];
                FilesystemTools.PrintContents(filePath, printLines, forcePlain);
                return 0;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriterColor.Write(ex.Message, true, ThemeColorType.Error);
                return ex.GetHashCode();
            }
        }

    }
}
