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
using Terminaux.Shell.Commands;
using System.Linq;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Files.Extensions;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Gets the default extension handlers and their info
    /// </summary>
    /// <remarks>
    /// This command lets you know the default extension handlers for all extensions
    /// </remarks>
    class GetDefaultExtHandlersCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var handlers = ExtensionHandlerTools.defaultHandlers;
            for (int i = 0; i < handlers.Count; i++)
            {
                ExtensionHandler? handler = ExtensionHandlerTools.GetExtensionHandler(handlers.ElementAt(i).Key, handlers.ElementAt(i).Value);
                if (handler is null)
                    continue;
                SeparatorWriterColor.WriteSeparatorColor($"{i + 1}/{handlers.Count}", ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
                TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GETALLEXTHANDLERS_EXTENSION") + ": ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write(handler.Extension, ThemeColorType.ListValue);
                TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GETDEFAULTEXTHANDLER_DEFAULTHANDLER") + ": ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write(handler.Implementer, ThemeColorType.ListValue);
            }
            variableValue = $"[{string.Join(", ", handlers.Select((h) => h.Value))}]";
            return 0;
        }
    }
}
