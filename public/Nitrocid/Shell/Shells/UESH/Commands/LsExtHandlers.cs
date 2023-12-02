﻿//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Files.Extensions;
using KS.Languages;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
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
                SeparatorWriterColor.WriteSeparatorKernelColor(Translate.DoTranslation("Extension handler information for") + $" {handler.Extension}", true, KernelColorType.ListTitle);
                TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("Handler implementer") + $": ", false, KernelColorType.ListEntry);
                TextWriterColor.WriteKernelColor(handler.Implementer, true, KernelColorType.ListValue);
                TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("Handler metadata") + $": ", false, KernelColorType.ListEntry);
                TextWriterColor.WriteKernelColor(handler.MimeType, true, KernelColorType.ListValue);
                TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("Contains handler function") + $": ", false, KernelColorType.ListEntry);
                TextWriterColor.WriteKernelColor($"{handler.Handler is not null}", true, KernelColorType.ListValue);
                TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("Contains info handler function") + $": ", false, KernelColorType.ListEntry);
                TextWriterColor.WriteKernelColor($"{handler.InfoHandler is not null}\n", true, KernelColorType.ListValue);
            }
            return 0;
        }

    }
}