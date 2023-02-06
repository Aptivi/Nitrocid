
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

using KS.Kernel.Debugging;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for Lyrics
    /// </summary>
    public static class LyricsSettings
    {

        private static int _Delay = 10000;

        /// <summary>
        /// [Lyrics] How many milliseconds to wait before the next lyric?
        /// </summary>
        public static int LyricsDelay
        {
            get
            {
                return _Delay;
            }
            set
            {
                if (value <= 0)
                    value = 10000;
                _Delay = value;
            }
        }

    }

    /// <summary>
    /// Display code for Lyrics
    /// </summary>
    public class LyricsDisplay : BaseScreensaver, IScreensaver
    {

        private Animations.Lyrics.LyricsSettings LyricsSettingsInstance;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Lyrics";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleBase.ConsoleWrapper.WindowWidth, ConsoleBase.ConsoleWrapper.WindowHeight);
            LyricsSettingsInstance = new Animations.Lyrics.LyricsSettings()
            {
                LyricsDelay = LyricsSettings.LyricsDelay
            };
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic() => Animations.Lyrics.Lyrics.Simulate(LyricsSettingsInstance);

    }
}
