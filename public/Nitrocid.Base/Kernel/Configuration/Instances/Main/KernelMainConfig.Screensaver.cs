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

using System;
using Nitrocid.Base.Misc.Screensaver;

namespace Nitrocid.Base.Kernel.Configuration.Instances
{
    /// <summary>
    /// Main kernel configuration instance
    /// </summary>
    public partial class KernelMainConfig : BaseKernelConfig
    {
        private int screensaverDelay = 10;

        /// <summary>
        /// Which screensaver do you want to lock your screen with?
        /// </summary>
        public string DefaultSaverName { get; set; } = "matrixbleed";
        /// <summary>
        /// Whether the screen idling is enabled or not
        /// </summary>
        public bool ScreenTimeoutEnabled
        {
            get => ScreensaverManager.scrnTimeoutEnabled;
            set
            {
                ScreensaverManager.scrnTimeoutEnabled = value;
                if (!value)
                    ScreensaverManager.StopTimeout();
                else
                    ScreensaverManager.StartTimeout();
            }
        }
        /// <summary>
        /// Minimum idling interval to launch screensaver
        /// </summary>
        public string ScreenTimeout
        {
            get => ScreensaverManager.scrnTimeout.ToString();
            set
            {
                // First, deal with merging milliseconds from old configs
                TimeSpan fallback = new(0, 5, 0);
                TimeSpan span;
                bool isOldFormat = int.TryParse(value, out int milliseconds);
                if (isOldFormat)
                {
                    span = TimeSpan.FromMilliseconds(milliseconds);
                    ScreensaverManager.scrnTimeout = span.TotalMinutes < 1.0d ? fallback : span;
                    return;
                }

                // Then, parse the timespan
                bool spanParsed = TimeSpan.TryParse(value, out span);
                if (!spanParsed)
                    ScreensaverManager.scrnTimeout = fallback;
                else
                    ScreensaverManager.scrnTimeout = span.TotalMinutes < 1.0d ? fallback : span;
            }
        }
        /// <summary>
        /// Enables debugging for screensavers. Please note that it may quickly fill the debug log and slightly slow the screensaver down, depending on the screensaver used. Only works if kernel debugging is enabled for diagnostic purposes.
        /// </summary>
        public bool ScreensaverDebug { get; set; }
        /// <summary>
        /// If you've acknowledged the photosensitive seizure warning, you can turn off the warning message that appears each time a fast-paced screensaver is run.
        /// </summary>
        public bool ScreensaverSeizureAcknowledged
        {
            get => ScreensaverManager.seizureAcknowledged;
            set => ScreensaverManager.seizureAcknowledged = value;
        }
        /// <summary>
        /// After locking the screen, ask for password
        /// </summary>
        public bool PasswordLock { get; set; } = true;
        /// <summary>
        /// If true, enables unified writing delay for all screensavers. Otherwise, it uses screensaver-specific configured delay values.
        /// </summary>
        public bool ScreensaverUnifiedDelay { get; set; } = true;
        /// <summary>
        /// How many milliseconds to wait before making the next write?
        /// </summary>
        public int ScreensaverDelay
        {
            get => screensaverDelay;
            set
            {
                if (value <= 0)
                    value = 10;
                screensaverDelay = value;
            }
        }
    }
}
