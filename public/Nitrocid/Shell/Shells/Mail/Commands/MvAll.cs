
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
using KS.Languages;
using KS.Network.Mail.Directory;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.Mail.Commands
{
    /// <summary>
    /// Moves all messages from a recipient to a folder
    /// </summary>
    /// <remarks>
    /// It allows you to bulk move all messages from a recipient to a folder. Depending on the number of messages in the folder, it may take some time to move all messages.
    /// </remarks>
    class Mail_MvAllCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (MailManager.MailMoveAllBySender(ListArgsOnly[0], ListArgsOnly[1]))
            {
                TextWriterColor.Write(Translate.DoTranslation("All mail made by {0} are moved successfully."), true, KernelColorType.Success, ListArgsOnly[0]);
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Failed to move all mail made by {0}."), true, KernelColorType.Error, ListArgsOnly[0]);
            }
        }

    }
}