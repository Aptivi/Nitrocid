
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
using KS.Misc.Notifications;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Shows recent notifications
    /// </summary>
    /// <remarks>
    /// If you need to see recent notifications, you can see them using this command. Any sent notifications will be put to the list that can be shown using this command. This is useful for dismissnotif command.
    /// </remarks>
    class ShowNotifsCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            int Count = 1;
            if (!(NotificationManager.NotifRecents.Count == 0))
            {
                foreach (Notification Notif in NotificationManager.NotifRecents)
                {
                    TextWriterColor.WriteKernelColor($"[{Count}/{NotificationManager.NotifRecents.Count}] {Notif.Title}: ", false, KernelColorType.ListEntry);
                    TextWriterColor.WriteKernelColor(Notif.Desc, false, KernelColorType.ListValue);
                    if (Notif.Type == NotificationType.Progress)
                    {
                        TextWriterColor.WriteKernelColor($" ({Notif.Progress}%)", false, Notif.ProgressFailed ? KernelColorType.Error : KernelColorType.Success);
                    }
                    TextWriterColor.Write();
                    Count += 1;
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("No recent notifications"));
            }
            return 0;
        }

    }
}
