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

using System.Collections.Generic;
using System.Linq;
using Nitrocid.Base.Files;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Security.Permissions;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Combines the two text files or more into the output file.
    /// </summary>
    /// <remarks>
    /// If you have two or more fragments of a complete text file, you can combine them using this command to generate a complete text file.
    /// </remarks>
    class CombineCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "combinestr";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_COMBINESTR_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
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
                ], true, true)
            ];

        public override CommandFlags Flags =>
            CommandFlags.RedirectionSupported | CommandFlags.Wrappable;

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            PermissionsTools.Demand(PermissionTypes.ManageFilesystem);
            string outputPath = FilesystemTools.NeutralizePath(parameters.ArgumentsList[0]);
            string inputPath = parameters.ArgumentsList[1];
            var combineInputPaths = parameters.ArgumentsList.Skip(2).ToArray();

            // Check all inputs
            bool areAllInputsBinary = false;
            bool areAllInputsText = false;
            bool isInputBinary = FilesystemTools.IsBinaryFile(inputPath);

            // Get all the input states and make them true if all binary
            List<bool> InputStates = [];
            foreach (string CombineInputPath in combineInputPaths)
                InputStates.Add(FilesystemTools.IsBinaryFile(CombineInputPath));

            // Check to see if all inputs are either binary or text.
            areAllInputsBinary = InputStates.Count == InputStates.Count((binary) => binary);
            areAllInputsText = InputStates.Count == InputStates.Count((binary) => !binary);
            if (!areAllInputsBinary && !areAllInputsText)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMBINE_MAYNOTMIX"), true, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Filesystem);
            }

            // Make a combined content array
            if (areAllInputsText)
            {
                var CombinedContents = FilesystemTools.CombineTextFiles(inputPath, combineInputPaths);
                FilesystemTools.MakeFile(outputPath, false);
                FilesystemTools.WriteContents(outputPath, CombinedContents);
            }
            else
            {
                var CombinedContents = FilesystemTools.CombineBinaryFiles(inputPath, combineInputPaths);
                FilesystemTools.MakeFile(outputPath, false);
                FilesystemTools.WriteAllBytes(outputPath, CombinedContents);
            }
            return 0;
        }

    }
}
