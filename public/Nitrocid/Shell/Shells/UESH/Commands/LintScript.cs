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
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Scripting;
using System;
using Terminaux.Base;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    class LintScriptCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            try
            {
                string pathToScript = FilesystemTools.NeutralizePath(parameters.ArgumentsList[0]);
                MESHParse.Execute(pathToScript, "", true);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_LINTSCRIPT_SUCCESS"), true, ThemeColorType.Success);
                variableValue = "1";
                return 0;
            }
            catch (TerminauxException tex)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_LINTSCRIPT_FAILED"), true, ThemeColorType.Error);
                TextWriterColor.Write(tex.Message, true, ThemeColorType.Error);
                variableValue = "0";
                return tex.GetHashCode();
            }
            catch (Exception ex)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_LINTSCRIPT_UNEXPECTEDERROR") + $" {ex.Message}", true, ThemeColorType.Error);
                variableValue = "0";
                return ex.GetHashCode();
            }
        }

    }
}
