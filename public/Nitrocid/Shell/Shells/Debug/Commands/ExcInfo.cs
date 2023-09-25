
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
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Exceptions;
using KS.Files;
using KS.Files.Folders;
using System.IO;
using System;

namespace KS.Shell.Shells.Debug.Commands
{
    /// <summary>
    /// You can view an exception info
    /// </summary>
    /// <remarks>
    /// This command lets you view the exception information from the list of available kernel exceptions, including the name, the number, and the message.
    /// </remarks>
    class Debug_ExcInfoCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string StringArgsOrig, string[] ListArgsOnlyOrig, string[] ListSwitchesOnly, ref string variableValue)
        {
            // Check to see if we really have the type
            string exceptionStr = ListArgsOnly[0];
            if (!Enum.TryParse(exceptionStr, out KernelExceptionType type))
            {
                // There is no such exception number being requested
                TextWriterColor.Write(Translate.DoTranslation("No such exception type") + $" {exceptionStr}", true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Debug;
            }

            // Get the exception type and its message
            TextWriterColor.Write("- " + Translate.DoTranslation("Exception type name") + ": ", false, KernelColorType.ListEntry);
            TextWriterColor.Write($"{type} [{(int)type}]", true, KernelColorType.ListValue);
            TextWriterColor.Write("- " + Translate.DoTranslation("Message") + ": ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(KernelExceptionMessages.GetMessageFromType(type), true, KernelColorType.ListValue);
            return 0;
        }

    }
}
