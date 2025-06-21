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
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using Terminaux.Base;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Makes the PC speaker beep.
    /// </summary>
    /// <remarks>
    /// This command lets you make a PC speaker beep. This requires that you have it installed.
    /// <br></br><br></br>
    /// This command used to give you an option to specify the time in milliseconds and the frequency, but it isn't cross-platform because it only works
    /// on Windows. With the help of a native utility to utilize the PC speaker completely in Linux, this would have been achieved. However, we're trying
    /// to be 100% .NET compatible and not depend on that utility. Fortunately, we've found two VT sequences that control the beep.
    /// </remarks>
    class BeepCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (parameters.ArgumentsList.Length >= 2)
            {
                if (!int.TryParse(parameters.ArgumentsList[0], out var freq))
                {
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_BEEP_INVALIDFREQ"), ThemeColorType.Error);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.Console);
                }
                if (!int.TryParse(parameters.ArgumentsList[1], out var ms))
                {
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_BEEP_INVALIDDURATION"), ThemeColorType.Error);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.Console);
                }
                ConsoleWrapper.Beep(freq, ms);
            }
            else
                ConsoleWrapper.Beep();
            return 0;
        }
    }
}
