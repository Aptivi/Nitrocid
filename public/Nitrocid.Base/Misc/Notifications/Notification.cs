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

using Newtonsoft.Json;
using Terminaux.Colors.Themes.Colors;
using System;
using Terminaux.Colors;
using Textify.General.Structures;

namespace Nitrocid.Base.Misc.Notifications
{
    /// <summary>
    /// Notification holder with title, description, and priority
    /// </summary>
    public class Notification : IEquatable<Notification>
    {

        [JsonProperty(nameof(Id))]
        private readonly Guid _id;
        [JsonProperty(nameof(Progress))]
        private int _Progress;
        [JsonProperty(nameof(CustomBeepTimes))]
        private int _CustomBeepTimes = 1;
        [JsonProperty(nameof(NotificationBorderColor))]
        private Color _NotificationBorderColor = Color.Empty;

        /// <summary>
        /// Notification ID
        /// </summary>
        [JsonIgnore]
        public Guid Id =>
            _id;

        /// <summary>
        /// Notification title
        /// </summary>
        [JsonProperty]
        public string Title { get; set; }

        /// <summary>
        /// Notification description
        /// </summary>
        [JsonProperty]
        public string Desc { get; set; }

        /// <summary>
        /// Notification priority
        /// </summary>
        [JsonProperty]
        public NotificationPriority Priority { get; set; }

        /// <summary>
        /// Notification type
        /// </summary>
        [JsonProperty]
        public NotificationType Type { get; set; }

        /// <summary>
        /// Whether the progress is indeterminate
        /// </summary>
        [JsonProperty]
        public bool ProgressIndeterminate { get; set; }

        /// <summary>
        /// Whether the progress failed
        /// </summary>
        [JsonProperty]
        public NotificationProgressState ProgressState { get; set; }

        /// <summary>
        /// Notification progress
        /// </summary>
        [JsonIgnore]
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

        /// <summary>
        /// Icon emoji representation for notification listing
        /// </summary>
        [JsonProperty]
        public WideChar IconEmoji { get; set; } = (WideChar)"🔔";

        // --> For Custom Priority Notifications

        /// <summary>
        /// Beep times (for custom priority notfications)
        /// </summary>
        [JsonIgnore]
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
        [JsonProperty]
        public Color CustomColor { get; set; } = ThemeColorsTools.GetColor("LowPriorityBorderColor");

        /// <summary>
        /// Custom title color (for custom priority notfications)
        /// </summary>
        [JsonProperty]
        public Color CustomTitleColor { get; set; } = ThemeColorsTools.GetColor("NotificationTitleColor");

        /// <summary>
        /// Custom description color (for custom priority notfications)
        /// </summary>
        [JsonProperty]
        public Color CustomDescriptionColor { get; set; } = ThemeColorsTools.GetColor("NotificationDescriptionColor");

        /// <summary>
        /// Custom progress color (for custom priority notfications)
        /// </summary>
        [JsonProperty]
        public Color CustomProgressColor { get; set; } = ThemeColorsTools.GetColor("NotificationProgressColor");

        /// <summary>
        /// Custom progress failure color (for custom priority notfications)
        /// </summary>
        [JsonProperty]
        public Color CustomProgressFailureColor { get; set; } = ThemeColorsTools.GetColor("NotificationFailureColor");

        /// <summary>
        /// Custom progress success color (for custom priority notfications)
        /// </summary>
        [JsonProperty]
        public Color CustomProgressSuccessColor { get; set; } = ThemeColorsTools.GetColor(ThemeColorType.Success);

        /// <summary>
        /// Upper left corner character for custom priority notification
        /// </summary>
        [JsonProperty]
        public char CustomUpperLeftCornerChar { get; set; } = '╭';

        /// <summary>
        /// Upper right corner character for custom priority notification
        /// </summary>
        [JsonProperty]
        public char CustomUpperRightCornerChar { get; set; } = '╮';

        /// <summary>
        /// Lower left corner character for custom priority notification
        /// </summary>
        [JsonProperty]
        public char CustomLowerLeftCornerChar { get; set; } = '╰';

        /// <summary>
        /// Lower right corner character for custom priority notification
        /// </summary>
        [JsonProperty]
        public char CustomLowerRightCornerChar { get; set; } = '╯';

        /// <summary>
        /// Upper frame character for custom priority notification
        /// </summary>
        [JsonProperty]
        public char CustomUpperFrameChar { get; set; } = '─';

        /// <summary>
        /// Lower frame character for custom priority notification
        /// </summary>
        [JsonProperty]
        public char CustomLowerFrameChar { get; set; } = '─';

        /// <summary>
        /// Left frame character for custom priority notification
        /// </summary>
        [JsonProperty]
        public char CustomLeftFrameChar { get; set; } = '│';

        /// <summary>
        /// Right frame character for custom priority notification
        /// </summary>
        [JsonProperty]
        public char CustomRightFrameChar { get; set; } = '│';

        /// <summary>
        /// Whether the progress has been compeleted successfully or with failure
        /// </summary>
        public bool ProgressCompleted =>
            _Progress >= 100 || ProgressState != NotificationProgressState.Progressing;

        /// <summary>
        /// The notification border color. Must be empty for custom priority notifications.
        /// </summary>
        [JsonIgnore]
        public Color NotificationBorderColor
        {
            get
            {
                return _NotificationBorderColor;
            }
            set
            {
                if (Priority != NotificationPriority.Custom)
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
        public Notification(string Title, string Desc, NotificationPriority Priority, NotificationType Type)
        {
            this.Title = Title;
            this.Desc = Desc;
            this.Priority = Priority;
            this.Type = Type;
            _id = Guid.NewGuid();
        }

        /// <summary>
        /// Checks to see if the notification matches another one
        /// </summary>
        /// <param name="obj">Notification object</param>
        /// <returns>True if there is a match; false otherwise.</returns>
        public override bool Equals(object? obj) =>
            Equals(obj as Notification);

        /// <summary>
        /// Checks to see if the notification matches another one
        /// </summary>
        /// <param name="other">Notification to compare</param>
        /// <returns>True if there is a match; false otherwise.</returns>
        public bool Equals(Notification? other)
        {
            return
                other is not null &&
                Id == other.Id &&
                Title == other.Title &&
                Desc == other.Desc &&
                Priority == other.Priority &&
                Type == other.Type &&
                CustomBeepTimes == other.CustomBeepTimes &&
                CustomColor == other.CustomColor &&
                CustomTitleColor == other.CustomTitleColor &&
                CustomDescriptionColor == other.CustomDescriptionColor &&
                CustomProgressColor == other.CustomProgressColor &&
                CustomProgressFailureColor == other.CustomProgressFailureColor &&
                CustomProgressSuccessColor == other.CustomProgressSuccessColor &&
                CustomUpperLeftCornerChar == other.CustomUpperLeftCornerChar &&
                CustomUpperRightCornerChar == other.CustomUpperRightCornerChar &&
                CustomLowerLeftCornerChar == other.CustomLowerLeftCornerChar &&
                CustomLowerRightCornerChar == other.CustomLowerRightCornerChar &&
                CustomUpperFrameChar == other.CustomUpperFrameChar &&
                CustomLowerFrameChar == other.CustomLowerFrameChar &&
                CustomLeftFrameChar == other.CustomLeftFrameChar &&
                CustomRightFrameChar == other.CustomRightFrameChar &&
                NotificationBorderColor == other.NotificationBorderColor &&
                ProgressIndeterminate == other.ProgressIndeterminate;
        }

        /// <summary>
        /// Checks to see if the notification matches another one
        /// </summary>
        /// <param name="other">Notification to compare</param>
        /// <returns>True if there is a match; false otherwise.</returns>
        public bool EqualsNoId(Notification? other)
        {
            return
                other is not null &&
                Title == other.Title &&
                Desc == other.Desc &&
                Priority == other.Priority &&
                Type == other.Type &&
                CustomBeepTimes == other.CustomBeepTimes &&
                CustomColor == other.CustomColor &&
                CustomTitleColor == other.CustomTitleColor &&
                CustomDescriptionColor == other.CustomDescriptionColor &&
                CustomProgressColor == other.CustomProgressColor &&
                CustomProgressFailureColor == other.CustomProgressFailureColor &&
                CustomProgressSuccessColor == other.CustomProgressSuccessColor &&
                CustomUpperLeftCornerChar == other.CustomUpperLeftCornerChar &&
                CustomUpperRightCornerChar == other.CustomUpperRightCornerChar &&
                CustomLowerLeftCornerChar == other.CustomLowerLeftCornerChar &&
                CustomLowerRightCornerChar == other.CustomLowerRightCornerChar &&
                CustomUpperFrameChar == other.CustomUpperFrameChar &&
                CustomLowerFrameChar == other.CustomLowerFrameChar &&
                CustomLeftFrameChar == other.CustomLeftFrameChar &&
                CustomRightFrameChar == other.CustomRightFrameChar &&
                NotificationBorderColor == other.NotificationBorderColor &&
                ProgressIndeterminate == other.ProgressIndeterminate;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            HashCode hash = new();
            hash.Add(Id);
            hash.Add(Title);
            hash.Add(Desc);
            hash.Add(Priority);
            hash.Add(Type);
            hash.Add(CustomBeepTimes);
            hash.Add(CustomColor);
            hash.Add(CustomTitleColor);
            hash.Add(CustomDescriptionColor);
            hash.Add(CustomProgressColor);
            hash.Add(CustomProgressFailureColor);
            hash.Add(CustomProgressSuccessColor);
            hash.Add(CustomUpperLeftCornerChar);
            hash.Add(CustomUpperRightCornerChar);
            hash.Add(CustomLowerLeftCornerChar);
            hash.Add(CustomLowerRightCornerChar);
            hash.Add(CustomUpperFrameChar);
            hash.Add(CustomLowerFrameChar);
            hash.Add(CustomLeftFrameChar);
            hash.Add(CustomRightFrameChar);
            hash.Add(NotificationBorderColor);
            hash.Add(ProgressIndeterminate);
            return hash.ToHashCode();
        }

        /// <inheritdoc/>
        public static bool operator ==(Notification left, Notification right) =>
            left.Equals(right);

        /// <inheritdoc/>
        public static bool operator !=(Notification left, Notification right) =>
            !(left == right);
    }
}
