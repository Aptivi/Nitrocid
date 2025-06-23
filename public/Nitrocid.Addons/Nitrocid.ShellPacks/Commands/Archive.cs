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

using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Files;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;

namespace Nitrocid.ShellPacks.Commands
{
    /// <summary>
    /// Opens an archive file to the archive shell
    /// </summary>
    /// <remarks>
    /// If you want to interact with the archive files, like extracting them, use this command. For now, only RAR and ZIP files are supported.
    /// </remarks>
    class ArchiveCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            parameters.ArgumentsList[0] = FilesystemTools.NeutralizePath(parameters.ArgumentsList[0]);
            DebugWriter.WriteDebug(DebugLevel.I, "File path is {0} and .Exists is {0}", vars: [parameters.ArgumentsList[0], FilesystemTools.FileExists(parameters.ArgumentsList[0])]);
            if (FilesystemTools.FileExists(parameters.ArgumentsList[0]))
            {
                ShellManager.StartShell("ArchiveShell", parameters.ArgumentsList[0]);
            }
            else
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_ARCHIVE_FILENOTFOUND"), true, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Filesystem);
            }
            return 0;
        }

    }
}
