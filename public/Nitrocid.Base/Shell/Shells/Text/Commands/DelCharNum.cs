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

using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using System;
using Textify.General;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Files.Editors.TextEdit;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Exceptions;

namespace Nitrocid.Base.Shell.Shells.Text.Commands
{
    /// <summary>
    /// Deletes a character from character number in specified line.
    /// </summary>
    /// <remarks>
    /// You can use this command to delete a character using a character number in a specified line. You can revise the print command output, but it will only tell you the line number and not the character number. To solve the problem, use the querychar command.
    /// </remarks>
    class DelCharNumCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (TextTools.IsStringNumeric(parameters.ArgumentsList[1]) & TextTools.IsStringNumeric(parameters.ArgumentsList[0]))
            {
                if (Convert.ToInt32(parameters.ArgumentsList[1]) <= TextEditShellCommon.FileLines.Count)
                {
                    TextEditTools.DeleteChar(Convert.ToInt32(parameters.ArgumentsList[0]), Convert.ToInt32(parameters.ArgumentsList[1]));
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_TEXT_DELCHARNUM_SUCCESS"), true, ThemeColorType.Success);
                    return 0;
                }
                else
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_LINENUMEXCEEDSLASTNUM"), true, ThemeColorType.Error);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.TextEditor);
                }
            }
            else
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_TEXT_DELCHARNUM_NUMINVALID"), true, ThemeColorType.Error);
                DebugWriter.WriteDebug(DebugLevel.E, "{0} and {1} are not numeric values.", vars: [parameters.ArgumentsList[0], parameters.ArgumentsList[1]]);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.TextEditor);
            }
        }

    }
}
