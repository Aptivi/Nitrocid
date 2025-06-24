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

using Nitrocid.Base.Misc.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Collections.Generic;
using Terminaux.Colors;

namespace Nitrocid.Tests.Misc.Notifications
{

    [TestClass]
    public class NotificationTests
    {

        /// <summary>
        /// Tests notification creation
        /// </summary>
        [TestMethod]
        public void TestNotifyCreate()
        {
            var Notif = new Notification("This is the title.", "This is the description.", NotificationPriority.Medium, NotificationType.Normal);
            Notif.ShouldNotBeNull();
        }

        /// <summary>
        /// Tests custom notification creation
        /// </summary>
        [TestMethod]
        public void TestNotifyCreateCustom()
        {
            var Notif = new Notification("This is the title.", "This is the description.", NotificationPriority.Custom, NotificationType.Normal)
            {
                CustomBeepTimes = 5,
                CustomColor = new Color(4),
                CustomDescriptionColor = new Color(4),
                CustomTitleColor = new Color(4),
                CustomProgressColor = new Color(4),
                CustomProgressFailureColor = new Color(4),
                CustomLeftFrameChar = '|',
                CustomRightFrameChar = '|',
                CustomUpperFrameChar = '-',
                CustomLowerFrameChar = '-',
                CustomUpperLeftCornerChar = '+',
                CustomUpperRightCornerChar = '+',
                CustomLowerLeftCornerChar = '+',
                CustomLowerRightCornerChar = '+'
            };
            Notif.ShouldNotBeNull();
        }

        /// <summary>
        /// Tests notification sending
        /// </summary>
        [TestMethod]
        public void TestNotifySend()
        {
            var Notif = new Notification("Notification title", "This is a high priority notification", NotificationPriority.High, NotificationType.Normal);
            NotificationManager.NotifySend(Notif);
            NotificationManager.NotifRecents.ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests notifications sending
        /// </summary>
        [TestMethod]
        public void TestNotifySendRange()
        {
            var Notif1 = new Notification("High notification title", "This is a high priority notification", NotificationPriority.High, NotificationType.Normal);
            var Notif2 = new Notification("Medium notification title", "This is a medium priority notification", NotificationPriority.Medium, NotificationType.Normal);
            var Notif3 = new Notification("Low notification title", "This is a low priority notification", NotificationPriority.Low, NotificationType.Normal);
            var Notifs = new List<Notification>() { Notif1, Notif2, Notif3 };
            NotificationManager.NotifySendRange(Notifs);
            NotificationManager.NotifRecents.ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests notification dismiss
        /// </summary>
        [TestMethod]
        public void TestNotifyDismiss()
        {
            var Notif = new Notification("Redundant title", "This is a redundant notification", NotificationPriority.Low, NotificationType.Normal);
            NotificationManager.NotifySend(Notif);
            NotificationManager.NotifDismiss(NotificationManager.NotifRecents.Count - 1).ShouldBeTrue();
        }

        /// <summary>
        /// Tests notification dismiss all
        /// </summary>
        [TestMethod]
        public void TestNotifyDismissAll()
        {
            NotificationManager.NotifDismissAll().ShouldBeTrue();
            NotificationManager.NotifRecents.ShouldBeEmpty();
        }

        /// <summary>
        /// Tests notification equality
        /// </summary>
        [TestMethod]
        public void TestNotifyEquality()
        {
            var Notif = new Notification("A title", "This is a notification", NotificationPriority.Low, NotificationType.Normal);
            var Notif2 = new Notification("A title", "This is a notification", NotificationPriority.Low, NotificationType.Normal);
            Notif.EqualsNoId(Notif2).ShouldBeTrue();
            Notif2.EqualsNoId(Notif).ShouldBeTrue();
        }

        /// <summary>
        /// Tests notification inequality
        /// </summary>
        [TestMethod]
        public void TestNotifyInequality()
        {
            var Notif = new Notification("A title", "This is a notification", NotificationPriority.Low, NotificationType.Normal);
            var Notif2 = new Notification("A title", "This is a notification", NotificationPriority.High, NotificationType.Normal);
            Notif.EqualsNoId(Notif2).ShouldBeFalse();
            Notif2.EqualsNoId(Notif).ShouldBeFalse();
        }

    }
}
