
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Files;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Scripting;
using System;

namespace KS.Shell.Shells.UESH.Commands
{
    class LintScriptCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, ref string variableValue)
        {
            try
            {
                string pathToScript = Filesystem.NeutralizePath(ListArgsOnly[0]);
                UESHParse.Execute(pathToScript, "", true);
                TextWriterColor.Write(Translate.DoTranslation("Script lint succeeded."), true, KernelColorType.Success);
                return 0;
            }
            catch (KernelException kex) when (kex.ExceptionType == KernelExceptionType.UESHScript)
            {
                TextWriterColor.Write(Translate.DoTranslation("Script lint failed. Most likely there is a syntax error. Check your script for errors and retry running the linter."), true, KernelColorType.Error);
                TextWriterColor.Write(kex.Message, true, KernelColorType.Error);
                return 10000 + (int)kex.ExceptionType;
            }
            catch (Exception ex)
            {
                TextWriterColor.Write(Translate.DoTranslation("Script linter failed unexpectedly trying to parse your script.") + $" {ex.Message}", true, KernelColorType.Error);
                return ex.GetHashCode();
            }
        }

    }
}
