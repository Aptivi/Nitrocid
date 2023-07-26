
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

using System;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Languages;
using KS.Misc.Notifications;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Dismisses a specific notification
    /// </summary>
    /// <remarks>
    /// Takes a notification number to dismiss a specified notification, if you're done with it, or you don't want clutter on your recents.
    /// <br></br>
    /// To show available notifications, consult shownotifs command.
    /// </remarks>
    class DismissNotifCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            int NotifIndex = (int)Math.Round(Convert.ToDouble(ListArgsOnly[0]) - 1d);
            if (NotificationManager.NotifDismiss(NotifIndex))
            {
                TextWriterColor.Write(Translate.DoTranslation("Notification dismissed successfully."));
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Error trying to dismiss notification."), true, KernelColorType.Error);
            }
        }

    }
}
