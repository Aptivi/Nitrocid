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
using Nitrocid.Base.Files.Extensions;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Sets the default extension handler
    /// </summary>
    /// <remarks>
    /// This command lets you set the default extension handler to a specified implementer
    /// </remarks>
    class SetExtHandlerCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "setexthandler";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SETEXTHANDLER_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "extension", new CommandArgumentPartOptions()
                    {
                        AutoCompleter = (_) => ExtensionHandlerTools.GetExtensionHandlers().Select((h) => h.Extension).ToArray(),
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_GETDEFAULTEXTHANDLER_ARGUMENT_EXTENSION_DESC"
                    }),
                    new CommandArgumentPart(true, "implementer", new CommandArgumentPartOptions()
                    {
                        AutoCompleter = (args) => ExtensionHandlerTools.GetExtensionHandlers(args[0]).Select((h) => h.Implementer).ToArray(),
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SETEXTHANDLER_ARGUMENT_IMPLEMENTER_DESC"
                    }),
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string extension = parameters.ArgumentsList[0];
            string implementer = parameters.ArgumentsList[1];
            if (!ExtensionHandlerTools.IsHandlerRegistered(extension))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GETDEFAULTEXTHANDLER_NOEXT"), ThemeColorType.Error);
                return 23;
            }
            if (!ExtensionHandlerTools.IsHandlerRegisteredSpecific(extension, implementer))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FILES_EXTENSIONS_EXCEPTION_NEEDSIMPLEMENTER"), ThemeColorType.Error);
                return 24;
            }
            ExtensionHandlerTools.SetExtensionHandler(extension, implementer);
            return 0;
        }
    }
}
