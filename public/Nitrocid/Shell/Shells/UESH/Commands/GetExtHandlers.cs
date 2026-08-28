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

using System.Linq;
using Nitrocid.Files.Extensions;
using Nitrocid.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
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
        public override string Command => 
            "getexthandlers";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_GETEXTHANDLERS_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "extension", new CommandArgumentPartOptions()
                    {
                        AutoCompleter = (_) => ExtensionHandlerTools.GetExtensionHandlers().Select((h) => h.Extension).ToArray(),
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_GETDEFAULTEXTHANDLER_ARGUMENT_EXTENSION_DESC"
                    }),
                ], true)
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            if (!ExtensionHandlerTools.IsHandlerRegistered(parameters.ArgumentsList[0]))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GETDEFAULTEXTHANDLER_NOEXT"), ThemeColorType.Error);
                return 22;
            }
            var handlers = ExtensionHandlerTools.GetExtensionHandlers(parameters.ArgumentsList[0]);
            for (int i = 0; i < handlers.Length; i++)
            {
                ExtensionHandler handler = handlers[i];
                SeparatorWriterColor.WriteSeparatorColor($"{i + 1}/{handlers.Length}", ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
                ListEntryWriterColor.RenderListEntry(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GETALLEXTHANDLERS_EXTENSION"), handler.Extension);
                ListEntryWriterColor.RenderListEntry(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GETALLEXTHANDLERS_EXTENSIONHANDLER"), handler.Implementer);
            }
            variableValue = $"[{string.Join(", ", handlers.Select((h) => h.Implementer))}]";
            return 0;
        }
    }
}
