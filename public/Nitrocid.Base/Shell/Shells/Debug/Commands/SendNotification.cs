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

using System;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel;
using Nitrocid.Base.Kernel.Time.Renderers;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Notifications;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;

namespace Nitrocid.Base.Shell.Shells.Debug.Commands
{
    /// <summary>
    /// Sends a notification for testing
    /// </summary>
    class SendNotificationCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "sendnotification";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_DEBUG_COMMAND_SENDNOTIFICATION_DESC");

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string notificationTitle = $"{RandomDriver.Random(1000, 9999)} - {KernelReleaseInfo.VersionFullStr}";
            string notificationDesc = $"{KernelReleaseInfo.ApiVersion} - {TimeDateRenderers.Render()}";
            NotificationPriority[] priorities = Enum.GetValues<NotificationPriority>();
            NotificationPriority priority = priorities[RandomDriver.RandomIdx(priorities.Length)];
            var notification = new Notification(notificationTitle, notificationDesc, priority, NotificationType.Normal);
            NotificationManager.NotifySend(notification);
            return 0;
        }

    }
}
