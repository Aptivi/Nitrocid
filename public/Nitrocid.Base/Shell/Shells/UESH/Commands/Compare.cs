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

using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using Terminaux.Themes.Colors;
using Nitrocid.Base.Files;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Exceptions;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
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
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMPARE_SOURCENOTFOUND"), ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Filesystem);
            }
            if (!FilesystemTools.FileExists(pathTwo))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMPARE_TARGETNOTFOUND"), ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Filesystem);
            }
            var compared = FilesystemTools.Compare(pathOne, pathTwo);
            if (compared.Length == 0)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMPARE_IDENTICAL"), ThemeColorType.Warning);
                return 0;
            }
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMPARE_DIFFERENT"));
            foreach (var (line, one, two) in compared)
            {
                ListEntryWriterColor.WriteListEntry($"[{line}]", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMPARE_DETAILS_DIFFERENT"), ThemeColorType.ListEntry, ThemeColorType.ListValue);
                ListEntryWriterColor.WriteListEntry("[-]", one, ThemeColorType.ListEntry, ThemeColorType.ListValue, 1);
                ListEntryWriterColor.WriteListEntry("[+]", two, ThemeColorType.ListEntry, ThemeColorType.ListValue, 1);
            }
            return 0;
        }
    }
}
