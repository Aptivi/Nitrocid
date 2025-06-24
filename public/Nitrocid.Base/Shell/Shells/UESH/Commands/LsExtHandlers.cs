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
using Nitrocid.Base.Languages;
using Nitrocid.Base.Files.Extensions;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Shows extension handlers and their implementers
    /// </summary>
    /// <remarks>
    /// This shows you a list of extensions and their handlers by their implementers.
    /// </remarks>
    class LsExtHandlersCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var handlers = ExtensionHandlerTools.GetExtensionHandlers();
            foreach (var handler in handlers)
            {
                SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_LSEXTHANDLERS_INFOFOR") + $" {handler.Extension}", ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
                TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_LSEXTHANDLERS_IMPLEMENTER") + $": ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write(handler.Implementer, true, ThemeColorType.ListValue);
                TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_LSEXTHANDLERS_METADATA") + $": ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write(handler.MimeType, true, ThemeColorType.ListValue);
                TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_LSEXTHANDLERS_HEADERINFO") + $": ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write($"{handler.Handler is not null}", true, ThemeColorType.ListValue);
                TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_LSEXTHANDLERS_HANDLERFUNCTION") + $": ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write($"{handler.InfoHandler is not null}\n", true, ThemeColorType.ListValue);
            }
            return 0;
        }

    }
}
