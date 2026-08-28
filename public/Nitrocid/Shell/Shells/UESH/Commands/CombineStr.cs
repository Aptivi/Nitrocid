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

using System.Linq;
using Nitrocid.Files;
using Nitrocid.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Combines the two text files or more into the console.
    /// </summary>
    /// <remarks>
    /// If you have two or more fragments of a complete text file, you can combine them using this command to print the complete text file to toe console.
    /// </remarks>
    class CombineStrCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "combine";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_COMBINE_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "output", new()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_COMBINE_ARGUMENT_OUTPUT_DESC"
                    }),
                    new CommandArgumentPart(true, "input", new()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_COMBINESTR_ARGUMENT_FIRSTINPUT_DESC"
                    }),
                    new CommandArgumentPart(true, "input2", new()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_COMBINESTR_ARGUMENT_SECONDINPUT_DESC"
                    }),
                    new CommandArgumentPart(false, "input3", new()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_COMBINESTR_ARGUMENT_THIRDINPUT_DESC"
                    }),
                ], false, true)
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string inputPath = parameters.ArgumentsList[0];
            var combineInputPaths = parameters.ArgumentsList.Skip(1).ToArray();

            // Make a combined content array
            var combinedContents = FilesystemTools.CombineTextFiles(inputPath, combineInputPaths);
            string combinedContentsStr = string.Join("\n", combinedContents);
            TextWriterColor.Write(combinedContentsStr);
            variableValue = combinedContentsStr;
            return 0;
        }

    }
}
