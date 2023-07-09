
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

using ColorSeq;
using KS.ConsoleBase.Colors;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;

namespace KS.Misc.Notifications
{
    /// <summary>
    /// Notification holder with title, description, and priority
    /// </summary>
    public class Notification
    {

        private int _Progress;
        private int _CustomBeepTimes = 1;
        private Color _NotificationBorderColor;

        /// <summary>
        /// Notification title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Notification description
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// Notification priority
        /// </summary>
        public NotificationManager.NotifPriority Priority { get; set; }

        /// <summary>
        /// Notification type
        /// </summary>
        public NotificationManager.NotifType Type { get; set; }

        /// <summary>
        /// Whether the progress failed
        /// </summary>
        public bool ProgressFailed { get; set; }

        /// <summary>
        /// Notification progress
        /// </summary>
        public int Progress
        {
            get
            {
                return _Progress;
            }
            set
            {
                if (value >= 100)
                {
                    _Progress = 100;
                }
                else if (value <= 0)
                {
                    _Progress = 0;
                }
                else
                {
                    _Progress = value;
                }
            }
        }

        // --> For Custom Priority Notifications

        /// <summary>
        /// Beep times (for custom priority notfications)
        /// </summary>
        public int CustomBeepTimes
        {
            get
            {
                return _CustomBeepTimes;
            }
            set
            {
                if (value <= 0)
                {
                    _CustomBeepTimes = 0;
                }
                else
                {
                    _CustomBeepTimes = value;
                }
            }
        }

        /// <summary>
        /// Custom color (for custom priority notfications)
        /// </summary>
        public Color CustomColor { get; set; } = ColorTools.GetColor(KernelColorType.LowPriorityBorder);

        /// <summary>
        /// Custom title color (for custom priority notfications)
        /// </summary>
        public Color CustomTitleColor { get; set; } = ColorTools.GetColor(KernelColorType.NotificationTitle);

        /// <summary>
        /// Custom description color (for custom priority notfications)
        /// </summary>
        public Color CustomDescriptionColor { get; set; } = ColorTools.GetColor(KernelColorType.NotificationDescription);

        /// <summary>
        /// Custom progress color (for custom priority notfications)
        /// </summary>
        public Color CustomProgressColor { get; set; } = ColorTools.GetColor(KernelColorType.NotificationProgress);

        /// <summary>
        /// Custom progress failure color (for custom priority notfications)
        /// </summary>
        public Color CustomProgressFailureColor { get; set; } = ColorTools.GetColor(KernelColorType.NotificationFailure);

        /// <summary>
        /// Upper left corner character for custom priority notification
        /// </summary>
        public char CustomUpperLeftCornerChar { get; set; } = '╔';

        /// <summary>
        /// Upper right corner character for custom priority notification
        /// </summary>
        public char CustomUpperRightCornerChar { get; set; } = '╗';

        /// <summary>
        /// Lower left corner character for custom priority notification
        /// </summary>
        public char CustomLowerLeftCornerChar { get; set; } = '╚';

        /// <summary>
        /// Lower right corner character for custom priority notification
        /// </summary>
        public char CustomLowerRightCornerChar { get; set; } = '╝';

        /// <summary>
        /// Upper frame character for custom priority notification
        /// </summary>
        public char CustomUpperFrameChar { get; set; } = '═';

        /// <summary>
        /// Lower frame character for custom priority notification
        /// </summary>
        public char CustomLowerFrameChar { get; set; } = '═';

        /// <summary>
        /// Left frame character for custom priority notification
        /// </summary>
        public char CustomLeftFrameChar { get; set; } = '║';

        /// <summary>
        /// Right frame character for custom priority notification
        /// </summary>
        public char CustomRightFrameChar { get; set; } = '║';

        /// <summary>
        /// Whether the progress has been compeleted successfully or with failure
        /// </summary>
        public bool ProgressCompleted => _Progress >= 100 | ProgressFailed;

        /// <summary>
        /// The notification border color. Must be null for custom priority notifications.
        /// </summary>
        public Color NotificationBorderColor
        {
            get
            {
                return _NotificationBorderColor;
            }
            set
            {
                if (!(Priority == NotificationManager.NotifPriority.Custom))
                {
                    _NotificationBorderColor = value;
                }
            }
        }

        /// <summary>
        /// Creates a new notification
        /// </summary>
        /// <param name="Title">Title of notification</param>
        /// <param name="Desc">Description of notification</param>
        /// <param name="Priority">Priority of notification</param>
        /// <param name="Type">Notification type</param>
        public Notification(string Title, string Desc, NotificationManager.NotifPriority Priority, NotificationManager.NotifType Type)
        {
            this.Title = Title;
            this.Desc = Desc;
            this.Priority = Priority;
            this.Type = Type;
        }

    }
}
