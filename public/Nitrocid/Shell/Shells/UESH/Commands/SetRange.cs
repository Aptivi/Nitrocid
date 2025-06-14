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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Makes a UESH array and sets their values
    /// </summary>
    /// <remarks>
    /// If you want to store a group of values in one variable, you can use this command to create arrays of values. Such variables will have the [n] suffix, for example, $values[1].
    /// </remarks>
    class SetRangeCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (!parameters.SwitchSetPassed)
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_SET_NEEDSSWITCH"), KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.ShellOperation);
            }
            variableValue = $"[{string.Join(", ", [.. parameters.ArgumentsList])}]";
            return 0;
        }

    }
}
