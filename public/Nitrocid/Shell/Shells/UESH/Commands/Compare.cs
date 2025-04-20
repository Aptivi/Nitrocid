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

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Files;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Compares two text files
    /// </summary>
    /// <remarks>
    /// This command will compare two text files and print the differences to the console.
    /// </remarks>
    class CompareCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string pathOne = FilesystemTools.NeutralizePath(parameters.ArgumentsList[0]);
            string pathTwo = FilesystemTools.NeutralizePath(parameters.ArgumentsList[1]);

            if (!FilesystemTools.FileExists(pathOne))
            {
                TextWriters.Write(Translate.DoTranslation("Source file doesn't exist."), KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Filesystem);
            }
            if (!FilesystemTools.FileExists(pathTwo))
            {
                TextWriters.Write(Translate.DoTranslation("Target file doesn't exist."), KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Filesystem);
            }
            var compared = FilesystemTools.Compare(pathOne, pathTwo);
            if (compared.Length == 0)
            {
                TextWriters.Write(Translate.DoTranslation("The two files are identical."), KernelColorType.Warning);
                return 0;
            }
            TextWriterColor.Write(Translate.DoTranslation("The two files are different."));
            foreach (var (line, one, two) in compared)
            {
                TextWriters.WriteListEntry($"[{line}]", Translate.DoTranslation("Different"), KernelColorType.ListEntry, KernelColorType.ListValue);
                TextWriters.WriteListEntry("[-]", one, KernelColorType.ListEntry, KernelColorType.ListValue, 1);
                TextWriters.WriteListEntry("[+]", two, KernelColorType.ListEntry, KernelColorType.ListValue, 1);
            }
            return 0;
        }
    }
}
