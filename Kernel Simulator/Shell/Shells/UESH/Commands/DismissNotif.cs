using System;
using KS.ConsoleBase.Colors;
using KS.Languages;

// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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

using KS.Misc.Notifications;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;
using Microsoft.VisualBasic.CompilerServices;

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
    class DismissNotifCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            int NotifIndex = (int)Math.Round(Conversions.ToDouble(ListArgsOnly[0]) - 1d);
            if (Notifications.NotifDismiss(NotifIndex))
            {
                TextWriterColor.Write(Translate.DoTranslation("Notification dismissed successfully."), true, ColorTools.ColTypes.Neutral);
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Error trying to dismiss notification."), true, ColorTools.ColTypes.Error);
            }
        }

    }
}