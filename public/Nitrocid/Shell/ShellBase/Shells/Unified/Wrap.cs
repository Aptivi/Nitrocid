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

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.ConsoleBase.Writers;

namespace Nitrocid.Shell.ShellBase.Shells.Unified
{
    /// <summary>
    /// Wraps a command
    /// </summary>
    /// <remarks>
    /// You can wrap a command so it stops outputting until you press a key if the console has printed lines that exceed the console window height. Only the commands that are explicitly set to be wrappable can be used with this command.
    /// </remarks>
    class WrapUnifiedCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            CommandExecutor.ExecuteCommandWrapped(parameters.ArgumentsText);
            return 0;
        }

        public override void HelpHelper()
        {
            // Print the wrappable commands along with help description
            var currentShell = ShellManager.ShellStack[^1];
            var currentType = currentShell.ShellType;
            var WrappableCmds = CommandExecutor.GetWrappableCommands(currentType);
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_WRAP_COMMANDSHELPER"));
            TextWriters.WriteList(WrappableCmds);
        }

    }
}
