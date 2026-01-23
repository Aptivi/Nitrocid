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

namespace Nitrocid.Kernel.Debugging.Testing.Facades
{
    internal class DismissNotifications : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Dismisses all sent notifications");
        public override void Run()
        {
            for (int i = NotificationManager.NotifRecents.Count - 1; i >= 0; i--)
                NotificationManager.NotifDismiss(i);
        }
    }
}
