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
using Nitrocid.Files.Extensions;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using System.Linq;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Gets the extension handlers with specific extension
    /// </summary>
    /// <remarks>
    /// This command lets you know all the extension handlers for a specified extension
    /// </remarks>
    class GetExtHandlersCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (!ExtensionHandlerTools.IsHandlerRegistered(parameters.ArgumentsList[0]))
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GETDEFAULTEXTHANDLER_NOEXT"), ThemeColorType.Error);
                return 22;
            }
            var handlers = ExtensionHandlerTools.GetExtensionHandlers(parameters.ArgumentsList[0]);
            for (int i = 0; i < handlers.Length; i++)
            {
                ExtensionHandler handler = handlers[i];
                SeparatorWriterColor.WriteSeparatorColor($"{i + 1}/{handlers.Length}", ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
                TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GETALLEXTHANDLERS_EXTENSION") + ": ", false, ThemeColorType.ListEntry);
                TextWriters.Write(handler.Extension, ThemeColorType.ListValue);
                TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GETALLEXTHANDLERS_EXTENSIONHANDLER") + ": ", false, ThemeColorType.ListEntry);
                TextWriters.Write(handler.Implementer, ThemeColorType.ListValue);
            }
            variableValue = $"[{string.Join(", ", handlers.Select((h) => h.Implementer))}]";
            return 0;
        }
    }
}
