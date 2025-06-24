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

using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Notifications;
using Nitrocid.Base.Users.Login;

namespace Nitrocid.Base.Kernel.Debugging.Testing.Facades
{
    internal class EnableNotifications : TestFacade
    {
        public override string TestName => LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTFACADES_ENABLENOTIFICATIONS_DESC");
        public override bool TestInteractive => false;
        public override object TestExpectedValue => true;
        public override void Run()
        {
            Login.LoggedIn = true;
            if (!NotificationManager.NotifThread.IsAlive)
                NotificationManager.NotifThread.Start();
            TestActualValue = NotificationManager.NotifThread.IsAlive;
        }
    }
}
