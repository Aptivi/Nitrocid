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
using Nitrocid.Base.Files;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// You can change your current working directory to another directory
    /// </summary>
    /// <remarks>
    /// You can change your working directory to another directory to execute listing commands, removing files, or creating directories on another directory.
    /// </remarks>
    class ChDirCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "chdir";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHDIR_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "directory/..", new()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_BULKRENAME_ARGUMENT_TARGETDIR_DESC"
                    }),
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            try
            {
                string directory = parameters.ArgumentsList[0];
                FilesystemTools.SetCurrDir(directory);
                return 0;
            }
            catch (Exception ex)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CHDIR_FAILURE"), true, ThemeColorType.Error, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                return ex.GetHashCode();
            }
        }

    }
}
