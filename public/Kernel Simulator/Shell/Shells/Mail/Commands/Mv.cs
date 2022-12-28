
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.ConsoleBase.Colors;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Misc.Writers.ConsoleWriters;
using KS.Network.Mail.Directory;
using KS.Shell.ShellBase.Commands;
using System;

namespace KS.Shell.Shells.Mail.Commands
{
    /// <summary>
    /// Moves a message to a folder
    /// </summary>
    /// <remarks>
    /// This command lets you move a message to a folder. It is especially useful when organizing mail messages manually.
    /// </remarks>
    class Mail_MvCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Message number is numeric? {0}", StringQuery.IsStringNumeric(ListArgsOnly[0]));
            if (StringQuery.IsStringNumeric(ListArgsOnly[0]))
            {
                MailManager.MailMoveMessage(Convert.ToInt32(ListArgsOnly[0]), ListArgsOnly[1]);
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Message number is not a numeric value."), true, KernelColorType.Error);
            }
        }

    }
}
