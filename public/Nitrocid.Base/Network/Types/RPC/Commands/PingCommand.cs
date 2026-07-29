
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

extern alias TextifyDep;

using System.Net;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Notifications;
using TextifyDep::Textify.General;

namespace Nitrocid.Base.Network.Types.RPC.Commands
{
    internal class PingCommand : IRPCCommand
    {
        public void Execute(string argument, IPEndPoint endpoint)
        {
            var testNotification = new Notification(LanguageTools.GetLocalized("NKS_NETWORK_TYPES_RPC_PINGACK_TITLE"), TextTools.FormatString(LanguageTools.GetLocalized("NKS_NETWORK_TYPES_RPC_PINGACK_DESC"), endpoint.Address.ToString()), NotificationPriority.Low, NotificationType.Normal);
            DebugWriter.WriteDebug(DebugLevel.I, "{0} pinged this device!", vars: [endpoint.Address.ToString()]);
            NotificationManager.NotifySend(testNotification);
        }
    }
}
