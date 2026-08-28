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
using Nitrocid.Files.LineEndings;
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
    /// Converts the line endings
    /// </summary>
    /// <remarks>
    /// If you have a text file that needs a change for its line endings, you can use this command to convert the line endings to your platform's format, or the format of your choice by using these switches:
    /// <br></br>
    /// <list type="table">
    /// <listheader>
    /// <term>Switches</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>-w</term>
    /// <description>Converts the line endings to the Windows format (CR + LF)</description>
    /// </item>
    /// <item>
    /// <term>-u</term>
    /// <description>Converts the line endings to the Unix format (LF)</description>
    /// </item>
    /// <item>
    /// <term>-m</term>
    /// <description>Converts the line endings to the Mac OS 9 format (CR)</description>
    /// </item>
    /// <item>
    /// <term>-force</term>
    /// <description>Forces conversion</description>
    /// </item>
    /// </list>
    /// <br></br>
    /// </remarks>
    class ConvertLineEndingsCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "convertlineendings";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CONVERTLINEENDINGS_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "textfile", new()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CONVERTLINEENDINGS_ARGUMENT_TEXTFILE_DESC"
                    }),
                ],
                [
                    new SwitchInfo("w", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CONVERTLINEENDINGS_SWITCH_W_DESC", new SwitchOptions()
                    {
                        ConflictsWith = ["u", "m"],
                        AcceptsValues = false
                    }),
                    new SwitchInfo("u", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CONVERTLINEENDINGS_SWITCH_U_DESC", new SwitchOptions()
                    {
                        ConflictsWith = ["m", "w"],
                        AcceptsValues = false
                    }),
                    new SwitchInfo("m", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CONVERTLINEENDINGS_SWITCH_M_DESC", new SwitchOptions()
                    {
                        ConflictsWith = ["u", "w"],
                        AcceptsValues = false
                    }),
                    new SwitchInfo("force", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CONVERTLINEENDINGS_SWITCH_FORCE_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    }),
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string targetTextFile = parameters.ArgumentsList[0];
            var targetLineEnding =
                parameters.ContainsSwitch("-w") ? FilesystemNewlineStyle.CRLF :
                parameters.ContainsSwitch("-u") ? FilesystemNewlineStyle.LF :
                parameters.ContainsSwitch("-m") ? FilesystemNewlineStyle.CR :
                FilesystemTools.NewlineStyle;
            bool force = parameters.ContainsSwitch("-force");

            // Convert the line endings
            if (FilesystemTools.IsBinaryFile(targetTextFile) && !force)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CONVERTLINEENDINGS_BINARYFILE"), true, ThemeColorType.Error);
                return 7;
            }
            FilesystemTools.ConvertLineEndings(targetTextFile, targetLineEnding, force);
            return 0;
        }

    }
}
