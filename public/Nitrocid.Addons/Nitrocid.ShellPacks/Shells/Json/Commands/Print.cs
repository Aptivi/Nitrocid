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

using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.ShellPacks.Shells.Json.Commands
{
    /// <summary>
    /// Prints a property or the whole file
    /// </summary>
    /// <remarks>
    /// You can use this command to print the contents of either the full JSON file or a property.
    /// </remarks>
    class PrintCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "print";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_COMMAND_PRINT_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(false, "propertyName", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_JSON_COMMAND_ARGUMENT_PROPERTYNAME_DESC"
                    })
                ])
            ];

        public override CommandFlags Flags =>
            CommandFlags.Wrappable;

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            var jsonShell = (JsonShell?)shell ??
                throw new KernelException(KernelExceptionType.JsonEditor, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_LASTSHELLTYPEMISMATCH"));
            if (parameters.ArgumentsList.Length > 0)
                TextWriterColor.Write(jsonShell.SerializeToString(parameters.ArgumentsText));
            else
                TextWriterColor.Write(jsonShell.SerializeToString("$"));
            return 0;
        }

    }
}
