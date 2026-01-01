//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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

using Nitrocid.Languages;
using Nitrocid.Misc.Notifications;
using Nitrocid.Misc.Splash;
using System.Threading;

namespace Nitrocid.Kernel.Debugging.Testing.Facades
{
    internal class SendNotificationProg : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Sends a progress notification to test the receiver");
        public override void Run(params string[] args)
        {
            SplashReport._KernelBooted = true;
            var Notif = new Notification(Translate.DoTranslation("Test progress notification"), Translate.DoTranslation("Description is here"), NotificationPriority.Low, NotificationType.Progress);
            NotificationManager.NotifySend(Notif);
            while (!Notif.ProgressCompleted)
            {
                Thread.Sleep(100);
                Notif.Progress += 1;
                if (Notif.Progress == 100)
                    Notif.ProgressState = NotificationProgressState.Success;
            }
            SplashReport._KernelBooted = false;
        }
    }
}
