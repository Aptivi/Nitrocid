//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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

using System.Collections.Generic;
using System.Linq;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Files;
using Terminaux.Shell.Commands;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Security.Permissions;

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

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            PermissionsTools.Demand(PermissionTypes.ManageFilesystem);
            string OutputPath = FilesystemTools.NeutralizePath(parameters.ArgumentsList[0]);
            string InputPath = parameters.ArgumentsList[1];
            var CombineInputPaths = parameters.ArgumentsList.Skip(2).ToArray();

            // Check all inputs
            bool AreAllInputsBinary = false;
            bool AreAllInputsText = false;
            bool IsInputBinary = FilesystemTools.IsBinaryFile(InputPath);

            // Get all the input states and make them true if all binary
            List<bool> InputStates = [];
            foreach (string CombineInputPath in CombineInputPaths)
                InputStates.Add(FilesystemTools.IsBinaryFile(CombineInputPath));

            // Check to see if all inputs are either binary or text.
            AreAllInputsBinary = InputStates.Count == InputStates.Where((binary) => binary).Count();
            AreAllInputsText = InputStates.Count == InputStates.Where((binary) => !binary).Count();
            if (!AreAllInputsBinary && !AreAllInputsText)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMBINE_MAYNOTMIX"), true, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Filesystem);
            }

            // Make a combined content array
            if (AreAllInputsText)
            {
                var CombinedContents = FilesystemTools.CombineTextFiles(InputPath, CombineInputPaths);
                FilesystemTools.MakeFile(OutputPath, false);
                FilesystemTools.WriteContents(OutputPath, CombinedContents);
            }
            else
            {
                var CombinedContents = FilesystemTools.CombineBinaryFiles(InputPath, CombineInputPaths);
                FilesystemTools.MakeFile(OutputPath, false);
                FilesystemTools.WriteAllBytes(OutputPath, CombinedContents);
            }
            return 0;
        }

    }
}
