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

using System.Text;
using Nitrocid.Base.Misc.Notifications;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Nitrocid.Base.Misc.Widgets.Implementations
{
    internal class NotificationIcons : BaseWidget, IWidget
    {
        public override string Cleanup(int left, int top, int width, int height) =>
            "";

        public override string Initialize(int left, int top, int width, int height) =>
            "";

        public override string Render(int left, int top, int width, int height)
        {
            var notificationListing = new StringBuilder();

            // List all notifications in a horizontal way, optionally extending to rows if required
            int processedWidth = 0;
            int processedHeight = 0;
            for (int i = 0; i < NotificationManager.NotifRecents.Count; i++)
            {
                Notification notification = NotificationManager.NotifRecents[i];
                Notification? nextNotification = i + 1 >= NotificationManager.NotifRecents.Count ? null : NotificationManager.NotifRecents[i + 1];

                // Add the icon to the notification listing string, but we need to choose the color according to the priority
                int iconWidth = ConsoleChar.EstimateCellWidth(notification.IconEmoji);
                var notifyBorderColor =
                    notification.Priority == NotificationPriority.Custom ? notification.CustomColor :
                    notification.Priority == NotificationPriority.High ? ThemeColorsTools.GetColor("HighPriorityBorderColor") :
                    notification.Priority == NotificationPriority.Medium ? ThemeColorsTools.GetColor("MediumPriorityBorderColor") :
                    ThemeColorsTools.GetColor("LowPriorityBorderColor");
                if (notification.NotificationBorderColor != Color.Empty)
                    notifyBorderColor = notification.NotificationBorderColor;
                notificationListing.Append(ColorTools.RenderSetConsoleColor(notifyBorderColor));
                notificationListing.Append(notification.IconEmoji.ToString());
                notificationListing.Append(ColorTools.RenderRevertForeground());
                processedWidth += iconWidth;

                // Add a space if we can process more notifications
                if (nextNotification is not null)
                {
                    int nextIconWidth = ConsoleChar.EstimateCellWidth(nextNotification.IconEmoji);
                    if (processedWidth + 1 + nextIconWidth > width)
                    {
                        // We have reached the end of the width! We'll add a new line only if we can.
                        processedWidth = 0;
                        if (processedHeight + 2 > height)
                        {
                            // In this case, we need to bail.
                            break;
                        }

                        // Add new lines
                        notificationListing.AppendLine("\n");
                        processedHeight += 2;
                    }
                    else
                    {
                        notificationListing.Append(' ');
                        processedWidth++;
                    }
                }
            }

            // Return the final rendered notification icons
            TextAlignment alignment =
                Options.TryGetValue("alignment", out object? alignmentValue) ?
                    alignmentValue is not null ? (TextAlignment)alignmentValue : TextAlignment.Middle :
                TextAlignment.Middle;
            var listingText = new BoundedText()
            {
                Text = notificationListing.ToString(),
                Top = top,
                Left = left,
                Width = width,
                Height = height,
                Settings =
                {
                    Alignment = alignment,
                }
            };
            return listingText.Render();
        }
    }
}
