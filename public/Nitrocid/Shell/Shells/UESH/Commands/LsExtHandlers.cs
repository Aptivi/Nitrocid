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

using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using Nitrocid.Languages;
using Nitrocid.Files.Extensions;
using Terminaux.Shell.Shells;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Shows extension handlers and their implementers
    /// </summary>
    /// <remarks>
    /// This shows you a list of extensions and their handlers by their implementers.
    /// </remarks>
    class LsExtHandlersCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "lsexthandlers";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_LSEXTHANDLERS_DESC");

        public override CommandFlags Flags =>
            CommandFlags.RedirectionSupported | CommandFlags.Wrappable;

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            var handlers = ExtensionHandlerTools.GetExtensionHandlers();
            foreach (var handler in handlers)
            {
                SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_LSEXTHANDLERS_INFOFOR") + $" {handler.Extension}", ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_LSEXTHANDLERS_IMPLEMENTER"), handler.Implementer);
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_LSEXTHANDLERS_METADATA"), handler.MimeType);
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_LSEXTHANDLERS_HEADERINFO"), $"{handler.Handler is not null}");
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_LSEXTHANDLERS_HANDLERFUNCTION"), $"{handler.InfoHandler is not null}");
            }
            return 0;
        }

    }
}
