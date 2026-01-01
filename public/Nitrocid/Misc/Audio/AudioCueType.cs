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

namespace Nitrocid.Misc.Audio
{
    /// <summary>
    /// Audio cue type
    /// </summary>
    public enum AudioCueType
    {
        /// <summary>
        /// Intense version of the alarm sound
        /// </summary>
        Alarm,
        /// <summary>
        /// Idle version of the alarm sound
        /// </summary>
        AlarmIdle,
        /// <summary>
        /// Intense version of the ambience sound
        /// </summary>
        Ambience,
        /// <summary>
        /// Idle version of the ambience sound
        /// </summary>
        AmbienceIdle,
        /// <summary>
        /// High-priority beep sound
        /// </summary>
        BeepHigh,
        /// <summary>
        /// Medium-priority beep sound
        /// </summary>
        BeepMedium,
        /// <summary>
        /// Low-priority beep sound
        /// </summary>
        BeepLow,
        /// <summary>
        /// Backspace keypress
        /// </summary>
        KeyboardCueBackspace,
        /// <summary>
        /// Enter keypress
        /// </summary>
        KeyboardCueEnter,
        /// <summary>
        /// Pressing any key
        /// </summary>
        KeyboardCueType,
        /// <summary>
        /// High-priority notifications
        /// </summary>
        NotificationHigh,
        /// <summary>
        /// Medium-priority notifications
        /// </summary>
        NotificationMedium,
        /// <summary>
        /// Low-priority notifications
        /// </summary>
        NotificationLow,
        /// <summary>
        /// Shutdown sound
        /// </summary>
        Shutdown,
        /// <summary>
        /// Special beep for special notifications
        /// </summary>
        SpecialBeep,
        /// <summary>
        /// Startup sound
        /// </summary>
        Startup,
    }
}
