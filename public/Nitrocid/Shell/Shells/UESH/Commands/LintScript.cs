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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Files;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Scripting;
using System;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    class LintScriptCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            try
            {
                string pathToScript = FilesystemTools.NeutralizePath(parameters.ArgumentsList[0]);
                UESHParse.Execute(pathToScript, "", true);
                TextWriters.Write(Translate.DoTranslation("Script lint succeeded."), true, KernelColorType.Success);
                variableValue = "1";
                return 0;
            }
            catch (KernelException kex) when (kex.ExceptionType == KernelExceptionType.UESHScript)
            {
                TextWriters.Write(Translate.DoTranslation("Script lint failed. Most likely there is a syntax error. Check your script for errors and retry running the linter."), true, KernelColorType.Error);
                TextWriters.Write(kex.Message, true, KernelColorType.Error);
                variableValue = "0";
                return KernelExceptionTools.GetErrorCode(kex.ExceptionType);
            }
            catch (Exception ex)
            {
                TextWriters.Write(Translate.DoTranslation("Script linter failed unexpectedly trying to parse your script.") + $" {ex.Message}", true, KernelColorType.Error);
                variableValue = "0";
                return ex.GetHashCode();
            }
        }

    }
}
