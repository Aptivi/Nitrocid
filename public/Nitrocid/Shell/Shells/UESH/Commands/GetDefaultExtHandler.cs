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
using Nitrocid.Files.Extensions;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Gets the default extension handler name
    /// </summary>
    /// <remarks>
    /// This command lets you know the default extension handler for a specified extension
    /// </remarks>
    class GetDefaultExtHandlerCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (!ExtensionHandlerTools.IsHandlerRegistered(parameters.ArgumentsList[0]))
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GETDEFAULTEXTHANDLER_NOEXT"), KernelColorType.Error);
                return 21;
            }
            var handler = ExtensionHandlerTools.GetExtensionHandler(parameters.ArgumentsList[0]) ??
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_EXCEPTION_HANDLERFAILED") + $" {parameters.ArgumentsList[0]}");
            TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GETDEFAULTEXTHANDLER_DEFAULTHANDLER") + ": ", false, KernelColorType.ListEntry);
            TextWriters.Write(handler.Implementer, KernelColorType.ListValue);
            variableValue = handler.Implementer;
            return 0;
        }
    }
}
