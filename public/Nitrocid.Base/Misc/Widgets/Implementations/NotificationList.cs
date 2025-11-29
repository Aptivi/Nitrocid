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

using System.Text;
using Nitrocid.Base.Misc.Notifications;
using Terminaux.Colors;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.CyclicWriters.Graphical;

namespace Nitrocid.Base.Misc.Widgets.Implementations
{
    internal class NotificationList : BaseWidget, IWidget
    {
        public override string Cleanup(int left, int top, int width, int height) =>
            "";

        public override string Initialize(int left, int top, int width, int height) =>
            "";

        public override string Render(int left, int top, int width, int height)
        {
            var notificationList = new StringBuilder();

            // Get all notifications and list them with cards
            int processedHeight = 0;
            foreach (var notification in NotificationManager.NotifRecents)
            {
                // Choose the color according to the priority
                var notifyBorderColor =
                    notification.Priority == NotificationPriority.Custom ? notification.CustomColor :
                    notification.Priority == NotificationPriority.High ? ThemeColorsTools.GetColor("HighPriorityBorderColor") :
                    notification.Priority == NotificationPriority.Medium ? ThemeColorsTools.GetColor("MediumPriorityBorderColor") :
                    ThemeColorsTools.GetColor("LowPriorityBorderColor");
                if (notification.NotificationBorderColor != Color.Empty)
                    notifyBorderColor = notification.NotificationBorderColor;

                // Add a card box
                int totalWidth = width - 2;
                var cardBoxFrame = new BoxFrame()
                {
                    Left = left,
                    Top = top + processedHeight,
                    Width = totalWidth,
                    Height = 3,
                    FrameColor = notifyBorderColor,
                };
                notificationList.Append(cardBoxFrame.Render());

                // Check the total interior width
                if (totalWidth <= 1)
                    break;

                // Check to see if we can at least print the notification icon
                if (totalWidth >= 2)
                {
                    var notificationIconText = new AlignedText()
                    {
                        Left = left + 1,
                        Top = top + processedHeight + 1,
                        Width = 2,
                        Text = notification.IconEmoji,
                        OneLine = true,
                    };
                    notificationList.Append(notificationIconText.Render());
                }
                
                // Now, check to see if we can print title and description
                if (totalWidth >= 6)
                {
                    var notificationTitleText = new AlignedText()
                    {
                        Left = left + 5,
                        Top = top + processedHeight + 1,
                        Width = totalWidth - 3,
                        Text = notification.Title,
                        OneLine = true,
                        ForegroundColor = notification.Priority == NotificationPriority.Custom ? notification.CustomTitleColor : ThemeColorsTools.GetColor("NotificationTitleColor"),
                    };
                    var notificationDescText = new BoundedText()
                    {
                        Left = left + 5,
                        Top = top + processedHeight + 2,
                        Width = totalWidth - 3,
                        Height = 2,
                        Text = notification.Desc,
                        ForegroundColor = notification.Priority == NotificationPriority.Custom ? notification.CustomDescriptionColor : ThemeColorsTools.GetColor("NotificationDescriptionColor"),
                    };
                    notificationList.Append(notificationTitleText.Render());
                    notificationList.Append(notificationDescText.Render());
                }

                // Process only if we can add more notifications
                processedHeight += 5;
                if (processedHeight + 5 > height)
                    break;
            }

            // Return the notification list
            return notificationList.ToString();
        }
    }
}
