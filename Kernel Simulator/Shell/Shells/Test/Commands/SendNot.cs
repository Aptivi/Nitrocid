

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
using KS.Shell.ShellBase.Commands;
using System;

namespace KS.Shell.Shells.Test.Commands
{
    /// <summary>
    /// It lets you test the notification system by sending the notification with the specified title and description on a specific priority.
    /// </summary>
    class Test_SendNotCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            var Notif = new Notification(ListArgsOnly[1], ListArgsOnly[2], (Notifications.NotifPriority)Convert.ToInt32(ListArgsOnly[0]), Notifications.NotifType.Normal);
            Notifications.NotifySend(Notif);
        }

    }
}
