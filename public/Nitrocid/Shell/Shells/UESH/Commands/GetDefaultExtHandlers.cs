
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
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Files.Extensions;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using System.Linq;

namespace KS.Shell.Shells.UESH.Commands
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
            var handlers = ExtensionHandlerTools.GetExtensionHandlers();
            for (int i = 0; i < handlers.Length; i++)
            {
                ExtensionHandler handler = handlers[i];
                SeparatorWriterColor.WriteSeparatorKernelColor($"{i + 1}/{handlers.Length}", KernelColorType.ListTitle);
                TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("Extension") + ": ", false, KernelColorType.ListEntry);
                TextWriterColor.WriteKernelColor(handler.Extension, KernelColorType.ListValue);
                TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("Default extension handler") + ": ", false, KernelColorType.ListEntry);
                TextWriterColor.WriteKernelColor(handler.Implementer, KernelColorType.ListValue);
            }
            variableValue = $"[{string.Join(", ", handlers.Select((h) => h.Implementer))}]";
            return 0;
        }
    }
}
